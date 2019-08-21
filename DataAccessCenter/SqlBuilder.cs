using System;
using System.Collections.Generic;
using System.Text;
using WolfInv.Com.MetaDataCenter;
using System.Xml;
namespace WolfInv.Com.DataCenter
{
    public class SqlBuilder
    {
        DataRequest req;
        public SqlBuilder(DataRequest dataReq)
        {
            req = dataReq;
        }

        public string GenerateColumnAndAlisName(List<RequestItem> Cols, bool AddAliasName,List<string> tables)
        {
            Dictionary<string, List<DataColumn>> ColGroups = new Dictionary<string, List<DataColumn>>();
            Dictionary<string, RequestItem> reqitems = new Dictionary<string, RequestItem>();
            for (int i = 0; i < Cols.Count; i++)
            {
                if (!reqitems.ContainsKey(Cols[i].DataPt.Name))
                {
                    reqitems.Add(Cols[i].DataPt.Name, Cols[i]);
                }
                DataColumn dc = MappingConvert.DataPointToColumn(Cols[i].DataPt);
                List<DataColumn> collist;
                if (dc == null) 
                    continue;
                if(!tables.Contains(dc.Table))//��������ã�ֱ�������������ⲿϵͳ�������ⲿ����Ϊ���ñ������ڱ�ϵͳ��ʵ�ʵ���
                {
                    continue;
                }
                if (ColGroups.ContainsKey(dc.Table))
                {
                    collist = ColGroups[dc.Table];
                }
                else
                {
                    collist = new List<DataColumn>();
                    ColGroups.Add(dc.Table, collist);
                }
                collist.Add(dc);
            }
            ////ColGroups = ReOrganizeTable(ColGroups);
            StringBuilder strb = new StringBuilder();
            //List<string> 

            foreach (string strTab in ColGroups.Keys)
            {
                string strTabcols = "";
                if(!tables.Contains(strTab))
                    tables.Add(strTab);
                List<DataColumn> collist = ColGroups[strTab];
                for (int i = 0; i < collist.Count; i++)
                {
                    RequestItem ri = reqitems[collist[i].DataPoint];

                    string sFullCol = string.Format("{0}.[{1}]", collist[i].Table, collist[i].Column);
                    if (ri.Format != null && ri.Format.Trim().Length > 0)
                    {
                        string[] flds = ri.FormatFields.Split(',');
                        string fmt = ri.Format;
                        for (int n = 0; n < flds.Length; n++)
                        {
                            string strindex = "{" + string.Format("{0}", n) + "}";
                            string sDpt = flds[n];
                            if (DataAccessCenter.DataPointMappings.ContainsKey(sDpt))
                            {
                                DataColumn FillCol = DataAccessCenter.DataColumnMappings[sDpt];
                                if(FillCol == null) continue ;
                                sDpt = string.Format("{0}.[{1}]",FillCol.Table, FillCol.Column);
                            }
                            fmt = fmt.Replace(strindex, sDpt);
                        }
                        sFullCol = fmt;// ri.Format.Replace(ri.DataPt.Name, string.Format("{0}.[{1}]", collist[i].Table, collist[i].Column));
                    }
                    if (AddAliasName)
                    {
                        strTabcols += string.Format("{0} as {1},", sFullCol, collist[i].DataPoint);
                    }
                    else
                    {
                        strTabcols += sFullCol + ",";
                    }
                }
                strb.Append(strTabcols);
            }
            string ret = strb.ToString();
            if (ret.Length > 1 && ret.EndsWith(","))
            {
                ret = ret.Substring(0, ret.Length - 1);
            }
            return ret;
        }

        public string GenerateQuerySql()
        {
            /*
            Dictionary<string, List<DataColumn>> ColGroups = new Dictionary<string, List<DataColumn>>();
           
            for (int i = 0; i < req.RequestItems.Count; i++)
            {
                DataColumn dc = MappingConvert.DataPointToColumn(req.RequestItems[i].DataPt);
                List<DataColumn> collist;
                if (ColGroups.ContainsKey(dc.Table))
                {
                    collist = ColGroups[dc.Table];
                }
                else
                {
                    collist = new List<DataColumn>();
                    ColGroups.Add(dc.Table, collist);
                }
                collist.Add(dc);
            }
            ////ColGroups = ReOrganizeTable(ColGroups);
            StringBuilder strb = new StringBuilder();
            List<string> tables = new List<string>();
            foreach (string strTab in ColGroups.Keys)
            {
                string strTabcols = "";
                tables.Add(strTab);
                List<DataColumn> collist = ColGroups[strTab];
                for (int i = 0; i < collist.Count; i++)
                {
                    strTabcols += string.Format("{0}.[{1}] as {2},", collist[i].Table, collist[i].Column, collist[i].DataPoint);
                }
                strb.Append(strTabcols);
            }
            string ret = strb.ToString();
            if (ret.Length > 1)
            {
                ret = ret.Substring(0, ret.Length - 1);
            }*/
            //
            List<string> tables = null;


            //2019/8/21 Ϊ�����ⲿϵͳ���������̷ǹ�����������Ȼ�ȡ��Ч���ٸ��ݱ��ȡ�ֶΣ��Ƿ��������ֶ�





            tables = getValidTables(req.RequestItems);





            string ret = GenerateColumnAndAlisName(req.RequestItems,true,tables);//��ȡ��group��ֵ����֤tables��������columns ��group by�ĺ�
            string sbtab = "";//GenerateFrom(ColGroups);//��ʹ�ø��߼�
            
            string sGroupBy = GenerateColumnAndAlisName(req.GroupByItems, false, tables);
            if (sGroupBy != null && sGroupBy.Trim().Length > 0)
            {
                sGroupBy = " Group By " + sGroupBy;
            }
            sbtab = GenerateFrom(tables);
            string strwhere = "";
            strwhere = TableSqlProcess.GenerateWhere(ref strwhere,req.ConditonGroup,tables);
            if (req.ConditonGroup != null)
            {

            }
            return string.Format("SELECT {0} {1} {5} {4} 1=1 {2} {3}",ret,sbtab,strwhere,this.GenerateOrderSql(req.OrderItems),req.GroupByItems.Count == 0?"Where":"Having",sGroupBy) ;
        }

