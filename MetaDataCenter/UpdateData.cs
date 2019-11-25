using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using XmlProcess;
using System.Data;
using System.IO;
using System.Linq;
namespace WolfInv.Com.MetaDataCenter
{
    public interface ICalc : IGroupGrid
    {


        bool AllowSum { get; set; }
        
        string SumItems { get; set; }
        
    }

    public interface IGroupGrid
    {
        bool AllowGroup { get; set; }
        string GroupBy { get; set; }


    }
    public class UpdateData:ICloneable,IXml,ICalc
    {
        public Dictionary<string, UpdateItem> Items = new Dictionary<string, UpdateItem>();
        public List<UpdateData> SubItems = new List<UpdateData>();
        public DataPoint keydpt;
        public string keyvalue;
        public bool Updated;
        public DataSet Dataset;
        public bool IsImported;
        public DataRequestType ReqType;

        #region 专门针对有grid的场景，下面三个属性均取自于grid，需要在getupdatedata()方法里赋值
        public bool AllowSum { get; set; }
        public bool AllowGroup { get; set; }
        public string GroupBy { get; set; }
        public string SumItems { get; set; }
        #endregion
        public UpdateData()
        {

        }

        public UpdateData(XmlNode node)
        {
            LoadXml(node);
        }

        public void LoadXml(XmlNode node)
        {
            if (node == null) return;
            this.keydpt  = new DataPoint(XmlUtil.GetSubNodeText(node, "@key"));
            this.keyvalue = XmlUtil.GetSubNodeText(node, "@val");
            this.ReqType = DataRequestType.Update;
            switch (XmlUtil.GetSubNodeText(node, "@type"))
            {
                case "Add":
                    {
                        ReqType = DataRequestType.Add;
                        break;
                    }
                case "Delete":
                    {
                        ReqType = DataRequestType.Delete;
                        break;
                    }
                case "BatchUpdate":
                    {
                        ReqType = DataRequestType.BatchUpdate ;
                        break ;
                    }
                default :
                    {
                        break;
                    }
            }
            XmlNodeList nodes = node.SelectNodes("./i");
            this.Items = new Dictionary<string, UpdateItem>();
            foreach(XmlNode nd in nodes)
            {
                UpdateItem ui = new UpdateItem(nd);
                this.Items.Add(ui.datapoint.Name,ui);
            }
            this.SubItems = new List<UpdateData>();
            XmlNodeList subnode = node.SelectNodes("./subdata/data");
            for (int i = 0; i < subnode.Count; i++)
            {
                UpdateData subdata = new UpdateData(subnode[i]);
                this.SubItems.Add(subdata);
            }
            XmlNode datasetnode = node.SelectSingleNode("./DataSet");
            if (datasetnode != null)
            {
                this.Dataset = new DataSet();
              
                StringReader sr = new StringReader(datasetnode.InnerXml);
                this.Dataset.ReadXml(sr, XmlReadMode.ReadSchema);

                sr.Close();
            }
        }

        public XmlNode ToXml(XmlNode parent)
        {
            XmlDocument xmldoc;
            if (parent == null)
            {
                xmldoc = new XmlDocument();
                xmldoc.LoadXml("<data/>");
                parent = xmldoc.SelectSingleNode("data");
            }
            //xmldoc = parent.OwnerDocument;
            XmlUtil.AddAttribute(parent, "key", this.keydpt == null?"":this.keydpt.Name);
            XmlUtil.AddAttribute(parent, "val", this.keyvalue);
            XmlUtil.AddAttribute(parent, "type", this.ReqType.ToString());
            //XmlNode node = XmlUtil.AddSubNode(parent, "data", true);
            foreach (UpdateItem ui in this.Items.Values)
            {
                ui.ToXml(parent);
            }
            if (this.SubItems != null && this.SubItems.Count > 0)
            {
                XmlNode node = XmlUtil.AddSubNode(parent, "subdata", false);
                for (int i = 0; i < this.SubItems.Count; i++)
                {
                    XmlNode subNode = node.OwnerDocument.CreateElement("data");// XmlUtil.AddSubNode(node, "data");
                    node.AppendChild(subNode);
                    this.SubItems[i].ToXml(subNode);
                }
            }
            if (this.Dataset != null)
            {
                MemoryStream strm = new MemoryStream();
                ////XmlTextWriter writer = new XmlTextWriter(strm,Encoding.UTF8);
                ////writer.Formatting = Formatting.Indented;
                ////writer.Indentation = 4;
                ////writer.IndentChar = ' ';
                ////writer.WriteStartDocument();
                ////this.Dataset.WriteXml(writer,XmlWriteMode.WriteSchema);
                ////writer.Flush();

               StreamWriter sw = new StreamWriter(strm); 
             
               // 用DataSet的WriteXml方法把DataSet写入内存流时，缺少XML文档的声明行，必须先加上 
               sw.WriteLine(@"<?xml version=""1.0"" standalone=""yes""?>");
               this.Dataset.WriteXml(sw, XmlWriteMode.WriteSchema);
               sw.Flush(); 
             

                string xmlstr = Encoding.UTF8.GetString(strm.GetBuffer());
                XmlDocument tmp = new XmlDocument();

                tmp.LoadXml(xmlstr);

                strm.Close();
                sw.Close();
                XmlNode node  = XmlUtil.AddSubNode (parent,"DataSet");
                node.AppendChild(((parent is XmlDocument)?parent as XmlDocument:parent.OwnerDocument).ImportNode(tmp.SelectSingleNode("NewDataSet"),true));
                
            }
            return parent;
        }

