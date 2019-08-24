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
    public interface ICalcGrid : ITag, ICalc
    {



        Dictionary<string, CommCtrlLib.DataGridColumn> DataGridColumns { get; }
        //IDataFieldHeaderColumnCollection Columns { get;set;}
        //IDataItemColumnCollection Items { get;set;}


    }

    public interface ITag
    {
        Object Tag { get;set;}
    }
    
    public interface IGroupItem
    {
        GridItemGroup Group { get; set; }
    }
    public class GridItemGroup
    {
        public GridItemGroup()
        {
            Items = new List<GridRow>();
        }

        public BaseGrid Grid { get; set; }
        public List<GridRow> Items { get; set; }
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

    public abstract class BaseGrid:ICalc
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
        public bool AllowSum { get; set; }
        public bool AllowGroup { get; set; }
        public string GroupBy { get; set; }
        public string SumItems { get; set; }

        public virtual void FillGrid(XmlNode cmbNode)
        {

            
            //grid列处理
            if(XmlUtil.GetSubNodeText(cmbNode, "@sum") != null)
                listViewObj.AllowSum = XmlUtil.GetSubNodeText(cmbNode, "@sum") == "1";
            if (XmlUtil.GetSubNodeText(cmbNode, "@allowgroup") != null)
                listViewObj.AllowGroup = XmlUtil.GetSubNodeText(cmbNode, "@allowgroup") == "1";
            listViewObj.GroupBy = XmlUtil.GetSubNodeText(cmbNode, "@groupby");
            listViewObj.SumItems = XmlUtil.GetSubNodeText(cmbNode, "@sumitems");
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

    public class GridCell
    {
        public string value;
        public string text;
        public bool Updated;
        public bool isKey;
        public DataGridColumn OwnerColumn { get; set; }
    }

  
}
