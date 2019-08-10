using System;
using System.Web.UI;
using WolfInv.Com.CommCtrlLib;
using System.Xml;
using WolfInv.Com.XPlatformCtrlLib;

namespace WolfInv.Com.CommWebCtrlLib
{
    public class ToolBarItemBuilderForWeb : ToolBarItemBuilder
    {
        ToolBarStrip toolStrip1;
        WebPageFormHandle frmmdl;
        public ToolBarItemBuilderForWeb(IFrame frmobj, ITag otoolbar)
            : base(frmobj, otoolbar)
        {
            frmmdl = frmobj as WebPageFormHandle;
            toolStrip1 = otoolbar as ToolBarStrip;
        }

        public override void InitToolBar(bool RightToLeft)
        {
            toolStrip1.Items.Clear();
            if (RightToLeft)
            {
                this.toolStrip1.Style.Add(HtmlTextWriterStyle.TextAlign,"right");
            }

        }

        public override ITag AddToolBarItem(CMenuItem mnu, ToolBarItemType type, EventHandler del)
        {
            ToolBarStripItem ret = null;
            switch (type)
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

            ToolBarStripItem ret = null;
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
                case ToolBarItemType.Button:
                default :    
                {
                        ToolBarStripButton searchbtn = new ToolBarStripButton();
                        //(frmmdl.DataFrm as Page).deAcceptButton = searchbtn as IButtonControl;
                        searchbtn.Text = lbl;
                        //this.toolStrip1.Items.Add(searchbtn);
                        searchbtn.Click += del;
                        ret = searchbtn;
                        break;
                    }
            }
            if(ret != null)
                this.toolStrip1.Items.Add(ret);

            return ret;
        }

        public override ITag AddToolBarItem(XmlNode xml, ToolBarItemType type, string itemid, params EventHandler[] del)
        {
            return base.AddToolBarItem(xml, type, itemid, del);
        }
     
    }
}