        string GenerateFrom(Dictionary<string, List<DataColumn>> ColGroups)
        {
            int cnt = 0;
            List<string> tablist = new List<string>();
            string strInnerSql = "";
            StringBuilder sbtab = new StringBuilder();
            #region for table
            foreach (string strTab in ColGroups.Keys)
            {

                if (cnt == 0)
                {
                    sbtab.Append(string.Format(" FROM {0} ", strTab));
                    cnt++;
                    tablist.Add(strTab);
                    continue;
                }
                //�����ϵӳ��������иñ�����ӳ���ϵ
                bool onFlag = false;
                sbtab.Append(string.Format(" LEFT JOIN {0} ", strTab));
                if (DataAccessCenter.DataTableRefs.ContainsKey(strTab))
                {
                    DataTableReference dtr = DataAccessCenter.DataTableRefs[strTab];
                    foreach (DataColumnReference dcr in dtr.References)
                    {
                        if (tablist.Contains(dcr.MainColumn.Table))//���ǰ����б��а������ñ���
                        {
                            sbtab.AppendFormat(" {0} {1} {2}.{3} = {4}.{5}", "", onFlag ? "AND" : "ON", dcr.MainColumn.Table, dcr.MainColumn.Column, dcr.LientColumn.Table, dcr.LientColumn.Column);
                            onFlag = true;
                        }
                    }
                }
                if (DataAccessCenter.DataTableReRefs.ContainsKey(strTab))
                {
                    DataTableReference dtr = DataAccessCenter.DataTableReRefs[strTab];
                    foreach (DataColumnReference dcr in dtr.References)
                    {
                        if (tablist.Contains(dcr.MainColumn.Table))//���ǰ����б��а������ñ���
                        {
                            sbtab.AppendFormat(" {0} {1} {2}.{3} = {4}.{5}", "", onFlag ? "AND" : "ON", dcr.MainColumn.Table, dcr.MainColumn.Column, dcr.LientColumn.Table, dcr.LientColumn.Column);
                            onFlag = true;
                        }
                    }
                }
                tablist.Add(strTab);
                cnt++;
            }
            #endregion
            return sbtab.ToString();
        }

        List<string> getValidTables(List<RequestItem> Cols)
        {
            List<string> tables = new List<string>();
            //HashSet<string> ColGroups = new HashSet<string>();
            for (int i = 0; i < Cols.Count; i++)
            {
               
                DataColumn dc = MappingConvert.DataPointToColumn(Cols[i].DataPt);
                if (dc == null)
                    continue;
                if (!tables.Contains(dc.Table))
                {
                    tables.Add(dc.Table);
                }
            }
            List<string> tablist = new List<string>();
           
            for(int cnt=0;cnt<tables.Count;cnt++)
            {
                string strTab = tables[cnt];
                if (cnt == 0)
                {
                    tablist.Add(strTab);
                    continue;
                }
                //�����ϵӳ��������иñ�����ӳ���ϵ
                bool onFlag = false;
                if (DataAccessCenter.DataTableReRefs.ContainsKey(strTab))
                {
                    DataTableReference dtr = DataAccessCenter.DataTableReRefs[strTab];
                    Dictionary<string, DataColumnReference> dcrs = dtr.GetMaps();

                    for (int re = tablist.Count - 1; re >= 0; re--)//��ǰ�ң���ֻ��һ��
                    {
                        if (dcrs.ContainsKey(tablist[re]))//�ҵ�
                        {
                            onFlag = true;
                            break;
                        }
                    }

                }
                if (!onFlag && DataAccessCenter.DataTableRefs.ContainsKey(strTab))
                {
                    DataTableReference dtr = DataAccessCenter.DataTableRefs[strTab];
                    Dictionary<string, DataColumnReference> dcrs = dtr.GetMaps();
                    for (int re = tablist.Count - 1; re >= 0; re--)//�Ӻ���ǰ�ң���ֻ��һ��
                    {
                        if (dcrs.ContainsKey(tablist[re]))//�ҵ�
                        {
                            onFlag = true;
                            break;
                        }

                    }

                }
                if (!onFlag)
                {
                    continue;
                }
                tablist.Add(strTab);
            }
            return tablist;
        }

