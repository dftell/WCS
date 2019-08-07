using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using WolfInv.Com.CommCtrlLib;
using System.Linq;
using WolfInv.Com.XPlatformCtrlLib;
namespace WolfInv.Com.CommCtrlLib
{

    public class ListGrid : ListView, ICalcGrid
    {
        bool _AllowSum = false;
        public bool AllowSum
        {
            get { return _AllowSum; }
            set { _AllowSum = value; }
        }
        public ListGrid() : base()
        {
            Items = new ListGridItemCollection(this);
            Columns = new ListGridColumnHeaderCollection(this);
            //SelectedItems = new SelectedListViewItemCollection(this);
            CheckedItems = new CheckedListViewItemCollection(this);
            _DataGridColumns = new Dictionary<string, DataGridColumn>();
        }


        List<ListGridItem> _Items = new List<ListGridItem>();
        ListGridItemCollection Items;

        public SelectedListViewItemCollection SelectedItems
        {
            get
            {
                SelectedListViewItemCollection slvis = new SelectedListViewItemCollection(this, base.SelectedItems);
                return slvis;
            }

        }


        public ListGridColumnHeaderCollection Columns;



        public new CheckedListViewItemCollection CheckedItems;

        //new SelectedListViewItemCollection SelectedItems;


        public class ListGridColumnHeaderCollection : ListView.ColumnHeaderCollection //, IDataFieldHeaderColumnCollection
        {
            public ListGridColumnHeaderCollection(ListView listview)
                : base(listview)
            {

            }
        }

        public class ListGridItemCollection : ListView.ListViewItemCollection
        {
            public ListGridItemCollection(ListGrid lv)
                : base(lv)
            {

            }
        }

        public class SelectedListViewItemCollection : ListView.SelectedListViewItemCollection
        {
            public List<ListGridItem> mylist = new List<ListGridItem>();
            public SelectedListViewItemCollection(ListGrid lg, ListView.SelectedListViewItemCollection blist)
                : base(lg)
            {
                mylist = new List<ListGridItem>();
                foreach (ListViewItem lvi in blist)
                {
                    mylist.Add(new ListGridItem(lvi));
                }
            }

            ListGridItem this[int index]
            {
                get
                {
                    if (index < 0)
                        return null;
                    if (mylist == null)
                        return null;
                    if (mylist.Count < index + 1)
                        return null;
                    return mylist[index] as ListGridItem;
                }
            }
        }

        public class SelectedIndexCollection : ListView.SelectedIndexCollection
        {
            public SelectedIndexCollection(ListGrid lv)
                : base(lv)
            {
            }
        }

        public class CheckedListViewItemCollection : ListView.CheckedListViewItemCollection
        {
            public CheckedListViewItemCollection(ListGrid lg) : base(lg) { }
        }

        public class CheckedIndexCollection : ListView.CheckedIndexCollection
        {
            public CheckedIndexCollection(ListGrid lv)
                : base(lv)
            {
            }
        }

        #region ICalcGrid 成员


        ////public new ListGridColumnHeaderCollection Columns
        ////{
        ////    get
        ////    {
        ////        return this._Columns;
        ////    }
        ////    set
        ////    {
        ////        _Columns = value as ListGridColumnHeaderCollection;
        ////    }
        ////}

        #endregion

        protected override void OnItemChecked(ItemCheckedEventArgs e)
        {

            base.OnItemChecked(e);

            if (e.Item is SumListGridItem)
            {
                bool status = e.Item.Checked;
                for (int i = 0; i < this.Items.Count; i++)
                {
                    if (this.Items[i] is SumListGridItem) continue;
                    this.Items[i].Checked = status;
                }
            }
        }



        #region ICalcGrid 成员

        Dictionary<string, DataGridColumn> _DataGridColumns;
        public Dictionary<string, DataGridColumn> DataGridColumns
        {
            get
            {
                return _DataGridColumns;
            }
        }

        #endregion
    }

    public class ListGridColumnHeader : ColumnHeader, IPermmsionControl, IDataFieldHeaderColumn
    {

        public string _DataField;
        public string _DataType;
        public string _Text;
        public bool Visable = true;
        public bool _NeedSum = false;
        #region IPermmsionControl 成员
        string _PermId;
        public string PermId
        {
            get
            {
                return _PermId;
            }
            set
            {
                _PermId = value;
            }
        }

        #endregion

        public ListGridColumnHeader() : base()
        {
        }

        public ListGridColumnHeader(ColumnHeader ch) : base()
        {
            this.Text = ch.Text;
            this.Name = ch.Name;

        }

        #region IDataFieldHeaderColumn 成员

        public string DataType
        {
            get
            {
                return _DataType;
            }
            set
            {
                _DataType = value;
            }
        }

        public string DataField
        {
            get
            {
                return _DataField;
            }
            set
            {
                _DataField = value;
            }
        }

        bool IDataFieldHeaderColumn.NeedSum
        {
            get
            {
                return _NeedSum;
            }
            set
            {
                _NeedSum = value;
            }
        }

        #endregion
    }


    public class ListGridItem : ListViewItem
    {
        protected ListViewItem realLvi = null;
        public ListGridItem(ListViewItem lvi)
            : base()
        {
            realLvi = lvi;
        }

        public ListGridItem()
            : base()
        {
        }

        public ListGridItem(string[] items) : base(items)
        {
            SubItems = new ListGridSubItemCollection(this);
        }

