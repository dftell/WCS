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

        
        string getTranReqs(XmlNode node)
        {
            if (node == null)
                return null;
            return null;
        }

        public bool getXmlData(XmlNode config, ref XmlDocument doc,ref XmlDocument xmlschema,ref string msg, XmlNode condition = null)
        {
            string strDefaultName = "DataTable1";
            string strRootName = "NewDataSet";
            string ret = "";
            string strName = XmlUtil.GetSubNodeText(config, "module/@name");
            string strReqJson = null;

            XmlNode xmlreq = config.SelectSingleNode("req");
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
                jdyreq.RequestSizeAndPage(1000, 1,xmlreq);
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
                        jdyreq.RequestSizeAndPage(totalsize, i,xmlreq);
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

        public bool getJsonData(XmlNode config, ref string strJson, ref string msg, XmlNode condition = null)
        {
            msg = "方法未实现！";
            return false;
        }

        public bool getDataSet(XmlNode config, ref DataSet ds, ref string msg, XmlNode condition = null)
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

    public class Json2XmlClass
    {
        XmlDocument schemadoc;
        List<TableGuider> tablist = new List<TableGuider>();
        Dictionary<string, TableGuider> dictabs = new Dictionary<string, TableGuider>();
        public Json2XmlClass(XmlDocument xmldoc)
        {
            schemadoc = xmldoc;
            if (schemadoc == null)
            {
                return;
            }
            InitTableGuides(schemadoc);
        }

        void InitTableGuides(XmlDocument doc)
        {
            XmlNode root = doc.SelectSingleNode("req");
            if (root == null)
                return;
            XmlNodeList tables = root.SelectNodes("Schema/Table");
            tablist = new List<TableGuider>();
            for (int i = 0; i < tables.Count; i++)
            {
                tablist.Add(new TableGuider(tables[i]));
                if (i > 0)
                {
                    dictabs.Add(tablist[i].TableName, tablist[i]);
                }

            }
            //dictabs = tablist.Skip(1).ToDictionary(a=>a.TableName,a=>a);

        }

        public bool getDataSetXml(string json, ref XmlDocument xmldoc, ref string msg, string checkNames = "code,errcode", string errmsg = "msg")
        {
            if (xmldoc == null)
            {
                xmldoc = new XmlDocument();
                xmldoc.LoadXml("<NewDataSet/>");
            }
            if (tablist.Count == 0)
            {
                msg = "未指定指引配置！";
                return false;
            }
            string mainname = tablist[0].TableName;
            XmlNode rootnode = xmldoc.SelectSingleNode("NewDataSet");
            if (string.IsNullOrEmpty(mainname))
            {
                msg = "指定指引中主表名未配置！";
                return false;
            }
            try
            {
                string strXml = XML_JSON.Json2XML("{\"root\":{0}}".Replace("{0}", json));
                XmlDocument tmp = new XmlDocument();
                tmp.LoadXml(strXml);
                XmlNode tmproot = tmp.SelectSingleNode("root");
                string[] errs = checkNames.Split(',');
                string errkey = null;
                for (int i = 0; i < errs.Length; i++)
                {
                    if (tmproot.SelectSingleNode(errs[i]) != null)
                    {
                        errkey = errs[i];
                        break;
                    }
                }
                if (errkey != null)
                {
                    if (XmlUtil.GetSubNodeText(tmproot, errkey) != "" && XmlUtil.GetSubNodeText(tmproot, errkey) != "0")//如果错误不等于0
                    {
                        msg = XmlUtil.GetSubNodeText(tmproot, string.Format("{0}", errmsg));
                        XmlNodeList mitemnodes = tmproot.SelectNodes(mainname);
                        if (mitemnodes.Count == 0)
                            return false;
                    }
                }
                XmlNamespaceManager xmlm = new XmlNamespaceManager(xmldoc.NameTable);
                xmlm.AddNamespace("json", "http://james.newtonking.com/projects/json");//添加命名空间
                XmlNodeList itemnodes = tmproot.SelectNodes(mainname);
                for (int i = 0; i < itemnodes.Count; i++)
                {
                    string submsg = null;
                    if (XmlUtil.GetSubNodeText(itemnodes[0], errkey) != "" && XmlUtil.GetSubNodeText(itemnodes[i], errkey) != "0")
                    {
                        submsg = XmlUtil.GetSubNodeText(itemnodes[i], errmsg);
                        msg = submsg;
                        return false;
                    }

                    //XmlNode NewRow = xmldoc.CreateElement(mainname, xmlm.LookupNamespace("json"));
                    XmlNode NewRow = xmldoc.CreateElement(mainname);
                    //XmlAttribute att = xmldoc.CreateAttribute("json:Array", xmlm.LookupNamespace("json"));
                    //att.Value = "true";
                    //NewRow.Attributes.Append(att);
                    rootnode.AppendChild(NewRow);
                    foreach (XmlNode cell in itemnodes[i].ChildNodes)
                    {
                        if (cell.ChildNodes.Count > 1)//如果有多节点
                        {
                            if (!dictabs.ContainsKey(cell.Name))//非定义的表跳过
                            {
                                continue;
                            }
                            AddSubTables(rootnode, cell, cell.Name, dictabs[cell.Name], itemnodes[i], mainname, xmlm);//新建表只加一层子节点,上级关键字及值传入
                        }
                        else
                        {
                            if (cell.FirstChild is XmlElement)//如果是元素
                            {
                                if (!dictabs.ContainsKey(cell.Name))//如果未定义跳过
                                {
                                    continue;
                                }
                                AddSubTables(rootnode, cell, cell.Name, dictabs[cell.Name], itemnodes[i], mainname, xmlm);//新建表只加一层子节点及值传入
                            }
                            else
                            {
                                cell.InnerText = cell.InnerText.Trim();//去除空格
                                NewRow.AppendChild(xmldoc.ImportNode(cell, true));//加简单节点
                            }
                        }

                    }
                }

            }
            catch (Exception ce)
            {
                msg = ce.Message;
                return false;
            }
            return true;
        }

        void AddSubTables(XmlNode rootnode, XmlNode cell, string subname, TableGuider tg, XmlNode mainnode, string mainname, XmlNamespaceManager xmlm)
        {
            XmlDocument xmldoc = ((rootnode is XmlDocument) ? rootnode as XmlDocument : rootnode.OwnerDocument);
            //XmlNode subnode = xmldoc.CreateElement(cell.Name, xmlm.LookupNamespace("json"));
            XmlNode subnode = xmldoc.CreateElement(cell.Name);
            //XmlAttribute att = xmldoc.CreateAttribute("json:Array", xmlm.LookupNamespace("json"));
            //att.Value = "true";
            //subnode.Attributes.Append(att);
            rootnode.AppendChild(subnode);
            foreach (XmlNode scell in cell.ChildNodes)
            {
                if (scell.ChildNodes.Count > 1)
                {
                    continue;
                }
                else
                {
                    if (scell.FirstChild is XmlElement)
                    {

                    }
                    else
                    {
                        scell.InnerText = scell.InnerText.Trim();//去除空格
                        subnode.AppendChild(xmldoc.ImportNode(scell, true));
                    }
                }
            }
            string[] keys = tg.Key.Split(tg.KeySplitor.ToCharArray());
            for (int i = 0; i < keys.Length; i++)
            {
                XmlNode keynode = xmldoc.CreateElement(string.Format("{0}_{1}", mainname, keys[i]));//防止子表有同样的节点，节点以主表+主表key命名
                XmlNode existKeyNode = mainnode.SelectSingleNode(keys[i]);
                if (existKeyNode != null)
                {
                    keynode.InnerText = existKeyNode.InnerText.Trim();
                }
                subnode.AppendChild(keynode);
            }
        }

        XmlDocument getSchema(string strSchema, XmlNode rootnode, string strDefaultName)
        {
            XmlDocument xmlschema = new XmlDocument();
            //xmlschema.LoadXml(jdy_GlbObject.getText("System.OutData.Schema", "schema", ".xsd"));
            xmlschema.LoadXml(strSchema);
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

        public virtual string getJsonString(DataSet ds)
        {

            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<root/>");
            if (doc == null)
            {
                return null;
            }

            XmlNode root = doc.SelectSingleNode("root");

            //root.AppendChild(doc.CreateElement(tablist[0].TableName));
            root = FillXmlByDatable(root, ds, 0, tablist);

            string ret = XML_JSON.XML2Json(doc, root.Name, true);
            tablist.ForEach(a =>
            {
                ret = ret.Replace(string.Format("${0}", a.TableName), a.TableName);

            });
            return ret;
        }
        XmlNode FillXmlByDatable(XmlNode parent, DataSet ds, int dtindex, List<TableGuider> guds)
        {
            if (ds.Tables.Count <= dtindex)
            {
                throw new Exception("Jdy:数据集超出索引！");
            }
            if (guds.Count <= dtindex)
            {
                throw new Exception("Jdy:Schema配置超出索引！");
            }

            TableGuider tg = guds[dtindex];
            TableGuider pretg = null;
            if (dtindex > 0)
            {
                pretg = guds[dtindex - 1];
            }
            if (tg.TableName == null || tg.TableName.Trim().Length == 0)
                throw new Exception("Jdy:Schema配置Json超出索引！");
            DataTable dt = ds.Tables[dtindex];

            string RepeatItem = guds[dtindex].TableName;
            XmlDocument doc = parent.OwnerDocument;
            if (doc == null)
                doc = parent as XmlDocument;
            XmlNamespaceManager xmlm = new XmlNamespaceManager(doc.NameTable);
            xmlm.AddNamespace("json", "http://james.newtonking.com/projects/json");//添加命名空间
            string filter = "";
            if (pretg == null || pretg.KeyValue == null)
            {
                filter = "1=1";
            }
            else
            {
                filter = string.Format("{0}='{1}'", pretg.NextRef, pretg.KeyValue);
            }
            DataRow[] drs = dt.Select(filter);
            for (int i = 0; i < drs.Length; i++)
            {

                XmlNode node = doc.CreateElement(RepeatItem, xmlm.LookupNamespace("json"));
                XmlAttribute att = doc.CreateAttribute("json:Array", xmlm.LookupNamespace("json"));
                att.Value = "true";
                node.Attributes.Append(att);
                //XmlUtil.AddAttribute(node, "json:Array", "true");
                string keyval = null;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string col = dt.Columns[j].ColumnName;
                    string val = drs[i][col].ToString();
                    XmlNode subnode = doc.CreateElement(col);
                    subnode.InnerText = val;
                    if (tg.Key == null || tg.Key.Trim().Length < dtindex + 1)
                    {
                        node.AppendChild(subnode);
                        continue;
                    }
                    if (tg.Key != null && tg.Key == col.ToLower())
                    {
                        keyval = val;
                    }
                    node.AppendChild(subnode);
                }
                if (keyval == null && string.IsNullOrEmpty(tg.Key))//没遇到主键，下一行
                {
                    parent.AppendChild(node);
                    continue;
                }
                int NextIdx = dtindex + 1;
                if (ds.Tables.Count <= NextIdx || guds.Count <= NextIdx)
                {
                    parent.AppendChild(node);
                    continue;
                }
                guds[NextIdx].KeyValue = keyval;
                node = FillXmlByDatable(node, ds, NextIdx, guds);
                parent.AppendChild(node);
            }
            return parent;
        }

    }


}