        string GenerateFrom(List<string> tables)
        {
            int cnt = 0;
            List<string> tablist = new List<string>();
            string strInnerSql = "";
            StringBuilder sbtab = new StringBuilder();
            #region for table
            for(int i=0;i<tables.Count;i++)
            {
                string strTab = tables[i];

                if (cnt == 0)
                {
                    sbtab.Append(string.Format(" FROM {0} ", strTab));
                    cnt++;
                    tablist.Add(strTab);
                    continue;
                }
                //���ӱ���ǰ�����ñ��ں������ԭ�����Ҳ���������£���֮ǰ�ı�����Ӻ�ǰ�ұ����õı�
                //�����ϵӳ��������иñ�����ӳ���ϵ 
                bool onFlag = false;
                sbtab.Append(string.Format(" LEFT JOIN {0} ", strTab));
                //if (DataAccessCenter.DataTableRefs.ContainsKey(strTab))
                //{
                //    DataTableReference dtr = DataAccessCenter.DataTableRefs[strTab];
                //    foreach (DataColumnReference dcr in dtr.References)
                //    {
                //        if (tablist.Contains(dcr.MainColumn.Table))//���ǰ����б��а������ñ���
                //        {
                //            sbtab.AppendFormat(" {0} {1} {2}.{3} = {4}.{5}", "", onFlag ? "AND" : "ON", dcr.MainColumn.Table, dcr.MainColumn.Column, dcr.LientColumn.Table, dcr.LientColumn.Column);
                //            onFlag = true;
                //        }
                //    }
                //}
                if ( DataAccessCenter.DataTableReRefs.ContainsKey(strTab))
                {
                    DataTableReference dtr = DataAccessCenter.DataTableReRefs[strTab];
                    //dtr.
                    Dictionary<string, DataColumnReference> dcrs = dtr.GetMaps();
                    
                    for (int re = tablist.Count - 1; re >= 0; re--)//��ǰ�ң���ֻ��һ��
                    {
                        if (dcrs.ContainsKey(tablist[re]))//�ҵ�
                        {
                            DataColumnReference dcr = dcrs[tablist[re]];
                            sbtab.AppendFormat("\r {0} {1} {2}.{3} = {4}.{5}", "", onFlag ? "AND" : "ON", dcr.MainColumn.Table, dcr.MainColumn.Column, dcr.LientColumn.Table, dcr.LientColumn.Column);
                            onFlag = true;
                            break;
                        }
                    }
                    
                }
                if (!onFlag && DataAccessCenter.DataTableRefs.ContainsKey(strTab))
                {
                    DataTableReference dtr = DataAccessCenter.DataTableRefs[strTab];
                    Dictionary<string, DataColumnReference> dcrs = dtr.GetMaps();
                    for (int re = tablist.Count - 1; re >= 0; re--)//�Ӻ���ǰ�ң���ֻ��һ��
                    {
                        if (dcrs.ContainsKey(tablist[re]))//�ҵ�
                        {
                            DataColumnReference dcr = dcrs[tablist[re]];
                            sbtab.AppendFormat(" {0} {1} {2}.{3} = {4}.{5}", "", onFlag ? "AND" : "ON", dcr.MainColumn.Table, dcr.MainColumn.Column, dcr.LientColumn.Table, dcr.LientColumn.Column);
                            onFlag = true;
                           
                            break;
                        }

                    }
                    
                }
                if (!onFlag)
                {
                    continue;
                    //throw new Exception(string.Format("{0}�޷��ҵ���ϵ��",strTab));
                }
                tablist.Add(strTab);
                cnt++;
            }
            #endregion
            return sbtab.ToString();
        }

        
        public List<string> GenerateUpdateSql(XmlNode node)
        {
            if (req.ConditonGroup == null) return null;
            if (req.ConditonGroup.Datapoint == null || req.ConditonGroup.Datapoint.Text == "") return null;
            DataPoint keypt = req.ConditonGroup.Datapoint;
            string val = req.ConditonGroup.value;
            Dictionary<string, Dictionary<string, DataColumn>> finallist = ReOrganizeTable(OrganizeTable(req.updatedata, keypt));
            UpdateSqlProcess usp = new UpdateSqlProcess();
            usp.datas = req.updatedata;
            usp.SingleSqlArr = finallist;
            usp.key = keypt;
            usp.val = val;
            usp.type = req.RequestType;
            return usp.GenerateSingleSql(req.ConditonGroup);
            
        }

        public List<string> GenerateUpdateSql(UpdateData datas,DataRequestType reqtype,DataCondition condition)
        {
            List<string> ret = new List<string>();
            Dictionary<string, UpdateData> tabupdatas = new Dictionary<string,UpdateData>();// UpdateData();
            foreach (string strdpt  in datas.Items.Keys)//�ֶ����ֱ𴫵ݣ�������������
            {
                DataColumn dc = MappingConvert.DataPointToColumn(datas.Items[strdpt].datapoint);
                if (dc == null)
                    continue;
                string tab = dc.Table;
                if (dc.OwnTable.IsView) continue;
                UpdateData subdata = new UpdateData();
                if(tabupdatas.ContainsKey(tab))
                {
                    subdata = tabupdatas[tab];
                }
                else
                {
                    tabupdatas.Add(tab,subdata);
                }
                subdata.Items.Add(strdpt,datas.Items[strdpt]);
                
            }
            foreach(string tab in tabupdatas.Keys)
            {
                UpdateData subdata = tabupdatas[tab];
                if (datas.keydpt.Name == condition.Datapoint.Name && (condition.strOpt.Trim().ToLower() == "in" || condition.strOpt.Trim().ToLower() == "like"))
                {
                }
                else
                {
                    AddRefKeyItemInData(ref subdata, tab, datas.keydpt.Name, datas.keyvalue,false);
                }
                subdata.keyvalue = datas.keyvalue;
                subdata.keydpt = datas.keydpt;
                DataPoint keydpt = datas.keydpt;
                string val = datas.keyvalue;



                Dictionary<string, Dictionary<string, DataColumn>> finallist = ReOrganizeTable(OrganizeTable(subdata, keydpt));
                UpdateSqlProcess usp = new UpdateSqlProcess();
                usp.datas = subdata;
                usp.SingleSqlArr = finallist;
                usp.key = keydpt;
                usp.val = val;
                usp.type = reqtype;
              
                List<string> retsqls = usp.GenerateSingleSql(condition);
                if(retsqls == null) return null;
                ret.AddRange(retsqls.ToArray()); 
            }
            return ret;
            
        }

