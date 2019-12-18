using System;
using System.Collections.Generic;
using System.Data;
using System.Xml;
using XmlProcess;
using WolfInv.Com.AccessDataBase;
using WolfInv.Com.MetaDataCenter;
using System.IO;
namespace WolfInv.Com.DataCenter
{
    /// <summary>
    /// 服务端数据中心,服务端唯一的公开的类
    /// </summary>
    public  class DataAccessCenter
    {
        public static Dictionary<string, DataColumn> DataColumnMappings;
        public static Dictionary<string, List<DataColumn>> DataTabelMappings;
        public static Dictionary<string, DataPoint> DataPointMappings;
        public static Dictionary<string, DataTableReference> DataTableRefs;//表映射关系
        public static Dictionary<string, DataTableReference> DataTableReRefs;//逆向表映射关系
        public static Dictionary<string, DataIdenTable> DataTables;
        public static Dictionary<string, List<string>> SimilarColumns;
        public static string strAppPath;
        public static string strLogPath;
        public static string strConn;
        public static LogClass lgc;
        public static DBAccessClass db;
        Dictionary<string, ConnectObject> Connects;
        public DataAccessCenter(string Apppath, Dictionary<string, ConnectObject> conns, XmlDocument mapxml, XmlDocument colxml)
        {
            Connects = conns;
            strAppPath = Apppath;
            strLogPath = Apppath + "\\log";
            lgc = new LogClass(strLogPath);
            try
            {
                ////if (Directory.Exists(strLogPath))
                ////{
                ////    Directory.Delete(strLogPath, true);
                    
                ////}
                Directory.CreateDirectory(strLogPath);
            }
            catch(Exception ce)
            {
            }
            //strConn = strconn;
            string connmsg = null;
            foreach (string strid in Connects.Keys)
            {
                
                try
                {
                    db = new DBAccessClass(Connects[strid].ConnectString);

                }
                catch(Exception ce)
                {
                    connmsg = ce.Message; 
                    continue;
                }
                strConn = Connects[strid].ConnectString;
                connmsg = null;
                break;
                
            }
            if (db == null)
            {
                throw new Exception(connmsg);
            }
            InitMappings(mapxml,colxml);
        }

