using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using WolfInv.Com.MetaDataCenter;
using System.Data;
namespace WolfInv.Com.WCS_Process
{
    /// <summary>
    /// 营业部或部门
    /// </summary>
    public class CDepartment
    {
        #region params
        int _DepId;
        string _DepName;
        CITMSUser _Charger;

        /// <summary>
        /// 部门编号
        /// </summary>
        public int DepId { get { return _DepId; } set { _DepId = value; } }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepName { get { return _DepName; } set { _DepName = value; } }

        /// <summary>
        /// 部门IT主管
        /// </summary>
        public CITMSUser Charger { get { return _Charger; } set { _Charger = value; } }

        #endregion param
    }

    public class CITMSUser
    {
        #region params
        int _UserId;
        string _LoginName;
        List<Group> _Groups;
        string _UserName;
        CDepartment _OwnerDep;
        string _Password;
        /// <summary>
        /// 用户编号
        /// </summary>
        public int UserId { get { return _UserId; } set { _UserId = value; } }
        /// <summary>
        /// 登录用户名
        /// </summary>
        public string LoginName { get { return _LoginName; } set { _LoginName = value; } }
        


        /// <summary>
        /// 所属组
        /// </summary>
        public List<Group> OwnGroups { get { return _Groups; } set { _Groups = value; } }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string UserName { get { return _UserName; } set { _UserName = value; } }

        /// <summary>
        /// 所属部门
        /// </summary>
        public CDepartment OwnerDep { get { return _OwnerDep; } set { _OwnerDep = value; } }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get { return _Password; } set { _Password = value; } }


        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="LoginPwd"></param>
        /// <returns></returns>
        /// 
        #endregion param

        bool LoginWithPwd;

        public string Login(string loginName, string LoginPwd)
        {
            return Login(loginName, LoginPwd, true);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="LoginPwd"></param>
        /// <param name="withpwd">是否用密码登陆，如果false,只要验证用户名</param>
        /// <returns></returns>
        public string Login(string loginName, string LoginPwd,bool withpwd)
        {
            LoginWithPwd = withpwd;
            if ((LoginPwd.Trim().Length  == 0 && withpwd) || loginName.Trim().Length == 0)
            {
                return "用户名和密码不能为空！";
            }
            CITMSUser currUser = this;
            currUser.LoginName = loginName;
            currUser.Password = LoginPwd;
            currUser.LoginWithPwd = withpwd;
            string retError = CheckUser(currUser);
            if (retError != null)
            {
                return retError;
            }
            return retError;
        }

        string CheckUser(CITMSUser user)
        {
            string strError = null;
            List<DataCondition> dcs = new List<DataCondition>();
            //GlobalShare.InitMappings();
            DataCondition dc_user = new DataCondition();
            DataCondition dc_pwd = new DataCondition();
            SystemKeyItem ski_user = null;
            SystemKeyItem ski_pwd = null;
            
            
            GlobalShare.SystemAppInfo.SystemItems.TryGetValue("usercode",out ski_user);
            GlobalShare.SystemAppInfo.SystemItems.TryGetValue("password", out ski_pwd);
            if (ski_user != null)
            {
                dc_user.Datapoint = new DataPoint(ski_user.datapoint);
                dc_user.value = user.LoginName;
            }
            if (ski_pwd != null && user.LoginWithPwd)
            {
                dc_pwd.Datapoint = new DataPoint(ski_pwd.datapoint);
                dc_pwd.value = user.Password;
            }
            if (dc_user.Datapoint == null || (dc_pwd.Datapoint == null && user.LoginWithPwd ))
            {
                return "用户名和密码字段未定义！";
            }
            dcs.Add(dc_user);
            if(user.LoginWithPwd)
                dcs.Add(dc_pwd);
            string msg = null;
            if(!GlobalShare.mapDataSource.ContainsKey(GlobalShare.SystemAppInfo.DataSource))
            {
                return string.Format("用户登录数据源名：[{0}]，数据源集未定义用户登录数据源！",GlobalShare.SystemAppInfo.DataSource);
            }
            DataSet ds = DataSource.InitDataSource(GlobalShare.mapDataSource[GlobalShare.SystemAppInfo.DataSource], dcs, out msg);
            
            if (msg != null)
            {
                return msg;
            }
            if (ds == null) return "无法连接到数据库！";
            if (ds.Tables.Count != 1 ||ds.Tables[0].Rows.Count != 1) return "用户名或密码错误!";
            DataRow dr = ds.Tables[0].Rows[0];
            UserGlobalShare userinfo = new UserGlobalShare(user.LoginName);
            userinfo.appinfo.UserInfo = new UpdateData();//重新实例化
            foreach (DataColumn col in ds.Tables[0].Columns)
            {
                string strcol = col.ColumnName;
                UpdateItem ui = new UpdateItem(strcol, dr[strcol].ToString());
                userinfo.appinfo.UserInfo.Items.Add(strcol, ui);
            }
            
            DataSource.GetDataSourceMapping();
            userinfo.mapDataSource = DataSource.GetGlobalSourcesClone();
            userinfo.UpdateSource();//替换datasource
            userinfo.CurrUser = user;
            if(!GlobalShare.UserAppInfos.ContainsKey(LoginName))
                GlobalShare.UserAppInfos.Add(LoginName ,userinfo);
            UserPerm uperm = new UserPerm(GlobalShare.SystemAppInfo.PermUserPoint, user.LoginName, userinfo);
            uperm.ToXml();
            //GlobalShare.DataChoices = ; userinfo.InitDataChoices(

            //
            GlobalShare.DataChoices =  DataChoice.InitDataChoiceMappings(null);
            userinfo.DataChoices = DataChoice.InitDataChoiceMappings( userinfo);
            return strError;
            
        }

        public string OtherLogin(string strUrl,string loginName, string LoginPwd,bool rethtml)
        {
            //string strUrl = "http://oa.cfzq.com/names.nsf";
            //string strdata = string.Format("Login&F_LogDb=lks/sys/lks_loginlog.nsf&F_appsetuppath=lks/sys/&F_serverurl=oa.cfzq.com&Username={0}&Password={1}", loginName, LoginPwd);
            //byte[] data = Encoding.ASCII.GetBytes(strdata);
            //string furl = string.Format("{0}?{1}", strUrl, strdata);
            string furl = string.Format(strUrl, loginName, LoginPwd);
            if(!rethtml)
                return furl;
            //string strxml = WebReqProcess.GetResult(furl);
            
            WebRequest wq = HttpWebRequest.Create(furl);
            wq.Method = "GET";
            //WebRequest wq = HttpWebRequest.Create(strUrl);
            ////wq.Method = "POST";
            ////wq.ContentType = "application/x-www-form-urlencoded";
            ////wq.ContentLength = data.Length ;
            ////Stream newstream = wq.GetRequestStream();
            ////newstream.Write(data, 0, data.Length);
            ////newstream.Close();
            
            using (WebResponse wp = wq.GetResponse())
            {

                Stream strm = wp.GetResponseStream();
                StreamReader sr = new StreamReader(strm);

                return sr.ReadToEnd();
            }
            return null;
        }
    }

    public class Group
    {
        string _GroupId;
        string _GroupName;
        List<Role> Roles;
    }

    public class Role
    {

    }

    public class FunctionPoint
    {

    }
    
    public enum OptType
    {
        EnView = 1,
        EnList = 2,
        EnEdit = 4,
        EnDel = 8,
        EnAudit = 16
    }
}
