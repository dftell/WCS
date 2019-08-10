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
    
    public partial class XWebForm_UserControl : System.Web.UI.UserControl, IXUserControl
    {
        public XWebForm_UserControl()
        {
            //this.UserControl_MainPanel = new Panel(); 
        }

        

        public void ReLoad(object send,EventArgs e)
        {
            OnLoad(e);
        }

        public void Page_Load(object sender, EventArgs e)
        {
            
        }
        public PlatformControlType ControlType { get { return PlatformControlType.Web; } }



        public void Controls_Add(IXControl ctrl)
        {
            Control c = ctrl as Control;
            this.UserControl_MainPanel.Controls.Add(c);
        }

        public override ControlCollection Controls
        {
            get
            {
                return this.UserControl_MainPanel.Controls;
            }
        }

        public void Controls_Clear()
        {
            Controls.Clear();
        }

        public void SetDock(XPlatformDockStyle dock)
        {
            //Dock = (DockStyle)dock;
        }

        public void ToTopLevel()
        {
            //BringToFront();

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }
    }


}