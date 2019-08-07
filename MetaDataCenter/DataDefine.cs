using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using XmlProcess;
using System.Data;
using System.IO;
namespace WolfInv.Com.MetaDataCenter
{
    public class DataPoint:IXml
    {
        public string Name;
        public string DataType;
        public string Text;
        public int Width;
        public string ComboName;

        public DataPoint()
        {
        }

        public DataPoint(string pointname)
        {
            this.Name = pointname;
        }

        public static Dictionary<string, DataPoint> InitMapping(XmlDocument xmldoc)
        {
            if (xmldoc == null) return null;
            Dictionary<string, DataPoint> DataPointMappings = new Dictionary<string, DataPoint>();

            //string strDpmPath = string.Format("{0}\\xml\\dataidmapping.xml", strAppPath);

            //    XmlDocument xmldoc = new XmlDocument();
            
                ////try
                ////{
                ////    xmldoc.Load(strDpmPath);
                ////}
                ////catch (Exception ce)
                ////{
                ////    throw new Exception("can't init the DataIdMapping File!");
                ////}
                XmlNodeList nodes = xmldoc.SelectNodes("/flds/f");
                DataPointMappings = new Dictionary<string, DataPoint>();
                foreach (XmlNode node in nodes)
                {
                    DataPoint dp = new DataPoint();
                    dp.LoadXml(node);
                    ////dp.Name = XmlUtil.GetSubNodeText(node, "@i");
                    ////dp.DataType = XmlUtil.GetSubNodeText(node, "@type");
                    if (!DataPointMappings.ContainsKey(dp.Name))
                    {
                        DataPointMappings.Add(dp.Name, dp);
                    }
                }
                return DataPointMappings;
        }

        public void LoadXml(XmlNode node)
        {
            Name = XmlUtil.GetSubNodeText(node, "@i");
            DataType = XmlUtil.GetSubNodeText(node, "@type");
            Text = XmlUtil.GetSubNodeText(node, "@udlbl");
            string strWidth = XmlUtil.GetSubNodeText(node, "@width");
            int.TryParse(strWidth, out Width);
            ComboName = XmlUtil.GetSubNodeText(node, "@combo");

        }

         public XmlNode ToXml(XmlNode parent)
        {
            XmlDocument xmldoc;
            if (parent == null)
            {
                xmldoc = new XmlDocument();
                xmldoc.Load("root");
                parent = xmldoc.SelectSingleNode("root");
            }
            xmldoc = parent.OwnerDocument;
            XmlNode node = XmlUtil.AddSubNode(parent, "f", true);
            XmlUtil.AddAttribute(node, "i", Name);
             if(Text != null && Text !="")
                XmlUtil.AddAttribute(node, "udlbl", Text);
            if(ComboName != null && ComboName != "")
                XmlUtil.AddAttribute(node, "combo", ComboName);
            if(Width > 0)
                XmlUtil.AddAttribute(node, "width", Width.ToString());
            XmlUtil.AddAttribute(node, "type", DataType);
            return node;
        }

        
        
    }

    public enum DataRequestType
    {
        Query,
        Add,
        Update,
        Delete,
        Import,
        BatchUpdate
    }

    public class DataRequest
    {
        public DataRequestType RequestType = DataRequestType.Query;
        public UpdateData updatedata;
        public List<RequestItem> RequestItems;
        public DataCondition ConditonGroup;
        public List<RequestItem> SubRequestItem;
        protected XmlNode _xml;
        public List<OrderItem> OrderItems;
        public List<RequestItem> GroupByItems;
        public DataRequest(XmlNode xml)
        {
            _xml = xml;
            Init(_xml);
        }

        public DataRequest()
        {
        }

        

