using System;
using System.Collections.Generic;
using System.Xml;
using XmlProcess;
using WolfInv.Com.MetaDataCenter;
using System.Data;
using System.Linq;
namespace WolfInv.Com.WCS_Process
{
    public class DataChoice : ICloneable
    {
        public string Name;
        public List<DataChoiceItem> Options = new List<DataChoiceItem>();
        public string DefaultValue;
        Dictionary<string, DataChoiceItem> _ValueItems;
        Dictionary<string, DataChoiceItem> ValueItems
        {
            get
            {
                if (_ValueItems == null)
                {
                    _ValueItems = new Dictionary<string, DataChoiceItem>();
                    for (int i = 0; i < Options.Count; i++)
                    {
                        DataChoiceItem dci = Options[i];
                        if (!_ValueItems.ContainsKey(dci.Value))
                        {
                            _ValueItems.Add(dci.Value, dci);
                        }
                    }
                }
                return _ValueItems;
            }
        }
        public DataChoice()
        {

        }

        public static Dictionary<string, DataChoice> InitDataChoiceMappings(UserGlobalShare userinfo)
        {
            Dictionary<string, DataChoice> DataChoiceMappings = new Dictionary<string, DataChoice>();
            XmlDocument xmldoc = GlobalShare.GetXmlFile(@"\xml\data_choice.xml");
            if(userinfo != null)
                      xmldoc =  GlobalShare.UpdateWithUseInfo(xmldoc,userinfo.UID,true);
            if (xmldoc == null)
            {
                throw new Exception("can't init the DataIdMapping File!");
            }
            
            XmlNodeList nodes = xmldoc.SelectNodes("/data_choices/combo");
            DataChoiceMappings = new Dictionary<string, DataChoice>();
            foreach (XmlNode node in nodes)
            {
                DataChoice dc = new DataChoice();
                dc.Name = XmlUtil.GetSubNodeText(node, "@id");
                dc.DefaultValue = XmlUtil.GetSubNodeText(node, "@default");
                XmlNodeList options = node.SelectNodes("./option");
                dc.Options = new List<DataChoiceItem>();
                for (int i = 0; i < options.Count; i++)
                {
                    DataChoiceItem dci = new DataChoiceItem();
                   
                    dci.Value = XmlUtil.GetSubNodeText(options[i], "@value");
                    dci.Text = XmlUtil.GetSubNodeText(options[i], "@text");
                    
                    dc.Options.Add(dci);
                }
                if (!DataChoiceMappings.ContainsKey(dc.Name))
                {
                    DataChoiceMappings.Add(dc.Name, dc);
                }
            }
            return DataChoiceMappings;
        }

        public DataChoiceItem FindChoiceByValue(string val)
        {
            if (val == null)
            {
                return null;
            }
            if (ValueItems != null)
            {
                if (ValueItems.ContainsKey(val))
                {
                    return ValueItems[val];
                }
                else
                {
                    return null;
                }
            }
            return null;
        }

