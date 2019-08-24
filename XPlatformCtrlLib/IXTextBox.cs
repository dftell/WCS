using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WolfInv.Com.XPlatformCtrlLib
{
    public interface IXTextBox: IXControl
    {
    }

    public interface IXButton : IXControl
    {

    }

    public interface IXPanel : IXControl, IXContainerControl
    {
        bool InForm { get; set; }
    }

    

    public interface IXLabel : IXControl
    {
        string Text { get; set; }
    }

    public interface IXListViewGrid : IXControl
    {

    }

    public interface IXDatePicker : IXControl
    {

    }

    public interface IXDateTimePicker : IXControl
    {

    }

    public interface IXTimePicker : IXControl
    {

    }

    public interface IXImagePicker : IXControl
    {

    }

    public interface IXImage : IXControl
    {

    }

    public interface IXPictureBox : IXControl
    {

    }

    public interface IXFilePicker : IXControl
    {

    }

    public interface IXMenu : IXControl
    {

    }

    public interface IXTreeView: IXControl
    {

    }
}