        public ListGridItem(object[] items) : base()
        {
            SubItems = new ListGridSubItemCollection(this);
            string[] inputs = new string[items.Length];
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] != null)
                    inputs[i] = items[i].ToString();
                else
                    inputs[i] = "";
                this.SubItems[i].Text = inputs[i];
            }

        }




        ListGridSubItemCollection SubItems;

        class ListGridSubItemCollection : ListGridItem.ListViewSubItemCollection
        {
            public ListGridSubItemCollection(ListGridItem lgi) : base(lgi) { }
        }

        public class ListGridSubItem : ListViewItem.ListViewSubItem
        {
            public ListGridSubItem() : base() { }
        }
    }

    public class SumListGridItem : ListGridItem
    {
        public SumListGridItem(string[] items)
            : base(items)
        {


        }

    }
    ////public class ListView
    ////{
    ////    public class ColumnHeaderCollection
    ////    {

    ////    }

    ////    public class ListViewItemCollection
    ////    {

    ////    }

    ////    public class SelectedListViewItemCollection
    ////    {

    ////    }


    ////    public class SelectedIndexCollection
    ////    {

    ////    }

    ////    public class CheckedListViewItemCollection
    ////    {

    ////    }

    ////    public class CheckedIndexCollection
    ////    {

    ////    }

    ////}


    //public class ColumnHeader
    //{
    //}

    ////public class ListViewItem
    ////{
    ////    public class ListViewSubItemCollection
    ////    {
    ////    }

    ////    public class ListViewSubItem
    ////    {
    ////    }
    ////}

    public class BaseListGrid : ListView
    {
        public bool AllowSum = false;
        public BaseListGrid()
            : base()
        {
            Items = new ListGridItemCollection(this);
            Columns = new ListGridColumnHeaderCollection(this);
            SelectedItems = new SelectedListViewItemCollection(this);
            CheckedItems = new CheckedListViewItemCollection(this);
        }

        List<ListGridItem> _Items = new List<ListGridItem>();
        ListGridItemCollection Items;




        public ListGridColumnHeaderCollection Columns;



        public new CheckedListViewItemCollection CheckedItems;

        public new SelectedListViewItemCollection SelectedItems;


        public class ListGridColumnHeaderCollection : ListView.ColumnHeaderCollection
        {
            public ListGridColumnHeaderCollection(ListView listview)
                : base(listview)
            {

            }
        }

        public class ListGridItemCollection : ListView.ListViewItemCollection
        {
            public ListGridItemCollection(BaseListGrid lv)
                : base(lv)
            {

            }
        }

        public class SelectedListViewItemCollection : ListView.SelectedListViewItemCollection
        {
            public SelectedListViewItemCollection(BaseListGrid lg)
                : base(lg)
            { }

            public ListGridItem this[int index]
            {
                get
                {
                    return base[index] as ListGridItem;
                }
            }
        }

        public class SelectedIndexCollection : ListView.SelectedIndexCollection
        {
            public SelectedIndexCollection(BaseListGrid lv)
                : base(lv)
            {
            }
        }

        public class CheckedListViewItemCollection : ListView.CheckedListViewItemCollection
        {
            public CheckedListViewItemCollection(BaseListGrid lg) : base(lg) { }
        }

        public class CheckedIndexCollection : ListView.CheckedIndexCollection
        {
            public CheckedIndexCollection(BaseListGrid lv)
                : base(lv)
            {
            }
        }
    }

    ////public class ListGridColumnHeader : ColumnHeader, IPermmsionControl
    ////{

    ////    public string DataField;
    ////    public string DataType;
    ////    public bool Visable = true;
    ////    public bool NeedSum = false;
    ////    #region IPermmsionControl 成员
    ////    string _PermId;
    ////    public string PermId
    ////    {
    ////        get
    ////        {
    ////            return _PermId;
    ////        }
    ////        set
    ////        {
    ////            _PermId = value;
    ////        }
    ////    }

    ////    #endregion

    ////    public ListGridColumnHeader() : base()
    ////    {
    ////    }

    ////    public ListGridColumnHeader(ColumnHeader ch) : base()
    ////    {
    ////        this.Text = ch.Text;
    ////        this.Name = ch.Name;

    ////    }
    ////}


    ////public class ListGridItem : ListViewItem
    ////{
    ////    public ListGridItem()
    ////        : base()
    ////    {
    ////    }

    ////    public ListGridItem(string[] items):base(items)
    ////    {
    ////        SubItems = new ListGridSubItemCollection(this);
    ////    }

    ////    public ListGridItem(object[] items):base()
    ////    {
    ////        SubItems = new ListGridSubItemCollection(this);
    ////        string[] inputs = new string[items.Length];
    ////        for (int i = 0; i < items.Length; i++)
    ////        {
    ////            if (items[i] != null)
    ////                inputs[i] = items[i].ToString();
    ////            else
    ////                inputs[i] = "";
    ////            this.SubItems[i].Text  = inputs[i];
    ////        }

    ////    }




    ////    public ListGridSubItemCollection SubItems;

    ////    public class ListGridSubItemCollection : ListViewItem.ListViewSubItemCollection
    ////    {
    ////        public ListGridSubItemCollection(ListGridItem lgi) : base(lgi) { }
    ////    }

    ////    public class ListGridSubItem : ListViewItem.ListViewSubItem
    ////    {
    ////        public ListGridSubItem() : base() { }
    ////    }
    ////}

    ////public class SumListGridItem : ListGridItem
    ////{
    ////    public SumListGridItem(string[] items)
    ////        : base(items)
    ////    {


    ////    }
    ////}

}


