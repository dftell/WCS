using System;
using System.Collections;
using System.Data;
using System.Configuration;
using System.Data.OleDb;

namespace WolfInv.Com.AccessDataBase
{
	/// <summary>
	/// AccessDB 的摘要说明。
	/// </summary>
	public class AccessDB
	{

		#region database 
		private static string GetConnStr()
		{
			
			string constr=ConfigurationSettings.AppSettings["ConnectionString"];
					
			return constr;
		}
	
		#endregion

		#region 获得/设置记录集
		public static DataSet GetDataSet(string sql)
		{
			if(isVirus(sql))
			{
				return null;
			}
			string constr=AccessDB.GetConnStr();
			System.Data.OleDb.OleDbConnection  oc=new OleDbConnection();
			
			try
			{
                oc.ConnectionString = constr;

                DataSet ds = new DataSet();
				if(oc.State ==ConnectionState.Closed)
				{
					oc.Open();
				}
				System.Data.OleDb.OleDbDataAdapter odap=new OleDbDataAdapter(sql,oc);
				odap.Fill (ds);
				oc.Close();
                return ds;
			}
			catch(Exception e)
			{
				////                throw e;
				oc.Close();
				return null;
			}

			
		}
        /// <summary>
        /// 是否是恶意代码或病毒
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        private static bool isVirus(string sql)
        {
            return false;
        }

        public static DataSet GetDataSet(string sql,ref string msg)
		{
			if(AccessDB.isVirus(sql))
			{
				return null;
			}
			string constr=AccessDB.GetConnStr();
			System.Data.OleDb.OleDbConnection  oc=new OleDbConnection();
			oc.ConnectionString =constr;
			
			System.Data.OleDb.OleDbDataAdapter odap=new OleDbDataAdapter(sql,oc);
			DataSet ds=new DataSet();
			try
			{
				if(oc.State ==ConnectionState.Closed)
				{
					oc.Open();
				}
				odap.Fill (ds);
				oc.Close();
			}
			catch(Exception e)
			{
				////                throw e;
				oc.Close();
				msg=e.Message.Trim();
				return null;
			}
			return ds;
		}
		#endregion

		#region 用记录集修改记录
		public static int UpdateTableByDataSet(DataTable dt,string sql,string key)
		{
			int ret=0;
			string constr=AccessDB.GetConnStr();
			System.Data.OleDb.OleDbConnection  myConn=new OleDbConnection();
			myConn.ConnectionString =constr;
			DataSet ds=new DataSet();
			try
			{
				if(myConn.State ==ConnectionState.Closed)
				{
					myConn.Open();
				}
				System.Data.OleDb.OleDbDataAdapter myDataAdapter=new OleDbDataAdapter(sql,myConn);
				System.Data.OleDb.OleDbCommandBuilder cb   =   new  OleDbCommandBuilder(myDataAdapter); 
				if(myConn.State==ConnectionState.Open)
				{
					myConn.Close();
				}
				myConn.Open();   
				bool selfadd=false;
				int currvalue=0;
				myDataAdapter.Fill(ds,dt.TableName);
				if(ds==null) return -1;
				int reccnt=ds.Tables[0].Rows.Count;
				for(int i=0;i<dt.Rows.Count;i++)
				{
					if(!dt.Columns.Contains(key))
					{
						dt.Columns.Add(key);
						break;
					}
					if(dt.Rows[i][key].ToString()==""||dt.Rows[i][key]==null)
					{
						selfadd=true;
						currvalue=int.Parse(AccessDB.GetDataSet("select currvalue from count_table where tablename='"+dt.TableName+"'").Tables[0].Rows[0][0].ToString());
						dt.Rows[i][key]=currvalue+i;
						ret=currvalue;
					}
					else
					{
						if(Comm.IsNumber(dt.Rows[i][key].ToString()))
						{
							ret=int.Parse(dt.Rows[i][key].ToString());
						}
						else
						{
							ret=1;
						}
					}
				}
				   
				//code   to   modify   data   in   DataSet   here   
				//Without   the   SqlCommandBuilder   this   line   would   fail   
				ds.Tables.Clear();
				ds.Tables.Add(dt);
				ds.Tables[0].TableName=dt.TableName;
				if(selfadd||reccnt==0)
				{
					myDataAdapter.InsertCommand=cb.GetInsertCommand();
					myDataAdapter.Update(ds,dt.TableName);
				}
				else
				{

					myDataAdapter.UpdateCommand=cb.GetUpdateCommand();
					myDataAdapter.Update(ds,dt.TableName);
				}
				
				if(selfadd)
				{
					++currvalue;
					string cntsql="update count_table set currvalue=currvalue"+dt.Rows.Count+" where tablename='"+dt.TableName+"'";
					System.Data.OleDb.OleDbCommand olc=new OleDbCommand(cntsql,myConn);
					olc.ExecuteNonQuery();
				}
				
			}
			catch(Exception e)
			{
				return -1;
			}
			finally
			{
				myConn.Close();
			}
			return ret;
		}

