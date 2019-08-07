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
        public string FromDataPoint;
        public string ToDataPoint;

        public void LoadXml(XmlNode node)
        {
            FromDataPoint = XmlUtil.GetSubNodeText(node, "@from");
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
            XmlUtil.AddAttribute(node, "from", this.FromDataPoint);
            XmlUtil.AddAttribute(node, "to", this.ToDataPoint);
            return node;
        }
    }
}
