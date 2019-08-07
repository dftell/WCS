using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WolfInv.Com.WCS_Process;
using WolfInv.Com.MetaDataCenter;
using WolfInv.Com.CommFormCtrlLib;
using WolfInv.Com.CommCtrlLib;
using System.Xml;
using WolfInv.Com.XPlatformCtrlLib;

namespace WCS
{
    public partial class frm_Main : Form
    {
        public UpdateData Data;
        public frm_Main()
        {
            InitializeComponent();
            //FrameSwitch.ParentForm = this;
            Init();
            
            
        }
        string Init()
        {
            //初始化菜单
            XmlDocument xmlmnu = new XmlDocument();
            XmlDocument xmlnav = new XmlDocument();
            try
            {
                xmlmnu.Load(Application.StartupPath + @"\xml\menus.xml");
                xmlnav.Load(Application.StartupPath + @"\xml\nav_main_main.xml");
            }
            catch(Exception ce)
            {
                return null;
            }
            MenuProcess mnuprss = new MenuProcess(xmlmnu, "mnu");
            List<CMenuItem> Menus = mnuprss.GenerateMenus();
            mnuprss = new MenuProcess(xmlnav,"nav");
            List<CMenuItem> Navigator = mnuprss.GenerateMenus();
            this.menuStrip1.Items.Clear();
            
            if (Menus == null)//无法初始化菜单
                return "02_001";
            for (int i = 0; i < Menus.Count; i++)
            {
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
                FillNavigator(mnu, Navigator[i]);
                this.treeView_nav.Nodes.Add(mnu);
            }

            //this.treeView_nav();
            return null;
        }

        void FillMenu(ToolStripMenuItem mnu, CMenuItem val)
        {
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
            if (mnu == null) mnu = new TreeNode();
            mnu.Text = val.MnuName;
            mnu.Tag = val;
            if (val.Expand)
                mnu.Expand();
            for (int i = 0; i < val.MnuItems.Count; i++)
            {
                TreeNode submnu = new TreeNode();
                FillNavigator(submnu, val.MnuItems[i]);
                mnu.Nodes.Add(submnu);

            }

        }


        private void frm_Main_Load(object sender, EventArgs e)
        {
            //ghgl.frm_ShowAllTypes frm = new ghgl.frm_ShowAllTypes();
            //frm.Dock = DockStyle.Fill;
            //frm.Parent = this.splitContainer1.Panel2;
            this.Icon = FrameSwitch.SystemIcon;
            this.Text = FrameSwitch.SystemText;
        }

        private void treeView_nav_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode tnode = e.Node;
            CMenuItem mnu = tnode.Tag as CMenuItem;
            while(tnode.Nodes.Count > 0 && mnu.LinkValue == "")
            {
                tnode = tnode.Nodes[0];
            }
            
            
            FrameSwitch.switchToView(this.Main_Plan, mnu);
           
        }

        private void frm_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void frm_Main_DoubleClick(object sender, EventArgs e)
        {
            
        }

        private void statusStrip1_DoubleClick(object sender, EventArgs e)
        {
            GlobalShare.DataCenterClientObj.RunTool();
        }


        
    }

}