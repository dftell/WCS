using WolfInv.Com.MetaDataCenter;
using System.Xml;
using XmlProcess;

namespace WolfInv.Com.CommCtrlLib
{
    public class CellViewItem : ViewItem
    {
        public bool multiline;
        public int height;
        public override void LoadXml(XmlNode node)
        {
            base.LoadXml(node);
            this.dpt = new DataPoint(XmlUtil.GetSubNodeText(node, "@f"));
        }
    }

  
}
