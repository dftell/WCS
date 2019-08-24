using WolfInv.Com.CommFormCtrlLib;
using WolfInv.Com.CommCtrlLib;
using System.Windows.Forms;
using WolfInv.Com.WCS_Process;
using System.Xml;
namespace WCS
{
    partial class frm_View
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.listView1 = new WolfInv.Com.CommCtrlLib.ListGrid();
            this.panel_main.SuspendLayout();
            this.panel_bottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_Title
            // 
            this.panel_Title.Size = new System.Drawing.Size(1200, 42);
            // 
            // panel_main
            // 
            this.panel_main.AutoSize = true;
            this.panel_main.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.panel_main.Controls.Add(this.listView1);
            this.panel_main.Margin = new System.Windows.Forms.Padding(12);
            this.panel_main.Size = new System.Drawing.Size(1190, 737);
            // 
            // panel_bottom
            // 
            this.panel_bottom.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel_bottom.Location = new System.Drawing.Point(0, 854);
            this.panel_bottom.Margin = new System.Windows.Forms.Padding(12);
            this.panel_bottom.Size = new System.Drawing.Size(1200, 46);
            // 
            // listView1
            // 
            this.listView1.AllowSum = false;
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.CheckBoxes = true;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Margin = new System.Windows.Forms.Padding(6);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(1184, 731);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView1_ColumnClick);
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            // 
            // frm_View
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(12);
            this.Name = "frm_View";
            this.Size = new System.Drawing.Size(1200, 900);
            this.ToolBar_OnSimpleSearchClicked += new System.EventHandler(this.SimpleSearch);
            this.ToolBar_ListSelectedItemsClicked += new WolfInv.Com.CommCtrlLib.ToolBarHandle(this.ListSelectedItems_Click);
            this.ToolBar_EditView += new WolfInv.Com.CommCtrlLib.ToolBarHandle(this.EditView_Click);
            this.ToolBar_NewCreate += new WolfInv.Com.CommCtrlLib.AddExistHandle(this.NewCreate_Click);
            this.ToolBar_Remove += this.frm_View_ToolBar_Remove;
            this.ToolBar_RefreshData += new WolfInv.Com.CommCtrlLib.ToolBarHandle(this.RefreshData_Click);
            this.ToolBar_Export += new WolfInv.Com.CommCtrlLib.ToolBarHandle(this.Export_Click);
            this.Load += new System.EventHandler(this.frm_View_Load);
            this.panel_main.ResumeLayout(false);
            this.panel_bottom.ResumeLayout(false);
            this.panel_bottom.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void Frm_View_ToolBar_Sync(WolfInv.Com.XPlatformCtrlLib.CMenuItem mnu)
        {
            //XmlDocument xmldoc = getExtraData(mnu);
        }



        #endregion

        private ListGrid listView1;

    }
}
