using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using XmlProcess;
using WolfInv.Com.MetaDataCenter;
using System.Data;

namespace WolfInv.Com.WCS_Process
{
    public class Permission
    {
    }


    public class PermHandle:IUserData
    {
        string _uid;
        #region IUserData 成员

        public string strUid
        {
            get
            {
                return _uid;
            }
            set
            {
                _uid = value;
            }
        }

        #endregion

        UserGlobalShare uinfo = null;
        public PermHandle(string _suid)
        {
            _uid = _suid;
            if (GlobalShare.UserAppInfos.TryGetValue(_uid, out uinfo))
            {
                return;
            }
        }

        public PermHandle(UserGlobalShare info)
        {
            uinfo = info;
        }
        //simply version for check perm. only need the permId. add by odin.xiong 2008-12-10 18:08:34
        public bool CheckPermId(string strPmId)
        {          
            
            if (uinfo == null || uinfo.PermXml == null) return true;
	        return CheckPermission(strPmId, uinfo.PermXml.OuterXml, "");
        }


        //This function is used to parse a single pmId(like "50|V*49/51|V\D" or "50|V+49/51|V\D"),
        //and check if the pmId has permission on  both the object type level and the object level.
        //the value of vObjPermission should be int or string(like "V\D")
        bool CheckPermission(string sPmId,string sObjTypePermission,string vObjPermission)
        {
	        string[] sVecPmId;
	        int lObjPermission = 0;
	        bool bIsPerm;
	        bIsPerm = false;

	        if (vObjPermission == null || vObjPermission == "")
	        {
		        lObjPermission = -1;
	        }
	        else
	        {
		        //when the vObjPermission is a int
                lObjPermission = 0;
                bool isint = int.TryParse(vObjPermission,out lObjPermission);
		        //when the vObjPermission is a string
                if (!isint)
		        {
			        lObjPermission = GetPermissionMask(vObjPermission);
		        }
	        }
	        if (sPmId.IndexOf("+",0) > 0)
	        {
		        sVecPmId = sPmId.Split('+');
		        for (int i=0,len=sVecPmId.Length; i<len; i++)
		        {
			        bIsPerm = CheckPermissionChip(sVecPmId[i], sObjTypePermission, lObjPermission);
			        if (bIsPerm)
			        {
				        break;
			        }
		        }
	        }
	        else if(sPmId.IndexOf("*",0) > 0)
	        {
		        sVecPmId = sPmId.Split('*');
		        if (sVecPmId.Length > 0)
		        {
			        bIsPerm = true;
		        }
		        for (int i=0,len=sVecPmId.Length; i<len; i++)
		        {
			        bIsPerm = bIsPerm & CheckPermissionChip(sVecPmId[i], sObjTypePermission, lObjPermission);
			        if (!bIsPerm)
			        {
				        break;
			        }
		        }
	        }
	        else
	        {
		        bIsPerm = CheckPermissionChip(sPmId, sObjTypePermission, lObjPermission);
	        }
	        return bIsPerm;
        }
        //the function will get a long value which is match the strPerm, a string value like 'VD'.
        //Added by leiying on 2006-1-25
        int GetPermissionMask(string strPerm)
        {
	        if (strPerm == null || strPerm == "")
		        return 0;

	        int PM_VIEW  = 1;
            int PM_LIST = 2;
            int PM_CREATE = 4;
            int PM_EDIT = 8;
            int PM_DELETE = 16;
            int PM_SHARE = 32;
            int PM_VIEW_DETAIL = 64;
            int PM_LIST_DETAIL = 128;
            int PM_CREATE_DETAIL = 256;
            int PM_EDIT_DETAIL = 512;
            int PM_DELETE_DETAIL = 1024;
            int PM_SHARE_DETAIL = 2048;
            int PM_EXPORT = 4096;
            int PM_EXPORT_DETAIL = 8192;

            int lPerm = 0, lPermission = 0;

	        string sTemp;
            int start = 0;
            int end = 0;
	        int nLen = strPerm.Length;

	        while(end < nLen)
	        {
		        end = strPerm.IndexOf("\\", end);
		        if(end == -1)
		        {
			        end = nLen;
		        }
		        sTemp = strPerm.Substring(start, end);
		        start = end + 1;
		        end = start;
		        switch (sTemp.ToUpper())
		        {
			        case "V":
				        lPerm = PM_VIEW;
				        break;
			        case "L":
				        lPerm = PM_LIST;
				        break;
			        case "C":
				        lPerm = PM_CREATE;
				        break;
			        case "E":
				        lPerm = PM_EDIT;
				        break;
			        case "D":
				        lPerm = PM_DELETE;
				        break;
			        case "S":
				        lPerm = PM_SHARE;
				        break;
			        case "VD":
				        lPerm = PM_VIEW_DETAIL;
				        break;
			        case "LD":
				        lPerm = PM_LIST_DETAIL;
				        break;
			        case "CD":
				        lPerm = PM_CREATE_DETAIL;
				        break;
			        case "ED":
				        lPerm = PM_EDIT_DETAIL;
				        break;
			        case "DD":
				        lPerm = PM_DELETE_DETAIL;
				        break;
			        case "SD":
				        lPerm = PM_SHARE_DETAIL;
				        break;
			        case "X":
				        lPerm = PM_EXPORT;
				        break;
			        case "XD":
				        lPerm = PM_EXPORT_DETAIL;
				        break;
			        default :
				        lPerm = 0;
				        break;
		        }
		        lPermission += lPerm;
	        }
	        return lPermission;
        }

//This function is used to parse a pmId chip(like "49/51|V\D"), and check if the pmId has permission on  both the object type level and the object level.
bool CheckPermissionChip(string sPmIdChip,string sObjTypePermission,int lObjPermission)
{
	bool bIsPerm = false;

	string[] sVec = sPmIdChip.Split('|');
	if (sVec.Length != 2)
	{
		return false;
	}

	string sObjTypes = sVec[0];
	string sPerms = sVec[1];

	if (sPerms.Length ==0)
	{
		return false;
	}

	string[] sVecObjType = sObjTypes.Split('/');
	int lPerms = GetPermissionMask(sPerms);
	bIsPerm = true;
	for (int j=0,len=sVecObjType.Length; j<len; j++)
	{
		int lObjTypePerms;
		string sObjType = sVecObjType[j];
		if (sObjType.Length ==0)
		{
			if ((lPerms & lObjPermission) == lPerms)
			{
				bIsPerm = true;
				break;
			}
			else
			{
				bIsPerm = false;
			}
		}
		else
		{
			lObjTypePerms = GetPermissionByObjectType(sObjType, sObjTypePermission);
			if ((lPerms & lObjTypePerms) == lPerms)
			{
				bIsPerm = true;
				break;
			}
			else
			{
				bIsPerm = false;
			}
		}
	}

	return bIsPerm;
}

int GetPermissionByObjectType(string strObjectType,string strPermissionXml)
{
    XmlDocument xmlDoc = new XmlDocument();
    xmlDoc.LoadXml(strPermissionXml);
	int lPermMask = 0;
	string sPermission ="";

	string sPermPath = "/ups/u/p[@o=" + strObjectType + "]/@v";
	XmlNode node = xmlDoc.SelectSingleNode(sPermPath);
    if (node == null) 
        sPermission = null;
    else
        sPermission = node.Value;
	if(sPermission != null && sPermission != "")
	{
		lPermMask = int.Parse(sPermission);
	}
	else
	{
		//Noded by leiying. Because the binary value of '-1' is '111...1'
		lPermMask = -1;
		/*
		    Xiaobo Yang, 2010/5/13.
		    If there isn't permission definition, it means the user 
		    hasn't right to access the feature.
		lPermMask = 0;
		 */
	}
	return lPermMask;
}


}

