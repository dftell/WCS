using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using XmlProcess;
using WolfInv.Com.WCS_Process;// ITMS_Process;
using WolfInv.Com.MetaDataCenter;
using System.Collections;
using System.IO;
using System.Net;
using System.Web;
using Microsoft;
using System.Reflection;
using WolfInv.Com.CommCtrlLib;
using WolfInv.Com.XPlatformCtrlLib;

namespace WolfInv.Com.CommFormCtrlLib
{
    /// <summary>
    /// This class is an implementation of the 'IComparer' interface.
    /// </summary>
    public class ListViewColumnSorter : IComparer
    {
        /// <summary>
        /// Specifies the column to be sorted
        /// </summary>
        private int ColumnToSort;
        /// <summary>
        /// Specifies the order in which to sort (i.e. 'Ascending').
        /// </summary>
        private SortOrder OrderOfSort;
        /// <summary>
        /// Case insensitive comparer object
        /// </summary>
        private CaseInsensitiveComparer ObjectCompare;

        /// <summary>
        /// Class constructor.  Initializes various elements
        /// </summary>
        public ListViewColumnSorter()
        {
            // Initialize the column to '0'
            ColumnToSort = 0;

            // Initialize the sort order to 'none'
            OrderOfSort = SortOrder.None;

            // Initialize the CaseInsensitiveComparer object
            ObjectCompare = new CaseInsensitiveComparer();
        }

        /// <summary>
        /// This method is inherited from the IComparer interface.  It compares the two objects passed using a case insensitive comparison.
        /// </summary>
        /// <param name="x">First object to be compared</param>
        /// <param name="y">Second object to be compared</param>
        /// <returns>The result of the comparison. "0" if equal, negative if 'x' is less than 'y' and positive if 'x' is greater than 'y'</returns>
        public int Compare(object x, object y)
        {
            int compareResult = 0;
            ListGridItem listviewX, listviewY;

            // Cast the objects to be compared to ListGridItem objects
            listviewX = (ListGridItem)x;
            listviewY = (ListGridItem)y;
            if (listviewX is SumListGridItem || listviewY is SumListGridItem)//合计项不参与排列
            {
                return compareResult;
            }
            // Compare the two items

            ////switch()
            ////{
            ////    case "":
            ////    case "":
            ////    {
            if (listviewX.ListView.Columns[ColumnToSort] is ListGridColumnHeader)
            {
                ListGridColumnHeader col = listviewX.ListView.Columns[ColumnToSort] as ListGridColumnHeader;
                
                switch (col.DataType)
                {
                    case "combo":
                    case "datacombo":
                        {
                            ListViewItem lviX = listviewX.ListView.Items[listviewX.Index];
                            ListViewItem lviY = listviewY.ListView.Items[listviewY.Index];
                            compareResult = ObjectCompare.Compare(listviewX.SubItems[ColumnToSort].Text, listviewY.SubItems[ColumnToSort].Text);
                            break;
                        }
                    case "int":
                    case "float":
                    case "money":
                    case "double":
                        {
                            float a;
                            float b;
                            float.TryParse(listviewX.SubItems[ColumnToSort].Text,out a);
                            float.TryParse(listviewY.SubItems[ColumnToSort].Text,out b);
                            compareResult = ObjectCompare.Compare(a,b);
                            break;
                        }
                    case "datetime":
                    case "date":
                    case "smalldatetime":
                        {
                            DateTime a;
                            DateTime b;
                            DateTime.TryParse(listviewX.SubItems[ColumnToSort].Text,out a);
                            DateTime.TryParse(listviewY.SubItems[ColumnToSort].Text, out b);
                            compareResult = ObjectCompare.Compare(a, b);
                            break;
                        }
                    default:
                        {
                            compareResult = ObjectCompare.Compare(listviewX.SubItems[ColumnToSort].Text, listviewY.SubItems[ColumnToSort].Text);
                            break;
                        }
                }
            }
            else
                    compareResult = ObjectCompare.Compare(listviewX.SubItems[ColumnToSort].Text, listviewY.SubItems[ColumnToSort].Text);
            ////        break;
            ////    }
            ////    default :
            ////        {
            ////            break ;
            ////        }
            ////}
            // Calculate correct return value based on object comparison
            if (OrderOfSort == SortOrder.Ascending)
            {
                // Ascending sort is selected, return normal result of compare operation
                return compareResult;
            }
            else if (OrderOfSort == SortOrder.Descending)
            {
                // Descending sort is selected, return negative result of compare operation
                return (-compareResult);
            }
            else
            {
                // Return '0' to indicate they are equal
                return 0;
            }
        }

        /// <summary>
        /// Gets or sets the number of the column to which to apply the sorting operation (Defaults to '0').
        /// </summary>
        public int SortColumn
        {
            set
            {
                ColumnToSort = value;
            }
            get
            {
                return ColumnToSort;
            }
        }

        /// <summary>
        /// Gets or sets the order of sorting to apply (for example, 'Ascending' or 'Descending').
        /// </summary>
        public SortOrder Order
        {
            set
            {
                OrderOfSort = value;
            }
            get
            {
                return OrderOfSort;
            }
        }

    }

    public class WinFormSwitcher : IFrameActionSwitch
    {
        public frm_Model CurrPage;
        public frm_Model LinkPage;
        static Assembly AppDllasmb;
        WinFormHandle FromWebForm;
        WinFormHandle CurrWebForm;
 
        void GetPageObject(BaseFormHandle FromLink, BaseFormHandle CurrForm)
        {
            FromWebForm = FromLink as WinFormHandle;
            CurrWebForm = CurrForm as WinFormHandle;
            CurrPage = FromWebForm.DataFrm as frm_Model;
            LinkPage = CurrWebForm.DataFrm as frm_Model;
        }
        static WinFormSwitcher()
        {
            
            try
            {
                AppDllasmb = Assembly.LoadFrom(string.Format("{0}\\WCS.exe", GlobalShare.AppDllPath));
            }
            catch (Exception ce)
            {
                try
                {
                    AppDllasmb = Assembly.LoadFile(string.Format("{0}\\WCS.exe", GlobalShare.AppDllPath));
                }
                catch (Exception se)
                {
                    throw se;
                }

            }
        }

        public WinFormSwitcher()
        {
        }


        public override bool CreateFrame(IFrameObj Container, BaseFormHandle CurrFormHandle, CMenuItem mnu, ref UpdateData data, ref string msg)
        {
            //throw new Exception("The method or operation is not implemented.");
            return true;
        }

        public override bool CreateWebPage(IFrameObj Container, BaseFormHandle CurrFormHandle, CMenuItem mnu, ref string msg)
        {
            if (Container == null)
            {
                msg = "无法定位容器！";
                return false;
            }
            //(Container as WebPageModel).Response.Redirect(mnu.LinkValue);

            return true;
            //throw new Exception("The method or operation is not implemented.");
        }

        public override bool CreateDialoger(IFrameObj Container, BaseFormHandle CurrFormHandle, CMenuItem mnu, ref UpdateData data, ref string msg)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override bool CreateSelect(IFrameObj Container, BaseFormHandle CurrFormHandle, CMenuItem mnu, ref UpdateData data, ref string msg)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override Assembly GetAssembly()
        {
            return WinFormSwitcher.AppDllasmb;
        }

        public override bool CreateTagFrame(IFrameObj Container, BaseFormHandle CurrFormHandle, CMenuItem mnu, ref UpdateData data, ref string msg)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        
    }

   
}
