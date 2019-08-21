using System;
using System.Collections.Generic;
using System.Xml;
using System.Data;
using XmlProcess;
using WolfInv.Com.MetaDataCenter;
namespace WolfInv.Com.WCS_Process
{
    public class WebDataInterface : DataInterface
    {
        AccessWebService aws;
        public WebDataInterface(string path,Dictionary<string, ConnectObject> objs)
            : base(path,objs)
        {
            if (connobj == null) return;
            aws = new AccessWebService(connobj);
        }
        public override DataSet GetDataSet(XmlNode xml, out string msg)
        {
            return aws.GetDataSet("GetDataList", xml, out msg);
            //throw new Exception("The method or operation is not implemented.");
        }

        public override string UpdateData(XmlNode xml)
        {
            return aws.GetString("UpdateData", xml);
            //throw new Exception("The method or operation is not implemented.");
        }

        public override XmlNode GetViewSettings(XmlNode xml)
        {
            string msg = null;
            return aws.GetXml("GetViewSettings", xml, out msg);
            //throw new Exception("The method or operation is not implemented.");
        }

        public override string DisConnect()
        {
            return null;
            //throw new Exception("The method or operation is not implemented.");
        }

        public override XmlDocument GetConfigXmlFile(string filepath,string module, string screen, string target, string flag)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml("<req/>");
            XmlNode root = xmldoc.SelectSingleNode("req");
            XmlUtil.AddAttribute(root, "reqtype", "file");
            XmlUtil.AddAttribute(root, "filepath", filepath);
            XmlUtil.AddAttribute(root, "module", module);
            XmlUtil.AddAttribute(root, "screen", screen);
            XmlUtil.AddAttribute(root, "target", target);
            XmlUtil.AddAttribute(root, "flag", flag);
            string msg = null;
            XmlDocument xmlret = aws.GetXml("GetXmlSettings", xmldoc, out msg) as XmlDocument;
            if (xmlret == null || msg != null)
            {
                throw new Exception(string.Format("获得文件{0}失败。失败原因：{1}",filepath,msg));
                //return null;
            }
            return xmlret;
        }
    }

}
