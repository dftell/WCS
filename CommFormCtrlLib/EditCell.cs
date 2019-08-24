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
    public class EditCell:DataControlItem
    {
        #region 变量
        public PanelRow OwnRow;
        CellViewItem vi = new CellViewItem();
        public string Field;//值域
        public string DisplayField;//显示域
        public string Value = "";

        public int Height;
        public string Type;
        public Label ItemLabel;
        public Control ItemControl;
        public bool ValueChanged;
        public string DefaultValue;
        public string DefaultText;
        public CMenuItem mnu;
        public string CtrolId;
        public bool MultiLine;
        public List<ResponseFieldClass> rpfs;
        public string TargetField;
        public bool ReadOnly;
        public bool NoNull;
        #endregion
        public EditCell()
        {
        }

        public EditCell(XmlNode node)
        {
            LoadXml(node);
            vi.LoadXml(node);

        }

        Control ValCtrl = null;
        public Control CurrMainControl
        {
            get
            {
                if (ValCtrl == null)
                {
                    ValCtrl = GetControl();
                }
                return ValCtrl;
            }
        }

        protected virtual Control GetControl()
        {
            
            return null;
        }


        public override void LoadXml(XmlNode node)
        {
            base.LoadXml(node);
            int _w = 0;
            int.TryParse(XmlUtil.GetSubNodeText(node, "@width"), out _w);
            this.Width = _w;
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
                    this.rpfs.Add(rfc);
                }
            }
        }



        public void FillLabel()
        {
            this.ItemLabel = new Label();
            //this.ItemLabel.Margin = new Padding(3, 6, 0, 0);
            this.ItemLabel.Text = this.Text + (this.NoNull ? "(*)" : "");
        }
        protected void OnControlValueChanged(object sender, EventArgs e)
        {
            if (this.TargetField != null && this.TargetField.Trim().Length > 0)
                ctrl_ValueChanged(new AutoResponseEventArgs(this.TargetField));
        }

        static void Numberctrl_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox txtMoney = (sender as TextBox);
            int kc = e.KeyValue;// e.KeyChar;
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
                        b2 = float.TryParse(txtMoney.Text + e.KeyValue.ToString(), out f);
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
            if (Value != null && val == Value)
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
                       
                        dc.Options = dcs;
                        
                        int ind = dc.FindIndexByValue(val);
                        if (dc.Options != null && ind >= 0 && ind < dc.Options.Count)
                        {
                            DataChoiceItem dci = dc.Options[ind];
                            

                            //

                            (this.ItemControl as ComboBox).SelectedValue = val;
                            //cb.SelectedIndex = ind;
                            if (this.ItemControl is BaseDataComboBox)
                            {
                                this.OnControlValueChanged(null, null);
                            }
                        }
                        else
                        {
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
            string ret = "";
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
            EditCell pc = this.OwnRow.OwnPanel.ControlList[this.TargetField];
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
                    pc.ChangeValue(retval, GetValue(true));
                    break;
                }
            }
            if (pc.ItemControl is BaseDataComboBox)
            {
                UpdateData data = null;
                BaseDataComboBox dcb = pc.ItemControl as BaseDataComboBox;
                ObjectSwith.FillTranData(pc.OwnRow.OwnPanel.OwnerForm, dcb, pc.mnu, ref data);
                dcb.NeedUpdateData = data;
                dcb.ChangeDataSource();
            }
        }

        protected string getEvalValue(ResponseFieldClass rfc, EditCell inputpc)
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
                    EditCell pc = inputpc.OwnRow.OwnPanel.ControlList[dpt];
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

