using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using XmlProcess;
using System.Reflection;
using System.Data;
using WolfInv.Com.WCS_Process;
using WolfInv.Com.MetaDataCenter;
using System.Text.RegularExpressions;
namespace WolfInv.Com.XPlatformCtrlLib
{
    /// <summary>
    /// 权限控制接口
    /// </summary>
    public interface IPermmsionControl
    {
        string PermId { get; set; }

    }

    

    /// <summary>
    /// 菜单逻辑类
    /// </summary>
    public class CMenuItem : ICloneable,IUserData,IXml,IPermmsionControl,iExtraData
    {
        string Uid;
        public string strUid { get { return Uid; } set { Uid = value; } }
        public string MnuId;
       
        public string MnuName;
        public LinkType linkType;
        public string LinkValue;
        public string GridSource;
        public string DetailSrouce;
        public string Module;
        public string Screen;
        public string Target;
        public string Params;
        public string Key;
        public string Title;
        public string LinkUrl;
        public string isSummary;

        public bool ReplaceMenu;
        public string FixField;
        CMenuItem _defaultSubItem;
        public bool Expand;
        public int WWidth = 900;
        public int WHeight = 720;
        public bool OnlyOperateSelectItems;
        public bool OnlyOperateSelectGroup;

        public bool NeedNotice;
        public string NoticeTitle;
        public string NoticeContent;


        public CMenuItem evt;
        public bool UseSubItems;//用子表，用于外部数据
        public bool OnlyKeyDisplay;

        public bool OnlyNoKeyDisplay;

        public string GridGroupBy;
        public bool GroupBeforeSave;
        public bool AllowCheckedMultiItems;

        //实现了 WCSExtraDataInterface的外部数据接口类
        public bool isextradata { get; set; }
        public string extradataassembly { get; set; }
        public string extradataclass { get; set; }
        //转换功能用
        public XmlNode extradataconvertconfig { get; set; }

        //外部数据用
        public XmlNode extradatagetconfig { get; set; }

        //更新类型
        public string extradatawritetype { get; set; }
        //外部数据类型
        public string extradatatype { get; set; }
        public bool extradatanosaveinclient { get; set; }
        //导入类型附加数据
        public XmlNode attatchinfo { get; set; }


        public List<CMenuItem> MnuItems = new List<CMenuItem>();
        public List<DataTranMapping> CopyTranDatas()
        {
            if (TranDataMapping == null) return null;
            List<DataTranMapping> ret = new List<DataTranMapping>();
            for (int i = 0; i < TranDataMapping.Count; i++)
            {
                DataTranMapping dtm = new DataTranMapping();
                dtm.FromDataPoint = TranDataMapping[i].FromDataPoint;
                dtm.ToDataPoint = TranDataMapping[i].ToDataPoint;
                ret.Add(dtm);
            }
            return ret;
        }

        public List<DataTranMapping> TranDataMapping;
        /// <summary>
        /// 默认子节点
        /// </summary>
        public CMenuItem defaultSubItem
        {
            get
            {
                if (_defaultSubItem == null && this.MnuItems != null && this.MnuItems.Count > 0)
                {
                    _defaultSubItem = this.MnuItems[0];
                }
                return _defaultSubItem;
            }
            set { _defaultSubItem = value; }
        }

        public CMenuItem(string uid)
        {
            Uid = uid;
        }

        public XmlNode ToXml(XmlNode parent)
        {
            return ToXml(parent, true);
        }

