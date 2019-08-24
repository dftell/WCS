using System;
using System.Collections.Generic;
using System.Text;
using XmlProcess;
using System.Xml;
namespace WolfInv.Com.MetaDataCenter
{
    public class DataMappings
    {
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
