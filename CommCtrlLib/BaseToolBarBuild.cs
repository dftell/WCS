using System;
using WolfInv.Com.MetaDataCenter;
using System.Xml;
using XmlProcess;
using WolfInv.Com.WCS_Process;
using WolfInv.Com.XPlatformCtrlLib;
namespace WolfInv.Com.CommCtrlLib
{
    public class BaseToolBarBuild
    {
        public IFrame frm;
        public ITag ToolBar;
        public ToolBarItemBuilder toolbarbld;
        public BaseToolBarBuild(IFrame ifrm,ITag toolbar)
        {
            frm = ifrm;
            ToolBar = toolbar;
        }

        

        public void LoadToolBar()
        {
            //toolbarbld = new ToolBarBuilder(frm, ToolBar);
            string strFilePath = "";
            string strClsName = this.ToString();
            string[] strArr = strClsName.Split('.');
            string strFolderName = "";
            string strFileName = "";
            if (strArr.Length >= 3)
            {
                strFolderName = strArr[strArr.Length - 2];
                strFileName = strArr[strArr.Length - 1];
            }
            strFilePath = string.Format("{0}\\{1}\\frm_{1}_{2}_{3}.xml", "", frm.strModule, frm.strScreen, frm.strTarget);

            XmlDocument xmldoc = GlobalShare.GetXmlFile(strFilePath);
            if (xmldoc == null)
            {
                throw new Exception(string.Format("can't load xml file {0}!",strFilePath));
            }
            string tbdir = XmlUtil.GetSubNodeText(xmldoc.SelectSingleNode("root"), "@RightToLeft");
            toolbarbld.InitToolBar(tbdir=="1");
            /*
            this.toolStrip1.Items.Clear();
            string tbdir = XmlUtil.GetSubNodeText(xmldoc.SelectSingleNode("root"), "@RightToLeft");
            if (tbdir == "1")
            {
                this.toolStrip1.RightToLeft = RightToLeft.Yes;
            }*/
            AddComboInToolBar(xmldoc);
            AddButtonInToolBar(xmldoc);
            AddSimpleSearchInToolBar(xmldoc);
        }

        void AddComboInToolBar(XmlDocument xmldoc)
        {
            XmlNode cmbNode = xmldoc.SelectSingleNode("/root/MainFrmComboSel/MainFrmCbx");
            if (cmbNode == null)
            {
                return;
            }
            //this.toolStrip1.Items.Add(new ToolStripLabel(XmlUtil.GetSubNodeText(cmbNode, "@caption")));
            //ToolStripComboBox combobox = new ToolStripComboBox("查看");
            //            combobox.SelectedIndexChanged += new EventHandler(combobox_SelectedIndexChanged);

            toolbarbld.AddToolBarItem(XmlUtil.GetSubNodeText(cmbNode, "@caption"),ToolBarItemType.Label);
            toolbarbld.AddToolBarItem("查看", ToolBarItemType.Combox);

            foreach (XmlNode node in cmbNode.SelectNodes("item"))
            {

                CMenuItem mnu = new CMenuItem(frm.strUid);
                mnu.MnuName = XmlUtil.GetSubNodeText(node, ".");
                mnu.LinkValue = XmlUtil.GetSubNodeText(node, "@queryString");
                //combobox.Items.Add(mnu.MnuName);

            }
            //this.toolStrip1.Items.Add(combobox);
        }

        protected void combobox_SelectedIndexChanged(object sender, EventArgs e) { }

        void AddButtonInToolBar(XmlDocument xmldoc)
        {
            XmlNodeList btnNodes = xmldoc.SelectNodes("/root/Action/Buttons/Button");
            if (btnNodes == null)
            {
                return;
            }
            foreach (XmlNode node in btnNodes)
            {

                CMenuItem mnu = new CMenuItem(frm.strUid);
                XmlNode evtnode = node.SelectSingleNode("evt");
                if (evtnode == null)
                {
                    mnu.MnuName = XmlUtil.GetSubNodeText(node, ".");
                    mnu.LinkValue = XmlUtil.GetSubNodeText(node, "@onclick");
                    mnu.MnuId = XmlUtil.GetSubNodeText(node, "@id");
                }
                else
                {
                    mnu = MenuProcess.GetMenu(null, evtnode,frm.strUid);

                }

                toolbarbld.AddToolBarItem(mnu, ToolBarItemType.Button,frm.ToolBarBtn_Click );
                toolbarbld.AddToolBarItem(null, ToolBarItemType.Separator);
                /* //用AddToolBarItem替代，期待实现
                ToolStripItem tsi;
                tsi = new ToolStripButton(mnu.MnuName);
                tsi.Tag = mnu;
                tsi.Click += new EventHandler(ToolBarBtn_Click);
                if (this.toolStrip1.Items.Count > 0)
                    this.toolStrip1.Items.Add(new ToolStripSeparator());
                this.toolStrip1.Items.Add(tsi);*/
            }
        }

        protected  virtual void AddSimpleSearchInToolBar(XmlDocument xmldoc)
        {
            //SearchBox
            XmlNode cmbNode = xmldoc.SelectSingleNode("/root/SearchBox");
            if (cmbNode == null)
            {
                return;
            }
            //toolbarbld.AddToolBarItem(cmbNode, ToolBarItemType.Mix, "simplesearch", frm.SimpleSearch);
            toolbarbld.AddToolBarItem(null, ToolBarItemType.Separator);
            toolbarbld.AddToolBarItem(XmlUtil.GetSubNodeText(cmbNode, "@text"), ToolBarItemType.Label);
            XmlNode condnodes = cmbNode.SelectSingleNode("./items");
            DataCondition cond = new DataCondition();
            if (condnodes != null)
            {
                DataCondition.FillCondition(condnodes, ref cond);
            }
            ITag ssearchbox = toolbarbld.AddToolBarItem(XmlUtil.GetSubNodeText(cmbNode, "@name"),condnodes,"", ToolBarItemType.TextBox,frm.SimpleSearch);
            ITag searchbtn = toolbarbld.AddToolBarItem(XmlUtil.GetSubNodeText(cmbNode, "@name"),null,XmlUtil.GetSubNodeText(cmbNode, "@text"), ToolBarItemType.Button, frm.SimpleSearch);
            
            ssearchbox.Tag = cond;
            searchbtn.Tag = ssearchbox;
            #region old code
            /*
            this.toolStrip1.Items.Add(new ToolStripSeparator());
            this.toolStrip1.Items.Add(new ToolStripLabel(XmlUtil.GetSubNodeText(cmbNode, "@text")));
            ToolStripTextBox ssearchbox = new ToolStripTextBox();

            ToolStripButton searchbtn = new ToolStripButton();
            (this.TopLevelControl as Form).AcceptButton = searchbtn as IButtonControl;
            searchbtn.Text = XmlUtil.GetSubNodeText(cmbNode, "btn/@text");
            searchbtn.Click += ToolBar_OnSimpleSearchClicked;
            ssearchbox.KeyUp += new KeyEventHandler(ssearchbox_KeyUp);
            XmlNode condnodes = cmbNode.SelectSingleNode("./items");
            DataCondition cond = new DataCondition();
            if (condnodes != null)
            {
                DataCondition.FillCondition(condnodes, ref cond);
            }
            ssearchbox.Tag = cond;
            searchbtn.Tag = ssearchbox;
            this.toolStrip1.Tag = searchbtn;
            this.toolStrip1.Items.Add(ssearchbox);
            this.toolStrip1.Items.Add(searchbtn);*/
            #endregion
        }
    }

}
