using System.Data;
using System.Text;
using System.Windows.Forms;

namespace WolfInv.Com.CommCtrlLib
{
    public partial class SubForm : Form
    {
        public SubForm()
        {
            InitializeComponent();
            this.ShowIcon = false;
            this.Text = "";
            this.WindowState = FormWindowState.Maximized;
            //this.MinimizeBox = false;
            //this.MaximizeBox = false;
            this.MdiParent = FrameSwitch.ParentForm;
            this.Icon = null;
            
        }
    }

}