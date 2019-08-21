using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;


namespace WolfInv.Com.XPlatformCtrlLib
{

    public partial class XWebForm_Panel : Panel, IXPanel
    {
        public PlatformControlType ControlType { get { return PlatformControlType.Web; } }
        public XWebForm_Panel()
        {
                     

        }
        public IXControl CurrMainControl { get; set; }
        public void Controls_Add(IXControl ctrl)
        {
            this.Controls.Add(ctrl as Control);
        }

        public void Controls_Clear()
        {
            this.Controls.Clear();
        }

        public void SetDock(XPlatformDockStyle dock)
        {
            if(dock== XPlatformDockStyle.Fill)
            {
                this.Width = new Unit(100, UnitType.Percentage);
                this.Height = new Unit(100, UnitType.Percentage);
            }
            //Dock = (DockStyle)dock;
        }
    }


}