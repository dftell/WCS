using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;
namespace WolfInv.Com.AccessDataBase
{
    public class DBAccessClass:IDisposable 
    {

        protected OleDbConnection conn;
        protected string ConnString;
        protected OleDbDataAdapter adp;
        protected OleDbCommand comm;
        protected OleDbTransaction tran;
        static DBAccessClass()
        {
            //conn = new OleDbConnection();
        }

        public DBAccessClass(string connstr)
        {
            ConnString = connstr;
            //Connect();
       
        }
        public void Connect()
        {
            Connect(ConnString);
        }
        public void Connect(string str)
        {
            if (conn == null)
                conn = new OleDbConnection(str);
            if (conn.ConnectionString == null || conn.ConnectionString.Trim().Length == 0)
                conn.ConnectionString = str;
            if (conn.State != ConnectionState.Open)
                conn.Open();
            if (comm == null)
                comm = new OleDbCommand();
            comm.Connection = conn;
        }
        public void Tran()
        {
            Connect();
            if (tran != null)
            {
                tran = null;
            }
            tran = conn.BeginTransaction();
            comm.Transaction = tran;
        }

        public void RollBack()
        {
            if (conn == null)
                return;
            if (conn.State != ConnectionState.Open)
                return;
            tran.Rollback();
        }

        public void Commit()
        {
            if (conn == null)
                return;
            if (conn.State != ConnectionState.Open)
                return;
            tran.Commit();
        }
        public void Close()
        {
            Close(true);
        }
        public void Close(bool CloseConn)
        {
            if (conn == null) return;
            if (conn.State != ConnectionState.Open) return;
            if (CloseConn)
            {
                conn.Close();
                comm = null;
                if (tran != null)
                {
                    
                    tran = null;
                }
                conn = null;
            }
        }

        public string GetResult(string sql,ref DataSet ds)
        {
            string ret = null;
            ds = new DataSet();
            try
            {
                Connect();
                comm.CommandText = sql;
                comm.CommandType = CommandType.Text;

                adp = new OleDbDataAdapter(comm);
                comm.ExecuteNonQuery();
                adp.Fill(ds,"DataTable");
                adp = null;
            }
            catch (Exception ce)
            {
                ret = ce.Message;
                
            }
            finally
            {
                Close(false);
            }
            return ret;
        }

        public string ExecSql(string sql)
        {
            int cnt = 0;
            return ExecSql(sql, out cnt, false);
        }

        public string FillDataSet(DataSet ds)
        {
            if (ds == null) return "填充数据集不能为空!";
            string ret = null;
            try
            {
                Connect();
                                
                //reccnt = comm.ExecuteNonQuery();
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    
                   string[] strcols = new string[ds.Tables[i].Columns.Count];
                   string[] strparams = new string[ds.Tables[i].Columns.Count];
                   for(int col=0;col<ds.Tables[i].Columns.Count;col++)
                   {
                       strcols[col] = ds.Tables[i].Columns[col].ColumnName ;
                       strparams[col] = "?" ;

                   }
                    
                    string sql = string.Format("select {1} from {0} where 1<>1",ds.Tables[i].TableName,string.Join(",",strcols));

                    DataTable dt = new DataTable();
                    OleDbDataAdapter oa = new OleDbDataAdapter(comm);
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = sql;
                    oa.SelectCommand = new OleDbCommand();
                    oa.SelectCommand.Connection = comm.Connection;
                    oa.SelectCommand.CommandText = sql;
                    oa.SelectCommand.CommandType = CommandType.Text;
                    if (comm.Transaction == null)
                        comm.Transaction = comm.Connection.BeginTransaction();
                    oa.SelectCommand.Transaction  = comm.Transaction;
                    oa.Fill(dt);
                    dt.TableName = ds.Tables[i].TableName;
                    for (int r = 0; r < ds.Tables[i].Rows.Count; r++)
                    {
                        dt.Rows.Add(ds.Tables[i].Rows[r].ItemArray);
                    }
                    //sql = "insert into #temp(p_g_order_no) values(@p_g_order_no)";
                    //sda.InsertCommand = new SqlCommand(sql, sqlT.Connection, sqlT);
                    //oa.InsertCommand.CommandType = CommandType.TableDirect;
                    //sda.InsertCommand.Parameters.Add("@p_g_order_no", SqlDbType.VarChar, 12, "p_g_order_no");
                    oa.InsertCommand = comm;
                    sql = string.Format("insert into {0}({1}) values({2})", ds.Tables[i].TableName, string.Join(",", strcols), string.Join(",", strparams));
                    
                    oa.InsertCommand.CommandType = CommandType.Text;

                    oa.InsertCommand.CommandText = sql;
                    oa.InsertCommand.Transaction = comm.Transaction;
                    oa.InsertCommand.Parameters.Clear();
                    for (int col = 0; col < strcols.Length; col++)
                    {
                        oa.InsertCommand.Parameters.Add("@" + strcols[col],OleDbType.VarChar, 500,strcols[col]);
                    }
                    //oa.Fill(dt);
                    //oa.Fill(ds);
                    OleDbCommandBuilder oldcommbd = new OleDbCommandBuilder(oa);
                    oa.Update(dt);
                    
                    //oa.InsertCommand.ExecuteNonQuery();
                    //ds.AcceptChanges();
                    dt.AcceptChanges();
                    //
                    oa.InsertCommand.Transaction.Commit();
                    //oldcommbd.
                    
                }
            }
            catch (Exception e)
            {
                ret = e.Message;
                comm.Transaction.Rollback();
            }
            finally
            {
                Close();
            }
            return ret;
            
        }

        public string ExecSql(string sql, out int reccnt,bool closeconn)
        {
            string ret = null;
            try
            {
                Connect();

                comm.CommandText = sql;
                comm.CommandType = CommandType.Text;
                reccnt = comm.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                ret = e.Message;
                reccnt = 0;
            }
            finally
            {
                Close(closeconn);
            }
            return ret;
        }

        #region IDisposable 成员

        public void Dispose()
        {
            this.adp = null;
            this.comm = null;
            this.tran = null;
            this.ConnString = null;
            this.conn = null;
            //throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
