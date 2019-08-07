using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using System.Xml;
using XmlProcess;
using ITMS_Process;
using MetaDataCenter;
using System.ComponentModel.Design;
using CommCtrlLib;
namespace CommWebCtrlLib
{
    public class EditPanel
    {
        public IKeyForm OwnerForm;
        public int RowCnt;
        public int ColumnCnt;
        public List<PanelRow> Rows = new List<PanelRow>();
        public Dictionary<string, PanelCell> ControlList = new Dictionary<string, PanelCell>();
        public EditPanel()
        {
        }



        public EditPanel Fill(XmlNode node)
        {
            
            XmlNodeList nodes = node.SelectNodes("row");
            this.Rows.Clear();
            this.ControlList.Clear();
            this.RowCnt = 0;
            this.ColumnCnt = 0;
            int nCols = 0;
            for (int i = 0; i < nodes.Count; i++)
            {
                
                PanelRow pr = new PanelRow();
                pr.OwnPanel = this;
                this.Rows.Add(pr);
                pr.Visual = !(XmlUtil.GetSubNodeText(nodes[i], "@hide") == "1");
                XmlNodeList cellnodes = nodes[i].SelectNodes("cell");
                for (int c = 0; c < cellnodes.Count; c++)
                {
                    PanelCell pc = null;
                    string strtype = XmlUtil.GetSubNodeText(cellnodes[c], "@type");
                    switch (strtype)
                    {
                        case "select":
                            {
                                pc = new SelectPenelCell(cellnodes[c]);
                                break;
                            }
                        case "datacombo":
                            {
                                pc = new DataComboPenelCell(cellnodes[c]);
                                break;
                            }
                        case "text":
                        case "int":
                        case "money":
                        default:
                            {
                                pc = new PanelCell(cellnodes[c]);
                                break;
                            }
                    }
                    pc.OwnRow = pr;
                    pc.ItemControl = pc.GetControl();
                    pc.FillLabel();
                    pc.ItemControl.Visible = pr.Visual;
                    pc.ItemLabel.Visible = pr.Visual;

                    if (!this.ControlList.ContainsKey(pc.Field))
                    {
                        this.ControlList.Add(pc.Field, pc);
                    }
                    
                    
                    pr.Cells.Add(pc);
                    pc.ChangeValue(pc.DefaultValue, pc.DefaultText);//设默认值
                }
                if (cellnodes.Count * 2 > this.ColumnCnt)
                {
                    this.ColumnCnt = cellnodes.Count * 2;
                }
                
               
            }
            this.RowCnt = nodes.Count;
            return this;
        }

        public void FillData(DataSet ds)
        {
            if (ControlList == null) return;
            if (ds == null) return;
            if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
            foreach (string key in ControlList.Keys)
            {
                PanelCell pc = ControlList[key];

                string val = "";
                string txt = "";
                if(pc.DisplayField != null && pc.DisplayField != "" && ds.Tables[0].Columns.Contains(pc.DisplayField))
                {
                    txt = ds.Tables[0].Rows[0][pc.DisplayField].ToString();
                }
                val = pc.ChangeValue(ds.Tables[0].Rows[0][pc.Field].ToString(), txt);
                pc.Value = val;

            }
        }
        
        public void CoverData(UpdateData data)
        {
            CoverData(data, false);
        }
        
        public void CoverData(UpdateData data,bool ChangeValue)
        {
            if (ControlList == null) return;
            if (data == null || data.Items == null || data.Items.Count == 0) return;
            foreach (string key in ControlList.Keys)
            {
                if (!data.Items.ContainsKey(key)) 
                    continue;
                PanelCell pc = ControlList[key];
                string txt = "";
                if (pc.DisplayField != null && pc.DisplayField != "" && data.Items.ContainsKey(pc.DisplayField))
                {
                    txt = data.Items[pc.DisplayField].value;
                }
                string val = pc.ChangeValue(data.Items[key].value,txt);
                if(ChangeValue)
                    pc.Value = val;

            }
        }

        public string CheckNull()
        {
            foreach (string strkey in ControlList.Keys)
            {
                PanelCell pc = ControlList[strkey];
                string val = pc.DefaultValue;
                string datava = pc.GetValue(false);
                if (datava != null && datava.Trim().Length > 0)
                {
                    val = datava;
                }
                if ((val == null || val.Trim().Length == 0) && pc.NoNull)
                {
                    
                    return string.Format("{0}不能为空！",pc.Text);
                }
            }
            return null;
        }
    }

