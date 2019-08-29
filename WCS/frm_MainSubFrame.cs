using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using XmlProcess;
using WolfInv.Com.WCS_Process;
using System.IO;
using WolfInv.Com.CommFormCtrlLib;
using WolfInv.Com.MetaDataCenter;
using WolfInv.Com.CommCtrlLib;
using System.Xml.Xsl;
using System.Xml.XPath;
using WolfInv.Com.XPlatformCtrlLib;
using System.Linq;
namespace WCS
{
    public partial class frm_MainSubFrame : frm_Model, IMutliDataInterface
    {
        

        #region ITranslateableInterFace 成员
        

        

        

        //List<DataTranMapping> ITranslateableInterFace.RefData { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        #endregion


        public SubGrid GridObj;
        public DataSet GridData;
        List<ListGridItem> lvis = new List<ListGridItem>();
        EditPanel PanelObj;
        public frm_MainSubFrame()
        {
            InitializeComponent();
            GridObj = new SubGrid(this);
            this.listView1.Tag = GridObj;
            GridObj.listViewObj = this.listView1;
           
        }

        

        public frm_MainSubFrame(string rowid)
        {
            InitializeComponent();
            strRowId = rowid;
            GridObj = new SubGrid(this);
            this.listView1.Tag = GridObj;
            GridObj.listViewObj = this.listView1;
        }

        void InitGrid(XmlNode xmldoc)
        {
            if (xmldoc == null) return;
            XmlNode cmbNode = xmldoc.SelectSingleNode("/root/Grid");
            if (cmbNode == null)
            {
                return;
            }
            
            GridObj.ToolBar = this.toolStrip_subtitle;
            GridObj.Lbl_Title  = this.Label_Title;
            GridObj.listViewObj = this.listView1;
            GridObj.FillGrid(cmbNode);
            GridObj.AllowGroup = GridObj.listViewObj.AllowGroup;
            GridObj.GroupBy = GridObj.listViewObj.GroupBy;
            GridObj.AllowSum = GridObj.listViewObj.AllowSum;
            GridObj.SumItems = GridObj.listViewObj.SumItems;
            if (GridObj.Lbl_Title != null)
                GridObj.Lbl_Title.Text = XmlUtil.GetSubNodeText(cmbNode, "@title");


            InitToolBarStrips(cmbNode,GridObj.ToolBar,SubGrid_btn_Click);
            AddGroupInToolBar(cmbNode, GridObj.ToolBar);
            //InitContextMenu(cmbNode, this.ContextMenuStrip, this.ToolBarBtn_Click);
            this.listView1.ContextMenuStrip = this.contextMenuStrip1;
        }


        public List<UpdateData> InjectedDatas
        {
            get; set;
        }

        #region IMutliDataInterface 成员

        public virtual List<UpdateData> GetDataList(List<UpdateData> OrgList, bool OnlyCheckedItem)
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

        public List<UpdateData> GetDataList(List<UpdateData> OrgList)
        {
            return GetDataList(OrgList, false);
        }

        public List<UpdateData> GetDataList(bool OnlyCheckedItem)
        {
            return GetDataList(null, OnlyCheckedItem);
        }

        #endregion

        void InitEditPanelDefaultValue()
        {
            foreach (PanelCell pc in PanelObj.ControlList.Values)
            {
                pc.ChangeValue(pc.DefaultValue, pc.DefaultText);
            }
        }

