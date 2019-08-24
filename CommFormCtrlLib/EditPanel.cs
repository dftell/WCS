using System.Collections.Generic;
using WolfInv.Com.MetaDataCenter;
using System.Xml;
using System.Data;
using XmlProcess;
using WolfInv.Com.CommCtrlLib;
using System.Windows.Forms;
using WolfInv.Com.XPlatformCtrlLib;

namespace WolfInv.Com.CommFormCtrlLib
{



    //////public class SelectPenelCell_bak : EditCell
    //////{
    //////    public Dictionary<string, string> Mappings = new Dictionary<string, string>();
    //////    public CMenuItem NewItemMnu;
    //////    public CMenuItem SelectItemMnu;

    //////    public SelectPenelCell_bak(XmlNode node)
    //////        : base(node)
    //////    {
    //////    }

    //////    public override void LoadXml(XmlNode node)
    //////    {
    //////        base.LoadXml(node);
    //////        XmlNodeList maps = node.SelectNodes("./Mappings/Map");
    //////        foreach (XmlNode mnode in maps)
    //////        {
    //////            string toField = XmlUtil.GetSubNodeText(mnode, "@i");
    //////            string FromField = XmlUtil.GetSubNodeText(mnode, "@v");
    //////            if (!Mappings.ContainsKey(toField))
    //////            {
    //////                Mappings.Add(toField, FromField);
    //////            }
    //////        }
    //////        XmlNode mnunode = node.SelectSingleNode("./menu[@id='NewItem']");
    //////        string name = mnunode.Value;//不知到底是什么
    //////        if (mnunode != null)
    //////        {
    //////            NewItemMnu = new CMenuItem(name).GetMenu(NewItemMnu, mnunode);
    //////        }
    //////        mnunode = node.SelectSingleNode("./menu[@id='SelectItem']");
    //////        if (mnunode != null)
    //////        {
    //////            SelectItemMnu = new CMenuItem(name).GetMenu(NewItemMnu, mnunode);
    //////        }

    //////    }

    //////    public override string ChangeValue(string val, string text)
    //////    {
    //////        BaseSelectItemControl sic = this.ItemControl as BaseSelectItemControl;
    //////        //sic.Value = val;
    //////        sic.ChangeValue(val, text);
    //////        return val;
    //////    }

    //////    public override string GetValue(bool IsText)
    //////    {
    //////        BaseSelectItemControl sic = this.ItemControl as BaseSelectItemControl;
    //////        return sic.GetValue(IsText);
    //////    }

    //////    public override Control GetControl()
    //////    {
    //////        BaseSelectItemControl sic = new BaseSelectItemControl(this.Value, this.Text);
    //////        sic.Width = this.Width;
    //////        sic.NewItem += new NewItemEventHandler(sic_NewItem);
    //////        sic.SelectItem += new SelectEventHandler(sic_SelectItem);
    //////        return sic;
    //////    }

    //////    void sic_NewItem(object send, EventArgs e)
    //////    {
    //////        //MessageBox.Show("lkdjlkjd");
    //////        return;

    //////    }

    //////    void sic_SelectItem(object send, EventArgs e)
    //////    {
    //////        BaseSelectItemControl sic = send as BaseSelectItemControl;
    //////        UpdateData ret = null;

