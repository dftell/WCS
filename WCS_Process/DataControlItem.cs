using System;
using System.Collections.Generic;
using System.Xml;
using XmlProcess;
using WolfInv.Com.MetaDataCenter;
using CommCtrlLib;
using System.Data;
using System.Windows.Forms;

namespace WolfInv.Com.WCS_Process
{
    public class DataControlItem : DataPoint, IXml, ICalcExpreable, IDataSourceable
    {
        
        public string RefPoint { get; set; }



        public string DataSourceName { get; set; }
        public string ValueField { get; set; }
        public string TextField { get; set; }
        /// <summary>
        /// combo 复杂组合多项分割字符串
        /// </summary>
        public string ComboItemsSplitString { get; set; }
        public int DefaultIndex { get; set; }

        /// <summary>
        /// 可计算列
        /// </summary>
        public string Method { get; set; }
        public string CalcExpr { get; set; }
        public string getValue(string uid, UpdateItem ui)
        {
            UpdateData ud = new UpdateData();
            ud.Items.Add(ui.text, ui);
            return getValue(uid, ud);
        }

        public override void LoadXml(XmlNode node)
        {
            base.LoadXml(node);

            DataSourceName = XmlUtil.GetSubNodeText(node, "@datasource");
            ValueField = XmlUtil.GetSubNodeText(node, "@valmember");
            TextField = XmlUtil.GetSubNodeText(node, "@txtmember");
            ComboItemsSplitString = XmlUtil.GetSubNodeText(node, "@membersplitor");
            CalcExpr = XmlUtil.GetSubNodeText(node, "@expr");
            Method = XmlUtil.GetSubNodeText(node, "@method");
            RefPoint = XmlUtil.GetSubNodeText(node, "@ref");
            int defidx = -1;
            if (int.TryParse(XmlUtil.GetSubNodeText(node, "@defaultindex"), out defidx))
            {
                DefaultIndex = defidx;
            }
        }

        public string getValue(string uid, UpdateData ud, string defaultval = null, int defaultIndex = -1)
        {
            DataControlItem dc = this;
            string val = null;
            string orgval = null;
            if (ud.Items.ContainsKey(dc.Name))//默认等于本字段值
            {
                val = ud.Items[dc.Name].value;
            }
            orgval = val;
            if (dc.DataType == "date")
            {
                DateTime dt;
                DateTime.TryParse(val, out dt);
                return dt.ToShortDateString();
            }
            if (dc.ComboName == "")
            {
                if (GlobalShare.DataPointMappings.ContainsKey(dc.Name))
                {
                    dc.ComboName = GlobalShare.DataPointMappings[dc.Name].ComboName;
                }
            }
            if (dc.DataType.ToLower() == "calcexpr")
            {
                CalcExpr ce = new CalcExpr();
                return ce.getValue(dc.CalcExpr, dc.Method, ud, ComboItemsSplitString);


            }
            if ((dc.DataType == "combo" || dc.DataType == "datacombo") && dc.ComboName != null && dc.ComboName.Trim().Length > 0)
            {
                DataChoice dcc = null;
                if (dc.DataType == "combo")
                    dcc = GlobalShare.GetGlobalChoice(uid, dc.ComboName);
                else
                {
                    DataControlItem dcb = this;
                    dcb.TextField = dc.TextField;
                    dcb.ValueField = dc.ValueField;
                    dcb.ComboItemsSplitString = dc.ComboItemsSplitString;
                    if(string.IsNullOrEmpty(dcb.DataSourceName))
                    {
                        this.DataSourceName = dcb.ComboName;
                    }
                    List<DataChoiceItem> dcis = dcb.GetDataSource(uid);
                    if (dcis == null)
                    {
                        throw new Exception("系统异常！");
                    }
                    dcc = new DataChoice();
                    dcc.Options.AddRange(dcis.ToArray());
                    List<string> vallist = new List<string>();
                    string[] strarr = dc.ValueField.Split(dc.ComboItemsSplitString.ToCharArray());
                    if (val == null || val.Trim().Length == 0)
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
                    if (this.RefPoint != null && this.RefPoint.Trim().Length > 0)
                    {
                        if (ud.Items.ContainsKey(this.RefPoint))
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
                        return string.Join(dc.ComboItemsSplitString, textlist);
                    }
                    else
                    {

                        if (defaultIndex >= 0 && defaultIndex < dcc.Options.Count)
                        {
                            return dcc.Options[defaultIndex].Text;
                        }
                        else
                        {
                            return orgval;
                        }

                    }

                }
                return orgval;

            }
            if (defaultval != null && defaultval.Trim().Length > 0)
            {
                if (val == null || val.Trim().Length == 0)
                    return defaultval;
            }
            return orgval;
        }

        public List<DataChoiceItem> GetDataSource(string strUid)
        {
            string strModel = "runningtime_S_{0}_U_{1}_V_{2}_S_{3}";
            string strdc = string.Format(strModel, this.DataSourceName, strUid, this.ValueField, this.ComboItemsSplitString);
            DataChoice dc = null;
            if (GlobalShare.DataChoices.ContainsKey(strdc))//保存默认dc到dc字典中，如果有同样的id+source直接获取
            {
                dc = GlobalShare.DataChoices[strdc];
            }
            else
            {
                if (this.DataSourceName == null || DataSourceName.Trim().Length == 0)
                    return null;
                List<DataCondition> conds = new List<DataCondition>();
                string msg = null;
                bool isextra = false;
                DataSet ds = DataSource.InitDataSource(this.DataSourceName, conds, strUid, out msg, ref isextra);
                if (ds == null)
                {
                    MessageBox.Show(string.Format("控件{0}无法获得数据！[{1}]", this.Name, msg));
                    return null;
                }
                dc = DataChoice.ConvertFromDataSet(ds, ValueField, TextField, ComboItemsSplitString);
                if (!GlobalShare.DataChoices.ContainsKey(strdc))
                {
                    GlobalShare.DataChoices.Add(strdc, dc);
                }
            }
            if (dc == null)
            {
                MessageBox.Show(string.Format("无法转换数据选择项{0}，这可能是由网络异常引起的，即将退出软件，请重新登录看是否正常！", strdc));


                return null;
            }
            if (!GlobalShare.DataChoices.ContainsKey(this.DataSourceName))//不断增加新的datachoice
            {
                GlobalShare.DataChoices.Add(this.DataSourceName, dc);
            }
            return dc.Options;
        }

    }

}
