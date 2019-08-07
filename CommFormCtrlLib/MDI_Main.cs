using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WolfInv.Com.WCS_Process;
using WolfInv.Com.MetaDataCenter;
using WolfInv.Com.CommCtrlLib;
using WolfInv.Com.CommFormCtrlLib;
using WolfInv.Com.XPlatformCtrlLib;

namespace WolfInv.Com.CommFormCtrlLib
{
    public partial class MDI_Main : Form,IUserData
    {
        public int childFormNumber = 0;
        string _uid;
        public string strUid { get { return _uid; } set { _uid = value; } }
        public MDI_Main(string uid)
        {
            _uid = uid;
            InitializeComponent();
            //FrameSwitch.ParentForm = this;
            Init();
            this.MainMenuStrip = this.menuStrip1;
            this.Icon = FrameSwitch.SystemIcon;
            this.Text = FrameSwitch.SystemText;
            this.statusStrip.Items.Clear();
            foreach (string key in GlobalShare.SystemAppInfo.SystemItems.Keys)
            {
                if (key == "password" || key == "today") continue;
                if (key.StartsWith("curr")) continue;
                string dpt = GlobalShare.SystemAppInfo.SystemItems[key].datapoint;
                if (GlobalShare.DefaultUserInfo.appinfo.UserInfo.Items.ContainsKey(dpt))
                {
                    
                    ToolStripStatusLabel lbl = new ToolStripStatusLabel();
                    lbl.Text = string.Format("{0}:{1}", GlobalShare.SystemAppInfo.SystemItems[key].text,GlobalShare.DefaultUserInfo.appinfo.UserInfo.Items[dpt].value);
                    if (this.statusStrip.Items.Count > 0) this.statusStrip.Items.Add(new ToolStripSeparator());
                    this.statusStrip.Items.Add(lbl);
                }
            }
            //this.toolStripStatusLabel.Text = GlobalShare.CurrUser.UserName;
        }

        string Init()
        {
            //初始化菜单
            MenuProcess mnuprss = new MenuProcess(GlobalShare.GetXmlFile(@"\xml\menus.xml"),strUid);
            List<CMenuItem> Menus = mnuprss.GenerateMenus();
            mnuprss = new MenuProcess(GlobalShare.GetXmlFile( @"\xml\nav_main_main.xml"),strUid);
            List<CMenuItem> Navigator = mnuprss.GenerateMenus();
            this.menuStrip1.Items.Clear();

            if (Menus == null)//无法初始化菜单
                return "02_001";
            for (int i = 0; i < Menus.Count; i++)
            {
                if (Menus[i].PermId == "0") continue;
                ToolStripMenuItem mnu = new ToolStripMenuItem();
                FillMenu(mnu, Menus[i]);
                this.menuStrip1.Items.Add(mnu);
            }

            //初始化导航栏
            this.treeView_nav.Nodes.Clear();
            if (Navigator == null)//无法初始化菜单
                return "02_001";
            for (int i = 0; i < Navigator.Count; i++)
            {
                TreeNode mnu = new TreeNode();
                if (Navigator[i].PermId == "0") continue;
                FillNavigator(mnu, Navigator[i]);
                this.treeView_nav.Nodes.Add(mnu);
            }

            //this.treeView_nav();
            return null;
        }

        void FillMenu(ToolStripMenuItem mnu, CMenuItem val)
        {
            if (val.PermId == "0") return;
            if (mnu == null) mnu = new ToolStripMenuItem();
            mnu.Text = val.MnuName;
            mnu.Tag = val;


            if (val.MnuItems == null || val.MnuItems.Count == 0)
            {
                mnu.Click += new EventHandler(mnu_Click);
                return;
            }
            for (int i = 0; i < val.MnuItems.Count; i++)
            {
                if (val.MnuItems[i].PermId == "0") continue;
                ToolStripMenuItem submnu = new ToolStripMenuItem();
                FillMenu(submnu, val.MnuItems[i]);
                mnu.DropDown.Items.Add(submnu);

            }

        }

        void mnu_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menu = sender as ToolStripMenuItem;
            CMenuItem mnu = menu.Tag as CMenuItem;
            ////if (mnu.LinkValue == null || mnu.LinkValue.Trim().Length == 0)
            ////{
            ////    return;
            ////}
            ////Type tFrm = Type.GetType(mnu.LinkValue);
            ////if (mnu.linkType != LinkType.WebPage)
            ////{

            ////    if (tFrm == null)
            ////        return;
            ////}
            ////this.splitContainer1.Panel2.Controls.Clear();//清除所有控件
            FrameSwitch.switchToView(this.Main_Plan, mnu);

        }

        void FillNavigator(TreeNode mnu, CMenuItem val)
        {
            if (val.PermId == "0") return;
            if (mnu == null) mnu = new TreeNode();
            mnu.Text = val.MnuName;
            mnu.Tag = val;
            if (val.Expand)
                mnu.Expand();
            for (int i = 0; i < val.MnuItems.Count; i++)
            {
                if (val.MnuItems[i].PermId == "0") continue;
                TreeNode submnu = new TreeNode();
                FillNavigator(submnu, val.MnuItems[i]);
                mnu.Nodes.Add(submnu);

            }

        }

        private void treeView_nav_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode tnode = e.Node;
            CMenuItem mnu = tnode.Tag as CMenuItem;
            while (tnode.Nodes.Count > 0 && mnu.LinkValue == "")
            {
                tnode = tnode.Nodes[0];
            }

            this.Cursor = Cursors.WaitCursor;
            FrameSwitch.switchToView(this.Main_Plan, mnu);
            this.Cursor = Cursors.Default;
        }

        private void frm_Main_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void frm_Main_DoubleClick(object sender, EventArgs e)
        {

        }

        private void statusStrip1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                
                ////foreach (UpdateData data in datas)
                ////{
                ////    DataSource ds = GlobalShare.mapDataSource[nec.Config.DataSource];
                ////    if(GlobalShare.mapDataSource.ContainsKey(nec.Config.GridSource))
                ////        ds.SubSource = GlobalShare.mapDataSource[nec.Config.GridSource];
                ////    DataCondition dc = new DataCondition();
                ////    dc.Datapoint = data.keydpt;
                ////    GlobalShare.DataCenterClientObj.UpdateDataList(ds,dc,data,DataRequestType.Add);
                ////}
                //return;
            }
            catch (Exception ce)
            {
                MessageBox.Show(ce.Message);
            }
            GlobalShare.DataCenterClientObj.RunTool();
        }

        private void treeView_nav_DoubleClick(object sender, EventArgs e)
        {
            
            Init();
        }


    }
}