        void InitMappings(XmlDocument mapxml,XmlDocument colxlm)
        {
            if (mapxml == null || colxlm == null)
            {
                throw new Exception("miss the xml files!");
                return;
            }
            if (DataPointMappings == null)
            {
                //string strDpmPath = string.Format("{0}\\xml\\dataidmapping.xml", strAppPath);
                XmlDocument xmldoc = new XmlDocument();
                try
                {
                    xmldoc.LoadXml(mapxml.OuterXml);
                }
                catch
                {
                    throw new Exception(string.Format("文件{0}不存在！","dataidmapping"));
                }
                DataPointMappings = DataPoint.InitMapping(xmldoc);
            }
            //get the data column mappings
            if ( DataColumnMappings  == null)
            {
                //string strDpmPath = string.Format("{0}\\xml\\sdataidmapping.xml", strAppPath);
                XmlDocument xmldoc = new XmlDocument();
                try
                {
                    xmldoc.LoadXml(colxlm.OuterXml);
                }
                catch (Exception ce)
                {
                    throw new Exception("can't init the server DataIdMapping File!");
                }
                XmlNodeList nodes = xmldoc.SelectNodes("/flds/f");
                DataColumnMappings = new Dictionary<string, DataColumn>();
                DataTabelMappings = new Dictionary<string, List<DataColumn>>();
                foreach (XmlNode node in nodes)
                {
                    DataColumn dp = new DataColumn();
                    dp.LoadXml(node);
                    if (!DataColumnMappings.ContainsKey(dp.DataPoint))
                    {
                        DataColumnMappings.Add(dp.DataPoint, dp);
                    }
                    List<DataColumn> dcs = new List<DataColumn>();
                    if (DataTabelMappings.ContainsKey(dp.Table))
                    {
                        dcs = DataTabelMappings[dp.Table];
                    }
                    else
                    {
                        DataTabelMappings.Add(dp.Table, dcs);
                    }
                    dcs.Add(dp);
                }
                //映射关系
                DataTableRefs = new Dictionary<string, DataTableReference>();
                DataTableReRefs = new Dictionary<string, DataTableReference>();
                foreach (string strDp in DataColumnMappings.Keys)
                {
                    DataColumn dc = DataColumnMappings[strDp];
                    if (dc.RefDataPoint != null && dc.RefDataPoint.Trim().Length > 0)
                    {
                        
                        if (DataColumnMappings.ContainsKey(dc.RefDataPoint))
                        {
                            
                            dc.RefColumn = DataColumnMappings[dc.RefDataPoint];
                            #region 映射关系
                            DataTableReference tableref = new DataTableReference();
                            if (DataTableRefs.ContainsKey(dc.Table))
                            {
                                tableref = DataTableRefs[dc.Table];
                            }
                            else
                            {
                                tableref.TableName = dc.Table;
                                DataTableRefs.Add(tableref.TableName,tableref);
                            }
                            if (tableref.References == null)
                            {
                                tableref.References = new List<DataColumnReference>();
                            }
                            DataColumnReference dcr = new DataColumnReference();
                            dcr.MainColumn = dc.RefColumn;
                            dcr.LientColumn = dc;
                            tableref.References.Add(dcr);
                            #endregion

                            #region 逆向映射关系
                            DataTableReference tablereref = new DataTableReference();
                            if (DataTableReRefs.ContainsKey(dc.RefColumn.Table))
                            {
                                tablereref = DataTableReRefs[dc.RefColumn.Table];
                            }
                            else
                            {
                                tablereref.TableName = dc.RefColumn.Table;
                                DataTableReRefs.Add(tablereref.TableName, tablereref);
                            }
                            if (tablereref.References == null)
                            {
                                tablereref.References = new List<DataColumnReference>();
                            }
                            DataColumnReference redcr = new DataColumnReference();
                            redcr.LientColumn = dc.RefColumn;
                            redcr.MainColumn = dc;
                            tablereref.References.Add(redcr);
                            #endregion
                        }
                        

                        
                    }
                }
                SimilarColumns = new Dictionary<string, List<string>>();
                foreach(string tab in DataTableReRefs.Keys)
                {
                    DataTableReference dtr = DataTableReRefs[tab];
                    //List<string> similarcol = new List<string>();
                    for (int i = 0; i < dtr.References.Count; i++)
                    {

                        DataColumnReference dcr= dtr.References[i];
                        List<string> similarcol = new List<string>();
                        if(SimilarColumns.ContainsKey(dcr.MainColumn.DataPoint))
                        {
                            similarcol = SimilarColumns[dcr.MainColumn.DataPoint];
                        }
                        else
                        {
                            SimilarColumns.Add(dcr.MainColumn.DataPoint, similarcol);
                        }
                        if (SimilarColumns.ContainsKey(dcr.LientColumn.DataPoint))
                        {
                            List<string> lists = SimilarColumns[dcr.LientColumn.DataPoint];
                            for (int l = 0; l < lists.Count; l++)
                            {
                                if (!similarcol.Contains(lists[l]))
                                {
                                    similarcol.Add(lists[l]);
                                }
                            }
                            SimilarColumns[dcr.LientColumn.DataPoint] = similarcol;
                        }
                        else
                        {
                            SimilarColumns.Add(dcr.LientColumn.DataPoint, similarcol);
                        }
                        if (!similarcol.Contains(dcr.LientColumn.DataPoint))
                        {
                            similarcol.Add(dcr.LientColumn.DataPoint);
                        }
                        if (!similarcol.Contains(dcr.MainColumn.DataPoint))
                        {
                            similarcol.Add(dcr.MainColumn.DataPoint);
                        }
                        
                    }
                }
                DataTables = new Dictionary<string, DataIdenTable>();
                foreach (string table in DataTabelMappings.Keys)
                {
                    DataIdenTable dit = new DataIdenTable();
                    dit.TableName = table;
                    dit.Columns = DataTabelMappings[table];
                    dit.IsView = table.StartsWith("vw_");
                    for (int i = 0; i < dit.Columns.Count; i++)
                    {
                        DataColumn dc = dit.Columns[i];
                        if (dc.IsIden)
                        {
                            dit.IdenColumn = dc.DataPoint;
                        }
                        if (dc.IsKey)
                        {
                            dit.Key = dc.DataPoint;
                        }
                        if (dit.ContainsKey(dc.DataPoint))
                        {
                            continue;
                        }
                        dit.Add(dc.DataPoint, dc);
                        dc.OwnTable = dit;
                        
                    }
                    DataTables.Add(table,dit);
                }
            }
        }

        public static DataSet GetDataList(XmlNode req,out string msg,bool debug=false)
        {
            msg = null;
            lgc.SetLogFileName("getDataList.txt");
            //DataTableRef dtrf = MappingConvert.GetTableRef("FSMS_Produces", "FSMS_TradeMarks");
            SqlBuilder sb = new SqlBuilder(new DataRequest(req));
            string strSql = sb.GenerateQuerySql();
            string datasource = XmlUtil.GetSubNodeText(req, "/ds/@id");
            lgc.DWrite("\rDataSource:{0}\rExec Sql:{1}\rRequest Xml:{2}", datasource, strSql, req.OuterXml);
            try
            {
                db.Connect();
            }
            catch (Exception e)
            {
                msg = e.Message + (debug? (":" + strSql):"");
                return null;
            }
            DataSet ds = null;
            db.SetDebug(debug);
            msg =db.GetResult(strSql,ref ds);// DBAccessClass.GetDataSet(strSql, ref msg);
            if (msg != null)
            {
                lgc.DWrite("\rError:{0},{1}", msg,strSql);
                return null;
            }
            return ds;
            
        }

