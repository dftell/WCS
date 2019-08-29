using WolfInv.Com.MetaDataCenter;
using System.Xml;
using XmlProcess;
using WolfInv.Com.WCS_Process;
namespace WolfInv.Com.CommCtrlLib
{

    public class ViewItem : DataControlItem
    {
        public DataPoint dpt;
        public bool Visable = true;
        public bool IsKeyValue;
        public bool IsKeyText;
        public int Index;
        public bool Sum;
        public bool Perm = true;
       
        /// <param name="node"></param>

        public override void LoadXml(XmlNode node)
        {
            base.LoadXml(node);
            this.Name = XmlUtil.GetSubNodeText(node, "@i");
            this.Text = XmlUtil.GetSubNodeText(node, "@text");
            if (this.Text == "")
            {
                this.Text = XmlUtil.GetSubNodeText(node, "@udlbl");
            }
            //int.TryParse(XmlUtil.GetSubNodeText(node, "@width"), out this.Width);
            this.DataType = XmlUtil.GetSubNodeText(node, "@type");
            this.ComboName = XmlUtil.GetSubNodeText(node, "@combo");
            this.Visable = !(XmlUtil.GetSubNodeText(node, "@hide") == "1");
            this.IsKeyText = XmlUtil.GetSubNodeText(node, "@keytext") == "1";
            this.IsKeyValue = XmlUtil.GetSubNodeText(node, "@keyvalue") == "1";
            ValueField = XmlUtil.GetSubNodeText(node, "@valmember");
            TextField = XmlUtil.GetSubNodeText(node, "@txtmember");
            ComboItemsSplitString = XmlUtil.GetSubNodeText(node, "@membersplitor");
            this.Sum = XmlUtil.GetSubNodeText(node, "@sum") == "1";
            string strIdx = XmlUtil.GetSubNodeText(node, "@index");
            Perm = !(XmlUtil.GetSubNodeText(node, "@perm") == "0");
            if (strIdx == "")
            {
                //this.Index = int.Parse(node.SelectSingleNode("position(.)").Value);
            }
            else
            {
                int.TryParse(strIdx, out this.Index);
            }
        }

        public void GetItem(DataPoint dp)
        {
            dpt = dp;
            this.Name = dp.Name;
            this.Text = dp.Text;
            this.Width = dp.Width;
            this.ComboName = dp.ComboName;
            this.DataType = dp.DataType;
        }


        

    }


}