    public class PanelRow
    {
        public int Index;
        public bool Visual = true;
        public EditPanel OwnPanel;
        public List<PanelCell> Cells = new List<PanelCell>();
    }
    
    public class ResponseFieldClass
    {
        public string CalcMethod;
        public string CalcExpr;
    }
   
    public class PanelCell
    {
        #region 变量
        public PanelRow OwnRow;
        CellViewItem vi = new CellViewItem();
        public string Field;//值域
        public string DisplayField;//显示域
        public string Value = "";
        public string Text = "";
        public int Width;
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
        public PanelCell()
        {
        }

        public PanelCell(XmlNode node)
        {
            LoadXml(node);
            vi.LoadXml(node);
         
        }

        public virtual void LoadXml(XmlNode node)
        {
            int.TryParse(XmlUtil.GetSubNodeText(node, "@width"), out this.Width);
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

        public virtual Control GetControl()
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
                        if (!GlobalShare.DataChoices.ContainsKey(vi.ComboName))//datachoice中不存在
                        {
                            break;
                        }
                        DataChoice dc = GlobalShare.DataChoices[vi.ComboName].Clone() as DataChoice;
                        ComboBox cbb = ctrl as ComboBox;
                        cbb.DataSource = dc.Options;
                        cbb.DisplayMember = "Text";
                        cbb.ValueMember = "Value";
                        cbb.SelectedIndex = -1;
                        cbb.SelectedIndexChanged += OnControlValueChanged;
                        cbb.Show();
                        
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
                       
                        dtp.ValueChanged +=new EventHandler(dtp_ValueChanged);
                        ctrl = dtp;
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
                        (ctrl as TextBox).ImeMode = ImeMode.Off;
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
                        Textctrl.ImeMode = ImeMode.OnHalf;
                        break;
                    }
            }
            if (ctrl == null) return ctrl;
            
