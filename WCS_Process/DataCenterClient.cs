using System;
using System.Collections.Generic;
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
            //DataAccessCenter.RunTool();
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
                    if(!ds.AllDataPoint.ContainsKey(conditions[i].Datapoint.Name))//如果数据源内没有的数据点，暂时不作为条件。就算是联结表中的字段，如果要查询，一定可以放入数据源中。
                    {
                        continue;
                    }
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

}
