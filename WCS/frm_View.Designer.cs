using WolfInv.Com.CommFormCtrlLib;
using WolfInv.Com.CommCtrlLib;
using System.Windows.Forms;

namespace WCS
{
    partial class frm_View
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        public ListViewColumnSorter  lvwColumnSorter;

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
            this.listView1 = new ListGrid();
            this.panel1.SuspendLayout();
            (this.panel_main as Panel).SuspendLayout();
            this.panel_bottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Size = new System.Drawing.Size(750, 21);
            // 
            // panel_main
            // 
            (this.panel_main as Panel).AutoSize = true;
            (this.panel_main as Panel).Controls.Add(this.listView1);
            (this.panel_main as Panel).Size = new System.Drawing.Size(743, 392);
            // 
            // panel_bottom
            // 
            this.panel_bottom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.panel_bottom.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel_bottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel_bottom.Location = new System.Drawing.Point(0, 447);
            this.panel_bottom.Size = new System.Drawing.Size(750, 26);
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
            this.listView1.Location = new System.Drawing.Point(5, 7);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(727, 376);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            this.listView1.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView1_ColumnClick);
            // 
            // frm_View
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Name = "frm_View";
            this.Size = new System.Drawing.Size(750, 473);
            this.Load += new System.EventHandler(this.frm_View_Load);
            this.ToolBar_NewCreate += new AddExistHandle(this.NewCreate_Click);
            this.ToolBar_OnSimpleSearchClicked += new System.EventHandler(this.SimpleSearch);
            this.ToolBar_RefreshData += new ToolBarHandle(this.RefreshData_Click);
            this.ToolBar_Export += new ToolBarHandle(this.Export_Click);
            this.ToolBar_ListSelectedItemsClicked += new ToolBarHandle(this.ListSelectedItems_Click);
            this.ToolBar_EditView += new ToolBarHandle(this.EditView_Click);
            this.ToolBar_PrintPDF += new ToolBarHandle(this.PrintPDF_Click); ;
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            (this.panel_main as Panel).ResumeLayout(false);
            this.panel_bottom.ResumeLayout(false);
            this.panel_bottom.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void Frm_View_ToolBar_PrintPDF()
        {
            throw new System.NotImplementedException();
        }

        #endregion

        private ListGrid listView1;

    }
}
