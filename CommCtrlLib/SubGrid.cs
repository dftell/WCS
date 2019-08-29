using System.Collections.Generic;
using System.Windows.Forms;
using WolfInv.Com.MetaDataCenter;
namespace WolfInv.Com.CommCtrlLib
{
    public class SubGrid : Grid
    {
        public SubGrid(IUserData frmhandle)
            : base(frmhandle)
        {

        }
        public ToolStrip ToolBar;
        //public string ListTitle;
        //
        public Label Lbl_Title;
        public List<UpdateData> GetUpdateData()
        {
            return GetUpdateData(true);
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
        public List<UpdateData> GetUpdateData(bool checkupdated,bool getText = false,bool onlyReadSelectedItems=false, bool onlyReadSelectedGroups = false,bool onlySign=false)
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
                        
                        if ( lvi.Group != null)
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
                UpdateData  subitem = new UpdateData();
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
                            subitem.Items.Add(this.Columns[c].dpt.Name,ui);
                        if (this.Columns[c].IsKeyValue)
                        {
                            subitem.keydpt = this.Columns[c].dpt;
                            subitem.keyvalue = gr.Items[this.Columns[c].DataField].value;
                            if(subitem.keyvalue == null || subitem.keyvalue.Trim().Length == 0)//如果关键字段为空，类型为新增
                            {
                                subitem.ReqType = DataRequestType.Add;
                            }
                        }
                    }
                    
                }
                if(gr.Removed)
                {
                    subitem.ReqType = DataRequestType.Delete;
                }
                ret.Add(subitem);
            }
            return ret;
        }

        

        public GridRow GetNewRow(ref GridRow gr,UpdateData newData)
        {
            if(gr == null)
                gr = new GridRow();
            Dictionary<string, DataGridColumn> dgcs = this.MapColumns;
            ItemValue iv = new ItemValue();
            iv.ItemValues = new Dictionary<string, string>();
            
            foreach (string dpt in dgcs.Keys)
            {

                if (newData.Items.ContainsKey(dpt))
                {
                    if (!gr.Items.ContainsKey(dpt))//新增
                    {
                        GridCell gc = new GridCell();
                        gc.value = newData.Items[dpt].value;
                        //gc.text = Grid.GetValue(newData.Items[dpt].text, dgcs[dpt], _frmhandle.strUid);
                        gc.text = newData.Items[dpt].text;
                        if(gc.text == null || gc.text.Trim().Length == 0)
                            gc.text = dgcs[dpt].getValue(_frmhandle.strUid, newData);
                        iv.ItemValues.Add(dpt, gc.value);
                        if (dgcs[dpt].IsKeyText)
                        {
                            iv.KeyText = gc.text;
                        }
                        if (dgcs[dpt].IsKeyValue)
                        {
                            iv.KeyValue = gc.value;
                            gc.isKey = true;
                        }
                        gr.Items.Add(dpt, gc);
                    }
                    else//更新
                    {
                        GridCell gc = gr.Items[dpt];
                        gc.value = newData.Items[dpt].value;
                        //gc.text = Grid.GetValue(newData.Items[dpt].text, dgcs[dpt], _frmhandle.strUid);//
                        gc.text = dgcs[dpt].getValue(_frmhandle.strUid, newData);
                        iv.ItemValues[dpt] = gc.value;
                    }
                }
                else
                {
                    if (!gr.Items.ContainsKey(dpt))//新增
                    {
                        GridCell gc = new GridCell();
                        gc.value = "";
                        gc.text = "";
                        iv.ItemValues.Add(dpt, gc.value);
                        if (dgcs[dpt].IsKeyText)
                        {
                            iv.KeyText = gc.text;
                        }
                        if (dgcs[dpt].IsKeyValue)
                        {
                            iv.KeyValue = gc.value;
                            gc.isKey = true;
                        }
                        gr.Items.Add(dpt, gc);
                    }
                    else//更新
                    {
                        //不需要更新
                    }
                }
            }
            gr.ItemValues = iv;
            gr.Updated = true;
            gr.OwnerGrid = this;
            return gr;
        }
    }

   
}