        void Init(XmlNode xml)
        {
            XmlNodeList nodes = xml.SelectNodes(".//req/f");
            RequestItems = new List<RequestItem>();
            foreach (XmlNode node in nodes)
            {
                RequestItem ri = new RequestItem();
                ri.LoadXml(node);
                RequestItems.Add(ri);
                //RequestItems.Add(new RequestItem(new DataPoint(XmlUtil.GetSubNodeText(node, "@i"))));
            }
            nodes = xml.SelectNodes(".//subreq/f");
            this.SubRequestItem  = new List<RequestItem>();
            foreach (XmlNode node in nodes)
            {
                RequestItem ri = new RequestItem();
                ri.LoadXml(node);
                this.SubRequestItem.Add(ri);
                //RequestItems.Add(new RequestItem(new DataPoint(XmlUtil.GetSubNodeText(node, "@i"))));
            }
            XmlNode condnode = xml.SelectSingleNode(".//condition/c");
            DataCondition.FillCondition(condnode,ref ConditonGroup);
            string strType = XmlUtil.GetSubNodeText(xml,"@type");
            #region request type
            switch (strType)
            {
                case "Update":
                    {
                        RequestType = DataRequestType.Update;
                        break;
                    }
                case "Delete":
                    {
                        RequestType = DataRequestType.Delete;
                        break;
                    }
                case "Add":
                    {
                        RequestType = DataRequestType.Add;
                        break;
                    }
                case "BatchUpdate":
                    {
                        RequestType = DataRequestType.BatchUpdate;
                        break;
                    }
                case "Query":
                default:
                    {
                        RequestType = DataRequestType.Query;
                        break;
                    }
            }
            #endregion
            updatedata = new UpdateData(xml.SelectSingleNode("//ds/data"));
            XmlNodeList ordnodes = xml.SelectNodes(".//order/f");
            if (ordnodes.Count > 0)
            {
                this.OrderItems = new List<OrderItem>();
                foreach (XmlNode node in ordnodes)
                {
                    OrderItem ord = new OrderItem();
                    ord.LoadXml(node);
                    this.OrderItems.Add(ord);
                }
            }
            XmlNodeList groupbynodes = xml.SelectNodes(".//groupby/f");
            GroupByItems = new List<RequestItem>();
            foreach (XmlNode node in groupbynodes)
            {
                RequestItem ri = new RequestItem();
                ri.LoadXml(node);
                GroupByItems.Add(ri);
            }
        }