        private void frm_MainSubFrame_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            this.LoadFlag = false;
            try
            {
                if (LoadControls() == false)
                {
                    //this.LoadFlag = true;
                    //this.LoadFlag = true;
                    this.Cursor = Cursors.Default;
                    return;
                }
                InitEditPanelDefaultValue();
                bool isExtraData = false;
                PanelObj.CoverData(this.NeedUpdateData, strRowId != "");
                if (strRowId != "")
                {
                    string msg = null;
                    bool isextra = false;
                    GridData = InitDataSource(DetailSource, out msg,ref isextra);
                    if (msg != null)
                    {
                        this.Cursor = Cursors.Default;
                        //this.LoadFlag = true;
                        MessageBox.Show(msg);
                        return;
                    }
                    List<UpdateData> ul = DataSource.DataSet2UpdateData(GridData, DetailSource,strUid);
                    if (ul.Count != 1)
                        return;
                    PanelObj.FillData(GridData, DetailSource);
                    if (isextra&& GridData.Tables.Count > 1)//如果是外部数据，并且表数大于1
                    {
                          
                        FillGridData(ul[0].SubItems);
                       
                    }
                    else
                    {
                        DataSet ds = InitDataSource(GridSource, out msg);
                        if (msg != null)
                        {
                            this.Cursor = Cursors.Default;
                            //this.LoadFlag = true;
                            MessageBox.Show(msg);
                            return;
                        }
                        FillGridData(ds, GridSource);
                        if (InjectedDatas != null)
                        {
                            FillGridData(InjectedDatas);
                        }
                    }

                }
                else
                {
                    if (this.InjectedDatas != null)
                    {
                        FillGridData(InjectedDatas);
                    }
                    //外部数据
                    if (this.NeedUpdateData != null && this.NeedUpdateData.SubItems != null && this.NeedUpdateData.SubItems.Count > 0)
                    {
                        FillGridData(NeedUpdateData.SubItems);
                    }
                }
                this.LoadFlag = true;
            }
            catch(Exception ce)
            {
                MessageBox.Show(ce.Message);
                this.LoadFlag = false;
            }
            this.Cursor = Cursors.Default;
            //this.Refresh();

        }

        void AddExist_Click(CMenuItem mnu)
        {
            //base.AddExist();
            UpdateData data = null;
            if (!FrameSwitch.ShowDialoger(null, this, mnu, ref data,true))
            {
                return;
            }
            if (data == null || data.SubItems == null) 
            {
                    return;
            }
            GridObj = this.listView1.Tag as SubGrid;
           
            foreach (UpdateData subdata in data.SubItems)
            {
                GridRow gr = null;
                gr = GridObj.GetNewRow(ref gr, subdata);
                if (mnu.TranDataMapping != null)
                {
                    for (int i = 0; i < mnu.TranDataMapping.Count; i++)
                    {
                        DataTranMapping dtm = mnu.TranDataMapping[i];
                        if (!gr.Items.ContainsKey(dtm.FromDataPoint.Name))//如果map中值不在grid ,contine
                        {
                            continue;
                        }
                        if (!subdata.Items.ContainsKey(dtm.ToDataPoint))
                        {
                            if (this.strKey == dtm.ToDataPoint)//如果主键在map中，将主健值赋予相应的列
                            {
                                gr.Items[dtm.FromDataPoint.Name].value = strRowId;
                                gr.Items[dtm.FromDataPoint.Name].Updated = true;
                            }
                            else
                            {
                                if (!GlobalShare.DataPointMappings.ContainsKey(dtm.ToDataPoint))
                                {
                                    gr.Items[dtm.FromDataPoint.Name].value = dtm.ToDataPoint;//常量
                                    gr.Items[dtm.FromDataPoint.Name].Updated = true;
                                }
                            }
                        }
                        else
                        {
                            gr.Items[dtm.FromDataPoint.Name].value = subdata.Items[dtm.ToDataPoint].value;
                            gr.Items[dtm.FromDataPoint.Name].Updated = true;
                        }
                    }
                }
                if (gr.Items.ContainsKey(this.strKey))
                {
                    gr.Items[this.strKey].value = this.strRowId;
                    gr.Items[this.strKey].Updated = true;
                }

                GridObj.Items.Add(gr);
            }
            GridObj.FillListView();
        }

        public override bool LoadControls()
        {
            
            XmlDocument xmldoc = this.GetConfigXml();
            if (xmldoc == null) return false;
            InitEditPanel(xmldoc);
            InitGrid(xmldoc);
            return true;
        }

        public override bool CheckData()
        {
            if (PanelObj == null)
                PanelObj = this.tableLayoutPanel1.Tag as EditPanel;
            if (PanelObj.ControlList == null) return false;
            string checkmsg = PanelObj.CheckNull();
            if (checkmsg != null)
            {
                MessageBox.Show(checkmsg);
                return false;
            }
            return true;
        }
        
        public override bool Save(CMenuItem mnu)
        {
            if( SaveData(mnu,DataRequestType.Update))
            {
                MessageBox.Show("保存成功");
                return true;
            }
            return false;
        }

