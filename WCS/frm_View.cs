using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using WolfInv.Com.WCS_Process;
using XmlProcess;
using System.IO;
using WolfInv.Com.MetaDataCenter;
using WolfInv.Com.CommFormCtrlLib;
using WolfInv.Com.CommCtrlLib;
using WolfInv.Com.HtmlExp;
using WolfInv.Com.XPlatformCtrlLib;
using System.Linq;

namespace WCS
{
    public partial class frm_View : frm_Model,IMutliDataInterface
    {
        public Grid GridObj;
        public DataSet GridData;
        List<ListGridItem> lvis = new List<ListGridItem>();
        #region events
        //public override  event EventHandler ToolBar_OnSimpleSearchClicked;

        //public event ToolBarHandle ToolBar_ListSelectedItemsClicked;

        //public override event ToolBarHandle ToolBar_EditView;

        //public override event ToolBarHandle ToolBar_RefreshData;

        #endregion

        //List<UpdateData> _injectedatas;
        public List<UpdateData> InjectedDatas
        {
            get; set;
        }

        

        public frm_View():base()
        {
            InitializeComponent();
        }
        
        public override void InitEvent()
        {
            base.InitEvent();
            this.ToolBar_SaveAs += new AddExistHandle(frm_View_ToolBar_SaveAs);
            //this.RefreshData +=new RefreshDataHandle();
        }

        void frm_View_ToolBar_SaveAs(CMenuItem mnu)
        {
            List<UpdateData> datas = this.GetDataList(true);
            if (datas.Count == 0)
            {
                MessageBox.Show("请先选择至少一条记录!");
                return;
            }

        }

        public frm_View(string key):base(key)
            
        {
            InitializeComponent();
            this.strRowId = key;
        }
        
        public override bool LoadControls()
        {
            XmlDocument xmldoc = GetConfigXml();
            if (xmldoc == null) return false;
            InitGrid(xmldoc);
            return true;
            
        }

        void InitGrid(XmlNode xmldoc)
        {
            if (xmldoc == null) 
            {
                MessageBox.Show("未配置Xml文件！");
                return; 
            }
            XmlNode cmbNode = xmldoc.SelectSingleNode("/root/Grid");
            if (cmbNode == null)
            {
                return;
            }
            GridObj = new Grid(this);
            GridObj.listViewObj = this.listView1;
            GridObj.FillGrid(cmbNode);
            GridObj.AllowGroup = GridObj.listViewObj.AllowGroup;
            GridObj.GroupBy = GridObj.listViewObj.GroupBy;
            GridObj.AllowSum = GridObj.listViewObj.AllowSum;
            GridObj.SumItems = GridObj.listViewObj.SumItems;
        }

        protected void FillData(DataSet ds)
        {
            
            ////this.listView1.VirtualMode = true;
            ////this.listView1.VirtualListSize = ds.Tables[0].Rows.Count;
            GridData = ds;
            if (GridObj == null) 
                GridObj = this.listView1.Tag as Grid;
            if (GridObj == null) return;
            GridObj.FillGridData(DataSource.DataSet2UpdateData(ds, this.GridSource, strUid));
            //this.listView1.DrawItem += new DrawListGridItemEventHandler(listView1_DrawItem);
            //this.listView1.RetrieveVirtualItem += new RetrieveVirtualItemEventHandler(listView1_RetrieveVirtualItem);
            /*
            lvis = new List<ListGridItem>();
            if (GridObj == null) 
                GridObj = this.listView1.Tag as Grid;
            if (GridObj == null) return;
            string[] grids = new string[GridObj.Columns.Count];
            for (int r = 0; r < GridData.Tables[0].Rows.Count; r++)
            {
                ItemValue iv = new ItemValue();
                iv.ItemValues = new Dictionary<string, string>();
                for (int i = 0; i < GridObj.Columns.Count; i++)
                {
                    if (GridObj.Columns[i].DataField == "") continue;
                    grids[i] = GridData.Tables[0].Rows[r][GridObj.Columns[i].DataField].ToString();
                    if (!iv.ItemValues.ContainsKey(GridObj.Columns[i].DataField))
                    {
                        iv.ItemValues.Add(GridObj.Columns[i].DataField, grids[i]);
                    }
                    if (GridObj.Columns[i].IsKeyValue) iv.KeyValue = grids[i];
                    if (GridObj.Columns[i].IsKeyText) iv.KeyText = grids[i];
                }
                ListGridItem lvi = new ListGridItem(grids);
                lvi.Tag = iv;
                lvis.Add(lvi);
            }
            this.listView1.Items.Clear();
            this.listView1.Items.AddRange(lvis.ToArray());
             */
            
            this.label_buttom.Text =  string.Format("合计：{0} 条件记录",this.listView1.Items.Count.ToString());
            this.listView1.Refresh();
        }

