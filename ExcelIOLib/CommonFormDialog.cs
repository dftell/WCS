using System;
using System.Windows.Forms;

namespace WolfInv.Com.ExcelIOLib
{
    public class CommonFormDialog :CommonDialog
    {
        Form Form;
        public CommonFormDialog()
        {
            Init();
        }
        public override void Reset()
        {
            
        }

        protected override bool RunDialog(IntPtr hwndOwner)
        {
            return true;
        }

        void Init()
        {
            Form = new Form();
        }
    }
}