        public List<string> GenerateAddSql()
        {
            return new List<string>();
        }

        public List<string> GenerateUpdateSql(Dictionary<string, Dictionary<string, DataColumn>> moder,DataPoint keypt,string val,UpdateData data)
        {
            return new List<string>();
        }

        Dictionary<string, List<DataColumn>> ReOrganizeTable(Dictionary<string,List<DataColumn>> tablist)
        {
            Dictionary<string, Dictionary<string, DataColumn>> input = new Dictionary<string, Dictionary<string, DataColumn>>();
            foreach (string tab in tablist.Keys)
            {
                Dictionary<string, DataColumn> cols = new Dictionary<string, DataColumn>();
                for (int i = 0; i < tablist[tab].Count; i++)
                {
                    DataColumn dc = tablist[tab][i];
                    if (!cols.ContainsKey(dc.DataPoint))
                    {
                        cols.Add(dc.DataPoint, dc);
                    }
                }
                input.Add(tab, cols);
               
            }
            Dictionary<string, Dictionary<string, DataColumn>> output = ReOrganizeTable(input);
               Dictionary<string, List<DataColumn>> ret = new Dictionary<string, List<DataColumn>>();
               foreach (string tab in output.Keys)
               {
 
                   DataColumn[] dcs = new DataColumn[output[tab].Values.Count];
                   output[tab].Values.CopyTo(dcs, 0);
                   List<DataColumn> lst = new List<DataColumn>();
                   lst.AddRange(dcs);
                   ret.Add(tab,lst );
               }
               return ret;

        }
        
        Dictionary<string, Dictionary<string, DataColumn>> ReOrganizeTable(Dictionary<string, Dictionary<string, DataColumn>> tablist)
        {
            List<string> tabs = new List<string>();
            if (tablist == null) return null;
            List<string> orgtabs = new List<string>();
            foreach (string tab in tablist.Keys)
            {
                orgtabs.Add(tab);
            }
            for (int i = 0; i < orgtabs.Count; i++)
            {
                string tab = orgtabs[i];
                if (!DataAccessCenter.DataTableRefs.ContainsKey(tab))
                {
                    if(!tabs.Contains(tab))
                        tabs.Insert(0, tab);
                }
                else
                {
                    DataTableReference tabref = DataAccessCenter.DataTableRefs[tab];
                    bool flag = false;
                    foreach (DataColumnReference dcr in tabref.References)
                    {
                        if (orgtabs.Contains(dcr.MainColumn.Table))
                        {
                            if (!tabs.Contains(dcr.MainColumn.Table))
                                tabs.Add(dcr.MainColumn.Table);
                            flag = true;
                        }
                        
                    }
                    if(!tabs.Contains(tab))
                        tabs.Add(tab);
                }
            }
            /*
            foreach (string tab in tablist.Keys)
            {
                if (!DataAccessCenter.DataTableRefs.ContainsKey(tab))//û������������������ǰ��
                {
                    tabs.Insert(0, tab);
                }
                else
                {
                    //���ǰ��ı���ǰ�����ã���ǰ���������
                    DataTableReference tabref = DataAccessCenter.DataTableRefs[tab];
                    bool flag = false ;
                    foreach (DataColumnReference dcr in tabref.References)
                    {
                        if (tabs.Contains(dcr.MainColumn.Table))
                        {
                            tabs.Add(tab);
                            flag = true;
                            break;
                        }
                    }
                    if (!flag)
                        tabs.Add(tab);

                }
            }
             * */
            //����Ƿ������ÿ�ǰ�ı�
            List<string> checktabs = new List<string>();
            checktabs = tabs;
            /*
            for (int i = 0; i < tabs.Count; i++)
            {
                if (!DataAccessCenter.DataTableReRefs.ContainsKey(tabs[i]))//����ñ�δ�������ã��ŵ���ǰ��
                {
                    checktabs.Insert(0, tabs[i]);
                }
                else
                {
                    DataTableReference tabref = DataAccessCenter.DataTableReRefs[tabs[i]];
                    //����ñ�ǰ��ı������ˣ��������õı�ǰ
                    bool flag = false;
                    foreach (DataColumnReference dcr in tabref.References)
                    {
                       
                        for (int n = 0; n < checktabs.Count; n++)
                        {
                            if (dcr.LientColumn.Table == checktabs[n])
                            {
                                
                                checktabs.Insert(n, tabs[i]);

                                break;
                            }
                        }
                        
                    }
                    if (!flag)
                        checktabs.Add(tabs[i]);
                }
            }
             */
            //���ռ�����б����
            Dictionary<string, Dictionary<string, DataColumn>> ret = new Dictionary<string, Dictionary<string, DataColumn>>();
            for (int i = 0; i < checktabs.Count ; i++)
            {
                ret.Add(checktabs[i],tablist[checktabs[i]]);
            }
            return ret;
        }

