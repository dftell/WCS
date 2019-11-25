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
using WolfInv.Com.ExcelIOLib;
using WolfInv.Com.WCSExtraDataInterface;
using System.Linq;
using System.Reflection;
using System.Collections;
using Newtonsoft.Json.Linq;

namespace WolfInv.Com.CommFormCtrlLib
{


    //[DefaultEvent("RefreshData")]
    public partial class frm_Model : XWinForm_UserControl, Ifrm_Model,IMainFrame
    {
        public ListViewColumnSorter lvwColumnSorter;
        #region members
        bool _Loaded = false;
        protected bool LoadFlag { get { return _Loaded; } set { _Loaded = value; } }
        ToolBarBuilderWinForm BehFrame;
        public CMenuItem FromMenu { get; set; }
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
        public IXLabel lb_Title { get { return tlb_Title; } }
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

        public IXPanel CurrMainPanel
        {
            get
            {
                if(this.TopLevelControl is IMainFrame)
                {
                    return (this.TopLevelControl as IMainFrame).CurrMainPanel;
                }
                return null;
            }
        }

        public bool AllowClose
        {
            get { return false; }
          
        }
        #endregion
        public frm_Model()
        {
 
            this.panel_main = new XWinForm_Panel();
            InitializeComponent();
            
        }
        
        public frm_Model(string skey)
        {
            strRowId = skey;
            InitializeComponent();
        }

        

        public DataSet InitDataSource(string sGridSource,out string msg,ref bool isExtraData)
        {
            msg = null;
            if (sGridSource == null || sGridSource.Trim().Length == 0)
            {
                msg = string.Format("数据源为空");
                return null;
            }
            return DataSource.InitDataSource(sGridSource, InitBaseConditions(), strUid, out msg,ref isExtraData);

        }

        public virtual bool CheckData()
        {
            return false;
        }

        protected void LoadToolBar()
        {

            XmlDocument xmldoc = GetConfigXml();
            if (xmldoc == null)
                return;
            
            //AddGroupInToolBar(xmldoc);
            //AddButtonInToolBar(xmldoc);
            InitToolBarStrips(xmldoc.SelectSingleNode("/root/Action"), this.toolStrip1, ToolBar_Clicked, "Buttons");
            InitContextMenu(xmldoc.SelectSingleNode("/root/Grid"), this.contextMenuStrip1,this.ToolBarBtn_Click, "contextmenu");
            AddSimpleSearchInToolBar(xmldoc);
            AddComboInToolBar(xmldoc);
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
            if (strPerm == "0")
                return;
            
            ToolStripComboBox combobox = new ToolStripComboBox("查看");
            combobox.SelectedIndexChanged += new EventHandler(combobox_SelectedIndexChanged);
            List<CMenuItem> menus = new List<CMenuItem>();
            XmlDocument combodoc = new XmlDocument();
            try
            {
                combodoc.LoadXml(cmbNode.InnerXml);
            }
            catch(Exception ce)
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
            combobox.RightToLeft = RightToLeft.No;
            this.toolStrip1.Items.Add(combobox);
            this.toolStrip1.Items.Add(new ToolStripLabel(XmlUtil.GetSubNodeText(cmbNode, "@caption")));
            if (menus.Count > 0)
                combobox.SelectedIndex = 0;
        }

        protected void AddGroupInToolBar(XmlNode xmldoc,ToolStrip ts,string groupname="group")
        {
            if (xmldoc == null) return;
            XmlNode cmbNode = xmldoc.SelectSingleNode(groupname);
            if (cmbNode == null)
            {
                return;
            }
            string strPerm = XmlUtil.GetSubNodeText(cmbNode, "@perm");
            if (strPerm == "0") return;
            

            ToolStripComboBox combobox = new ToolStripComboBox("分组");


            combobox.SelectedIndexChanged += new EventHandler(combobox_SelectedIndexChanged);
            //ComboBoxEx cbe = new ComboBoxEx();
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
            combobox.RightToLeft = RightToLeft.No;
            ts.Items.Add(combobox);
            ts.Items.Add(new ToolStripLabel(XmlUtil.GetSubNodeText(cmbNode, "@caption")));
            //if (menus.Count > 0)
            //    combobox.SelectedIndex = 0;
        }

        

        protected virtual void combobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(sender == null)
            {
                return;
            }
            ToolStripComboBox tsc = sender as ToolStripComboBox;
            if (tsc == null)
                return;
            if (tsc.SelectedIndex < 0)
            {
                return;
            }
            List<CMenuItem> menus = tsc.Tag as List<CMenuItem>;
            ToolStripButton tmp = new ToolStripButton();
            tmp.Tag = menus[tsc.SelectedIndex];
            ToolBarBtn_Click(tmp,e);
        }
        
