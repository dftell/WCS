using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using WolfInv.Com.MetaDataCenter;
namespace WolfInv.Com.WCS_Process
{
    public class SystemConts
    {
        public static string currmonthfirstdate { get; set; }
        public static string currmonthenddate { get; set; }
        public static string premonthfirstdate { get; set; }
        public static string premonthenddate { get; set; }

        static string ToDate(DateTime dt)
        {
            return string.Format("{0}-{1}-{2}", dt.Year, dt.Month.ToString().PadLeft(2, '0'), dt.Day.ToString().PadLeft(2, '0'));
           
        }

        static SystemConts()
        {
            DateTime now = DateTime.Now;
            DateTime currfd = now.AddDays(now.Day * -1+1);
            DateTime preld = currfd.AddDays(-1);
            DateTime prefd = currfd.AddMonths(-1);
            DateTime currld = currfd.AddMonths(1).AddDays(-1);
            currmonthfirstdate = ToDate(currfd);
            currmonthenddate = ToDate(currld);
            premonthfirstdate = ToDate(prefd);
            premonthenddate = ToDate(preld);
        }

        public static Dictionary<string,UpdateItem> ToDictionary()
        {
            Dictionary<string, UpdateItem> ret = new Dictionary<string, UpdateItem>();
            Type t = typeof(SystemConts);
            foreach (PropertyInfo pi in t.GetProperties(BindingFlags.Public | BindingFlags.Static))
            {
                UpdateItem ui = new UpdateItem(pi.Name, pi.GetValue(null).ToString());
                ret.Add(ui.datapoint.Name, ui);
            }
            return ret;
        }
    }
}
