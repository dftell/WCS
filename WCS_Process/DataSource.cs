using System;
using System.Collections.Generic;
using System.Xml;
using System.Data;
using XmlProcess;
using WolfInv.Com.MetaDataCenter;
using WolfInv.Com;
using System.Linq;
using System.Xml.Linq;
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
            CurrNode = node;
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
            return InitDataSource(dscName,keys,values,GlobalShare.DefaultUser,out msg);
        }
        
        public static DataSet InitDataSource(string dsrcName,string[] keys,string[] values,string uid,out string msg)
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
            return InitDataSource(dsrcName, dcs,uid,out msg);
        }

        public static DataSet InitDataSource(string dsrcName, List<DataCondition> dc,string uid,out string msg)
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
            if (ds.isextradata)
            {
                msg = null;
                WCSExtraDataAdapter adp = new WCSExtraDataAdapter(ds.extradataconvertconfig);
                DataSet ret = adp.getData(ds.extradataassembly, ds.extradataclass, ds.extradatagetconfig, ds.extradatatype, ref msg);
                //DataSet ds = null;
                return FilterExtraData(ret,dc);
            }
            return GlobalShare.DataCenterClientObj.GetData(ds, dc,out msg);
        }

        public static DataSet InitDataSource(DataSource dsrobj, List<DataCondition> dcs, out string msg)
        {
            if(dsrobj.isextradata)
            {
                msg = null;
                WCSExtraDataAdapter adp = new WCSExtraDataAdapter(dsrobj.extradataconvertconfig);
                DataSet ds = adp.getData(dsrobj.extradataassembly, dsrobj.extradataclass, dsrobj.extradatagetconfig, dsrobj.extradatatype, ref msg);
                //DataSet ds = null;
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
                conditions.Add(string.Format("{0}{1}'{2}'", dc.Datapoint.Name, dc.strOpt, dc.value));
            }
            if (conditions.Count == 0)
                return ds;
            DataRow[] drs = ds.Tables[0].Select(string.Join(" And ", conditions.ToArray()));
            DataSet ret = ds.Clone();
            drs.ToList().ForEach(a=> {
                //DataRow dr = ret.Tables[0].NewRow();
                ret.Tables[0].Rows.Add(a.ItemArray);
            });
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

        public static List<UpdateData> DataSet2UpdateData(DataSet ds, string dsrcName, string uid)
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
            
            for(int i=0;i<ds.Tables[0].Rows.Count;i++)
            {
                UpdateData ud = new UpdateData();
                foreach (DataPoint dpt in dsr.AllDataPoint.Values)
                {
                    if (ud.Items.ContainsKey(dpt.Name))
                    {
                        continue;
                    }
                    string val = null;
                    if(ds.Tables[0].Columns.Contains(dpt.Name))
                    {
                        val = ds.Tables[0].Rows[i][dpt.Name].ToString();
                    }
                    ud.Items.Add(dpt.Name, new UpdateItem(dpt.Name, val));
                }
                
                ret.Add(ud);
            }
            return ret;
        }

        #region ICloneable 成员

        public object Clone()
        {
            DataSource ds = new DataSource(this.SourceName);
            ds.xmlReq = this.xmlReq.CloneNode(true);
            ds.LoadXml(this.CurrNode);
            if (this.SubSource != null)
            {
                ds.SubSource = this.SubSource.Clone() as DataSource;
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
