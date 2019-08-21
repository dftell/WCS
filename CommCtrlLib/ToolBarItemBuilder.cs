using System;
using System.Xml;
using WolfInv.Com.XPlatformCtrlLib;
namespace WolfInv.Com.CommCtrlLib
{
    public abstract class ToolBarItemBuilder
    {
        protected object ToolBar;
        protected IFrame frmObj;
        public ToolBarItemBuilder(IFrame frm, ITag toolbar)
        {
            frmObj = frm;
            ToolBar = toolbar;
            
        }
        public abstract void InitToolBar(bool LeftToRight);

        public abstract ITag AddToolBarItem(CMenuItem mnu, ToolBarItemType type, EventHandler del);

        public virtual ITag AddToolBarItem(string lbl, ToolBarItemType type)
        {
            return AddToolBarItem(lbl, type, null);
        }

        public abstract ITag AddToolBarItem(string lbl, ToolBarItemType type, EventHandler del);
        public abstract ITag AddToolBarItem(XmlNode xml, ToolBarItemType type, string itemid, params EventHandler[] del);

        public abstract ITag AddToolBarItem(string id, XmlNode backxml, string lbl, ToolBarItemType type, EventHandler del);
    }

}
