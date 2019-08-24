using System.Windows.Forms;
using WolfInv.Com.XPlatformCtrlLib;
namespace WolfInv.Com.CommCtrlLib
{
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


