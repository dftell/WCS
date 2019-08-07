using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Data;
using WolfInv.Com.DataCenter;
using XmlProcess;
using WolfInv.Com.MetaDataCenter;
namespace WolfInv.Com.WCS_Process
{
    public class DataCenterClient
    {
        DataInterface DataAccObj;
        
        //WebDataInterface wdi_xml;
        public void RunTool()
        {
            //
            DataAccessCenter.RunTool();
        }

        //ConnectObject connobj;
        string AppXmlPath;
        public DataCenterClient(string apppath, Dictionary<string, ConnectObject> connobjs_data, Dictionary<string, ConnectObject> connobjs_file)
        {
            //
            AppXmlPath = apppath;
            ConnectObject connobj = null;
            foreach (ConnectObject obj in connobjs_data.Values)
            {
                connobj = obj;
                break;
            }
            if (connobj == null)
                throw new Exception("没有定义连接对象！");
            if (connobj.Method == ConnectMethod.DataBase)
            {
                DataAccObj = new ClientDataInterface(apppath, connobjs_data);
            }
            else
            {
                DataAccObj = new WebDataInterface(apppath,connobjs_data);
            }
            
        }

        public DataSet GetData(string userid, string dsrname,List<DataCondition> conditions, out string msg)
        {
            UserGlobalShare ugs = null;
            bool rs = GlobalShare.UserAppInfos.TryGetValue(userid, out ugs);
            if (!rs || ugs == null)
            {
                msg = string.Format("用户{0}非法！",userid);
                return null;
            }
            DataSource ds = null;
            rs = ugs.mapDataSource.TryGetValue(dsrname, out ds);
            if (!rs || ds == null)
            {
                msg = string.Format("数据源集{0}未定义或者加载错误！", dsrname);
                return null;
            }
            return GetData(ds, conditions, out msg);
        }

        public DataSet GetData(string userid, string dsrname,out string msg)
        {
            return GetData(userid, dsrname, null, out msg);
        }

        public DataSet GetData(DataSource ds,out string msg)
        {
            XmlNode xml = ds.xmlReq;
            return GetData(xml,out msg);
        }

        public DataSet GetData(DataSource ds,List<DataCondition> conditions,out string msg)
        {
            XmlNode xml = ds.xmlReq;
            XmlDocument xmldoc = new XmlDataDocument();
            xmldoc.LoadXml(xml.OuterXml);
            xml = xmldoc.SelectSingleNode(".");
            XmlNode condgroupnode = xml.SelectSingleNode("ds/condition");  
            DataCondition reqcond = new DataCondition();
            DataCondition.FillCondition(xml.SelectSingleNode("ds/condition/c"), ref reqcond);
            if (conditions != null)
            {
                for (int i = 0; i < conditions.Count; i++)
                {
                    if ((conditions[i].Datapoint == null || conditions[i].Datapoint.Name == "" || conditions[i].value == "") && (conditions[i].SubConditions == null || conditions[i].SubConditions.Count == 0))
                        continue;
                    if (reqcond == null)
                        reqcond = new DataCondition();
                    if (reqcond.SubConditions == null)
                        reqcond.SubConditions = new List<DataCondition>();
                    reqcond.SubConditions.Add(conditions[i]);

                    //改为增加条件
                    /*
                    XmlNodeList nodes = xml.SelectNodes(string.Format("ds/condition//c[@i='{0}']",conditions[i].Datapoint.Name));

                    foreach (XmlNode node in nodes)
                    {
                        XmlAttribute att = node.Attributes["v"];
                        if (att == null)
                        {
                            att = node.OwnerDocument.CreateAttribute("v");
                            node.Attributes.Append(att);
                        }
                        att.Value = conditions[i].value;
                    }
                     */

                }
            }
            if (condgroupnode != null)
            {
                condgroupnode.RemoveAll();
            }
            else
            {
                //condgroupnode = new XmlNode();
            }
            reqcond.ToXml(condgroupnode);
            return GetData(xml,out msg);
        }

        public DataSet GetData(XmlNode xml,out string msg)
        {
            ////if (connobj.Method == ConnectMethod.WebSvr)
            ////{
            ////    return wdi.GetDataSet(xml,out msg);
            ////}
            ////return DataAccessCenter.GetDataList(xml,out msg);
            return DataAccObj.GetDataSet(xml, out msg);
        }

