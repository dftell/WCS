using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using WolfInv.Com.WCS_Process;
namespace WolfInv.Com.CommWebCtrlLib
{
    public class WebCommFunction
    {
        public static void SwitchView(Page page,string url,string Target)
        {
        string sHtml = @"
            <a name=""test"" href=""{0}"" target=""{1}"" style=""display:none"">ldkjsljfl</a>
<script>
test.click();
</script>             
            ";
        page.Response.Write(string.Format(sHtml, url, Target));
        }
        
    }

    public class MessageBox
    {
        static string Title;
        static MessageBox()
        {
            Title = GlobalShare.SystemAppInfo.Title;
        }

        public static void Alert(Page page, string msg)
        {
            string sJavascript = @"
            <script language='vbscript'>Msgbox ""{0}"",,""{1}""</script>
            ";
            page.Response.Write(string.Format(sJavascript, msg, Title));
        }

        public static void Alert(Page page,string title, string msg)
        {
            string sJavascript = @"
            <script language='vbscript'>Msgbox ""{0}"",,""{1}""</script>
            ";
            page.Response.Write(string.Format(sJavascript, msg, title));
        }
    
        
    }
}