        public virtual void LoadXml(XmlNode node)
        {
            this.MnuId = XmlUtil.GetSubNodeText(node, "@id");
            this.MnuName = XmlUtil.GetSubNodeText(node, "@name");
            if(this.MnuName.Trim().Length == 0)
            {
                this.MnuName = XmlUtil.GetSubNodeText(node,".");
            }
            this.LinkValue = XmlUtil.GetSubNodeText(node, "@classname");
            this.GridSource = XmlUtil.GetSubNodeText(node, "@gridsource");
            if (this.GridSource == "")
                this.GridSource = XmlUtil.GetSubNodeText(node.ParentNode, "@gridsource");
            this.DetailSrouce = XmlUtil.GetSubNodeText(node, "@datasource");
            this.Params = XmlUtil.GetSubNodeText(node, "@param");
            this.Module = XmlUtil.GetSubNodeText(node, "@module");
            this.UseSubItems = XmlUtil.GetSubNodeText(node, "@usesubitems") == "1";
            this.LinkUrl = XmlUtil.GetSubNodeText(node, "@linkurl");
            this.isSummary = XmlUtil.GetSubNodeText(node, "@issummary");
            this.NeedNotice = XmlUtil.GetSubNodeText(node, "@neednotice")=="1";
            this.NoticeTitle = XmlUtil.GetSubNodeText(node, "@noticetitle");
            this.NoticeContent = XmlUtil.GetSubNodeText(node, "@noticecontent");
            this.GroupBeforeSave = XmlUtil.GetSubNodeText(node, "@groupbeforesave")=="1";

            this.Screen = XmlUtil.GetSubNodeText(node, "@screen");
            this.Target = XmlUtil.GetSubNodeText(node, "@target");
            this.Title = XmlUtil.GetSubNodeText(node, "@title");
            this.Key = XmlUtil.GetSubNodeText(node, "@key");
            this.FixField = XmlUtil.GetSubNodeText(node, "@fixfield");
            this.Expand = XmlUtil.GetSubNodeText(node, "@expand") == "1";
            this.ReplaceMenu = XmlUtil.GetSubNodeText(node, "@list") == "1";
            this.OnlyOperateSelectItems = XmlUtil.GetSubNodeText(node, "@onlyoperateselectitems") == "1";
            this.OnlyOperateSelectGroup = XmlUtil.GetSubNodeText(node, "@onlyoperateselectgroup") == "1";
            this.OnlyKeyDisplay = XmlUtil.GetSubNodeText(node, "@onlykeydisplay") == "1";
            this.OnlyNoKeyDisplay = XmlUtil.GetSubNodeText(node, "@onlynokeydisplay") == "1";
            this.AllowCheckedMultiItems = XmlUtil.GetSubNodeText(node, "@allowcheckedmultiItems") == "1";
            this.GridGroupBy = XmlUtil.GetSubNodeText(node, "@groupby");


            if (!int.TryParse(XmlUtil.GetSubNodeText(node, "@winwidth"), out this.WWidth))
            {
                WWidth = 900;
            }
            if (!int.TryParse(XmlUtil.GetSubNodeText(node, "@winheight"), out this.WHeight))
            {
                WHeight = 720;
            }
            this.isextradata = XmlUtil.GetSubNodeText(node, "@isextradata")=="1";
            this.extradatanosaveinclient = XmlUtil.GetSubNodeText(node, "@extradatanosaveinclient") == "1";
            
            this.extradataassembly = XmlUtil.GetSubNodeText(node, "@extradataassembly");
            this.extradataclass = XmlUtil.GetSubNodeText(node, "@extradataclass");
            this.extradataconvertconfig = node.SelectSingleNode("extradataconvertconfig");
            this.extradatagetconfig = node.SelectSingleNode("extradatagetconfig");
            this.extradatatype = XmlUtil.GetSubNodeText(node, "@extradatatype");
            this.extradatawritetype = XmlUtil.GetSubNodeText(node, "@extradatawritetype");


            this.attatchinfo = node.SelectSingleNode("attatchinfo");

            XmlNodeList mapnodes = node.SelectNodes("./Maps/Map");
            if (mapnodes.Count != 0)
            {
                this.TranDataMapping = new List<DataTranMapping>();
                foreach (XmlNode mapnode in mapnodes)
                {
                    DataTranMapping dtm = new DataTranMapping();
                    dtm.LoadXml(mapnode);
                    this.TranDataMapping.Add(dtm);
                }
            }
            XmlNodeList smapnodes = node.SelectNodes("./SendMaps/Map");
            if (mapnodes.Count != 0)
            {
                this.SendMappings = new List<DataTranMapping>();
                foreach (XmlNode mapnode in mapnodes)
                {
                    DataTranMapping dtm = new DataTranMapping();
                    dtm.LoadXml(mapnode);
                    this.SendMappings.Add(dtm);
                }
            }

            if (node.SelectSingleNode("evt")!=null)
            {
                this.evt = new CMenuItem(this.Uid);
                this.evt.LoadXml(node.SelectSingleNode("evt"));
            }


            string strtype = XmlUtil.GetSubNodeText(node, "@type");
            switch (strtype)
            {
                case "WebPage":
                    {
                        this.linkType = LinkType.WebPage;
                        break;
                    }
                case "Form":
                    {
                        this.linkType = LinkType.Form;
                        break;
                    }
                case "Dialog":
                    {
                        this.linkType = LinkType.Dialog;
                        break;
                    }
                case "Select":
                    {
                        this.linkType = LinkType.Select;
                        break;
                    }
                case "PrintPDF":
                    {
                        this.linkType = LinkType.PrintToPDF;
                        break;
                    }
                case "UserControl":
                case "":
                default:
                    {
                        this.linkType = LinkType.UserControl;
                        break;
                    }
            }
        }

