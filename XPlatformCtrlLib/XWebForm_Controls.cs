using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WolfInv.Com.XPlatformCtrlLib
{
    public class XWebForm_Control : Control, IXControl
    {
        public PlatformControlType ControlType { get { return PlatformControlType.WinForm; } }

    }

    public class XWebForm_UserControl : UserControl, IXUserControl
    {
        public PlatformControlType ControlType { get { return PlatformControlType.WinForm; } }



        public void Controls_Add(IXControl ctrl)
        {
            Control c = ctrl as Control;
            this.Controls.Add(c);
        }

        public void Controls_Clear()
        {
            this.Controls.Clear();
        }

        public void SetDock(XPlatformDockStyle dock)
        {
            //Dock = (DockStyle)dock;
        }

        public void ToTopLevel()
        {
            //BringToFront();

        }
    }

    public class XWebForm_Form : Page, IXForm
    {
        public PlatformControlType ControlType { get { return PlatformControlType.WinForm; } }

        public void Controls_Add(IXControl ctrl)
        {
            Control c = ctrl as Control;
            this.Controls.Add(c);
        }

        public void Controls_Clear()
        {
            this.Controls.Clear();
        }

        public void InitForm(CMenuItem mnu, Icon icon)
        {
            
            //if (Height == 0 || Width == 0)
            //{
            //    this.Size = new Size(800, 600);
            //}
            //else
            //{
            //    Size = new Size(mnu.WWidth, mnu.WHeight);
            //}
            //Icon = icon;
            //Text = mnu.Title;
            
            //MaximizeBox = false;
            //MinimizeBox = false;
            //FormBorderStyle = FormBorderStyle.FixedSingle;
            //frm.WindowState = FormWindowState.Maximized;
            //frm.StartPosition = FormStartPosition.CenterParent;
            //objInst.AllowClose = false;


            //frm.Cursor = Cursors.WaitCursor;


        }



        public void SetDock(XPlatformDockStyle dock)
        {
            //Dock = (DockStyle)dock;
        }

        public bool ShowIXDialog()
        {
            ////if (ShowDialog(ParentForm) != DialogResult.Yes)
            ////{
            ////    return false;
            ////}
            //////Cursor = Cursors.Default;
            return true;
        }
    }


    public class XWebForm_TextBox : TextBox, IXTextBox
    {
        public PlatformControlType ControlType { get { return PlatformControlType.WinForm; } }

    }

    public class XWebForm_Button : Button, IXButton
    {
        public PlatformControlType ControlType { get { return PlatformControlType.WinForm; } }

    }

    public class XWebForm_Panel : Panel, IXPanel
    {
        public PlatformControlType ControlType { get { return PlatformControlType.WinForm; } }

        public void Controls_Add(IXControl ctrl)
        {
            this.Controls.Add(ctrl as Control);
        }

        public void Controls_Clear()
        {
            this.Controls.Clear();
        }

        public void SetDock(XPlatformDockStyle dock)
        {
            //Dock = (DockStyle)dock;
        }
    }



    public class XWebForm_Label : Label, IXLabel
    {
        public PlatformControlType ControlType { get { return PlatformControlType.WinForm; } }

    }

    public class XWebForm_ListViewGrid : DataGrid, IXListViewGrid
    {
        public PlatformControlType ControlType { get { return PlatformControlType.WinForm; } }

    }

    public class XWebForm_DatePicker : Control, IXDatePicker
    {
        public XWebForm_DatePicker()
        {
            //默认设置成日期
        }

        public PlatformControlType ControlType { get { return PlatformControlType.WinForm; } }

    }

    public class XWebForm_DateTimePicker : Control, IXDateTimePicker
    {
        public XWebForm_DateTimePicker()
        {
            //设置成日期时间
        }
        public PlatformControlType ControlType { get { return PlatformControlType.WinForm; } }

    }

    public class XWebForm_TimePicker : Control, IXTimePicker
    {
        public XWebForm_TimePicker()
        {

        }
        public PlatformControlType ControlType { get { return PlatformControlType.WinForm; } }

    }

    public class XWebForm_ImagePicket : Control, IXImagePicker
    {
        public PlatformControlType ControlType { get { return PlatformControlType.WinForm; } }

    }

    //public abstract class XWinForm_Image : Image,IXImage
    //{

    //}

    public class XWebForm_PictureBox : Control, IXPictureBox
    {
        public PlatformControlType ControlType { get { return PlatformControlType.WinForm; } }

    }

    public class XWebForm_FilePicket : IXFilePicker
    {
        public PlatformControlType ControlType { get { return PlatformControlType.WinForm; } }

    }

    public class XWebForm_Menu : Menu, IXMenu
    {
        public PlatformControlType ControlType { get { return PlatformControlType.WinForm; } }

    }

    public class XWebForm_TreeView : TreeView, IXTreeView
    {
        public PlatformControlType ControlType { get { return PlatformControlType.WinForm; } }

    }
}
