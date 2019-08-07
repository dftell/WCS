using WolfInv.Com.CommFormCtrlLib;
using WolfInv.Com.CommCtrlLib;
using System.Windows.Forms;
namespace WCS
{
    partial class frm_MainSubFrame
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.listView1 = new ListGrid();
            this.Panel_ToolBar = new System.Windows.Forms.Panel();
            this.Label_Title = new System.Windows.Forms.Label();
            this.toolStrip_subtitle = new System.Windows.Forms.ToolStrip();
            this.panel_subtoolbar = new System.Windows.Forms.Panel();
            this.splitContainer_detail = new System.Windows.Forms.SplitContainer();
            this.panel1.SuspendLayout();
            (this.panel_main as Panel).SuspendLayout();
            this.panel_bottom.SuspendLayout();
            this.Panel_ToolBar.SuspendLayout();
            this.panel_subtoolbar.SuspendLayout();
            this.splitContainer_detail.Panel1.SuspendLayout();
            this.splitContainer_detail.Panel2.SuspendLayout();
            this.splitContainer_detail.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_main
            // 
            (this.panel_main as Panel).Controls.Add(this.splitContainer_detail);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(9, 14);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(569, 162);
            this.tableLayoutPanel1.TabIndex = 0;
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
            this.listView1.Location = new System.Drawing.Point(8, 66);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(571, 83);
            this.listView1.TabIndex = 1;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            // 
            // Panel_ToolBar
            // 
            this.Panel_ToolBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Panel_ToolBar.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.Panel_ToolBar.Controls.Add(this.Label_Title);
            this.Panel_ToolBar.Location = new System.Drawing.Point(7, 5);
            this.Panel_ToolBar.Name = "Panel_ToolBar";
            this.Panel_ToolBar.Size = new System.Drawing.Size(572, 27);
            this.Panel_ToolBar.TabIndex = 3;
            // 
            // Label_Title
            // 
            this.Label_Title.AutoSize = true;
            this.Label_Title.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label_Title.ForeColor = System.Drawing.Color.White;
            this.Label_Title.Location = new System.Drawing.Point(5, 8);
            this.Label_Title.Name = "Label_Title";
            this.Label_Title.Size = new System.Drawing.Size(47, 12);
            this.Label_Title.TabIndex = 0;
            this.Label_Title.Text = "label1";
            // 
            // toolStrip_subtitle
            // 
            this.toolStrip_subtitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.toolStrip_subtitle.AutoSize = false;
            this.toolStrip_subtitle.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip_subtitle.Location = new System.Drawing.Point(-5, 0);
            this.toolStrip_subtitle.Name = "toolStrip_subtitle";
            this.toolStrip_subtitle.Size = new System.Drawing.Size(569, 25);
            this.toolStrip_subtitle.TabIndex = 1;
            this.toolStrip_subtitle.Text = "toolStrip2";
            // 
            // panel_subtoolbar
            // 
            this.panel_subtoolbar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_subtoolbar.Controls.Add(this.toolStrip_subtitle);
            this.panel_subtoolbar.Location = new System.Drawing.Point(11, 38);
            this.panel_subtoolbar.Name = "panel_subtoolbar";
            this.panel_subtoolbar.Size = new System.Drawing.Size(568, 29);
            this.panel_subtoolbar.TabIndex = 4;
            // 
            // splitContainer_detail
            // 
            this.splitContainer_detail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_detail.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_detail.Name = "splitContainer_detail";
            this.splitContainer_detail.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer_detail.Panel1
            // 
            this.splitContainer_detail.Panel1.Controls.Add(this.tableLayoutPanel1);
            // 
            // splitContainer_detail.Panel2
            // 
            this.splitContainer_detail.Panel2.Controls.Add(this.panel_subtoolbar);
            this.splitContainer_detail.Panel2.Controls.Add(this.Panel_ToolBar);
            this.splitContainer_detail.Panel2.Controls.Add(this.listView1);
            this.splitContainer_detail.Size = new System.Drawing.Size(585, 376);
            this.splitContainer_detail.SplitterDistance = 220;
            this.splitContainer_detail.TabIndex = 5;
            // 
            // frm_MainSubFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "frm_MainSubFrame";
            this.Load += new System.EventHandler(this.frm_MainSubFrame_Load);
            this.ToolBar_AddExist += new AddExistHandle(this.AddExist_Click);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            (this.panel_main as Panel).ResumeLayout(false);
            this.panel_bottom.ResumeLayout(false);
            this.panel_bottom.PerformLayout();
            this.Panel_ToolBar.ResumeLayout(false);
            this.Panel_ToolBar.PerformLayout();
            this.panel_subtoolbar.ResumeLayout(false);
            this.splitContainer_detail.Panel1.ResumeLayout(false);
            this.splitContainer_detail.Panel1.PerformLayout();
            this.splitContainer_detail.Panel2.ResumeLayout(false);
            this.splitContainer_detail.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private WolfInv.Com.CommCtrlLib.ListGrid listView1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel Panel_ToolBar;
        private System.Windows.Forms.Label Label_Title;
        private System.Windows.Forms.ToolStrip toolStrip_subtitle;
        private System.Windows.Forms.Panel panel_subtoolbar;
        private System.Windows.Forms.SplitContainer splitContainer_detail;

    }
}
