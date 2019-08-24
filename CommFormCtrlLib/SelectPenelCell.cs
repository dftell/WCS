using System.Collections.Generic;
using WolfInv.Com.MetaDataCenter;
using System.Xml;
using XmlProcess;
using WolfInv.Com.CommCtrlLib;
using System.Windows.Forms;
using System;
using WolfInv.Com.XPlatformCtrlLib;

namespace WolfInv.Com.CommFormCtrlLib
{
    public class SelectPenelCell : PanelCell
    {
        public Dictionary<string, string> Mappings = new Dictionary<string, string>();
        public CMenuItem NewItemMnu;
        public CMenuItem SelectItemMnu;
        public SelectPenelCell(string uid)
            : base(uid)
        {
        }
        public SelectPenelCell(XmlNode node, string uid)
            : base(node, uid)
        {
        }

        public override void LoadXml(XmlNode node)
        {
            base.LoadXml(node);
            XmlNodeList maps = node.SelectNodes("./Mappings/Map");
            foreach (XmlNode mnode in maps)
            {
                string toField = XmlUtil.GetSubNodeText(mnode, "@i");
                string FromField = XmlUtil.GetSubNodeText(mnode, "@v");
                if (!Mappings.ContainsKey(toField))
                {
                    Mappings.Add(toField, FromField);
                }
            }
            XmlNode mnunode = node.SelectSingleNode("./menu[@id='NewItem']");
            if (mnunode != null)
            {
                NewItemMnu = MenuProcess.GetMenu(NewItemMnu, mnunode, strUid);
            }
            mnunode = node.SelectSingleNode("./menu[@id='SelectItem']");
            if (mnunode != null)
            {
                SelectItemMnu = MenuProcess.GetMenu(NewItemMnu, mnunode, strUid);
            }

        }

        public override string ChangeValue(string val, string text)
        {
            SelectItemControl sic = this.ItemControl as SelectItemControl;
            //sic.Value = val;
            sic.ChangeValue(val, text);
            return val;
        }

        public override string GetValue(bool IsText)
        {
            SelectItemControl sic = this.ItemControl as SelectItemControl;
            return sic.GetValue(IsText);
        }

        protected override Control GetControl()
        {
            SelectItemControl sic = new SelectItemControl(this.Value, this.Text);
            sic.Width = this.Width;
            sic.NewItem += new NewItemEventHandler(sic_NewItem);
            sic.SelectItem += new SelectEventHandler(sic_SelectItem);
            return sic;
        }

        void sic_NewItem(object send, EventArgs e)
        {
            MessageBox.Show("lkdjlkjd");
            return;

        }

        void sic_SelectItem(object send, EventArgs e)
        {
            SelectItemControl sic = send as SelectItemControl;
            UpdateData ret = null;

            if (sic == null) return;
            if (this.SelectItemMnu == null)
            {
                MessageBox.Show("未设定选择事件！");
                return;
            }
            if (!FrameSwitch.ShowDialoger(null, this.OwnRow.OwnPanel.OwnerForm, this.SelectItemMnu, ref ret))
            {
                return;
            }
            if (ret == null)
            {
                MessageBox.Show("选择的值为空！");
                return;
            }
            Dictionary<string, PanelCell> pcs = this.OwnRow.OwnPanel.ControlList;
            if (pcs == null) return;
            foreach (string tostr in this.Mappings.Keys)
            {
                if (!pcs.ContainsKey(tostr))//需要传递值的控件不存在，跳过
                {
                    continue;
                }
                PanelCell pc = pcs[tostr];
                string fromstr = this.Mappings[tostr];
                if (!ret.Items.ContainsKey(fromstr))
                {
                    pc.ChangeValue(fromstr, "");//如果返回的数据中不存在该数据点，认定赋一个固定值给控件
                }
                else
                {
                    if(ret.Items[fromstr].text == null)
                    {
                        ret.Items[fromstr].text = ret.Items[fromstr].value;
                    }
                    pc.ChangeValue(ret.Items[fromstr].value, ret.Items[fromstr].text);//传递返回的值
                }
            }
            if (pcs.ContainsKey(this.DisplayField) && ret.Items.ContainsKey(this.DisplayField))
            {
                pcs[this.DisplayField].ChangeValue(ret.Items[this.DisplayField].value, ret.Items[this.DisplayField].value);
                sic.Text = ret.Items[this.DisplayField].value;
            }
            //this.OwnRow.OwnPanel.CoverData(ret);
            return;

        }
    }


}

