using System;
using System.Collections.Generic;
using System.Xml;
using System.Data;
using XmlProcess;
using WolfInv.Com.MetaDataCenter;
using WolfInv.Com;
using System.Linq;
using System.Xml.Linq;
using WolfInv.Com.WCSExtraDataInterface;
using System.Windows.Forms;

namespace WolfInv.Com.WCS_Process
{
    public interface iExtraData
    {
        //实现了 WCSExtraDataInterface的外部数据接口类
        bool isextradata { get; set; }
        string extradataassembly { get; set; }
        string extradataclass { get; set; }
        //转换功能用
        XmlNode extradataconvertconfig { get; set; }

        //外部数据用
        XmlNode extradatagetconfig { get; set; }

        //外部数据类型
        string extradatatype { get; set; }

        void LoadXml(XmlNode node);
    }

    public class DataSource:ICloneable,iExtraData
    {
        public string SourceName;
        public XmlNode xmlReq;
        public DataSource SubSource;
        public string SubSourceName;
        public string MainKey;
        public XmlNode CurrNode;

        Dictionary<string, DataPoint> _allpoint;
        public Dictionary<string,DataPoint> AllDataPoint
        {
            get
            {
                if(_allpoint == null)
                {

                    foreach(XmlNode node in this.xmlReq.SelectNodes("req/f"))
                    {
                        if (_allpoint == null)
                            _allpoint = new Dictionary<string, DataPoint>();
                        string name = XmlUtil.GetSubNodeText(node, "@i");
                        if (!_allpoint.ContainsKey(name))
                        {
                            DataPoint dp = new DataPoint(name);
                            _allpoint.Add(name, dp);
                        }
                    }

                }
                return _allpoint;
            }
        }


        public DataSource(string datasrc)
        {
            SourceName = datasrc;
        }

        #region extradata
        //实现了 WCSExtraDataInterface的外部数据接口类
        public bool isextradata { get; set; }
        public string extradataassembly { get; set; }
        public string extradataclass { get; set; }
        //转换功能用
        public XmlNode extradataconvertconfig { get; set; }

        //外部数据用
        public XmlNode extradatagetconfig { get; set; }

        //外部数据类型
        public string extradatatype { get; set; }

        #endregion

        public void LoadXml(XmlNode node)
        {
            string strsubname = XmlUtil.GetSubNodeText(node, "@subsource");
            ////if(!string.IsNullOrEmpty(strsubname))
            ////{
            ////    if (!GlobalShare.mapDataSource.ContainsKey(strsubname))
            ////    {
            ////        this.SubSource = GlobalShare.mapDataSource[strsubname];
            ////    }
            ////    MainKey = XmlUtil.GetSubNodeText(node,"@mainkey");
            ////}
            CurrNode = node;
            this.SubSourceName = strsubname;
            this.MainKey = XmlUtil.GetSubNodeText(node, "@mainkey");
            this.isextradata = XmlUtil.GetSubNodeText(node, "@isextradata")=="1";
            this.extradataassembly = XmlUtil.GetSubNodeText(node, "@extradataassembly");
            this.extradataclass = XmlUtil.GetSubNodeText(node, "@extradataclass");
            this.extradataconvertconfig = node.SelectSingleNode("extradataconvertconfig");
            this.extradatagetconfig = node.SelectSingleNode("extradatagetconfig");
            this.extradatatype = XmlUtil.GetSubNodeText(node, "extradatatype");
        }

        public static Dictionary<string, DataSource> GetDataSourceMapping()
        {
            Dictionary<string, DataSource> ret = new Dictionary<string, DataSource>();

            ////string xmlpath = string.Format("{0}\\xml\\datasource.xml",apppath);
            ////XmlDocument xmldoc = new XmlDocument();
            ////try
            ////{
            ////    xmldoc.Load(xmlpath);
            ////}
            ////catch(Exception ce)
            ////{
            ////    throw new Exception("can't get DataSource Config!");
            ////}
            XmlDocument xmldoc = GlobalShare.GetXmlFile("\\xml\\datasource.xml");
            if(xmldoc == null) throw new Exception("can't get DataSource Config!");
            XmlNodeList nodes = xmldoc.SelectNodes("/datasrcs/ds");
            foreach(XmlNode node in nodes)
            {
                string srcname = XmlUtil.GetSubNodeText(node,"@id");
                if(srcname != "" && !ret.ContainsKey(srcname))
                {
                    DataSource ds = new DataSource(srcname);
                    ds.LoadXml(node);
                    ds.xmlReq = node;
                    ret.Add(srcname,ds);
                }
            }
            return ret;
        }

