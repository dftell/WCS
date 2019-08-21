using System;
using System.Windows.Forms;
using System.Xml;
using WolfInv.Com.CommCtrlLib;
using WolfInv.Com.XPlatformCtrlLib;
namespace WolfInv.Com.CommFormCtrlLib
{
    public class ToolBarBuilderItemForWin : ToolBarItemBuilder 
    {
        ToolBarStrip toolStrip1;
        frm_Model frmmdl;
        public ToolBarBuilderItemForWin(IFrame frm,ITag toolbar):base(frm,toolbar)
        {
            toolStrip1 = toolbar as ToolBarStrip;
            frmmdl = base.frmObj as frm_Model;
        }

        public override void InitToolBar(bool LeftToRight)
        {
            toolStrip1.Items.Clear();
            if (LeftToRight)
            {
                this.toolStrip1.RightToLeft = RightToLeft.Yes;
            }
            
        }

        public override ITag AddToolBarItem(CMenuItem mnu, ToolBarItemType type, EventHandler del)
        {
            ToolBarStripItem ret = null;
            switch(type)
            {
                case ToolBarItemType.Button:
                default:
                    {
                        ToolBarStripButton tsi = new ToolBarStripButton();
                        tsi.Text = mnu.MnuName;
                        tsi.Tag = mnu;
                        tsi.Click += del;
                        ret = tsi;
                        break;
                    }
            }
            this.toolStrip1.Items.Add(ret);
            return ret;
        }
        public override ITag AddToolBarItem(string lbl, ToolBarItemType type, EventHandler del)
        {
            return AddToolBarItem("", null, lbl, type, del);
        }
        public override ITag AddToolBarItem(string id,XmlNode backxml, string lbl, ToolBarItemType type, EventHandler del)
        {

            ToolBarStripItem ret = null;
            ret.Name = id;
            switch (type)
            {
                case ToolBarItemType.Separator:
                    {
                        
                        if (this.toolStrip1.Items.Count > 0)
                        {
                            ret = new ToolBarStripSeparator();
                        }
                        
                        break;
                    }
                case ToolBarItemType.Label:
                    {
                        this.toolStrip1.Items.Add(ret = new ToolBarStripLabel());
                        
                        ret.Text = lbl;
                        break;
                    }
                case ToolBarItemType.Button:
                    {
                        ToolBarStripButton searchbtn = new ToolBarStripButton();
                        (frmmdl.TopLevelControl as Form).AcceptButton = searchbtn as IButtonControl;
                        searchbtn.Text = lbl;
                        //this.toolStrip1.Items.Add(searchbtn);
                        searchbtn.Click += del;
                        ret = searchbtn;
                        break;
                    }
                case ToolBarItemType.Combox:
                    {
                        ToolBarStripComobox combo = new ToolBarStripComobox();
                        combo.Text = lbl;
                        //this.toolStrip1.Items.Add(combo);
                        ret = combo;
                        break;
                    }
                case ToolBarItemType.TextBox:
                    {
                        ToolStripTextBoxD ssearchbox = new ToolStripTextBoxD();
                        ret = ssearchbox;
                        break;
                    }
            }
            this.toolStrip1.Items.Add(ret);
 
            return ret;
        }

        public override ITag AddToolBarItem(XmlNode xml, ToolBarItemType type, string itemid, params EventHandler[] del)
        {
            string lbl = xml.SelectSingleNode(itemid).Value;
            ITag ret = AddToolBarItem(lbl, type, del[0]);
            return ret;
        }

    }
}
