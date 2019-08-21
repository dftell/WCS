using System;
using System.Windows.Forms;
namespace WolfInv.Com.CommCtrlLib
{
    public class AutoCalcTextItem : TextBox
    {
        public string TargetField;

        public event AutoCalcEventHandler TextChanged;

        protected override void OnTextChanged(EventArgs e)
        {
            this.TextChanged(new AutoResponseEventArgs(this.TargetField));
        }
    }

    
}
