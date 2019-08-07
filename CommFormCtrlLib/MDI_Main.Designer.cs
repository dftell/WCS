using System.Windows.Forms;
using WolfInv.Com.XPlatformCtrlLib;

namespace WolfInv.Com.CommFormCtrlLib
{
    partial class MDI_Main
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.treeView_nav = new System.Windows.Forms.TreeView();
            this.Main_Plan = new  XWinForm_Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 800);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Padding = new System.Windows.Forms.Padding(2, 0, 28, 0);
            this.statusStrip.Size = new System.Drawing.Size(1264, 36);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "StatusStrip";
            this.statusStrip.DoubleClick += new System.EventHandler(this.statusStrip1_DoubleClick);
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(62, 31);
            this.toolStripStatusLabel.Text = "状态";
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(12, 4, 0, 4);
            this.menuStrip1.Size = new System.Drawing.Size(1264, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // treeView_nav
            // 
            this.treeView_nav.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView_nav.Location = new System.Drawing.Point(0, 0);
            this.treeView_nav.Margin = new System.Windows.Forms.Padding(6);
            this.treeView_nav.Name = "treeView_nav";
            this.treeView_nav.Size = new System.Drawing.Size(300, 776);
            this.treeView_nav.TabIndex = 7;
            this.treeView_nav.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_nav_AfterSelect);
            this.treeView_nav.DoubleClick += new System.EventHandler(this.treeView_nav_DoubleClick);
            // 
            // Main_Plan
            // 
            (this.Main_Plan as Panel).Dock = System.Windows.Forms.DockStyle.Fill;
            (this.Main_Plan as Panel).Location = new System.Drawing.Point(0, 0);
            (this.Main_Plan as Panel).Name = "Main_Plan";
            (this.Main_Plan as Panel).Size = new System.Drawing.Size(960, 776);
            (this.Main_Plan as Panel).TabIndex = 9;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeView_nav);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add((this.Main_Plan as Panel));
            this.splitContainer1.Size = new System.Drawing.Size(1264, 776);
            this.splitContainer1.SplitterDistance = 300;
            this.splitContainer1.TabIndex = 11;
            // 
            // MDI_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 836);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "MDI_Main";
            this.Text = "MDI_Main";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ToolTip ToolTip;
        private System.Windows.Forms.MenuStrip menuStrip1;
        public System.Windows.Forms.TreeView treeView_nav;
        public IXPanel Main_Plan;
        public System.Windows.Forms.SplitContainer splitContainer1;
    }
}



