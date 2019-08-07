using System.Xml;
using XmlProcess;
namespace WolfInv.Com.WCS_Process
{
    public class SystemKeyItem
    {
        public string type;
        public string datapoint;
        public string text;

        public void LoadXml(XmlNode node)
        {
            this.type = XmlUtil.GetSubNodeText(node, "@id");
            this.datapoint = XmlUtil.GetSubNodeText(node, "@f");
            this.text = XmlUtil.GetSubNodeText(node, "@text");
        }
    }
}