    public class UserPerm:IUserData 
    {
        string _uid;
        #region IUserData 成员

        public string strUid
        {
            get
            {
                return _uid;
            }
            set
            {
                _uid = value;
            }
        }

        #endregion

        string _key;
        public FuncPointPermMappingCollection PermList;
        UserGlobalShare _ugs;
        public UserPerm(string key,string uid)
        {
            _key = key;
            _uid = uid;
            PermList = new FuncPointPermMappingCollection();
        }

        public UserPerm(string key, string uid, UserGlobalShare ugs)
        {
            _key = key;
            _uid = uid;
            PermList = new FuncPointPermMappingCollection();
            _ugs = ugs;
        }

        DataSet GetRolePerms()
        {
            
            string source = GlobalShare.SystemAppInfo.PermDataSource;
            if (_ugs == null)
            {

                bool existuser = GlobalShare.UserAppInfos.TryGetValue(_uid, out _ugs);
                if (existuser == false)
                {
                    return null;
                }

            }
            SystemKeyItem ski = null;
        
            DataCondition dc = new DataCondition();
            dc.Datapoint = new DataPoint(_key);
            dc.value = _uid;
            string msg = null;
            List<DataCondition> dcs = new List<DataCondition>();
            dcs.Add(dc);
            bool isextra = false;
            DataSet permdata = DataSource.InitDataSource(source, dcs,_uid,out msg,ref isextra);
            return permdata;
        }

