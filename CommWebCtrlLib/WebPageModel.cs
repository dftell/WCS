using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Drawing;
using WolfInv.Com.CommCtrlLib;
using WolfInv.Com.MetaDataCenter;
using System.Collections.Generic;

namespace WolfInv.Com.CommWebCtrlLib
{
    public class WebPageModel:Page, IKeyForm, ITranslateableInterFace, IUserData, IKeyTransable, ITag, IDataSouceControl, IFrameObj,IModuleControl
    {
        public List<UpdateData> InjectedDatas
        {
            get; set;
        }

        
        object _Tag;
        string _strModule ="system";
        string _strScreen = "main";
        string _strTarget = "summary";
        public object Tag{get{return _Tag ;}set{_Tag = value ;}}

        WebPageFormHandle behobj;
        public BaseFormHandle BehHandleObject { get { return behobj; } set { behobj = value as WebPageFormHandle; } }

        public string strRowId { get; set; }
        public string strKey { get; set; }
        public UpdateData NeedUpdateData { get; set; }
        public List<DataTranMapping> TranData { get; set; }
        public string strUid { get; set; }
        public string GridSource { get; set; }
        public string DetailSource { get; set; }
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

        protected Table MainFrame;
        protected ToolBarStrip toolbarstrip;
        protected TableCell TdMain;
        protected TableCell TdBottom;
        protected Label PageTitle;
        public WebPageModel()
        {

            

     
            
            //behobj.currpage = 
            
            
        }

        ////protected override void OnLoad(EventArgs e)
        ////{
        ////    base.OnLoad(e);
        ////    Init_Compenent();
        ////}

        public override ControlCollection Controls
        {
            get
            {
                Control ctrl = this.FindControl("form1");
                if (ctrl == null)
                {
                    return base.Controls;
                }
                return ctrl.Controls;
            }
        }

        

        protected virtual void InitPage()
        {

            string rnd = this.Request["rnd"];
            if (rnd == null || rnd.Trim().Length == 0)
            {
                //MessageBox.Alert(this, "外部请求！");
                return;
            }
            if (!WebPageSwitcher.ExtraReqMappings.ContainsKey(rnd))
            {
                MessageBox.Alert(this.Page, "非法的外部请求！");
                return;
            }
            this.BehHandleObject = WebPageSwitcher.ExtraReqMappings[rnd].Handle;
            this.BehHandleObject.DataFrm = this;
            behobj = this.BehHandleObject as WebPageFormHandle;
            ToolBarBuildForWeb tbf = new ToolBarBuildForWeb(behobj, this.toolbarstrip);
            tbf.LoadToolBar();
            this.PageTitle.Text = this.BehHandleObject.Title;
            //this.BehHandleObject = new WebPageFormHandle();
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            Init_Compenent();
            if (!this.IsPostBack)
            {
                
                
            }
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
            TrToolBar.Height = new Unit(20,UnitType.Pixel);
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
            //Controls.Add(MainFrame);
        }

        public UpdateData GetCurrFrameData()
        {
            throw new NotImplementedException();
        }

        public List<DataCondition> InitBaseConditions()
        {
            throw new NotImplementedException();
        }

        public DataSet InitDataSource(string sGridSource, out string msg)
        {
            throw new NotImplementedException();
        }

        #region copy from frm_model

        #endregion
    }
}
