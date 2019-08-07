using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using System.Xml;
using XmlProcess;
using WolfInv.Com.WCS_Process;
using WolfInv.Com.MetaDataCenter;
using System.Data;
using System.IO;
namespace CommFormCtrlLib
{
    public class Grid
    {
        public List<DataGridColumn> Columns = new List<DataGridColumn>();

        Dictionary<string, DataGridColumn> _MapColumns ;
        public Dictionary<string, DataGridColumn> MapColumns
        {
            get
            {
                if (_MapColumns == null)
                {
                    _MapColumns = new Dictionary<string, DataGridColumn>();
                    foreach (DataGridColumn dgc in this.Columns)
                    {
                        if (!_MapColumns.ContainsKey(dgc.DataField))
                        {
                            _MapColumns.Add(dgc.DataField, dgc);
                        }
                    }
                }
                return _MapColumns;
            }
        }

        public CMenuItem ItemDbClickedMenu;
        public ListView listViewObj;
        public Dictionary<string, ViewItem> ViewList = new Dictionary<string, ViewItem>();
        public virtual void FillGrid(XmlNode cmbNode)
        {
            //grid列处理
            XmlNodeList nodes = cmbNode.SelectNodes("cols/f");
            if (nodes.Count > 0)
            {
                this.listViewObj.Columns.Clear();

                foreach (XmlNode node in nodes)
                {
                    ViewItem vi = new ViewItem();
                    vi.LoadXml(node);
                    ColumnHeader ch = new ColumnHeader();
                    ch.Text = vi.Text;
                    ch.Width = vi.Width;


                    DataGridColumn dgc = new DataGridColumn();
                    dgc.LoadXml(node);
                    dgc.Text = ch.Text;
                    dgc.Width = ch.Width;
                    dgc.DataField = vi.Name;
                    //dgc.Hide = ch.Width == 0;
                    //dgc.IsKeyText = vi.IsKeyText ;
                    //dgc.IsKeyValue = vi.IsKeyValue;
                    this.listViewObj.Columns.Add(ch);

                    this.Columns.Add(dgc);

                    if (ViewList.ContainsKey(vi.Name))
                    {
                        continue;
                    }
                    ViewList.Add(vi.Name, vi);
                }
                //AcceptRejectRule
            }
            //事件初始化
            XmlNodeList evtnodes = cmbNode.SelectNodes("action/evt");
            if (evtnodes.Count > 0)
            {
                foreach (XmlNode node in evtnodes)
                {
                    CMenuItem mnu = null;
                    mnu = MenuProcess.GetMenu(mnu, node);
                    string strEvtName = XmlUtil.GetSubNodeText(node, "@id");
                    //'mnu = CMenuItem.
                    switch (strEvtName)
                    {
                        case "dbclick":
                            {
                                this.ItemDbClickedMenu = mnu;
                                //this.listView1.DoubleClick += new EventHandler(listView1_DoubleClick);
                                break;
                            }

                    }
                }
            }
            this.listViewObj.Tag = this;
        }

        public virtual void FillGridData(DataSet ds)
        {
            List<ListViewItem> lvis = new List<ListViewItem>();
            string[] grids = new string[this.Columns.Count];
            for (int r = 0; r < ds.Tables[0].Rows.Count; r++)
            {
                ItemValue iv = new ItemValue();
                iv.ItemValues = new Dictionary<string, string>();
                GridRow gr = new GridRow();
                for (int i = 0; i < this.Columns.Count; i++)
                {
                    GridCell gc = new GridCell();
                    if (this.Columns[i].DataField == "") continue;

                    string strval = ds.Tables[0].Rows[r][this.Columns[i].DataField].ToString();
                    gc.value = strval;
                    grids[i] = GetValue(strval, this.Columns[i]);
                    gc.text = grids[i];
                    if (!iv.ItemValues.ContainsKey(this.Columns[i].DataField))
                    {
                        iv.ItemValues.Add(this.Columns[i].DataField, grids[i]);
                    }
                    if (this.Columns[i].IsKeyValue) iv.KeyValue = grids[i];
                    if (this.Columns[i].IsKeyText) iv.KeyText = grids[i];
                    if(gr.Items.ContainsKey(this.Columns[i].DataField))
                    {
                        continue ;
                    }
                    gr.Items.Add(this.Columns[i].DataField,gc);
                }
                ListViewItem lvi = new ListViewItem(grids);
                gr.ItemValues = iv;
                lvi.Tag = gr;
                
                lvis.Add(lvi);
            }
            this.listViewObj.Items.Clear();
            this.listViewObj.Items.AddRange(lvis.ToArray());
        }

        public static string GetValue(string val, DataGridColumn dc)
        {
            if (dc.DataType == "date")
            {
                DateTime dt;
                DateTime.TryParse(val, out dt);
                return dt.ToShortDateString();
            }
            if (dc.ComboName == "")
            {
                dc.ComboName = GlobalShare.DataPointMappings[dc.dpt.Name].ComboName;
            }
            if (dc.ComboName != null && dc.ComboName.Trim().Length > 0)
            {
                if (GlobalShare.DataChoices.ContainsKey(dc.ComboName))
                {
                    DataChoice dcc = GlobalShare.DataChoices[dc.ComboName];
                    DataChoiceItem dci = dcc.FindChoiceByValue(val);
                    if (dci != null)
                    {
                        return dci.Text;
                    }
                }

            }

            return val;
        }