        public Dictionary<string, List<DataPoint>> GetViewDataPointList(DataSource ds)
        {
            Dictionary<string, List<DataPoint>> dptlist = new Dictionary<string, List<DataPoint>>();
            XmlNode node = null;
            ////if (connobj.Method == ConnectMethod.WebSvr)
            ////{
            ////    node = wdi.GetViewSettings(ds.xmlReq);
            ////}
            ////else
            ////{
            ////    node = DataAccessCenter.GetViewSource(ds.xmlReq);
            ////}
            node = DataAccObj.GetViewSettings(ds.xmlReq);
            if (node == null) return dptlist;
            XmlNodeList nodes = node.SelectNodes("/data/tab");
            foreach (XmlNode tabnode in nodes)
            {
                string strTab =  XmlUtil.GetSubNodeText(tabnode,"@name");
                if (dptlist.ContainsKey(strTab))
                {
                    continue;
                }
                List<DataPoint> tablist = new List<DataPoint>();
                XmlNodeList dptnodes = tabnode.SelectNodes("f");
                foreach (XmlNode dptnode in dptnodes)
                {
                    DataPoint dpt = new DataPoint();
                    dpt.LoadXml(dptnode);
                    tablist.Add(dpt);
                
                }
                dptlist.Add(strTab, tablist);
            }
            return dptlist;
        }

        public string UpdateDataList(DataSource ds, DataCondition conditions, UpdateData updata, DataRequestType type)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(ds.xmlReq.OuterXml);
            DataRequest dr;
            
            dr= new DataRequest(xmldoc.SelectSingleNode("ds"));
            if (ds.SubSource != null)
            {
                DataRequest subreq = new DataRequest(ds.SubSource.xmlReq);
                dr.SubRequestItem = subreq.RequestItems;
            }
            dr.updatedata = updata;
            dr.ConditonGroup = conditions;
            dr.RequestType = type;
            return UpdateDataList(dr.ToXml());
        }

         string UpdateDataList(XmlNode xml)
        {
            ////string msg = null;
            ////if (connobj.Method == ConnectMethod.WebSvr)
            ////{
            ////    msg = wdi.UpdateData(xml);
            ////    if (msg == "")
            ////    {
            ////        return null;
            ////    }
            ////    return msg;
            ////}
            ////else
            ////{

            ////}
            string ret = DataAccObj.UpdateData(xml);
            if (ret == "")
            {
                ret = null;
            }
            return ret;
            //return DataAccessCenter.UpdateDataList(xml);
        }

