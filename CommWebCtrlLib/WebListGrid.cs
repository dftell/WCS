using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Collections;
using WolfInv.Com.CommCtrlLib;
using WolfInv.Com.XPlatformCtrlLib;

namespace WolfInv.Com.CommWebCtrlLib
{
    public class WebListGrid:DataGrid,ICalcGrid
    {
        
        public bool AllowSum = false;

        public WebListGrid():base()
        {


            this._DataGridColumns = new Dictionary<string, WolfInv.Com.CommCtrlLib.DataGridColumn>();
        }

        protected override void OnItemDataBound(DataGridItemEventArgs e)
        {
            for(int i=0;i< this.Columns.Count;i++)// e.Item.Cells.Count ;i++)
            {
                System.Web.UI.Control ctrl = e.Item.Cells[i];
                if(ctrl  is TableCell)
                {
                    
                    TableCell lbl = ctrl as TableCell;
                    if (this.Columns[i] is BoundColumn)
                    {
                        BoundColumn bc = this.Columns[i] as BoundColumn;
                        WolfInv.Com.CommCtrlLib.DataGridColumn dgc = null;
                        bool exitcol = this._DataGridColumns.TryGetValue(bc.DataField, out dgc);
                        if (!exitcol) continue;
                        switch (dgc.DataType)
                        {
                            case "date":
                            case "datetime":
                            case "smalldatetime":
                                {
                                    DateTime dt;
                                    bool isdate = DateTime.TryParse(lbl.Text, out dt);
                                    if (isdate)
                                        lbl.Text = dt.ToShortDateString();
                                    break;
                                }
                            case "text":
                            case "varchar":
                            case "nvarchar":
                            default:
                                {
                                    if (lbl.Text.Length > 50)
                                    {
                                        lbl.Text = lbl.Text.Substring(0, 10) + "..";
                                    }
                                    break;
                                }
                        }
                    }
                }
            }
        }

       



        #region ICalcGrid 成员
        bool _AllowSum;
        bool ICalcGrid.AllowSum
        {
            get
            {
                return _AllowSum;
            }
            set
            {
                _AllowSum = value;
            }
        }

        #endregion

        #region ITag 成员

        public object Tag
        {
            get
            {
                return new object();// return ViewState["GridObj"];
            }
            set
            {
                //ViewState["GridObj"] = value;
            }
        }

        #endregion

        #region ICalcGrid 成员

        Dictionary<string, WolfInv.Com.CommCtrlLib.DataGridColumn> _DataGridColumns;
        public Dictionary<string, WolfInv.Com.CommCtrlLib.DataGridColumn> DataGridColumns
        {
            get
            {
                return _DataGridColumns;
            }
           
        }

        #endregion
    }

  

    public class WebListGridColumnHeader : System.Web.UI.WebControls.DataGridColumn, IPermmsionControl,IDataFieldHeaderColumn
    {
        public string _DataType;
        public bool _NeedSum = false ;
        public string _DataField;
        public string _Text;
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

        public WebListGridColumnHeader():base()
        {
            
        }

        public WebListGridColumnHeader(WolfInv.Com.CommCtrlLib.DataGridColumn ch)
            : base()
        {
            this.HeaderText = ch.Name;
            this.DataField = ch.dpt.Name;
            this.DataType = ch.DataType;
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

        public string DisplayText
        {
            get
            {
                return _Text;
            }
            set
            {
                _Text = value;
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

        public string Text
        {
            get { return HeaderText; }
            set { HeaderText = value; }
        }
        #endregion

        public int Width
        {
            get { return (int)HeaderStyle.Width.Value; }
            set { HeaderStyle.Width  = new Unit(value); }
        }
    }
    

    public class WebListGridItem : DataGridItem
    {
        public WebListGridItem(int index, int datasetid, ListItemType itemtype)
            : base(index, datasetid, itemtype)
        {
        }

       
    }

    public class SumWebListGridItem : WebListGridItem
    {
        public SumWebListGridItem(int index, int datasetid, ListItemType itemtype)
            : base(index, datasetid, itemtype)
        {
        }
    }
}