        public override string ToString()
        {
            XmlNode node = ToXml(null, false);
            if (node == null) return "";
            return node.OuterXml;
        }

        public XmlNode ToXml(XmlNode parent,bool Rec)
        {
            XmlNode node = null;
            XmlDocument xmldoc;
            if (parent == null)
            {
                xmldoc = new XmlDocument();
                xmldoc.LoadXml("<menu/>");
                parent = xmldoc.SelectSingleNode("menu");
                node = parent;
            }
            else
            {
                node = XmlUtil.AddSubNode(parent, "menu");
            }
            if (node == null) return null;
            XmlUtil.AddAttribute(node, "uid", strUid);
            if (this.MnuName != null && this.MnuName != "")
                XmlUtil.AddAttribute(node, "name", this.MnuName);
            if (this.MnuId != null && this.MnuId != "") { XmlUtil.AddAttribute(node, "id", this.MnuId); }
            if (this.GridSource != null && this.GridSource != "") { XmlUtil.AddAttribute(node, "gridsource", this.GridSource); }
            if (this.DetailSrouce != null && this.DetailSrouce != "") { XmlUtil.AddAttribute(node, "datasource", this.DetailSrouce); }
            if (this.Params != null && this.Params != "") { XmlUtil.AddAttribute(node, "param", this.Params); }
            if (this.Module != null && this.Module != "") { XmlUtil.AddAttribute(node, "module", this.Module); }
            if (this.Screen != null && this.Screen != "") { XmlUtil.AddAttribute(node, "screen", this.Screen); }
            if (this.Target != null && this.Target != "") { XmlUtil.AddAttribute(node, "target", this.Target); }
            if (this.Title != null && this.Title != "") { XmlUtil.AddAttribute(node, "title", this.Title); }
            if (this.LinkUrl != null && this.LinkUrl != "") { XmlUtil.AddAttribute(node, "linkurl", this.LinkUrl); }
            if (this.isSummary != null && this.LinkUrl != "") { XmlUtil.AddAttribute(node, "issummary", this.isSummary); }
            if (this.Key != null && this.Key != "") { XmlUtil.AddAttribute(node, "key", this.Key); }
            if (this.FixField != null && this.FixField != "") { XmlUtil.AddAttribute(node, "fixfield", this.Key); }
            if (this.LinkValue != null && this.LinkValue != "") { XmlUtil.AddAttribute(node, "classname", this.LinkValue); }
            XmlUtil.AddAttribute(node, "winwidth", this.WWidth.ToString());
            XmlUtil.AddAttribute(node, "winheight", this.WHeight.ToString());
            if (this.PermId == "0") { XmlUtil.AddAttribute(node, "perm", "0"); };
            if (this.Expand)
                XmlUtil.AddAttribute(node, "expand", "1");
            if (this.linkType != LinkType.UserControl)
            {
                XmlUtil.AddAttribute(node, "type", this.linkType.ToString());
            }
            if (this.TranDataMapping != null)
            {
                XmlNode mapsnode = XmlUtil.AddSubNode(node, "Maps");
                for (int i = 0; i < this.TranDataMapping.Count; i++)
                {
                    TranDataMapping[i].ToXml(mapsnode);
                }
            }
            if (this.MnuItems == null) return node;
            if (!Rec) return node;//只输出当前菜单
            for (int i = 0; i < this.MnuItems.Count; i++)
            {
                CMenuItem dc = new CMenuItem(Uid);
                dc.ToXml(node);
            }
            return node;
        }
        
