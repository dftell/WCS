using System;
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
                XmlDocument tmp = new XmlDocument();
                tmp.LoadXml(strXml);
                doc = FillXml(tmp, doc, strRootName, strDefaultName);
                xmlschema = getSchema(doc.SelectSingleNode(strRootName), strDefaultName);
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
                        FillXml(tmp, doc, strRootName, strDefaultName);
                    }
                }
                doc = ClearSubNode(doc, strRootName, strDefaultName);
            
            }
            catch(Exception e)
            {
                msg = e.Message;
                return false;
            }
            return true;
        }

       

        XmlDocument FillXml(XmlDocument tmp,XmlDocument doc,string rootName,string strDefaultName)
        {
            
            try
            {
                if (doc == null)
                {
                    doc = new XmlDocument();
                    doc.LoadXml(jdy_GlbObject.getText("System.OutData.Model", "schema", ".xml"));
                }
                string errkey = "root/errcode";
                XmlNode errnode = tmp.SelectSingleNode(errkey);
                if (errnode ==null)
                {
                    errkey = "root/code";
                    errnode = tmp.SelectSingleNode(errkey);
                }
                if (XmlUtil.GetSubNodeText(tmp, errkey) != "" && XmlUtil.GetSubNodeText(tmp, errkey) != "0")//如果错误不等于0
                {
                    throw new Exception(XmlUtil.GetSubNodeText(tmp, "root/msg"));
                    return doc;
                }
                XmlNode rootnode = doc.SelectSingleNode(rootName);
                XmlNodeList itemnodes = tmp.SelectNodes("root/items");
                //doc.LoadXml("<DataTable TableName=\"table1\"/>");
                for (int i = 0; i < itemnodes.Count; i++)
                {
                    XmlNode NewRow = doc.CreateElement(strDefaultName);
                    doc.SelectSingleNode(rootName).AppendChild(NewRow);
                    foreach (XmlNode cell in itemnodes[i].ChildNodes)
                    {
                        NewRow.AppendChild(doc.ImportNode(cell, true));
                    }
                    //XmlNode addnode = doc.SelectSingleNode("root").AppendChild(doc.ImportNode(itemnodes[i], true));

                }
            }
            catch(Exception ce)
            {
                throw ce;
            }
            return doc;
        }

        XmlDocument ClearSubNode(XmlDocument doc, string rootName, string strDefaultName)
        {
            foreach (XmlNode node in doc.SelectNodes(string.Format("{0}/{1}", rootName, strDefaultName)))
            {
                for (int i = node.ChildNodes.Count - 1; i >= 0; i--)
                {
                    XmlNode subnode = node.ChildNodes[i];
                    if (subnode.ChildNodes.Count > 1)
                    {
                        node.RemoveChild(subnode);
                    }
                    else
                    {
                        if (subnode.FirstChild is XmlElement)
                        {
                            node.RemoveChild(subnode);
                        }
                        else
                        {

                        }
                    }
                }
            }
            return doc;
        }

        XmlDocument getSchema(XmlNode rootnode,string strDefaultName)
        {
            XmlDocument xmlschema = new XmlDocument();
            xmlschema.LoadXml(jdy_GlbObject.getText("System.OutData.Schema", "schema", ".xsd"));
            XmlNamespaceManager xmlm = new XmlNamespaceManager(xmlschema.NameTable);
            xmlm.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema");//添加命名空间

            XmlNode node = xmlschema.SelectSingleNode("xs:schema/xs:element/xs:complexType/xs:choice/xs:element/xs:complexType/xs:sequence", xmlm);
            XmlNodeList list = rootnode.SelectNodes(strDefaultName);
            if (list.Count == 0)
            {
                return xmlschema;
            }
            HashSet<string> allcolumn = new HashSet<string>();
            for (int i = 0; i < list[0].ChildNodes.Count; i++)
            {
                if (allcolumn.Contains(list[0].ChildNodes[i].Name))
                {
                    continue;
                }
                if (list[0].ChildNodes[i].ChildNodes.Count > 1)
                {
                    continue;
                }
                else
                {
                    if (list[0].ChildNodes[i].FirstChild is XmlElement)
                    {
                        continue;
                    }
                }
                XmlNode el = xmlschema.CreateElement("xs:element", "http://www.w3.org/2001/XMLSchema");
                XmlAttribute attname = xmlschema.CreateAttribute("name");
                XmlAttribute atttype = xmlschema.CreateAttribute("type");
                attname.Value = list[0].ChildNodes[i].Name;
                atttype.Value = "xs:string";
                el.Attributes.Append(attname);
                el.Attributes.Append(atttype);
                if (!allcolumn.Contains(attname.Value))
                {
                    allcolumn.Add(attname.Value);
                }
                node.AppendChild(el);
            }
            return xmlschema;
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
                if (jdyreq is JDYSCM_SaleOrder_Update_Class)
                {
                    msg = "销售订单外部数据不能修改，只能新增或删除！";
                    return false;
                }
                jdyreq.InitClass(jdy_GlbObject.mlist[t.Name]);
                jdyreq.InitRequestJson();
                jdyreq.Req_PostData = getRequestJson(jdyreq,data);
                if (jdy_GlbObject.mlist[t.Name].RequestMethodUseGET)
                {
                    ret = jdyreq.GetRequest();
                }
                else
                    ret = jdyreq.PostRequest();
                string strXml = XML_JSON.Json2XML("{\"root\":{0}}".Replace("{0}", ret));
                XmlDocument tmp = new XmlDocument();
                tmp.LoadXml(strXml);
                doc = FillXml(tmp, doc, strRootName, strDefaultName);
                xmlschema = getSchema(doc.SelectSingleNode(strRootName), strDefaultName);
                
                doc = ClearSubNode(doc, strRootName, strDefaultName);

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