        public override bool SaveClientData(UpdateData updata, DataRequestType type = DataRequestType.Update)
        {
            return base.SaveClientData(updata, type);
        }
       

        void InitEditPanel(XmlNode xmldoc)
        {
            if (xmldoc == null) return;
            XmlNode cmbNode = xmldoc.SelectSingleNode("/root/EditPanel");
            if (cmbNode == null)
            {
                return;
            }
            PanelObj = new EditPanel(strUid);
            PanelObj.OwnerForm = this;
            PanelObj.Fill(cmbNode,NeedUpdateData);
            //if(PanelObj.Height > 0)
            //    this.splitContainer_detail.SplitterDistance = PanelObj.Height;
            //edit panel列处理
            //this.tableLayoutPanel1 = new TableLayoutPanel();
            this.tableLayoutPanel1.Controls.Clear();
            this.tableLayoutPanel1.RowStyles.Clear();
            this.tableLayoutPanel1.ColumnStyles.Clear();
            this.tableLayoutPanel1.RowCount = PanelObj.RowCnt;
            this.tableLayoutPanel1.ColumnCount = PanelObj.ColumnCnt;
            for (int i = 0; i < PanelObj.Rows.Count; i++)
            {
                PanelRow pr = PanelObj.Rows[i];
                RowStyle rs = new RowStyle();

                for (int c = 0; c < pr.Cells.Count; c++)
                {
                    this.tableLayoutPanel1.GrowStyle = TableLayoutPanelGrowStyle.AddColumns;
                    PanelCell pc = pr.Cells[c];
                    if (pc.ItemControl.Height > rs.Height)
                    {
                        rs.Height = pc.ItemControl.Height;
                    }

                    this.tableLayoutPanel1.Controls.Add(pc.ItemLabel, 2 * c, i);
                    //pc.ItemLabel.Anchor = AnchorStyles.None;
                    this.tableLayoutPanel1.Controls.Add(pc.ItemControl, 2 * c + 1, i);
                    if (c == pr.Cells.Count - 1)
                    {
                        this.tableLayoutPanel1.SetColumnSpan(pc.ItemControl, PanelObj.ColumnCnt - pr.Cells.Count * 2 + 1);
                    }

                }
                this.tableLayoutPanel1.GrowStyle = TableLayoutPanelGrowStyle.AddRows;
                rs.SizeType = SizeType.AutoSize;
                if (!pr.Visual)
                {
                    rs.Height = 0;
                }
                this.tableLayoutPanel1.RowStyles.Add(rs);


            }
            this.tableLayoutPanel1.Tag = PanelObj;

            //this.listView1.Tag = GridObj;

        }

        ////DataSet InitDataSource(string srcname)
        ////{
        ////    if (srcname == null || srcname.Trim().Length == 0)
        ////        return null;
        ////    return DataSource.InitDataSource(srcname, new string[1] { strKey }, new string[1] { strRowId });

        ////    //strRowId 
        ////}
        
        protected void FillGridData(DataSet ds,string strsrc)
        {

            GridData = ds;
            if (GridObj == null)
                GridObj = this.listView1.Tag as SubGrid;
            if (GridObj == null) return;
            GridObj.FillGridData(DataSource.DataSet2UpdateData(ds, this.GridSource, strUid));
            this.label_buttom.Text = string.Format("合计：{0} 条件记录", ds.Tables[0].Rows.Count.ToString());
            this.listView1.Refresh();
        }

        protected override void FillGridData(List<UpdateData> ds)
        {

            if (GridObj == null)
                GridObj = this.listView1.Tag as SubGrid;
            if (GridObj == null) return;
            GridObj.FillGridData(ds);
            this.label_buttom.Text = string.Format("合计：{0} 条件记录", ds.Count.ToString());
            this.listView1.Refresh();
        }

