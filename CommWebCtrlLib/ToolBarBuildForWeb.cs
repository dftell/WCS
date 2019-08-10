using WolfInv.Com.CommCtrlLib;

namespace WolfInv.Com.CommWebCtrlLib
{
    public class ToolBarBuildForWeb:BaseToolBarBuild 
    {
        WebPageFormHandle webfrm;
        ToolBarStrip toolbar;
        public ToolBarBuildForWeb(IFrame frmobj,ITag otoolbar):base(frmobj,otoolbar)
        {
            webfrm = frmobj as WebPageFormHandle;
            toolbar = otoolbar as ToolBarStrip;
            this.toolbarbld = new ToolBarItemBuilderForWeb(webfrm,toolbar);
        }
    }
}