		#endregion

		#region 用记录集修改记录
		public static int UpdateTableByDataSet(DataSet ds,string sql,string tablename,string key)
		{
			int ret=0;
			string constr=AccessDB.GetConnStr();
			System.Data.OleDb.OleDbConnection  myConn=new OleDbConnection();
			myConn.ConnectionString =constr;
			
			try
			{
				if(myConn.State ==ConnectionState.Closed)
				{
					myConn.Open();
				}
				System.Data.OleDb.OleDbDataAdapter myDataAdapter=new OleDbDataAdapter(sql,myConn);
				System.Data.OleDb.OleDbCommandBuilder cb   =   new  OleDbCommandBuilder(myDataAdapter); 
				if(myConn.State==ConnectionState.Open)
				{
					myConn.Close();
				}
				myConn.Open();   
				bool selfadd=false;
				int currvalue=0;
				if(ds.Tables[0].Rows[0][key].ToString()==""||ds.Tables[0].Rows[0][key]==null)
				{
					selfadd=true;
					currvalue=int.Parse(AccessDB.GetDataSet("select currvalue from count_table where tablename='"+tablename+"'").Tables[0].Rows[0][0].ToString());
					ds.Tables[0].Rows[0][key]=currvalue;
					ret=currvalue;
				}
				else
				{
					if(Comm.IsNumber(ds.Tables[0].Rows[0][key].ToString()))
					{
						ret=int.Parse(ds.Tables[0].Rows[0][key].ToString());
					}
					else
					{
						ret=1;
					}
				}
				myDataAdapter.Fill(ds,tablename);   
				//code   to   modify   data   in   DataSet   here   
				//Without   the   SqlCommandBuilder   this   line   would   fail   
				if(selfadd||ds.Tables[1].Rows.Count==0)
				{
					myDataAdapter.InsertCommand=cb.GetInsertCommand();
					ds.Tables.RemoveAt(1);
					ds.Tables[0].TableName=tablename;
					myDataAdapter.Update(ds,tablename);
				}
				else
				{

					myDataAdapter.UpdateCommand=cb.GetUpdateCommand();
//////					//ds.Tables[1].Rows[0]=ds.Tables[0].Rows[0];
					foreach(DataColumn cls in ds.Tables[0].Columns)
					{
						if(!ds.Tables[1].Columns.Contains(cls.ColumnName)) continue;
						ds.Tables[1].Rows[0][cls.ColumnName]=ds.Tables[0].Rows[0][cls.ColumnName];
					}

					//ds.Tables.RemoveAt(1);
					//ds.Tables[0].TableName=tablename;

					myDataAdapter.Update(ds,tablename);
				}
				
				if(selfadd)
				{
					++currvalue;
					string cntsql="update count_table set currvalue="+currvalue.ToString()+" where tablename='"+tablename+"'";
					System.Data.OleDb.OleDbCommand olc=new OleDbCommand(cntsql,myConn);
					olc.ExecuteNonQuery();
				}
				
			}
			catch(Exception e)
			{
				return -1;
			}
			finally
			{
				myConn.Close();
			}
			return ret;
		}

		#endregion

