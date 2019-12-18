using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using XmlProcess;
using WolfInv.Com.MetaDataCenter;
using System.Reflection;
using System.Linq;

namespace WolfInv.Com.WCS_Process
{
    public enum SystemMode
    {
        FullMode,//完整模式，独立系统运行
        PluginMode //外挂，插件模式，以植入的系统为主
    }

   
    public class GlobalShare
    {
        /// <summary>
        /// 系统应用模式
        /// </summary>
        public static SystemMode AppMode;
        public static Assembly MainAssem;

        
        /// <summary>
        /// 数据连接方式
        /// </summary>
        /// 
        //
/// <summary>
        /// 数据库访问程序目录
/// </summary>
        public static string AppPath;//数据库访问程序目录
       /// <summary>
        /// 提供给客户端的xml程序访问目录
       /// </summary>
        public static string AppXmlPath;
        /// <summary>
        /// 应用程序目录
        /// </summary>
        public static string RootPath;//web根目录
        public static string AppDllPath;//客户端提供的xml程序访问目录
        public static Dictionary<string, ConnectObject> DataConnectObjs;
        public static Dictionary<string, ConnectObject> FileConnectObjs;
        
        static DataInterface FileAccObj;
        public static DataCenterClient DataCenterClientObj;



        public static bool Logined = false;

        public static ConnectObject FileAccConnObj;
        public static Dictionary<string,DataSource> mapDataSource;
        
        public static CITMSUser CurrUser;
        //static List<DocumentType> t_DocumentTypeList;
        public static List<DocumentType> DocumentTypeList;
        //public static List<ITType> ITTypeList;
        //public static List<CMenuItem> Menus;
        //public static List<CMenuItem> Navigator;
        static Dictionary<int, DocumentType> _mapDocumentTypeList;
        public static Dictionary<string, DataPoint> DataPointMappings;
        //
        public static Dictionary<string, DataChoice> DataChoices;
        //
        //
        
        public static AppInfo SystemAppInfo;
        public static Dictionary<string, UserGlobalShare> UserAppInfos =new Dictionary<string, UserGlobalShare>();
        public static Dictionary<int, DocumentType> mapDocumentTypeList
        {
            get 
            {
                if (_mapDocumentTypeList == null && DocumentTypeList != null)
                {
                    _mapDocumentTypeList = new Dictionary<int, DocumentType>();
                    foreach (DocumentType dt in DocumentTypeList)
                    {
                        if (!_mapDocumentTypeList.ContainsKey(dt.TypeId))
                        {
                            _mapDocumentTypeList.Add(dt.TypeId,dt);
                        }
                    }
                }
                return _mapDocumentTypeList;
            }
        }
        
        public static XmlDocument _PermXml;
        public static string DefaultUser //for window form
        {
            get
            {
                foreach (string key in UserAppInfos.Keys)
                {
                    return key;
                }
                return null;
            }

        }
        public static UserGlobalShare DefaultUserInfo
        {
            get
            {
                foreach (UserGlobalShare ret in UserAppInfos.Values)
                {
                    return ret;
                }
                return null;
            }
        }
        public GlobalShare()
        {
            
        }

        public static bool IsSystemParam(string strPoint, out string val)
        {
            val = null;
            if (strPoint == null)
                return false;
            strPoint = strPoint.Trim().Replace("{", "").Replace("}", "").Trim();
            if (GlobalShare.UserAppInfos.First().Value.appinfo.UserInfo.Items.ContainsKey(strPoint))
            {
                val = GlobalShare.UserAppInfos.First().Value.appinfo.UserInfo.Items[strPoint].value;
                return true;
            }
            return false;
        }


        public static void Init(string strAppPath)
        {
            if (FileConnectObjs == null)
            {
                //AppXmlPath = strAppPath;
                //
                AppPath = strAppPath;
                FileConnectObjs = InitConnectOjects(strAppPath, false);
                InitMappings();
            }
            return ;
        }

        public static string InitMappings()
        {

            /*
            //初始化资料类型
            if (DocumentTypeList == null)
                DocumentTypeList = new DocumentTypeProcess(DataConnectObj.ConnectString).GetDocumentTyes();
            //初始化IT设备类型
            if (ITTypeList == null)
                ITTypeList = new ITTypeProcess(DataConnectObj.ConnectString).GetITTypes();
            //ITTypeList
            */
            //初始化数据源
       
            DataConnectObjs = InitConnectOjects(GlobalShare.AppPath, true);
            DataCenterClientObj = new DataCenterClient(GlobalShare.AppPath, DataConnectObjs, FileConnectObjs);
            if (UserAppInfos == null)
            {
                UserAppInfos = new Dictionary<string, UserGlobalShare>();
            }
            try
            {
                
                //XmlDocument xmldoc = new XmlDocument();
                if (mapDataSource == null)
                    mapDataSource = DataSource.GetDataSourceMapping();

                //初始化dataidmappings
                if (DataPointMappings == null)
                {
                    XmlDocument xmldoc = GlobalShare.GetXmlFile("\\xml\\dataidmapping.xml");
                    if (xmldoc == null) throw new Exception("can't get DataSource Config!");
                    DataPointMappings = DataPoint.InitMapping(xmldoc);
                }
                
                
                
            }
            catch (Exception ce)
            {
                throw new Exception(string.Format("无法初始化Mappings,原因:{0}", ce.Message));
            }
            return null;
        }

