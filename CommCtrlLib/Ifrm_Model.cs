using System;
using System.Collections.Generic;
using System.Data;
using WolfInv.Com.CommCtrlLib;
using WolfInv.Com.MetaDataCenter;
using WolfInv.Com.XPlatformCtrlLib;
namespace WolfInv.Com.CommCtrlLib
{
    public  interface Ifrm_Model : IXUserControl, IFrame, IKeyForm, ILink, ITranslateableInterFace, IUserData, IKeyTransable, ITag, IDataSouceControl, IFrameObj
    {
        IXLabel lb_Title { get;  }
    }
}