        public XmlNode ToXml()
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml("<ds/>");
            XmlNode root = xmldoc.SelectSingleNode("ds");
            //请求类型
            XmlUtil.AddAttribute(root, "type", this.RequestType.ToString());
            //输出请求
            XmlNode reqnode = XmlUtil.AddSubNode(root, "req", false);
            for (int i = 0; i < this.RequestItems.Count; i++)
            {
                if (this.RequestItems[i].DataPt.Name != "")
                {
                    this.RequestItems[i].ToXml(reqnode);
                }
                //    XmlUtil.AddAttribute(XmlUtil.AddSubNode(reqnode, "i"), "f", this.RequestItems[i].DataPt.Name);
                
            }
            if (this.SubRequestItem != null)
            {
                XmlNode subreqnode = XmlUtil.AddSubNode(root, "subreq", false);
                for (int i = 0; i < this.SubRequestItem.Count; i++)
                {
                    if (this.SubRequestItem[i].DataPt.Name != "")
                    {
                        this.SubRequestItem[i].ToXml(subreqnode);
                    }
                    //    XmlUtil.AddAttribute(XmlUtil.AddSubNode(reqnode, "i"), "f", this.RequestItems[i].DataPt.Name);

                }
            }
            //输出条件
            if(this.ConditonGroup != null && (this.ConditonGroup.SubConditions != null && this.ConditonGroup.SubConditions.Count  >0 ||(this.ConditonGroup.Datapoint.Name !="")))//条件非空
            {
                XmlNode condition = XmlUtil.AddSubNode(root, "condition");
                this.ConditonGroup.ToXml(condition);
            }
            //更新数据项
            if (this.updatedata != null)
            {
                XmlNode data = XmlUtil.AddSubNode(root, "data");
                this.updatedata.ToXml(data);
                
            }
            //排列项
            if (this.OrderItems != null && this.OrderItems.Count > 0)
            {
                XmlNode ords = XmlUtil.AddSubNode(root, "order");
                for (int i = 0; i < OrderItems.Count; i++)
                {
                    OrderItems[i].ToXml(ords);
                }
            }
            //group by
            if (GroupByItems.Count > 0)
            {
                XmlNode gpbynode = XmlUtil.AddSubNode(root, "groupby", false);
                for (int i = 0; i < this.RequestItems.Count; i++)
                {
                    if (this.GroupByItems[i].DataPt.Name != "")
                    {
                        this.GroupByItems[i].ToXml(gpbynode);
                    }
                }
            }
            return root;
        }
    }
    [SerializableAttribute]
    public class DataCondition : FormatItem,IXml
    {
        public ConditionLogic Logic = ConditionLogic.And;
        public DataPoint Datapoint;
        public string value;
        public string strOpt = "=";//操作
        public List<DataCondition> SubConditions;
        public bool IsValidate = true;
        public Dictionary<string, DataCondition> AllList = new Dictionary<string,DataCondition>();


        public void LoadXml(XmlNode node)
        {
            this.Logic = XmlUtil.GetSubNodeText(node, "@l") == "Or" ? ConditionLogic.Or : ConditionLogic.And;
            this.Datapoint = new DataPoint(XmlUtil.GetSubNodeText(node, "@i"));
            this.strOpt = XmlUtil.GetSubNodeText(node, "@o") == "" ? "=" : XmlUtil.GetSubNodeText(node, "@o");
            this.value = XmlUtil.GetSubNodeText(node, "@v");
            (this as FormatItem).LoadXml(node);
        }

        public static void FillCondition(XmlNode node,ref DataCondition cond)
        {
            if (node == null) return;
            XmlNodeList nodes = node.SelectNodes("./c");
            
            if (cond == null)
            {
                cond = new DataCondition();
            }
            cond.Logic = XmlUtil.GetSubNodeText(node, "@l").Trim().ToLower() == "or" ? ConditionLogic.Or : ConditionLogic.And;
            cond.Datapoint = new DataPoint(XmlUtil.GetSubNodeText(node, "@i"));
            cond.strOpt = XmlUtil.GetSubNodeText(node, "@o") == "" ? "=" : XmlUtil.GetSubNodeText(node, "@o");
            cond.value = XmlUtil.GetSubNodeText(node, "@v");
            (cond as FormatItem).LoadXml(node);
            if (cond.Datapoint.Name != "" && cond.value == "")
                cond.IsValidate = false;
            if (cond.Datapoint.Name != "" && !cond.AllList.ContainsKey(cond.Datapoint.Name))
            {
                cond.AllList.Add(cond.Datapoint.Name, cond);
            }
            if (nodes.Count > 0)
            {
                cond.SubConditions  = new List<DataCondition>();
                for (int i = 0; i < nodes.Count; i++)
                {
                    string sPerm = XmlUtil.GetSubNodeText(nodes[i], "@perm");
                    if (sPerm == "0")
                    {
                        continue;
                    }
                    DataCondition dc = null;
                    FillCondition(nodes[i], ref dc);
                    
                    cond.SubConditions.Add(dc);
                }
            }
        }

        public XmlNode ToXml(XmlNode parent)
        {
            XmlDocument xmldoc;
            XmlNode node = parent;
            if (parent == null)
            {
                xmldoc = new XmlDocument();
                xmldoc.LoadXml("<c/>");
                parent = xmldoc.SelectSingleNode("c");
            }
            //XmlNode 
            if (node == null)
            {
                node = parent;
            }
            else
            {
                node = XmlUtil.AddSubNode(parent, "c", true);
            }
            if (node == null) return null;
            if (this.Datapoint != null && this.Datapoint.Name != "")
                XmlUtil.AddAttribute(node, "i", this.Datapoint.Name);
            if (this.Logic != ConditionLogic.And)
            {
                XmlUtil.AddAttribute(node, "l", this.Logic.ToString());
            }
            if (this.strOpt != "=")
            {
                XmlUtil.AddAttribute(node, "o", this.strOpt);
            }
            if (this.value != null && this.value != "")
            {
                XmlUtil.AddAttribute(node, "v", this.value);
            }
            (this as FormatItem).ToXml(node);
            if (this.SubConditions == null) 
                return node;
            for (int i = 0; i < this.SubConditions.Count ; i++)
            {
                DataCondition dc = this.SubConditions[i];
                dc.ToXml(node);
            }
            return node;
        }
    }
    
    public enum ConditionLogic
    {
        And,
        Or
    }

    public class FormatItem : IXml
    {
        public string Format;//请求函数格式
        public string FormatFields;//函数填充项
        public void LoadXml(XmlNode node)
        {
            this.Format = XmlUtil.GetSubNodeText(node, "@fmt");
            this.FormatFields = XmlUtil.GetSubNodeText(node, "@flds");
        }

        public XmlNode ToXml(XmlNode parent)
        {
            if (this.Format != null && this.Format.Trim().Length > 0)
            {
                XmlUtil.AddAttribute(parent, "fmt", this.Format);
                XmlUtil.AddAttribute(parent, "flds", this.FormatFields);
            }
            return parent;
        }
    }

    public class RequestItem : FormatItem,IXml
    {
        public RequestItem()
        {
        }

        public RequestItem(DataPoint dp)
        {
            this.DataPt = dp;
        }
        public DataPoint DataPt;
        public bool ReadOnly;
        
        public void LoadXml(XmlNode node)
        {
            this.DataPt = new DataPoint(XmlUtil.GetSubNodeText(node, "@i"));
            this.ReadOnly = XmlUtil.GetSubNodeText(node,"@r") == "1";
            (this as FormatItem).LoadXml(node);
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
            XmlUtil.AddAttribute(node, "i", this.DataPt.Name);
            if (this.ReadOnly)
            {
                XmlUtil.AddAttribute(node, "r", this.ReadOnly ? "1" : "0");
            }
            (this as FormatItem).ToXml(node);

            return node;
        }
    }

    public class DataChoiceItem : ICloneable
    {
        public string _Text;
        public string _Value;
        public string Text
        {
            get { return _Text; }
            set { _Text = value; }
        }

        public string Value
        {
            get { return _Value; }
            set { _Value = value; }
        }

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }

    public class DataRowChoiceItem : DataChoiceItem
    {
        public UpdateData Data;
        public DataRowChoiceItem()
        {
            Data = new UpdateData();
        }
    }

    public class UpdateData:ICloneable,IXml
    {
        public Dictionary<string, UpdateItem> Items = new Dictionary<string, UpdateItem>();
        public List<UpdateData> SubItems = new List<UpdateData>();
        public DataPoint keydpt;
        public string keyvalue;
        public bool Updated;
        public DataSet Dataset;
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

    public class UpdateItem:IXml
    {
        public DataPoint datapoint;
        public string value;
        public string text;
        public bool Validate = true;//表示该数据点是否需要修改
        /// <summary>
        /// 生成xml节点
        /// </summary>
        /// <param name="parent">指定父节点</param>
        /// <returns></returns>
        public XmlNode ToXml(XmlNode parent)
        {
            XmlDocument xmldoc;
            if (parent == null)
            {
                xmldoc = new XmlDocument();
                xmldoc.Load("root");
                parent = xmldoc.SelectSingleNode("root");
            }
            xmldoc = parent.OwnerDocument;
            XmlNode node = XmlUtil.AddSubNode(parent, "i", true);
            XmlUtil.AddAttribute(node, "f", datapoint.Name);
            XmlUtil.AddAttribute(node, "v", value);
            return node;
        }

        public UpdateItem()
        {
        }

        public UpdateItem(XmlNode node)
        {
            LoadXml(node);
        }

        public UpdateItem(string DataPoint, string val)
        {
            datapoint = new DataPoint(DataPoint);
            value = val;
        }

        public void LoadXml(XmlNode node)
        {
            datapoint = new DataPoint(XmlUtil.GetSubNodeText(node, "@f"));
            value = XmlUtil.GetSubNodeText(node, "@v");
        }
    }

    public class ItemValue:ICloneable 
    {
        public Dictionary<string, string> ItemValues;
        public string KeyValue;
        public string KeyText;

        #region ICloneable 成员

        public object Clone()
        {
            ItemValue ret = new ItemValue();
            ret.ItemValues = new Dictionary<string, string>();
            ret.KeyText = this.KeyText;
            ret.KeyValue = this.KeyValue;
            foreach (string key in this.ItemValues.Keys)
            {
                ret.ItemValues.Add(key, this.ItemValues[key]);
            }
            return ret;
        }

        #endregion
    }

    /// <summary>
    /// 数据连接方式
    /// </summary>
    public enum ConnectMethod
    {
        /// <summary>
        /// 连接WebService
        /// </summary>
        WebSvr,
        /// <summary>
        /// 直接连接数据库
        /// </summary>
        DataBase,
        ///<summary>本地xml</summary>
        ///
        ///
        Client
    }

    public class ConnectObject
    {
        public string id;
        public string ConnectString;
        public ConnectMethod Method;
        public ConnectObject(string strid, ConnectMethod method, string connStr)
        {
            id = strid;
            ConnectString = connStr;
            Method = method;
        }
    }

    public class OrderItem : RequestItem
    {
        //public string dpt;
        public string desc;

        public new  XmlNode ToXml(XmlNode parent)
        {
            ////XmlDocument xmldoc;
            ////if (parent == null)
            ////{
            ////    xmldoc = new XmlDocument();
            ////    xmldoc.Load("<root/>");
            ////    parent = xmldoc.SelectSingleNode("root");
            ////}
            ////XmlNode node = XmlUtil.AddSubNode(parent, "f", true);
            ////XmlUtil.AddAttribute(node, "i", dpt);
           //extend the order item as special requestitem by zhouys 3/30 2012
            XmlNode node = (this as RequestItem).ToXml(parent);
            if(desc != null && desc.Trim().Length >0)
                XmlUtil.AddAttribute(node, "d", desc);
            return node;
        }

        public new void LoadXml(XmlNode node)
        {
            //extend the order item as special requestitem by zhouys 3/30 2012
            //dpt = XmlUtil.GetSubNodeText(node, "@i");
            (this as RequestItem).LoadXml(node);
            desc = XmlUtil.GetSubNodeText(node, "@d");
        }
    }
}
