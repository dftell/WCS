using System.Collections.Generic;
using WolfInv.Com.MetaDataCenter;
using System.Xml;
using XmlProcess;
using WolfInv.Com.CommCtrlLib;
using System.Windows.Forms;
using System;
using WolfInv.Com.WCS_Process;
using WolfInv.Com.XPlatformCtrlLib;

namespace WolfInv.Com.CommFormCtrlLib
{
    public class PanelCell :EditCell,IUserData, IPermmsionControl
    {
        public UpdateData CurrPanelData { get; set; }//整个Panel数据

        #region IPermmsionControl 成员
        string _permid;
        public string PermId
        {
            get
            {
                return _permid;
            }
            set
            {
                _permid = value;
            }
        }

        #endregion
        string _uid;
        public string strUid { get { return _uid; } set { _uid = value; } }
        #region 变量
        //public PanelRow OwnRow;
        CellViewItem vi = new CellViewItem();
        //public string Field;//值域
        //public string DisplayField;//显示域
        //public string Value = "";
        //public string Text = "";
        //public int Width;
        //public int Height;
        //public string Type;
        //public Label ItemLabel;
        //public Control ItemControl;
        //public bool ValueChanged;
        //public string DefaultValue;
        //public string DefaultText;
        //public CMenuItem mnu;
        //public string CtrolId;
        //public bool MultiLine;
        //public List<ResponseFieldClass> rpfs;
        //public string TargetField;
        //public bool ReadOnly;
        //public bool NoNull;
        #endregion
        public PanelCell(string uid)
        {
            strUid = uid;
        }

        public PanelCell(XmlNode node, string uid)
        {

            strUid = uid;
            LoadXml(node);
            vi.LoadXml(node);

        }

       

        public override void LoadXml(XmlNode node)
        {
            base.LoadXml(node);
            int _w = 0;
            int.TryParse(XmlUtil.GetSubNodeText(node, "@width"), out _w);
            Width = _w;
            //this.Height 
            this.Field = XmlUtil.GetSubNodeText(node, "@f");
            this.Text = XmlUtil.GetSubNodeText(node, "@text");
            this.Type = XmlUtil.GetSubNodeText(node, "@type");
            this.DisplayField = XmlUtil.GetSubNodeText(node, "@tf");
            this.DefaultValue = XmlUtil.GetSubNodeText(node, "@default");
            this.DefaultText = XmlUtil.GetSubNodeText(node, "@defaulttext");

            this.CtrolId = XmlUtil.GetSubNodeText(node, "@f");
            string strHeight = XmlUtil.GetSubNodeText(node, "@height");
            this.MultiLine = XmlUtil.GetSubNodeText(node, "@multiline") == "1";
            this.TargetField = XmlUtil.GetSubNodeText(node, "@targetfld");
            this.ReadOnly = XmlUtil.GetSubNodeText(node, "@readonly") == "1";
            this.NoNull = XmlUtil.GetSubNodeText(node, "@nonull") == "1";
            this.PermId = XmlUtil.GetSubNodeText(node, "@perm");
            if (strHeight.Trim().Length > 0)
            {
                int height = 0;
                int.TryParse(strHeight, out height);
                if (height > 0)
                {
                    this.Height = height;
                }
            }
            XmlNodeList evtnodes = node.SelectNodes("./evt");
            if (evtnodes.Count > 0)
            {
                this.rpfs = new List<ResponseFieldClass>();
                foreach (XmlNode evtnode in evtnodes)
                {
                    ResponseFieldClass rfc = new ResponseFieldClass();
                    rfc.CalcExpr = XmlUtil.GetSubNodeText(evtnode, "@expr");
                    rfc.CalcMethod = XmlUtil.GetSubNodeText(evtnode, "@method");
                    rfc.ForceUpdate = !(XmlUtil.GetSubNodeText(evtnode, "@forceupdate") == "0");
                    this.rpfs.Add(rfc);
                }
            }
        }

