using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Xml;
using XmlProcess;
using WolfInv.Com.WCS_Process;
using WolfInv.Com.MetaDataCenter;
using System.Collections;
using System.IO;
using System.Net;
using System.Web;
using Microsoft;
using System.Reflection;
using WolfInv.Com.CommCtrlLib;
using WolfInv.Com.XPlatformCtrlLib;

namespace WolfInv.Com.CommWebCtrlLib
{
    

    public class WebPageSwitcher : IFrameActionSwitch
    {
        public WebPageModel CurrPage;
        public WebPageModel LinkPage;
        static Assembly AppDllasmb;
        WebPageFormHandle FromWebForm;
        WebPageFormHandle CurrWebForm;
        public static Dictionary<string, ExtraRequest> ExtraReqMappings;

        void GetPageObject(BaseFormHandle FromLink,BaseFormHandle CurrForm)
        {
            FromWebForm = FromLink as WebPageFormHandle;
            CurrWebForm = CurrForm as WebPageFormHandle;
            CurrPage = FromWebForm.DataFrm as WebPageModel;
            LinkPage = CurrWebForm.DataFrm as WebPageModel;
        }
        static WebPageSwitcher()
        {
            ExtraReqMappings = new Dictionary<string, ExtraRequest>();
            try
            {
                AppDllasmb = GlobalShare.MainAssem;// Assembly.LoadFrom(string.Format("{0}\\bin\\ITMS_APP.dll", GlobalShare.AppDllPath));
            }
            catch (Exception ce)
            {
                try
                {
                    AppDllasmb = GlobalShare.MainAssem; ;// Assembly.LoadFile(string.Format("{0}\\bin\\ITMS_APP.dll", GlobalShare.AppDllPath));
                }
                catch(Exception se)
                {
                    throw se;
                }
                
            }
        }

        public WebPageSwitcher()
        {
        }


        public override bool CreateFrame(IFrameObj Container, BaseFormHandle CurrFormHandle, CMenuItem mnu, ref UpdateData data, ref string msg)
        {
            //throw new Exception("The method or operation is not implemented.");
            return true;
        }

        public override bool CreateWebPage(IFrameObj Container, BaseFormHandle CurrFormHandle, CMenuItem mnu, ref string msg)
        {
            if (Container == null)
            {
                msg = "无法定位容器！";
                return false;
            }
            (Container as WebPageModel).Response.Redirect(mnu.LinkValue);
            
            return true;
            //throw new Exception("The method or operation is not implemented.");
        }

        public override bool CreateDialoger(IFrameObj Container, BaseFormHandle CurrFormHandle, CMenuItem mnu, ref UpdateData data, ref string msg)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override bool CreateSelect(IFrameObj Container, BaseFormHandle CurrFormHandle, CMenuItem mnu, ref UpdateData data, ref string msg)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override Assembly GetAssembly()
        {
            return WebPageSwitcher.AppDllasmb;
        }

        public override bool CreateTagFrame(IFrameObj Container, BaseFormHandle CurrFormHandle, CMenuItem mnu, ref UpdateData data, ref string msg)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        
    }
}