        public static DataSet InitDataSource(string dscName, string[] keys, string[] values)
        {
            string msg = null;
            bool isextradata = false;
            return InitDataSource(dscName,keys,values,GlobalShare.DefaultUser,out msg,ref isextradata);
        }
        public static DataSet InitDataSource(string dsrcName, string[] keys, string[] values, string uid, out string msg)
        {
            bool isExtraData = false;
            return InitDataSource(dsrcName, keys, values, uid,out msg,ref isExtraData);
        }
        public static DataSet InitDataSource(string dsrcName,string[] keys,string[] values,string uid,out string msg,ref bool isExtraData)
        {
            msg = null;
            List<DataCondition> dcs = new List<DataCondition>();
            if (keys != null && values != null)
            {
                int cnt = keys.Length;
                if (values.Length < keys.Length)
                {
                    cnt = values.Length;
                }
                for (int i = 0; i < cnt; i++)
                {
                    DataCondition dc = new DataCondition();
                    dc.Datapoint = new DataPoint(keys[i]);
                    dc.value = values[i];
                    dcs.Add(dc);
                }
            }
            return InitDataSource(dsrcName, dcs,uid,out msg, ref isExtraData);
        }

        public static DataSet InitDataSource(string dsrcName, List<DataCondition> dc,string uid,out string msg, ref bool isExtraData)
        {
            if (!GlobalShare.UserAppInfos.ContainsKey(uid))
            {
                msg = string.Format("用户[{0}]未登录！",uid);
                return null;
            }
            msg = null;
            if (!GlobalShare.UserAppInfos[uid].mapDataSource.ContainsKey(dsrcName))
            {
                msg = string.Format("所查询数据源{0}未定义！",dsrcName);
                return null;
            }
            DataSource ds = GlobalShare.UserAppInfos[uid].mapDataSource[dsrcName];
            isExtraData = ds.isextradata;
            if (ds.isextradata)
            {
                msg = null;
                WCSExtraDataAdapter adp = new WCSExtraDataAdapter(uid,ds.extradataconvertconfig);
                DataSet ret = null;
                bool succ = adp.getData(ds.extradataassembly, ds.extradataclass, ds.extradatagetconfig, ds.extradatatype, ref ret,ref msg);
                if (succ == true)
                    return FilterExtraData(ret, dc);
                else
                {
                    return null;
                }
            }
            return GlobalShare.DataCenterClientObj.GetData(ds, dc,out msg);
        }
        public static UpdateData getDefaultData(string dsrcName,string uid)
        {
            if (!GlobalShare.UserAppInfos.ContainsKey(uid))
            {
                return null;
            }

            if (!GlobalShare.UserAppInfos[uid].mapDataSource.ContainsKey(dsrcName))
            {
                return null;
            }
            DataSource dsr = GlobalShare.UserAppInfos[uid].mapDataSource[dsrcName];
            UpdateData ud = new UpdateData();
            foreach (DataPoint dpt in dsr.AllDataPoint.Values)
            {
                if (ud.Items.ContainsKey(dpt.Name))
                {
                    continue;
                }
                string val = null;
                
                ud.Items.Add(dpt.Name, new UpdateItem(dpt.Name, val));

            }
            return ud;
        }

        public static DataSet InitDataSource(DataSource dsrobj, List<DataCondition> dcs, out string msg)//必须要修改，但目前无任何有效功能调用
        {
            if(dsrobj.isextradata)
            {
                msg = null;
                WCSExtraDataAdapter adp = new WCSExtraDataAdapter(null,dsrobj.extradataconvertconfig);
                DataSet ds = null;
                bool succ = adp.getData(dsrobj.extradataassembly, dsrobj.extradataclass, dsrobj.extradatagetconfig, dsrobj.extradatatype,ref ds, ref msg);
                //DataSet ds = null;
                if(!succ)
                {
                    return null;
                }
                return FilterExtraData(ds,dcs);
            }
            return GlobalShare.DataCenterClientObj.GetData(dsrobj, dcs, out msg);
        }