        public static Dictionary<string, ConnectObject> InitConnectOjects(string AppPath, bool dataconns)
        {
            
                
            if (!dataconns)//初始化时，三个目录统一
            {
                GlobalShare.AppXmlPath = AppPath;
                //GlobalShare.AppDllPath = AppPath;
            }
            if (AppPath == null)
            {
                return null;
            }
            
            Dictionary<string, ConnectObject> ret = new Dictionary<string, ConnectObject>();
            XmlDocument xmldoc = null;
            if (!dataconns)
            {
                xmldoc = new XmlDocument();
                xmldoc.Load(GlobalShare.AppPath + "\\config.xml");
            }
            else
            {
                // xml/config.xml file
                xmldoc = new XmlDocument();
                try
                {
                    if (GlobalShare.AppXmlPath == GlobalShare.AppPath)
                        xmldoc.Load(GlobalShare.AppPath + "\\xml\\config.xml");
                    else
                        xmldoc.Load(GlobalShare.AppXmlPath + "\\xml\\config.xml");
                }
                catch
                {
                    xmldoc = GlobalShare.GetXmlFile("\\xml\\config.xml", null, null, null, null);//本地
                }
            }
            
            if (xmldoc == null) throw new Exception("can't get xml document !");
            XmlNodeList nodes = xmldoc.SelectNodes("/root/DataSource");
            foreach (XmlNode node in nodes)
            {
                //初始化连接对象
                ConnectMethod connmtd = XmlProcess.XmlUtil.GetSubNodeText(node, "@Type") == "Web" ? ConnectMethod.WebSvr : ConnectMethod.DataBase;
                string connstr = XmlUtil.GetSubNodeText(node, "@Src");
                string strid = XmlUtil.GetSubNodeText(node, "@id");
                if (ret.ContainsKey(strid)) continue;
                ConnectObject DataConnectObj = new ConnectObject(strid,connmtd, connstr);
                ret.Add(strid, DataConnectObj);
                
                if (!dataconns)
                {
                    FileAccConnObj = DataConnectObj;
                    GlobalShare.AppXmlPath = DataConnectObj.ConnectString;
                    if (DataConnectObj.Method == ConnectMethod.DataBase)
                    {
                        GlobalShare.AppXmlPath = DataConnectObj.ConnectString;
                    }
                    else
                    {
                       
                    }
                    
                    break;
                }
                
            }

            if (dataconns)
            {
                SystemAppInfo = new AppInfo();
                SystemAppInfo.LoadXml(xmldoc.SelectSingleNode("/root/AppInfo"));
            }
            else
            {
                XmlNode node = xmldoc.SelectSingleNode("/root/@SystemMode");
                AppMode = (node==null ||node.Value.Trim().ToUpper().Equals("PLUGIN")==false)? SystemMode.FullMode:SystemMode.PluginMode;
                if (FileAccConnObj.Method == ConnectMethod.WebSvr)
                {
                    FileAccObj = new WebDataInterface(AppXmlPath, ret);
                }
                else
                {
                    FileAccObj = new ClientDataInterface(AppXmlPath, ret);
                }
            }
            return ret;
        }

        public static XmlNode UpdateWithUseInfo(XmlNode input, string uid)
        {
            UserGlobalShare userinfo = null;
            bool sc = UserAppInfos.TryGetValue(uid, out userinfo);
            if (!sc || userinfo == null) return input;
            return userinfo.UpdateWithUseInfo(input);
        }

        public static XmlDocument UpdateWithUseInfo(XmlDocument input,string uid,bool isDocument)
        {
            XmlNode ret = UpdateWithUseInfo(input, uid);
            if (ret is XmlDocument)
                return ret as XmlDocument;
            return input;
        }

        public static DataChoice GetGlobalChoice(string uid,string chsid)
        {
            if (!UserAppInfos.ContainsKey(uid))
            {
                return null;
            }
            if (!UserAppInfos[uid].DataChoices.ContainsKey(chsid))
            {
                return null;
            }
            return UserAppInfos[uid].DataChoices[chsid];
        }

        ////public void InitDataChoices()
        ////{
        ////    //初始化datachoice
        ////    if (DataChoices == null)
        ////    {
        ////        DataChoices = DataChoice.InitDataChoiceMappings(null);
        ////    }
        ////}

        public static XmlDocument GetXmlFileString(string filepath,string strModule, string strScreen, string strTarget, string sFlag)
        {
            string strFilePath = string.Format("{0}\\{1}",GlobalShare.AppXmlPath,filepath);
            
            XmlDocument xmldoc = new XmlDocument();
            try
            {
                xmldoc.Load(strFilePath);
                return xmldoc;
            }
            catch (Exception ce)
            {
                
            }
            strFilePath = string.Format("{0}\\{1}\\{4}_{1}_{2}_{3}.xml", AppXmlPath , strModule, strScreen, strTarget,sFlag);
            xmldoc = new XmlDocument();
            try
            {
                xmldoc.Load(strFilePath);
            }
            catch (Exception ce)
            {
                return null;
            }
            //this.toolStrip1.Items.Clear();
            return xmldoc;
        }

        /// <summary>
        /// 通过接口对象访问到本地或者网络的文件
        /// </summary>
        /// <param name="filepath">文件的相对路径</param>
        /// <returns></returns>
        public static XmlDocument GetXmlFile(string filepath)
        {
            if (FileAccObj == null)
                return GetXmlFileString(filepath, null, null, null, null);
            return FileAccObj.GetConfigXmlFile(filepath);
        }
        
        public static XmlDocument GetXmlFile(string filepath, string module, string screen, string target, string flag)
        {
            return FileAccObj.GetConfigXmlFile(filepath, module, screen, target, flag);
        }
     }
}
