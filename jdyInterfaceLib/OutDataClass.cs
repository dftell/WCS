using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using WolfInv.Com.WCS_Process;
using WolfInv.Com.JsLib;
using XmlProcess;
using System.Reflection;
namespace WolfInv.Com.jdyInterfaceLib
{
    public class JdyOutDataClass:WCSExtraDataInterface
    {
        string strAccess_Token;
        string strdbId;
        public JdyOutDataClass()
        {
            strAccess_Token = jdy_GlbObject.Access_token;
            strdbId = jdy_GlbObject.dbId;
        }

        public DataSet getDataSet(XmlNode config)
        {
            return new DataSet();
        }

        public string getJsonData(XmlNode config)
        {
            return null;
        }

        public XmlDocument getXmlData(XmlNode config,ref XmlDocument xmlschema)
        {
            XmlDocument doc = new XmlDocument();
            string strDefaultName = "DataTable1";
            string ret = "";
            string strName = XmlUtil.GetSubNodeText(config, "module/@name");
            Assembly assem = Assembly.GetExecutingAssembly();
            
            try
            {
                doc.LoadXml(jdy_GlbObject.getText("System.OutData.Model", "schema", ".xml"));
                Type t = assem.GetType(string.Format("{0}.{1}",assem.FullName.Split(',')[0],strName));
                if(t == null)
                {
                    return doc;
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
                if(XmlUtil.GetSubNodeText(tmp,"root/errcode") == "1")//如果错误不等于0
                {
                    return doc;
                }
                XmlNode rootnode = doc.SelectSingleNode("NewDataSet");
                XmlNodeList itemnodes = tmp.SelectNodes("root/items");
                //doc.LoadXml("<DataTable TableName=\"table1\"/>");
                for(int i=0;i<itemnodes.Count;i++)
                {
                    XmlNode NewRow = doc.CreateElement(strDefaultName);
                    doc.SelectSingleNode("NewDataSet").AppendChild(NewRow);
                    foreach (XmlNode cell in itemnodes[i].ChildNodes)
                    {
                        NewRow.AppendChild(doc.ImportNode(cell, true));
                    }
                    //XmlNode addnode = doc.SelectSingleNode("root").AppendChild(doc.ImportNode(itemnodes[i], true));
                    
                }
                xmlschema = new XmlDocument();
                xmlschema.LoadXml(jdy_GlbObject.getText("System.OutData.Schema", "schema", ".xsd"));
                XmlNamespaceManager xmlm = new XmlNamespaceManager(xmlschema.NameTable);
                xmlm.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema");//添加命名空间

                XmlNode node = xmlschema.SelectSingleNode("xs:schema/xs:element/xs:complexType/xs:choice/xs:element/xs:complexType/xs:sequence", xmlm);
                XmlNodeList list = rootnode.SelectNodes(strDefaultName);
                if (list.Count == 0)
                {
                    return doc;
                }
                HashSet<string> allcolumn = new HashSet<string>();
                for (int i = 0; i < list[0].ChildNodes.Count; i++)
                {
                    if(allcolumn.Contains(list[0].ChildNodes[i].Name))
                    {
                        continue;
                    }
                    if(list[0].ChildNodes[i].ChildNodes.Count>1)
                    {
                        continue;
                    }
                    else
                    {
                        if(list[0].ChildNodes[i].FirstChild is XmlElement)
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
                    if(!allcolumn.Contains(attname.Value))
                    {
                        allcolumn.Add(attname.Value);
                    }
                    node.AppendChild(el);
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
                        if (XmlUtil.GetSubNodeText(tmp, "root/errcode") == "1")//如果错误不等于0
                        {
                            return doc;
                        }
                        itemnodes = tmp.SelectNodes("root/items");
                        //doc.LoadXml("<root/>");
                        for (int j = 0; j < itemnodes.Count; j++)
                        {
                            XmlNode NewRow = doc.CreateElement(strDefaultName);
                            foreach (XmlNode cell in itemnodes[j].ChildNodes)
                            {
                                NewRow.AppendChild(doc.ImportNode(cell, true));
                            }
                            //XmlNode addnode = doc.SelectSingleNode("root").AppendChild(doc.ImportNode(itemnodes[i], true));
                            doc.SelectSingleNode("NewDataSet").AppendChild(NewRow);
                        }
                    }
                }
            
                
                
            }
            catch(Exception e)
            {

            }
            
            foreach(XmlNode node in doc.SelectNodes(string.Format("{0}/{1}","NewDataSet",strDefaultName)))
            {
                for (int i=node.ChildNodes.Count-1;i>=0 ;i--)
                {
                    XmlNode subnode = node.ChildNodes[i];
                    if (subnode.ChildNodes.Count > 1)
                    {
                        node.RemoveChild(subnode);
                    }
                    else
                    {
                        if(subnode.FirstChild is XmlElement)
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
    }
}