		#region 用记录集修改记录
		public static int UpdateTableByDataSet(DataSet ds,string sql,string tablename,string key,string DirectDel)
		{
			int ret=0;
			string constr=AccessDB.GetConnStr();
			System.Data.OleDb.OleDbConnection  myConn=new OleDbConnection(constr);
			try
			{
				
				System.Data.OleDb.OleDbDataAdapter myDataAdapter=new OleDbDataAdapter(sql,myConn);
				System.Data.OleDb.OleDbCommandBuilder cb   =   new  OleDbCommandBuilder(myDataAdapter); 
				if(myConn.State==ConnectionState.Open)
				{
					myConn.Close();
				}
				myConn.Open();   
				bool selfadd=false;
				int currvalue=0;
				if(ds.Tables[0].Rows[0][key].ToString()==""||ds.Tables[0].Rows[0][key]==null)
				{
					selfadd=true;
					currvalue=int.Parse(AccessDB.GetDataSet("select currvalue from count_table where tablename='"+tablename+"'").Tables[0].Rows[0][0].ToString());
					ds.Tables[0].Rows[0][key]=currvalue;
					ret=currvalue;
				}
				else
				{
					if(Comm.IsNumber(ds.Tables[0].Rows[0][key].ToString()))
					{
						ret=int.Parse(ds.Tables[0].Rows[0][key].ToString());
					}
					else
					{
						ret=1;
					}
				}
				myDataAdapter.Fill(ds,tablename);   
				//code   to   modify   data   in   DataSet   here   
				//Without   the   SqlCommandBuilder   this   line   would   fail   
				if(selfadd||ds.Tables[1].Rows.Count==0)
				{
					myDataAdapter.InsertCommand=cb.GetInsertCommand();
					ds.Tables.RemoveAt(1);
					ds.Tables[0].TableName=tablename;
					myDataAdapter.Update(ds,tablename);
				}
				else
				{

					myDataAdapter.UpdateCommand=cb.GetUpdateCommand();
					//////					//ds.Tables[1].Rows[0]=ds.Tables[0].Rows[0];
//////					foreach(DataColumn cls in ds.Tables[0].Columns)
//////					{
//////						if(!ds.Tables[1].Columns.Contains(cls.ColumnName)) continue;
//////						ds.Tables[1].Rows[0][cls.ColumnName]=ds.Tables[0].Rows[0][cls.ColumnName].ToString();
//////					}

					ds.Tables.RemoveAt(1);
					ds.Tables[0].TableName=tablename;

					myDataAdapter.Update(ds,tablename);
				}
				
				if(selfadd)
				{
					++currvalue;
					string cntsql="update count_table set currvalue="+currvalue.ToString()+" where tablename='"+tablename+"'";
					System.Data.OleDb.OleDbCommand olc=new OleDbCommand(cntsql,myConn);
					olc.ExecuteNonQuery();
				}
				
			}
			catch(Exception e)
			{
				return -1;
			}
			finally
			{
				myConn.Close();
			}
			return ret;
		}
		public static int UpdateTableByDataSet(DataSet ds,string sql,string tablename,string key,bool mutil)
		{
			int ret=0;
			string constr=AccessDB.GetConnStr();
			System.Data.OleDb.OleDbConnection  myConn=new OleDbConnection(constr);
			try
			{
				
				System.Data.OleDb.OleDbDataAdapter myDataAdapter=new OleDbDataAdapter(sql,myConn);
				System.Data.OleDb.OleDbCommandBuilder cb   =   new  OleDbCommandBuilder(myDataAdapter);   
				myConn.Open();   
				bool selfadd=false;
				int currvalue=0;
				if(ds.Tables[0].Rows[0][key].ToString()==""||ds.Tables[0].Rows[0][key]==null)
				{
					selfadd=true;
					currvalue=int.Parse(AccessDB.GetDataSet("select currvalue from count_table where tablename='"+tablename+"'").Tables[0].Rows[0][0].ToString());
					ds.Tables[0].Rows[0][key]=currvalue;
					ret=currvalue;
				}
				else
				{
					if(Comm.IsNumber(ds.Tables[0].Rows[0][key].ToString()))
					{
						ret=int.Parse(ds.Tables[0].Rows[0][key].ToString());
					}
					else
					{
						ret=1;
					}
				}
				myDataAdapter.Fill(ds,tablename);   
				//code   to   modify   data   in   DataSet   here   
				//Without   the   SqlCommandBuilder   this   line   would   fail   
				if(selfadd||ds.Tables[1].Rows.Count==0)
				{
					myDataAdapter.InsertCommand=cb.GetInsertCommand();
					for(int i=0;i<ds.Tables[0].Rows.Count;i++)
					{
						DataRow dr=ds.Tables[1].NewRow();
						foreach(DataColumn cls in ds.Tables[0].Columns)
						{
							
							if(!ds.Tables[1].Columns.Contains(cls.ColumnName)) continue;
							string val=ds.Tables[0].Rows[i][cls.ColumnName].ToString();
							switch(cls.DataType.ToString())
							{
								case "System.Int32":
									dr[cls.ColumnName]=Comm.IsNumber(val)==true?int.Parse(val):0;
									break;
								case  "System.Double":
									dr[cls.ColumnName]=Comm.IsNumber(val)==true?Double.Parse(val):0;
									break;
								default:
									dr[cls.ColumnName]=val;
									break;
							}
						
						}
						ds.Tables[1].Rows.Add(dr);
					}
					myDataAdapter.Update(ds.Tables[1]);
				}
				else
				{
					myDataAdapter.UpdateCommand=cb.GetUpdateCommand();
					//ds.Tables[1].Rows[0]=ds.Tables[0].Rows[0];

					foreach(DataColumn cls in ds.Tables[0].Columns)
					{
						if(ds.Tables[1].Columns.Contains(cls.ColumnName))
						ds.Tables[1].Rows[0][cls.ColumnName]=ds.Tables[0].Rows[0][cls.ColumnName].ToString();
					}
					myDataAdapter.Update(ds,tablename);
				}
				
				if(selfadd)
				{
					++currvalue;
					string cntsql="update count_table set currvalue="+currvalue.ToString()+" where tablename='"+tablename+"'";
					System.Data.OleDb.OleDbCommand olc=new OleDbCommand(cntsql,myConn);
					olc.ExecuteNonQuery();
				}
			}
			catch(Exception e)
			{
				return -1;
			}
			finally
			{
				myConn.Close();
			}
			return ret;
		}
		#endregion

