using System.Collections.Generic;
using System.Xml;
using XmlProcess;
namespace WolfInv.Com.WCS_Process
{
    public class UserGlobalShare
    {
        public Dictionary<string, DataSource> mapDataSource;
        public CITMSUser CurrUser;
        public Dictionary<string, DataChoice> DataChoices;
        public AppInfo appinfo;
        public XmlDocument PermXml;

        public CITMSUser user;
        public string UID;
        public UserGlobalShare(string uid)
        {
            UID = uid;
            appinfo = new AppInfo();
            appinfo.userglobal = this;
        }

        public void UpdateSource()
        {
            foreach (string name in  mapDataSource.Keys)
            {
                mapDataSource[name].xmlReq = UpdateWithUseInfo(mapDataSource[name].xmlReq,this.appinfo);
            }
        }
        public XmlNode UpdateWithUseInfo(XmlNode input)
        {
            return UpdateWithUseInfo(input, this.appinfo);
        }

        public static XmlNode UpdateWithUseInfo(XmlNode input,AppInfo userinfo )
        {
            if (input == null) return input;
            if (userinfo == null) return input;
            XmlNode rootnode = null;
            if (input is XmlDocument)
                rootnode = input.SelectSingleNode("/*");
            else
                rootnode = input.SelectSingleNode(".");

            string strxml = input.OuterXml;
            foreach (SystemKeyItem keyitem in GlobalShare.SystemAppInfo.SystemItems.Values)
            {
                if (userinfo.UserInfo.Items.ContainsKey(keyitem.datapoint))
                {
                    string val = userinfo.UserInfo.Items[keyitem.datapoint].value;
                    strxml = strxml.Replace("{" + keyitem.type + "}", val);
                }

            }
            if (strxml != input.OuterXml)
            {
                XmlDocument newdoc = new XmlDocument();
                newdoc.LoadXml(strxml);
                if (input is XmlDocument)
                {
                    return ChangeDocumentByPerm(userinfo.userglobal,newdoc);
                }
                return ChangeDocumentByPerm(userinfo.userglobal,newdoc.SelectSingleNode(rootnode.Name));
                //                prtnode.AppendChild(xmldoc.ImportNode(newdoc.SelectSingleNode("ds"), true));
            }

            return ChangeDocumentByPerm(userinfo.userglobal,input);
        }

        static XmlNode ChangeDocumentByPerm(UserGlobalShare userinfo,XmlNode node)
        {
            XmlNodeList nodes = node.SelectNodes("//*[@pmId]");
            foreach (XmlNode snode in nodes)
            {
                string pmid = XmlUtil.GetSubNodeText(snode, "@pmId");
                PermHandle ph = new PermHandle(userinfo);
                bool perm = ph.CheckPermId(pmid);
                if (!perm)
                {
                    XmlUtil.AddAttribute(snode, "perm", "0",true);
                }

            }
            return node;
        }

    }
}
