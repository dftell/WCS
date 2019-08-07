using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using WolfInv.Com.MetaDataCenter;
using System.IO;
using XmlProcess;
namespace WolfInv.Com.HtmlExp
{
    public class NoteExpClass
    {
        XmlDocument xmldoc;
        NoteList notelist ;
        XmlNode linknode;
        XmlNode tableconfig;
        List<HtmlTableExp> tabs;
        public NoteConfig Config;
        string ConfigPath;
        public NoteExpClass(string configpath)
        {
            ConfigPath = configpath;
            XmlDocument xmldoc = new XmlDocument();
            try
            {
                xmldoc.Load(configpath);
            }
            catch
            {
                return ;
            }
            tabs = new List<HtmlTableExp>();
            linknode = xmldoc.SelectSingleNode("/root/link");
            XmlNode noteconfig = linknode.SelectSingleNode("list");
            tableconfig = linknode.SelectSingleNode("htmlExp");
            XmlNodeList tablist = tableconfig.SelectNodes("table");
            notelist = new NoteList(noteconfig);
            Config = notelist.Config;
            tabs = new List<HtmlTableExp>();
            foreach (XmlNode tabnode in tablist)
            {
                HtmlTableExp hte = new HtmlTableExp("", tabnode);
                tabs.Add(hte);
            }
        }
        public List<UpdateData> GetDatas()
        {
            List<UpdateData> datas = new List<UpdateData>();
            bool debug = false;
            #region debug
            if (debug)
            {

                DirectoryInfo dr = Directory.GetParent(ConfigPath);
                StreamReader strrd = new StreamReader(File.Open(string.Format(@"{0}\html.txt", dr.Name), FileMode.Open));
                string strret = strrd.ReadToEnd();
                strrd.Close();
                string input = strret;
                UpdateData updata = new UpdateData();
                foreach (HtmlTableExp hte in tabs)
                {
                    string ret = hte.RepaceSign(input);
                    //ret = hte.AddSign(ret);
                    //ret = hte.RemoveSign(ret);
                    input = ret;
                    ret = hte.GetMiddleText(input, hte.config.StartAt, hte.config.EndAt,true);
                    if (ret == null)
                    {
                        continue;
                    }
                    ret = hte.RemoveSign(ret);
                    //
                    ret = hte.RemoveUnNormalAtt(ret);
                    XmlDocument dataxml = new XmlDocument();
                    dataxml.LoadXml(ret);
                    NoteListItem nli = new NoteListItem();
                    updata = nli.ReadHtmlRecord(dataxml,hte.config.DataModelConfig,ref updata);
                    //tabs.Add(hte);
                }
                updata.keydpt = new DataPoint(Config.Key);
                datas.Add(updata);
                return datas;
            }
            #endregion
            //return datas;
      
            string url = XmlUtil.GetSubNodeText(linknode, "@url");
            string strxml = WebReqProcess.GetResult(url);
            StringBuilder stb = new StringBuilder(strxml);
            XmlDocument datalist = new XmlDocument();
            try
            {
                datalist.LoadXml(strxml);
            }
            catch
            {
                throw new Exception(strxml);
                return datas;
            }
            
            
            notelist.ReadNotes(datalist);
            if (notelist.Items.Count == 0) 
            {
                return datas;
            }
            
            
            foreach (NoteListItem nli in notelist.Items.Values)
            {
                
                string htmlurl = string.Format(XmlUtil.GetSubNodeText(tableconfig, "@url"), nli.uid);

                string retHtml = WebReqProcess.GetResult(htmlurl);
                //XmlNodeList tablist = tableconfig.SelectNodes("table");
                string input = retHtml;
                foreach (HtmlTableExp hte in tabs)
                {
                    string ret = hte.RepaceSign(input);
                    //ret = hte.AddSign(ret);
                    ret = ret;
                    ret = hte.RemoveSign(ret);
                    ret = hte.RepaceSign(ret);
                    ret = hte.GetMiddleText(ret, hte.config.StartAt, hte.config.EndAt, true);
                    if (ret == null)
                    {
                        continue;
                    }
                    ret = hte.RemoveInnerSign(ret);
                    XmlDocument dataxml = new XmlDocument();

                    ret = hte.RemoveUnNormalAtt(ret);
                    try
                    {
                        
                        dataxml.LoadXml(ret);
                        nli.NoteData = nli.ReadHtmlRecord(dataxml, hte.config.DataModelConfig, ref nli.NoteData);
                    }
                    catch (Exception ce)
                    {
                        frm_UpdateXml frm = new frm_UpdateXml(ret);
                        if (frm.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
                        {
                            dataxml.LoadXml(frm._Xml);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                nli.NoteData.keydpt = new DataPoint(nli.Owner.Config.Key);
                datas.Add(nli.NoteData);
            }
            return datas;
        }
    }



    public class NoteList
    {
        public NoteConfig Config;
        
        public Dictionary<string,NoteListItem> Items;
        public NoteList(XmlNode config)
        {
            Config = new NoteConfig(config);
            Items = new Dictionary<string, NoteListItem>();
        }

        public void ReadNotes(XmlNode node)
        {
            XmlNodeList nodes = node.SelectNodes("viewentries/viewentry");
            foreach(XmlNode notenode in nodes)
            {
                NoteListItem nli = new NoteListItem(this);
                nli.NoteData = nli.ReadOneRecord(notenode);
                if (!Items.ContainsKey(nli.uid))
                {
                    Items.Add(nli.uid, nli);
                }
            }
        }

     }


    public class NoteListItem
    {
        public UpdateData NoteData;
        public NoteList Owner;
        public string uid;
        public NoteListItem(NoteList list)
        {
            Owner = list;
            NoteData = new UpdateData();
        }

        public NoteListItem()
        {
            NoteData = new UpdateData();
        }

        public UpdateData ReadOneRecord(XmlNode node)
        {
            if (this.Owner == null)
            {
                throw new Exception("还未指定NoteList!你可以调用ReadOneRecord(XmlNode node,NoteConfig Config)来执行！");
            }
            return ReadOneRecord(node,this.Owner.Config);
        }
        public UpdateData  ReadOneRecord(XmlNode node,NoteConfig Config)
        {
            if (node == null)
                return null;
            UpdateData ret = new UpdateData();
            //UpdateItem ui = new UpdateItem();
            uid = XmlUtil.GetSubNodeText(node, "@unid");
            XmlNodeList nodes = node.SelectNodes("viewentry/entrydata");
            foreach (NoteConfigItem nci  in Config.Items.Values)
            {
                XmlNode xmlnode = node.SelectSingleNode(string.Format("entrydata[@name='{0}']",nci.Name));
                if (xmlnode == null)
                {
                    UpdateItem defautui = new UpdateItem(nci.DataPoint,nci.Value);
                    if (!ret.Items.ContainsKey(nci.DataPoint))
                        ret.Items.Add(nci.DataPoint, defautui);
                    continue;
                }
                string val = XmlUtil.GetSubNodeText(xmlnode, ".");
                if (nci.Type == "datetime")
                {
                    if (val.Length >= 8)
                    {
                        val = val.Substring(0, 8);
                        val = val.Insert(4, "-");
                        val = val.Insert(7, "-");
                    }
                }
                UpdateItem ui = new UpdateItem(nci.DataPoint,val);
                if(!ret.Items.ContainsKey(nci.DataPoint))
                    ret.Items.Add(nci.DataPoint,ui);
            }
            
            return ret;
        }

        public UpdateData ReadHtmlRecord(XmlNode node, NoteConfig config, ref UpdateData data)
        {
            if (data == null)
                data = new UpdateData();
            if (node == null || config == null || config.Items.Count == 0)
                return data;
            if (!config.IsList)//非grid
            {
                foreach(string strname in config.Items.Keys)
                {
                    NoteConfigItem nci = config.Items[strname];
                    XmlNode valnode = node.SelectSingleNode(strname);
                    string val = XmlUtil.GetSubNodeText(valnode,".");
                    
                    if (nci.NoNull && val.Trim().Length == 0)
                        continue;
                    if (nci.DataPoint.Trim().Length == 0) continue;
                    UpdateItem ui = new UpdateItem(nci.DataPoint,val);
                    if (!data.Items.ContainsKey(ui.datapoint.Name))
                    {
                        data.Items.Add(ui.datapoint.Name, ui);
                    }
                }
            }
            else //列表处理
            {
                XmlNodeList headernodes = node.SelectNodes("//TR[1]/TD");
                Dictionary<string, int> cols = new Dictionary<string, int>();
                for (int i = 0; i < headernodes.Count; i++)
                {
                    string val = XmlUtil.GetSubNodeText(headernodes[i],".").Trim().Replace(" ","") ;
                    if (config.Items.ContainsKey(val))//如果头行单元格内容匹配到配置项中的name
                    {
                        if (!cols.ContainsKey(val))
                        {
                            cols.Add(val, i+1);
                        }
                    }
                }
                if (cols.Count == 0) return data;
                XmlNodeList datanodes = node.SelectNodes("//TR[position()>1]");
                foreach (XmlNode datanode in datanodes)
                {
                    UpdateData subdata = new UpdateData();
                    bool Err = false;
                    foreach (string strkey in cols.Keys)
                    {
                        int colindex = cols[strkey];
                        XmlNode tdnode = datanode.SelectSingleNode(string.Format("TD[{0}]",colindex));
                        
                        string val = XmlUtil.GetSubNodeText(tdnode, ".").Trim().Replace(" ", "").Replace("\r\n","");
                        if (val.Length == 0 && config.Items[strkey].NoNull)
                        {
                            Err = true;
                            break;
                        }
                        subdata.Items.Add(config.Items[strkey].DataPoint, new UpdateItem(config.Items[strkey].DataPoint, val));
                    }
                    if (Err)
                        continue;
                    subdata.keydpt = new DataPoint(config.Key);
                    data.SubItems.Add(subdata);

                }
                
            }
            return data;
        }
    }

    public class NoteConfig
    {
        public bool IsList;
        public string Key;
        public string DataSource;
        public string GridSource;
        public Dictionary<string, NoteConfigItem> Items;
        public NoteConfig(XmlNode node)
        {
            Items = new Dictionary<string, NoteConfigItem>();
            LoadXml(node);
        }

        public void LoadXml(XmlNode node)
        {
            if (node == null) return;
            IsList = XmlUtil.GetSubNodeText(node, "@list") == "1";
            Key = XmlUtil.GetSubNodeText(node, "@key");
            DataSource = XmlUtil.GetSubNodeText(node, "@datasource");
            GridSource = XmlUtil.GetSubNodeText(node, "@gridsource");
            XmlNodeList nodes = node.SelectNodes("./cols/col");
            for (int i = 0; i < nodes.Count; i++)
            {
                NoteConfigItem nci = new NoteConfigItem(nodes[i]);
                if(!Items.ContainsKey(nci.Name))
                    Items.Add(nci.Name, nci);
            }

        }

    }

    public class NoteConfigItem
    {

        public string Name;
        public string Type;
        public string DataPoint;
        public string Value;
        public bool NoNull;
        public NoteConfigItem()
        {
        }
        public NoteConfigItem(XmlNode node)
        {
            LoadXml(node);
        }

        public void LoadXml(XmlNode node)
        {
            if (node == null) return;
            Name = XmlUtil.GetSubNodeText(node, "@name");
            Type = XmlUtil.GetSubNodeText(node, "@datatype");
            DataPoint = XmlUtil.GetSubNodeText(node, "@datapoint");
            Value = XmlUtil.GetSubNodeText(node, "@value");
            NoNull = XmlUtil.GetSubNodeText(node, "@nonull") == "1";
        }
    }
    
    public class HtmlTableExp
    {
        string strtable;
        public HTMLTableConfig config;
        public HtmlTableExp(string str,XmlNode TableModel)
        {
            strtable = str;
            config = new HTMLTableConfig(TableModel);
        }

        public string RepaceSign(string input)
        {
            string ret = input;
            for (int i = 0; i < this.config.ReplaceItems.Count; i++)
            {
                ret = ret.Replace(this.config.ReplaceItems[i].Target, this.config.ReplaceItems[i].Value);
            }
            ret = ret.Replace("\n", "");
            return ret;
        }

        public string RemoveInnerSign(string input)
        {
            for (int i = 0; i < this.config.InnerRemoveItems.Count; i++)
            {
                string strMid = GetMiddleText(input, config.InnerRemoveItems[i].Target, config.InnerRemoveItems[i].Value, true);
                while (strMid != null)
                {
                    //if (strMid == null) continue;
                    input = input.Replace(strMid, "");
                    strMid = GetMiddleText(input, config.InnerRemoveItems[i].Target, config.InnerRemoveItems[i].Value, true);
                }
            }
            return input;
        }
        public string AddSign(string input)
        {
            string ret = input;
            for (int i = 0; i < this.config.AddItems.Count; i++)
            {
                //增加
                ret = ret.Replace(this.config.ReplaceItems[i].Target, string.Format("{0}{1}",this.config.ReplaceItems[i].Target,this.config.ReplaceItems[i].Value));
                //再替换多余的
                ret = ret.Replace(string.Format("{0}{1}{1}", this.config.ReplaceItems[i].Target, this.config.ReplaceItems[i].Value), string.Format("{0}{1}", this.config.ReplaceItems[i].Target, this.config.ReplaceItems[i].Value));
            }
            return ret;
        }

        public string RemoveSign(string input)
        {
            for (int i = 0; i < this.config.RemoveItems.Count; i++)
            {
                string strMid = GetMiddleText(input, config.RemoveItems[i].Target, config.RemoveItems[i].Value,true);
                while (strMid != null)
                {
                    //if (strMid == null) continue;
                    input = input.Replace(strMid, "");
                    strMid = GetMiddleText(input, config.RemoveItems[i].Target, config.RemoveItems[i].Value, true);
                }
            }
            return input;
        }

        public string GetMiddleText(string input, string strStartAt,string strEndAt,bool IncludeSign)
        {
            string ret = input.ToLower();
            int startat = ret.IndexOf(strStartAt.ToLower());
            if(startat < 0)
                return null;
            int endat = ret.IndexOf(strEndAt.ToLower(), startat + strStartAt.Length);
            if (endat <= startat)
                return null;
            return input.Substring(startat + (IncludeSign ? 0 : strStartAt.Length), endat - startat - (IncludeSign ? (-strEndAt.Length) : strStartAt.Length));
        }

        public string RemoveUnNormalAtt(string input)
        {
            string[] arr = input.Split('=');
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i].StartsWith("'") || arr[i].StartsWith("\""))
                {
                    continue;
                }
                //增加'
                if (i == 0) continue;
                string strend = arr[i].Substring(arr[i].Length - 1);
                int icheck = 0;
                bool check = int.TryParse(strend, out icheck);
                if (check) continue;
                string currexp = arr[i];
                for (int si = 0; si < currexp.Length; si++)
                {
                    if (currexp.Substring(si, 1) == ">" || currexp.Substring(si, 1) == " ")//为空，表示需要加引号
                    {
                        currexp = currexp.Insert(si, "'");
                        currexp = "'" + currexp;
                        arr[i] = currexp;
                        break;
                    }
                }
            }
            return string.Join("=", arr);
        }
    
        
    }


