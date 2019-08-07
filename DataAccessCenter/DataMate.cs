using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml;
using XmlProcess;
using AccessDataBase;
using WolfInv.Com.MetaDataCenter;
using System.Collections;
using System.IO;
namespace WolfInv.Com.DataCenter
{
    /// <summary>
    /// �������������,�����Ψһ�Ĺ�������
    /// </summary>
    public  class DataAccessCenter
    {
        public static Dictionary<string, DataColumn> DataColumnMappings;
        public static Dictionary<string, List<DataColumn>> DataTabelMappings;
        public static Dictionary<string, DataPoint> DataPointMappings;
        public static Dictionary<string, DataTableReference> DataTableRefs;//��ӳ���ϵ
        public static Dictionary<string, DataTableReference> DataTableReRefs;//�����ӳ���ϵ
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
                    throw new Exception(string.Format("�ļ�{0}�����ڣ�","dataidmapping"));
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
                //ӳ���ϵ
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
                            #region ӳ���ϵ
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

                            #region ����ӳ���ϵ
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

        public static DataSet GetDataList(XmlNode req,out string msg)
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
                msg = e.Message;
                return null;
            }
            DataSet ds = null;
            msg =db.GetResult(strSql,ref ds);// DBAccessClass.GetDataSet(strSql, ref msg);
            if (msg != null)
            {
                lgc.DWrite("\rError:{0}", msg);
                return null;
            }
            return ds;
            
        }

        public static string UpdateDataList(XmlNode req)
        {
            

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
                    return "��������Ϊ��!";
                }
                try
                {
                    db.Connect();
                    db.Tran();
                    msg = db.FillDataSet(ds);
                }
                catch
                {
                    msg = "�������!";
                    db.RollBack();
                }
                finally
                {
                    db.Commit();
                }
                return msg;
            }

            //datareq.RequestType = reqtype;
            //�ֿ�Ϊ���ӱ����ݷֱ����ɣ���
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

            if (Udata.keyvalue == null || Udata.keyvalue == "" || datareq.RequestType == DataRequestType.Add)//����
            {
                //�������ı�ʶֵ
                DataColumn dc = MappingConvert.DataPointToColumn(Udata.keydpt);
                DataIdenTable dit = dc.OwnTable;
                string indsql = "select  {1} as {2} from {0} where {3} =  ident_current('{0}') ";
                string mtable = MappingConvert.DataPointToColumn(Udata.keydpt).Table;//����
                DataSet ds = null;
                if (dit.IdenColumn != null)
                {
                    DataColumn keypt = MappingConvert.DataPointToColumn(new DataPoint(dit.IdenColumn));
                    string msg = db.GetResult(string.Format(indsql, mtable, dc.Column, dc.DataPoint, keypt.Column),ref ds);
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
                        return string.Format("�����{0}ʧ��!", mtable);
                    }
                }
                Udata.keyvalue = ds.Tables[0].Rows[0][0].ToString();//��ʶֵ
                OtherData.keyvalue = ds.Tables[0].Rows[0][0].ToString();//��ʶֵ
            }
            
            
            
            List<string> strSqls = sb.GenerateUpdateSql(OtherData, datareq.RequestType,datareq.ConditonGroup);
            //ִ�дӱ�sql
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
            //������ϸ��
            if (Udata.SubItems == null || Udata.SubItems.Count == 0 )
            {
                db.Commit();
                db.Close();
                return null;
            }
            Dictionary<string, RequestItem> subdic = new Dictionary<string, RequestItem>();
            if (datareq.SubRequestItem == null)
            {
                return "�޷��ҵ��ӱ�����Դ���ã�";
            }
            for (int i = 0; i< datareq.SubRequestItem.Count; i++)
            {
                RequestItem ri = datareq.SubRequestItem[i];
                if (ri.ReadOnly)
                {
                    continue;
                }
                DataColumn dc = MappingConvert.DataPointToColumn(ri.DataPt);
                if (dc.OwnTable.IsView)//��ͼ�У�����Ҫ����
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

                //�ҵ�����������Ӧ����
                
                UpdateData subdata = datareq.updatedata.SubItems[i];
                //����ֻ����,����ģ����û�е������ֻ������˵������£�����ִ���ں��洦��
                if ((subdata.keyvalue == null || subdata.keyvalue.Trim().Length == 0) && subdata.keydpt.Name == datareq.updatedata.keydpt.Name)
                {
                    subdata.keyvalue = datareq.updatedata.keyvalue;
                }
                foreach (string datakey in subdata.Items.Keys)
                {
                    if(!subdic.ContainsKey(datakey))
                        subdata.Items[datakey].Validate = false;
                }
                string subtable = MappingConvert.DataPointToColumn(subdata.keydpt).Table;//�����ݱ� ��Ҫ��
                string ret = SqlBuilder.AddRefKeyItemInData(ref subdata, subtable,Udata.keydpt.Name,Udata.keyvalue,false);
                if (ret != null)
                {
                    db.RollBack();
                    return ret;
                }
                DataCondition dc = new DataCondition();
                
                dc.LoadXml(datareq.ConditonGroup.ToXml(null));
                if (datareq.RequestType != DataRequestType.BatchUpdate)
                {
                    if (subdata.keyvalue != null && subdata.keyvalue.Trim().Length > 0)//�ӽڵ��������ӽڵ���Ϊ�ؼ���
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

        public static void RunTool()
        {
            ToolMain frm = new ToolMain();
            frm.Show();
        }

        public static string DisConnect()
        {
            db.Close(true);
            return null;
        }
    }

    public class LogClass
    {
        string LogPath;
        string FileName;
        public bool Debug;
        public void SetLogFileName(string filename)
        {
            FileName = filename;
        }
        public LogClass(string logpath)
        {
            LogPath = logpath;
        }
        public void DWrite(string fmt, params string[] parm)
        {
            Write(FileName, fmt, parm);
        }

        public void Write(string filepath,string fmt, params string[] parm)
        {
            if (!Debug) return;
            string logfile = string.Format("{0}\\{1}",LogPath,filepath);
            if (!File.Exists(logfile))
            {
                File.CreateText(logfile);
            }
            try
            {
                StreamWriter sw = File.AppendText(logfile);
                sw.WriteLine(string.Format(fmt,parm));
                sw.Close();
            }
            catch (Exception e)
            {
                return;
            }
        }
    }

    public class DataTableReference
    {
        public string TableName;
        public List<DataColumnReference> References;
        Dictionary<string, DataColumnReference> m_maps;
        public Dictionary<string, DataColumnReference> GetMaps()
        {
            if (References == null) return null;
                if (m_maps == null)
                {
                    m_maps = new Dictionary<string, DataColumnReference>();
                    for (int i = 0; i < References.Count; i++)
                    {
                        DataColumnReference dcr = References[i];
                        string key = null;
                        key = dcr.MainColumn.DataPoint;
                        
                        DataColumn dc = DataCenter.DataAccessCenter.DataColumnMappings[key];
                        if (!m_maps.ContainsKey(dc.Table))
                        {
                            m_maps.Add(dc.Table, dcr);
                        }
                    }
                }
                return m_maps;
           
        }
    }

    //ref column
    public class DataColumnReference
    {
        public DataColumn  MainColumn;
        public DataColumn  LientColumn;
    }

    public class DataColumn
    {
        public string DataPoint;
        public string Table;
        public string Column;
        public string Index;
        public string DataType;
        public int Length;
        public string DataBase;
        public string RefDataPoint;
        public bool IsKey;
        public bool IsIden;
        public DataColumn RefColumn;
        public DataIdenTable OwnTable;
        public bool IsDefault;
        public void LoadXml(XmlNode node)
        {
            DataPoint = XmlUtil.GetSubNodeText(node, "@id");
            Column = XmlUtil.GetSubNodeText(node, "@col");
            Table = XmlUtil.GetSubNodeText(node, "@tab");
            int.TryParse(XmlUtil.GetSubNodeText(node, "@len"),out Length);
            DataType = XmlUtil.GetSubNodeText(node, "@type");
            RefDataPoint = XmlUtil.GetSubNodeText(node, "@ref");
            IsKey = XmlUtil.GetSubNodeText(node, "@key") == "1";
            IsIden = XmlUtil.GetSubNodeText(node, "@iden") == "1";
            IsDefault  = XmlUtil.GetSubNodeText(node, "@default") == "1";
        }

        public XmlNode ToXml(XmlNode parent)
        {
            XmlDocument xmldoc;
            if (parent == null)
            {
                xmldoc = new XmlDocument();
                xmldoc.LoadXml("<data/>");
                parent = xmldoc.SelectSingleNode("data");
            }
            XmlNode node = XmlUtil.AddSubNode(parent, "f");
            XmlUtil.AddAttribute(node, "id", this.DataPoint);
            XmlUtil.AddAttribute(node,"col",this.Column);
            XmlUtil.AddAttribute(node, "tab", this.Table);
            XmlUtil.AddAttribute(node, "type", this.DataType);
            XmlUtil.AddAttribute(node, "len", this.Length.ToString());
            if(this.RefDataPoint != null && this.RefDataPoint.Trim().Length > 0) 
                XmlUtil.AddAttribute(node, "ref", this.RefDataPoint);
            if (this.IsKey)
                XmlUtil.AddAttribute(node, "key", "1");
            if (this.IsIden)
                XmlUtil.AddAttribute(node, "iden", "1");
            if (this.IsDefault)
                XmlUtil.AddAttribute(node, "default", "1");
            return node;
        }
    }

    public class DataIdenTable:Dictionary<string,DataColumn>
    {
        public List<DataColumn> Columns = new List<DataColumn>();
        public string TableName;
        public bool IsView;
        public string Key;
        public string IdenColumn;
        public string AName;
        

    }
}
