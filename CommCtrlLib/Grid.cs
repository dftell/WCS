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
using System.Linq;

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="checkupdated"></param>
        /// <param name="getText"></param>
        /// <param name="onlyReadSelectedItems"></param>
        /// <param name="onlyReadSelectedGroups"></param>
        /// <param name="onlySign">只标志，批更新专用，将子项标记为修改，修改后回传</param>
        /// <returns></returns>
        public List<UpdateData> GetUpdateData(bool checkupdated, bool getText = false, bool onlyReadSelectedItems = false, bool onlyReadSelectedGroups = false, bool onlySign = false)
        {
            List<UpdateData> ret = new List<UpdateData>();
            if (listobj == null || listobj.Items == null)
                return ret;
            HashSet<string> selectGrps = new HashSet<string>();
            //onlyReadSelectedGroups是在onlyReadSelectedItems为true的上进一步约束，如果是onlyReadSelectedGroups为true,就算未选，如果属于组，一样要进行操作
            if (onlyReadSelectedGroups && onlyReadSelectedItems)
            {
                for (int i = 0; i < listobj.CheckedIndices.Count; i++)
                {
                    ListViewItem lvi = listobj.Items[listobj.CheckedIndices[i]];
                    if (onlyReadSelectedGroups)//如果只读已选的
                    {

                        if (lvi.Group != null)
                        {
                            if (!selectGrps.Contains(lvi.Group.Header))
                                selectGrps.Add(lvi.Group.Header);
                        }
                    }
                }
            }
            for (int i = 0; i < listobj.Items.Count; i++)
            {
                ListViewItem lvi = listobj.Items[i];
                GridRow gr = listobj.Items[i].Tag as GridRow;
                if (gr == null)//汇总行
                    continue;
                if (onlySign)
                {
                    gr.Updated = true;
                }

                if (!lvi.Checked)//如果没选
                {
                    if (onlyReadSelectedItems)//如果只显示选择的
                    {
                        if (onlyReadSelectedGroups)//只显示同组的
                        {
                            if (lvi.Group != null && selectGrps.Contains(lvi.Group.Header))//如果属于
                            {

                            }
                            else
                            {
                                if (!onlySign)
                                    continue;
                                else
                                    gr.Updated = false;
                            }

                        }
                        else//不同组的
                        {
                            if (!onlySign)
                            {
                                continue;
                            }
                            else
                            {
                                gr.Updated = false;
                            }
                        }
                    }

                }


                if (!gr.Updated && checkupdated)
                {
                    continue;
                }
                UpdateData subitem = new UpdateData();
                if (this is SubGrid)
                {
                    subitem.Updated = gr.Updated;
                    for (int c = 0; c < this.Columns.Count; c++)
                    {
                        if (gr.Items.ContainsKey(this.Columns[c].DataField))
                        {
                            UpdateItem ui = new UpdateItem();
                            ui.datapoint = this.Columns[c].dpt;
                            ui.value = gr.Items[this.Columns[c].DataField].value;
                            ui.text = gr.Items[this.Columns[c].DataField].text;
                            if (!subitem.Items.ContainsKey(this.Columns[c].dpt.Name))
                                subitem.Items.Add(this.Columns[c].dpt.Name, ui);
                            if (this.Columns[c].IsKeyValue)
                            {
                                subitem.keydpt = this.Columns[c].dpt;
                                subitem.keyvalue = gr.Items[this.Columns[c].DataField].value;
                                if (subitem.keyvalue == null || subitem.keyvalue.Trim().Length == 0)//如果关键字段为空，类型为新增
                                {
                                    subitem.ReqType = DataRequestType.Add;
                                }
                            }
                        }

                    }

                    if (gr.Removed)
                    {
                        subitem.ReqType = DataRequestType.Delete;
                    }
                }
                else
                {
                    subitem = gr.ExtraSourceData;
                    if(getText)
                    {
                        subitem.Items.Values.ToList().ForEach(a => a.text = a.value);
                        subitem.SubItems.ForEach(
                            a =>
                            {
                                a.Items.Values.ToList().ForEach(p => p.text = p.value);
                            }
                            );
                    }
                }
                ret.Add(subitem);
            }
            return ret;
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
                    gc.OwnerColumn = this.Columns[i];
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
            string[] grparr = null;
            if(listobj.AllowGroup && listobj.GroupBy!= null&& listobj.GroupBy.Trim().Length>0)
            {
                grparr = listobj.GroupBy.Split(',');
            }

            Dictionary<string, ListViewGroup> allgroup = new Dictionary<string, ListViewGroup>();  
            for (int r = 0; r < this.Items.Count; r++)
            {
                string[] grids = new string[this.Columns.Count];
                UpdateData currdata =  this.Items[r].ToUpdateData();
                
                for (int i = 0; i < this.Columns.Count; i++)
                {
                    if (this.Columns[i].DataField == "") continue;

                    if (this.Items[r].Items.ContainsKey(this.Columns[i].DataField))
                    {
                        this.Items[r].Items[this.Columns[i].DataField].OwnerColumn = this.Columns[i];
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
                List<string> strGrp = new List<string>();
                if (listobj.AllowGroup && grparr != null)
                {

                    grparr.ToList().ForEach(a => {
                        if(this.Items[r].Items.ContainsKey(a))
                        {
                            strGrp.Add(this.Items[r].Items[a].text);
                        }
                        else
                        {
                            strGrp.Add(a);
                        }
                    });
                    string name = string.Join(",", strGrp);
                    if(!allgroup.ContainsKey(name))
                    {
                        allgroup.Add(name, new ListViewGroup(name));
                    }
                    allgroup[name].Items.Add(lvi);
                    
                }

                lvi.Tag = this.Items[r];
                lvis.Add(lvi);
            }

            listobj.Items.Clear();
            listobj.Items.AddRange(lvis.ToArray());
            if(listobj.AllowGroup && grparr != null)
            {
                listobj.Groups.AddRange(allgroup.Values.ToArray());   
            }
            if (listobj.AllowSum && lvis.Count > 0)
            {
                intgrids[0] = "合计：";
                int col = 0;
                this.Columns.ForEach(a=> {
                    if(a.Sum)
                    {

                        intgrids[col] = this.Items.Sum(row => {
                            float val = 0;
                            if (!row.Items.ContainsKey(a.DataField))
                                return 0;
                            float.TryParse(row.Items[a.DataField].value,out val);
                            return val;
                        }).ToString();
                        
                    }
                    col++;
                });
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
