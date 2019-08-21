using WolfInv.Com.MetaDataCenter;
using System;
using WolfInv.Com.WCS_Process;
using System.Collections.Generic;

namespace WolfInv.Com.CommCtrlLib
{
    //目前所有list 的值全部从此处取，
    //未来编辑界面数据也从此处取
    public class DataControlItem : DataPoint, IXml
    {
        public string getValue(string uid, UpdateItem ui)
        {
            UpdateData ud = new UpdateData();
            ud.Items.Add(ui.text, ui);
            return getValue(uid, ud);
        }
        public string getValue(string uid,  UpdateData ud)
        {
            DataPoint dc = this;
            string val = null;
            if (ud.Items.ContainsKey(dc.Name))//默认等于本字段值
            {
                val = ud.Items[dc.Name].value;
            }
            if (dc.DataType == "date")
            {
                DateTime dt;
                DateTime.TryParse(val, out dt);
                return dt.ToShortDateString();
            }
            if (dc.ComboName == "")
            {
                dc.ComboName = GlobalShare.DataPointMappings[dc.Name].ComboName;
            }
            if (dc.DataType.ToLower() == "calcexpr")
            {
                CalcExpr ce = new CalcExpr();
                return ce.getValue(dc.CalcExpr, dc.Method, ud,ComboItemsSplitString);


            }
            if ((dc.DataType == "combo" || dc.DataType == "datacombo") && dc.ComboName != null && dc.ComboName.Trim().Length > 0)
            {
                DataChoice dcc = null;
                if (dc.DataType == "combo")
                    dcc = GlobalShare.GetGlobalChoice(uid, dc.ComboName);
                else
                {
                    DataComboBox dcb = new DataComboBox(dc.ComboName, uid);
                    dcb.TextField = dc.TextField;
                    dcb.ValueField = dc.ValueField;
                    dcb.ComboItemsSplitString = dc.ComboItemsSplitString;
                    List<DataChoiceItem> dcis = dcb.GetDataSource();
                    dcc = new DataChoice();
                    dcc.Options.AddRange(dcis.ToArray());
                    List<string> vallist = new List<string>();
                    string[] strarr = dc.ValueField.Split(dc.ComboItemsSplitString.ToCharArray());
                    if (val == null||val.Trim().Length==0)
                    {
                        for (int i = 0; i < strarr.Length; i++)
                        {
                            string dpname = strarr[i];
                            if (ud.Items.ContainsKey(dpname))
                            {
                                vallist.Add(ud.Items[dpname].value);
                            }
                            else//常量
                            {
                                vallist.Add(dpname);
                            }
                        }
                        val = string.Join(dc.ComboItemsSplitString, vallist);
                    }
                    if (this.RefPoint!= null && this.RefPoint.Trim().Length>0)
                    {
                        if(ud.Items.ContainsKey(this.RefPoint))
                        {
                            val = ud.Items[this.RefPoint].value;
                        }
                        else
                        {
                            vallist = new List<string>();
                            strarr = RefPoint.Split(dc.ComboItemsSplitString.ToCharArray());
                            for (int i = 0; i < strarr.Length; i++)
                            {
                                string dpname = strarr[i];
                                if (ud.Items.ContainsKey(dpname))
                                {
                                    vallist.Add(ud.Items[dpname].value);
                                }
                                else//常量
                                {
                                    vallist.Add(dpname);
                                }
                            }
                            val = string.Join(dc.ComboItemsSplitString, vallist);
                        }
                    }
                }
                if (dcc != null)
                {
                    
                    DataChoiceItem dci = dcc.FindChoiceByValue(val);
                    if (dci != null)
                    {
                        List<string> textlist = new List<string>();
                        string[] strarr = dc.TextField.Split(dc.ComboItemsSplitString.ToCharArray());
                        for (int i = 0; i < strarr.Length; i++)
                        {

                            string dpname = strarr[i];
                            if (dci.Data == null)//简单数据选项
                            {
                                return dci.Text;
                            }
                            else
                            {
                                if (dci.Data.Items.ContainsKey(dpname))
                                {
                                    textlist.Add(dci.Data.Items[dpname].value);
                                }
                                else
                                {
                                    textlist.Add(dpname);
                                }
                            }
                        }
                        return string.Join(dc.ComboItemsSplitString,textlist);
                    }
                }

            }

            return val;
        }
    }


}