    //////        if (sic == null) return;
    //////        if (this.SelectItemMnu == null)
    //////        {
    //////            //MessageBox.Show("未设定选择事件！");
    //////            return;
    //////        }
    //////        if (!FrameSwitch.ShowDialoger(null, this.OwnRow.OwnPanel.OwnerForm, this.SelectItemMnu, ref ret))
    //////        {
    //////            return;
    //////        }
    //////        if (ret == null)
    //////        {
    //////            MessageBox.Show("选择的值为空！");
    //////            return;
    //////        }
    //////        Dictionary<string, EditCell> pcs = this.OwnRow.OwnPanel.ControlList;
    //////        if (pcs == null) return;
    //////        foreach (string tostr in this.Mappings.Keys)
    //////        {
    //////            if (!pcs.ContainsKey(tostr))//需要传递值的控件不存在，跳过
    //////            {
    //////                continue;
    //////            }
    //////            EditCell pc = pcs[tostr];
    //////            string fromstr = this.Mappings[tostr];
    //////            if (!ret.Items.ContainsKey(fromstr))
    //////            {
    //////                pc.ChangeValue(fromstr, "");//如果返回的数据中不存在该数据点，认定赋一个固定值给控件
    //////            }
    //////            else
    //////            {
    //////                pc.ChangeValue(ret.Items[fromstr].value, ret.Items[fromstr].text);//传递返回的值
    //////            }
    //////        }
    //////        if (pcs.ContainsKey(this.DisplayField) && ret.Items.ContainsKey(this.DisplayField))
    //////        {
    //////            pcs[this.DisplayField].ChangeValue(ret.Items[this.DisplayField].value, ret.Items[this.DisplayField].value);
    //////            sic.Text = ret.Items[this.DisplayField].value;
    //////        }
    //////        //this.OwnRow.OwnPanel.CoverData(ret);
    //////        return;

    //////    }
    //////}

    public class DataComboPenelCell_bak : EditCell,IDataSourceable
    {
        public string DataSourceName { get; set; }
        public string ValueField { get; set; }
        public string TextField { get; set; }
        public string ComboItemsSplitString { get; set; }
        public string Id;
        public DataComboPenelCell_bak(XmlNode node)
            : base(node)
        {

        }

        public override void LoadXml(XmlNode node)
        {
            base.LoadXml(node);
            DataSourceName = XmlUtil.GetSubNodeText(node, "@datasource");
            ValueField = XmlUtil.GetSubNodeText(node, "@valmember");
            TextField = XmlUtil.GetSubNodeText(node, "@txtmember");
            ComboItemsSplitString = XmlUtil.GetSubNodeText(node, "@membersplitor");
            Id = XmlUtil.GetSubNodeText(node, "@id");
            mnu = new CMenuItem(Id).GetMenu(null, node);
        }

