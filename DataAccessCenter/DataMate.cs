using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using XmlProcess;
using System.Collections;
using System.IO;
namespace WolfInv.Com.DataCenter
{

    public class LogClass
    {
        string LogPath;
        string FileName;
        public bool Debug;
        public void SetLogFileName(string filename)
        {
            FileName = filename;
        }
        public LogClass(string logpath)
        {
            LogPath = logpath;
        }
        public void DWrite(string fmt, params string[] parm)
        {
            Write(FileName, fmt, parm);
        }

        public void Write(string filepath,string fmt, params string[] parm)
        {
            if (!Debug) return;
            string logfile = string.Format("{0}\\{1}",LogPath,filepath);
            if (!File.Exists(logfile))
            {
                File.CreateText(logfile);
            }
            try
            {
                StreamWriter sw = File.AppendText(logfile);
                sw.WriteLine(string.Format(fmt,parm));
                sw.Close();
            }
            catch (Exception e)
            {
                return;
            }
        }
    }

    //ref column
    public class DataColumnReference
    {
        public DataColumn  MainColumn;
        public DataColumn  LientColumn;
    }

    public class DataColumn
    {
        public string DataPoint;
        public string Table;
        public string Column;
        public string Index;
        public string DataType;
        public int Length;
        public string DataBase;
        public string RefDataPoint;
        public bool IsKey;
        public bool IsIden;
        public DataColumn RefColumn;
        public DataIdenTable OwnTable;
        public bool IsDefault;
        public void LoadXml(XmlNode node)
        {
            DataPoint = XmlUtil.GetSubNodeText(node, "@id");
            Column = XmlUtil.GetSubNodeText(node, "@col");
            Table = XmlUtil.GetSubNodeText(node, "@tab");
            int.TryParse(XmlUtil.GetSubNodeText(node, "@len"),out Length);
            DataType = XmlUtil.GetSubNodeText(node, "@type");
            RefDataPoint = XmlUtil.GetSubNodeText(node, "@ref");
            IsKey = XmlUtil.GetSubNodeText(node, "@key") == "1";
            IsIden = XmlUtil.GetSubNodeText(node, "@iden") == "1";
            IsDefault  = XmlUtil.GetSubNodeText(node, "@default") == "1";
        }

        public XmlNode ToXml(XmlNode parent)
        {
            XmlDocument xmldoc;
            if (parent == null)
            {
                xmldoc = new XmlDocument();
                xmldoc.LoadXml("<data/>");
                parent = xmldoc.SelectSingleNode("data");
            }
            XmlNode node = XmlUtil.AddSubNode(parent, "f");
            XmlUtil.AddAttribute(node, "id", this.DataPoint);
            XmlUtil.AddAttribute(node,"col",this.Column);
            XmlUtil.AddAttribute(node, "tab", this.Table);
            XmlUtil.AddAttribute(node, "type", this.DataType);
            XmlUtil.AddAttribute(node, "len", this.Length.ToString());
            if(this.RefDataPoint != null && this.RefDataPoint.Trim().Length > 0) 
                XmlUtil.AddAttribute(node, "ref", this.RefDataPoint);
            if (this.IsKey)
                XmlUtil.AddAttribute(node, "key", "1");
            if (this.IsIden)
                XmlUtil.AddAttribute(node, "iden", "1");
            if (this.IsDefault)
                XmlUtil.AddAttribute(node, "default", "1");
            return node;
        }
    }

    public class DataIdenTable:Dictionary<string,DataColumn>
    {
        public List<DataColumn> Columns = new List<DataColumn>();
        public string TableName;
        public bool IsView;
        public string Key;
        public string IdenColumn;
        public string AName;
        

    }
}