        public static DataSet FilterExtraData(DataSet ds, List<DataCondition> dcs)
        {
            if (ds == null)
                return ds;
            if (dcs.Count == 0)
                return ds;
            List<string> conditions = new List<string>();
            
            for(int i=0;i<dcs.Count;i++)
            {
                DataCondition dc = dcs[i];
                if (dc.value == null || dc.value.Trim().Length == 0)
                    continue;
                if (dc.Datapoint.Name == null || dc.Datapoint.Name.Trim().Length == 0)
                    continue;
                if(!ds.Tables[0].Columns.Contains(dc.Datapoint.Name))
                {
                    continue;
                }
                if(dc.strOpt == "like")
                {
                    conditions.Add(string.Format("{0} {1} '%{2}%'", dc.Datapoint.Name, dc.strOpt, dc.value));
                }
                else
                    conditions.Add(string.Format("{0} {1} '{2}'", dc.Datapoint.Name, dc.strOpt, dc.value));
            }
            if (conditions.Count == 0)
                return ds;
            DataRow[] drs = ds.Tables[0].Select(string.Join(" And ", conditions.ToArray()));
            DataSet ret = ds.Clone();
            drs.ToList().ForEach(a=> {
                //DataRow dr = ret.Tables[0].NewRow();
                ret.Tables[0].Rows.Add(a.ItemArray);
            });
            if (ds.Tables.Count > 1)
            {
                for (int i = 1; i < ds.Tables.Count; i++)
                {
                    for (int j = 0; j < ds.Tables[i].Rows.Count; j++)
                        ret.Tables[i].Rows.Add(ds.Tables[i].Rows[j].ItemArray);
                }
            }
            return ret;
        }

        public static Dictionary<string, DataSource> GetGlobalSourcesClone()
        {
            Dictionary<string, DataSource> ret = new Dictionary<string, DataSource>();
            foreach(DataSource ds in GlobalShare.mapDataSource.Values)
            {
                if (!ret.ContainsKey(ds.SourceName))
                {
                    ret.Add(ds.SourceName, ds.Clone() as DataSource);
                }
            }
            return ret;
        }

        public static List<UpdateData> DataSet2UpdateData(DataSet ds, string dsrcName, string uid,int tableindex=0,string keycol=null,string keycolval=null)
        {
            List<UpdateData> ret = new List<UpdateData>();
            if (!GlobalShare.UserAppInfos.ContainsKey(uid))
            {
                return null;
            }

            if (!GlobalShare.UserAppInfos[uid].mapDataSource.ContainsKey(dsrcName))
            {
                return null;
            }
            DataSource dsr =  GlobalShare.UserAppInfos[uid].mapDataSource[dsrcName];
            bool HasSubItems = false;
            string key = null;
            if(!string.IsNullOrEmpty(dsr.SubSourceName))
            {
                HasSubItems = true;
                key = dsr.MainKey;
                if(GlobalShare.UserAppInfos[uid].mapDataSource.ContainsKey(dsr.SubSourceName))
                {
                    dsr.SubSource = GlobalShare.UserAppInfos[uid].mapDataSource[dsr.SubSourceName];
                }
                
            }
            DataTable dt = ds.Tables[tableindex];

            DataTable subdt = null;
            if (ds.Tables.Count > tableindex+1)
            {
                subdt = ds.Tables[tableindex + 1];
            }
            string tablename = dt.TableName;
            string strSelect = "1=1";
            if(!string.IsNullOrEmpty(keycol))
            {
                strSelect = string.Format("{0}='{1}'",keycol,keycolval);
            }
            DataRow[] rows = dt.Select(strSelect);
            for(int i=0;i< rows.Length;i++)
            {
                UpdateData ud = new UpdateData();
                foreach (DataPoint dpt in dsr.AllDataPoint.Values)
                {
                    if (ud.Items.ContainsKey(dpt.Name))
                    {
                        continue;
                    }
                    string val = null;
                    if(dt.Columns.Contains(dpt.Name))
                    {
                        val = dt.Rows[i][dpt.Name].ToString();
                    }
                    ud.Items.Add(dpt.Name, new UpdateItem(dpt.Name, val));
                   
                }
                foreach(DataColumn dc in dt.Columns)
                {
                    if (ud.Items.ContainsKey(dc.ColumnName))
                        continue;
                    ud.Items.Add(dc.ColumnName,new UpdateItem(dc.ColumnName, rows[i][dc.ColumnName].ToString()));
                }
                ret.Add(ud);
            }
            if (ret.Count == 0)
                return ret;
            UpdateData tud = ret[0];
            string usekey = null;
            if (HasSubItems && !string.IsNullOrEmpty(key))
            {
                DataTranMappings dtms = new DataTranMappings();

                if (tud.Items.ContainsKey(key))
                {
                    //keyval = tud.Items[key].value;
                    usekey = key;
                }
                else
                {
                    if (dsr.isextradata)
                    {
                        dtms.LoadXml(dsr.extradataconvertconfig);
                        
                        if (dtms.AllTo.ContainsKey(key))
                        {
                            usekey = string.Format("{0}", dtms.AllTo[key].FromDataPoint.Name);
                        }
                        if(usekey == null)
                        {
                            if(dtms.AllFrom.ContainsKey(key))
                            {
                                usekey = string.Format("{0}", dtms.AllFrom[key].ToDataPoint);
                            }
                        }
                    }
                }
            }
            if(!HasSubItems)
            {
                return ret;
            }
            for(int i=0;i<ret.Count;i++)
            {
                string strKey = usekey ?? "";
                if(ret[i].Items.ContainsKey(strKey))
                {
                    string keyval = ret[i].Items[usekey].value;
                    List<UpdateData> subdatas = DataSet2UpdateData(ds, dsr.SubSourceName, uid, tableindex + 1, strKey, keyval);
                    if (subdatas != null)
                        ret[i].SubItems.AddRange(subdatas);
                }
            }

            return ret;
        }

