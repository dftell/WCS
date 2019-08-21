using System;
using System.ComponentModel;
using System.Windows.Forms;
using Xilium.CefGlue.WindowsForms;
namespace WolfInv.Com.CommCtrlLib
{
    public class WebForm : SubForm
    {
        public string id;
        public CefWebBrowser wb;
        string _url;
        public WebForm(string strid, string url)
        {
            id = strid;
            _url = url;
            Init();
            //if (url.ToLower().StartsWith("http"))
            //{
            wb.StartUrl = url;
            //}
            //else
            //{
            //    wb.Url = new Uri(string.Format("http://oa.cfzq.com{0}", url));
        //}

            //this.MdiParent = FrameSwitch.ParentForm;
        }

        public void Init()
        {
            Width = 1024;
            Height = 600;

            StartPosition = FormStartPosition.CenterParent;
            if (wb == null)
            {
                wb = new CefWebBrowser();
            }
            //wb.NewWindow += new CancelEventHandler(wb_NewWindow);
            //wb.Navigating += new WebBrowserNavigatingEventHandler(wb_Navigating);
            wb.Dock = DockStyle.Fill;
            Controls.Add(wb);
            WindowState = FormWindowState.Maximized;
            this.FormClosing += new FormClosingEventHandler(WebForm_FormClosing);

        }

        void wb_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {

            e.Cancel = true;
            if (e.Url.ToString() == _url)
                return;
            WebForm frm = null;
            if (!FrameSwitch.ShowWebForms.TryGetValue((sender as WebBrowser).StatusText, out frm))
            {
                frm = new WebForm(e.Url.ToString(), e.Url.ToString());
            }
            //frm.MdiParent = FrameSwitch.ParentForm;
            frm.Focus();
            frm.Show();
        }

        void WebForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (FrameSwitch.ShowWebForms.ContainsKey(this.id))
            {
                FrameSwitch.ShowWebForms.Remove(this.id);
            }
        }

        void wb_NewWindow(object sender, CancelEventArgs e)
        {
            e.Cancel = true;

            WebForm frm = null;
            if (!FrameSwitch.ShowWebForms.TryGetValue((sender as WebBrowser).StatusText, out frm))
            {

                // frm = new WebForm((sender as WebBrowser).StatusText,  (sender as WebBrowser).StatusText);
                //frm = new WebForm(this.wb.StatusText.ToString(), this.wb.StatusText.ToString());
            }
            //frm.MdiParent = FrameSwitch.ParentForm;
            frm.Focus();
            frm.Show();
        }
    }


}