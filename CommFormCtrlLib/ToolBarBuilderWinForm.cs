using WolfInv.Com.CommCtrlLib;
namespace WolfInv.Com.CommFormCtrlLib
{
    public class ToolBarBuilderWinForm : BaseToolBarBuild
    {
        frm_Model frm;
        public ToolBarBuilderWinForm(IFrame ifrm,ITag itoolbar)
            : base(ifrm,itoolbar)
        {
            frm = ifrm as frm_Model;
            toolbarbld = new ToolBarBuilderItemForWin(ifrm, itoolbar);
        }


    }
}