        void SubGrid_btn_Click(object sender, EventArgs e)
        {
            CMenuItem mnu = (sender as ToolStripButton).Tag as CMenuItem;
            if (mnu == null) return;
            try
            {
                switch (mnu.MnuId)
                {
                    case "Remove":
                        {
                            RemoveSubItem();
                            break;
                        }
                    case "RemoveAll":
                        {
                            RemoveAllSubItem();

                            break;
                        }
                    case "AddNew":
                        {
                            AddNewSubItem(mnu);
                            break;
                        }
                    case "AddExist":
                        {
                            //AddExist(mnu);
                            AddExist_Click(mnu);
                            //OnAddExit(mnu);
                            break;
                        }
                    case "Import":
                        {

                            ImportData(mnu);

                            break;
                        }
                    case "Export":
                        {
                            ToolBarBtn_Click(sender,e);
                            break;
                        }
                    
                    default:
                        {
                            break;
                        }
                }
            }
            catch (Exception ce)
            {
                MessageBox.Show(ce.Message);
            }
        }

        void AddNewSubItem(CMenuItem mnu)
        {
            this.Cursor = Cursors.WaitCursor;
            UpdateData newData = new UpdateData();
            if (!FrameSwitch.ShowDialoger(null, this,mnu, ref newData))
            {
                this.Cursor = Cursors.Default;
                return ;
            }
            this.Cursor = Cursors.WaitCursor;
            if (newData == null) return ;
            if (GridObj == null)
                GridObj = this.listView1.Tag as SubGrid;
            GridRow gr = null;
            GridObj.GetNewRow(ref gr, newData);
            if (gr == null) return ;
            GridObj.Items.Add(gr);
            GridObj.FillListView();
            this.Cursor = Cursors.Default;
            return ;
        }
        void RemoveAllSubItem()
        {
            foreach( ListViewItem  lvi in this.listView1.Items)
            {
                lvi.Checked = true;
            }
            RemoveSubItem();
        }
        void RemoveSubItem()
        {
            bool DirDel = false;
            if (this.strRowId == "")
            {
                DirDel = true;
            }
            if (GridObj == null)
                GridObj = this.listView1.Tag as SubGrid;
            if (this.listView1.CheckedIndices.Count == 0)
            {
                MessageBox.Show("为执行删除操作，至少需选择一行！");
                return;
            }
            if(MessageBox.Show("确定要删除记录","删除确认",MessageBoxButtons.YesNo)== DialogResult.No)
            {
                return;
            }
            for (int i = this.listView1.CheckedIndices.Count - 1;i>=0 ; i--)
            {
                int chkid = this.listView1.CheckedIndices[i];
                ListViewItem lvi = this.listView1.Items[chkid];
                GridRow gi = lvi.Tag as GridRow;
                if (gi == null)
                    continue;
                bool NewItem = false;
                if (gi.IsImported)
                {
                    NewItem = true;
                }
                var keys = gi.Items.Where(a => a.Value.isKey);//手工增加的经ds加载关键字列有iskey标记
                
                if (keys.Count() > 0)
                {
                    if(keys.First().Value.value == null||keys.First().Value.value.Trim().Length == 0)//关键字为空
                    {
                        NewItem = true;//为新建项，直接删除
                    }
                }
                if (DirDel|| NewItem) //如果还没保存，直接删除
                {

                    lvi.Checked = false;
                    this.listView1.Items.RemoveAt(chkid);
                    GridObj.Items.Remove(gi);//删掉内存，保存时直接保存整个主从
                }
                else
                {
                   
                    gi.Updated = true;
                    gi.Removed = true;
                }

                //gi.Items.Select(a=>a.Value)
            }
            if (DirDel)
                return;
            if (this.listView1.CheckedItems.Count == 0)
                return;
            if (this.SaveData(null,DataRequestType.Delete))//如果
            {
                this.frm_MainSubFrame_Load(null, null);
            }
            else
            {
                return;
            }
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (!this.LoadFlag)
                return;
            ListGridItem lvi = this.listView1.SelectedItems[0] as ListGridItem;

            //ListGridItem lvi = this.listView1.GetItemAt(tmpPoint.X, tmpPoint.Y);
            if (lvi == null) return;
            if(lvi.Tag is GridRow)
            {

            }
            else
            {
                MessageBox.Show("合并项无双击响应事件！");
                return;
            }
            
            ItemValue iv = (lvi.Tag as GridRow).ItemValues;
            if (iv == null) return;
            if (GridObj == null) GridObj = this.listView1.Tag as SubGrid;
            if (GridObj.ItemDbClickedMenu == null || GridObj.ItemDbClickedMenu.LinkValue == null || GridObj.ItemDbClickedMenu.LinkValue.Trim().Length == 0)
                return;
            GridObj.ItemDbClickedMenu.Params = iv.KeyValue;
            GridObj.ItemDbClickedMenu.MnuName = string.Format(GridObj.ItemDbClickedMenu.MnuName.Trim().Length == 0 ? "{0}" : GridObj.ItemDbClickedMenu.MnuName, iv.KeyText);
            UpdateData data = (lvi.Tag as GridRow).ToUpdateData() ;

            if (FrameSwitch.ShowDialoger(this.panel_main, this, GridObj.ItemDbClickedMenu,ref data))
            {
                if (data.Updated)
                {
                    GridRow oldrow = lvi.Tag as GridRow;
                    GridObj.Items.Remove(oldrow);
                    GridRow gr = GridObj.GetNewRow(ref oldrow, data);
                    GridObj.Items.Add(gr);
                    GridObj.FillListView();
                    //RefreshData();
                }
            }
        }
        
