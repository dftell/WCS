using System.Collections.Generic;
using WolfInv.Com.MetaDataCenter;

namespace WolfInv.Com.CommCtrlLib
{
    public class GridRow
    {
        public Dictionary<string, GridCell> Items = new Dictionary<string, GridCell>();
        public ItemValue ItemValues;
        public bool Updated;
        public bool Removed;
        public bool IsImported;
        public BaseGrid OwnerGrid;
        /// <summary>
        /// 该变量用于外部数据
        /// </summary>
        public UpdateData ExtraSourceData;

        public UpdateData ToUpdateData()
        {
            UpdateData data = new UpdateData();
            return ToUpdateData(ref data);
        }

        public UpdateData ToUpdateData(ref UpdateData OrgData)
        {
            if (OrgData == null)
            {
                OrgData = new UpdateData();
            }
            UpdateData ret = OrgData;
            foreach (string strkey in this.Items.Keys)
            {
                UpdateItem ui = new UpdateItem(strkey, this.Items[strkey].value);
                ret.Items.Add(strkey, ui);
            }
            return ret;
        }
    }

  
}
