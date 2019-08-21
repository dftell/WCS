using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using XmlProcess;
using System.Data;
using System.IO;

namespace WolfInv.Com.MetaDataCenter
{
    public class UpdateData:ICloneable,IXml
    {
        public Dictionary<string, UpdateItem> Items = new Dictionary<string, UpdateItem>();
        public List<UpdateData> SubItems = new List<UpdateData>();
        public DataPoint keydpt;
        public string keyvalue;
        public bool Updated;
        public DataSet Dataset;
        public bool IsImported;
        public DataRequestType ReqType;
        public UpdateData()
        {

        }

        public UpdateData(XmlNode node)
        {
            LoadXml(node);
        }

        public void LoadXml(XmlNode node)
        {
            if (node == null) return;
            this.keydpt  = new DataPoint(XmlUtil.GetSubNodeText(node, "@key"));
            this.keyvalue = XmlUtil.GetSubNodeText(node, "@val");
            this.ReqType = DataRequestType.Update;
            switch (XmlUtil.GetSubNodeText(node, "@type"))
            {
                case "Add":
                    {
                        ReqType = DataRequestType.Add;
                        break;
                    }
                case "Delete":
                    {
                        ReqType = DataRequestType.Delete;
                        break;
                    }
                case "BatchUpdate":
                    {
                        ReqType = DataRequestType.BatchUpdate ;
                        break ;
                    }
                default :
                    {
                        break;
                    }
            }
            XmlNodeList nodes = node.SelectNodes("./i");
            this.Items = new Dictionary<string, UpdateItem>();
            foreach(XmlNode nd in nodes)
            {
                UpdateItem ui = new UpdateItem(nd);
                this.Items.Add(ui.datapoint.Name,ui);
            }
            this.SubItems = new List<UpdateData>();
            XmlNodeList subnode = node.SelectNodes("./subdata/data");
            for (int i = 0; i < subnode.Count; i++)
            {
                UpdateData subdata = new UpdateData(subnode[i]);
                this.SubItems.Add(subdata);
            }
            XmlNode datasetnode = node.SelectSingleNode("./DataSet");
            if (datasetnode != null)
            {
                this.Dataset = new DataSet();
              
                StringReader sr = new StringReader(datasetnode.InnerXml);
                this.Dataset.ReadXml(sr, XmlReadMode.ReadSchema);

                sr.Close();
            }
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
            //xmldoc = parent.OwnerDocument;
            XmlUtil.AddAttribute(parent, "key", this.keydpt == null?"":this.keydpt.Name);
            XmlUtil.AddAttribute(parent, "val", this.keyvalue);
            XmlUtil.AddAttribute(parent, "type", this.ReqType.ToString());
            //XmlNode node = XmlUtil.AddSubNode(parent, "data", true);
            foreach (UpdateItem ui in this.Items.Values)
            {
                ui.ToXml(parent);
            }
            if (this.SubItems != null && this.SubItems.Count > 0)
            {
                XmlNode node = XmlUtil.AddSubNode(parent, "subdata", false);
                for (int i = 0; i < this.SubItems.Count; i++)
                {
                    XmlNode subNode = node.OwnerDocument.CreateElement("data");// XmlUtil.AddSubNode(node, "data");
                    node.AppendChild(subNode);
                    this.SubItems[i].ToXml(subNode);
                }
            }
            if (this.Dataset != null)
            {
                MemoryStream strm = new MemoryStream();
                ////XmlTextWriter writer = new XmlTextWriter(strm,Encoding.UTF8);
                ////writer.Formatting = Formatting.Indented;
                ////writer.Indentation = 4;
                ////writer.IndentChar = ' ';
                ////writer.WriteStartDocument();
                ////this.Dataset.WriteXml(writer,XmlWriteMode.WriteSchema);
                ////writer.Flush();

               StreamWriter sw = new StreamWriter(strm); 
             
               // 用DataSet的WriteXml方法把DataSet写入内存流时，缺少XML文档的声明行，必须先加上 
               sw.WriteLine(@"<?xml version=""1.0"" standalone=""yes""?>");
               this.Dataset.WriteXml(sw, XmlWriteMode.WriteSchema);
               sw.Flush(); 
             

                string xmlstr = Encoding.UTF8.GetString(strm.GetBuffer());
                XmlDocument tmp = new XmlDocument();

                tmp.LoadXml(xmlstr);

                strm.Close();
                sw.Close();
                XmlNode node  = XmlUtil.AddSubNode (parent,"DataSet");
                node.AppendChild(parent.OwnerDocument.ImportNode(tmp.SelectSingleNode("NewDataSet"),true));
                
            }
            return parent;
        }

        

        #region ICloneable 成员

        public object Clone()
        {
            UpdateData ret = new UpdateData();
            ret.LoadXml(this.ToXml(null));
            return ret;
            //throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
