using System;
using System.Text;
using System.Web;
using System.Data;
using System.Net;

namespace WolfInv.Com.AccessDataBase
{
	/// <summary>
	/// comm ��ժҪ˵����
	/// </summary>
	public class Comm
	{
		#region �����ļ�
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
		
		public static string SaveFile(System.Web.UI.HtmlControls.HtmlInputFile file1,string endfix,System.Web.UI.Page page)
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
						
						filename=path+filename+endfix;
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

		public Comm()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}
		public static void DDLBind(System.Web.UI.WebControls.DropDownList ddl,DataTable dt,string vField,string tField,bool allownull)
		{
			if(dt==null) return ;
			if(!dt.Columns.Contains(vField)||!dt.Columns.Contains(tField)) return;
			ddl.DataSource = dt;
			ddl.DataTextField = tField;
			ddl.DataValueField  = vField;
			ddl.DataBind();
			if(allownull)	ddl.Items.Insert(0,"");
		}
		public static void DDLBind(string sql,System.Web.UI.WebControls.DropDownList D1,string VField,string TField)
		{
			D1.DataSource=AccessDB.GetDataSet(sql).Tables[0].DefaultView;
			D1.DataTextField =TField;
			D1.DataValueField=VField;
			D1.DataBind();		
			D1.Items.Insert(0,"");
		}

		/// <summary>
		/// ��ʡ��������
		/// </summary>
		/// <param name="ddl"></param>
		public static void BindProvince(System.Web.UI.WebControls.DropDownList ddl)
		{
			Comm.DDLBind("select * from base_provinces order by code",ddl,"code","name");
		}

		/// <summary>
		/// ��֧ϵ������
		/// </summary>
		/// <param name="ddl"></param>
		public static void BindRemus(System.Web.UI.WebControls.DropDownList ddl)
		{
			Comm.DDLBind("select * from remus_table where (disable<>1 or disable is null) order by code",ddl,"code","name");
		}

		/// <summary>
		/// ��ָ��ʡ���µ�֧ϵ������
		/// </summary>
		/// <param name="ddl">������ؼ�</param>
		/// <param name="str_province">ʡ�ݱ���</param>
		public static void BindRemus(System.Web.UI.WebControls.DropDownList ddl,string str_province)
		{
			Comm.DDLBind("select * from remus_table where (disable<>1 or disable is null) and province='"+str_province+"'order by code",ddl,"code","name");
		}
		/// <summary>
		/// ������Ϣ��
		/// </summary>
		/// <param name="msg"></param>
		public static void Message(string msg)
		{
			StringBuilder sb=new StringBuilder();
			sb.Append("<script language=\"vbscript\">");
			sb.Append("msgbox \""+msg+"\"");
			sb.Append("</script>");
			System.Web.HttpContext.Current.Response.Write(sb.ToString());
		}

		public static void RetureValue(System.Web.UI.Page page,string val)
		{
			StringBuilder sb=new StringBuilder();
			sb.Append("<script language=javascript>\n");
			sb.Append("window.returnValue= \""+val+"\";\n" );
			sb.Append("window.close();\n" );
			sb.Append("</script>");
			page.Response.Clear();
			page.Response.Write(sb.ToString());
		}

		#region �ַ����Ƿ�������
		/// <summary>
		/// �ַ����Ƿ�������
		/// </summary>
		/// <param name="num"></param>
		/// <returns></returns>
		public static bool IsNumber(string num)
		{
			try
			{
				//				int.Parse(num);
				float.Parse(num);
				return true;
			}
			catch

			{
				return false;
			}
		}
		#endregion

		public static bool ModelRunStatusCheck(string ModelName,ref string name)
		{
			//return true;
			string sql="select status,modelname from runstatus_table where model='{0}'";
			sql=string.Format(sql,ModelName);
			DataSet ds=AccessDB.GetDataSet(sql);
			if(ds==null)return false;
			if(ds.Tables[0].Rows.Count==0) return false;
			name=ds.Tables[0].Rows[0]["modelname"].ToString().Trim();
			if(ds.Tables[0].Rows[0]["status"].ToString().Trim().ToLower() !="off") return true;
			return false;
		}

		public static void ModelContral(string ModelName,System.Web.UI.Page page)
		{
			//string msg="ģ��";
			//string name="";
			//if(!Comm.ModelRunStatusCheck(ModelName,ref name))
			//{
			//	msg=msg+name+"�ѱ�ͣ��";
			//	if(ModelName.ToLower().Trim()=="all")
			//	{
			//		page.Response.Redirect("http://www.chinazhou.com");
			//		return;
			//	}
			//	else
			//	{
			//		page.Response.Redirect("../contral/index.aspx?msg="+msg);
			//		return;
			//	}
			//}
			//Cookie ck=Cookie.ReadCookie();
			//if(ModelName=="contral")
			//{
			//	msg=msg+name+"����Ȩ�޲�����";
			//	try
			//	{
			//		if(ck.Usergroup !="admin")
			//		{
			//			page.Response.Redirect("../contral/index.aspx?msg="+msg);
			//		}
			//	}
			//	catch
			//	{
			//		page.Response.Redirect("../contral/index.aspx?msg="+msg);
			//	}
			//}
		}

		public static bool UserGroupCheck(string usergroup,string ModelName,ref bool AllowQuary,ref bool AllowUpdate,ref bool AllowNew)
		{
			switch(usergroup)
			{
				case "guest":
					AllowQuary=true;
					break;
				case "member":
					AllowNew=true;
					break;
				case "vip":
					AllowUpdate=true;
					break;
				case "admin":
					AllowUpdate=true;
					break;
				default:
					AllowQuary=true;
					break;
			}
			return true;
		}

		public static bool IsAdminGroup(string grp)
		{
			if(grp.ToLower().Trim()=="admin")
			{
				return true;
			}
			return false;
		}

		#region ���˵��ַ����з��źͿո�
		/// <summary>
		/// ���˵��ַ����з��źͿո񣬷��Ű���,��.</>\\(����%{%}������������-����+*������$#@!~
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string FilterString(string str)
		{
			str=str.Trim().Replace(" ","").Replace("  ","");//���˵��ո�
			string filter_sign=",��.</>\\(����%{%}������������-����+*������$#@!~)=";//���˵��������
			for(int i=0;i<filter_sign.Length;i++)
			{
				str=str.Replace(filter_sign.Substring(i,1),"");
			}
			return str;
		}
		#endregion

		public static string CStr(int int_str)
		{
			return int_str.ToString().Trim();
		}

		
		public static bool ControlNullChk(System.Web.UI.Page page,string ctr_name)
		{
			System.Web.UI.Control ctr=page.FindControl(ctr_name);
			if(ctr==null) return false;
			string ctrtype=ctr.GetType().ToString();
			ctrtype=ctrtype.Split('.')[ctrtype.Split('.').Length-1];
			string txt="";
			switch(ctrtype.Trim())
			{
				case "TextBox":
				{
					txt=(ctr as System.Web.UI.WebControls.TextBox).Text;
					break;
				}
				case "DropDownList":
				{
					txt=(ctr as System.Web.UI.WebControls.DropDownList).SelectedValue;
					break;
				}
			}
			if(txt.Trim()=="")
			{
				return false;
			}
			return true;
		}

	}

	
}
