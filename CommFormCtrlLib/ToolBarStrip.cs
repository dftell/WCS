using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using WolfInv.Com.CommCtrlLib;
namespace WolfInv.Com.CommFormCtrlLib
{
    public class ToolBarStrip:ToolStrip,ITag
    {
        public ToolBarStrip():base()
        {
        }
    }

    public class ToolBarStripItem : ToolStripItem, ITag
    {

    }

    public class ToolBarStripButton : ToolBarStripItem
    {
        public ToolBarStripButton() : base() { }
    }

    public class ToolBarStripSeparator : ToolBarStripItem
    {
        public ToolBarStripSeparator() : base() { }
    }

    public class ToolBarStripLabel : ToolBarStripItem
    {
        public ToolBarStripLabel() : base() { }
    }
    public class ToolStripTextBoxD : ToolBarStripItem
    {
        public ToolStripTextBoxD() : base() { }
    }
    public class ToolBarStripComobox : ToolBarStripItem
    {
        public ToolBarStripComobox() : base() { }
    }
}
