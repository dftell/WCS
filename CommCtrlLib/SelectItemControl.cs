using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Data;
using System.Reflection;
using System.Text;
using WolfInv.Com.MetaDataCenter;
using System.Windows.Forms;
using WolfInv.Com.WCS_Process;
using WolfInv.Com.XPlatformCtrlLib;
namespace WolfInv.Com.CommCtrlLib
{

    public delegate void SelectEventHandler(Object send, EventArgs e);
    public delegate void NewItemEventHandler(Object send, EventArgs e);
   


    public delegate void AutoCalcEventHandler(AutoResponseEventArgs args);

    
    
    public class AutoResponseEventArgs : EventArgs
    {
        public string TargetField;
        public AutoResponseEventArgs(string targetfld)
        {
            TargetField = targetfld;
        }
    }

    public class SelectItemControl : System.Windows.Forms.UserControl
    {
        protected TextBox txtctrl;
        protected Button newbtn;
        protected TextBox txtval;

        public CMenuItem SelectItemMenu;
        public CMenuItem NewItemMenu;
        public UpdateData ReturnData;
        string mValue;
        public string Value
        {
            get { return mValue; }
            set { mValue = value; }
        }


        public int TextBoxWidth
        {
            get
            {
                return txtctrl.Width;
            }
            set
            {
                txtctrl.Width = value;
                this.newbtn.Left = txtctrl.Width;
                this.Width = this.txtctrl.Width + this.newbtn.Width;
            }
        }

        public SelectItemControl(string value, string text)
        {
            Init();
            this.Text = text;
            this.Value = value;

        }

        public void ChangeValue(string val, string txt)
        {
            this.txtval.Text = val;
            this.txtctrl.Text = txt;
        }

        public string GetValue(bool IsText)
        {
            return IsText ? this.txtctrl.Text : this.txtval.Text;
        }

        public event SelectEventHandler SelectItem;
        public event NewItemEventHandler NewItem;

        public string Text
        {
            get { return this.txtctrl.Text; }
            set { this.txtctrl.Text = value; }
        }

        void Init()
        {
            txtval = new TextBox();
            txtval.Text = "";
            txtval.Visible = false;
            txtval.Width = 0;
            txtctrl = new TextBox();
            txtctrl.Top = 0;
            txtctrl.Left = 0;
            //txtctrl.Margin = new Padding(0, 3, 0, 0);
            txtctrl.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
            txtctrl.ReadOnly = true;
            newbtn = new Button();
            newbtn.Margin = new Padding(0);
            newbtn.AutoSize = false;
            newbtn.Top = 0;
            newbtn.Left = txtctrl.Width;
            newbtn.Text = "+";
            newbtn.Width = 20;
            newbtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.Width = this.txtctrl.Width + this.newbtn.Width;
            newbtn.Click += new EventHandler(newbtn_Click);
            txtctrl.Click += new EventHandler(txtctrl_Click);
            this.Controls.Add(txtctrl);
            this.Controls.Add(newbtn);
            this.Height = this.txtctrl.Height + 1;
        }

        void txtctrl_Click(object sender, EventArgs e)
        {
            this.SelectItem(this, e);
        }

        void newbtn_Click(object sender, EventArgs e)
        {
            this.NewItem(this, e);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // SelectItemControl
            // 
            this.Name = "SelectItemControl";
            this.Size = new System.Drawing.Size(141, 26);
            this.ResumeLayout(false);

        }


    }

    public class AutoCalcTextItem : TextBox
    {
        public string TargetField;

        public event AutoCalcEventHandler TextChanged;

        protected override void OnTextChanged(EventArgs e)
        {
            this.TextChanged(new AutoResponseEventArgs(this.TargetField));
        }
    }

    public class DataComboBox : ComboBox, ITranslateableInterFace
    {

        public string DataSourceName;
        public string ValField;
        public string TxtField;
        public string UId;
        public DataComboBox(string DataName,string uid)
        {
            DataSourceName = DataName;
            UId = uid;

        }

        #region ITranslateableInterFace 成员
        UpdateData _data;
        List<DataTranMapping> _maps;

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
                return _maps;
            }
            set
            {
                _maps = value;
            }
        }

        public UpdateData GetCurrFrameData()
        {
            return null;
        }

        #endregion



        public List<DataChoiceItem> GetDataSource()
        {
            if (this.DataSourceName == null || DataSourceName.Trim().Length == 0)
                return null;
            List<DataCondition> conds = new List<DataCondition>();
            if (this.TranData != null)
            {
                for (int i = 0; i < this.TranData.Count; i++)
                {
                    if (this.NeedUpdateData.Items.ContainsKey(TranData[i].ToDataPoint))
                    {
                        string strval = this.NeedUpdateData.Items[this.TranData[i].ToDataPoint].value;
                        if (strval == null || strval.Trim().Length == 0)
                        {
                            continue;
                        }
                        DataCondition cond = new DataCondition();
                        cond.Datapoint = new DataPoint(TranData[i].ToDataPoint);
                        cond.value = strval;
                        cond.Logic = ConditionLogic.And;
                        conds.Add(cond);
                    }
                }
            }
            string msg = null;
            DataSet ds = WCS_Process.DataSource.InitDataSource(this.DataSourceName, conds,this.Name,out msg);
            if (ds == null)
            {
                MessageBox.Show(string.Format("控件{0}无法获得数据！[{1}]", this.Name,msg));
                return null;
            }
            DataChoice dc = DataChoice.ConvertFromDataSet(ds, ValField, TxtField);
            if (dc == null)
            {
                MessageBox.Show(string.Format("无法转换数据选择项{0}", this.Name));
                return null;
            }
            if (!GlobalShare.DataChoices.ContainsKey(this.DataSourceName))//不断增加新的datachoice
            {
                GlobalShare.DataChoices.Add(this.DataSourceName, dc);
            }
            return dc.Options;
        }

        public void ChangeDataSource()
        {

            this.DataSource = GetDataSource();
            if (this.DataSource == null) return;
            this.ValueMember = "Value";
            this.DisplayMember = "Text";
            this.RefreshItems();
            this.SelectedIndex = -1;
        }
    }

    
}