        public object Clone()
        {
            //return this as object;//不复制，引用同一个对象
            //
            return MenuProcess.GetMenu(null,this.ToXml(null),Uid);
            //return this.MemberwiseClone(); //浅复制
            //            return new person() as CMenuItem;//深复制
        }

        public CMenuItem GetMenu(CMenuItem parent, XmlNode node)
        {
            CMenuItem ret = parent;
            return MenuProcess.GetMenu(parent, node,Uid);
            //return ret;
        }

        public CMenuItem GetMenu(string xml)
        {
            XmlDocument xmldoc = new XmlDocument();
            try
            {
                xmldoc.LoadXml(xml);
            }
            catch
            {
                return null;
            }
            return GetMenu(null, xmldoc.SelectSingleNode("menu"));
        }

        #region IPermmsionControl 成员
        string _permid;
        public string PermId
        {
            get
            {
                return _permid;
            }
            set
            {
                _permid = value;
            }
        }

        public List<DataTranMapping> SendMappings { get; private set; }

        #endregion
    }

    public enum LinkType
    {
        WebPage,
        Form,
        UserControl,
        Dialog,
        Select,
        PrintToPDF
    }


    public class MenuProcess:IUserData 
    {
        string Uid;
        public string strUid { get { return Uid; } set { Uid = value; } }
        protected XmlDocument xmldoc = new XmlDocument();
        public MenuProcess(XmlDocument objxmldoc,string uid)
        {
            xmldoc = objxmldoc;
            if(xmldoc == null)
                throw new Exception(string.Format("menu xml docment load failed!"));
            xmldoc = GlobalShare.UpdateWithUseInfo(objxmldoc, uid) as XmlDocument;
            Uid = uid;
            ////try
            ////{
            ////    xmldoc.Load(mnuXmlPath);
            ////    xmldoc.LoadXml(GlobalShare.UpdateWithUseInfo(xmldoc, uid).OuterXml);
            ////}
            ////catch (Exception ce)
            ////{
            ////    throw new Exception(string.Format("can't load the file {0}!Error:{1}", mnuXmlPath,ce.Message));
            ////}
            //xmldoc.LoadXml(GlobalShare.UpdateWithUseInfo(xmldoc).OuterXml);
        }

