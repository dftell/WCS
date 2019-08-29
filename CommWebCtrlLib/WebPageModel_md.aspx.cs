using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using WolfInv.Com.CommCtrlLib;
using WolfInv.Com.CommWebCtrlLib;
using WolfInv.Com.MetaDataCenter;
using WolfInv.Com.WCS_Process;
using WolfInv.Com.XPlatformCtrlLib;
using XmlProcess;

namespace WolfInv.Com.CommWebCtrlLib
{
    
    public partial class WebPageModel_md : Page, Ifrm_Model
    {
        protected Table MainFrame;
        protected ToolBarStrip toolbarstrip;
        protected Label PageTitle;
        protected TableCell TdMain;
        protected TableCell TdBottom;
        protected Page CurrPage;
        public void SetPage(Page pg)
        {
            CurrPage = pg;
        }
        #region members

        bool _Loaded = false;
        protected bool LoadFlag { get { return _Loaded; } set { _Loaded = value; } }
        //ToolBarBuilderWinForm BehFrame;
        public IKeyForm Link;
        string _strModule = "System";
        string _strScreen = "main";
        string _strTarget = "Summary";
        public string _strRowId = "";
        public string _GridSource;
        public string _DetailSource;
        public string GridSource { get { return _GridSource; } set { _GridSource = value; } }
        public string DetailSource { get { return _DetailSource; } set { _DetailSource = value; } }
        public string _strKey;
        WebPageFormHandle behobj;
        //public BaseFormHandle BehHandleObject { get { return behobj; } set { behobj = value as WebPageFormHandle; } }
        string _uid;
        public string strUid { get { return _uid; } set { _uid = value; } }
        #region IKeyForm 成员

        public string strRowId { get { return _strRowId; } set { _strRowId = value; } }

        public string strKey { get { return _strKey; } set { _strKey = value; } }

        #endregion

        #region ITranslateableInterFace 成员
        UpdateData _data;
        List<DataTranMapping> _trandata;

