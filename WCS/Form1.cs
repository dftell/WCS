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
using WebKit.Interop;
using WebKit;
using WolfInv.Com.XPlatformCtrlLib;
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
            WebKitBrowser wv = sender as WebKitBrowser;
            if (e.Url == null)
                return;
            this.Cursor = Cursors.Default;
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
                //webView1.Navigate(GlobalShare.SystemAppInfo.LoginUrl);
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
                //webView1.Navigate(GlobalShare.SystemAppInfo.LoginUrl);
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

    }
}