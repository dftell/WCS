using System.Collections.Generic;

namespace WolfInv.Com.CommFormCtrlLib
{
    public class PanelRow
    {
        public int Index;
        public bool Visual = true;
        public EditPanel OwnPanel;
        public List<PanelCell> Cells = new List<PanelCell>();
    }


}