        Dictionary<string, Dictionary<string, DataColumn>> OrganizeTable(UpdateData updata, DataPoint keypt)
        {
            Dictionary<string, Dictionary<string, DataColumn>> ret = new Dictionary<string, Dictionary<string, DataColumn>>();
            //panel data
            if (updata.Items != null && updata.Items.Count > 0)
            {
                foreach (string strdpt in updata.Items.Keys)
                {
                    Dictionary<string,DataColumn> dcs = new Dictionary<string,DataColumn>();
                    DataPoint dp = updata.Items[strdpt].datapoint ;
                    DataColumn dc = MappingConvert.DataPointToColumn(dp);
                    if (ret.ContainsKey(dc.Table))
                    {
                        dcs = ret[dc.Table];
                    }
                    else
                    {
                        ret.Add(dc.Table ,dcs);
                    }
                    if(!dcs.ContainsKey(dc.DataPoint))
                        dcs.Add(dc.DataPoint,dc);
                }
            }
            
            return ret;
        }

        string GenerateOrderSql(List<OrderItem> ords)
        {
            if (ords == null || ords.Count == 0) return "";
            int cnt = 0;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < ords.Count; i++)
            {
                OrderItem oi = ords[i];
                if (!DataAccessCenter.DataColumnMappings.ContainsKey(oi.DataPt.Name))
                {
                    continue;
                }
                DataColumn dc = DataAccessCenter.DataColumnMappings[oi.DataPt.Name];



                //support format order sentence by zhouys 3/29 2012
                //sb.AppendFormat("{0} {1}.{2} {3} ", cnt == 0 ? "Order By" : ",", dc.Table, dc.Column, oi.desc);
                string strRet = string.Format("{0} {1} {2}  ", cnt == 0 ? "Order By" : ",", oi.Format.Trim().Length == 0 ? string.Format("{0}.{1}", dc.Table, dc.Column) : oi.Format, oi.desc);
                strRet = string.Format(strRet, string.Format("{0}.{1}", dc.Table, dc.Column));
                sb.Append(strRet);
                cnt++;
            }
            return sb.ToString();
        }

