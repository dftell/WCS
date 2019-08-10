using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WolfInv.Com.XPlatformCtrlLib;
namespace WolfInv.Com.XPlatformCtrlLib
{
    public partial class WebUserControl1 : System.Web.UI.UserControl,IXControl
    {
        public PlatformControlType ControlType
        {
            get
            {
                return PlatformControlType.Web;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}