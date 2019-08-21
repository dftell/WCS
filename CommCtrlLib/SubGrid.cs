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
        public List<UpdateData> GetUpdateData(bool checkupdated)
        {
            List<UpdateData> ret = new List<UpdateData>();
            if (listobj == null || listobj.Items == null)
                return ret;
            for (int i = 0; i < listobj.Items.Count; i++)
            {
                GridRow gr = listobj.Items[i].Tag as GridRow;
                if (gr == null)
                    continue;
                if (!gr.Updated && checkupdated)
                {
                    continue;
                }
                UpdateData  subitem = new UpdateData();
                for (int c = 0; c < this.Columns.Count; c++)
                {
                    if (gr.Items.ContainsKey(this.Columns[c].DataField))
                    {
                        UpdateItem ui = new UpdateItem();
                        ui.datapoint = this.Columns[c].dpt;
                        ui.value = gr.Items[this.Columns[c].DataField].value;
                        if(!subitem.Items.ContainsKey(this.Columns[c].dpt.Name))
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
