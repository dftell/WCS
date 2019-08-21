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
using WolfInv.Com.CommCtrlLib;
namespace WolfInv.Com.CommCtrlLib
{

    public class Grid : BaseGrid
    {

        protected ListGrid listobj;
        protected IUserData _frmhandle = null;
        public Grid(IUserData frmhandle):base(frmhandle)
        {
            _frmhandle = frmhandle;
        }

        
      
        public virtual void FillGridData_deleted(DataSet ds)
        {
            this.Items.Clear();
            if (ds == null) return;
            if (ds.Tables.Count == 0) return;
            for (int r = 0; r < ds.Tables[0].Rows.Count; r++)
            {
                ItemValue iv = new ItemValue();
                iv.ItemValues = new Dictionary<string, string>();
                GridRow gr = new GridRow();
                UpdateData currdata = new UpdateData();
                for (int i = 0; i < this.Columns.Count; i++)
                {
                    
                    GridCell gc = new GridCell();
                    if (this.Columns[i].DataField == "")
                        continue;
                    if (!ds.Tables[0].Columns.Contains(this.Columns[i].DataField))//如果不存在该列,一般都是datasource忘记包括了该列，锁着三者不统一所致
                    {
                        continue;
                    }
                    string strval = ds.Tables[0].Rows[r][this.Columns[i].DataField].ToString();
                    currdata.Items.Add(this.Columns[i].DataField,new UpdateItem( this.Columns[i].DataField,strval));//数据源为数据库时，要不断地更新，通过前面进来的updatedata更新后面的引用列
                    gc.value = strval;
                    //gc.text = GetValue(strval, this.Columns[i],_frmhandle.strUid);
                    gc.text = this.Columns[i].getValue(_frmhandle.strUid, currdata);
                    if (!iv.ItemValues.ContainsKey(this.Columns[i].DataField))
                    {
                        iv.ItemValues.Add(this.Columns[i].DataField, gc.text);
                    }

                    if (this.Columns[i].IsKeyValue)
                    {
                        gc.isKey = true;
                        iv.KeyValue = gc.text;
                    }
                    if (this.Columns[i].IsKeyText) iv.KeyText = gc.text;
                    if(gr.Items.ContainsKey(this.Columns[i].DataField))
                    {
                        continue ;
                    }
                    gr.Items.Add(this.Columns[i].DataField,gc);
                }
                gr.OwnerGrid = this;
                gr.ItemValues = iv;
                this.Items.Add(gr);
                
            }
            FillListView();
        }

        /// <summary>
        /// 该函数用于外部数据
        /// </summary>
        /// <param name="ds"></param>
        public virtual void FillGridData(List<UpdateData> ds)
        {
            this.Items.Clear();
            if (ds == null) return;
            if (ds.Count == 0) return;
            for (int r = 0; r < ds.Count; r++)
            {
                ItemValue iv = new ItemValue();
                iv.ItemValues = new Dictionary<string, string>();
                GridRow gr = new GridRow();
                gr.IsImported = ds[r].IsImported;
                gr.Updated = ds[r].Updated;
                
                for (int i = 0; i < this.Columns.Count; i++)
                {
                    GridCell gc = new GridCell();
                    if (this.Columns[i].DataField == "") continue;
                    if (!ds[r].Items.ContainsKey(this.Columns[i].DataField))//如果不存在该列
                    {
                        continue;
                    }
                    string strval = ds[r].Items[this.Columns[i].DataField].value;
                    gc.value = strval;
                    //gc.text = GetValue(strval, this.Columns[i], _frmhandle.strUid);
                    gc.text = this.Columns[i].getValue(_frmhandle.strUid, ds[r]);
                    if (!iv.ItemValues.ContainsKey(this.Columns[i].DataField))
                    {
                        iv.ItemValues.Add(this.Columns[i].DataField, gc.text);
                    }
                    if (this.Columns[i].IsKeyValue)
                    {
                        gc.isKey = true;
                        iv.KeyValue = gc.text;
                    }
                    if (this.Columns[i].IsKeyValue) iv.KeyValue = gc.text;
                    if (this.Columns[i].IsKeyText) iv.KeyText = gc.text;
                    if (gr.Items.ContainsKey(this.Columns[i].DataField))
                    {
                        continue;
                    }
                    gr.Items.Add(this.Columns[i].DataField, gc);
                }
                gr.ExtraSourceData = ds[r];
                gr.OwnerGrid = this;
                gr.ItemValues = iv;
                this.Items.Add(gr);

            }
            FillListView();
        }


