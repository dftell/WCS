using System.Collections.Generic;
using System.Data;
using WolfInv.Com.MetaDataCenter;
using System.Windows.Forms;
using WolfInv.Com.WCS_Process;
using System;
using System.Linq;

namespace WolfInv.Com.CommCtrlLib
{

    public class DataComboBox1 : ComboBox, ITranslateableInterFace, IUserData, IDataSourceable
    {
        //public Panel Owner;
        string _uid;
        public string strUid
        {
            get
            {
                if (_uid == null)
                {
                    Control parent = this.Parent;
                    while (parent != null)
                    {
                        if (parent is IUserData)
                        {
                            _uid = (parent as IUserData).strUid;
                            if (_uid != null)
                            {
                                break;
                            }
                        }
                        parent = parent.Parent;
                    }
                }
                if (_uid == null)
                    throw new Exception("用户ID不能为空");
                return _uid;
            }
            set { _uid = value; }
        }
        public string DataSourceName { get; set; }
        public string ValueField { get; set; }
        public string TextField { get; set; }
        public string ComboItemsSplitString { get; set; }
        public DataComboBox1(string DataName, string uid)
        {
            DataSourceName = DataName;
            _uid = uid;
        }

        #region ITranslateableInterFace 成员
        UpdateData _data;
        List<DataTranMapping> _maps;

