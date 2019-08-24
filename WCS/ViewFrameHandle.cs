using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Xml;
using WolfInv.Com.WCS_Process;
using WolfInv.Com.MetaDataCenter;
using WolfInv.Com.CommFormCtrlLib;
using WolfInv.Com.CommCtrlLib;
using WolfInv.Com.XPlatformCtrlLib;

namespace WCS
{
    public class ViewFrameHandle : WinFormHandle
    {
        List<ListGridItem> lvis = new List<ListGridItem>();
        Grid GridObj;
        ListGrid listView1;
        ViewFrameHandle frm;
        frm_View fview;
        public ViewFrameHandle()
            : base()
        {
            ;
        }
        public ViewFrameHandle(string id)
            : base(id)
        {
        }

        public override UpdateData GetUpdateData(bool JudgeValueChanged, bool UpdateFrameData,bool getText=false)
        {
            return new UpdateData();
        }


        public override bool LoadControls()
        {
            fview = this.DataFrm as frm_View;
            fview.BehHandleObject = this;
            XmlDocument xmldoc = GetConfigXml();
            if(xmldoc == null) return false ;
            InitGrid(GetConfigXml(),GridObj,listView1);

            return true;
        }

        void InitGrid(XmlNode xmldoc, Grid Grid,ListGrid listView1)
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
            GridObj.listViewObj = listView1;
            GridObj.FillGrid(cmbNode);
            GridObj.AllowGroup = GridObj.listViewObj.AllowGroup;
            GridObj.GroupBy = GridObj.listViewObj.GroupBy;
            GridObj.AllowSum = GridObj.listViewObj.AllowSum;
            GridObj.SumItems = GridObj.listViewObj.SumItems;
        }

        protected void FillData(DataSet ds)
        {

            if (GridObj == null)
                GridObj = listView1.Tag as Grid;
            if (GridObj == null) return;
            GridObj.FillGridData(DataSource.DataSet2UpdateData(ds, this.GridSource, strUid));    
            fview.label_buttom.Text = string.Format("合计：{0} 条件记录", listView1.Items.Count-1);
            listView1.Refresh();
        }

        public override void BoundDataControls(params ITag[] controls)
        {
            this.listView1 = controls[0] as ListGrid;
        }

        public override void InitEvent()
        {
            base.InitEvent();
        }

        #region 事件处理

        protected void EditView_Click()
        {
            ////if (GridObj == null) GridObj = this.listView1.Tag as Grid;
            ////frm_EditView frm = new frm_EditView(this.GridSource, this, GridObj.ViewList);
            ////frm.strUid = this.strUid;
            ////if (frm.ShowDialog(this) == DialogResult.Yes)
            ////{
            ////    this.frm_View_Load(null, null);
            ////}
        }

        protected void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == fview.lvwColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (fview.lvwColumnSorter.Order == SortOrder.Ascending)
                {
                    fview.lvwColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    fview.lvwColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                fview.lvwColumnSorter.SortColumn = e.Column;
                fview.lvwColumnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            this.listView1.Sort();
            if (this.listView1.Items.Count > 0)
            {
                this.listView1.EnsureVisible(0);
                this.listView1.Items[0].Selected = true;

            }

        }

        protected void NewCreate_Click(CMenuItem mnu)
        {
            if (mnu.Key == null || mnu.Key == "")
            {
                if (GridObj == null)
                    GridObj = this.listView1.Tag as Grid;
                GridObj.ItemDbClickedMenu.Params = "";
                GridObj.ItemDbClickedMenu.MnuName = string.Format(GridObj.ItemDbClickedMenu.MnuName.Trim().Length == 0 ? "{0}" : GridObj.ItemDbClickedMenu.MnuName, "新建");
                if (FrameSwitch.switchToView(fview.panel_main, fview, GridObj.ItemDbClickedMenu))
                {
                    RefreshData_Click();
                }
            }
            else
            {
                if (FrameSwitch.switchToView(fview.panel_main, this, mnu))
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
            DataSet dss = DataSource.InitDataSource(GridSource, conds, strUid, out msg);
            if (msg != null || dss == null)
            {
                MessageBox.Show(msg);
                return;
            }
            FillData(dss);


        }

        protected void Export_Click()
        {
            if (GridObj == null)
                GridObj = this.listView1.Tag as Grid;
            GridObj.ExportGrid();
        }

        protected void ListSelectedItems_Click()
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

        protected void RefreshData_Click()
        {
            string msg = null;
            DataSet ds = InitDataSource(GridSource, out msg);
            if (msg != null)
            {
                MessageBox.Show(msg);
                return;
            }
            //InitDataSource();
            if (ds == null)
                return;
            FillData(ds);
        }
        #endregion

        public override List<UpdateData> GetDataList(List<UpdateData> OrgList, bool OnlyCheckedItem)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        
    }
}