    public class HTMLTableConfig
    {
        public string StartAt;
        public string EndAt;
        public bool IsList;
        public string Key;
        public List<TextProcessItem> ReplaceItems;
        public List<TextProcessItem> AddItems;
        public List<TextProcessItem> RemoveItems;
        public List<TextProcessItem> InnerRemoveItems;
        public NoteConfig DataModelConfig;
        public HTMLTableConfig(XmlNode xmlnode)
        {
            ReplaceItems = new List<TextProcessItem>();
            AddItems = new List<TextProcessItem>();
            RemoveItems = new List<TextProcessItem>();
            LoadXml(xmlnode);
        }

        public void LoadXml(XmlNode node)
       {
            IsList = XmlUtil.GetSubNodeText(node, "@list") == "1";
            Key = XmlUtil.GetSubNodeText(node, "@key");
            StartAt = XmlUtil.GetSubNodeText(node, "@startat");
            EndAt = XmlUtil.GetSubNodeText(node, "@endat");
            GetTextProcessItems(node.SelectSingleNode("replaces"), "rpl", ref ReplaceItems);
            GetTextProcessItems(node.SelectSingleNode("adds"), "add", ref AddItems);
            GetTextProcessItems(node.SelectSingleNode("removes"), "rem", ref RemoveItems);
            GetTextProcessItems(node.SelectSingleNode("innerremoves"), "rem", ref InnerRemoveItems);
            DataModelConfig = new NoteConfig(node);
            DataModelConfig.IsList = this.IsList;
        }

        void GetTextProcessItems(XmlNode node,string flag,ref List<TextProcessItem> items)
        {
            
            if(items == null)
                items = new List<TextProcessItem>();
            if (node == null) return;
            XmlNodeList nodes = node.SelectNodes(string.Format("./{0}", flag));
            for (int i = 0; i < nodes.Count; i++)
            {
                TextProcessItem tpi = new TextProcessItem(nodes[i]);
                items.Add(tpi);
            }

        }

        

    }

    public class TextProcessItem
    {
        public string Target;
        public string Value;

        public TextProcessItem()
        {
        }
        public TextProcessItem(XmlNode node)
        {
            LoadXml(node);
        }

        public void LoadXml(XmlNode node)
        {
            if (node == null) return;
            Target = XmlUtil.GetSubNodeText(node, "@target");
            Value = XmlUtil.GetSubNodeText(node, "@val");
        }
    }

    

}
