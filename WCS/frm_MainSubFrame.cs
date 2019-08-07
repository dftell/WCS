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

namespace WCS
{
    public partial class frm_MainSubFrame : frm_Model, ISaveableInterFace
    {
        

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

            if (GridObj.Lbl_Title != null)
                GridObj.Lbl_Title.Text = XmlUtil.GetSubNodeText(cmbNode, "@title");
            //ToolStripLabel lb = new ToolStripLabel(XmlUtil.GetSubNodeText(cmbNode,"@title") );
            //ToolBar.Items.Add(lb);
            XmlNode node = cmbNode.SelectSingleNode("toolbar");
            if (node == null) return;
            GridObj.ToolBar.Items.Clear();
            GridObj.ToolBar.RightToLeft = XmlUtil.GetSubNodeText(node, "@RightToLeft") == "1" ? RightToLeft.Yes : RightToLeft.No;
            XmlNodeList nodelist = node.SelectNodes("button");
            foreach (XmlNode bnode in nodelist)
            {
                string sPerm = XmlUtil.GetSubNodeText(bnode, "@perm");
                ToolStripButton btn = new ToolStripButton();
                CMenuItem mnu = MenuProcess.GetMenu(null, bnode,strUid);
                btn.Name = mnu.MnuId;
                btn.Text = mnu.MnuName;
                btn.Tag = mnu;
                btn.Enabled = !(sPerm == "0");
                btn.Click += new EventHandler(SubGrid_btn_Click);
                GridObj.ToolBar.Items.Add(btn);
            }
        }

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
            if (LoadControls() == false)
            {
                //this.LoadFlag = true;
                //this.LoadFlag = true;
                this.Cursor = Cursors.Default;
                return;
            }
            InitEditPanelDefaultValue();
            PanelObj.CoverData(this.NeedUpdateData, strRowId != "");
            if (strRowId != "")
            {
                string msg = null;
                GridData = InitDataSource(DetailSource, out msg);
                if (msg != null)
                {
                    this.Cursor = Cursors.Default;
                    //this.LoadFlag = true;
                    MessageBox.Show(msg);
                    return;
                }
                PanelObj.FillData(GridData);
                
            }
            if (strRowId != "")
            {
                string msg = null;
                DataSet ds = InitDataSource(GridSource, out msg);
                if (msg != null)
                {
                    this.Cursor = Cursors.Default;
                    //this.LoadFlag = true;
                    MessageBox.Show(msg);
                    return;
                }
                FillGridData(ds);
     
            }
            else
            {
                //外部数据
                if (this.NeedUpdateData != null && this.NeedUpdateData.SubItems!= null && this.NeedUpdateData.SubItems.Count > 0)
                {
                    FillGridData(NeedUpdateData.SubItems);
                }
            }
            this.LoadFlag = true;
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
                        if (!gr.Items.ContainsKey(dtm.FromDataPoint))//如果map中值不在grid ,contine
                        {
                            continue;
                        }
                        if (!subdata.Items.ContainsKey(dtm.ToDataPoint))
                        {
                            if (this.strKey == dtm.ToDataPoint)//如果主键在map中，将主健值赋予相应的列
                            {
                                gr.Items[dtm.FromDataPoint].value = strRowId;
                                gr.Items[dtm.FromDataPoint].Updated = true;
                            }
                            else
                            {
                                if (!GlobalShare.DataPointMappings.ContainsKey(dtm.ToDataPoint))
                                {
                                    gr.Items[dtm.FromDataPoint].value = dtm.ToDataPoint;//常量
                                    gr.Items[dtm.FromDataPoint].Updated = true;
                                }
                            }
                        }
                        else
                        {
                            gr.Items[dtm.FromDataPoint].value = subdata.Items[dtm.ToDataPoint].value;
                            gr.Items[dtm.FromDataPoint].Updated = true;
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
        
        public override bool Save()
        {
            return SaveData();
        }

        public  bool SaveData()
        {
            if (PanelObj == null)
                PanelObj = this.tableLayoutPanel1.Tag as EditPanel;
            if (PanelObj.ControlList == null) return false;
            int cnt = 0;
            UpdateData updata = this.GetUpdateData(true);
            if (!updata.Updated) return true;
            DataSource ds = GlobalShare.mapDataSource[DetailSource];
            List<DataCondition> conds = new List<DataCondition>();
            DataCondition dcond = new DataCondition();
            dcond.Datapoint = new DataPoint(this.strKey);
            dcond.value = this.strRowId;
            conds.Add(dcond);
            DataRequestType type = DataRequestType.Update;
            if (this.strRowId == null || this.strRowId == "")
            {
                type = DataRequestType.Add;
            }
            DataSource grid_source = GlobalShare.mapDataSource[this.GridSource];
            ds.SubSource = grid_source;
            string msg = GlobalShare.DataCenterClientObj.UpdateDataList(ds, dcond, updata, type);
            if (msg != null)
            {
                MessageBox.Show(msg);
                return false;
            }
            return true;
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
            PanelObj.Fill(cmbNode);
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

        protected void FillGridData(DataSet ds)
        {

            GridData = ds;
            if (GridObj == null)
                GridObj = this.listView1.Tag as SubGrid;
            if (GridObj == null) return;
            GridObj.FillGridData(ds);
            this.label_buttom.Text = string.Format("合计：{0} 条件记录", lvis.Count.ToString());
            this.listView1.Refresh();
        }

        protected void FillGridData(List<UpdateData> ds)
        {

            if (GridObj == null)
                GridObj = this.listView1.Tag as SubGrid;
            if (GridObj == null) return;
            GridObj.FillGridData(ds);
            this.label_buttom.Text = string.Format("合计：{0} 条件记录", lvis.Count.ToString());
            this.listView1.Refresh();
        }

        void SubGrid_btn_Click(object sender, EventArgs e)
        {
            CMenuItem mnu = (sender as ToolStripButton).Tag as CMenuItem;
            if (mnu == null) return;
            switch (mnu.MnuId)
            {
                case "Remove":
                    {
                        RemoveSubItem();
                        break;
                    }
                case "RemoveAll":
                    {
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
                default:
                    {
                        break;
                    }
            }
        }

        void AddNewSubItem(CMenuItem mnu)
        {
            UpdateData newData = new UpdateData();
            if (!FrameSwitch.ShowDialoger(null, this,mnu, ref newData))
            {
                return ;
            }
            if (newData == null) return ;
            if (GridObj == null)
                GridObj = this.listView1.Tag as SubGrid;
            GridRow gr = null;
            GridObj.GetNewRow(ref gr, newData);
            if (gr == null) return ;
            GridObj.Items.Add(gr);
            GridObj.FillListView();
            return ;
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
            for (int i = this.listView1.CheckedIndices.Count - 1;i>=0 ; i--)
            {
                GridRow gi = this.listView1.Items[this.listView1.CheckedIndices[i]].Tag as GridRow;
                if (gi == null) continue;
                GridObj.Items.Remove(gi);
                
            }
            GridObj.FillListView();
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (!this.LoadFlag) return;
            ListGridItem lvi = this.listView1.SelectedItems[0] as ListGridItem;

            //ListGridItem lvi = this.listView1.GetItemAt(tmpPoint.X, tmpPoint.Y);
            if (lvi == null) return;
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

        public override UpdateData GetUpdateData(bool CheckValueChanged, bool UpdateFrameData)
        {
            
            UpdateData updata = new UpdateData();
            updata.Items = new Dictionary<string,UpdateItem>();
            int cnt = 0;
            foreach (string dpt in PanelObj.ControlList.Keys)
            {
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
                updata.Items.Add(dpt,ui);
                cnt++;
            }
            if (GridObj == null)
                GridObj = this.listView1.Tag as SubGrid;
            updata.SubItems = GridObj.GetUpdateData(CheckValueChanged);
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
        



    }

 }
