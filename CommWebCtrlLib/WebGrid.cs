using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Xml;
using XmlProcess;
using WolfInv.Com.WCS_Process;
using WolfInv.Com.MetaDataCenter;
using System.Data;
using System.IO;
using WolfInv.Com.CommCtrlLib;

namespace WolfInv.Com.CommWebCtrlLib
{
    public class WebGrid:BaseGrid
    {
        public WebGrid(IUserData frmhandle)
            : base(frmhandle)
        {

        }
        WebListGrid listobj;
        protected override IDataFieldHeaderColumn GetHeaderColumn()
        {
            return new WebListGridColumnHeader();
        }

        protected override void ClearItems()
        {
            
        }

        protected override void AddColumnItem(IDataFieldHeaderColumn ch)
        {
            if (listobj == null)
                listobj = this.listViewObj as WebListGrid;
            if (ch.Width == 0)
                return;
            BoundColumn bc = new BoundColumn();
            bc.DataField = ch.DataField;
            bc.HeaderStyle.Width = new Unit(ch.Width);
            bc.HeaderText = ch.Text ;
            listobj.Columns.Add(bc);
            listobj.Width = new Unit(listobj.Width.Value + ch.Width);
            // listobj.Columns.Add(ch as System.Web.UI.WebControls.DataGridColumn);
        }

        public virtual void FillGridData(DataSet ds)
        {
            this.Items.Clear();
            if (ds == null) return;
            if (ds.Tables.Count == 0) return;
            this.listobj.DataSource = ds.Tables[0].DefaultView;
            this.listobj.DataBind();
            
             
            //FillListView();
        }

    }

    public class SubGrid : WebGrid
    {
        public Panel ToolBar;
        //public string ListTitle;
        //
        public Label Lbl_Title;

        public SubGrid(IUserData frmhandle)
            : base(frmhandle)
        {

        }
    }

    


  
   
}
