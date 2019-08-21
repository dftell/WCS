using System;
using System.Collections.Generic;
namespace WolfInv.Com.MetaDataCenter
{
    public class ItemValue:ICloneable 
    {
        public Dictionary<string, string> ItemValues;
        public string KeyValue;
        public string KeyText;

        #region ICloneable 成员

        public object Clone()
        {
            ItemValue ret = new ItemValue();
            ret.ItemValues = new Dictionary<string, string>();
            ret.KeyText = this.KeyText;
            ret.KeyValue = this.KeyValue;
            foreach (string key in this.ItemValues.Keys)
            {
                ret.ItemValues.Add(key, this.ItemValues[key]);
            }
            return ret;
        }

        #endregion
    }
}