        void ToPermList(DataSet ds)
        {
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                return;
            }
            this.PermList = new FuncPointPermMappingCollection();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                FuncPointPermMapping fpp = new FuncPointPermMapping();
                
                foreach(SystemKeyItem uki in GlobalShare.SystemAppInfo.PermItems.Values)
                {
                    string val = ds.Tables[0].Rows[i][uki.datapoint].ToString();
                    if (ds.Tables[0].Columns.Contains(uki.datapoint))
                    {
                        if (uki.type == "mapid")
                        {
                            fpp.MappingId = val;
                        }
                        if (uki.type == "funcid")
                        {
                            fpp.FuncId = val;
                        }
                        if (uki.type == "permval")
                        {
                            fpp.PermVal = val;
                        }

                    }
                }
                this.PermList.Items.Add(fpp);
            }
        }

        public void ToXml()
        {
            DataSet ds = GetRolePerms();
            ToPermList(ds);
            _ugs.PermXml = new XmlDocument();
            _ugs.PermXml.LoadXml("<ups/>");
            XmlNode root = _ugs.PermXml.SelectSingleNode("ups");
            this.PermList.ToXml(root);
        }
    }

    public class FuncPointPermMappingCollection : IXml 
    {
        public List<FuncPointPermMapping> Items = new List<FuncPointPermMapping>();

        #region IXml 成员

        public XmlNode ToXml(XmlNode parent)
        {
            //throw new Exception("The method or operation is not implemented.");
            XmlDocument xmldoc;
            if (parent == null)
            {
                xmldoc = new XmlDocument();
                xmldoc.LoadXml("<u/>");
                parent = xmldoc.SelectSingleNode("u");
            }
            xmldoc = parent.OwnerDocument;
            XmlNode node = XmlUtil.AddSubNode(parent, "u", true);
            for (int i = 0; i < Items.Count; i++)
            {
                Items[i].ToXml(node);
            }
            return node;
        }

        public void LoadXml(XmlNode node)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }

    public class FuncPointPermMapping:IXml 
    {
        public string MappingId;
        public string FuncId;
        public string PermVal;


        #region IXml 成员

        public XmlNode ToXml(XmlNode parent)
        {
            XmlDocument xmldoc;
            if (parent == null)
            {
                xmldoc = new XmlDocument();
                xmldoc.LoadXml("<p/>");
                parent = xmldoc.SelectSingleNode("p");
            }
            xmldoc = parent.OwnerDocument;
            XmlNode node = XmlUtil.AddSubNode(parent, "p", true);
            XmlUtil.AddAttribute(node, "i", this.MappingId);
            XmlUtil.AddAttribute(node, "o", this.FuncId);
            XmlUtil.AddAttribute(node, "v", this.PermVal);
            return node;
            //	<p i="2576" o="57" v="16383"/>
        }

        public void LoadXml(XmlNode node)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
