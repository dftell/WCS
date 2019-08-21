using System.Windows.Forms;

namespace WolfInv.Com.XPlatformCtrlLib
{
    public class XWinForm_UserControl : UserControl, IXUserControl
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

        public void SetDock(XPlatformDockStyle dock)
        {
            Dock = (DockStyle)dock;
        }

        public void ToTopLevel()
        {
            BringToFront();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // XWinForm_UserControl
            // 
            this.Name = "XWinForm_UserControl";
            this.Size = new System.Drawing.Size(748, 744);
            this.ResumeLayout(false);

        }
    }
}