        public UpdateData NeedUpdateData
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
            }
        }

        public List<DataTranMapping> TranData
        {
            get
            {
                return _maps;
            }
            set
            {
                _maps = value;
            }
        }

        public List<DataTranMapping> RefData { get; set; }
        public List<UpdateData> InjectedDatas { get; set; }

        public UpdateData GetCurrFrameData()
        {
            return null;
        }

        #endregion

        public List<DataChoiceItem> GetDataSource()
        {
            if (this.DataSourceName == null || DataSourceName.Trim().Length == 0)
                return null;
            List<DataCondition> conds = new List<DataCondition>();
            if (this.TranData != null)
            {
                for (int i = 0; i < this.TranData.Count; i++)
                {
                    if (this.NeedUpdateData.Items.ContainsKey(TranData[i].ToDataPoint))
                    {
                        string strval = this.NeedUpdateData.Items[this.TranData[i].ToDataPoint].value;
                        if (strval == null || strval.Trim().Length == 0)
                        {
                            continue;
                        }
                        DataCondition cond = new DataCondition();
                        cond.Datapoint = new DataPoint(TranData[i].ToDataPoint);
                        cond.value = strval;
                        cond.Logic = ConditionLogic.And;
                        conds.Add(cond);
                    }
                }
            }
            string msg = null;
            DataSet ds = WolfInv.Com.WCS_Process.DataSource.InitDataSource(this.DataSourceName, conds, strUid, out msg);
            if (msg != null || ds == null)
            {
                MessageBox.Show(string.Format("控件{0}无法获得数据！错误：{1}", this.Name, msg));
                return null;
            }
            DataChoice dc = DataChoice.ConvertFromDataSet(ds, ValueField, TextField, true, ComboItemsSplitString);
            if (dc == null)
            {
                MessageBox.Show(string.Format("无法转换数据选择项{0}", this.Name));
                return null;
            }
            //DataChoice dcc = GlobalShare.GetGlobalChoice(strUid, DataSourceName);
            if (!GlobalShare.UserAppInfos[strUid].DataChoices.ContainsKey(this.DataSourceName))//不断增加新的datachoice
            {
                GlobalShare.UserAppInfos[strUid].DataChoices.Add(this.DataSourceName, dc);
            }
            return dc.Options;
        }

        public void ChangeDataSource()
        {

            this.DataSource = GetDataSource();
            if (this.DataSource == null) return;
            this.ValueMember = "Value";
            this.DisplayMember = "Text";
            //this.RefreshItems();
            //this.SelectedIndex = -1;
        }
    }


    public class DataComboBox : ComboBox, ITranslateableInterFace,IDataSourceable
    {

        public string DataSourceName { get; set; }
        public string ValueField { get; set; }
        public string TextField { get; set; }
        public string ComboItemsSplitString { get; set; }
        //public string UId;
        public DataComboBox(string DataName,string uid)
        {
            DataSourceName = DataName;
            //UId = uid;
            _uid = uid;

        }
        string _uid;
        public string strUid
        {
            get
            {
                if (_uid == null)
                {
                    Control parent = this.Parent;
                    while (parent != null)
                    {
                        if (parent is IUserData)
                        {
                            _uid = (parent as IUserData).strUid;
                            if (_uid != null)
                            {
                                break;
                            }
                        }
                        parent = parent.Parent;
                    }
                }
                if (_uid == null)
                    throw new Exception("用户ID不能为空");
                return _uid;
            }
            set { _uid = value; }
        }

        #region ITranslateableInterFace 成员
        UpdateData _data;
        List<DataTranMapping> _maps;

        public UpdateData NeedUpdateData
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
            }
        }

        public List<DataTranMapping> TranData
        {
            get
            {
                return _maps;
            }
            set
            {
                _maps = value;
            }
        }

        public List<UpdateData> InjectedDatas { get; set; }

        public UpdateData GetCurrFrameData()
        {
            return null;
        }

        #endregion

        public static void ClearRunningTimeDataSource()
        {
            string key = "runningtime_S_";
            List<string> currdcs =GlobalShare.DataChoices.Keys.ToList();
            currdcs.ForEach(a =>
            {
                if (a.StartsWith(key))
                {
                    GlobalShare.DataChoices.Remove(a);
                }
            });
        }
            

        public List<DataChoiceItem> GetDataSource()
        {
            string strModel = "runningtime_S_{0}_U_{1}_V_{2}_S_{3}";
            string strdc = string.Format(strModel, this.DataSourceName, this.strUid,this.ValueField,this.ComboItemsSplitString);
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
                if (this.TranData != null)
                {
                    for (int i = 0; i < this.TranData.Count; i++)
                    {
                        if (this.NeedUpdateData.Items.ContainsKey(TranData[i].ToDataPoint))
                        {
                            string strval = this.NeedUpdateData.Items[this.TranData[i].ToDataPoint].value;
                            if (strval == null || strval.Trim().Length == 0)
                            {
                                continue;
                            }
                            DataCondition cond = new DataCondition();
                            cond.Datapoint = new DataPoint(TranData[i].ToDataPoint);
                            cond.value = strval;
                            cond.Logic = ConditionLogic.And;
                            conds.Add(cond);
                        }
                    }
                }
                string msg = null;
                DataSet ds = WCS_Process.DataSource.InitDataSource(this.DataSourceName, conds, this.strUid, out msg);
                if (ds == null)
                {
                    MessageBox.Show(string.Format("控件{0}无法获得数据！[{1}]", this.Name, msg));
                    return null;
                }
                dc = DataChoice.ConvertFromDataSet(ds, ValueField, TextField,ComboItemsSplitString);
                if (!GlobalShare.DataChoices.ContainsKey(strdc))
                {
                    GlobalShare.DataChoices.Add(strdc,dc);
                }
            }
            if (dc == null)
            {
                MessageBox.Show(string.Format("无法转换数据选择项{0}", this.Name));
                return null;
            }
            if (!GlobalShare.DataChoices.ContainsKey(this.DataSourceName))//不断增加新的datachoice
            {
                GlobalShare.DataChoices.Add(this.DataSourceName, dc);
            }
            return dc.Options;
        }

        public void ChangeDataSource()
        {

            this.DataSource = GetDataSource();
            if (this.DataSource == null) return;
            this.ValueMember = "Value";
            this.DisplayMember = "Text";
            this.RefreshItems();
            this.SelectedIndex = -1;
        }
    }

    
}
