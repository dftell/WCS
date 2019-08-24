using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using WolfInv.Com.WCS_Process;
using System.Xml;
using System.IO;
using WolfInv.Com.MetaDataCenter;
using WolfInv.Com.XPlatformCtrlLib;
namespace WolfInv.Com.CommCtrlLib
{
    public partial class Dlg_CommModel : Form, IKeyForm, ITranslateableInterFace,IUserData ,IKeyTransable,ITag ,ILink
    {
        public bool MultiSelect;
        public IKeyForm Link;
        public string strModule;
        public string strScreen = "main";
        public string strTarget = "list";
        public string _strRowId = "";
        public string GridSource;
        public string DetailSource;
        public string _strKey;
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
        string _uid;
        public string strUid { get { return _uid; } set { _uid = value; } }
        public Grid GridObj;
        public DataSet GridData;
        UpdateData ReturnData;
        #region ITranslateableInterFace 成员
        List<DataTranMapping> _trandata;

        public UpdateData NeedUpdateData
        {
            get
            {
                return ReturnData;
            }
            set
            {
                ReturnData = value;
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

        public string strRefKey { get; set; }
        public string strRefRowId { get; set; }
        public List<DataTranMapping> RefData { get; set; }
        public List<UpdateData> InjectedDatas { get; set; }
        public CMenuItem FromMenu { get; set; }
        IKeyForm ILink.Link { get;set; }

        public UpdateData GetCurrFrameData()
        {
            return NeedUpdateData;
        }
        #endregion
        
        public Dlg_CommModel()
        {
            InitializeComponent();
        }

         bool LoadControls()
        {
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
            XmlDocument xmldoc = this.GetConfigXml();
            if (xmldoc == null)
            {
                return false;
            }
            InitGrid(xmldoc);
            BoundSearchType(xmldoc);
            return true;
        }

        protected XmlDocument GetConfigXml()
        {
            string strFilePath = string.Format("{0}\\{1}\\dlg_{1}_{2}_{3}.xml", "", strModule, strScreen, strTarget);
            return GlobalShare.UpdateWithUseInfo(GlobalShare.GetXmlFile(strFilePath), strUid) as XmlDocument;
        }
        
        void InitGrid(XmlDocument xmldoc)
        {
            XmlNode cmbNode = xmldoc.SelectSingleNode("/root/Grid");
            if (cmbNode == null)
            {
                return;
            }
            GridObj = new Grid(this);
            GridObj.listViewObj = this.listView1 ;
            GridObj.FillGrid(cmbNode);
            GridObj.AllowGroup = GridObj.listViewObj.AllowGroup;
            GridObj.GroupBy = GridObj.listViewObj.GroupBy;
            GridObj.AllowSum = GridObj.listViewObj.AllowSum;

        }

        void InitDataSource()
        {
            if (GridSource == null || GridSource.Trim().Length == 0)
                return;
            string msg = null;
            GridData = DataSource.InitDataSource(GridSource,InitBaseConditions(),strUid,out msg);

            //strRowId 
        }

        protected void FillData(DataSet ds)
        {

            GridData = ds;
            if (GridObj == null)
                GridObj = this.listView1.Tag as Grid;
            if (GridObj == null) return;
            GridObj.FillGridData(DataSource.DataSet2UpdateData(ds,this.GridSource,strUid));
            this.lbl_count.Text = string.Format("合计：{0} 条件记录", this.listView1.Items.Count.ToString());
            //this.listView1.Refresh();
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
                    if (GlobalShare.DataPointMappings.ContainsKey(data.FromDataPoint.Name))
                    {
                        continue;
                    }
                    DataCondition datacond = new DataCondition();
                    datacond.value = data.FromDataPoint.Text;
                    datacond.Datapoint = new DataPoint(data.ToDataPoint);
                    conds.Add(datacond);
                }
            }
            conds.Add(ds);
            return conds;
        }


        protected void RefreshData()
        {
            InitDataSource();
            //InitDataSource();
            if (GridData == null)
                return;
            FillData(GridData);
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }

