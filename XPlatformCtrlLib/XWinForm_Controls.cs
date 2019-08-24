using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WolfInv.Com.XPlatformCtrlLib;
using System.Windows.Forms;
using System.Drawing;

namespace WolfInv.Com.XPlatformCtrlLib
{


    public class XWinForm_Control : Control, IXControl
    {
        public PlatformControlType ControlType { get { return PlatformControlType.WinForm; } }

    }
    
    public class XWinForm_Form : Form, IXForm
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
        public IXControl CurrMainControl { get; set; }
        public void InitForm(CMenuItem mnu,Icon icon)
        {
            if (Height == 0 || Width == 0)
            {
                this.Size = new Size(800, 600);
            }
            else
            {
                Size = new Size(mnu.WWidth, mnu.WHeight);
            }
            Icon = icon;
            Text = mnu.Title;
            MaximizeBox = false;
            MinimizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            //frm.WindowState = FormWindowState.Maximized;
            //frm.StartPosition = FormStartPosition.CenterParent;
            //objInst.AllowClose = false;
            
            
            //frm.Cursor = Cursors.WaitCursor;

            
        }

        

        public void SetDock(XPlatformDockStyle dock)
        {
            Dock = (DockStyle)dock;
        }

        public bool ShowIXDialog()
        {
            if (ShowDialog(ParentForm) != DialogResult.Yes)
            {
                return false;
            }
            Cursor = Cursors.Default;
            return true;
        }
    }


    public class XWinForm_TextBox:TextBox,IXTextBox
    {
        public PlatformControlType ControlType { get { return PlatformControlType.WinForm; } }

    }

    public class XWinForm_Button : Button,IXButton
    {
        public PlatformControlType ControlType { get { return PlatformControlType.WinForm; } }

    }

    public class XWinForm_Panel : Panel,IXPanel
    {
        public PlatformControlType ControlType { get { return PlatformControlType.WinForm; } }

        public void Controls_Add(IXControl ctrl)
        {
            this.Controls.Add(ctrl as Control);
        }
        public IXControl CurrMainControl { get; set; }
        public bool InForm { get; set; }

        public void Controls_Clear()
        {
            this.Controls.Clear();
        }

        public void SetDock(XPlatformDockStyle dock)
        {
            Dock = (DockStyle)dock;
        }
    }

    

    public class XWinForm_Label : Label, IXLabel
    {
        public PlatformControlType ControlType { get { return PlatformControlType.WinForm; } }

    }

    public class XWinForm_ListViewGrid : ListView,IXListViewGrid
    {
        public PlatformControlType ControlType { get { return PlatformControlType.WinForm; } }

    }

    public class XWinForm_DatePicker :DateTimePicker, IXDatePicker
    {
        public XWinForm_DatePicker()
        {
            //默认设置成日期
        }

        public PlatformControlType ControlType { get { return PlatformControlType.WinForm; } }

    }

    public class XWinForm_DateTimePicker : DateTimePicker,IXDateTimePicker
    {
        public XWinForm_DateTimePicker()
        {
            //设置成日期时间
        }
        public PlatformControlType ControlType { get { return PlatformControlType.WinForm; } }

    }

    public class XWinForm_TimePicker : DateTimePicker,IXTimePicker
    {
        public XWinForm_TimePicker()
        {

        }
        public PlatformControlType ControlType { get { return PlatformControlType.WinForm; } }

    }

    public class XWinForm_ImagePicket : IXImagePicker
    {
        public PlatformControlType ControlType { get { return PlatformControlType.WinForm; } }

    }

    //public abstract class XWinForm_Image : Image,IXImage
    //{

    //}

    public class XWinForm_PictureBox : PictureBox, IXPictureBox
    {
        public PlatformControlType ControlType { get { return PlatformControlType.WinForm; } }

    }

    public class XWinForm_FilePicket : IXFilePicker
    {
        public PlatformControlType ControlType { get { return PlatformControlType.WinForm; } }

    }

    public class XWinForm_Menu : MainMenu,IXMenu
    {
        public PlatformControlType ControlType { get { return PlatformControlType.WinForm; } }

    }

    public class XWinForm_TreeView :TreeView,IXTreeView
    {
        public PlatformControlType ControlType { get { return PlatformControlType.WinForm; } }

    }
}