        public UpdateData getGroupData(bool Group=true,string strGroupBy=null,string strSum=null,bool getText=false)
        {
            
            if (strGroupBy == null)
                strGroupBy = this.GroupBy;
            if (!Group||strGroupBy==null||strGroupBy.Trim().Length == 0)//不分组
                return this;
            if(strSum == null)
            {
                strSum = this.SumItems;
            }
            //if (this.SubItems == null || this.SubItems.Count < 2)
            //    return this;
            if (strSum == null)
                strSum = this.SumItems;
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml("<root/>");
            UpdateData ret = new UpdateData(this.ToXml(xmldoc.SelectSingleNode("root")));
            string[] grplist = strGroupBy.Split(',');
            Dictionary<string, UpdateData> allgrp = new Dictionary<string, UpdateData>();
            this.SubItems.ForEach(subitem=> {
                List<UpdateItem> grpkeys = new List<UpdateItem>();
                
                grplist.ToList().ForEach(keyitem => {
                    UpdateItem ui = new UpdateItem(keyitem,null);
                    if (subitem.Items.ContainsKey(keyitem))
                    {
                        
                        ui = subitem.Items[keyitem];
                        grpkeys.Add(ui);
                    }
                    else
                    {
                        ui.value = keyitem;
                        ui.text = keyitem;
                        grpkeys.Add(ui);//常数
                    }
                });
                string grpkey = string.Join(",", grpkeys.Select(a => {
                    string val= getText? a.text: a.value;
                    if(string.IsNullOrEmpty(val))
                    {
                        val = a.value;
                    }
                    return val;
                }));//必须要判断
                if (!allgrp.ContainsKey(grpkey))
                {
                    allgrp.Add(grpkey, new UpdateData());
                }
                allgrp[grpkey].keydpt = grpkeys[0].datapoint;//只有一个数据点，可能需要多个呀，以后再说????????????????????????????????????????
                allgrp[grpkey].keyvalue = grpkey;

                List<string> allkeys = grpkeys.Select(a => a.datapoint.Name).ToList();
                this.Items.Values.ToList().ForEach(pitem => { //将顶级信息下传到子节点

                    if (!allkeys.Contains(pitem.datapoint.Name))
                    {
                        grpkeys.Add( pitem);
                    }
                });
                
                
                grpkeys.ForEach(keyitem =>
                {
                    if(!allgrp[grpkey].Items.ContainsKey(keyitem.datapoint.Name))
                        allgrp[grpkey].Items.Add(keyitem.datapoint.Name,keyitem);
                });
                
                allgrp[grpkey].SubItems.Add(subitem);
            });
            if(SumItems!= null&& SumItems.Trim().Length > 0)
            {
                string[] sums = SumItems.Split(',');
                allgrp.Values.ToList().ForEach(grp=> { //对合并计算项加入items,值为和
                    sums.ToList().ForEach(si =>
                    {
                        UpdateItem ui = new UpdateItem(si,null);
                        if (grp.Items.ContainsKey(si))
                        {
                            ui = grp.Items[si];
                        }

                        ui.value = grp.SubItems.Sum(sui => 
                            {
                                UpdateItem ssui = sui.Items[si];
                                string intval = ssui.value;
                                long retlng = 0;
                                long.TryParse(intval, out retlng);
                                return retlng;

                            }).ToString();
                        ui.text = grp.SubItems.Sum(sui =>
                        {
                            UpdateItem ssui = sui.Items[si];
                            string intval = ssui.text;
                            long retlng = 0;
                            long.TryParse(intval, out retlng);
                            return retlng;

                        }).ToString();
                    });

                });
            }
            ret.SubItems.Clear();
            allgrp.Values.ToList().ForEach(
                grp=> {
                    ret.SubItems.Add(grp);
                });
            return ret;
        }

        #region ICloneable 成员

        public object Clone()
        {
            UpdateData ret = new UpdateData();
            ret.LoadXml(this.ToXml(null));
            return ret;
            //throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
