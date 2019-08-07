using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using WolfInv.Com.CommFormCtrlLib;
using WolfInv.Com.CommCtrlLib;
using System.Xml;
using WolfInv.Com.MetaDataCenter;
using WolfInv.Com.XPlatformCtrlLib;

namespace WCS
{
    public partial class frm_TabFrame :frm_Model,IFrameObj,IUserData 
    {
        FormTabFrameHandle currhandle;
        
        public frm_TabFrame()
        {
            InitializeComponent();

        }

        public frm_TabFrame(string id)
        {
            InitializeComponent();
            this.strRowId = id;
        }

        private void frm_TabFrame_Load(object sender, EventArgs e)
        {
            ////currhandle = this.BehHandleObject as FormTabFrameHandle;
            ////currhandle.LoadControls();
            LoadControls();
        }

        protected override XmlDocument GetConfigXml()
        {
            return GetConfigXml("tab");
        }

        public override bool LoadControls()
        {
           //return base.LoadControls();
            XmlDocument xmldoc = GetConfigXml();
            if (xmldoc == null) return false;
            this.toolStrip1.Visible = false;
            this.panel1.Visible = false;
            this.panel_main.SetDock(XPlatformDockStyle.Fill);
            XmlNodeList nodes = xmldoc.SelectNodes("/root/tabs/tab");
            this.tabControl1.TabPages.Clear();
            for (int i = 0; i < nodes.Count; i++)
            {
                CMenuItem mnu = new CMenuItem(this.strUid);
                mnu.LoadXml(nodes[i]);
                TabPage tp = new TabPage();
                tp.Text = mnu.Title;
                IXPanel mainpanel = new XWinForm_Panel();
                mainpanel.SetDock(XPlatformDockStyle.Fill);
                tp.Controls.Add((mainpanel as Panel));
                UpdateData data = this.NeedUpdateData;
                mnu.Params = this.strRowId;
                mnu.MnuName = this.lb_Title.Text;
                FrameSwitch.switchToView(mainpanel, null, mnu, ref data);
                this.tabControl1.TabPages.Add(tp);
            }
            //InitGrid(xmldoc);
            return true;
        }
    }

    public class FormTabFrameHandle : WinFormHandle
    {
        frm_TabFrame frm;
        TabControlEx tabcontainer;
        public FormTabFrameHandle():base()
        {
            frm = this.DataFrm as frm_TabFrame;
        }
        public FormTabFrameHandle(string id):base(id)
        {
        }

        public override UpdateData GetUpdateData(bool JudgeValueChanged, bool UpdateFrameData)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    
        public void InitTabs()
        {
            
        }

        public override bool LoadControls()
        {
            XmlDocument xmldoc = this.GetConfigXml();
            if (xmldoc == null)
                return false;
            XmlNodeList nodes = xmldoc.SelectNodes("/root/tabs/tab");
            for (int i = 0; i < nodes.Count; i++)
            {
                CMenuItem mnu = new CMenuItem(this.strUid);
                mnu.LoadXml(nodes[i]);
                WinFormSwitcher wfs = new WinFormSwitcher();
                UpdateData data = null;
                string msg = null;
                BaseFormHandle handle = null;
                bool ret = wfs.GetFrameHandle(null, null, mnu, ref data, ref msg,ref handle);
                if (!ret || msg != null)
                    continue;

            }
            return true;
        }

        public override void BoundDataControls(params ITag[] controls)
        {
            tabcontainer = controls[0] as TabControlEx;
        }



        public override List<UpdateData> GetDataList(List<UpdateData> OrgList, bool OnlyCheckedItem)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        
    }

    public class TabControlEx : TabControl, ITag
    {
        public TabControlEx()
            : base()
        {
        }
    }
}
