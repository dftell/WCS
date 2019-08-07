using System.Collections.Generic;
using System.Xml;
using XmlProcess;
using WolfInv.Com.MetaDataCenter;
namespace WolfInv.Com.WCS_Process
{
    public class AppInfo
    {
        public UserGlobalShare userglobal;
        public string AssemName
        {
            get
            {
                return GlobalShare.MainAssem.GetName(false).Name;
            }
        }
        public string Title;
        public string FormText;
        public string IconPath;
        public string UserField;
        public string PasswordField;
        public string DataSource;
        public string PermDataSource;
        public string PermUserPoint;
        public string LoginUrl;
        public UpdateData UserInfo = new UpdateData();
        public Dictionary<string, SystemKeyItem> SystemItems = new Dictionary<string, SystemKeyItem>();
        public Dictionary<string, SystemKeyItem> StatusItems = new Dictionary<string, SystemKeyItem>();
        public Dictionary<string, SystemKeyItem> PermItems = new Dictionary<string, SystemKeyItem>();
        public void LoadXml(XmlNode node)
        {
            this.Title = XmlUtil.GetSubNodeText(node, "@title");
            this.FormText = XmlUtil.GetSubNodeText(node, "@formtext");
            this.IconPath = XmlUtil.GetSubNodeText(node, "@icon");
            this.DataSource = XmlUtil.GetSubNodeText(node, "@datasouce");
            this.LoginUrl = XmlUtil.GetSubNodeText(node, "@loginurl");
            //this.AssemName = GlobalShare.MainAssem.FullName;
            XmlNodeList nodes = node.SelectNodes("UserInfos/f");
            foreach (XmlNode infonode in nodes)
            {
                SystemKeyItem ski = new SystemKeyItem();
                ski.LoadXml(infonode);
                if(!SystemItems.ContainsKey(ski.type))
                {
                    SystemItems.Add(ski.type, ski);
                }
            }
            XmlNode permnode = node.SelectSingleNode("Perm");
            this.PermDataSource = XmlUtil.GetSubNodeText(permnode, "@source");
            this.PermUserPoint = XmlUtil.GetSubNodeText(permnode, "@userpoint");
            XmlNodeList permsettings = permnode.SelectNodes("f");
            foreach (XmlNode infonode in permsettings)
            {
                SystemKeyItem ski = new SystemKeyItem();
                ski.LoadXml(infonode);
                if (!this.PermItems.ContainsKey(ski.type))
                {
                    this.PermItems.Add(ski.type, ski);
                }
            }
        }
    }
}