        public static string UpdateDataList(XmlNode req)
        {

            string MainTableName = null;
            DataRequest datareq = new DataRequest(req);
            SqlBuilder sb = new SqlBuilder(datareq);
            //DataRequestType reqtype = DataRequestType.Update;
            UpdateData Udata = datareq.updatedata;
            XmlNode data = req.SelectSingleNode("./Data");
            string type = XmlUtil.GetSubNodeText(data,"@type");

            //support import data zhouys 3/28 2012
            if (XmlUtil.GetSubNodeText(req, "@type") == "Import")
            {
                string msg = null;
                DataSet ds = datareq.updatedata.Dataset;
                if (ds == null)
                {
                    return "导入内容为空!";
                }
                try
                {
                    db.Connect();
                    db.Tran();
                    msg = db.FillDataSet(ds);
                }
                catch
                {
                    msg = "导入错误!";
                    db.RollBack();
                }
                finally
                {
                    db.Commit();
                }
                return msg;
            }

            //datareq.RequestType = reqtype;
            //分开为主从表数据分别生成ｓｑｌ
            UpdateData MainData = new UpdateData();
            UpdateData OtherData = new UpdateData();
            DataColumn keycol = MappingConvert.DataPointToColumn(Udata.keydpt);
            Dictionary<string, RequestItem> AllReqs = new Dictionary<string, RequestItem>();
            foreach (RequestItem ri in datareq.RequestItems)
            {
                if (!AllReqs.ContainsKey(ri.DataPt.Name))
                {
                    AllReqs.Add(ri.DataPt.Name, ri);
                }
            }
            foreach(string dpt in Udata.Items.Keys)
            {
                if (keycol!=null && keycol.OwnTable.ContainsKey(dpt))
                {
                    MainData.Items.Add(dpt, Udata.Items[dpt]);
                }
                else
                {
                    if (!Udata.Items[dpt].Validate) 
                        continue;
                    if (AllReqs.ContainsKey(dpt) && AllReqs[dpt].ReadOnly)
                    {
                        continue;
                    }
                    OtherData.Items.Add(dpt, Udata.Items[dpt]);
                }
            }
            MainData.keydpt = Udata.keydpt;
            MainData.keyvalue = Udata.keyvalue;
            OtherData.keydpt = Udata.keydpt;
            OtherData.keyvalue = Udata.keyvalue;
            try
            {
                db.Connect();
                db.Tran();
            }
            catch (Exception e)
            {
                db.Close();
                return e.Message;
            }
            List<string> mainsql = sb.GenerateUpdateSql(MainData,datareq.RequestType,datareq.ConditonGroup);
            if (mainsql.Count > 0 && mainsql[0].Length > 0)
            {
                string msg = db.ExecSql(mainsql[0]);
                if (msg != null)
                {
                    db.RollBack();
                    db.Close(true);
                    return msg;
                }
            }

            if ((Udata.SubItems == null || Udata.SubItems.Count == 0) &&  OtherData.Items.Count ==0)
            {
                db.Commit();
                db.Close();
                return null;
            }

            if (Udata.Items.Count>0 &&  (Udata.keyvalue == null || Udata.keyvalue == "" || datareq.RequestType == DataRequestType.Add))//新增,前提是items长度不为0
            {
                //获得主表的标识值
                DataColumn dc = MappingConvert.DataPointToColumn(Udata.keydpt);
                DataIdenTable dit = dc.OwnTable;
                string indsql = "select  {1} as {2} from {0} where {3} =  ident_current('{0}') ";
                string mtable = MappingConvert.DataPointToColumn(Udata.keydpt).Table;//主表
                DataSet ds = null;
                if (dit.IdenColumn != null)
                {
                    /* 无需判断，明显降低速度*/
                    DataColumn keypt = MappingConvert.DataPointToColumn(new DataPoint(dit.IdenColumn));
                    string keysql = string.Format(indsql, mtable, dc.Column, dc.DataPoint, keypt.Column);
                    string msg = db.GetResult(keysql, ref ds);
                    if (msg != null)
                    {
                        db.RollBack();
                        db.Close(true);
                        return msg;
                    }
                    
                }
                if (ds == null)
                {
                    indsql = "select ident_current('{0}')";
                    string msg = db.GetResult(string.Format(indsql, mtable),ref ds);
                    if (msg == null)
                    {
                        db.RollBack();
                        db.Close();
                        return msg;
                    }
                    if (ds == null)
                    {
                        db.RollBack();
                        db.Close();
                        return string.Format("保存表{0}失败!", mtable);
                    }
                }
                Udata.keyvalue = ds.Tables[0].Rows[0][0].ToString();//标识值
                OtherData.keyvalue = ds.Tables[0].Rows[0][0].ToString();//标识值
            }
            
            
            
            List<string> strSqls = sb.GenerateUpdateSql(OtherData, datareq.RequestType,datareq.ConditonGroup);
            //执行从表sql
            for (int i = 0; i < strSqls.Count; i++)
            {
                 string msg = null;// DBAccessClass.ExecSQL(strSqls[i]);
                    msg =db.ExecSql(strSqls[i]);
                    if (msg != null)
                    {
                        db.RollBack();
                        db.Close();
                        return msg;
                    }
                
            }
            //处理明细表
            if (Udata.SubItems == null || Udata.SubItems.Count == 0 )
            {
                db.Commit();
                db.Close();
                return null;
            }
            Dictionary<string, RequestItem> subdic = new Dictionary<string, RequestItem>();
            if (datareq.SubRequestItem == null)
            {
                return "无法找到子表数据源配置！";
            }
            for (int i = 0; i< datareq.SubRequestItem.Count; i++)
            {
                RequestItem ri = datareq.SubRequestItem[i];
                if (ri.ReadOnly)
                {
                    continue;
                }
                DataColumn dc = MappingConvert.DataPointToColumn(ri.DataPt);
                if (dc.OwnTable.IsView)//视图列，不需要更新
                {
                    continue;
                }
                if (!subdic.ContainsKey(ri.DataPt.Name))
                {
                    subdic.Add(ri.DataPt.Name, ri);
                }
            }
            for (int i = 0; i < datareq.updatedata.SubItems.Count; i++)
            {

                //找到子数据中相应的列
                
                UpdateData subdata = datareq.updatedata.SubItems[i];
                //过滤只读列,请求模版中没有的项，包括只读项将过滤掉不更新，具体执行在后面处理
                if ((subdata.keyvalue == null || subdata.keyvalue.Trim().Length == 0) && subdata.keydpt.Name == datareq.updatedata.keydpt.Name)
                {
                    subdata.keyvalue = datareq.updatedata.keyvalue;
                }
                foreach (string datakey in subdata.Items.Keys)
                {
                    if(!subdic.ContainsKey(datakey))
                        subdata.Items[datakey].Validate = false;
                }
                string subtable = MappingConvert.DataPointToColumn(subdata.keydpt).Table;//子数据表 主要表
                string ret = null;
                if (Udata.keydpt.Name != subdata.keydpt.Name)
                {
                    ret = SqlBuilder.AddRefKeyItemInData(ref subdata, subtable, Udata.keydpt.Name, Udata.keyvalue, false);
                    if (ret != null)
                    {
                        db.RollBack();
                        return ret;
                    }
                }
                DataCondition dc = new DataCondition();
                
                dc.LoadXml(datareq.ConditonGroup.ToXml(null));
                if (datareq.RequestType != DataRequestType.BatchUpdate)
                {
                    if (subdata.keyvalue != null && subdata.keyvalue.Trim().Length > 0)//子节点优先用子节点作为关键字
                    {
                        if (dc.value != null && dc.value.Trim().Length > 0)
                        {
                            dc.value = subdata.keyvalue;
                            dc.Datapoint = subdata.keydpt;
                            dc.strOpt = "=";
                        }
                    }
                }
                strSqls = sb.GenerateUpdateSql(subdata,subdata.ReqType,dc);
                if (strSqls == null)
                {
                    db.RollBack();
                    return ret;
                }
                for (int s = 0; s < strSqls.Count; s++)
                {
                    if (strSqls[s] == null || strSqls[s].Trim().Length == 0) continue;
                    string msg = null;// DBAccessClass.ExecSQL(strSqls[s]);
                    msg = db.ExecSql(strSqls[s]);
                    if (msg != null)
                    {
                        db.RollBack();
                        db.Close();
                        return msg;
                    }
                    //db.ExecuteDataSet(strSqls[s]);
                }
            }
            db.Commit();
            db.Close();
            return null;
        }

        public static XmlNode GetViewSource(XmlNode req)
        {
            DataRequest dr = new DataRequest(req);
            ViewDataPointProcess vdpp = new ViewDataPointProcess(dr);
            return vdpp.GetDataPoint();
        }

        ////public static void RunTool()
        ////{
        ////    ToolMain frm = new ToolMain();
        ////    frm.Show();
        ////}

        public static string DisConnect()
        {
            db.Close(true);
            return null;
        }
    }
}
