using System;
using System.Data;
using WolfInv.Com.CommCtrlLib;
using WolfInv.Com.MetaDataCenter;
using System.Collections.Generic;
using WolfInv.Com.WCS_Process;
using System.Web.UI;

namespace WolfInv.Com.CommWebCtrlLib
{
    public abstract class WebPageFormHandle : BaseFormHandle
    {
        public WebPageModel CurrPage;
        public WebPageFormHandle()
        {
            
        }

        public WebPageFormHandle(string rowid)
        {
            this.strRowId = rowid;
        }
        protected Page currpage;
        public void SetPage(Page pg)
        {
            currpage = pg;
        }
        public override DataSet InitDataSource(string sGridSource, out string msg)
        {
            msg = null;
            if (sGridSource == null || sGridSource.Trim().Length == 0)
            {
                msg = string.Format("数据源为空");
                return null; ;
            }
            return DataSource.InitDataSource(sGridSource, InitBaseConditions(), strUid, out msg);

        }

        public override List<DataCondition> InitBaseConditions()
        {

            List<DataCondition> conds = new List<DataCondition>();
            DataCondition ds = new DataCondition();
            ds.Datapoint = new DataPoint(this.strKey);
            ds.value = this.strRowId;
            ds.Logic = ConditionLogic.And;
            if (this.TranData != null)
            {
                foreach (DataTranMapping data in this.TranData)
                {
                    DataCondition datacond = new DataCondition();
                    datacond.value = data.FromDataPoint;
                    datacond.Datapoint = new DataPoint(data.ToDataPoint);
                    conds.Add(datacond);
                }
            }
            conds.Add(ds);
            return conds;
        }

        public override UpdateData GetUpdateData(bool JudgeValueChanged, bool UpdateFrameData)
        {
            return null;
        }
        public override UpdateData GetUpdateData(bool JudgeValueChanged)
        {
            return base.GetUpdateData(JudgeValueChanged);
        }

        public override List<UpdateData> GetDataList(List<UpdateData> OrgList, bool OnlyCheckedItem)
        {
            return new List<UpdateData>();
        }

    }
}
