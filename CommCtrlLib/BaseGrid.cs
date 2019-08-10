using System;
using System.Collections.Generic;
using System.Text;
using WolfInv.Com.MetaDataCenter;
using System.Xml;
using XmlProcess;
using System.Collections;
using WolfInv.Com.XPlatformCtrlLib;

namespace WolfInv.Com.CommCtrlLib
{
    public interface ITag
    {
        Object Tag { get;set;}
    }
    public interface ICalcGrid:ITag 
    {
        bool AllowSum { get;set; }
        Dictionary<string, CommCtrlLib.DataGridColumn> DataGridColumns { get;}
        //IDataFieldHeaderColumnCollection Columns { get;set;}
        //IDataItemColumnCollection Items { get;set;}
        

    }

    public interface IDataFieldHeaderColumn
    {
        string DataType { get;set;}
        string DataField { get;set;}
        string Text { get;set;}
        int Width { get;set;}
        bool NeedSum { get;set;}
    }
    public class HeaderBuilder
    {
        public IDataFieldHeaderColumn HeaderColumn;
        HeaderBuilder()
        {

        }
    }

    public interface IDataFieldHeaderColumnCollection : ICollection, IEnumerable
    {
    }

    ////public interface IDataItemColumnCollection : ICollection, IEnumerator
    ////{
    ////}

    public abstract class BaseGrid 
    {
        public IUserData frmhandle;


        protected BaseGrid(IUserData handle)
        {
            frmhandle = handle;
        }

        public List<DataGridColumn> Columns = new List<DataGridColumn>();

        Dictionary<string, DataGridColumn> _MapColumns;

        public List<GridRow> Items = new List<GridRow>();

        public Dictionary<string, DataGridColumn> MapColumns
        {
            get
            {
                if (_MapColumns == null)
                {
                    _MapColumns = new Dictionary<string, DataGridColumn>();
                    foreach (DataGridColumn dgc in this.Columns)
                    {
                        if (!_MapColumns.ContainsKey(dgc.DataField))
                        {
                            _MapColumns.Add(dgc.DataField, dgc);
                        }
                    }
                }
                return _MapColumns;
            }
        }

        public CMenuItem ItemDbClickedMenu;

        public Dictionary<string, ViewItem> ViewList = new Dictionary<string, ViewItem>();

        public ICalcGrid listViewObj ;

 
        
        public virtual void FillGrid(XmlNode cmbNode)
        {


            //grid列处理
            if(XmlUtil.GetSubNodeText(cmbNode, "@sum") != null)
                listViewObj.AllowSum = XmlUtil.GetSubNodeText(cmbNode, "@sum") == "1";

            XmlNodeList nodes = cmbNode.SelectNodes("cols/f");
            if (nodes.Count > 0)
            {
                //listobj.Columns = new ListGrid.ListGridColumnHeaderCollection(this.listViewObj);
                ClearItems();
                
                foreach (XmlNode node in nodes)
                {
                    ViewItem vi = new ViewItem();
                    vi.LoadXml(node);
                    
                    IDataFieldHeaderColumn ch = GetHeaderColumn();
                    ch.Text = vi.Text;
                    ch.Width = vi.Width;
                    if (!vi.Perm)
                    {
                        ch.Width = 0;
                    }

                    DataGridColumn dgc = new DataGridColumn(this);
                    dgc.LoadXml(node);

                    dgc.Text = ch.Text;
                    dgc.Width = ch.Width;
                    dgc.DataField = vi.Name;


                    ch.DataType = dgc.DataType;
                    ch.NeedSum = dgc.Sum;
                    ch.DataField = dgc.dpt.Name;
                    AddColumnItem(ch);
                    this.Columns.Add(dgc);

                    if (ViewList.ContainsKey(vi.Name))
                    {
                        continue;
                    }
                    ViewList.Add(vi.Name, vi);
                    if(!listViewObj.DataGridColumns.ContainsKey(dgc.dpt.Name))
                        listViewObj.DataGridColumns.Add(dgc.dpt.Name, dgc);
                }
                
                //AcceptRejectRule
            }
            //事件初始化
            XmlNodeList evtnodes = cmbNode.SelectNodes("action/evt");
            if (evtnodes.Count > 0)
            {
                foreach (XmlNode node in evtnodes)
                {
                    CMenuItem mnu = MenuProcess.GetMenu(null, node,frmhandle.strUid);
                    string strEvtName = XmlUtil.GetSubNodeText(node, "@id");
                    //'mnu = CMenuItem.
                    switch (strEvtName)
                    {
                        case "dbclick":
                            {
                                this.ItemDbClickedMenu = mnu;
                                //this.listView1.DoubleClick += new EventHandler(listView1_DoubleClick);
                                break;
                            }

                    }
                }
            }
            listViewObj.Tag = this;
        }

