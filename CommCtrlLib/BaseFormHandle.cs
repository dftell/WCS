using System;
using System.Collections.Generic;
using WolfInv.Com.MetaDataCenter;
using System.Xml;
using WolfInv.Com.WCS_Process;
using System.Data;
using WolfInv.Com.XPlatformCtrlLib;
namespace WolfInv.Com.CommCtrlLib
{
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

        public event ToolBarHandle ToolBar_Remove;

        public event ToolBarHandle ToolBar_Delete;

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
            ToolBar_Remove += BaseFormHandle_ToolBar_Remove;
        }

        private void BaseFormHandle_ToolBar_Remove()
        {
            
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
                case "Remove"://sub items remove
                    {
                        ToolBar_Remove();
                        break;
                    }
                case "Delete":
                    {
                        ToolBar_Delete();
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

}
