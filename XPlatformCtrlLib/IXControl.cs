using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WolfInv.Com.XPlatformCtrlLib
{

    public enum XPlatformDockStyle
    {
        None,
        
        Top,
        Bottom,
        Left,
        Right,
        Fill
    }
    public interface IXDockable
    {
        void SetDock(XPlatformDockStyle dock);
    }

    public interface ITitlable
    {
        string Title { get; set; }
    }

    

    public enum PlatformControlType
    {
        WinForm,
        Web
    }
    public interface IXControl
    {
        PlatformControlType ControlType { get; }
        //string Id { get; set; }

    }

    public interface IXContainerControl:IXDockable
    {
        void Controls_Add(IXControl ctrl);
        void Controls_Clear();
        IXControl CurrMainControl { get; set; }

    }

    public interface IToTopLevel
    {
        void ToTopLevel();
    }


    public interface IXUserControl: IXControl, IXContainerControl, IXDockable, IToTopLevel
    {
        
    }

    public interface IXForm : IXControl, IXContainerControl, IXDockable
    {
        void InitForm(CMenuItem mnu, Icon icon);
        bool ShowIXDialog();
    }
}