        public UpdateData NeedUpdateData
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
            }
        }

        public List<DataTranMapping> TranData
        {
            get
            {
                return _trandata;
            }
            set
            {
                _trandata = value;
            }
        }

        public UpdateData GetCurrFrameData()
        {
            return this.GetUpdateData(false, false);
        }
        #endregion

        public bool AllowClose
        {
            get { return false; }

        }
        #endregion
        public WebPageModel_md():base()
        {
            //Init_Compenent();

        }
        public IXControl CurrMainControl { get; set; }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        public WebPageModel_md(string skey):base()
        {
            strRowId = skey;
            //Init_Compenent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Init_Compenent();
        }

        protected virtual void InitPage(Page pg,string rnd)
        {

            //string rnd = this.Request["rnd"];
            //if (rnd == null || rnd.Trim().Length == 0)
            //{
            //    //MessageBox.Alert(this, "外部请求！");
            //    return;
            //}
            //string rnd = new Random().NextDouble().ToString();
            if (!WebPageSwitcher.ExtraReqMappings.ContainsKey(rnd))
            {
                MessageBox.Alert(pg, "非法的外部请求！");
                return;
            }
            this.BehHandleObject = WebPageSwitcher.ExtraReqMappings[rnd].Handle;
            this.BehHandleObject.DataFrm = this;
            behobj = this.BehHandleObject as WebPageFormHandle;
            ToolBarBuildForWeb tbf = new ToolBarBuildForWeb(behobj, this.toolbarstrip);
            //tbf.LoadToolBar();
            //this.BehHandleObject = new WebPageFormHandle();
        }

        public virtual void LoadPage(Page pg,string rnd)
        {

        }
        public void Page_Load(object sender, EventArgs e)
        {

        }

        

        protected void Init_Compenent()
        {

            toolbarstrip = new ToolBarStrip();
            MainFrame = new Table();
            MainFrame.ID = "MainFrame";
            MainFrame.Width = new Unit(100, UnitType.Percentage);
            MainFrame.Height = new Unit(100, UnitType.Percentage);

            //头行
            TableRow TrHeader = new TableRow();
            TableCell TdHeader = null;
            TrHeader.BackColor = SystemColors.ControlDark;
            TrHeader.Height = new Unit(20);
            TdHeader = new TableCell();
            TdHeader.HorizontalAlign = HorizontalAlign.Left;
            PageTitle = new Label();
            PageTitle.CssClass = "Bold:true;";
            PageTitle.Text = "Home";
            TdHeader.Controls.Add(PageTitle);
            TrHeader.Cells.Add(TdHeader);

            //工具栏
            TableRow TrToolBar = new TableRow();
            TableCell TDToolBar = new TableCell();
            TrToolBar.BackColor = SystemColors.Control;
            TrToolBar.Height = new Unit(20, UnitType.Pixel);
            TrToolBar.Cells.Add(TDToolBar);
            TDToolBar.Controls.Add(toolbarstrip);

            //中间行
            TableRow TrMiddle = new TableRow();
            TrMiddle.Height = new Unit(470, UnitType.Pixel);
            TdMain = new TableCell();
            TdMain.ID = "TdMain";
            TdMain.HorizontalAlign = HorizontalAlign.Justify;
            TdMain.VerticalAlign = VerticalAlign.Top;
            TdMain.Height = new Unit(450, UnitType.Pixel);
            TdMain.Width = new Unit(600);
            TdMain.Style.Add(HtmlTextWriterStyle.Overflow, "auto");
            TrMiddle.Cells.Add(TdMain);


            //底行
            TableRow TrBottom = new TableRow();
            TrBottom.Height = new Unit(20, UnitType.Pixel);
            TrBottom.BackColor = SystemColors.ControlLight;
            TdBottom = new TableCell();
            TdBottom.HorizontalAlign = HorizontalAlign.Left;
            TrBottom.Cells.Add(TdBottom);



            MainFrame.Rows.Add(TrHeader);
            MainFrame.Rows.Add(TrToolBar);
            MainFrame.Rows.Add(TrMiddle);
            MainFrame.Rows.Add(TrBottom);
            this.Controls.Add(MainFrame);
        }


        public List<UpdateData> InjectedDatas
        {
            get; set;
        }

        public DataSet InitDataSource(string sGridSource, out string msg)
        {
            msg = null;
            if (sGridSource == null || sGridSource.Trim().Length == 0)
            {
                msg = string.Format("数据源为空");
                return null; ;
            }
            bool isextra = false;
            return DataSource.InitDataSource(sGridSource, InitBaseConditions(), strUid, out msg,ref isextra);

        }

        public virtual bool CheckData()
        {
            return false;
        }

        protected void LoadToolBar()
        {

            XmlDocument xmldoc = GetConfigXml();

            AddComboInToolBar(xmldoc);
            AddButtonInToolBar(xmldoc);
            AddSimpleSearchInToolBar(xmldoc);
        }

        void AddComboInToolBar(XmlDocument xmldoc)
        {
            if (xmldoc == null) return;
            XmlNode cmbNode = xmldoc.SelectSingleNode("/root/MainFrmComboSel/MainFrmCbx");
            if (cmbNode == null)
            {
                return;
            }
            string strPerm = XmlUtil.GetSubNodeText(cmbNode, "@perm");
            if (strPerm == "0") return;
            ToolBarStripLabel tl = new ToolBarStripLabel();
            tl.Text = XmlUtil.GetSubNodeText(cmbNode, "@caption");
            this.toolbarstrip.Items.Add(tl);

            ToolBarStripComobox combobox = new ToolBarStripComobox(); //"查看"
            combobox.Text = "查看";
            //combobox.SelectedIndexChanged += new EventHandler(combobox_SelectedIndexChanged);
            combobox.SelectedIndexChanged += new EventHandler(combobox_SelectedIndexChanged);

            List<CMenuItem> menus = new List<CMenuItem>();
            XmlDocument combodoc = new XmlDocument();
            try
            {
                combodoc.LoadXml(cmbNode.InnerXml);
            }
            catch
            {
                return;
            }
            MenuProcess mp = new MenuProcess(combodoc, this.strUid);
            menus = mp.GenerateMenus();
            foreach (CMenuItem mnu in menus)
            {
                combobox.Items.Add(mnu.MnuName);
            }
            combobox.Tag = menus;
            this.toolbarstrip.Items.Add(combobox);
            if (menus.Count > 0)
                combobox.SelectedIndex = 0;
        }

        protected virtual void combobox_SelectedIndexChanged(object sender, EventArgs e) { }

        void AddButtonInToolBar(XmlDocument xmldoc)
        {
            if (xmldoc == null) return;
            XmlNodeList btnNodes = xmldoc.SelectNodes("/root/Action/Buttons/Button");
            if (btnNodes == null)
            {
                return;
            }
            foreach (XmlNode node in btnNodes)
            {
                string strPerm = XmlUtil.GetSubNodeText(node, "@perm");
                bool disable = false;
                if (strPerm == "0")
                    disable = true;
                CMenuItem mnu = new CMenuItem(strUid);
                XmlNode evtnode = node.SelectSingleNode("evt");
                if (evtnode == null)
                {
                    mnu.MnuName = XmlUtil.GetSubNodeText(node, ".");
                    mnu.LinkValue = XmlUtil.GetSubNodeText(node, "@onclick");
                    mnu.MnuId = XmlUtil.GetSubNodeText(node, "@id");
                }
                else
                {
                    mnu = MenuProcess.GetMenu(null, evtnode, strUid);
                    if (mnu.PermId == "0")
                    {
                        disable = true;
                    }
                }
                //ToolStripItem tsi;
                ToolBarStripItem tsi;
                tsi = new ToolBarStripButton(mnu.MnuName);
                tsi.Tag = mnu;
                tsi.Click += new EventHandler(ToolBarBtn_Click);
                if (disable)
                    tsi.Enabled = false;
                if (this.toolbarstrip.Items.Count > 0)
                    this.toolbarstrip.Items.Add(new ToolBarStripSeparator());
                this.toolbarstrip.Items.Add(tsi);
            }
        }

        void AddSimpleSearchInToolBar(XmlDocument xmldoc)
        {
            //SearchBox
            if (xmldoc == null) return;
            XmlNode cmbNode = xmldoc.SelectSingleNode("/root/SearchBox");
            if (cmbNode == null)
            {
                return;
            }
            this.toolbarstrip.Items.Add(new ToolBarStripSeparator());
            this.toolbarstrip.Items.Add(new ToolBarStripLabel(XmlUtil.GetSubNodeText(cmbNode, "@text")));
            ToolStripTextBoxD ssearchbox = new ToolStripTextBoxD();

            ToolBarStripButton searchbtn = new ToolBarStripButton();
            //(this.Parent as Page).AcceptButton = searchbtn as IButtonControl;
            searchbtn.Text = XmlUtil.GetSubNodeText(cmbNode, "btn/@text");
            searchbtn.Click += ToolBar_OnSimpleSearchClicked;
            //ssearchbox.KeyUp += new KeyEventHandler(ssearchbox_KeyUp);
            XmlNode condnodes = cmbNode.SelectSingleNode("./items");
            DataCondition cond = new DataCondition();
            if (condnodes != null)
            {
                DataCondition.FillCondition(condnodes, ref cond);
            }
            ssearchbox.Tag = cond;
            searchbtn.Tag = ssearchbox;
            this.toolbarstrip.Tag = searchbtn;
            this.toolbarstrip.Items.Add(ssearchbox);
            this.toolbarstrip.Items.Add(searchbtn);
        }

        protected void frm_Model_Load(object sender, EventArgs e)
        {
            InitEvent();
            //
            if (strKey != null)//防止设计器里面加载loadtoolbar()
                LoadToolBar();
            //ToolBarBuilderWinForm tbbf = new ToolBarBuilderWinForm(this, this.toolStrip1);

            //tbbf.LoadToolBar();

        }

        //void ssearchbox_KeyUp(object sender, KeyEventArgs e)
        //{
        //    if (e.KeyCode == Keys.Enter)
        //    {
        //        ToolBar_OnSimpleSearchClicked(this.toolStrip1.Tag as ToolStripButton, null);
        //        return;
        //    }
        //}

        public virtual List<DataCondition> InitBaseConditions()
        {

            List<DataCondition> conds = new List<DataCondition>();
            DataCondition ds = new DataCondition();
            ds.Datapoint = new DataPoint(this.strKey);
            ds.value = this.strRowId;
            ds.Logic = ConditionLogic.And;
            if (this.TranData != null)
            {
                foreach (DataTranMapping data in this.TranData)
                {
                    DataCondition datacond = new DataCondition();
                    datacond.value = data.FromDataPoint.Text;
                    datacond.Datapoint = new DataPoint(data.ToDataPoint);
                    conds.Add(datacond);
                }
            }
            conds.Add(ds);
            return conds;
        }

        public void ToolBarBtn_Click(object sender, EventArgs e)
        {
            //if (this.LoadFlag ==false) return;
            CMenuItem mnu = (sender as ToolBarStripButton).Tag as CMenuItem;
            if (mnu == null) return;
            switch (mnu.MnuId)
            {
                case "EditView":
                    {
                        ToolBar_EditView();
                        //EditView();
                        break;
                    }
                case "SaveClose":
                    {
                        if (ToolBar_SaveClose(mnu))
                        {
                            ToolBar_RefreshData();
                        }
                        break;
                    }
                case "SaveNew":
                    {
                        ToolBar_SaveAndCreateNew(mnu);
                        break;
                    }
                case "New":
                    {
                        ToolBar_NewCreate(mnu);
                        break;
                    }
                case "Remove":
                    {
                        ToolBar_Remove(mnu);
                        break;
                    }
                case "ExportExcel":
                    {
                        ToolBar_Export();
                        break;
                    }
                case "Refresh":

                    {
                        ToolBar_RefreshData();
                        break;
                    }
                case "OKNoSave":
                    {
                        ToolBar_OkNoSave();
                        break;
                    }
                case "AddRows":
                    {
                        ToolBar_AddExist(mnu);
                        break;
                    }
                case "ListSelectedItems"://
                    {
                        ToolBar_ListSelectedItemsClicked();
                        break;
                    }
                case "ExportTo":
                    {
                        ToolBar_ExportTo(mnu);
                        break;
                    }
                default:
                    {
                        ToolBar_OtherEvent(mnu);
                        break;
                    }
            }
        }

        protected bool SaveClose_Click(CMenuItem mnu)
        {
            if (!CheckData())
            {
                return false;
            }
            if (Save(mnu))
            {

                //(this.TopLevelControl as Form).Close();
                ////Page frm = this.Parent as Page;
                ////if (frm == null)
                ////    frm = this.TopLevelControl as Form;
                ////frm.DialogResult = DialogResult.Yes;
                ////frm.Close();


            }
            else
            {
                ////Form frm = this.Parent as Form;
                //////MessageBox.Show("保存失败！");
                //////frm.DialogResult = DialogResult.No;
                //////frm.Close();
                MessageBox.Alert(CurrPage, "保存失败！");
                return false;
            }
            return true;
        }

        protected void SaveNew_Click(CMenuItem mnu)
        {
            if (!CheckData())
            {
                return;
            }
            if (Save(mnu))
            {
                this.strRowId = "";
                LoadControls();
            }
            else
            {


                MessageBox.Alert(CurrPage, "保存失败！");
            }
        }

        protected void OkNoSave_Click()
        {
            if (!CheckData())
            {
                return;
            }
            UpdateData data = GetUpdateData(true);//需要检查是否有改变值
            //if (data.Items.Count > 0)
            //{


            //(this.TopLevelControl as Form).Close();
            ////Form frm = this.Parent as Form;
            ////frm.DialogResult = DialogResult.Yes;
            ////frm.Close();

            // }
            return;
        }

        #region IFrame 成员

        public virtual event EventHandler ToolBar_OnSimpleSearchClicked;

        public virtual event ToolBarHandle ToolBar_ListSelectedItemsClicked;

        public virtual event ToolBarHandle ToolBar_EditView;

        public virtual event AddExistHandle ToolBar_PrintPDF;

        public virtual event ToolBarResponseHandle ToolBar_SaveClose;

        public virtual event AddExistHandle ToolBar_SaveAndCreateNew;

        public virtual event AddExistHandle ToolBar_AddExist;

        public virtual event AddExistHandle ToolBar_NewCreate;

        public virtual event ToolBarHandle ToolBar_RefreshData;

        public virtual event AddExistHandle ToolBar_Remove;

        public virtual event ToolBarHandle ToolBar_Export;

        public virtual event ToolBarHandle ToolBar_OkNoSave;

        public virtual event EventHandler ToolBar_Clicked;

        public virtual event AddExistHandle ToolBar_SaveAs;

        public virtual event AddExistHandle ToolBar_OtherEvent;

        public virtual event AddExistHandle ToolBar_ExportTo;
        public event AddExistHandle ToolBar_EditMappings;
        public event AddExistHandle ToolBar_BatchUpdate;
        public event ToolBarHandle ToolBar_SelectAll;
        public event ToolBarHandle ToolBar_InSelect;

        public virtual void SimpleSearch(object sender, EventArgs e) { }

        public virtual void RefreshData_Click() { }

        public virtual UpdateData GetUpdateData(bool JudgeValueChanged, bool UpdateFrameData,bool getText=false)
        {
            UpdateData ret = new UpdateData();
            if (this.TranData == null) return ret;
            for (int i = 0; i < this.TranData.Count; i++)
            {
                if (GlobalShare.DataPointMappings.ContainsKey(this.TranData[i].ToDataPoint))
                {
                    if (!ret.Items.ContainsKey(this.TranData[i].ToDataPoint))
                    {

                        ret.Items.Add(this.TranData[i].ToDataPoint, new UpdateItem(this.TranData[i].ToDataPoint, this.TranData[i].FromDataPoint.Text));
                    }
                }
            }
            return ret;
            //throw new Exception("The method or operation is not implemented.");
        }

        public virtual UpdateData GetUpdateData(bool JudgeValueChanged)
        {
            return GetUpdateData(false, false);
            //throw new Exception("The method or operation is not implemented.");
        }

        public virtual void ToolBar_OtherEvent_Click(CMenuItem mnu)
        {
            if (mnu.LinkValue == "") return;
            if (mnu.ReplaceMenu)
            {
                CMenuItem rplmnu = mnu.Clone() as CMenuItem;
                rplmnu.Params = strRowId;
                rplmnu.MnuName = this.lb_Title.Text;
                mnu = rplmnu;
            }
            if (mnu.linkType == LinkType.Dialog || mnu.linkType == LinkType.Select)
            {
                UpdateData data = null;
                if (FrameSwitch.ShowDialoger(null, this, mnu, ref data))
                {
                    ToolBar_RefreshData();
                }
            }
            else
            {
                FrameSwitch.switchToView(null, this, mnu);
            }

        }

        public virtual bool Save(CMenuItem mnu)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        protected virtual XmlDocument GetConfigXml()
        {
            string strFilePath = string.Format("{0}\\{1}\\frm_{1}_{2}_{3}.xml", "", strModule, strScreen, strTarget);
            return GlobalShare.UpdateWithUseInfo(GlobalShare.GetXmlFile(strFilePath), strUid) as XmlDocument;
        }

        protected XmlDocument GetConfigXml(string flag)
        {
            string strFilePath = string.Format("{0}\\{1}\\{4}_{1}_{2}_{3}.xml", "", strModule, strScreen, strTarget, flag);
            return GlobalShare.UpdateWithUseInfo(GlobalShare.GetXmlFile(strFilePath), strUid) as XmlDocument;
        }

        protected XmlDocument GetConfigXsl(string name)
        {
            string strFilePath = string.Format("{0}\\{1}\\xsl\\{2}", "", strModule, name);
            return GlobalShare.UpdateWithUseInfo(GlobalShare.GetXmlFile(strFilePath), strUid) as XmlDocument;
        }

        public virtual bool LoadControls()
        {
            //throw new Exception("The method or operation is not implemented.");
            return true;
        }

        public virtual void InitEvent()
        {
            ToolBar_Clicked += new EventHandler(ToolBarBtn_Click);
            ToolBar_OnSimpleSearchClicked += new EventHandler(SimpleSearch);
            ToolBar_OkNoSave += new ToolBarHandle(OkNoSave_Click);
            ToolBar_SaveClose += SaveClose_Click;
            ToolBar_SaveAndCreateNew += SaveNew_Click;
            ToolBar_RefreshData += RefreshData_Click;
            ToolBar_OtherEvent += new AddExistHandle(ToolBar_OtherEvent_Click);
            ToolBar_ExportTo += new AddExistHandle(frm_Model_ToolBar_ExportTo);
        }

        public virtual void frm_Model_ToolBar_ExportTo(CMenuItem mnu)
        {
            MessageBox.Alert(CurrPage, "还未实现该功能！");
        }

        #endregion

        #region IModuleControl 成员

        public string strModule
        {
            get { return _strModule; }
            set { _strModule = value; }
        }

        public string strScreen
        {
            get { return _strScreen; }
            set { _strScreen = value; }
        }

        public string strTarget
        {
            get { return _strTarget; }
            set { _strTarget = value; }
        }

        #endregion

        #region IFrameObj 成员
        BaseFormHandle _BehHandleObject;
        public BaseFormHandle BehHandleObject
        {
            get
            {
                return _BehHandleObject;
            }
            set
            {
                _BehHandleObject = value;
            }
        }

        public string strRefKey { get; set; }
        public string strRefRowId { get; set; }
        public List<DataTranMapping> RefData { get; set; }
        public IXLabel lb_Title { get; set; }
        IKeyForm ILink.Link { get; set; }
        public object Tag { get; set; }

        public PlatformControlType ControlType
        {
            get
            {
                return PlatformControlType.Web;
            }
        }

        public CMenuItem FromMenu { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        #endregion

        #region IMutliDataInterface 成员

        public virtual List<UpdateData> GetDataList(List<UpdateData> OrgList, bool OnlyCheckedItem)
        {
            return new List<UpdateData>();
        }

        public List<UpdateData> GetDataList(List<UpdateData> OrgList)
        {
            return GetDataList(OrgList, false);
        }

        public List<UpdateData> GetDataList(bool OnlyCheckedItem)
        {
            return GetDataList(null, OnlyCheckedItem);
        }

        #endregion

        private void panel1_DoubleClick(object sender, EventArgs e)
        {
            string msg = @"module:{0}
screen:{1}
target:{2}
key:{3}
param:{4}
grid:{5}
detail:{6}
class:{7}";
            msg = string.Format(msg, strModule, strScreen, strTarget, strKey, strRowId, GridSource, DetailSource, this.GetType());
            MessageBox.Alert(CurrPage, msg);
        }

        public DataSet InitDataSource(string sGridSource, List<DataCondition> dc, out string msg)
        {
            msg = "还未实现该功能";
            return null;
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            CurrPage.Dispose();

        }

        DataSet IDataSouceControl.InitDataSource(string sGridSource, out string msg)
        {
            msg = "还未实现该功能";
            return null;
        }

        public void Controls_Add(IXControl ctrl)
        {
            Controls.Add(ctrl as Control);
        }

        public void Controls_Clear()
        {
            Controls.Clear();
        }

        public void SetDock(XPlatformDockStyle dock)
        {
            
        }

        public void ToTopLevel()
        {
            
        }
    }

}