        public void ReDoMenuXml()
        {
            return;
            XmlNodeList xmlrefnodes = xmldoc.SelectNodes("/menus//menu[@reffile]");//检查菜单srcList属性
            foreach (XmlNode node in xmlrefnodes)
            {
                XmlNode parent = node.ParentNode;
                string path = XmlUtil.GetSubNodeText(node,"@reffile");//文件名
                try
                {
                    MenuProcess mp = new MenuProcess(GlobalShare.GetXmlFile(path),Uid);
                    
                    parent.InsertBefore(parent.OwnerDocument.ImportNode(mp.xmldoc.SelectSingleNode("menus/*"), true),node);
                    parent.RemoveChild(node);
                }
                catch
                {
                    continue;
                }

            }
            /**
             * 
<menu id="SupportMgr" name="供应商管理" isSep="" default="SupportMgr"  classname="ITMS_APP.frm_View" gridsource="SupportSummary" module="Support" screen="main" target="summary"  srcType="" expand="0">
		<menu type="list" gridsource="ITEqeitTypes" id="ET001" name="ET002" title="供应商" classname="ITMS_APP.support.frm_supportlist" param="ET001" target=""/>	
	</menu>
             * **/
            /**
                 * 改从datasource取得值 2010/11/4 by zhouys
                 * 
                 * **/

            XmlNodeList xmlnodes = xmldoc.SelectNodes("/menus//menu[@list='1']");//检查菜单srcList属性
            for (int ncnt = xmlnodes.Count -1;ncnt >=0;ncnt--)
            {
                XmlNode node = xmlnodes[ncnt];////////////////////
                CMenuItem cmnu = new CMenuItem(Uid);
                ///////////////////////////////////////////////////////////cmnu = GetMenu(cmnu, node,Uid);
                if (!cmnu.ReplaceMenu) continue;
                string strds = XmlUtil.GetSubNodeText(node, "@repeatsource");
                if (!GlobalShare.mapDataSource.ContainsKey(strds))//如果不存在数据源，删除掉模板节点
                {
                    node.ParentNode.RemoveChild(node);
                    continue;
                }
                string msg = null;
                DataSet ds = GlobalShare.DataCenterClientObj.GetData(Uid, strds, out msg);
          
                if (msg != null || ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0)//如果无法找到数据源，删除掉模板节点
                {
                    node.ParentNode.RemoveChild(node);
                    continue;
                }
                XmlNode pnode = node.ParentNode;
               
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    CMenuItem newmnu = cmnu.Clone() as CMenuItem;
                    List<string> names = DataPointReg.GetExpresses(cmnu.MnuName);
                    if (names != null)
                    {
                        for (int n = 0; n < names.Count; n++)
                        {
                            if (ds.Tables[0].Columns.Contains(names[n]))
                            {
                                newmnu.MnuName = newmnu.MnuName.Replace("{" + names[n] + "}", ds.Tables[0].Rows[i][names[n]].ToString());
                            }
                            else
                            {
                                string strname = newmnu.MnuName;//for debug
                            }
                        }
                    }
                    else
                    {
                        if (ds.Tables[0].Columns.Contains(cmnu.MnuName))
                        {
                            newmnu.MnuName = ds.Tables[0].Rows[i][cmnu.MnuName].ToString();
                        }
                    }
                    if (newmnu.TranDataMapping == null)
                        newmnu.TranDataMapping = new List<DataTranMapping>();
                    if (ds.Tables[0].Columns.Contains(cmnu.Params))
                    {
                        newmnu.Params = ds.Tables[0].Rows[i][cmnu.Params].ToString();
                        
                        if (newmnu.TranDataMapping.Count > 0 && (
                            (GlobalShare.DataPointMappings.ContainsKey(newmnu.TranDataMapping[0].FromDataPoint.Name) &&
                             newmnu.TranDataMapping[0].FromDataPoint.Text == cmnu.Params) ||
                             newmnu.TranDataMapping[0].ToDataPoint == cmnu.Key))
                        {
                            newmnu.TranDataMapping[0].FromDataPoint.Text = newmnu.Params;
                        }
                        else
                        {
                            DataTranMapping map = new DataTranMapping();
                            map.FromDataPoint.Text = newmnu.Params;
                            map.ToDataPoint = cmnu.Key;
                            newmnu.TranDataMapping.Add(map);
                        }

                    }
                    else
                    {
                        DataTranMapping map = new DataTranMapping();
                        map.FromDataPoint.Text = newmnu.Params;
                        map.ToDataPoint = cmnu.Key;
                        newmnu.TranDataMapping.Add(map);
                    }
                    if (ds.Tables[0].Columns.Contains(cmnu.MnuId))
                    {
                        newmnu.MnuId = ds.Tables[0].Rows[i][cmnu.MnuId].ToString();
                    }
                    newmnu.Key = cmnu.Key;
                    newmnu.ReplaceMenu = false;
                    XmlNode newmnunode =  newmnu.ToXml(pnode);
                    if (newmnunode != null)
                    {
                        XmlNodeList subnodes = node.SelectNodes(".//menu");
                        for (int ci = 0; ci < subnodes.Count; ci++)
                        {
                            
                            //newsubmnu.MnuName = ds.Tables[0].Rows[i][cmnu.MnuName].ToString();
                            XmlUtil.AddAttribute(subnodes[ci],"param", newmnu.Params,true);
                            XmlUtil.AddAttribute(subnodes[ci], "id", newmnu.PermId, true);
                            XmlUtil.AddAttribute(subnodes[ci], "key", newmnu.Key, true);
                            if(subnodes[ci].ParentNode == node)
                                newmnunode.AppendChild(subnodes[ci].CloneNode(true));
                        }
                        
                    }
                }
                node.ParentNode.RemoveChild(node);//删除掉模板节点
                continue;



                string ListName = XmlUtil.GetSubNodeText(node, "@srcList");
                string TypeName = XmlUtil.GetSubNodeText(node, "@srcType");
                if (ListName.Length == 0) continue;


                //如果有srcList属性，子节点将列出GlobalShare中对应变量列表中的值
                XmlNodeList snodes = node.SelectNodes("menu");
                foreach (XmlNode subnode in snodes)
                {
                    string subNodeName = XmlUtil.GetSubNodeText(subnode, "@id");
                    if (subNodeName.Substring(0, 1) != "@")
                    {
                        continue;
                    }
                    Type typegs = typeof(GlobalShare);
                    Type typeItem = Type.GetType(TypeName);
                    FieldInfo fi = typegs.GetField(ListName);
                    object objlist = fi.GetValue(null);
                    //List<object> objs = objlist as List<object>;

                    IList objs = objlist as IList;
                    ////if (objlist != null && objlist is IList && (objlist )) 
                    ////{
                    ////    objs = objlist as List<DocumentType>;
                    ////}
                    //List<object> objs = fi as List<object>;


                    for (int i = 0; i < objs.Count; i++)//根据列表变量中值新建节点
                    {
                        Type typeobj = objs[i].GetType();
                        XmlNode newNode = xmldoc.CreateElement("menu");

                        foreach (XmlAttribute att in subnode.Attributes)
                        {
                            XmlAttribute xmlatt = xmldoc.CreateAttribute(att.Name);
                            PropertyInfo datafi = typeobj.GetProperty(att.Value.Replace("@", ""));
                            if (datafi != null)
                            {
                                object obj = datafi.GetValue(objs[i], null);
                                xmlatt.Value = obj == null ? "" : obj.ToString();
                            }
                            else
                            {
                                xmlatt.Value = att.Value;
                            }
                            newNode.Attributes.Append(xmlatt);
                        }
                        // iterator  
                        node.AppendChild(newNode);
                    }
                    node.RemoveChild(subnode);
                    break;
                }
            }
        }

