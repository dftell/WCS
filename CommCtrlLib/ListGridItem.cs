using System.Windows.Forms;
namespace WolfInv.Com.CommCtrlLib
{
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


        new ListGridGroupCollection Group;

        new ListGridSubItemCollection SubItems;

        class ListGridGroupCollection : ListGridItem.ListViewSubItemCollection
        {
            public ListGridGroupCollection(ListGridItem lgi) : base(lgi) { }
        }

        class ListGridSubItemCollection : ListGridItem.ListViewSubItemCollection
        {
            public ListGridSubItemCollection(ListGridItem lgi) : base(lgi) { }
        }

        public class ListGridSubItem : ListViewItem.ListViewSubItem
        {
            public ListGridSubItem() : base() { }
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