        protected void FillData(List<UpdateData> ds)
        {
            if (GridObj == null)
                GridObj = this.listView1.Tag as Grid;
            if (GridObj == null) return;
            GridObj.FillGridData(ds);
            this.label_buttom.Text = string.Format("合计：{0} 条件记录", this.listView1.Items.Count.ToString());
            this.listView1.Refresh();
        }

        private void frm_View_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (!LoadControls()) return;
            RefreshData_Click();
            this.Cursor = Cursors.Default;
            
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {

            if (this.listView1.SelectedItems.Count == 0) return;
            
            
            ListGridItem lvi = this.listView1.SelectedItems[0] as ListGridItem;
            if (lvi is SumListGridItem) return;//合计项不能双击；
            //ListGridItem lvi = this.listView1.GetItemAt(tmpPoint.X, tmpPoint.Y);
            if (lvi == null) return;
            ItemValue iv = (lvi.Tag as GridRow).ItemValues;
            if (iv == null) return;
            if (GridObj == null) GridObj = this.listView1.Tag as Grid;
            if (GridObj.ItemDbClickedMenu == null || GridObj.ItemDbClickedMenu.LinkValue == null || GridObj.ItemDbClickedMenu.LinkValue.Trim().Length == 0)
                return;
            CMenuItem mnu = GridObj.ItemDbClickedMenu.Clone() as CMenuItem;
            mnu.Params = iv.KeyValue;
            if (mnu.MnuName.Trim().Length == 0)
            {
                mnu.MnuName = iv.KeyText;
            }
            else
            {
                List<string> names = DataPointReg.GetExpresses(mnu.MnuName);
                if (names != null)
                {
                    GridRow gr = (lvi.Tag as GridRow);
                    for (int i = 0; i < names.Count; i++)
                    {
                        if (gr.Items.ContainsKey(names[i]))//数据项中包括
                        {
                            mnu.MnuName = mnu.MnuName.Replace("{" + names[i] + "}", gr.Items[names[i]].value);
                        }
                    }
                }
                else
                {
                    mnu.MnuName = iv.KeyText;
                }
            }
            //GridObj.ItemDbClickedMenu.MnuName = string.Format(GridObj.ItemDbClickedMenu.MnuName.Trim().Length == 0 ? "{0}" : GridObj.ItemDbClickedMenu.MnuName, iv.KeyText);
            UpdateData data = this.GetUpdateData(false, false);
            if ((lvi.Tag as GridRow).ExtraSourceData != null) data = (lvi.Tag as GridRow).ExtraSourceData;//如果是外部数据
            if (mnu.linkType == LinkType.Dialog || mnu.linkType == LinkType.Select)
            {
                
                string msg = null;
                //new FormActionHandle().CreateFrame(this,this.BehHandleObject,GridObj.ItemDbClickedMenu, ref data,ref msg);
                if (FrameSwitch.ShowDialoger(this.CurrMainPanel, this, mnu, ref data) == true)
                {
                    RefreshData_Click();
                }
            }
            else
                FrameSwitch.switchToView(this.CurrMainPanel, this, mnu, ref data);
                
        }

        protected void EditView_Click()
        {
            if (GridObj == null) GridObj = this.listView1.Tag as Grid;
            frm_EditView frm = new frm_EditView(this.GridSource,this,GridObj.ViewList);
            frm.strUid = this.strUid;
            if (frm.ShowDialog(this) == DialogResult.Yes)
            {
                this.frm_View_Load(null, null);
            }
        }

        protected void PrintPDF_Click()
        {
            if (GridObj == null) GridObj = this.listView1.Tag as Grid;
            frm_EditView frm = new frm_EditView(this.GridSource, this, GridObj.ViewList);
            frm.strUid = this.strUid;
            if (frm.ShowDialog(this) == DialogResult.Yes)
            {
                this.frm_View_Load(null, null);
            }
        }


        protected  void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            lv_CloumnClick(sender as ListView, e);
        }