        public List<CMenuItem> GenerateMenus()
        {
            //ReDoMenuXml();
            List<CMenuItem> retList = new List<CMenuItem>();
            XmlNodeList nodes = xmldoc.SelectNodes("/menus/menu");
            for (int i = 0; i < nodes.Count; i++)
            {
                CMenuItem mnu = new CMenuItem(Uid);
                mnu.LoadXml(nodes[i]);
                FillMnu(mnu, nodes[i]);
                retList.Add(mnu);
            }
            return retList;
        }
  
        public static CMenuItem GetMenu(CMenuItem mnu, XmlNode node,string uid)
        {
            if (mnu == null) mnu = new CMenuItem(uid);
            mnu.LoadXml(node);
            return mnu;
            mnu.MnuId = XmlUtil.GetSubNodeText(node, "@id");
            mnu.MnuName = XmlUtil.GetSubNodeText(node, "@name");
            mnu.LinkValue = XmlUtil.GetSubNodeText(node, "@classname");
            mnu.GridSource = XmlUtil.GetSubNodeText(node, "@gridsource");
            if (mnu.GridSource == "")
                mnu.GridSource = XmlUtil.GetSubNodeText(node.ParentNode, "@gridsource");
            mnu.DetailSrouce = XmlUtil.GetSubNodeText(node, "@datasource");
            mnu.Params = XmlUtil.GetSubNodeText(node, "@param");
            mnu.Module = XmlUtil.GetSubNodeText(node, "@module");
            mnu.Screen = XmlUtil.GetSubNodeText(node, "@screen");
            mnu.Target = XmlUtil.GetSubNodeText(node, "@target");
            mnu.Title = XmlUtil.GetSubNodeText(node, "@title");
            mnu.LinkUrl = XmlUtil.GetSubNodeText(node, "@linkurl");
            mnu.isSummary = XmlUtil.GetSubNodeText(node, "@issummary");
            mnu.Key = XmlUtil.GetSubNodeText(node, "@key");
            mnu.PermId = XmlUtil.GetSubNodeText(node, "@perm");
            mnu.FixField = XmlUtil.GetSubNodeText(node, "@fixfield");
            mnu.Expand = XmlUtil.GetSubNodeText(node, "@expand") == "1";
            mnu.ReplaceMenu = XmlUtil.GetSubNodeText(node, "@list") == "1";
            if (!int.TryParse(XmlUtil.GetSubNodeText(node, "@winwidth"), out mnu.WWidth))
            {
                mnu.WWidth = 900;
            }
            if (!int.TryParse(XmlUtil.GetSubNodeText(node, "@winheight"), out mnu.WHeight))
            {
                mnu.WHeight = 720;
            }
            mnu.isextradata = XmlUtil.GetSubNodeText(node, "@isextradata")=="1";
            mnu.extradataassembly = XmlUtil.GetSubNodeText(node, "@extradataassembly");
            mnu.extradataclass = XmlUtil.GetSubNodeText(node, "@extradataclass");
            mnu.extradataconvertconfig = node.SelectSingleNode("extradataconvertconfig");
            mnu.extradatagetconfig = node.SelectSingleNode("extradatagetconfig");
            mnu.extradatatype = XmlUtil.GetSubNodeText(node, "@extradatatype");
            mnu.attatchinfo = node.SelectSingleNode("attatchinfo");
            mnu.GridGroupBy = XmlUtil.GetSubNodeText(node, "@groupby");
            XmlNodeList mapnodes = node.SelectNodes("./Maps/Map");
            if (mapnodes.Count != 0)
            {
                mnu.TranDataMapping = new List<DataTranMapping>();
                foreach (XmlNode mapnode in mapnodes)
                {
                    DataTranMapping dtm = new DataTranMapping();
                    dtm.LoadXml(mapnode);
                    mnu.TranDataMapping.Add(dtm);
                }
            }
            string strtype = XmlUtil.GetSubNodeText(node, "@type");
            switch (strtype)
            {
                case "WebPage":
                    {
                        mnu.linkType = LinkType.WebPage;
                        break;
                    }
                case "Form":
                    {
                        mnu.linkType = LinkType.Form;
                        break;
                    }
                case "Dialog":
                    {
                        mnu.linkType = LinkType.Dialog;
                        break;
                    }
                case "Select":
                    {
                        mnu.linkType = LinkType.Select;
                        break;
                    }
                case "PrintPDF":
                    {
                        mnu.linkType = LinkType.PrintToPDF;
                        break;
                    }
                case "UserControl":
                case "":
                default:
                    {
                        mnu.linkType = LinkType.UserControl;
                        break;
                    }
            }
            return mnu;
        }

        void FillMnu(CMenuItem mnu, XmlNode node)
        {
            mnu.LoadXml(node);// GetMenu(mnu, node,Uid);
            XmlNodeList subnodes = node.SelectNodes("menu");
            for (int i = 0; i < subnodes.Count; i++)
            {
                CMenuItem submnu = new CMenuItem(Uid);
                submnu.LoadXml(subnodes[i]);
                FillMnu(submnu, subnodes[i]);
                mnu.MnuItems.Add(submnu);
            }
        }
    }

}
