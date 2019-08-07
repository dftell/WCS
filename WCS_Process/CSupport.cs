using System;
using System.Collections.Generic;
using System.Text;

namespace WolfInv.Com.WCS_Process
{
    public class CSupport
    {
        public int SupportId;
        public string SupportName;
        public string City;
        public string Address;
        public string Tel;
        public string Fax;
        public string Saler;
        public string CellPhone;
        public string Remark;
        public List<CProdTypeRelax> ProdTypes;
    }

    public class CProdTypeRelax
    {
        public ITType ProdType;
        public int Level;
        public bool IsKey;
    }

  
}