        public void ExportGrid()
        {


            SaveFileDialog ofd = new SaveFileDialog();
            ofd.Filter = "Excel文件(*.xls)|*.xls";
            //ofd.Multiselect = false;
            if (ofd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            ExcelExporter exper = new ExcelExporter();
            exper.Export(this, ofd.FileName);
            return;

            Stream saveStream = ofd.OpenFile();

            string strFile = ofd.FileName;



            StreamWriter sw = new StreamWriter(saveStream, Encoding.GetEncoding("GB2312"));
            StringBuilder sline = new StringBuilder();
            for (int col = 0; col < this.Columns.Count; col++)
            {
                if (this.Columns[col].Hide || !this.Columns[col].Visable)
                {
                    continue;
                }
                sline.AppendFormat("{0}\t", this.Columns[col].Text);
                //colcnt++;
            }
            sw.WriteLine(sline.ToString());
            for (int i = 0; i < this.listViewObj.Items.Count; i++)
            {
                int colcnt = 0;
                sline = new StringBuilder();
                for (int col = 0; col < this.Columns.Count; col++)
                {
                    if (this.Columns[col].Hide || !this.Columns[col].Visable)
                    {
                        continue;
                    }
                    sline.AppendFormat("{0}\t", this.listViewObj.Items[i].SubItems[col].Text.Replace("\t", ""));
                    colcnt++;
                }
                sw.WriteLine(sline.ToString());
            }
            sw.Close();
            saveStream.Close();
            MessageBox.Show("导出成功！");
            //Response.End(); 

        }

        
    }

    public class SubGrid : Grid
    {
        public ToolStrip ToolBar;
        //public string ListTitle;
        //
        public Label Lbl_Title;
        
        public List<UpdateData> GetUpdateData()
        {
            List<UpdateData> ret = new List<UpdateData>();
            for (int i = 0; i < this.listViewObj.Items.Count; i++)
            {
                GridRow gr = this.listViewObj.Items[i].Tag as GridRow;
                if (!gr.Updated)
                {
                    continue;
                }
            }
            return ret;
        }
    }

    public class GridRow
    {
        public Dictionary<string, GridCell> Items = new Dictionary<string,GridCell>();
        public ItemValue ItemValues;
        public bool Updated;
    }

    public class GridCell
    {
        public string value;
        public string text;
        public bool Updated;

    }

    public class DataGridColumn : ViewItem
    {
        ////public string Text;
        public string DataField
        {
            get { return this.dpt.Name; }
            set { this.dpt = new DataPoint(value); }
        }
        public bool Hide
        {
            get { return !this.Visable; }
            set { this.Visable = value; }
        }
        ////public int Width;
        ////public bool Hide;
        ////public int Index;
        ////public bool IsKeyValue;
        ////public bool IsKeyText;
    }

    public class ViewItem : DataPoint
    {
        public DataPoint dpt;
        public bool Visable = true;
        public bool IsKeyValue;
        public bool IsKeyText;
        public int Index;

        public virtual void LoadXml(XmlNode node)
        {
            //base.LoadXml(node);
            this.Name = XmlUtil.GetSubNodeText(node, "@i");
            this.Text = XmlUtil.GetSubNodeText(node, "@text");
            if (this.Text == "")
            {
                this.Text = XmlUtil.GetSubNodeText(node, "@udlbl");
            }
            int.TryParse(XmlUtil.GetSubNodeText(node, "@width"), out this.Width);
            this.DataType = XmlUtil.GetSubNodeText(node, "@type");
            this.ComboName = XmlUtil.GetSubNodeText(node, "@combo");
            this.Visable = !(XmlUtil.GetSubNodeText(node, "@hide") == "1");
            this.IsKeyText = XmlUtil.GetSubNodeText(node, "@keytext") == "1";
            this.IsKeyValue = XmlUtil.GetSubNodeText(node, "@keyvalue") == "1";
            string strIdx = XmlUtil.GetSubNodeText(node, "@index");
            if (strIdx == "")
            {
                //this.Index = int.Parse(node.SelectSingleNode("position(.)").Value);
            }
            else
            {
                int.TryParse(strIdx, out this.Index);
            }
        }

        public void GetItem(DataPoint dp)
        {
            dpt = dp;
            this.Name = dp.Name;
            this.Text = dp.Text;
            this.Width = dp.Width;
            this.ComboName = dp.ComboName;
            this.DataType = dp.DataType;
        }
    }

    public class CellViewItem : ViewItem
    {
        public bool multiline;
        public int height;
        public override void LoadXml(XmlNode node)
        {
            base.LoadXml(node);
            this.dpt = new DataPoint(XmlUtil.GetSubNodeText(node, "@f"));
        }
    }

  
   
}
