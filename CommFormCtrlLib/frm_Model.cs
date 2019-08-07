using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using WolfInv.Com.WCS_Process;
using XmlProcess;
using System.Xml;
using System.IO;
using WolfInv.Com.MetaDataCenter;
using WolfInv.Com.CommCtrlLib;
using WolfInv.Com.XPlatformCtrlLib;
namespace WolfInv.Com.CommFormCtrlLib
{
    

    //[DefaultEvent("RefreshData")]
    public partial class frm_Model : XWinForm_UserControl, Ifrm_Model
    {
        #region members
        bool _Loaded = false;
        protected bool LoadFlag { get { return _Loaded; } set { _Loaded = value; } }
        ToolBarBuilderWinForm BehFrame;
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
        XWinForm_Label tlb_Title;
        public XWinForm_Label lb_Title { get;  }
        string _uid;
        public string strUid { get { return _uid; } set { _uid = value; } }
        #region IKeyForm 成员

        public string strRowId{ get{ return _strRowId; } set { _strRowId = value; } }

        public string strKey{ get { return _strKey; } set { _strKey = value; } }

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
            return this.GetUpdateData(false,false);
        }
        #endregion

        public bool AllowClose
        {
            get { return false; }
          
        }
        #endregion
        public frm_Model()
        {
            if(this.lb_Title == null)
                this.lb_Title = new XWinForm_Label();
            this.panel_main = new XWinForm_Panel();
            InitializeComponent();
            
        }
        
        public frm_Model(string skey)
        {
            strRowId = skey;
            InitializeComponent();
        }

        public List<UpdateData> InjectedDatas
        {
            get;set;
        }

