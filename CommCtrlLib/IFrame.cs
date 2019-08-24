using System;
using System.Text;
using WolfInv.Com.MetaDataCenter;
using System.IO;
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
    public delegate bool ToolBarResponseHandle(CMenuItem mnu);
    //public delegate UpdateData GetUpdateData(bool JudgeValueChanged);
    public interface IFrame : ITag, IModuleControl,IUserData
    {
        
        #region virtual function

        event EventHandler ToolBar_OnSimpleSearchClicked;
        event ToolBarHandle ToolBar_ListSelectedItemsClicked;
        event ToolBarHandle ToolBar_EditView;
        event ToolBarResponseHandle ToolBar_SaveClose;
        event AddExistHandle ToolBar_SaveAndCreateNew;
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

        UpdateData GetUpdateData(bool JudgeValueChanged, bool UpdateFrameData=true,bool getText=false);
        UpdateData GetUpdateData(bool JudgeValueChanged);
        bool Save(CMenuItem mnu);

        
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

    public interface IMainFrame
    {
        IXPanel CurrMainPanel
        {
            get;
        }
    }

    

    public enum ToolBarItemType
    {
        Separator,Label,Button,Combox,TextBox,ImageButton,Image,Menu,Mix
    }

}
