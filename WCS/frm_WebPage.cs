using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WolfInv.Com.CommFormCtrlLib;
using WolfInv.Com.CommCtrlLib;
using Xilium.CefGlue.WindowsForms;
using Xilium.CefGlue;
using WolfInv.Com.WCS_Process;
namespace WCS
{
    public partial class frm_WebPage :frm_Model
    {
        string logurl;
        public frm_WebPage()
        {
            InitializeComponent();
        }

        private void frm_WebPage_Load(object sender, EventArgs e)
        {

            webview.Visible = false;
            
        }

        private void Webview_AddressChanged(object sender, AddressChangedEventArgs e)
        {
            BeginInvoke(new Action(() => {
                //MessageBox.Show(e.Address);
            }));
        }

        private void Webview_StatusMessage(object sender, Xilium.CefGlue.WindowsForms.StatusMessageEventArgs e)
        {
            BeginInvoke(new Action(() => {
                //MessageBox.Show(e.Value);
            }));
        }

        private void Webview_LoadError(object sender, Xilium.CefGlue.WindowsForms.LoadErrorEventArgs e)
        {
            BeginInvoke(new Action(() => {
                //MessageBox.Show(string.Format("url:{0};code:{1};text:{2}",e.FailedUrl,e.ErrorCode,e.ErrorText));
            }));
        }

        private void Webview_LoadEnd(object sender, Xilium.CefGlue.WindowsForms.LoadEndEventArgs e)
        {
            if(e.Frame.Url == logurl)//for log
            {
                GlobalShare.Logined = true;
                e.Frame.LoadUrl(FromMenu.LinkUrl);
                
                BeginInvoke(new Action(()=>{

                    //e.Frame.LoadUrl(FromMenu.LinkUrl);
                }));
                
            }
            else
            {
                webview.Visible = true;
            }
        }

        private void frm_WebPage_DockChanged(object sender, EventArgs e)
        {
            if(!GlobalShare.Logined)//如果未登陆，先登陆
            {
                CITMSUser user = GlobalShare.UserAppInfos.Values.First().CurrUser;
                string url = string.Format(GlobalShare.SystemAppInfo.LoginUrl, user.LoginName, user.Password);
                webview.StartUrl = url;

                logurl = url;
                //webview.Browser.GetMainFrame().LoadUrl(FromMenu.LinkUrl);
                webview.LoadError += Webview_LoadError;
                webview.AddressChanged += Webview_AddressChanged;
                webview.LoadEnd += Webview_LoadEnd;
            }
            else
            {
                webview.StartUrl = FromMenu.LinkUrl;
                //webview.Browser.GetMainFrame().LoadUrl(FromMenu.LinkUrl);
                webview.LoadError += Webview_LoadError;
                webview.AddressChanged += Webview_AddressChanged;
                webview.LoadEnd += Webview_LoadEnd;
            }
            //string url = 
            
        }
    }
}