        protected override Control GetControl()
        {
            Control ctrl = null;
            //AutoResponseEventArgs arespargs = new AutoResponseEventArgs(this.TargetField);

            switch (this.Type)
            {

                case "combo":
                    {
                        ctrl = new ComboBox();
                        (ctrl as ComboBox).DropDownStyle = ComboBoxStyle.DropDownList;
                        if (vi.ComboName == null || vi.ComboName == "")//如果在view中未指定，用dataidpoint中默认的combo
                        {
                            vi.ComboName = GlobalShare.DataPointMappings[vi.dpt.Name].ComboName;
                        }
                        if (vi.ComboName == null || vi.ComboName == "")
                            break;
                        DataChoice dch = GlobalShare.GetGlobalChoice(_uid, vi.ComboName);
                        if (dch == null)//datachoice中不存在
                        {
                            break;
                        }
                        DataChoice dc = dch.Clone() as DataChoice;
                        ComboBox cbb = ctrl as ComboBox;
                        cbb.DataSource = dc.Options;
                        cbb.DisplayMember = "Text";
                        cbb.ValueMember = "Value";
                        cbb.SelectedIndex = -1;
                        cbb.Enabled = this.ReadOnly;
                        cbb.SelectedIndexChanged += OnControlValueChanged;
                        cbb.Show();
                        if (PermId == "0") cbb.Visible = false;
                        break;
                    }
                case "date":
                case "smalldatetime":
                case "datetime":
                    {
                        DateTimePicker dtp = new DateTimePicker();
                        dtp.ValueChanged += new EventHandler(dtp_ValueChanged);
                        //dtp.Value = DateTime.MaxValue.AddDays(-1);
                        dtp.Format = DateTimePickerFormat.Custom;
                        //dtp.Format = DateTimePickerFormat.Short;
                        dtp.CustomFormat = "yyyy-MM-dd";
                        dtp.CustomFormat = " ";

                        dtp.ValueChanged += new EventHandler(dtp_ValueChanged);
                        ctrl = dtp;
                        if (PermId == "0") dtp.Enabled = false;
                        //dtp.Enabled = !this.ReadOnly;
                        break;
                    }
                //13548560528 丢失的钱包
                case "int":
                case "money":
                case "float":
                case "double":
                    {
                        ctrl = new TextBox();
                        //ctrl.Enabled = !ReadOnly;
                        ctrl.KeyPress += new KeyPressEventHandler(Numberctrl_KeyUp);
                        (ctrl as TextBox).ReadOnly = this.ReadOnly;
                        (ctrl as TextBox).TextChanged += OnControlValueChanged;
                        //(ctrl as TextBox).ImeMode = ImeMode.OnHalf;
                        if (PermId == "0")
                            (ctrl as TextBox).Enabled = false;
                        break;
                    }
                case "text":
                case "varchar":
                case "nvarchar":
                default:
                    {

                        TextBox Textctrl = new TextBox();
                        Textctrl.Multiline = this.MultiLine;
                        Textctrl.ScrollBars = ScrollBars.Both;
                        if (this.rpfs != null && this.rpfs.Count > 0)
                        {
                            Textctrl.ReadOnly = true;
                        }
                        Textctrl.TextChanged += OnControlValueChanged;
                        Textctrl.ReadOnly = this.ReadOnly;
                        ctrl = Textctrl;
                        if (PermId == "0") Textctrl.Enabled = false;
                        //Textctrl.ImeMode = ImeMode.OnHalf ;
                        break;
                    }
            }
            if (ctrl == null) return ctrl;

            ctrl.Width = this.Width;
            ctrl.Height = this.Height;
            ctrl.Name = this.CtrolId;
            //
            if (PermId != "0")
            {
                ctrl.Enabled = !ReadOnly;
            }
            return ctrl;
        }

        public void FillLabel()
        {
            this.ItemLabel = new Label();
            this.ItemLabel.Margin = new Padding(3, 6, 0, 0);
            this.ItemLabel.Text = this.Text + (this.NoNull ? "(*)" : "");
        }
        protected virtual void OnControlValueChanged(object sender, EventArgs e)
        {
            
            if (this.TargetField != null && this.TargetField.Trim().Length > 0)
                ctrl_ValueChanged(new AutoResponseEventArgs(this.TargetField));
        }

        static void Numberctrl_KeyUp(object sender, KeyPressEventArgs e)
        {
            TextBox txtMoney = (sender as TextBox);
            int kc = e.KeyChar;
            if ((kc < 48 || kc > 57) && kc != 8)
            {
                if (kc == 46)                       //小数点
                {
                    if (txtMoney.Text.Length <= 0)
                        e.Handled = true;           //小数点不能在第一位
                    else
                    {
                        float f;
                        float oldf;
                        bool b1 = false, b2 = false;
                        b1 = float.TryParse(txtMoney.Text, out oldf);
                        b2 = float.TryParse(txtMoney.Text + e.KeyChar.ToString(), out f);
                        if (b2 == false)
                        {
                            if (b1 == true)
                                e.Handled = true;
                            else
                                e.Handled = false;
                        }
                        else
                        {
                            e.Handled = false;
                            return;
                        }
                    }
                }
                e.Handled = true;
            }

        }

        void dtp_ValueChanged(object sender, EventArgs e)
        {
            //(sender as DateTimePicker).Format = DateTimePickerFormat.Custom;
            (sender as DateTimePicker).CustomFormat = null;
            OnControlValueChanged(null, null);
        }