        protected void NewCreate_Click(CMenuItem mnu)
        {
            if (GridObj == null)
                GridObj = this.listView1.Tag as Grid;

            if (mnu.Key == null || mnu.Key == "")
            {
                mnu = GridObj.ItemDbClickedMenu;
                GridObj.ItemDbClickedMenu.Params = "";
                GridObj.ItemDbClickedMenu.MnuName = string.Format(GridObj.ItemDbClickedMenu.MnuName.Trim().Length == 0 ? "{0}" : GridObj.ItemDbClickedMenu.MnuName, "新建");

                UpdateData data = this.GetUpdateData(false, false);
                //if ((lvi.Tag as GridRow).ExtraSourceData != null) data = (lvi.Tag as GridRow).ExtraSourceData;//如果是外部数据
                if (mnu.linkType == LinkType.Dialog || mnu.linkType == LinkType.Select)
                {

                    string msg = null;
                    //new FormActionHandle().CreateFrame(this,this.BehHandleObject,GridObj.ItemDbClickedMenu, ref data,ref msg);
                    if (FrameSwitch.ShowDialoger(this.CurrMainPanel, this, mnu, ref data) == true)
                    {
                        RefreshData_Click();
                    }
                }
                else
                {
                    if (FrameSwitch.switchToView(this.Parent as IXContainerControl, this, mnu))
                    {
                        RefreshData_Click();
                    }
                }
            }
            else
            {
                if (FrameSwitch.switchToView(this.panel_main, this, mnu))
                {
                    RefreshData_Click();
                }
            }
        }

        public void SimpleSearch(object sender, EventArgs e)
        {
            //base.searchbtn_Click(sender, e);
            ToolStripTextBox txtbox = (sender as ToolStripButton).Tag as ToolStripTextBox;
            if (txtbox.Text.Trim().Length == 0)
            { this.RefreshData_Click(); return; }
            DataCondition dc = txtbox.Tag as DataCondition;
            List<DataCondition> conds = InitBaseConditions();
            DataCondition ds = new DataCondition();
            ds.Datapoint = new DataPoint(this.strKey);
            ds.value = this.strRowId;
            ds.Logic = ConditionLogic.And;
            dc.Logic = ConditionLogic.And;
            if (dc.SubConditions == null || dc.SubConditions.Count == 0)
            {
                this.RefreshData_Click();
                return;
            }
            foreach (DataCondition sdc in dc.SubConditions)
            {
                sdc.value = txtbox.Text;
                sdc.Logic = ConditionLogic.Or;
            }
            conds.Add(ds);
            conds.Add(dc);
            if (GridSource == null || GridSource.Trim().Length == 0)
                return;
            string msg = null;
            GridData = DataSource.InitDataSource(GridSource, conds,strUid,out msg);
            if (msg != null || GridData == null)
            {
                MessageBox.Show(msg);
                return;
            }
            FillData(GridData);
        }

        protected  void Export_Click()
        {
            if(GridObj == null)
                GridObj = this.listView1.Tag as Grid;
            GridObj.ExportGrid();
        }

        protected  void ListSelectedItems_Click()
        {
            if (GridObj == null) GridObj = this.listView1.Tag as Grid;
            GridObj.Items.Clear();
            foreach (ListGridItem lgi in this.listView1.CheckedItems)
            {
                if (lgi is SumListGridItem) continue;
                GridObj.Items.Add(lgi.Tag as GridRow);
            }
            GridObj.FillListView();
        }

        protected  void RefreshData_Click()
        {
            string msg = null;
            if (this.InjectedDatas == null || this.InjectedDatas.Count == 0)
            {
                GridData = InitDataSource(GridSource, out msg);
                if (msg != null)
                {
                    MessageBox.Show(msg);
                    return;
                }
                //InitDataSource();
                if (GridData == null)
                    return;
                FillData(GridData);
            }
            else
            {
                FillData(this.InjectedDatas);
            }
        }

        public override void ToolBar_OtherEvent_Click(CMenuItem mnu)
        {
            //base.ToolBar_OtherEvent_Click(mnu);
            switch (mnu.MnuId)
            {
                case "Injected":
                    {
                        this.Cursor = Cursors.WaitCursor;
                        List<UpdateData> datas = null;
                        try
                        {
                            datas = GetOADatas();
                        }
                        catch (Exception ce)
                        {
                            MessageBox.Show("使用该功能前必须用OA帐号和密码登陆系统！");
                            this.Cursor = Cursors.Default;
                            return;
                        }

                        UpdateData data = null;
                        FrameSwitch.switchToView(null, null, mnu, ref data, datas);
                        this.Cursor = Cursors.Default;
                        break;
                    }
                case "":
                default:
                    {
                        base.ToolBar_OtherEvent_Click(mnu);
                        break;
                    }
            }
        }
        #region IMutliDataInterface 成员
       