        protected void BoundSearchType(XmlNode cmbNode)
        {
            XmlNodeList condnodes = cmbNode.SelectNodes("./root/SearchBox/items/c");
            List<DataCondition> conds = new List<DataCondition>();
            List<DataChoiceItem> dcs = new List<DataChoiceItem>();
            foreach (XmlNode cnode in condnodes)
            {

                DataCondition cond = new DataCondition();

                DataCondition.FillCondition(cnode, ref cond);
                conds.Add(cond);
                DataChoiceItem dci = new DataChoiceItem();
                dci.Value = cond.Datapoint.Name;
                if (GlobalShare.DataPointMappings.ContainsKey(dci.Value))
                {
                    dci.Text = GlobalShare.DataPointMappings[dci.Value].Text;
                }
                dcs.Add(dci);

            }
            this.comboBox_column.DataSource = dcs;
            this.comboBox_column.DisplayMember = "Text";
            this.comboBox_column.ValueMember = "Value";
            this.comboBox_column.Tag = conds;
        }

        private void btn_yes_Click(object sender, EventArgs e)
        {

            if (!MultiSelect)
            {
                if (this.listView1.SelectedItems.Count == 0)
                {
                    MessageBox.Show("请选择至少一行！");
                    return;
                }
                GridRow gr = this.listView1.SelectedItems[0].Tag as GridRow;
                if (gr == null)
                {
                    MessageBox.Show("无法获得绑定数据！");
                    return;
                }
                this.ReturnData = gr.ToUpdateData();
                if (ReturnData == null)
                {
                    MessageBox.Show("行数据转化失败！");
                    return;
                }

            }
            else
            {
                if (this.ReturnData == null)
                    this.ReturnData = new UpdateData();
                this.ReturnData.SubItems = new List<UpdateData>();
                foreach (ListGridItem lvi in this.listView1.CheckedItems)
                {
                    GridRow gr = lvi.Tag as GridRow;
                    if (gr == null)
                    {
                        MessageBox.Show("无法获得绑定数据！");
                        return;
                    }
                    UpdateData subdata = gr.ToUpdateData();
                    if (ReturnData == null)
                    {
                        MessageBox.Show("行数据转化失败！");
                        return;
                    }
                    this.ReturnData.SubItems.Add(subdata);
                }
            }
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void Dlg_CommModel_Load(object sender, EventArgs e)
        {
            this.listView1.CheckBoxes = this.MultiSelect;
            if (!LoadControls()) return;
            RefreshData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //base.searchbtn_Click(sender, e);
            if (this.comboBox_column.SelectedIndex < 0)
            {
                return;
            }
            List<DataCondition> dcs = this.comboBox_column.Tag as List<DataCondition>;
            DataCondition dc = dcs[this.comboBox_column.SelectedIndex];
            List<DataCondition> conds = InitBaseConditions();
            
            ////DataCondition ds = new DataCondition();
            ////ds.Datapoint = new DataPoint(this.strKey);
            ////ds.value = this.strRowId;
            ////ds.Logic = ConditionLogic.And;
            dc.Logic = ConditionLogic.And;
            dc.value = this.txt_searchkey.Text;
            ////if (dc.SubConditions == null || dc.SubConditions.Count == 0)
            ////{
            ////    this.RefreshData();
            ////    return;
            ////}
            ////foreach (DataCondition sdc in dc.SubConditions)
            ////{
            ////    sdc.value = txtbox.Text;
            ////    sdc.Logic = ConditionLogic.Or;
            ////}
            ////conds.Add(ds);
            conds.Add(dc);
            if (GridSource == null || GridSource.Trim().Length == 0)
                return;
            string msg = null;
            GridData = DataSource.InitDataSource(GridSource, conds,strUid ,out msg);
            if (msg != null)
            {
                MessageBox.Show(msg);
                return;
            }
            if (GridData == null)
            {
                MessageBox.Show("返回结果为空！");
                return;
            }
            FillData(GridData);
            //RefreshData();
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            this.btn_yes_Click(null, null);
        }



        private void txt_searchkey_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyData == Keys.Enter)
            //{
            //    this.button1_Click(null, null);
            //    e.Handled = true;
            //    return;
            //}
        }

        private void txt_searchkey_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (e.KeyChar == 0)
            //{
            //    e.Handled = true;
            //    return;
            //}
        }
    }
}