		#region 执行 sql
		public static string ExecSQL(string sql)
		{
			if(AccessDB.isVirus(sql))
			{
				return null;
			}
			string constr=AccessDB.GetConnStr();
			System.Data.OleDb.OleDbConnection  oc1=new OleDbConnection(constr);
			System.Data.OleDb.OleDbCommand ocmd1=new OleDbCommand(sql,oc1);
			try
			{
				if(ocmd1.Connection.State ==ConnectionState.Open)
				{
					ocmd1.Connection.Close();
				}
				ocmd1.Connection.Open();
				ocmd1.ExecuteNonQuery();
				oc1.Close();
			}
			catch(Exception e)
			{
				//throw e;
				oc1.Close();
				return e.Message+e.StackTrace;
			}

			return null;
		}
		#endregion

		#region 从excel获得数据
		/// <summary>
		/// 从excel中获得数据集
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="filename"></param>
		/// <returns></returns>
		public static DataSet GetDataSetFromExcel(string sql,string filename)
		{
		    string strconn="Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source="+filename+";"+ "Extended Properties=Excel 8.0;";
		    OleDbConnection conn = new OleDbConnection(strconn);
		    OleDbDataAdapter myCommand = new OleDbDataAdapter(sql,conn);
		    DataSet ds = new DataSet();
		    try
		    {
		        myCommand.Fill(ds);
		        conn.Close();
		    }
		    catch
		    {
		        conn.Close();
		        return null;
		    }
		    return ds;
		}

		#endregion

		#region 执行存储过程
		public static bool Exec_Proc(string proc_name,System.Data.OleDb.OleDbParameter[] param,ref int retcnt)
		{
			string constr=AccessDB.GetConnStr();
			System.Data.OleDb.OleDbConnection  oc1=new OleDbConnection(constr);
			System.Data.OleDb.OleDbCommand ocmd1=new OleDbCommand();
			try
			{
				if(oc1.State ==ConnectionState.Closed)
				{
					oc1.Open();
				}				
				ocmd1.CommandType=CommandType.StoredProcedure;
				ocmd1.Connection =oc1;
				
				ocmd1.CommandText =proc_name;
				
				for(int i=0;i<param.Length;i++)
				{
					ocmd1.Parameters.Add(param[i]);
				}
				
				retcnt=ocmd1.ExecuteNonQuery();
			}
			catch(Exception e)
			{
				//throw e;
				return false;
			}
			finally
			{
				oc1.Close();
			}
			return true;
		}

		public static DataSet Exec_Proc(string proc_name,System.Data.OleDb.OleDbParameter[] param)
		{
			DataSet ds = null;
			string constr=AccessDB.GetConnStr();
			System.Data.OleDb.OleDbConnection  oc1=new OleDbConnection(constr);
			System.Data.OleDb.OleDbCommand ocmd1=new OleDbCommand();
			System.Data.OleDb.OleDbDataAdapter oad = new OleDbDataAdapter(ocmd1);
			try
			{
				if(oc1.State ==ConnectionState.Closed)
				{
					oc1.Open();
				}				
				ocmd1.CommandType=CommandType.StoredProcedure;
				ocmd1.Connection =oc1;
				
				ocmd1.CommandText =proc_name;
				
				for(int i=0;i<param.Length;i++)
				{
					ocmd1.Parameters.Add(param[i]);
				}
				
				ocmd1.ExecuteNonQuery();
				oad.Fill(ds);
			}
			catch(Exception e)
			{
				//throw e;
				return null;
			}
			finally
			{
				oc1.Close();
			}
			return ds;
		}
		#endregion

		#region 保存文件
		public static string SaveFile(System.Web.UI.HtmlControls.HtmlInputFile file1,System.Web.UI.Page page)
		{
			try
			{
				if( file1.PostedFile!=null )
				{
					string filename;
					string path=page.Server.MapPath(".\\up\\");
					if(file1.PostedFile.ContentLength>0&&file1.PostedFile.ContentLength<20971520)
					{
						filename=System.DateTime.Now.Year.ToString()+System.DateTime.Now.Month.ToString()+System.DateTime.Now.Day.ToString()+
							System.DateTime.Now.Hour.ToString()+System.DateTime.Now.Minute.ToString()+System.DateTime.Now.Second.ToString()+
							System.DateTime.Now.Millisecond.ToString();
						
						filename=path+filename;
						file1.PostedFile.SaveAs(filename);
						return filename;
					}
				}
			}
			catch(Exception e)
			{
				//				string c=e.Message;
				return "";
			}
			return "";
		}
		#endregion
	}
}
