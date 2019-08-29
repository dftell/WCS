using System;
using System.Collections.Generic;
using System.Text;
using XmlProcess;
using System.Xml;
namespace WolfInv.Com.MetaDataCenter
{
    public class DataTranMappings : List<DataTranMapping>
    {
        public DataTranMappings()
        {

        }

        Dictionary<string, DataTranMapping> t_AllFrom;
        public Dictionary<string, DataTranMapping> AllFrom
        {
            
            get
            {
                if (t_AllFrom != null)
                    return t_AllFrom;
                t_AllFrom = new Dictionary<string, DataTranMapping>();
                this.ForEach(a => {
                    if(!t_AllFrom.ContainsKey(a.FromDataPoint.Name))
                        t_AllFrom.Add(a.FromDataPoint.Name, a);
                });
                return t_AllFrom;
            }
        }

        Dictionary<string, DataTranMapping> t_AllTo;
        public Dictionary<string,DataTranMapping> AllTo
        {
            get
            {
                if (t_AllTo != null)
                    return t_AllTo;
                t_AllTo = new Dictionary<string, DataTranMapping>();
                this.ForEach(a => {
                    if (!t_AllTo.ContainsKey(a.ToDataPoint))
                        t_AllTo.Add(a.ToDataPoint, a);
                });
                return t_AllTo;
            }
        }

        public void LoadXml(XmlNode xmlnode)
        {
            this.Clear();
            XmlNodeList nodes = xmlnode.SelectNodes("Maps/Map");
            foreach(XmlNode node in nodes)
            {
                DataTranMapping dt = new DataTranMapping();
                dt.LoadXml(node);
                this.Add(dt);
            }
        }
    }

    public class DataTranMapping
    {
        public DataPoint FromDataPoint;
        public string ToDataPoint;

        public void LoadXml(XmlNode node)
        {
            FromDataPoint = new DataPoint(XmlUtil.GetSubNodeText(node, "@from"));
            FromDataPoint.Text = XmlUtil.GetSubNodeText(node, "@fromvalue");
            ToDataPoint = XmlUtil.GetSubNodeText(node, "@to");
        }

        public XmlNode ToXml(XmlNode parent)
        {
            XmlDocument xmldoc;
            if (parent == null)
            {
                xmldoc = new XmlDocument();
                xmldoc.LoadXml("<root/>");
                parent = xmldoc.SelectSingleNode("root");
            }

            XmlNode node = XmlUtil.AddSubNode(parent, "Map");
            XmlUtil.AddAttribute(node, "from", this.FromDataPoint.Name);
            XmlUtil.AddAttribute(node, "to", this.ToDataPoint);
            return node;
        }
    }
}
