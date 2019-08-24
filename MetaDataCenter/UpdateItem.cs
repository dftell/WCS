using System.Xml;
using XmlProcess;
namespace WolfInv.Com.MetaDataCenter
{
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
            XmlUtil.AddAttribute(node, "t", text);
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
            text = XmlUtil.GetSubNodeText(node, "@t");
            datapoint.Text = value;

        }
    }
}
