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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_MainSubFrame));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.Panel_ToolBar = new System.Windows.Forms.Panel();
            this.Label_Title = new System.Windows.Forms.Label();
            this.splitContainer_detail = new System.Windows.Forms.SplitContainer();
            this.panel_subtoolbar = new System.Windows.Forms.Panel();
            this.listView1 = new WolfInv.Com.CommCtrlLib.ListGrid();
            this.toolStrip_subtitle = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.panel_main.SuspendLayout();
            this.panel_bottom.SuspendLayout();
            this.Panel_ToolBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_detail)).BeginInit();
            this.splitContainer_detail.Panel1.SuspendLayout();
            this.splitContainer_detail.Panel2.SuspendLayout();
            this.splitContainer_detail.SuspendLayout();
            this.panel_subtoolbar.SuspendLayout();
            this.toolStrip_subtitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_Title
            // 
            this.panel_Title.Location = new System.Drawing.Point(0, 25);
            this.panel_Title.Size = new System.Drawing.Size(1200, 42);
            // 
            // panel_main
            // 
            this.panel_main.Controls.Add(this.splitContainer_detail);
            this.panel_main.Size = new System.Drawing.Size(1190, 727);
            // 
            // panel_bottom
            // 
            this.panel_bottom.Location = new System.Drawing.Point(0, 848);
            this.panel_bottom.Size = new System.Drawing.Size(1200, 52);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(6);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1186, 100);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // Panel_ToolBar
            // 
            this.Panel_ToolBar.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.Panel_ToolBar.Controls.Add(this.Label_Title);
            this.Panel_ToolBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.Panel_ToolBar.Location = new System.Drawing.Point(0, 0);
            this.Panel_ToolBar.Margin = new System.Windows.Forms.Padding(6);
            this.Panel_ToolBar.Name = "Panel_ToolBar";
            this.Panel_ToolBar.Size = new System.Drawing.Size(1186, 54);
            this.Panel_ToolBar.TabIndex = 3;
            // 
            // Label_Title
            // 
            this.Label_Title.AutoSize = true;
            this.Label_Title.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label_Title.ForeColor = System.Drawing.Color.White;
            this.Label_Title.Location = new System.Drawing.Point(10, 16);
            this.Label_Title.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.Label_Title.Name = "Label_Title";
            this.Label_Title.Size = new System.Drawing.Size(88, 24);
            this.Label_Title.TabIndex = 0;
            this.Label_Title.Text = "label1";
            // 
            // splitContainer_detail
            // 
            this.splitContainer_detail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_detail.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer_detail.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_detail.Margin = new System.Windows.Forms.Padding(6);
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
            this.splitContainer_detail.Panel2.Resize += new System.EventHandler(this.splitContainer_detail_Panel2_Resize);
            this.splitContainer_detail.Size = new System.Drawing.Size(1186, 723);
            this.splitContainer_detail.SplitterDistance = 100;
            this.splitContainer_detail.SplitterWidth = 8;
            this.splitContainer_detail.TabIndex = 5;
            this.splitContainer_detail.DockChanged += new System.EventHandler(this.splitContainer_detail_DockChanged);
            // 
            // panel_subtoolbar
            // 
            this.panel_subtoolbar.Controls.Add(this.listView1);
            this.panel_subtoolbar.Controls.Add(this.toolStrip_subtitle);
            this.panel_subtoolbar.Location = new System.Drawing.Point(14, 76);
            this.panel_subtoolbar.Margin = new System.Windows.Forms.Padding(6);
            this.panel_subtoolbar.Name = "panel_subtoolbar";
            this.panel_subtoolbar.Size = new System.Drawing.Size(1097, 509);
            this.panel_subtoolbar.TabIndex = 6;
            // 
            // listView1
            // 
            this.listView1.AllowGroup = false;
            this.listView1.AllowSum = false;
            this.listView1.CheckBoxes = true;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.GroupBy = null;
            this.listView1.Location = new System.Drawing.Point(6, 67);
            this.listView1.Margin = new System.Windows.Forms.Padding(6);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(982, 412);
            this.listView1.SumItems = null;
            this.listView1.TabIndex = 3;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView1_ColumnClick);
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            this.listView1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseUp);
            // 
            // toolStrip_subtitle
            // 
            this.toolStrip_subtitle.AutoSize = false;
            this.toolStrip_subtitle.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip_subtitle.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripDropDownButton1});
            this.toolStrip_subtitle.Location = new System.Drawing.Point(0, 0);
            this.toolStrip_subtitle.Name = "toolStrip_subtitle";
            this.toolStrip_subtitle.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.toolStrip_subtitle.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.toolStrip_subtitle.Size = new System.Drawing.Size(1097, 49);
            this.toolStrip_subtitle.TabIndex = 1;
            this.toolStrip_subtitle.Text = "toolStrip2";
            this.toolStrip_subtitle.DockChanged += new System.EventHandler(this.toolStrip_subtitle_DockChanged);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(36, 46);
            this.toolStripButton1.Text = "toolStripButton1";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(54, 46);
            this.toolStripDropDownButton1.Text = "toolStripDropDownButton1";
            // 
            // frm_MainSubFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Margin = new System.Windows.Forms.Padding(12);
            this.Name = "frm_MainSubFrame";
            this.Size = new System.Drawing.Size(1200, 900);
            this.ToolBar_ChangeGroup += new WolfInv.Com.CommCtrlLib.ToolBarResponseHandle(this.frm_MainSubFrame_ToolBar_ChangeGroup);
            this.ToolBar_BatchUpdate += Frm_MainSubFrame_ToolBar_BatchUpdate ;
            this.ToolBar_AddExist += new WolfInv.Com.CommCtrlLib.AddExistHandle(this.AddExist_Click);
            this.Load += new System.EventHandler(this.frm_MainSubFrame_Load);
            this.DockChanged += new System.EventHandler(this.frm_MainSubFrame_DockChanged);
            this.Controls.SetChildIndex(this.panel_Title, 0);
            this.Controls.SetChildIndex(this.panel_main, 0);
            this.Controls.SetChildIndex(this.panel_bottom, 0);
            this.panel_main.ResumeLayout(false);
            this.panel_bottom.ResumeLayout(false);
            this.panel_bottom.PerformLayout();
            this.Panel_ToolBar.ResumeLayout(false);
            this.Panel_ToolBar.PerformLayout();
            this.splitContainer_detail.Panel1.ResumeLayout(false);
            this.splitContainer_detail.Panel1.PerformLayout();
            this.splitContainer_detail.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_detail)).EndInit();
            this.splitContainer_detail.ResumeLayout(false);
            this.panel_subtoolbar.ResumeLayout(false);
            this.toolStrip_subtitle.ResumeLayout(false);
            this.toolStrip_subtitle.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel Panel_ToolBar;
        private System.Windows.Forms.Label Label_Title;
        private System.Windows.Forms.SplitContainer splitContainer_detail;
        private Panel panel_subtoolbar;
        private ListGrid listView1;
        private ToolStrip toolStrip_subtitle;
        private ToolStripButton toolStripButton1;
        private ToolStripDropDownButton toolStripDropDownButton1;
    }
}
