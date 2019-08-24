using System.Collections.Generic;
using System.Windows.Forms;
namespace WolfInv.Com.CommCtrlLib
{
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