        public override UpdateData GetUpdateData(bool CheckValueChanged)
        {
            return GetUpdateData(CheckValueChanged, true);
        }
        public override UpdateData GetUpdateData(bool CheckValueChanged, bool UpdateFrameData = true, bool getText = false)
        {
            return GetUpdateData(CheckValueChanged, UpdateFrameData ,  getText);
        }

        protected override void PrintPDF(CMenuItem mnu)
        {
            UpdateData ret = null;
            UpdateData ud = null;
            ud = GetUpdateData(false, false, true,mnu.OnlyOperateSelectItems, mnu.OnlyOperateSelectGroup);
            FrameSwitch.switchToView(this.panel_main, null, mnu, ref ret, new UpdateData[1] { ud }.ToList());
        }

        protected override bool BatchUpdate(Grid gd, CMenuItem mnu)
        {
            this.Cursor = Cursors.WaitCursor;
            UpdateData ret = null;
            UpdateData ud = null;
            if(this.listView1.CheckedItems.Count==0)
            {
                MessageBox.Show("请选择需要批量修改的数据！");
                return false;
            }
            ud = GetUpdateData(false, true, true, mnu.OnlyOperateSelectItems, mnu.OnlyOperateSelectGroup,true);
            UpdateData subdata = ud.SubItems.Where(a => a.Updated==true).First();
            if(subdata == null)
            {
                MessageBox.Show("没有行状态被标记为修改！");
                return false;
            }
            if (!FrameSwitch.ShowDialoger(null, null, mnu, ref  subdata ))
            {
                this.Cursor = Cursors.Default;
                return false;
            }
            this.Cursor = Cursors.WaitCursor;
            if (subdata == null)
                return false;
            if (GridObj == null)
                GridObj = this.listView1.Tag as SubGrid;
            string[] items = gd.GroupBy.Split(',');
            ud.SubItems.ForEach(a =>{
                if(a.Updated)//如果子列表需要更新
                {
                    for(int i=0;i<items.Length;i++)
                    {
                        if(subdata.Items.ContainsKey(items[i]) && a.Items.ContainsKey(items[i]))
                        {
                            a.Items[items[i]].value = subdata.Items[items[i]].value;
                        }
                    }
                }
            });
            GridRow gr = null;
            ////GridObj.GetNewRow(ref gr, newData);
            ////if (gr == null) return;
            //GridObj.Items.Add(gr);
            //GridObj.FillListView();
            GridObj.FillGridData(ud.SubItems);
            this.Cursor = Cursors.Default;
            return true;
        }

