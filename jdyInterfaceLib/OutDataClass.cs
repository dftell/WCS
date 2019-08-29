﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using WolfInv.Com.WCSExtraDataInterface;
using WolfInv.Com.JsLib;
using XmlProcess;
using System.Reflection;
namespace WolfInv.Com.jdyInterfaceLib
{
    public class JdyOutDataClass:IWCSExtraDataInterface
    {
        string strAccess_Token;
        string strdbId;
        public JdyOutDataClass()
        {
            strAccess_Token = jdy_GlbObject.Access_token;
            strdbId = jdy_GlbObject.dbId;
        }

        

        public bool getXmlData(XmlNode config, ref XmlDocument doc,ref XmlDocument xmlschema,ref string msg)
        {
            string strDefaultName = "DataTable1";
            string strRootName = "NewDataSet";
            string ret = "";
            string strName = XmlUtil.GetSubNodeText(config, "module/@name");
            Assembly assem = Assembly.GetExecutingAssembly();
            try
            {
                Type t = assem.GetType(string.Format("{0}.{1}",assem.FullName.Split(',')[0],strName));
                if(t == null)
                {
                    msg = "无法识别的外部访问类";
                    return false;
                }
                JDYSCM_Bussiness_List_Class jdyreq = Activator.CreateInstance(t) as JDYSCM_Bussiness_List_Class;
                jdyreq.InitClass(jdy_GlbObject.mlist[t.Name]);
                jdyreq.InitRequestJson();
                jdyreq.RequestSizeAndPage(1000, 1);
                if(jdy_GlbObject.mlist[t.Name].RequestMethodUseGET)
                {
                    ret = jdyreq.GetRequest();
                }
                else
                    ret = jdyreq.PostRequest();
                string strXml = XML_JSON.Json2XML("{\"root\":{0}}".Replace("{0}", ret));
                XmlDocument schemadoc = jdyreq.getRequestSchema();
                XmlDocument tmp = new XmlDocument();
                tmp.LoadXml(strXml);
                Json2XmlClass j2x = new Json2XmlClass(schemadoc);
                if(!j2x.getDataSetXml(ret, ref doc, ref msg))
                {
                    return false;
                }
                int totalPage = int.Parse(XmlUtil.GetSubNodeText(tmp, "root/totalPages"));
                int totalsize = int.Parse(XmlUtil.GetSubNodeText(tmp, "root/totalsize"));
                if (totalPage>1)//如果不止一页
                {
                    for(int i=2;i<=totalPage;i++)
                    {
                        jdyreq.RequestSizeAndPage(totalsize, i);
                        if (jdy_GlbObject.mlist[t.Name].RequestMethodUseGET)
                        {
                            ret = jdyreq.GetRequest();
                        }
                        else
                            ret = jdyreq.PostRequest();
                        strXml = XML_JSON.Json2XML("{\"root\":{0}}".Replace("{0}", ret));
                        tmp = new XmlDocument();
                        tmp.LoadXml(strXml);
                        if(!j2x.getDataSetXml(ret,ref doc,ref msg))
                        {
                            return false;
                        }
                    }
                }

            }
            catch(Exception e)
            {
                msg = e.Message;
                return false;
            }
            return true;
        }

       

       
       
        public bool writeXmlData(XmlNode config, DataSet data,ref XmlDocument doc, ref XmlDocument xmlschema,ref string msg, string writetype = "Add")
        {
            doc = null;
            string strDefaultName = "DataTable1";
            string strRootName = "NewDataSet";
            string ret = "";
            string strName = XmlUtil.GetSubNodeText(config, "module/@name");
            Assembly assem = Assembly.GetExecutingAssembly();
            strName = string.Format(strName, writetype);
            try
            {
                Type t = assem.GetType(string.Format("{0}.{1}", assem.FullName.Split(',')[0], strName));
                if (t == null)
                {
                    msg = "无法识别的类型";
                    return false;
                }
                
                JDYSCM_Bussiness_List_Class jdyreq = Activator.CreateInstance(t) as JDYSCM_Bussiness_List_Class;
                ////if (jdyreq is JDYSCM_SaleOrder_Update_Class)
                ////{
                ////    msg = "销售订单外部数据不能修改，只能新增或删除！";
                ////    return false;
                ////}
                jdyreq.InitClass(jdy_GlbObject.mlist[t.Name]);
                jdyreq.InitRequestJson();
                XmlDocument schemadoc = jdyreq.getRequestSchema();
                Json2XmlClass j2x = new Json2XmlClass(schemadoc);
                //jdyreq.Req_PostData = getRequestJson(jdyreq,data);
                jdyreq.Req_PostData = j2x.getJsonString(data);
                if (jdy_GlbObject.mlist[t.Name].RequestMethodUseGET)
                {
                    ret = jdyreq.GetRequest();
                }
                else
                    ret = jdyreq.PostRequest();
                string strXml = XML_JSON.Json2XML("{\"root\":{0}}".Replace("{0}", ret));
                XmlDocument tmp = new XmlDocument();
                tmp.LoadXml(strXml);
                ////doc = FillXml(tmp, doc, strRootName, strDefaultName);
                ////xmlschema = getSchema(doc.SelectSingleNode(strRootName), strDefaultName);

                ////doc = ClearSubNode(doc, strRootName, strDefaultName);
                
                if(!j2x.getDataSetXml(ret,ref doc,ref msg))
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                msg = e.Message;
                return false;
            }
            return true;
        }

        string getRequestJson(JDYSCM_Bussiness_List_Class jc,DataSet ds)
        {
            return jc.RequestDataSet(ds);
            return null;
        }

        public bool getJsonData(XmlNode config, ref string strJson, ref string msg)
        {
            msg = "方法未实现！";
            return false;
        }

        public bool getDataSet(XmlNode config, ref DataSet ds, ref string msg)
        {
            msg = "方法未实现！";
            return false;
        }

        public bool writeJsonData(XmlNode config, DataSet data, ref string strJson, ref string msg, string writetype = "Add")
        {
            msg = "方法未实现！";
            return false;
        }

        public bool writeDataSet(XmlNode config, DataSet data, ref DataSet ret, ref string msg, string writetype = "Add")
        {
            msg = "方法未实现！";
            return false;
        }
    }

    
}