        public virtual string ChangeValue(string val, string text)
        {
            string strRet = val;
            if (Value != null && val == Value && this.Type != "combo" && this.Type != "datacombo")
            {
                return strRet;
            }

            switch (this.Type)
            {
                case "date":
                case "datetime":
                    {
                        DateTime dt;
                        DateTime.TryParse(val, out dt);
                        strRet = dt.ToShortDateString();
                        (this.ItemControl as DateTimePicker).Text = strRet;

                        break;
                    }

                case "combo":
                case "datacombo":
                    {
                        DataChoice dc = new DataChoice();
                        List<DataChoiceItem> dcs = (this.ItemControl as ComboBox).DataSource as List<DataChoiceItem>;
                        //List<DataChoiceItem> dcis = dcb.GetDataSource();
                        dc.Options = dcs;
                        int ind = dc.FindIndexByValue(val);
                        if (dc.Options != null && ind >= 0 && ind < dc.Options.Count)
                        {
                            DataChoiceItem dci = dc.Options[ind];

                            //

                            (this.ItemControl as ComboBox).SelectedValue = dci.Value;
                            //cb.SelectedIndex = ind;
                            if (this.ItemControl is DataComboBox)
                            {
                                this.OnControlValueChanged(null, null);
                            }
                        }
                        else
                        {
                            if((this.ItemControl as ComboBox).SelectedIndex>=0)
                                (this.ItemControl as ComboBox).SelectedIndex = -1;
                        }
                        break;
                    }


                default:
                    {
                        (this.ItemControl as TextBox).Text = val;
                        break;
                    }
            }
            return strRet;
        }

        public virtual string GetValue(bool IsText)
        {
            string ret = base.GetValue(IsText);
            switch (this.Type)
            {
                case "combo":
                case "datacombo":
                    {
                        if ((this.ItemControl as ComboBox).SelectedItem == null)
                        {
                            return "";
                        }
                        if (!IsText)
                            ret = ((this.ItemControl as ComboBox).SelectedItem as DataChoiceItem).Value;
                        else
                            ret = ((this.ItemControl as ComboBox).SelectedItem as DataChoiceItem).Text;
                        break;
                    }

                case "datetime":
                case "date":
                    {
                        ret = (this.ItemControl as DateTimePicker).Text;
                        break;
                    }
                default:
                    {
                        ret = (this.ItemControl as TextBox).Text;
                        break;
                    }

            }
            return ret;
        }

        protected virtual void ctrl_ValueChanged(AutoResponseEventArgs args)
        {
            args.TargetField = this.TargetField;
            if (!this.OwnRow.OwnPanel.ControlList.ContainsKey(this.TargetField))
                return;
            PanelCell pc = this.OwnRow.OwnPanel.ControlList[this.TargetField];
            if (pc == null)
                return;
            if (pc.rpfs != null && pc.rpfs.Count > 0)
            {
                for (int i = 0; i < pc.rpfs.Count; i++)//
                {
                    ResponseFieldClass rfc = pc.rpfs[i];
                    string retval = getEvalValue(rfc, this);
                    if (retval == null)
                        continue;
                    if (!rfc.ForceUpdate && (pc.GetValue(false) != null && pc.GetValue(false).Trim().Length > 0))
                        continue;//非强制更新如果目标字段已非空，不予处理 2011/02/16 zhouys
                    pc.ChangeValue(retval, pc.GetValue(true));
                    break;
                }
            }
            if (pc.ItemControl is DataComboBox)
            {
                UpdateData data = null;
                DataComboBox dcb = pc.ItemControl as DataComboBox;
                FrameSwitch.FillTranData(pc.OwnRow.OwnPanel.OwnerForm, dcb,ref pc.mnu, ref data);
                dcb.NeedUpdateData = data;
                dcb.ChangeDataSource();
            }
        }

        protected string getEvalValue(ResponseFieldClass rfc, PanelCell inputpc)
        {
            if (!rfc.CalcExpr.Contains(inputpc.Field))
            {
                return null;
            }
            string[] cols = rfc.CalcExpr.Split(',');
            if (cols.Length == 0)
                return null;
            //List<string> strparam = new List<string>();
            for (int i = 1; i < cols.Length; i++)
            {
                string dpt = cols[i];
                string val = dpt;//默认为常量
                if (inputpc.OwnRow.OwnPanel.ControlList.ContainsKey(dpt))//field pc
                {
                    PanelCell pc = inputpc.OwnRow.OwnPanel.ControlList[dpt];
                    val = pc.GetValue(false);
                    if (val == null || val.Trim().Length == 0)//引用的其中一个值为空，暂时不计算结果
                        return null;
                }
                string strindex = "{" + string.Format("{0}", i - 1) + "}";
                cols[0] = cols[0].Replace(strindex, val);
                //strparam.Add(val);
            }
            CalcExpr ce = new CalcExpr();
            HandleBase hb = ce.GetHandleClass(cols[0], rfc.CalcMethod);
            return hb.Handle();
            //return null;
        }

    }


}