        public  UpdateData GetUpdateData(bool CheckValueChanged, bool UpdateFrameData = true, bool getText = false, bool onlyReadSelectedItems = false, bool onlyReadSelectedGroups = false,bool onlySign=false)
        {
            
            UpdateData updata = new UpdateData();
            updata.Items = new Dictionary<string,UpdateItem>();
            int cnt = 0;
            foreach (string dpt in PanelObj.ControlList.Keys)
            {
                if(!PanelObj.ControlList.ContainsKey(dpt))
                {
                    continue;
                }
                PanelCell pc = PanelObj.ControlList[dpt];
                string strVal = pc.GetValue(false);
                if (pc.Value == strVal && CheckValueChanged)//如果值未改变
                {
                    continue;
                }
                UpdateItem ui = new UpdateItem();
                ui.datapoint = new DataPoint(dpt);
                ui.value = strVal;
                ui.text = pc.GetValue(true);
                if(!updata.Items.ContainsKey(dpt))
                    updata.Items.Add(dpt,ui);
                cnt++;
            }
            if (GridObj == null)
                GridObj = this.listView1.Tag as SubGrid;
            updata.SubItems = GridObj.GetUpdateData(CheckValueChanged,  getText, onlyReadSelectedItems, onlyReadSelectedGroups,onlySign);
            updata.AllowSum = GridObj.AllowSum;
            updata.AllowGroup = GridObj.AllowGroup;
            updata.GroupBy = GridObj.GroupBy;
            updata.SumItems = GridObj.listViewObj.SumItems;
            cnt += updata.SubItems.Count;
            updata.keydpt = new DataPoint(this.strKey);
            updata.keyvalue = this.strRowId;
            if (cnt == 0)
                updata.Updated = false;
            else
                updata.Updated = true;
            if(UpdateFrameData)
                NeedUpdateData = updata;
            return updata;
        }


        public override void frm_Model_ToolBar_ExportTo(CMenuItem mnu)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                UpdateData data = this.GetUpdateData(false, false);
                XmlDocument xsldoc = this.GetConfigXsl("OAPurchaseDetail.xsl");
                XslTransform tran = new XslTransform();
                XmlNode xmldoc = data.ToXml(null);
                //StringBuilder sb = new StringBuilder(xmldoc.OuterXml);
                TextReader xmlreader = new StringReader(xmldoc.OuterXml);
                TextReader xslreader = new StringReader(xsldoc.OuterXml);
                XmlReader xslrder = new XmlTextReader(xslreader);
                tran.Load(xslrder);
                StringBuilder htmlsb = new StringBuilder();
                TextWriter htmlwriter = new StringWriter(htmlsb);
                XPathDocument pathdoc = new XPathDocument(xmlreader);
                tran.Transform(pathdoc, null, htmlwriter, null);
                Clipboard.SetText(htmlsb.ToString());
                //Clipboard.s
                MessageBox.Show("已将数据复制到粘贴板，可直接粘贴到OA的html框内！");
            }
            catch (Exception ce)
            {
                MessageBox.Show(ce.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            
        }

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            this.lv_CloumnClick(sender as ListView, e);
        }

        private void frm_MainSubFrame_DockChanged(object sender, EventArgs e)
        {
            
        }

        private void splitContainer_detail_DockChanged(object sender, EventArgs e)
        {
            
            
        }

        private void toolStrip_subtitle_DockChanged(object sender, EventArgs e)
        {

        }

        private void splitContainer_detail_Panel2_Resize(object sender, EventArgs e)
        {
            int allheight = this.splitContainer_detail.Panel2.Height;
            int allwidth = this.splitContainer_detail.Panel2.Width;

            int left = panel_Title.Left;

            this.panel_subtoolbar.Left = left;
            this.panel_subtoolbar.Top = this.panel_Title.Height +this.panel_Title.Top + 2;
            this.panel_subtoolbar.Width = allwidth - 1;
            this.panel_subtoolbar.Height = allheight - this.panel_Title.Height;
            this.toolStrip1.Top = 1;
            this.toolStrip1.Left = left;
            this.listView1.Top = this.toolStrip1.Height + 5;
            this.listView1.Left = left;
            this.listView1.Width = allwidth ;
            this.listView1.Height = this.panel_subtoolbar.Height- toolStrip1.Height-36;
            this.listView1.Scrollable = true;
        }

        private bool Frm_MainSubFrame_ToolBar_BatchUpdate(CMenuItem mnu)
        {
            this.Cursor = Cursors.WaitCursor;
            bool succ = BatchUpdate(this.GridObj, mnu);
            this.Cursor = Cursors.Default;
            if (!succ)
            {
                return false;
                //MessageBox.Show("更新失败");
            }
            else
            {
                return true;
            }
            return false;
        }

        private void listView1_MouseUp(object sender, MouseEventArgs e)
        {
            this.listView1.ContextMenuStrip = this.contextMenuStrip1;
        }

        private bool frm_MainSubFrame_ToolBar_ChangeGroup(CMenuItem mnu)
        {
            return ChangeGridGroup(this.GridObj, mnu);
        }
    }

 }