        public static string AddRefKeyItemInData(ref UpdateData subdata, string subtable, string key, string val,bool ForceInsert)
        {

            DataPoint seckey = null;
            //�������ݱ�����ñ��в���,������õ�������,���е�ֵӦ��������ļ�ֵ���
            #region

            if (DataAccessCenter.DataTableRefs.ContainsKey(subtable))
            {
                DataTableReference dtr = DataAccessCenter.DataTableRefs[subtable];
                for (int r = 0; r < dtr.References.Count; r++)
                {
                    if (dtr.References[r].MainColumn.DataPoint == key)//������õ���ƥ��
                    {
                        seckey = new DataPoint(dtr.References[r].LientColumn.DataPoint);
                        break;
                    }
                }
            }
            else//����
            {
                return string.Format("��{0}���ô���", subtable);
            }
            if (seckey == null)
            {
                
                DataColumn dc = MappingConvert.DataPointToColumn(new DataPoint(key));
                if (dc.Table != subtable)
                {
                    DataTableRef tabref = MappingConvert.GetTableRef(dc.Table, subtable);
                    if (tabref == null)
                    {
                        tabref = MappingConvert.GetTableRef(subtable, dc.Table);
                    }
                    if (tabref == null)
                    {
                        if (ForceInsert)
                            return string.Format("��{0}���ô���,�������������޷��ҵ�ƥ��������У�", subtable);
                        else
                            return null;
                    }
                    if (tabref.Column.Table == subtable)
                    {
                        seckey = new DataPoint(tabref.Column.DataPoint);
                    }
                    else
                    {
                        while (tabref.Next != null)
                        {
                            tabref = tabref.Next;
                            if (tabref.Table == subtable)
                            {
                                seckey = new DataPoint(tabref.Column.DataPoint);
                                break;
                            }
                        }

                    }
                }
                else
                {
                    seckey = new DataPoint(key);
                }


            }
            #endregion
            if (seckey == null)
            {
                if (ForceInsert)
                    return "�޷����ֹؼ���";
                else
                    return null;
            }
            if (subdata.Items.ContainsKey(seckey.Name))//������������а�����������
            {
                if(val != null && val.Trim().Length >0)
                    subdata.Items[seckey.Name].value = val;
            }
            else//���û�д��ڣ����������к������ֵ��Ϊ������s
            {
                subdata.Items.Add(seckey.Name, new UpdateItem(seckey.Name, val));
            }
            return null;
        }
        
    }

    public class UpdateSqlProcess
    {
        public DataRequestType type;
        public Dictionary<string, Dictionary<string, DataColumn>> SingleSqlArr = new Dictionary<string, Dictionary<string, DataColumn>>();
        public Dictionary<string, Dictionary<string, DataColumn>> BatchSqlArr = new Dictionary<string, Dictionary<string, DataColumn>>();

        public UpdateData datas;
        public DataPoint key;
        public string val;
        public UpdateSqlProcess()
        {

        }

        public List<string> GenerateSingleSql(DataCondition condition)
        {
            List<string> sqls = new List<string>();
            foreach (string tab in SingleSqlArr.Keys)
            {
                List<UpdateItem> tabupdatas = new List<UpdateItem>();
                foreach (string strdpt in datas.Items.Keys)
                {
                    if (MappingConvert.DataPointToColumn(datas.Items[strdpt].datapoint).Table == tab)
                    {
                        tabupdatas.Add(datas.Items[strdpt]);
                    }
                }
                TableSqlProcess tsp ;
                switch(type)
                {
                    case DataRequestType.Add:
                        {
                            tsp = new TableAddSqlProcess();
                            break;
                        }
                    case DataRequestType.Delete:
                        {
                            tsp = new TableDeleteSqlProcess();
                            break;
                        }
                    case DataRequestType.Update:
                    case DataRequestType.BatchUpdate :
                    default:
                        {
                            tsp = new TableUpdateSqlProcess();
                            break;
                        }
                }

                tsp.datas = tabupdatas;
                
                DataColumn dc = DataAccessCenter.DataColumnMappings[condition.Datapoint.Name];
                //string sql = TableSqlProcess.GetColumnWhere(condition, dc);
                string sql = tsp.GenerateSql(tab, key, val, condition);
                if(sql != null && sql.Trim().Length > 0)
                    sqls.Add(sql);
            }
            return sqls;

        }
    }

    public abstract class TableSqlProcess
    {
        public List<UpdateItem> datas;
        public abstract string GenerateSql(string Table, DataPoint key, string keyval,DataCondition dc);
        
        public static string GenerateWhere(ref string ret, DataCondition datacond,List<string> tables)
        {
            if (ret == null)
                ret = "";
            if (datacond == null) return ret;
            string substr = "";

            if (datacond.SubConditions != null)
            {
                for (int i = 0; i < datacond.SubConditions.Count; i++)
                {
                    GenerateWhere(ref substr, datacond.SubConditions[i],tables);
                }
                ret += string.Format("{1} ({2} {0})", substr.ToString(), datacond.Logic.ToString(),substr.ToString().Trim().StartsWith("Or") ?"1<>1":"1=1");
            }
            else
            {
                if (!datacond.IsValidate) return ret;
                //ret = new StringBuilder();
                if (!DataAccessCenter.DataPointMappings.ContainsKey(datacond.Datapoint.Name))
                {
                    List<string> names = DataPointReg.GetExpresses(datacond.Datapoint.Name);
                    if (names != null)
                    {
                        for (int n = 0; n < names.Count; n++)
                        {
                            DataPoint dpt = new DataPoint(names[n]);
                            DataColumn dptdc = MappingConvert.DataPointToColumn(dpt);
                            if (!tables.Contains(dptdc.Table))
                            {
                                continue;
                            }
                            ret += GetColumnWhere(datacond, dptdc);
                            return ret;
                        }
                    }
                    else
                    {
                        return ret;
                    }
                }
                DataColumn dc = MappingConvert.DataPointToColumn(datacond.Datapoint);
                if(!tables.Contains(dc.Table))
                {
                    return ret;
                }
                ret += GetColumnWhere(datacond, dc); //string.Format(" {0} {1}.{2}{3}'{4}'", datacond.Logic.ToString(), dc.Table, dc.Column, datacond.strOpt, datacond.value);
            }
            return ret;
        }
        
        public static string GetColumnWhere(DataCondition datacond, DataColumn dc)
        {
           
            string strRet = "";
            string strFmtResult = "";
            //support the format condition by zhouys 3/31 2012
            if (datacond.Format != null && datacond.Format.Trim().Length > 0)
            {
                strFmtResult = "";
                DataCondition ri = datacond;
                string[] flds = ri.FormatFields.Split(',');
                string fmt = ri.Format;
                for (int n = 0; n < flds.Length; n++)
                {
                    string strindex = "{" + string.Format("{0}", n) + "}";
                    string sDpt = flds[n];
                    if (DataAccessCenter.DataPointMappings.ContainsKey(sDpt))
                    {
                        DataColumn FillCol = DataAccessCenter.DataColumnMappings[sDpt];
                        if (FillCol == null) continue;
                        sDpt = string.Format("{0}.[{1}]", FillCol.Table, FillCol.Column);
                    }
                    fmt = fmt.Replace(strindex, sDpt);
                }
                strFmtResult = fmt;
            }
            else
            {
                strFmtResult = string.Format("{0}.{1}", dc.Table, dc.Column);
            }
            if (datacond.strOpt.Trim().ToLower().Contains("is"))//for operate "is" or "is not",return  zhouys 2012/03/28 
            {
                return string.Format(" {0} {1} {2} {3} ", datacond.Logic.ToString(), strFmtResult, datacond.strOpt, datacond.value); ;
            }
            switch (dc.DataType)
            {
                case "int":
                case "money":
                case "float":
                    {

                        
                        float testint;
                        string scalc = datacond.value;
                        MathHandle mh = new MathHandle(datacond.value);
                        try
                        {
                            string result = mh.Handle();
                            float.TryParse(result, out testint);
                            if (!float.TryParse(result, out testint))//�����������
                            {
                                strRet = "";
                                break;
                            }
                            
                        }
                        catch(Exception ce)
                        {
                            //strRet = "";
                            //throw new Exception("�������֧�ָù��ܣ�");
                        }
                        if (!DataAccessCenter.DataPointMappings.ContainsKey(datacond.Datapoint.Name))
                        {
                            strRet = string.Format("{0} {1} {2} ({3})", datacond.Logic.ToString(), strFmtResult, datacond.strOpt, scalc);
                            //strRet = strRet.Replace("{" + dc.DataPoint + "}", string.Format("{0}.{1}", dc.Table, dc.Column));
                        }
                        else
                        {
                            strRet = string.Format(" {0} {1} {2} ({3})", datacond.Logic.ToString(), strFmtResult, datacond.strOpt, scalc);
                        }
                        break;
                    }
                case "date":
                case "datetime":
                case "smalldatetime":
                    {
                        if (datacond.strOpt == "like")
                        {
                            strRet = string.Format(" {0} convert(varchar(10),{1},120) {2} '%{3}%'", datacond.Logic.ToString(), strFmtResult, datacond.strOpt, datacond.value);
                        }
                        else
                        {
                            strRet = string.Format(" {0} {1} {2} '{3}'", datacond.Logic.ToString(), strFmtResult, datacond.strOpt, datacond.value);
                        }
                        break;
                    }
                case "text":
                case "varchar":
                case "nvarchar":
                default:
                    {
                        if (datacond.strOpt == "" || datacond.strOpt == "=")
                            strRet = string.Format(" {0} {1} {2}'{3}'", datacond.Logic.ToString(), strFmtResult, datacond.strOpt, datacond.value);
                        if (datacond.strOpt.Trim().ToLower() == "like")
                            strRet = string.Format(" {0} {1} {2} '%{3}%'", datacond.Logic.ToString(), strFmtResult, datacond.strOpt, datacond.value);
                        if (datacond.strOpt.Trim().ToLower() == "in")
                        {
                            string[] stritems = datacond.value.Split(',');
                            string strAll = string.Join("',", stritems);
                            strRet = string.Format(" {0} {1} {2} '3'", datacond.Logic.ToString(), strFmtResult, datacond.strOpt, strAll);
                        }
                        break;
                    }
            }
            return strRet;
        }
    }

    public class TableDeleteSqlProcess : TableSqlProcess
    {
        public override string GenerateSql(string Table, DataPoint key, string keyval, DataCondition condition)
        {
            if (datas.Count == 0) return null;
            StringBuilder sb = new StringBuilder();
            int updatecnt = 0;
            DataColumn keycol = null;
            if (condition != null && condition.Datapoint != null)
            {
                keycol = DataAccessCenter.DataColumnMappings[condition.Datapoint.Name];
            }
            DataColumn dcl = MappingConvert.DataPointToColumn(key);
            if (dcl == null)
            {
                return null;
            }
            if (dcl.Table != Table)
            {

                DataTableRef dtr = MappingConvert.GetTableRef(Table, dcl.Table);//��ù�ϵ
                if (dtr == null)
                {
                    dtr = MappingConvert.GetTableRef(dcl.Table, Table);
                }
                if (dtr == null)
                    return "";
                string strWhere = "From " + dtr.Table;
                while (dtr != null)
                {

                    strWhere += string.Format(" INNER JOIN {0} ON {1}.{2} = {0}.{3}", dtr.RefTable, dtr.Table, dtr.Column.Column, dtr.RefColumn.Column);
                    dtr = dtr.Next;
                }

                return string.Format("Delete  {0}  {1} WHERE 1=1 {2} ", Table,  strWhere, TableSqlProcess.GetColumnWhere(condition, keycol));
            }
            else
            {
                return string.Format("Delete {0} WHERE 1=1 {1} ", Table, TableSqlProcess.GetColumnWhere(condition, keycol));
            }
        }
    }

    public class TableUpdateSqlProcess:TableSqlProcess 
    {
        public override string GenerateSql(string Table, DataPoint key, string keyval,DataCondition condition)
        {

            if (datas.Count == 0) return null;
            StringBuilder sb = new StringBuilder();
            int updatecnt = 0;
            DataColumn keycol = null;
            if(condition != null && condition.Datapoint!=null)
            {
                keycol= DataAccessCenter.DataColumnMappings[condition.Datapoint.Name];
            }
            #region for
            for (int i = 0; i < datas.Count; i++)
            {
                if (!datas[i].Validate || datas[i].datapoint.Name == key.Name)//�����������������
                {
                    continue;
                }
                DataColumn dc = MappingConvert.DataPointToColumn(datas[i].datapoint);
                if (dc == null)
                {
                    throw new Exception(string.Format("���ݵ�{0}δ����!",datas[i].datapoint.Name));
                    return null;
                }
                string val = "Null";
                switch (dc.DataType)
                {
                    case "int":
                    case "money":
                    case "float":
                        {
                            if (datas[i].value.Trim().Length > 0)
                            {
                                float ft = 0;
                                if (!float.TryParse(datas[i].value, out ft))
                                {
                                    try
                                    {
                                        MathHandle mh = new MathHandle(datas[i].value);
                                        string mhval = mh.Handle();
                                        if (!float.TryParse(mhval, out ft))
                                        {
                                            throw new Exception("�޷����������ַ������������ֶ��У�");
                                        }
                                        val = mhval;
                                        break;
                                    }
                                    catch
                                    {
                                    }
                                    
                                    //return null;
                                }

                                val = datas[i].value;
                            }
                            break;
                        }
                    case "date":
                    case "smalldatetime":
                    case "time":
                        {
                            if (datas[i].value.Trim().Length > 0)
                            {
                                val = string.Format("cast('{0}' as datetime)", datas[i].value);
                            }
                            break;
                        }
                    case "varchar":
                    case "nvarchar":
                    case "text":
                    default:
                        {
                            val = string.Format("'{0}'", datas[i].value.Replace("'", "''"));
                            break;
                        }

                }
                if (datas[i].datapoint.Name == condition.Datapoint.Name) continue;
                sb.AppendFormat("{3}.{0} = {1} {2}", MappingConvert.DataPointToColumn(datas[i].datapoint).Column, val, i < datas.Count - 1 ? "," : "", Table);
                updatecnt++;
            }
            #endregion
            if (updatecnt == 0) return "";
            string strcols = sb.ToString();
            if (strcols.EndsWith(","))//����Զ��Ž�����ɾ������
            {
                strcols = strcols.Substring(0, strcols.Length - 1);
            }
            //DataColumn dc = MappingConvert.DataPointToColumn(key);
            DataColumn dcl = MappingConvert.DataPointToColumn(key);
            if (dcl == null)
            {
                return null;
            }
            if (dcl.Table != Table)
            {
                
                DataTableRef dtr = MappingConvert.GetTableRef(Table, dcl.Table);//��ù�ϵ
                if (dtr == null)
                {
                    dtr = MappingConvert.GetTableRef(dcl.Table, Table);
                }
                if (dtr == null) 
                    return "";
                string strWhere = "From " + dtr.Table;
                while (dtr != null)
                {
                    
                    strWhere += string.Format(" INNER JOIN {0} ON {1}.{2} = {0}.{3}", dtr.RefTable,dtr.Table,dtr.Column.Column, dtr.RefColumn.Column);
                    dtr = dtr.Next;
                }
                
                return string.Format("UPDATE  {0} SET {1} {2} WHERE 1=1 {3} ", Table, strcols, strWhere, TableSqlProcess.GetColumnWhere(condition,keycol));
            }
            else
            {
                return string.Format("UPDATE {0} SET {1} WHERE 1=1 {2} ", Table, strcols, TableSqlProcess.GetColumnWhere(condition, keycol));
            }
        }

    }

    public class TableAddSqlProcess : TableSqlProcess
    {
        public override string GenerateSql(string Table, DataPoint key, string keyval,DataCondition condition)
        {
            if (datas.Count == 0) return null;
            StringBuilder sbcol = new StringBuilder();
            StringBuilder sbval = new StringBuilder();
            int addcnt = 0;
            DataColumn keycol = null;
            if (condition != null && condition.Datapoint != null)
            {
                keycol = DataAccessCenter.DataColumnMappings[condition.Datapoint.Name];
            }
            
            for (int i = 0; i < datas.Count; i++)
            {
                DataColumn dc = MappingConvert.DataPointToColumn(datas[i].datapoint);
                //if (!datas[i].Validate || (datas[i].datapoint.Name == key.Name && dc.IsIden ) ) 
                if (!datas[i].Validate || dc.IsIden )
                    continue; //�ؼ���������������в�����
                if ((datas[i].value == null || datas[i].value.Trim().Length == 0) && dc.IsDefault)
                {
                    continue;//Ĭ��ֵ���ֵΪ�ղ�����
                }
                sbcol.AppendFormat("{0}{1}", dc.Column,i < datas.Count - 1 ? "," : "");
                string val = "Null";
                switch (dc.DataType)
                {
                    case "int":
                    case "money":
                    case "float":
                        {
                            if (datas[i].value.Trim().Length > 0)
                            {
                                try
                                {
                                    float ft = 0;
                                    MathHandle mh = new MathHandle(datas[i].value);
                                    string sval = mh.Handle();
                                    if (!float.TryParse(sval, out ft))
                                    {
                                        throw new Exception("�޷����������ַ������������ֶ��У�");
                                        return null;
                                    }
                                }
                                catch
                                {
                                    //throw new Exception("�������֧�ָù��ܣ�");
                                }
                                val = datas[i].value;
                            }
                            break;
                        }
                    case "date":
                    case "smalldatetime":
                    case "datetime":
                    case "time":
                        {
                            if (datas[i].value.Trim().Length > 0)
                            {
                                string realval = datas[i].value;
                                if (realval.IndexOf("T") > 0)
                                {
                                    realval = realval.Substring(0, realval.IndexOf("T"));
                                    //DateTime.TryParseExact 
                                    if (realval.Length == 8)
                                    {
                                        realval = realval.Insert(4, "-");
                                        realval = realval.Insert(7, "-");
                                    }
                                }
                                DateTime dt;
                                bool isdate = DateTime.TryParse(realval,out dt);
                                if (!isdate || dt.CompareTo(new DateTime(1900,1,2)) < 1)
                                {
                                    val = "null";
                                }
                                else
                                {
                                    val = string.Format("cast('{0}' as datetime)", dt.ToShortDateString());
                                }
                            }
                            break;
                        }
                    case "varchar":
                    case "nvarchar":
                    case "text":
                    default :
                        {
                            val = string.Format("'{0}'", datas[i].value.Replace("'", "''"));
                            break;
                        }

                }
                sbval.AppendFormat("{0} {1}", val, i < datas.Count - 1 ? "," : "");
           
                addcnt++;
                //sb.AppendFormat("{3}.{0} = '{1}'{2}", MappingConvert.DataPointToColumn(datas[i].datapoint).Column, datas[i].value, i < datas.Count - 1 ? "," : "", Table);
            }
            if (addcnt == 0) return "";
            string strcol = sbcol.ToString();
            string strval = sbval.ToString();
            if (strcol.EndsWith(","))//����Զ��Ž�����ɾ������
            {
                strcol = strcol.Substring(0, strcol.Length-1);
            }
            if (strval.EndsWith(","))//����Զ��Ž�����ɾ������
            {
                strval = strval.Substring(0, strval.Length-1);
            }
            return string.Format("INSERT INTO {0}({1}) VALUES({2})", Table, strcol, strval);
        }
    }


}