        public virtual void FillListView()
        {
            List<ListGridItem> lvis = new List<ListGridItem>();
            string[] intgrids = new string[this.Columns.Count];
            for (int r = 0; r < this.Items.Count; r++)
            {
                string[] grids = new string[this.Columns.Count];
                UpdateData currdata =  this.Items[r].ToUpdateData();
                for (int i = 0; i < this.Columns.Count; i++)
                {
                    if (this.Columns[i].DataField == "") continue;

                    if (this.Items[r].Items.ContainsKey(this.Columns[i].DataField))
                    {
                        //this.Items[r].Items[this.Columns[i].DataField].text = GetValue(this.Items[r].Items[this.Columns[i].DataField].value, this.Columns[i], _frmhandle.strUid);
                        this.Items[r].Items[this.Columns[i].DataField].text = this.Columns[i].getValue(_frmhandle.strUid, currdata);
                        grids[i] = this.Items[r].Items[this.Columns[i].DataField].text;
                    }
                    if (this.Columns[i].Sum)
                    {
                        
                        long fval = 0;
                        long.TryParse(grids[i], out fval);
                        long sumval = 0;
                        long.TryParse(intgrids[i], out sumval);
                        sumval = sumval + fval;
                        intgrids[i] = sumval.ToString();
                    }
                }
                ListGridItem lvi = new ListGridItem(grids);
                lvi.Tag = this.Items[r];
                lvis.Add(lvi);
            }
            

            listobj.Items.Clear();
            listobj.Items.AddRange(lvis.ToArray());
            if (listobj.AllowSum && lvis.Count > 0)
            {
                intgrids[0] = "合计：";
                SumListGridItem lgi = new SumListGridItem(intgrids);
                listobj.Items.Add(lgi);
            }
        }

        

        public static string GetValue(string val, DataGridColumn dc,string uid)
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
            if(dc.DataType == "calcexpr")
            {
                CalcExpr ce = new CalcExpr();
                HandleBase hb = ce.GetHandleClass(dc.Method, dc.CalcExpr);
                val = hb.Handle();
                
                
            }
            if ((dc.DataType == "combo" || dc.DataType == "datacombo") && dc.ComboName != null && dc.ComboName.Trim().Length > 0)
            {
                DataChoice dcc = null;
                if (dc.DataType == "combo")
                    dcc = GlobalShare.GetGlobalChoice(dc.Owner.frmhandle.strUid, dc.ComboName);
                else
                {
                    DataComboBox dcb = new DataComboBox(dc.ComboName,uid);
                    dcb.TextField = dc.TextField;
                    dcb.ValueField = dc.ValueField;
                    dcb.ComboItemsSplitString = dc.ComboItemsSplitString;
                    List<DataChoiceItem> dcis = dcb.GetDataSource();
                    dcc = new DataChoice();
                    dcc.Options.AddRange(dcis.ToArray());
                }
                if ( dcc!= null)
                {

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
            for (int i = 0; i < listobj.Items.Count; i++)
            {
                int colcnt = 0;
                sline = new StringBuilder();
                for (int col = 0; col < this.Columns.Count; col++)
                {
                    if (this.Columns[col].Hide || !this.Columns[col].Visable)
                    {
                        continue;
                    }
                    sline.AppendFormat("{0}\t", listobj.Items[i].SubItems[col].Text.Replace("\t", ""));
                    colcnt++;
                }
                sw.WriteLine(sline.ToString());
            }
            sw.Close();
            saveStream.Close();
            MessageBox.Show("导出成功！");
            //Response.End(); 

        }



        protected override IDataFieldHeaderColumn GetHeaderColumn()
        {
            return new ListGridColumnHeader();
        }

        protected override void ClearItems()
        {
            if(listobj == null)
                listobj = this.listViewObj as ListGrid;
            listobj.Items.Clear();
        }

        protected override void AddColumnItem(IDataFieldHeaderColumn ch)
        {
            (listobj.Columns as ListGrid.ListGridColumnHeaderCollection).Add(ch as ColumnHeader);
        }
    }

   
}