        protected virtual Control GetControl()
        {
            BaseDataComboBox ctrl = new BaseDataComboBox(DataSourceName);
            ctrl.TextField = TextField;
            ctrl.ValueField = ValueField;
            ctrl.Name = this.Field;
            UpdateData data = new UpdateData();
            FrameSwitch.FillTranData(this.OwnRow.OwnPanel.OwnerForm, ctrl,ref mnu, ref data);
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

    public class EditPanel_bak:EditPanel
    {
        public string Id;
        public IKeyForm OwnerForm;
        public int RowCnt;
        public int ColumnCnt;
        public List<PanelRow> Rows = new List<PanelRow>();
        public Dictionary<string, EditCell> ControlList = new Dictionary<string, EditCell>();
        ////public EditPanel_bak()
        ////{
        ////}

        public EditPanel_bak(string id):base(id)
        {
            Id = id;
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
                    EditCell pc = null;
                    string strtype = XmlUtil.GetSubNodeText(cellnodes[c], "@type");
                    switch (strtype)
                    {
                        case "select":
                            {
                                pc = new SelectPenelCell(cellnodes[c].Value);
                                break;
                            }
                        case "datacombo":
                            {
                                pc = new DataComboPenelCell(cellnodes[c].Value);
                                break;
                            }
                        case "text":
                        case "int":
                        case "money":
                        default:
                            {
                                pc = new EditCell(cellnodes[c]);
                                break;
                            }
                    }
                    pc.OwnRow = pr;
                    pc.ItemControl = pc.CurrMainControl;
                    pc.FillLabel();
                    pc.ItemControl.Visible = pr.Visual;
                    pc.ItemLabel.Visible = pr.Visual;

                    if (!this.ControlList.ContainsKey(pc.Field))
                    {
                        this.ControlList.Add(pc.Field, pc);
                    }


                    pr.Cells.Add(pc as PanelCell);
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
                EditCell pc = ControlList[key];

                string val = "";
                string txt = "";
                if (pc.DisplayField != null && pc.DisplayField != "" && ds.Tables[0].Columns.Contains(pc.DisplayField))
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

        public void CoverData(UpdateData data, bool ChangeValue)
        {
            if (ControlList == null) return;
            if (data == null || data.Items == null || data.Items.Count == 0) return;
            foreach (string key in ControlList.Keys)
            {
                if (!data.Items.ContainsKey(key))
                    continue;
                EditCell pc = ControlList[key];
                string txt = "";
                if (pc.DisplayField != null && pc.DisplayField != "" && data.Items.ContainsKey(pc.DisplayField))
                {
                    txt = data.Items[pc.DisplayField].value;
                }
                string val = pc.ChangeValue(data.Items[key].value, txt);
                if (ChangeValue)
                    pc.Value = val;

            }
        }

        public string CheckNull()
        {
            foreach (string strkey in ControlList.Keys)
            {
                EditCell pc = ControlList[strkey];
                string val = pc.DefaultValue;
                string datava = pc.GetValue(false);
                if (datava != null && datava.Trim().Length > 0)
                {
                    val = datava;
                }
                if ((val == null || val.Trim().Length == 0) && pc.NoNull)
                {

                    return string.Format("{0}不能为空！", pc.Text);
                }
            }
            return null;
        }
    }
   

    public class EditPanel : IUserData
    {
        string _uid;
        public string strUid { get { return _uid; } set { _uid = value; } }
        public IKeyForm OwnerForm;
        public int RowCnt;
        public int ColumnCnt;
        public List<PanelRow> Rows = new List<PanelRow>();
        public Dictionary<string, PanelCell> ControlList = new Dictionary<string, PanelCell>();
        public int Height;
        public EditPanel(string uid)
        {
            strUid = uid;
        }



        public EditPanel Fill(XmlNode node,UpdateData ud)
        {
            int height = 0;
            if (int.TryParse(XmlUtil.GetSubNodeText(node, "@height"), out height))
            {
                this.Height = height;
            }
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
                                pc = new SelectPenelCell(cellnodes[c], strUid);
                                break;
                            }
                        case "datacombo":
                            {
                                pc = new DataComboPenelCell(cellnodes[c], strUid);
                                break;
                            }
                        case "text":
                        case "int":
                        case "money":
                        default:
                            {
                                pc = new PanelCell(cellnodes[c], strUid);
                                break;
                            }
                    }
                    //if(XmlUtil.GetSubNodeText(nodes[i], "@hide") == "1")
                    //{
                    //    continue;
                    //}
                    pc.LoadXml(cellnodes[c]);
                    pc.OwnRow = pr;
                    pc.ItemControl = pc.CurrMainControl;
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

                if (pc.DisplayField != null && pc.DisplayField != "" && ds.Tables[0].Columns.Contains(pc.DisplayField))
                {
                    txt = ds.Tables[0].Rows[0][pc.DisplayField].ToString();
                }
                else
                {
                    //continue;
                }
                if (ds.Tables[0].Columns.Contains(pc.Field))
                {
                    val = pc.ChangeValue(ds.Tables[0].Rows[0][pc.Field].ToString(), txt);
                    pc.Value = val;
                }

            }
        }

        public void CoverData(UpdateData data)
        {
            CoverData(data, false);
        }

        public void CoverData(UpdateData data, bool ChangeValue)
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
                string val = pc.ChangeValue(data.Items[key].value, txt);
                if (ChangeValue)
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

                    return string.Format("{0}不能为空！", pc.Text);
                }
            }
            return null;
        }
    }


}