        void AddButtonInToolBar(ToolStrip tools, XmlDocument xmldoc)
        {
            if (xmldoc == null)
                return;
            XmlNodeList btnNodes = xmldoc.SelectNodes("/root/Action/Buttons/Button");
            if (btnNodes == null)
            {
                return;
            }
            foreach (XmlNode node in btnNodes)
            {
                CMenuItem mnu = new CMenuItem(strUid);
                string strPerm = XmlUtil.GetSubNodeText(node, "@perm");
                bool disable = false;
                if (strPerm == "0")
                    disable = true;
                
                XmlNode evtnode = node.SelectSingleNode("evt");
                if (evtnode == null)
                {
                    mnu.LoadXml(node);
                    mnu.MnuName = XmlUtil.GetSubNodeText(node, ".");
                    mnu.LinkValue = XmlUtil.GetSubNodeText(node, "@onclick");
                    mnu.MnuId = XmlUtil.GetSubNodeText(node, "@id");
                }
                else
                {
                    mnu = MenuProcess.GetMenu(null, evtnode, strUid);
                    mnu.LoadXml(evtnode);
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
            string strPerm = XmlUtil.GetSubNodeText(cmbNode, "@perm");
            if (strPerm == "0")
                return;
            if (this.toolStrip1.Items.Count>0)
                this.toolStrip1.Items.Add(new ToolStripSeparator());
            
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
            
            this.toolStrip1.Items.Add(searchbtn);
            this.toolStrip1.Items.Add(ssearchbox);
            this.toolStrip1.Items.Add(new ToolStripLabel(XmlUtil.GetSubNodeText(cmbNode, "@text")));
        }
        
        protected void frm_Model_Load(object sender, EventArgs e)
        {
            if (FromMenu != null && (FromMenu.linkType == LinkType.Dialog|| FromMenu.linkType == LinkType.PrintToPDF))
            {
                this.btn_close.Visible = false;
                if (this.CurrMainPanel == null)
                {

                }
                else
                {
                    if (this.CurrMainPanel.InForm)
                    {
                        this.btn_close.Visible = false;
                    }
                    else
                    {
                        this.btn_close.Visible = true;
                    }
                }
                
            }
            else
            {
                
            }
            InitEvent();
            LoadControls();
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
                    datacond.value =  data.FromDataPoint.Text;
                    if(this is ITranslateableInterFace)
                    {
                        if((this as ITranslateableInterFace).NeedUpdateData.Items.ContainsKey(data.FromDataPoint.Name))//直接兑现
                        {
                            datacond.value = (this as ITranslateableInterFace).NeedUpdateData.Items[data.FromDataPoint.Name].value;
                            datacond.Datapoint = new DataPoint(data.ToDataPoint);
                            conds.Add(datacond);
                            continue;
                        }
                    }
                    if (GlobalShare.DataPointMappings.ContainsKey(data.FromDataPoint.Name) && string.IsNullOrEmpty(datacond.value))
                    {
                        continue;
                    }
                    string val = null;
                    if (GlobalShare.IsSystemParam(data.FromDataPoint.Name, out val))
                    {
                        datacond.value = val;
                        datacond.Datapoint = new DataPoint(data.ToDataPoint);
                        conds.Add(datacond);
                        continue;
                    }
                    else
                    {
                        datacond.value = data.FromDataPoint.Name;
                    }
                    
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
            CMenuItem mnu = (sender as ToolStripItem).Tag as CMenuItem;
            if (mnu == null) return;
            if(mnu.NeedNotice)
            {
                if(MessageBox.Show(mnu.NoticeContent,mnu.NoticeTitle,MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
            }

            try
            {
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
                    case "ChangeGroup":
                        {
                            if(ToolBar_ChangeGroup(mnu))
                            {
                                ToolBar_RefreshData();
                            }
                            break;
                        }
                    case "btn_Ssearch":
                    case "combo_Selected":
                    case "FilterExtraData":
                        {
                            ToolBar_FilterExtraData(mnu);
                            break;
                        }
                    case "SaveNew":
                        {
                            ToolBar_SaveAndCreateNew(mnu);
                            break;
                        }
                    case "New":
                        {
                            DataComboBox.ClearRunningTimeDataSource();
                            ToolBar_NewCreate(mnu);
                            if (ToolBar_RefreshData != null)
                            {
                                ToolStripComboBox tcb = this.toolStrip1.Items["查看"] as ToolStripComboBox;
                                combobox_SelectedIndexChanged(tcb, null);
                                //ToolBar_RefreshData();
                            }
                            break;
                        }
                    case "ExportExcel":
                        {
                            ToolBar_Export();
                            break;
                        }
                    case "Sync":
                        {
                            ToolBar_Sync(mnu);
                            break;
                        }
                    case "Remove":
                        {
                            ToolBar_Remove(mnu);
                            break;
                        }
                    case "Import":
                        {
                            DataComboBox.ClearRunningTimeDataSource();
                            ToolBar_Import(mnu);
                            break;
                        }
                    case "PrintToPDF":
                        {
                            ToolBar_PrintPDF(mnu);
                            break;
                        }
                    case "Refresh":

                        {
                            DataComboBox.ClearRunningTimeDataSource();
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
                    case "BatchUpdate":
                        {
                            ToolBar_BatchUpdate(mnu);
                            break;
                        }
                    default:
                        {
                            ToolBar_OtherEvent(mnu);
                            break;
                        }
                }
            }
            catch(Exception ce)
            {
                MessageBox.Show(ce.Message);
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
                ////Form frm = this.Parent as Form;
                ////if(frm == null) 
                ////    frm = this.TopLevelControl as Form;
                ////frm.DialogResult = DialogResult.Yes;
                ////frm.Close();
                if(this.FromMenu.linkType == LinkType.Dialog)
                {
                    this.TopLevelControl.Dispose();
                    return true;
                }
                btn_close_Click(null, null);


            }
            else
            {
                Form frm = this.Parent as Form;
                //MessageBox.Show("保存失败！");
                //frm.DialogResult = DialogResult.No;
                //frm.Close();
                return false;
            }
            return false;
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
                //LoadControls();
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

        public virtual event AddExistHandle ToolBar_PrintPDF;

        public virtual event AddExistHandle ToolBar_FilterExtraData;

        public virtual event ToolBarResponseHandle ToolBar_SaveClose;

        public virtual event ToolBarResponseHandle ToolBar_ChangeGroup;

        public virtual event ToolBarResponseHandle ToolBar_BatchUpdate;

        public virtual event AddExistHandle ToolBar_SaveAndCreateNew;

        public virtual event AddExistHandle ToolBar_AddExist;

        public virtual event AddExistHandle ToolBar_NewCreate;

        public virtual event ToolBarResponseHandle ToolBar_Remove;

        public virtual event ToolBarHandle ToolBar_RefreshData;

        public virtual event AddExistHandle ToolBar_Sync;

        public virtual event AddExistHandle ToolBar_Import;

        public virtual event ToolBarHandle ToolBar_Export;

        public virtual event ToolBarHandle ToolBar_OkNoSave;

        public virtual event EventHandler ToolBar_Clicked;

        public virtual event AddExistHandle ToolBar_SaveAs;

        public virtual event AddExistHandle ToolBar_OtherEvent;

        public virtual event AddExistHandle ToolBar_ExportTo;
        public event AddExistHandle ToolBar_EditMappings;

        public event ToolBarHandle ToolBar_SelectAll;
        public event ToolBarHandle ToolBar_InSelect;

        public virtual void SimpleSearch(object sender, EventArgs e)
        {
        }

        public virtual void RefreshData_Click(){}
        
        public virtual UpdateData GetUpdateData(bool JudgeValueChanged, bool UpdateFrameData,bool getText=false)
        {
            UpdateData ret = new UpdateData();
            if (this.TranData == null) return ret;
            for (int i = 0; i < this.TranData.Count; i++)
            {
                if(GlobalShare.DataPointMappings.ContainsKey(this.TranData[i].ToDataPoint))
                {
                    if (!ret.Items.ContainsKey(this.TranData[i].ToDataPoint))
                    {
                        
                        ret.Items.Add(this.TranData[i].ToDataPoint, new UpdateItem(this.TranData[i].ToDataPoint,this.TranData[i].FromDataPoint.Name));
                    }
                }
            }
            return ret;
            //throw new Exception("The method or operation is not implemented.");
        }

        public virtual UpdateData GetUpdateData(bool JudgeValueChanged)
        {
            return GetUpdateData(false, false,true);
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
                FrameSwitch.switchToView(this.CurrMainPanel, this, mnu);
            }
                        
        }
        
        //public bool SyncData(CMenuItem mnu)
        //{
        //    if (mnu.extradataclass.Trim().Length == 0 || mnu.extradataconvertconfig == null || mnu.extradatagetconfig == null || mnu.extradatatype.Trim().Length == 0)
        //    {
        //        MessageBox.Show("未正确配置同步参数");
        //        return false;
        //    }
        //    WCSExtraDataClass edc = new WCSExtraDataClass(mnu.extradataassembly,mnu.extradataclass,mnu.extradatatype, mnu.extradatagetconfig);
        //    XmlDocument xmldoc = null;
        //    XmlDocument xmlschema = null;
        //    bool succ = edc.getExtraData(ref xmldoc,ref xmlschema);
        //    if(!succ)
        //    {
        //        MessageBox.Show("获取外部数据失败");
        //        return false;
        //    }
        //    return true;
                
        //}


        ////public XmlDocument getExtraData(CMenuItem mnu)
        ////{
        ////    if (mnu.extradataclass.Trim().Length == 0 || mnu.extradataconvertconfig == null || mnu.extradatagetconfig == null || mnu.extradataassembly.Trim().Length == 0)
        ////    {
        ////        MessageBox.Show("未正确配置同步参数");
        ////        return null;
        ////    }
        ////    WCSExtraDataClass edc = new WCSExtraDataClass(mnu.extradataassembly, mnu.extradataclass, mnu.extradatatype, mnu.extradatagetconfig);
        ////    XmlDocument xmldoc = null;
        ////    XmlDocument xmlschema = null;
        ////    bool succ = edc.getExtraData(ref xmldoc,ref xmlschema);
        ////    if (!succ)
        ////    {
        ////        MessageBox.Show("获取外部数据失败");
        ////        return null;
        ////    }
        ////    return xmldoc;
        ////}

        


        public virtual bool Save(CMenuItem mnu)
        {
            return true;
        }

        public virtual bool SaveData(CMenuItem mnu,DataRequestType type = DataRequestType.Update)
        {
            
            int cnt = 0;
            UpdateData org_updata = this.GetUpdateData(false);//获得修改后的数据 
            bool orgstatus = org_updata.Updated;
            DataRequestType rt = type;
            if(string.IsNullOrEmpty(org_updata.keyvalue))
            {
                rt = DataRequestType.Add;
            }
            org_updata.ReqType = rt;
            List<UpdateData> savedatas = new List<UpdateData>();
            if(mnu == null)//直接保存
            {
                return SaveClientData(org_updata, rt);
                return true;
            }
            if (mnu.GroupBeforeSave)
            {
                org_updata = org_updata.getGroupData(true, mnu.GridGroupBy);
                savedatas = org_updata.SubItems;
                savedatas.ForEach(a => { a.Updated = orgstatus; a.ReqType = rt; });
            }
            else
            {
                savedatas.Add(org_updata);
            }
            cnt = 0;
            foreach (UpdateData updata in savedatas)
            {

                if (!updata.Updated)
                    return false;
                //updata.ReqType = DataRequestType.Update;
                //if (updata.keydpt != null && updata.keydpt.Name.Trim().Length > 0)
                //{
                //    if (updata.keyvalue == null || updata.keyvalue.Trim().Length == 0)
                //        updata.ReqType = DataRequestType.Add;
                //}
                if (mnu == null && updata.SubItems != null && updata.SubItems.Count > 0)
                {
                    MessageBox.Show("外部数据，未指定保存事件，不能删除子数据！");
                    return false;
                }
                if (mnu == null)//如果没有子事件，直接保存到本地
                {
                    return SaveClientData(updata, type);
                }
                CMenuItem useMenu = mnu;
                if (mnu.evt != null)
                {
                    useMenu = mnu.evt;
                }
                string extramsg = "";
                if (useMenu.isextradata)
                {
                    UpdateData ud = null;
                    bool succ = SaveExtraData(useMenu, updata, ref ud);
                    if (!succ)
                    {
                        if (DialogResult.Cancel == MessageBox.Show( string.Format("保存数据{0}时遇到错误.点确定按钮，本次将跳过错误内容，请您记录关键内容，下次单独存储！点取消按钮，本次保存将停止，已经保存的数据请您在可删除的界面手动删除！", string.Join(",", updata.Items.Select(a => a.Value.value)), ""), "跳过错误内容，继续保存", MessageBoxButtons.YesNoCancel))
                        {
                            return false;
                        }
                        continue;// return false;
                    }
                    if (!updata.Items.ContainsKey(updata.keydpt.Name))
                    {
                        updata.Items.Add(updata.keydpt.Name, null);
                    }
                    updata.Items[updata.keydpt.Name] = ud.Items[updata.keydpt.Name];
                    updata.keyvalue = ud.Items[updata.keydpt.Name].value;
                    if (this.FromMenu.TranDataMapping != null)
                    {

                        this.FromMenu.TranDataMapping.ForEach(a =>
                        {
                            if (a.FromDataPoint.Name.Equals(updata.keydpt.Name))
                            {
                                a.FromDataPoint.Text = updata.keyvalue;
                            }
                        });
                    }
                    if (updata.SubItems != null && updata.SubItems.Count > 0)//不传吧
                    {
                        if (this.FromMenu.TranDataMapping != null)
                        {

                            updata.SubItems.ForEach(a =>
                            {
                                this.FromMenu.TranDataMapping.ForEach(b =>
                                {
                                    if (b.FromDataPoint.Text != null && b.FromDataPoint.Text.Trim().Length > 0)
                                    {
                                        if (a.Items.ContainsKey(b.ToDataPoint))
                                        {
                                            a.Items[b.ToDataPoint].value = b.FromDataPoint.Text;
                                        }
                                    }
                                });
                            });
                        }
                    }
                    extramsg = "[外部数据已保存,请删除外部数据！]";
                    this.label_buttom.Text = string.Format("共计{0}组数据需要保存，正在保存第{1}组！", savedatas.Count,++cnt);
                    this.label_buttom.Refresh();
                }
                if (useMenu.extradatanosaveinclient)
                {
                    continue;
                }
                bool suc = SaveClientData(updata, type);
                if (!suc)
                {
                    MessageBox.Show(string.Format("保存本地数据错误！{0}", extramsg));
                    return false;
                }
            }
            return true;
        }

        public virtual bool SaveExtraData(CMenuItem mnu ,UpdateData data,ref UpdateData ret)//保存外部数据
        {
            if(mnu.extradataconvertconfig == null||mnu.extradatagetconfig == null || mnu.extradataclass.Trim().Length == 0 || mnu.extradataassembly.Trim().Length == 0)
            {
                MessageBox.Show("请正确配置事件！");
                return false;
            }
            //data = GetUpdateData(false);
            WCSExtraDataAdapter wda = new WCSExtraDataAdapter(this.strUid,mnu.extradataconvertconfig,true);
            DataSet ds = null;
            List<UpdateData> ul = new List<UpdateData>();
            ul.Add(data);
            ds = DataSource.UpdateData2DataSet(ul, ref ds, data.keydpt.Name,data.keyvalue);
            string msg = null;
            DataSet dret = null;
            bool succ = wda.writeData(mnu.extradataassembly, mnu.extradataclass, mnu.extradatagetconfig, mnu.extradatatype, data.ReqType.ToString(), ds,ref dret, ref msg);
            if(!succ)
            {
                MessageBox.Show(msg);
                return false;
            }
            ul = DataSource.DataSet2UpdateData(dret, this.DetailSource, this.strUid);
            if(ul==null ||(ul!= null&& ul.Count!=1))
            {
                MessageBox.Show("返回数据错误");
                return false;
            }
            ret = ul[0];
            return true;
        }

        public virtual bool SaveClientData(UpdateData updata,DataRequestType type=DataRequestType.Update)
        {
            if (!updata.Updated) return true;
            DataSource ds = GlobalShare.mapDataSource[DetailSource];
            List<DataCondition> conds = new List<DataCondition>();
            DataCondition dcond = new DataCondition();
            dcond.Datapoint = new DataPoint(this.strKey);
            dcond.value = this.strRowId;
            conds.Add(dcond);
            //DataRequestType type = DataRequestType.Update;
            if (this.strRowId == null || this.strRowId == "")
            {
                type = DataRequestType.Add;
            }
            if (GlobalShare.mapDataSource.ContainsKey(this.GridSource))
            {
                DataSource grid_source = GlobalShare.mapDataSource[this.GridSource];
                ds.SubSource = grid_source;
            }
            //updata.SubItems.Where(a=>a.ReqType)
            string msg = GlobalShare.DataCenterClientObj.UpdateDataList(ds, dcond, updata, type);
            if (msg != null)
            {
                MessageBox.Show(msg);
                return false;
            }
            return true;
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
            ToolBar_SaveAndCreateNew +=new AddExistHandle(SaveNew_Click);
            ToolBar_RefreshData += new ToolBarHandle(RefreshData_Click);
            ToolBar_OtherEvent += new AddExistHandle(ToolBar_OtherEvent_Click);
            ToolBar_ExportTo += new AddExistHandle(frm_Model_ToolBar_ExportTo);
            ToolBar_Import += new AddExistHandle(Frm_Model_ToolBar_Import);
            ToolBar_FilterExtraData += Frm_Model_ToolBar_FilterExtraData;
            ToolBar_PrintPDF += PrintPDF;
        }

        private void Frm_Model_ToolBar_FilterExtraData(CMenuItem mnu)
        {
            if(mnu == null)
            {
                return;
            }
            DataTranMappings dtm = new DataTranMappings();
            if(this.TranData!= null)
                dtm.AddRange(this.TranData);
            if (mnu.TranDataMapping == null || mnu.TranDataMapping.Count == 0)
            {
                this.TranData = null;
                this.ToolBar_RefreshData();
                this.TranData = dtm;
                return;
            }
            
            
            mnu.TranDataMapping.ForEach(
                a =>
                {
                    if(dtm.AllTo.ContainsKey(a.ToDataPoint))
                    {
                        // = dtm.AllTo[a.ToDataPoint].FromDataPoint.Text;
                    }
                    else
                    {
                        if (this.TranData == null)
                            this.TranData = new List<DataTranMapping>();
                        this.TranData.Add(a);
                    }
                }
                );
            this.ToolBar_RefreshData();
            this.TranData = dtm;
        }


        protected virtual void PrintPDF(CMenuItem mnu)
        {
            
            UpdateData ret = null;
            UpdateData ud = null;
            ud = GetUpdateData(false, false, true);
            FrameSwitch.switchToView(this.panel_main,null,mnu,ref ret, new UpdateData[1] {ud }.ToList());
        }

        private void Frm_Model_ToolBar_Import(CMenuItem mnu)
        {

            ImportData(mnu);


        }

        protected void ImportData(CMenuItem mnu)
        {
            if (mnu.extradatagetconfig == null)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show("导入Excel文件配置错误！");
                return;
            }
            UpdateData MainData = GetUpdateData(false);
            this.NeedUpdateData = MainData;
            if (mnu.TranDataMapping != null)//如果有传入值
            {
                int mcnt = 0;
                for(int i=0;i<mnu.TranDataMapping.Count;i++)
                {
                    if (MainData == null)
                        continue;
                    string tname = mnu.TranDataMapping[i].FromDataPoint.Name;
                    if(MainData.Items.ContainsKey(tname))//传输字段在数据中
                    {
                        if(MainData.Items[tname].value == null || MainData.Items[tname].value.Trim().Length == 0)//制订了传输的值为空，跳过
                        {
                            continue;
                        }
                        mnu.TranDataMapping[i].FromDataPoint.Text = MainData.Items[tname].value;
                    }
                    else
                    {
                        
                        string val = null;
                        if(GlobalShare.IsSystemParam(mnu.TranDataMapping[i].FromDataPoint.Name, out val))
                        {
                            mnu.TranDataMapping[i].FromDataPoint.Text = val;
                        }
                        else//常数，不支持计算，拼接等
                        {
                            
                        }
                    }
                    mcnt++;
                }
                //if (mcnt < mnu.TranDataMapping.Count)
                //{
                    
                //    MessageBox.Show("请先指定传输值！");
                //    return;
                //}
            }
            DataComboBox.ClearRunningTimeDataSource();
            OpenExcelSheetDialog ofd = new OpenExcelSheetDialog();
            DialogResult res = ofd.ShowDialog(this);
            this.Cursor = Cursors.WaitCursor;
            if ( res == DialogResult.OK)
            {
                
                XmlUtil.AddAttribute(mnu.extradatagetconfig, "excelpath", ofd.ExcelFileName);
                XmlUtil.AddAttribute(mnu.extradatagetconfig, "excelsheet", ofd.SheetName);
                WCSExtraDataAdapter wa = new WCSExtraDataAdapter(this.strUid, mnu.extradataconvertconfig);
                string msg = null;
                DataSet ds = null;
                bool succ = wa.getData(mnu.extradataassembly, mnu.extradataclass, mnu.extradatagetconfig, mnu.extradatatype, ref ds,ref msg);
                if (succ == false)
                {
                    this.Cursor = Cursors.Default;
                    MessageBox.Show(msg);
                    return;
                }
                else
                {
                    if(msg!=null)//如果非空，提示！
                    {
                        //MessageBox.Show(msg);
                    }
                }
                List<DataControlItem> attCols = new List<DataControlItem>();
                if(mnu.attatchinfo!=null)
                {
                    XmlNodeList attlist = mnu.attatchinfo.SelectNodes("./cols/f");
                    foreach(XmlNode anode in attlist)
                    {
                        DataControlItem dp = new DataControlItem();
                        dp.LoadXml(anode);
                        attCols.Add(dp);
                    }
                }
                List<UpdateData> InjectData = DataSource.DataSet2UpdateData(ds,GridSource, this.strUid);
               
                if(this is ITranslateableInterFace)
                {
                    for(int i=0;i< InjectData.Count;i++)
                    {
                        UpdateData ud = InjectData[i];
                        
                        ud.ReqType = DataRequestType.Add;
                        ud.IsImported = true;
                        ud.Updated = true;
                        if (mnu.TranDataMapping != null)
                        {
                            for(int j=0;j<mnu.TranDataMapping.Count;j++)
                            {
                                string from = mnu.TranDataMapping[j].FromDataPoint.Text;
                                string to = mnu.TranDataMapping[j].ToDataPoint;
                                if(ud.Items.ContainsKey(to))
                                {
                                    ud.Items[to].value = from;
                                }
                                else
                                {
                                    ud.Items.Add(to,new UpdateItem(to, from)) ;
                                }
                            }
                        }
                        for (int j=0;j<attCols.Count;j++)
                        {
                            string attname = attCols[j].Name;
                            string val = attCols[j].getValue(this.strUid, ud);
                            if (ud.Items.ContainsKey(attname))
                            {
                                ud.Items[attname].value = val;
                            }
                            else
                            {
                                ud.Items.Add(attname, new UpdateItem(attname, val));
                            }
                        }
                        //InjectData.Add(ud);
                    }
                    //this.NeedUpdateData = ud;
                }
                CMenuItem newMenu = this.FromMenu;
                UpdateData mydata = null;
                FrameSwitch.switchToView(this.Parent as IXContainerControl,this,newMenu,ref mydata, InjectData);
                this.Cursor = Cursors.Default;
                MessageBox.Show("导入成功！");
            }
            this.Cursor = Cursors.Default;
        }

        protected void lv_CloumnClick(ListView listView1,ColumnClickEventArgs e)
        {
            // Determine if clicked column is already the column that is being sorted.
            if (lvwColumnSorter == null)
                lvwColumnSorter = new ListViewColumnSorter();
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (lvwColumnSorter.Order == SortOrder.Ascending)
                {
                    lvwColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            //this.listView1.Columns

            listView1.ListViewItemSorter = lvwColumnSorter;
            listView1.Sort();
            if (listView1.Items.Count > 0)
            {
                listView1.EnsureVisible(0);
                listView1.Items[0].Selected = true;
            }
        }

        protected virtual void FillGridData(DataSet ds,string name)
        {

        }

        protected virtual void FillGridData(List<UpdateData> ds)
        {

        }

        protected void InitToolBarStrips(XmlNode cmbNode, ToolStrip trip, EventHandler e,string strToolKey= "toolbar")
        {
            //ToolStripLabel lb = new ToolStripLabel(XmlUtil.GetSubNodeText(cmbNode,"@title") );
            //ToolBar.Items.Add(lb);
            XmlNode node = cmbNode.SelectSingleNode(strToolKey);
            if (node == null) return;
            //trip.Items.Clear();
            trip.RightToLeft = XmlUtil.GetSubNodeText(cmbNode, "@RightToLeft") == "0" ? RightToLeft.No : RightToLeft.Yes;
            InitButtons(node,trip, trip.Items, e, "button", RightToLeft.Yes);
        }


        protected void InitContextMenu(XmlNode cmbNode, ToolStrip trip, EventHandler e, string strToolKey = "contextmenu")
        {
            //ToolStripLabel lb = new ToolStripLabel(XmlUtil.GetSubNodeText(cmbNode,"@title") );
            //ToolBar.Items.Add(lb);
            if (cmbNode == null)
                return;
            XmlNode node = cmbNode.SelectSingleNode(strToolKey);
            if (node == null)
                return;
            trip.Items.Clear();
            //trip.RightToLeft = XmlUtil.GetSubNodeText(cmbNode, "@RightToLeft") == "0" ? RightToLeft.No : RightToLeft.Yes;
            InitButtons(node,trip, trip.Items, e, "menu",RightToLeft.No);
        }

        void InitButtons(XmlNode node,ToolStrip topctrl, ToolStripItemCollection tsic, EventHandler e,string nodename="button", RightToLeft RtL=RightToLeft.Yes)
        {
            string btnkey = "button";
            if (nodename!=null)
            {
                btnkey = nodename;
            }
            
            XmlNodeList nodelist = node.SelectNodes(btnkey);
            if(nodelist.Count == 0)
            {
                btnkey = "Button";
                nodelist = node.SelectNodes(btnkey); 
            }
            if (nodelist.Count == 0)
            {
                btnkey = "menu";
                nodelist = node.SelectNodes(btnkey);
            }
            foreach (XmlNode bnode in nodelist)
            {

                ToolStripItem btn = null;
                bool isDdb = false;
                if (bnode.SelectNodes(btnkey).Count > 0)
                {
                    if (topctrl is ContextMenuStrip)
                    {
                        btn = new ToolStripMenuItem();
                    }
                    else
                    {
                        btn = new ToolStripMenuItem();
                    }
                    btn.RightToLeft = RightToLeft.No;
                    isDdb = true;
                }
                else
                {
                    if (topctrl is ContextMenuStrip)
                    {
                        btn = new ToolStripMenuItem();
                    }
                    else
                    {
                        btn = new ToolStripButton();
                        
                    }
                    btn.Click += new EventHandler(e);
                }

                string sPerm = XmlUtil.GetSubNodeText(bnode, "@perm");

                CMenuItem mnu = MenuProcess.GetMenu(null, bnode, strUid);
                mnu.LoadXml(bnode);
                btn.Name = mnu.MnuId;
                btn.Text = mnu.MnuName;
                btn.Tag = mnu;
                btn.AutoSize = true;
                btn.Enabled = !(sPerm == "0");
                if(mnu.OnlyKeyDisplay && string.IsNullOrEmpty(this.strRowId))
                {
                    btn.Enabled = false;
                }
                if (mnu.OnlyNoKeyDisplay && !string.IsNullOrEmpty(this.strRowId))
                {
                    btn.Enabled = false;
                }
                //btn.RightToLeft = RtL;
                tsic.Add(btn);
                if (isDdb)
                {
                    InitButtons(bnode,topctrl, (btn as ToolStripDropDownItem).DropDownItems,e, btnkey, RtL);
                }
            }
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
        //IXLabel Ifrm_Model.lb_Title { get; }
        IKeyForm ILink.Link { get; set; }
        public bool UseSubItems { get; set; }

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
            ////this.Dock = this.Dock == DockStyle.Fill ?DockStyle.None: DockStyle.Fill;
            ////if (this.Dock != DockStyle.Fill)
            ////    this.BorderStyle = BorderStyle.Fixed3D;
            ////else
            ////    this.BorderStyle = BorderStyle.None;
            //MessageBox.Show(msg);
        }

        public DataSet InitDataSource(string sGridSource, List<DataCondition> dc, out string msg)
        {
            throw new NotImplementedException();
        }

        public DataSet InitDataSource(string sGridSource, out string msg)
        {
            bool isextra = false;
            return InitDataSource(sGridSource, out msg, ref isextra);
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            if(this.FromMenu!= null)
            {
                this.panel_main.CurrMainControl = null;
                if(FrameSwitch.AllModules.ContainsKey(this.FromMenu.MnuId))
                    FrameSwitch.AllModules.Remove(this.FromMenu.MnuId);
            }
            this.Dispose();
            
        }

        protected bool ChangeGridGroup(Grid gd,CMenuItem mnu)
        {
            if (gd == null)
                return false;
            if (mnu == null)
                return false;
            ListView lv = gd.listViewObj as ListView;
            if (lv == null)
                return false;
            if (mnu.AllowCheckedMultiItems)
            {
                if(lv.CheckedItems.Count>0)
                {
                    MessageBox.Show("只允许操作单选项！");
                    return false;
                }
            }
            if(gd.GroupBy == mnu.GridGroupBy)
            {
                return true;
            }
            //gd.AllowGroup = true;
            //gd.GroupBy = mnu.GridGroupBy;
            UpdateData ud =   GetUpdateData(false, false, false);
            //ud = ud.getGroupData(true, mnu.GridGroupBy, null, false);
            gd.GroupBy = mnu.GridGroupBy;
            gd.listViewObj.GroupBy = mnu.GridGroupBy;
            FillGridData(ud.SubItems);
            return true;
        }

        protected virtual bool BatchUpdate(Grid gd,CMenuItem mnu)
        {
            return true;
        }
    }

    public class ComboBoxEx : ToolStripComboBox
    {
        TreeView lst = new TreeView();

        public ComboBoxEx()
        {
            //this.DrawMode = DrawMode.OwnerDrawFixed;//只有设置这个属性为OwnerDrawFixed才可能让重画起作用
            lst.KeyUp += new KeyEventHandler(lst_KeyUp);
            lst.MouseUp += new MouseEventHandler(lst_MouseUp);
            // lst.KeyDown += new KeyEventHandler(lst_KeyDown);
            lst.Leave += new EventHandler(lst_Leave);
            lst.CheckBoxes = true;
            lst.ShowLines = false;
            lst.ShowPlusMinus = false;
            lst.ShowRootLines = false;
            this.DropDownHeight = 1;
        }

        void lst_Leave(object sender, EventArgs e)
        {
            lst.Hide();
        }
        #region Property

        [Description("选定项的值"), Category("Data")]
        public List<TreeNode> SelectedItems
        {
            get
            {
                List<TreeNode> lsttn = new List<TreeNode>();
                foreach (TreeNode tn in lst.Nodes)
                {
                    if (tn.Checked)
                    {
                        lsttn.Add(tn);
                    }
                }
                return lsttn;
            }
        }

        /// <summary>
        /// 数据源
        /// </summary>
        [Description("数据源"), Category("Data")]
        public object DataSource
        {
            get;
            set;
        }
        /// <summary>
        /// 显示字段
        /// </summary>
        [Description("显示字段"), Category("Data")]
        public string DisplayFiled
        {
            get;
            set;
        }
        /// <summary>
        /// 值字段
        /// </summary>
        [Description("值字段"), Category("Data")]
        public string ValueFiled
        {
            get;
            set;
        }
        #endregion


        public void DataBind()
        {
            this.BeginUpdate();
            if (DataSource != null)
            {
                if (DataSource is IDataReader)
                {
                    DataTable dataTable = new DataTable();
                    dataTable.Load(DataSource as IDataReader);

                    DataBindToDataTable(dataTable);
                }
                if(DataSource is XmlNode)
                {
                    DataBindToDataTable(DataSource as XmlNode);
                }
                else if (DataSource is DataView || DataSource is DataSet || DataSource is DataTable)
                {
                    DataTable dataTable = null;

                    if (DataSource is DataView)
                    {
                        dataTable = ((DataView)DataSource).ToTable();
                    }
                    else if (DataSource is DataSet)
                    {
                        dataTable = ((DataSet)DataSource).Tables[0];
                    }
                    else
                    {
                        dataTable = ((DataTable)DataSource);
                    }

                    DataBindToDataTable(dataTable);
                }
                else if (DataSource is IEnumerable)
                {
                    DataBindToEnumerable((IEnumerable)DataSource);
                }
                else
                {
                    throw new Exception("DataSource doesn't support data type: " + DataSource.GetType().ToString());
                }
            }
            else
            {
                lst.Nodes.Clear();
            }

            lst.ItemHeight = this.DropDownHeight;
            lst.BorderStyle = BorderStyle.FixedSingle;
            lst.Size = new Size(this.Width, this.Height * (this.MaxDropDownItems - 1) - (int)this.Height / 2);
            //lst.Location = new Point(this.Left, this.Top + this.DropDownHeight + 6);
            this.Parent.Controls.Add(lst);
            lst.Hide();
            this.EndUpdate();
        }


        private void DataBindToDataTable(XmlNode  node)
        {
            lst.Nodes.Clear();
            XmlNodeList dt = node.SelectNodes("item");
            foreach (XmlNode dr in dt)
            {
                TreeNode tn = new TreeNode();
                tn.Text = XmlUtil.GetSubNodeText(dr, "@text");
                tn.Tag = XmlUtil.GetSubNodeText(dr, "@point");

                tn.Checked = false;
                lst.Nodes.Add(tn);
            }
        }

        private void DataBindToDataTable(DataTable dt)
        {
            foreach (DataRow dr in dt.Rows)
            {
                TreeNode tn = new TreeNode();
                if (!string.IsNullOrEmpty(DisplayFiled) && !string.IsNullOrEmpty(ValueFiled))
                {
                    tn.Text = dr[DisplayFiled].ToString();
                    tn.Tag = dr[ValueFiled].ToString();
                }
                else if (string.IsNullOrEmpty(ValueFiled))
                {
                    tn.Text = dr[DisplayFiled].ToString();
                    tn.Tag = dr[DisplayFiled].ToString();
                }
                else if (string.IsNullOrEmpty(DisplayFiled))
                {
                    tn.Text = dr[ValueFiled].ToString();
                    tn.Tag = dr[ValueFiled].ToString();
                }
                else
                {
                    throw new Exception("ValueFiled和DisplayFiled至少保证有一项有值");
                }

                tn.Checked = false;
                lst.Nodes.Add(tn);
            }
        }

        /// <summary>
        /// 绑定到可枚举类型
        /// </summary>
        /// <param name="enumerable">可枚举类型</param>
        private void DataBindToEnumerable(IEnumerable enumerable)
        {
            IEnumerator enumerator = enumerable.GetEnumerator();
            while (enumerator.MoveNext())
            {
                object currentObject = enumerator.Current;
                lst.Nodes.Add(CreateListItem(currentObject));
            }
        }



        private TreeNode CreateListItem(Object obj)
        {
            TreeNode item = new TreeNode();

            if (obj is string)
            {
                item.Text = obj.ToString();
                item.Tag = obj.ToString();
            }
            else
            {
                if (DisplayFiled != "")
                {
                    item.Text = GetPropertyValue(obj, DisplayFiled);
                }
                else
                {
                    item.Text = obj.ToString();
                }

                if (ValueFiled != "")
                {
                    item.Tag = GetPropertyValue(obj, ValueFiled);
                }
                else
                {
                    item.Tag = obj.ToString();
                }
            }
            return item;
        }


        private string GetPropertyValue(object obj, string propertyName)
        {
            object result = null;

            result = ObjectUtil.GetPropertyValue(obj, propertyName);
            return result == null ? String.Empty : result.ToString();
        }

        #region override


        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            bool Pressed = (e.Control && ((e.KeyData & Keys.A) == Keys.A));
            if (Pressed)
            {
                this.Text = "";
                for (int i = 0; i < lst.Nodes.Count; i++)
                {
                    lst.Nodes[i].Checked = true;
                    if (this.Text != "")
                    {
                        this.Text += ",";
                    }
                    this.Text += lst.Nodes[i].Tag;
                }
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            this.DroppedDown = false;

        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            this.DroppedDown = false;
            lst.Focus();
        }

        protected override void OnDropDown(EventArgs e)
        {
            string strValue = this.Text;
            if (!string.IsNullOrEmpty(strValue))
            {
                List<string> lstvalues = strValue.Split(',').ToList<string>();
                foreach (TreeNode tn in lst.Nodes)
                {
                    if (tn.Checked && !lstvalues.Contains(tn.Tag.ToString()) && !string.IsNullOrEmpty(tn.Tag.ToString().Trim()))
                    {
                        tn.Checked = false;
                    }
                    else if (!tn.Checked && lstvalues.Contains(tn.Tag.ToString()) && !string.IsNullOrEmpty(tn.Tag.ToString().Trim()))
                    {
                        tn.Checked = true;
                    }
                }
            }

            lst.Show();

        }
        #endregion

        private void lst_KeyUp(object sender, KeyEventArgs e)
        {
            this.OnKeyUp(e);
        }

        private void lst_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                this.Text = "";
                for (int i = 0; i < lst.Nodes.Count; i++)
                {
                    if (lst.Nodes[i].Checked)
                    {
                        if (this.Text != "")
                        {
                            this.Text += ",";
                        }
                        this.Text += lst.Nodes[i].Tag;
                    }
                }
            }
            catch
            {
                this.Text = "";
            }
            bool isControlPressed = (Control.ModifierKeys == Keys.Control);
            bool isShiftPressed = (Control.ModifierKeys == Keys.Shift);
            if (isControlPressed || isShiftPressed)
                lst.Show();
            else
                lst.Hide();
        }

    }


    /// <summary>
    /// 对象帮助类
    /// </summary>
    public class ObjectUtil
    {
        /// <summary>
        /// 获取对象的属性值
        /// </summary>
        /// <param name="obj">可能是DataRowView或一个对象</param>
        /// <param name="propertyName">属性名</param>
        /// <returns>属性值</returns>
        public static object GetPropertyValue(object obj, string propertyName)
        {
            object result = null;

            try
            {
                if (obj is DataRow)
                {
                    result = (obj as DataRow)[propertyName];
                }
                else if (obj is DataRowView)
                {
                    result = (obj as DataRowView)[propertyName];
                }
                else if (obj is JObject)
                {
                    result = (obj as JObject).Value<JValue>(propertyName).Value; //.getValue(propertyName);
                }
                else
                {
                    result = GetPropertyValueFormObject(obj, propertyName);
                }
            }
            catch (Exception)
            {
                // 找不到此属性
            }

            return result;
        }

        /// <summary>
        /// 获取对象的属性值
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="propertyName">属性名（"Color"、"BodyStyle"或者"Info.UserName"）</param>
        /// <returns>属性值</returns>
        private static object GetPropertyValueFormObject(object obj, string propertyName)
        {
            object rowObj = obj;
            object result = null;

            if (propertyName.IndexOf(".") > 0)
            {
                string[] properties = propertyName.Split('.');
                object tmpObj = rowObj;

                for (int i = 0; i < properties.Length; i++)
                {
                    PropertyInfo property = tmpObj.GetType().GetProperty(properties[i], BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    if (property != null)
                    {
                        tmpObj = property.GetValue(tmpObj, null);
                    }
                }

                result = tmpObj;
            }
            else
            {
                PropertyInfo property = rowObj.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (property != null)
                {
                    result = property.GetValue(rowObj, null);
                }
            }

            return result;
        }
    }

}