        public DataSet InitDataSource(string sGridSource,out string msg)
        {
            msg = null;
            if (sGridSource == null || sGridSource.Trim().Length == 0)
            {
                msg = string.Format("数据源为空");
                return null; ;
            }
            return DataSource.InitDataSource(sGridSource, InitBaseConditions(), strUid, out msg);

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
            if (xmldoc == null) return ;
            XmlNode cmbNode = xmldoc.SelectSingleNode("/root/MainFrmComboSel/MainFrmCbx");
            if (cmbNode == null)
            {
                return;
            }
            string strPerm = XmlUtil.GetSubNodeText(cmbNode, "@perm");
            if (strPerm == "0") return;
            this.toolStrip1.Items.Add(new ToolStripLabel(XmlUtil.GetSubNodeText(cmbNode, "@caption")));
            ToolStripComboBox combobox = new ToolStripComboBox("查看");
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
            this.toolStrip1.Items.Add(combobox);
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
                ToolStripItem tsi;
                tsi = new ToolStripButton(mnu.MnuName);
                tsi.Tag = mnu;
                tsi.Click += new EventHandler(ToolBarBtn_Click);
                if (disable)
                    tsi.Enabled = false;
                if(this.toolStrip1.Items.Count > 0)
                    this.toolStrip1.Items.Add(new ToolStripSeparator());
                this.toolStrip1.Items.Add(tsi);
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
                 DataCondition.FillCondition(condnodes,ref cond);
            }
            ssearchbox.Tag = cond;
            searchbtn.Tag = ssearchbox;
            this.toolStrip1.Tag  = searchbtn;
            this.toolStrip1.Items.Add(ssearchbox);
            this.toolStrip1.Items.Add(searchbtn);
        }
        
        protected void frm_Model_Load(object sender, EventArgs e)
        {
            InitEvent();
            //
            if(strKey != null)//防止设计器里面加载loadtoolbar()
                LoadToolBar();
            //ToolBarBuilderWinForm tbbf = new ToolBarBuilderWinForm(this, this.toolStrip1);

            //tbbf.LoadToolBar();
            
        }
        
        void ssearchbox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ToolBar_OnSimpleSearchClicked(this.toolStrip1.Tag as ToolStripButton, null);
                return;
            }
        }

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
                    datacond.value = data.FromDataPoint;
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
            CMenuItem mnu = (sender as ToolStripButton).Tag as CMenuItem;
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
                        if (ToolBar_SaveClose())
                        {
                            ToolBar_RefreshData();
                        }
                        break;
                    }
                case "SaveNew":
                    {
                        ToolBar_SaveAndCreateNew();
                        break;
                    }
                case "New":
                    {
                        ToolBar_NewCreate(mnu);
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

        protected bool SaveClose_Click()
        {
            if (!CheckData())
            {
                return false;
            }
            if (Save())
            {

                //(this.TopLevelControl as Form).Close();
                Form frm = this.Parent as Form;
                if(frm == null) 
                    frm = this.TopLevelControl as Form;
                frm.DialogResult = DialogResult.Yes;
                frm.Close();


            }
            else
            {
                Form frm = this.Parent as Form;
                //MessageBox.Show("保存失败！");
                //frm.DialogResult = DialogResult.No;
                //frm.Close();
                return false;
            }
            return true;
        }

        protected void SaveNew_Click()
        {
            if (!CheckData())
            {
                return;
            }
            if (Save())
            {
                this.strRowId = "";
                LoadControls();
            }
            else
            {


                MessageBox.Show("保存失败！");
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
            Form frm = this.Parent as Form;
            frm.DialogResult = DialogResult.Yes;
            frm.Close();

            // }
            return;
        }

        #region IFrame 成员

        public virtual event EventHandler ToolBar_OnSimpleSearchClicked;

        public virtual event ToolBarHandle ToolBar_ListSelectedItemsClicked;

        public virtual event ToolBarHandle ToolBar_EditView;

        public virtual event ToolBarHandle ToolBar_PrintPDF;

        public virtual event ToolBarResponseHandle ToolBar_SaveClose;

        public virtual event ToolBarHandle ToolBar_SaveAndCreateNew;

        public virtual event AddExistHandle ToolBar_AddExist;

        public virtual event AddExistHandle ToolBar_NewCreate;

        public virtual event ToolBarHandle ToolBar_RefreshData;



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

        public virtual void SimpleSearch(object sender, EventArgs e) {}

        public virtual void RefreshData_Click(){}
        
        public virtual UpdateData GetUpdateData(bool JudgeValueChanged, bool UpdateFrameData)
        {
            UpdateData ret = new UpdateData();
            if (this.TranData == null) return ret;
            for (int i = 0; i < this.TranData.Count; i++)
            {
                if(GlobalShare.DataPointMappings.ContainsKey(this.TranData[i].ToDataPoint))
                {
                    if (!ret.Items.ContainsKey(this.TranData[i].ToDataPoint))
                    {
                        
                        ret.Items.Add(this.TranData[i].ToDataPoint, new UpdateItem(this.TranData[i].ToDataPoint,this.TranData[i].FromDataPoint));
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
        
        public virtual bool Save()
        {
            throw new Exception("The method or operation is not implemented.");
        }
        protected virtual XmlDocument GetConfigXml()
        {
            string strFilePath  = string.Format("{0}\\{1}\\frm_{1}_{2}_{3}.xml", "", strModule, strScreen, strTarget);
            return GlobalShare.UpdateWithUseInfo(GlobalShare.GetXmlFile(strFilePath),strUid) as XmlDocument;
        }

        protected XmlDocument GetConfigXml(string flag)
        {
            string strFilePath = string.Format("{0}\\{1}\\{4}_{1}_{2}_{3}.xml", "", strModule, strScreen, strTarget,flag);
            return GlobalShare.UpdateWithUseInfo(GlobalShare.GetXmlFile(strFilePath), strUid) as XmlDocument;
        }

        protected XmlDocument GetConfigXsl(string name)
        {
            string strFilePath = string.Format("{0}\\{1}\\xsl\\{2}", "", strModule,name);
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
            ToolBar_OkNoSave +=new ToolBarHandle(OkNoSave_Click);
            ToolBar_SaveClose +=new ToolBarResponseHandle(SaveClose_Click);
            ToolBar_SaveAndCreateNew +=new ToolBarHandle(SaveNew_Click);
            ToolBar_RefreshData += new ToolBarHandle(RefreshData_Click);
            ToolBar_OtherEvent += new AddExistHandle(ToolBar_OtherEvent_Click);
            ToolBar_ExportTo += new AddExistHandle(frm_Model_ToolBar_ExportTo);
        }

        public virtual void frm_Model_ToolBar_ExportTo(CMenuItem mnu)
        {
            throw new Exception("The method or operation is not implemented.");
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
        IXLabel Ifrm_Model.lb_Title { get; set; }
        IKeyForm ILink.Link { get; set; }

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
            msg = string.Format(msg, strModule, strScreen, strTarget, strKey, strRowId, GridSource, DetailSource,this.GetType());
            MessageBox.Show(msg);
        }

        public DataSet InitDataSource(string sGridSource, List<DataCondition> dc, out string msg)
        {
            throw new NotImplementedException();
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Dispose();
            
        }
    }

    public class ToolBarBuilderWinForm : BaseToolBarBuild
    {
        frm_Model frm;
        public ToolBarBuilderWinForm(IFrame ifrm,ITag itoolbar)
            : base(ifrm,itoolbar)
        {
            frm = ifrm as frm_Model;
            toolbarbld = new ToolBarBuilderItemForWin(ifrm, itoolbar);
        }


    }
    
    public class ToolBarBuilderItemForWin : ToolBarItemBuilder 
    {
        ToolBarStrip toolStrip1;
        frm_Model frmmdl;
        public ToolBarBuilderItemForWin(IFrame frm,ITag toolbar):base(frm,toolbar)
        {
            toolStrip1 = toolbar as ToolBarStrip;
            frmmdl = base.frmObj as frm_Model;
        }

        public override void InitToolBar(bool LeftToRight)
        {
            toolStrip1.Items.Clear();
            if (LeftToRight)
            {
                this.toolStrip1.RightToLeft = RightToLeft.Yes;
            }
            
        }

        public override ITag AddToolBarItem(CMenuItem mnu, ToolBarItemType type, EventHandler del)
        {
            ToolBarStripItem ret = null;
            switch(type)
            {
                case ToolBarItemType.Button:
                default:
                    {
                        ToolBarStripButton tsi = new ToolBarStripButton();
                        tsi.Text = mnu.MnuName;
                        tsi.Tag = mnu;
                        tsi.Click += del;
                        ret = tsi;
                        break;
                    }
            }
            this.toolStrip1.Items.Add(ret);
            return ret;
        }
        public override ITag AddToolBarItem(string lbl, ToolBarItemType type, EventHandler del)
        {
            return AddToolBarItem("", null, lbl, type, del);
        }
        public override ITag AddToolBarItem(string id,XmlNode backxml, string lbl, ToolBarItemType type, EventHandler del)
        {

            ToolBarStripItem ret = null;
            ret.Name = id;
            switch (type)
            {
                case ToolBarItemType.Separator:
                    {
                        
                        if (this.toolStrip1.Items.Count > 0)
                        {
                            ret = new ToolBarStripSeparator();
                        }
                        
                        break;
                    }
                case ToolBarItemType.Label:
                    {
                        this.toolStrip1.Items.Add(ret = new ToolBarStripLabel());
                        
                        ret.Text = lbl;
                        break;
                    }
                case ToolBarItemType.Button:
                    {
                        ToolBarStripButton searchbtn = new ToolBarStripButton();
                        (frmmdl.TopLevelControl as Form).AcceptButton = searchbtn as IButtonControl;
                        searchbtn.Text = lbl;
                        //this.toolStrip1.Items.Add(searchbtn);
                        searchbtn.Click += del;
                        ret = searchbtn;
                        break;
                    }
                case ToolBarItemType.Combox:
                    {
                        ToolBarStripComobox combo = new ToolBarStripComobox();
                        combo.Text = lbl;
                        //this.toolStrip1.Items.Add(combo);
                        ret = combo;
                        break;
                    }
                case ToolBarItemType.TextBox:
                    {
                        ToolStripTextBoxD ssearchbox = new ToolStripTextBoxD();
                        ret = ssearchbox;
                        break;
                    }
            }
            this.toolStrip1.Items.Add(ret);
 
            return ret;
        }
  
        public override ITag AddToolBarItem(XmlNode xml, ToolBarItemType type, string itemid, params EventHandler[] del)
        {
            return base.AddToolBarItem(xml, type, itemid, del);
        }
        
    }

    public abstract class WinFormHandle : BaseFormHandle 
    {
        protected WinFormHandle()
            : base()
        {
        }

        protected WinFormHandle(string id) : base(id) { }

        public override DataSet InitDataSource(string sGridSource, out string msg)
        {
            msg = null;
            if (sGridSource == null || sGridSource.Trim().Length == 0)
            {
                msg = string.Format("数据源为空");
                return null; ;
            }
            return DataSource.InitDataSource(GridSource, InitBaseConditions(), strUid, out msg);

        }

        ////public override DataSet InitDataSource(string sGridSource, List<DataCondition> dc,string id, out string msg)
        ////{
        ////    msg = null;
        ////    if (sGridSource == null || sGridSource.Trim().Length == 0)
        ////    {
        ////        msg = string.Format("数据源为空");
        ////        return null; ;
        ////    }
        ////    return DataSource.InitDataSource(GridSource, InitBaseConditions(), strUid, out msg);

        ////}
    }
}