        public static UpdateData DataRow2UpdateData(DataRow dr)
        {
            UpdateData ud = new UpdateData();
            
            foreach(DataColumn dc in dr.Table.Columns)
            {
                UpdateItem ui = new UpdateItem();
                ui.datapoint = new DataPoint();
                ui.datapoint.Name = dc.ColumnName;
                ui.value = dr[dc.ColumnName]?.ToString();
                if (!ud.Items.ContainsKey(dc.ColumnName))
                    ud.Items.Add(dc.ColumnName, ui);
            }
            return ud;
        }

        public static DataSet UpdateData2DataSet(List<UpdateData> uds,ref DataSet ret,string key,string keyval)
        {
            DataSet ds = ret;
            if (ds == null)
                ds = new DataSet();
            if (ds.Tables.Count <= 0)
                ds.Tables.Add();
            int curridx = ds.Tables.Count - 1;
            DataTable currtab = ds.Tables[curridx];
            //DataSet ret = new DataSet();
            if (uds.Count == 0)
                return null;
            UpdateData ud = uds.First();
            ud.Items.ToList().ForEach(a=> {
                if(!currtab.Columns.Contains(a.Key))
                    currtab.Columns.Add(a.Key);
                if(ud.keydpt.Name ==a.Key)
                {
                    if(keyval!=null && keyval.Trim().Length == 0)
                        keyval = a.Value.value;
                }
            });
            if(ud.keyvalue!=null&& ud.keyvalue.Trim().Length>0)
            {
                keyval = ud.keyvalue;
            }
            if(key != null)
            {
                if(!currtab.Columns.Contains(key))
                    currtab.Columns.Add(key);
                if (ud.SubItems.Count > 0)
                {
                    ds.Tables.Add();//新加一个表
                    UpdateData2DataSet(ud.SubItems, ref ds, key, keyval);
                }
            }
            
            
            uds.ForEach(a => {

                DataRow dr = currtab.NewRow();
                a.Items.ToList().ForEach(b=> {
                    if(currtab.Columns.Contains(b.Key))
                        dr[b.Key] = b.Value.value;
                });
                if (key != null)
                {
                    dr[key] = keyval;
                }
                currtab.Rows.Add(dr);
            });
            
            
            return ds;
        }

        #region ICloneable 成员

        public object Clone()
        {
            DataSource ds = new DataSource(this.SourceName);
            DataSource subds = null;
            ds.xmlReq = this.xmlReq.CloneNode(true);
            ds.LoadXml(this.CurrNode);
            if (string.IsNullOrEmpty(this.SubSourceName))
            {
                if (GlobalShare.mapDataSource.ContainsKey(SubSourceName))
                {
                    SubSource = GlobalShare.mapDataSource[SubSourceName];
                }
            }
            else
            {
                ds.SubSource = null;
            }
            return ds;
        }

        #endregion
    }
    
}