            ctrl.Width = this.Width;
            ctrl.Height = this.Height;
            ctrl.Name = this.CtrolId;
            //
            ctrl.Enabled = !ReadOnly;
            return ctrl;
        }

        public void FillLabel()
        {
            this.ItemLabel = new Label();
            this.ItemLabel.Margin = new Padding(3, 6, 0, 0);
            this.ItemLabel.Text = this.Text + (this.NoNull?"(*)":"");
        }
        protected void OnControlValueChanged(object sender, EventArgs e)
        {
            if (this.TargetField != null && this.TargetField.Trim().Length > 0)
                ctrl_ValueChanged(new AutoResponseEventArgs(this.TargetField));
        }
  
        static void Numberctrl_KeyUp(object sender, KeyPressEventArgs e)
        {
            TextBox txtMoney =(sender as TextBox);
            int kc = e.KeyChar;
            if ((kc < 48 || kc > 57) && kc != 8)
            { 
                if(kc == 46)                       //小数点
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

        public virtual string ChangeValue(string val,string text)
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
                            if (this.ItemControl is DataComboBox)
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
                    pc.ChangeValue(retval, GetValue(true));
                    break;
                }
            }
            if (pc.ItemControl is DataComboBox)
            {
                UpdateData data = null;
                DataComboBox dcb = pc.ItemControl as DataComboBox;
                FrameSwitch.FillTranData(pc.OwnRow.OwnPanel.OwnerForm, dcb, pc.mnu, ref data);
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

    public class SelectPenelCell:PanelCell
    {
        public Dictionary<string, string> Mappings = new Dictionary<string, string>();
        public CMenuItem NewItemMnu;
        public CMenuItem SelectItemMnu;

        public SelectPenelCell(XmlNode node)
            : base(node)
        {
        }

        public override void LoadXml(XmlNode node)
        {
            base.LoadXml(node);
            XmlNodeList maps = node.SelectNodes("./Mappings/Map");
            foreach (XmlNode mnode in maps)
            {
                string toField = XmlUtil.GetSubNodeText(mnode, "@i");
                string FromField = XmlUtil.GetSubNodeText(mnode, "@v");
                if (!Mappings.ContainsKey(toField))
                {
                    Mappings.Add(toField, FromField);
                }
            }
            XmlNode mnunode = node.SelectSingleNode("./menu[@id='NewItem']");
            if (mnunode != null)
            {
                NewItemMnu = MenuProcess.GetMenu(NewItemMnu, mnunode);
            }
            mnunode = node.SelectSingleNode("./menu[@id='SelectItem']");
            if (mnunode != null)
            {
                SelectItemMnu = MenuProcess.GetMenu(NewItemMnu, mnunode);
            }

        }

        public override  string ChangeValue(string val,string text)
        {
            SelectItemControl sic = this.ItemControl as SelectItemControl;
            //sic.Value = val;
            sic.ChangeValue(val, text);
            return val;
        }

        public override string GetValue(bool IsText)
        {
            SelectItemControl sic = this.ItemControl as SelectItemControl;
            return sic.GetValue(IsText);
        }

        public override Control GetControl()
        {
            SelectItemControl sic = new SelectItemControl(this.Value, this.Text);
            sic.Width  = this.Width;
            sic.NewItem += new NewItemEventHandler(sic_NewItem);
            sic.SelectItem +=new SelectEventHandler(sic_SelectItem);
            return sic;
        }

        void sic_NewItem(object send, EventArgs e)
        {
            MessageBox.Show("lkdjlkjd");
            return;
            
        }

        void sic_SelectItem(object send, EventArgs e)
        {
            SelectItemControl sic = send as SelectItemControl;
            UpdateData ret = null;
            
            if (sic == null) return;
            if (this.SelectItemMnu == null)
            {
                MessageBox.Show("未设定选择事件！");
                return;
            }
            if (!FrameSwitch.ShowDialoger(null,this.OwnRow.OwnPanel.OwnerForm, this.SelectItemMnu, ref ret))
            {
                return;
            }
            if (ret == null)
            {
                MessageBox.Show("选择的值为空！");
                return;
            }
            Dictionary<string, PanelCell> pcs = this.OwnRow.OwnPanel.ControlList;
            if (pcs == null) return;
            foreach (string tostr in this.Mappings.Keys)
            {
                if (!pcs.ContainsKey(tostr))//需要传递值的控件不存在，跳过
                {
                    continue;
                }
                PanelCell pc = pcs[tostr];
                string fromstr = this.Mappings[tostr];
                if (!ret.Items.ContainsKey(fromstr))
                {
                    pc.ChangeValue(fromstr, "");//如果返回的数据中不存在该数据点，认定赋一个固定值给控件
                }
                else
                {
                    pc.ChangeValue(ret.Items[fromstr].value, ret.Items[fromstr].text);//传递返回的值
                }
            }
            if (pcs.ContainsKey(this.DisplayField) && ret.Items.ContainsKey(this.DisplayField))
            {
                pcs[this.DisplayField].ChangeValue(ret.Items[this.DisplayField].value, ret.Items[this.DisplayField].value);
                sic.Text = ret.Items[this.DisplayField].value;
            }
            //this.OwnRow.OwnPanel.CoverData(ret);
            return;
            
        }
    }

    public class DataComboPenelCell : PanelCell
    {
        public string DataSource;
        public string ValueField;
        public string TextField;

        public DataComboPenelCell(XmlNode node)
            : base(node)
        {
            
        }

        public override void LoadXml(XmlNode node)
        {
            base.LoadXml(node);
            DataSource = XmlUtil.GetSubNodeText(node, "@datasource");
            ValueField = XmlUtil.GetSubNodeText(node, "@valmember");
            TextField = XmlUtil.GetSubNodeText(node, "@txtmember");
            mnu = MenuProcess.GetMenu(null, node);
        }

        public override Control GetControl()
        {
            DataComboBox ctrl = new DataComboBox(DataSource);
            ctrl.TxtField = TextField;
            ctrl.ValField = ValueField;
            ctrl.Name = this.Field;
            UpdateData data = new UpdateData();
            FrameSwitch.FillTranData(this.OwnRow.OwnPanel.OwnerForm,ctrl,this.mnu,ref data);
            ctrl.NeedUpdateData = data;
            ctrl.ChangeDataSource();
            ctrl.SelectedIndexChanged += OnControlValueChanged;
            ctrl.Width = this.Width;
            return ctrl;
        }

        
        
        
        
      
        public void RefreshItems()
        {
            DataComboBox ctrl = ItemControl as DataComboBox;
            ctrl.ChangeDataSource();
        }
    }

    
}
