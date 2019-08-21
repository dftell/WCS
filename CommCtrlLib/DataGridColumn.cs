using System;
using System.Collections.Generic;
using WolfInv.Com.MetaDataCenter;
using WolfInv.Com.WCS_Process;

namespace WolfInv.Com.CommCtrlLib
{
    public class DataGridColumn : ViewItem,IXml
    {
        public BaseGrid Owner;
        ////public string Text;
        public DataGridColumn(BaseGrid grid)
        {
            Owner = grid;
        }
        public string DataField
        {
            get { return this.dpt.Name; }
            set { this.dpt = new DataPoint(value); }
        }
        public bool Hide
        {
            get { return !this.Visable; }
            set { this.Visable = value; }
        }
        ////public int Width;
        ////public bool Hide;
        ////public int Index;
        ////public bool IsKeyValue;
        ////public bool IsKeyText;
        ///
        public static string GetValue(string val, DataGridColumn dc, string uid)
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
            if (dc.DataType == "calcexpr")
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
                    DataComboBox dcb = new DataComboBox(dc.ComboName, uid);
                    dcb.TextField = dc.TextField;
                    dcb.ValueField = dc.ValueField;
                    dcb.ComboItemsSplitString = dc.ComboItemsSplitString;
                    List<DataChoiceItem> dcis = dcb.GetDataSource();
                    dcc = new DataChoice();
                    dcc.Options.AddRange(dcis.ToArray());
                }
                if (dcc != null)
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


    }

  
}
