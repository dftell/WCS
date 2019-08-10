//using Microsoft.Toolkit.Forms.UI.Controls;
using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WolfInv.Com.CommCtrlLib;
using WolfInv.Com.CommFormCtrlLib;
using WolfInv.Com.WCS_Process;
using WebKit;
using WebKit.JSCore;


using WolfInv.Com.XPlatformCtrlLib;
using System.Net;
//using Microsoft.Toolkit.Win32.UI.Controls.WinForms;
namespace WCS
{
    public partial class Form1 : Form
    {
        Task<string> webViewRet;
        
        public Form1()
        {
            
            InitializeComponent();
            //Form1_Load(null,null);
            
            
            //webView1 as 
            ////webView1
            //webView1.DocumentCompleted += WebView1_DocumentCompleted; ;
            
            ////webView1.DOMContentLoaded += WebView1_DOMContentLoaded;
            //this.Controls.Add(webView1);
        }

        private void WebView1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }



        private void btn_out_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_enter_Click(object sender, EventArgs e)
        {
            
            
            
            CITMSUser user = new CITMSUser();
            Cursor = Cursors.WaitCursor;
            bool loginwithpwd = true;
            if (GlobalShare.SystemAppInfo.LoginUrl != null && GlobalShare.SystemAppInfo.LoginUrl.Length > 0)
            {
                loginwithpwd = false;
            }
            if (loginwithpwd)//非web登录
            {
                string err = user.Login(this.txt_user.Text, this.txt_pwd.Text, loginwithpwd);
                Cursor = Cursors.Default;
                if (err != null)
                {
                    MessageBox.Show(err);
                    return;
                }
            }
            else
            {
                string strurl = GlobalShare.SystemAppInfo.LoginUrl;
                string reurl = GetRedirectUrl(strurl);
                //this.webKitBrowser1.Navigate(GlobalShare.SystemAppInfo.LoginUrl);
                webKitBrowser1.AllowNavigation = true;
                webKitBrowser1.Url = new Uri(strurl);
                this.webKitBrowser1.Visible = true;
                this.webKitBrowser1.BringToFront();
                Application.DoEvents();
                return;
            }
            Form frm = new MDI_Main(user.LoginName);
            FrameSwitch.ParentForm = frm;
            if (!loginwithpwd)
            {
                IXPanel container = (frm as MDI_Main).Main_Plan;
                (frm as MDI_Main).splitContainer1.SplitterDistance = 0;
                string url = user.OtherLogin(GlobalShare.SystemAppInfo.LoginUrl,this.txt_user.Text, this.txt_pwd.Text, false);
                ////WebForm wfrm = new WebForm("oa1", url);
                ////wfrm.wb.Refresh();
                CMenuItem mnu = new CMenuItem("Main");
                mnu.linkType = LinkType.WebPage;
                mnu.LinkValue = url;
                //FrameSwitch.ShowWebForms.Add("oa1", wfrm);
                FrameSwitch.switchToView(container, null, mnu);
            }

            //string mailurl = string.Format("http://mail.cfzq.com/lks/mail/{0}.nsf", this.txt_user.Text.Trim());
            //WebForm wfrmmail = new WebForm("mailbox_bussiness",  url);
            
            this.Hide();
            //Form frm = new frm_Main();
            //wfrm.WindowState = FormWindowState.Maximized;
            frm.ShowDialog(this);
           
            //wfrm.Show();
            
            this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Lbl_SystemName.Text = GlobalShare.SystemAppInfo.Title;
            FrameSwitch.SystemText = GlobalShare.SystemAppInfo.Title;
            this.Text = GlobalShare.SystemAppInfo.FormText;
            string iconpath = string.Format("{0}\\{1}", GlobalShare.AppPath, GlobalShare.SystemAppInfo.IconPath);
            if (File.Exists(iconpath))
            {
                Icon icon = new Icon(iconpath);
                FrameSwitch.SystemIcon = icon;
                
            }
            else
            {
                FrameSwitch.SystemIcon = this.Icon;
            }
            this.Icon = FrameSwitch.SystemIcon;
            bool debug =false;
            debug = true;
            if (debug)
            {
                ////this.txt_pwd.Text = "abcdef";
                ////this.txt_user.Text = "zhouys";
                ////this.btn_enter_Click(null, null);
            }
            if(GlobalShare.SystemAppInfo.LoginUrl!=null)
            {
                this.Cursor = Cursors.WaitCursor;
                //webKitBrowser1.Navigate(GlobalShare.SystemAppInfo.LoginUrl);
                //webKitBrowser1.Url = new Uri(GlobalShare.SystemAppInfo.LoginUrl);
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //
            GlobalShare.DataCenterClientObj.DisConnect();
            //GlobalShare = null;
            //GlobalShare.appinfos
        }

        

        

        string WebLogin()
        {
            AddScript("login", "login");
            return InvokeScriptFunc("Self_Login", this.txt_user.Text, this.txt_pwd.Text);
        }

        void AddScript(string strModule, string strFileName)
        {

            string scriptEl = this.getScriptText(strModule, string.Format("{0}.js", strFileName));

            //webView1.cre (scriptEl);
            //webView1.StringByEvaluatingJavaScriptFromString(scriptEl);
        }

        [System.Runtime.InteropServices.ComVisibleAttribute(true)]
        class myClass
        {
            private WebKitBrowser webKitBrowser;
            public myClass(WebKitBrowser webkit)
            {
                this.webKitBrowser = webkit;
            }
            public void Say(string msg)
            {
                webKitBrowser.Navigate(msg);
            }
        }
        //ScriptLoaded = true;
    
        
        string getScriptText(string module,string scriptName)
        {
            string ret = "";
            string FilePath = string.Format("{0}\\{1}\\{2}", Application.StartupPath, module,scriptName);
            try
            {
                StreamReader fs = new StreamReader(FilePath, Encoding.Default);
                ret = fs.ReadToEnd();
                fs.Close();
            }
            catch (Exception e)
            {
                //throw e;
            }
            return ret;
        }

        string InvokeScriptFunc(string FuncName,params string[] objs)
        {
            //string ret = webView1.StringByEvaluatingJavaScriptFromString(string.Format("SendMsg(\"{0}\",\"{1}\")", objs));
            return null; ;
        }

        private void webKitBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if(e!= null)
            {
                WebKitBrowser wb = (sender as WebKitBrowser);
            }
        }

        private void webKitBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            string url = e.Url.OriginalString;
            if(e.Cancel == true)
            {
                return;
            }
        }

        private void webKitBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            string url = e.Url.OriginalString;
        }

        /// <summary>
        /// 获取页面重定向url
        /// </summary>
        /// <param name="url"></param>
        /// <param name="referer"></param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public static string GetRedirectUrl(string url, string referer = "", string cookie = "")
        {
            string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
        
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "HEAD";
                req.ProtocolVersion = HttpVersion.Version10;
                //req.ContentType = "application/json";// "application/x-www-form-urlencoded";
                req.UserAgent = DefaultUserAgent;
                req.Referer = referer;
                req.AllowAutoRedirect = false;
                if (cookie.Length > 0)
                {
                    req.Headers.Add("Cookie:" + cookie);
                }
                WebResponse response = req.GetResponse();
                return response.Headers["Location"];
            }
            catch (Exception e)
            {
                //TextTool.Log(e, "获取url重定向地址错误");
                return null;
            }
        }
    }
}