using System;
using System.Collections.Generic;
using System.Text;
using WolfInv.Com.MetaDataCenter;
using System.Xml;
using XmlProcess;
using WolfInv.Com.WCS_Process;
using System.IO;
using System.Data;
using WolfInv.Com.XPlatformCtrlLib;
namespace WolfInv.Com.CommCtrlLib
{
    //public delegate void EventHandler(object sender, EventArgs e);
    //public delegate void ListSelectedItemsHandle(object sender);
    //public delegate void EditViewHandle(object sender);
    //public delegate bool SaveCloseHandle(object sender);
    //public delegate void SaveNewHandle(object sender);
    public delegate void AddExistHandle(CMenuItem mnu);
    ////public delegate bool SaveHandle(object sender);
    ////public delegate void NewCreateHandle(object sender);
    ////public delegate void RefreshDataHandle(object sender);
    ////public delegate void LoadControlsHandle(object sender);
    ////public delegate void ExportHandle(object sender);
    ////public delegate void OkNoSaveHandle(object sender);

    public delegate void ToolBarHandle();
    public delegate bool ToolBarResponseHandle();
    //public delegate UpdateData GetUpdateData(bool JudgeValueChanged);
    public interface IFrame : ITag, IModuleControl,IUserData
    {
        
        #region virtual function

        event EventHandler ToolBar_OnSimpleSearchClicked;
        event ToolBarHandle ToolBar_ListSelectedItemsClicked;
        event ToolBarHandle ToolBar_EditView;
        event ToolBarResponseHandle ToolBar_SaveClose;
        event ToolBarHandle ToolBar_SaveAndCreateNew;
        event AddExistHandle ToolBar_AddExist;
        event AddExistHandle ToolBar_NewCreate;
        event ToolBarHandle ToolBar_RefreshData;
        //event ToolBarHandle LoadControls;
        event ToolBarHandle ToolBar_Export;
        event ToolBarHandle ToolBar_OkNoSave;
        event EventHandler ToolBar_Clicked;
        event AddExistHandle ToolBar_SaveAs;
        event AddExistHandle ToolBar_OtherEvent;
        event AddExistHandle ToolBar_ExportTo;

        UpdateData GetUpdateData(bool JudgeValueChanged, bool UpdateFrameData);
        UpdateData GetUpdateData(bool JudgeValueChanged);
        bool Save();
        bool CheckData();
        void InitEvent();
        bool LoadControls();
        void ToolBarBtn_Click(object sender, EventArgs e);
        void SimpleSearch(object sender,EventArgs e);
        #endregion

        //BaseFormHandle BehHandleObject { get;set;}
 
    }

    public interface IFrameObj:ITag 
    {
        BaseFormHandle BehHandleObject { get;set;}
    }

    public abstract class BaseFormHandle : IFrame, IKeyTransable, IUserData, IKeyForm, ITranslateableInterFace, IMutliDataInterface
    {
        #region var
        string _uid;

        object _Tag;
        public string Title;
        public IFrameObj DataFrm;

        public IKeyForm Link;
        public IFrameActionSwitch FrameSwitcher;
        string _strModule;
        string _strScreen = "main";
        string _strTarget = "list";
        public string _strRowId = "";
        public string GridSource;
        public string DetailSource;
        public string _strKey;

        public object Tag { get { return _Tag; } set { _Tag = value; } }
        public string strUid { get { return _uid; } set { _uid = value; } }

        #endregion

        #region instance
        protected BaseFormHandle()
        {
            
        }

        protected BaseFormHandle(string id)
        {
            this.strRowId = id;
            //DataFrm.Tag = this;
        }
        #endregion

        #region IKeyForm 成员

        public string strRowId
        {
            get
            {
                return _strRowId;
            }
            set
            {
                _strRowId = value;
            }
        }

        public string strKey
        {
            get
            {
                return _strKey;
            }
            set
            {
                _strKey = value;
            }
        }

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

        #region IFrame 成员

        public event EventHandler ToolBar_OnSimpleSearchClicked;

        public event ToolBarHandle ToolBar_ListSelectedItemsClicked;

        public event ToolBarHandle ToolBar_EditView;

        public event ToolBarHandle ToolBar_PrintPDF;

        public event ToolBarResponseHandle ToolBar_SaveClose;

        public event ToolBarHandle ToolBar_SaveAndCreateNew;

        public event AddExistHandle ToolBar_AddExist;

        public event AddExistHandle ToolBar_NewCreate;

        public event ToolBarHandle ToolBar_RefreshData;


        public event ToolBarHandle ToolBar_Export;

        public event ToolBarHandle ToolBar_OkNoSave;

        public event EventHandler ToolBar_Clicked;

        public event AddExistHandle ToolBar_SaveAs;

        public event AddExistHandle ToolBar_OtherEvent;

        public event AddExistHandle ToolBar_ExportTo;

        public virtual void SimpleSearch(object sender, EventArgs e)
        {
        }

        public abstract UpdateData GetUpdateData(bool JudgeValueChanged, bool UpdateFrameData);

        public virtual UpdateData GetUpdateData(bool JudgeValueChanged)
        {
            return GetUpdateData(false, false);
            //throw new Exception("The method or operation is not implemented.");
        }

        public virtual bool Save()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool CheckData()
        {
            return false;
        }

