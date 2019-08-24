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
using WolfInv.Com.JsLib;
using System.Linq;


using WolfInv.Com.XPlatformCtrlLib;
using System.Net;
using Xilium.CefGlue;
using Newtonsoft.Json.Linq;
using Xilium.CefGlue.WindowsForms;
using WolfInv.Com.AccessWebLib;
using System.Text.RegularExpressions;
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

        


        private void btn_out_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_enter_Click(object sender, EventArgs e)
        {
            this.btn_enter.Enabled = false;
            
            
            CITMSUser user = new CITMSUser();
            Cursor = Cursors.WaitCursor;
            bool loginwithpwd = true;
            user.LoginName = this.txt_user.Text.Trim();
            user.Password = this.txt_pwd.Text.Trim();
            if (GlobalShare.SystemAppInfo.LoginUrl != null && GlobalShare.SystemAppInfo.LoginUrl.Length > 0)
            {
                loginwithpwd = false;
                
            }
            string err = user.Login(this.txt_user.Text, this.txt_pwd.Text, loginwithpwd);
            if (loginwithpwd)//非web登录
            {
                
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
                //string reurl = GetRedirectUrl(strurl);
                string url = string.Format(strurl, txt_user.Text, txt_pwd.Text);
                //CefWebBrowser webctrl11 = new CefWebBrowser();
                
                
                string html = AccessWebServerClass.GetData(url);
                if(html == null)
                {
                    this.btn_enter.Enabled = true;
                    MessageBox.Show(string.Format("服务器返回异常值！{0}", "返回对象为空。"));
                    return;
                }
                Regex reg = new Regex(@"\((.*?)\)");
                string strjson = null;
                if (!reg.IsMatch(html))
                {
                    this.btn_enter.Enabled = true;
                    MessageBox.Show(string.Format("服务器返回异常值！{0}",html));
                    return;
                }
                MatchCollection m = reg.Matches(html);
                if (m.Count > 0)
                {
                    strjson = m[0].Value;
                }
                strjson = strjson.Substring(1, strjson.Length - 2);
                JObject retOjb = XML_JSON.ToJsonObj(strjson);
                if(retOjb == null || retOjb["msg"] == null || retOjb["msg"].Value<string>()!="OK")
                {
                    this.btn_enter.Enabled = true;
                    MessageBox.Show(string.Format("密码错误:{0}",retOjb["msg"].Value<string>()));
                    return;
                }
                user.UserId = retOjb["data"]["userId"].Value<int>();
                
                Application.DoEvents();
                //System.Threading.Thread.Sleep(3 * 1000);
                
                //GlobalShare.UserAppInfos = new System.Collections.Generic.Dictionary<string, UserGlobalShare>();
                //GlobalShare.UserAppInfos.Add(user.LoginName, new UserGlobalShare(user.LoginName));
                //GlobalShare.UserAppInfos.Values.First().CurrUser = user;
                this.Hide();
                //Form frm = new frm_Main();
                //wfrm.WindowState = FormWindowState.Maximized;
                Form frm1 = null;
                if (GlobalShare.SystemAppInfo.NoNeedLeftTreeView)
                    frm1 = new frm_Main(user.LoginName);
                else
                    frm1 = new MDI_Main(this.txt_user.Text);
                frm1.ShowDialog(this);

                //wfrm.Show();

                this.Close();
                return;
            }
            MDI_Main frm = new MDI_Main(this.txt_user.Text);
            FrameSwitch.ParentForm = frm;
            if (!loginwithpwd)
            {
                
                //GlobalShare.DefaultUserInfo;
                
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

      

        private sealed class SourceVisitor : CefStringVisitor
        {
            private readonly Action<string> _callback;

            public SourceVisitor(Action<string> callback)
            {
                _callback = callback;
            }

            protected override void Visit(string value)
            {
                _callback(value);
            }

            public string ReadText()
            {
                string res = null;
                Visit(res);
                return res;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Lbl_SystemName.Text = GlobalShare.SystemAppInfo.FormText;
            FrameSwitch.SystemText = GlobalShare.SystemAppInfo.Title;
            this.Text = GlobalShare.SystemAppInfo.Title;
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
                //this.Cursor = Cursors.WaitCursor;
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