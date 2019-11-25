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