        public int FindIndexByValue(string val)
        {
            int ret = -1;
            if (this.Options == null) return ret;
            for (int i = 0; i < this.Options.Count; i++)
            {
                MathHandle mhdle = new MathHandle(this.Options[i].Value);
                string exeval = mhdle.Handle();
                if (exeval != null && exeval != this.Options[i].Value)
                {
                    string matched;
                }
                if (exeval == null)
                {
                    exeval = this.Options[i].Value;
                }

                if (exeval == val)
                {
                    return i;
                    //break;
                }
            }
            return ret;
        }
        public static DataChoice ConvertFromDataSet(DataSet ds)
        {
            return ConvertFromDataSet(ds, null, null,null);
        }
        public static DataChoice ConvertFromDataSet(DataSet ds, string valfld, string txtfld, string splitor = null)
        {
            return ConvertFromDataSet(ds, valfld, txtfld, false,splitor);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="valfld"></param>
        /// <param name="txtfld"></param>
        /// <param name="isRowItem"></param>
        /// <returns></returns>
        public static DataChoice ConvertFromDataSet(DataSet ds, string valfld, string txtfld,bool isRowItem, string splitor=",")
        {
            if (ds == null)
                return null;
            if (ds.Tables.Count < 1)
                return null;
            if (ds.Tables[0].Columns.Count < 1)
                return null;
            DataChoice ret = new DataChoice();
            string strval = ds.Tables[0].Columns[0].ColumnName;
            string strtxt = strval;
            if (valfld != null && valfld.Trim().Length > 0)
            {
                strval = valfld;
            }

            if (txtfld != null && txtfld.Trim().Length > 0)
            {
                strtxt = txtfld;
            }
            else
            {
                if (ds.Tables[0].Columns.Count > 1)
                {
                    strtxt = ds.Tables[0].Columns[1].ColumnName;
                }
            }
            Dictionary<string, DataColumn> allcols = new Dictionary<string, DataColumn>();
            for(int i=0;i<ds.Tables[0].Columns.Count;i++)
            {
                allcols.Add(ds.Tables[0].Columns[i].ColumnName, ds.Tables[0].Columns[i]);
            }
            string[] vals;
            string[] txts;
            if (splitor == null || splitor.Trim().Length == 0)
            {
                vals = new string[] { strval };
                txts = new string[] { strtxt };
            }
            else
            { 
                vals = getSplitNames(strval, splitor);
                txts = getSplitNames(strtxt, splitor);
            }
            




            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                DataRow dr = ds.Tables[0].Rows[i];
                DataChoiceItem dci = null;
                if (isRowItem)
                {
                    dci = new DataRowChoiceItem();
                    
                }
                else
                {
                    dci = new DataChoiceItem();
                }
                dci.Data = new UpdateData();
                for (int c = 0; c < dr.Table.Columns.Count; c++)
                {
                    UpdateItem ui = new UpdateItem(dr.Table.Columns[c].ColumnName, dr[c].ToString());
                    if (!dci.Data.Items.ContainsKey(ui.datapoint.Name))
                    {
                        dci.Data.Items.Add(ui.datapoint.Name, ui);
                    }
                }
                List<string> valitems = new List<string>();
                for(int c=0;c<vals.Length;c++)
                {
                    if (allcols.ContainsKey(vals[c]))
                    {
                        valitems.Add(dr[vals[c]].ToString());
                    }
                    else
                    {
                        valitems.Add(vals[c]);
                    }
                    

                }
                List<string> txtitems = new List<string>();
                for (int c = 0; c < txts.Length; c++)
                {
                    if (allcols.ContainsKey(txts[c]))
                    {
                        txtitems.Add(dr[txts[c]].ToString());
                    }
                    else
                    {
                        txtitems.Add(txts[c]);
                    }
                }
                dci.Value = string.Join(splitor, valitems);
                dci.Text = string.Join(splitor, txtitems);
                //dci.Value = dr[strval].ToString();
                //dci.Text = dr[strtxt].ToString();
                if (ret.Options == null)
                    ret.Options = new List<DataChoiceItem>();
                //if(!ret.ValueItems.ContainsKey(dci.Value))
                if(ret.Options.FindAll(a=>a.Value.Equals(dci.Value)).Count==0)
                    ret.Options.Add(dci);
            }
            return ret;
        }

        static string[] getSplitNames(string val,string strsplit)
        {
            return val.Split(strsplit.ToCharArray());
        }

        #region ICloneable 成员

        public object Clone()
        {
            DataChoice ret = new DataChoice();
            ret.Name = this.Name;
            ret.DefaultValue = this.DefaultValue;
            if (this.Options == null)
            {
                return ret;
            }
            ret.Options = new List<DataChoiceItem>();
            for (int i = 0; i < this.Options.Count; i++)
            {
                DataChoiceItem dci = this.Options[i].Clone() as DataChoiceItem;
                ret.Options.Add(dci);
            }
            return ret;
        }

        #endregion
    }

    
}
