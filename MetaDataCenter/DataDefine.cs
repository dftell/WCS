using System;
using System.Collections.Generic;
using System.Xml;
using XmlProcess;
namespace WolfInv.Com.MetaDataCenter
{
    public interface ICalcExpreable
    {
        /// <summary>
        /// 可计算列
        /// </summary>
        string Method { get; set; }
        string CalcExpr { get; set; }
    }
    public interface IDataSourceable
    {
        string DataSourceName { get; set; }
        string ValueField { get; set; }
        string TextField { get; set; }
        /// <summary>
        /// combo 复杂组合多项分割字符串
        /// </summary>
        string ComboItemsSplitString { get; set; }
    }

    public enum DataRequestType
    {
        Query,
        Add,
        Update,
        Delete,
        Import,
        BatchUpdate,
        RemoveItem
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
        public UpdateData Data { get; set; }
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
        
        public DataRowChoiceItem()
        {
            Data = new UpdateData();
        }
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