        public abstract bool LoadControls();

        public virtual void InitEvent()
        {
            ToolBar_Clicked += new EventHandler(ToolBarBtn_Click);
            ToolBar_OnSimpleSearchClicked += new EventHandler(SimpleSearch);
            ToolBar_OkNoSave += new ToolBarHandle(BaseForm_ToolBar_OkNoSave);
            ToolBar_SaveClose += new ToolBarResponseHandle(BaseForm_ToolBar_SaveClose);
            ToolBar_SaveAndCreateNew += new ToolBarHandle(BaseForm_ToolBar_SaveAndCreateNew);
            ToolBar_OtherEvent += new AddExistHandle(BaseFormHandle_ToolBar_OtherEvent);
        }

        protected void BaseFormHandle_ToolBar_OtherEvent(CMenuItem mnu)
        {
            if (mnu.LinkValue == "") return;
            if (mnu.ReplaceMenu)
            {
                CMenuItem rplmnu = mnu.Clone() as CMenuItem;
                rplmnu.Params = strRowId;
                rplmnu.MnuName = this.Title;
                mnu = rplmnu;
            }
            UpdateData data = null;
            string msg = null;
            if (mnu.linkType == LinkType.Dialog || mnu.linkType == LinkType.Select)
            {

                if (FrameSwitcher.ShowDialoger(this.DataFrm, this, mnu, ref data, ref msg))
                {
                    ToolBar_RefreshData();
                }
            }
            else
            {
                BaseFormHandle handle = null;
                FrameSwitcher.GetFrameHandle(this.DataFrm, this, mnu, ref data, ref msg, ref handle);
            }
        }

        public virtual bool BaseForm_ToolBar_SaveClose()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual void BaseForm_ToolBar_OkNoSave()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual  void BaseForm_ToolBar_SaveAndCreateNew()
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

        public void ToolBarBtn_Click(object sender, EventArgs e)
        {
            CMenuItem mnu = (sender as ITag).Tag as CMenuItem;
            if (mnu == null) return;
            switch (mnu.MnuId)
            {
                case "PrintPDF":
                    {
                        ToolBar_PrintPDF();
                        break;
                    }
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
                case "SaveAs":
                    {
                        ToolBar_SaveAs(mnu);
                        break;
                    }
                default:
                    {
                        ToolBar_OtherEvent(mnu);
                        break;
                    }
            }
        }

        //InitBaseConditions
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

        public abstract DataSet InitDataSource(string SourceName, out string msg);

        public abstract void BoundDataControls(params ITag[] controls);

        public string InitToolBar(ITag ToolBarObj)
        {
            BaseToolBarBuild btbb = new BaseToolBarBuild(this, ToolBarObj);
            btbb.frm = this;
            btbb.LoadToolBar();
            return null;
        }

        public XmlDocument GetConfigXml(string flag)
        {
            string strFilePath = string.Format("{0}\\{1}\\{4}_{1}_{2}_{3}.xml", "", strModule, strScreen, strTarget, flag);
            return GlobalShare.UpdateWithUseInfo(GlobalShare.GetXmlFile(strFilePath), strUid) as XmlDocument;
        }

        public XmlDocument GetConfigXml()
        {
            return GetConfigXml("frm");
        }

        #region IMutliDataInterface 成员
        List<UpdateData> _injectedatas;
        public List<UpdateData> InjectedDatas
        {
            get { return _injectedatas; }
            set { _injectedatas = value; }
        }

        public abstract List<UpdateData> GetDataList(List<UpdateData> OrgList, bool OnlyCheckedItem);

        public List<UpdateData> GetDataList(List<UpdateData> OrgList)
        {
            return GetDataList(OrgList, false);
        }

        public List<UpdateData> GetDataList(bool OnlyCheckedItem)
        {
            return GetDataList(null, OnlyCheckedItem);
        }

        #endregion
    
        
    }

   

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

    

    public enum ToolBarItemType
    {
        Separator,Label,Button,Combox,TextBox,ImageButton,Image,Menu,Mix
    }

    public class ToolBarItemBuilder
    {
        protected object ToolBar;
        protected IFrame frmObj;
        public ToolBarItemBuilder(IFrame frm, ITag toolbar)
        {
            frmObj = frm;
            ToolBar = toolbar;
            
        }
        public virtual void InitToolBar(bool LeftToRight)
        {
            throw new Exception("请先实现该功能！");
        }

        public virtual ITag AddToolBarItem(CMenuItem mnu, ToolBarItemType type, EventHandler del)
        {
            throw new Exception("请先实现该功能！");
            //return null;
        }

        public virtual ITag AddToolBarItem(string lbl, ToolBarItemType type)
        {
            return AddToolBarItem(lbl, type, null);
        }

        public virtual ITag AddToolBarItem(string lbl, ToolBarItemType type, EventHandler del)
        {
            throw new Exception("请先实现该功能！");
        }
        public virtual ITag AddToolBarItem(XmlNode xml, ToolBarItemType type, string itemid, params EventHandler[] del)
        {
            throw new Exception("请先实现该功能！");
        }

        public virtual ITag AddToolBarItem(string id, XmlNode backxml, string lbl, ToolBarItemType type, EventHandler del)
        {
            throw new Exception("请先实现该功能！");

        }
    }

}