        public List<UpdateData> GetDataList(List<UpdateData> OrgList)
        {
            return GetDataList(OrgList, false);
        }

        public List<UpdateData> GetDataList(bool OnlyCheckedItem)
        {
            return GetDataList(null, OnlyCheckedItem);
        }


        public List<UpdateData> GetDataList(List<UpdateData> OrgList, bool OnlyCheckedItem)
        {
            if (OrgList == null)
                OrgList = new List<UpdateData>();
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                if (OnlyCheckedItem && !listView1.Items[i].Checked) continue;
                GridRow row = listView1.Items[i].Tag as GridRow;
                OrgList.Add(row.ToUpdateData());
            }
            return OrgList;
        }

        #endregion

        List<UpdateData> GetOADatas()
        {
            NoteExpClass nec = new NoteExpClass(Application.StartupPath + "\\import\\config.xml");
            return nec.GetDatas();
        }

        protected override void combobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //base.combobox_SelectedIndexChanged(sender, e);
            ToolStripComboBox combo = sender as ToolStripComboBox;
            List<CMenuItem> menus = combo.Tag as List<CMenuItem>;
            if (menus == null) return;
            if(menus.Count <= combo.SelectedIndex || combo.SelectedIndex < 0)
            {
                return;
            }
            DataTranMapping dtm = new DataTranMapping();
            dtm.FromDataPoint.Text = menus[combo.SelectedIndex].Params;
            dtm.ToDataPoint = menus[combo.SelectedIndex].Key;
            int ExistMapIndex = -1;
            if(this.TranData == null) this.TranData = new List<DataTranMapping>();
            for(int i=0;i<this.TranData.Count;i++)
            {
                DataTranMapping key = this.TranData[i];
                if(key.ToDataPoint == dtm.ToDataPoint)
                {
                    ExistMapIndex = i;
                    break ;
                }
            }
            if (ExistMapIndex >= 0)
            {
                this.TranData[ExistMapIndex].FromDataPoint = dtm.FromDataPoint;
            }
            else
            {
                this.TranData.Add(dtm);
            }
            if (this.GridObj != null)
            {
                RefreshData_Click();
            }
        }

        private bool frm_View_ToolBar_Remove(CMenuItem mnu)
        {
            bool DirDel = false;
            if (this.strRowId == "")
            {
                DirDel = true;
            }
            if (GridObj == null)
                GridObj = this.listView1.Tag as Grid;
            if (this.listView1.CheckedIndices.Count == 0)
            {
                MessageBox.Show("为执行删除操作，至少需选择一行！");
                return false;
            }
            if (MessageBox.Show("确定要删除记录", "删除确认", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return false;
            }
            List<UpdateData> ul = new List<UpdateData>();
            bool NoPass = false;
            for (int i = this.listView1.CheckedIndices.Count - 1; i >= 0; i--)
            {
                int chkid = this.listView1.CheckedIndices[i];
                ListViewItem lvi = this.listView1.Items[chkid];
                GridRow gi = lvi.Tag as GridRow;
                if (gi == null)
                    continue;
                var keyitem = gi.Items.Where(a =>a.Value.isKey);
                if(keyitem == null)
                {
                    this.listView1.Items[this.listView1.CheckedIndices[i]].Selected = true;
                    NoPass = true;
                    continue;
                }
                
                UpdateData ud = new UpdateData();
                ud.ReqType = DataRequestType.Delete;
                ud.keydpt = new DataPoint(keyitem.First().Key);
                ud.keydpt.Text = keyitem.First().Value.value;
                ud.keyvalue = ud.keydpt.Text;

                ud.Items.Add(keyitem.First().Key,new UpdateItem(keyitem.First().Key,keyitem.First().Value.value));
                ul.Add(ud);
            }
            if(NoPass)
            {
                return false;
            }

            if (this.listView1.CheckedItems.Count == 0)
                return false;
            if (this.SaveData(null, DataRequestType.Delete))//如果
            {
                RefreshData_Click();
                return true;
            }
            else
            {
                return false;
            }
            
        }
    }
    
}
