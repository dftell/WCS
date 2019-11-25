using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using XmlProcess;
using WolfInv.Com.WCS_Process;
using System.IO;
using System.Xml;
using WolfInv.Com.MetaDataCenter;
using WolfInv.Com.CommFormCtrlLib;

using WolfInv.Com.CommCtrlLib;


using System.Collections;
using System.Runtime.InteropServices;

using System.Diagnostics;
using System.Drawing.Drawing2D;
using WolfInv.Com.XPlatformCtrlLib;

namespace WCS
{
    public partial class frm_frame : frm_Model,ITranslateableInterFace
    {




        public DataSet GridData;
        EditPanel PanelObj;
        public frm_frame()
        {
            InitializeComponent();
        }

        public frm_frame(string strrowid)
        {
            InitializeComponent();
            strRowId = strrowid;
        }

        private void frm_frame_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (NeedUpdateData == null)
                NeedUpdateData = DataSource.getDefaultData(DetailSource, this.strUid);
            if (!LoadControls()) return;
            InitEditPanelDefaultValue();
            PanelObj.CoverData(this.NeedUpdateData,this.strRowId!="");
            string msg = null;
            
            if (msg != null)
            {
                MessageBox.Show(msg);
                return;
            }
            if (strRowId != "")
            {
                GridData = InitDataSource(DetailSource, out msg);
                PanelObj.FillData(GridData,DetailSource);
            }
            
            this.Refresh();
            this.Cursor = Cursors.Default;
        }

        public override bool LoadControls()
        {
            this.toolStrip1.Width = this.Width - 1;
            XmlDocument xmldoc = this.GetConfigXml();
            if (xmldoc == null) return false;
            InitEditPanel(xmldoc);
            return true;

        }

        ////void InitDataSource()
        ////{
        ////    if (DetailSource == null || DetailSource.Trim().Length == 0)
        ////        return;
        ////    GridData = DataSource.InitDataSource(DetailSource, new string[1] { strKey }, new string[1] { strRowId });

        ////    //strRowId 

        ////}
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
            return SaveData(mnu);
        }
        public bool SaveClientData(CMenuItem mnu, UpdateData updata, DataRequestType type = DataRequestType.Update)
        {
            return false;
        }
        public override bool SaveData(CMenuItem mnu, DataRequestType type= DataRequestType.Update)
        {
            bool ret = base.SaveData(mnu, type);
            if (!ret)
                return ret;
            return true;
            UpdateData updata = GetUpdateData(true);
            //GlobalShare.DataCenterClientObj
            if (updata.Items.Count  == 0) return true;
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
            string msg = GlobalShare.DataCenterClientObj.UpdateDataList(ds, dcond, updata, type);
            if (msg != null)
            {
                MessageBox.Show(msg);
                return false;
            }
            return true;
        }

        void InitEditPanelDefaultValue()
        {
            foreach (PanelCell pc in PanelObj.ControlList.Values)
            {
                pc.ChangeValue(pc.DefaultValue, pc.DefaultText);
            }
        }

        void InitEditPanel(XmlNode xmldoc)
        {
            UpdateData ud = this.NeedUpdateData;
            if(ud == null)
            {
                
            }
            XmlNode cmbNode = xmldoc.SelectSingleNode("/root/EditPanel");
            if (cmbNode == null)
            {
                return;
            }
            PanelObj = new EditPanel(strUid);
            PanelObj.OwnerForm = this;
            PanelObj.Fill(cmbNode,ud);
            
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
                        this.tableLayoutPanel1.SetColumnSpan(pc.ItemControl,Math.Max(PanelObj.ColumnCnt - pr.Cells.Count * 2 + 1,1));
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

        public override UpdateData GetUpdateData(bool CheckValueChanged)
        {
            return GetUpdateData(CheckValueChanged, true);
        }

        public override UpdateData GetUpdateData(bool CheckValueChanged, bool UpdateFrameData,bool getText=false)
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
            updata.keydpt = new DataPoint(this.strKey);
            updata.keyvalue = this.strRowId;
            if (cnt == 0)
                updata.Updated = false;
            else
                updata.Updated = true;
            if(UpdateFrameData)
                NeedUpdateData = updata;
            NeedUpdateData.ReqType = DataRequestType.Update;
            if (updata.keyvalue.Trim().Length == 0)
                NeedUpdateData.ReqType = DataRequestType.Add;
            return updata;
        }

        private void btn_close(object sender, EventArgs e)
        {
            this.Dispose();
        }

        
    }
    
}