        protected abstract IDataFieldHeaderColumn GetHeaderColumn();

        protected abstract void ClearItems();

        protected abstract void AddColumnItem(IDataFieldHeaderColumn ch);
    }

    public class GridRow
    {
        public Dictionary<string, GridCell> Items = new Dictionary<string, GridCell>();
        public ItemValue ItemValues;
        public bool Updated;
        public BaseGrid OwnerGrid;
        /// <summary>
        /// 该变量用于外部数据
        /// </summary>
        public UpdateData ExtraSourceData;

        public UpdateData ToUpdateData()
        {
            UpdateData data = new UpdateData();
            return ToUpdateData(ref data);
        }

        public UpdateData ToUpdateData(ref UpdateData OrgData)
        {
            if (OrgData == null)
            {
                OrgData = new UpdateData();
            }
            UpdateData ret = OrgData;
            foreach (string strkey in this.Items.Keys)
            {
                UpdateItem ui = new UpdateItem(strkey, this.Items[strkey].value);
                ret.Items.Add(strkey, ui);
            }
            return ret;
        }
    }

    public class GridCell
    {
        public string value;
        public string text;
        public bool Updated;

    }

    public class DataGridColumn : ViewItem,IXml
    {
        public BaseGrid Owner;
        ////public string Text;
        public DataGridColumn(BaseGrid grid)
        {
            Owner = grid;
        }
        public string DataField
        {
            get { return this.dpt.Name; }
            set { this.dpt = new DataPoint(value); }
        }
        public bool Hide
        {
            get { return !this.Visable; }
            set { this.Visable = value; }
        }
        ////public int Width;
        ////public bool Hide;
        ////public int Index;
        ////public bool IsKeyValue;
        ////public bool IsKeyText;
    }

    public class ViewItem : DataPoint,IXml
    {
        public DataPoint dpt;
        public bool Visable = true;
        public bool IsKeyValue;
        public bool IsKeyText;
        public int Index;
        public bool Sum;
        public bool Perm = true;

        public virtual void LoadXml(XmlNode node)
        {
            //base.LoadXml(node);
            this.Name = XmlUtil.GetSubNodeText(node, "@i");
            this.Text = XmlUtil.GetSubNodeText(node, "@text");
            if (this.Text == "")
            {
                this.Text = XmlUtil.GetSubNodeText(node, "@udlbl");
            }
            int.TryParse(XmlUtil.GetSubNodeText(node, "@width"), out this.Width);
            this.DataType = XmlUtil.GetSubNodeText(node, "@type");
            this.ComboName = XmlUtil.GetSubNodeText(node, "@combo");
            this.Visable = !(XmlUtil.GetSubNodeText(node, "@hide") == "1");
            this.IsKeyText = XmlUtil.GetSubNodeText(node, "@keytext") == "1";
            this.IsKeyValue = XmlUtil.GetSubNodeText(node, "@keyvalue") == "1";
            this.Sum = XmlUtil.GetSubNodeText(node, "@sum") == "1";
            string strIdx = XmlUtil.GetSubNodeText(node, "@index");
            Perm = !(XmlUtil.GetSubNodeText(node, "@perm") == "0");
            if (strIdx == "")
            {
                //this.Index = int.Parse(node.SelectSingleNode("position(.)").Value);
            }
            else
            {
                int.TryParse(strIdx, out this.Index);
            }
        }

        public void GetItem(DataPoint dp)
        {
            dpt = dp;
            this.Name = dp.Name;
            this.Text = dp.Text;
            this.Width = dp.Width;
            this.ComboName = dp.ComboName;
            this.DataType = dp.DataType;
        }
    }

    public class CellViewItem : ViewItem
    {
        public bool multiline;
        public int height;
        public override void LoadXml(XmlNode node)
        {
            base.LoadXml(node);
            this.dpt = new DataPoint(XmlUtil.GetSubNodeText(node, "@f"));
        }
    }

  
}