        public string DisConnect()
        {
            return DataAccObj.DisConnect();
        }
    }

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

    public class ClientDataInterface : DataInterface
    {
        //AccessWebService aws;
        DataAccessCenter dac;
        public ClientDataInterface(string apppath,Dictionary<string, ConnectObject> objs)
            : base(apppath,objs)
        {
            if (connobj == null) return;
            XmlDocument xmlmap = GlobalShare.GetXmlFile("\\xml\\dataidmapping.xml");
            XmlDocument xmlcol = GlobalShare.GetXmlFile("\\xml\\sdataidmapping.xml");
            if (GlobalShare.DataConnectObjs == null) return;
            dac = new DataAccessCenter(GlobalShare.AppPath , GlobalShare.DataConnectObjs ,xmlmap,xmlcol);
        }
        public override DataSet GetDataSet(XmlNode xml, out string msg)
        {

            return DataAccessCenter.GetDataList(xml,out msg);
            //throw new Exception("The method or operation is not implemented.");
        }

        public override string UpdateData(XmlNode xml)
        {
            return DataAccessCenter.UpdateDataList(xml);
            //throw new Exception("The method or operation is not implemented.");
        }

        public override XmlNode GetViewSettings(XmlNode xml)
        {
            string msg = null;
            return DataAccessCenter.GetViewSource(xml);
            //throw new Exception("The method or operation is not implemented.");
        }

        public override string DisConnect()
        {
            return DataAccessCenter.DisConnect();
            //throw new Exception("The method or operation is not implemented.");
        }

        public override XmlDocument GetConfigXmlFile(string filepath,string module, string screen, string target, string flag)
        {
            XmlDocument xmldoc = GlobalShare.GetXmlFileString(filepath,module, screen, target, flag);

            return xmldoc;   
        }
    }

    

    public class DataSource:ICloneable 
    {
        public string SourceName;
        public XmlNode xmlReq;
        public DataSource SubSource;
        public DataSource(string datasrc)
        {
            SourceName = datasrc;
        }

        public static Dictionary<string, DataSource> GetDataSourceMapping()
        {
            Dictionary<string, DataSource> ret = new Dictionary<string, DataSource>();

            ////string xmlpath = string.Format("{0}\\xml\\datasource.xml",apppath);
            ////XmlDocument xmldoc = new XmlDocument();
            ////try
            ////{
            ////    xmldoc.Load(xmlpath);
            ////}
            ////catch(Exception ce)
            ////{
            ////    throw new Exception("can't get DataSource Config!");
            ////}
            XmlDocument xmldoc = GlobalShare.GetXmlFile("\\xml\\datasource.xml");
            if(xmldoc == null) throw new Exception("can't get DataSource Config!");
            XmlNodeList nodes = xmldoc.SelectNodes("/datasrcs/ds");
            foreach(XmlNode node in nodes)
            {
                string srcname = XmlUtil.GetSubNodeText(node,"@id");
                if(srcname != "" && !ret.ContainsKey(srcname))
                {
                    DataSource ds = new DataSource(srcname);
                    ds.xmlReq = node;
                    ret.Add(srcname,ds);
                }
            }
            return ret;
        }

        public static DataSet InitDataSource(string dscName, string[] keys, string[] values)
        {
            string msg = null;
            return InitDataSource(dscName,keys,values,GlobalShare.DefaultUser,out msg);
        }
        
        public static DataSet InitDataSource(string dsrcName,string[] keys,string[] values,string uid,out string msg)
        {
            msg = null;
            List<DataCondition> dcs = new List<DataCondition>();
            if (keys != null && values != null)
            {
                int cnt = keys.Length;
                if (values.Length < keys.Length)
                {
                    cnt = values.Length;
                }
                for (int i = 0; i < cnt; i++)
                {
                    DataCondition dc = new DataCondition();
                    dc.Datapoint = new DataPoint(keys[i]);
                    dc.value = values[i];
                    dcs.Add(dc);
                }
            }
            return InitDataSource(dsrcName, dcs,uid,out msg);
        }

        public static DataSet InitDataSource(string dsrcName, List<DataCondition> dc,string uid,out string msg)
        {
            if (!GlobalShare.UserAppInfos.ContainsKey(uid))
            {
                msg = string.Format("用户[{0}]未登录！",uid);
                return null;
            }
            msg = null;
            if (!GlobalShare.UserAppInfos[uid].mapDataSource.ContainsKey(dsrcName))
            {
                msg = string.Format("所查询数据源{0}未定义！",dsrcName);
                return null;
            }
            DataSource ds = GlobalShare.UserAppInfos[uid].mapDataSource[dsrcName];
            
            return GlobalShare.DataCenterClientObj.GetData(ds, dc,out msg);
        }

        public static DataSet InitDataSource(DataSource dsrobj, List<DataCondition> dcs, out string msg)
        {
            return GlobalShare.DataCenterClientObj.GetData(dsrobj, dcs, out msg);
        }

        public static Dictionary<string, DataSource> GetGlobalSourcesClone()
        {
            Dictionary<string, DataSource> ret = new Dictionary<string, DataSource>();
            foreach(DataSource ds in GlobalShare.mapDataSource.Values)
            {
                if (!ret.ContainsKey(ds.SourceName))
                {
                    ret.Add(ds.SourceName, ds.Clone() as DataSource);
                }
            }
            return ret;
        }

        #region ICloneable 成员

        public object Clone()
        {
            DataSource ds = new DataSource(this.SourceName);
            ds.xmlReq = this.xmlReq.CloneNode(true);
            if (this.SubSource != null)
            {
                ds.SubSource = this.SubSource.Clone() as DataSource;
            }
            else
            {
                ds.SubSource = null;
            }
            return ds;
        }

        #endregion
    }

}
