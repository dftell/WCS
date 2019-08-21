using System;
using System.Collections.Generic;
using System.Xml;
using System.Data;
using WolfInv.Com.MetaDataCenter;
namespace WolfInv.Com.WCS_Process
{
    public abstract class DataInterface
    {
        protected ConnectObject connobj;
        public DataInterface(string apppath,Dictionary<string,ConnectObject> objs)
        {
            if (objs == null) return;
            foreach (ConnectObject conn in objs.Values)
            {
                connobj = conn;
                if (conn.Method == ConnectMethod.WebSvr)
                {
                    
                    break;
                }
            }
            if (connobj == null)
            {
                throw new Exception("未定义访问的Web站点！");
            }
        }

        public abstract DataSet GetDataSet(XmlNode xml, out string msg);
        
        public DataSet GetDataSet(XmlNode xml)
        {
            string msg = null;
            return GetDataSet(xml, out msg);
        }
        public abstract string UpdateData(XmlNode xml);
        public abstract XmlNode GetViewSettings(XmlNode xml);
        public abstract XmlDocument GetConfigXmlFile(string filepath,string module, string screen, string target, string flag);
        public XmlDocument GetConfigXmlFile(string filepath)
        {
            return GetConfigXmlFile(filepath, null, null, null, null);
        }
        public abstract string DisConnect();
    }

}
