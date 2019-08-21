using System.Data;
using WolfInv.Com.WCS_Process;
using WolfInv.Com.CommCtrlLib;
namespace WolfInv.Com.CommFormCtrlLib
{
    public abstract class WinFormHandle : BaseFormHandle 
    {
        protected WinFormHandle()
            : base()
        {
        }

        protected WinFormHandle(string id) : base(id) { }

        public override DataSet InitDataSource(string sGridSource, out string msg)
        {
            msg = null;
            if (sGridSource == null || sGridSource.Trim().Length == 0)
            {
                msg = string.Format("数据源为空");
                return null; ;
            }
            return DataSource.InitDataSource(GridSource, InitBaseConditions(), strUid, out msg);

        }

        ////public override DataSet InitDataSource(string sGridSource, List<DataCondition> dc,string id, out string msg)
        ////{
        ////    msg = null;
        ////    if (sGridSource == null || sGridSource.Trim().Length == 0)
        ////    {
        ////        msg = string.Format("数据源为空");
        ////        return null; ;
        ////    }
        ////    return DataSource.InitDataSource(GridSource, InitBaseConditions(), strUid, out msg);

        ////}
    }
}
