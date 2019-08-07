namespace WolfInv.Com.DataCenter
{
    partial class ToolMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ToolMain));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_DataSource = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_datapoints = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_ViewDesign = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_FrameDesign = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_DataSource,
            this.toolStripSeparator1,
            this.toolStripButton_datapoints,
            this.toolStripSeparator2,
            this.toolStripButton_ViewDesign,
            this.toolStripSeparator3,
            this.toolStripButton_FrameDesign});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(622, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton_DataSource
            // 
            this.toolStripButton_DataSource.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton_DataSource.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_DataSource.Image")));
            this.toolStripButton_DataSource.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_DataSource.Name = "toolStripButton_DataSource";
            this.toolStripButton_DataSource.Size = new System.Drawing.Size(69, 22);
            this.toolStripButton_DataSource.Text = "数据源测试";
            this.toolStripButton_DataSource.Click += new System.EventHandler(this.toolStripButton_DataSource_Click);
            // 
            // toolStripButton_datapoints
            // 
            this.toolStripButton_datapoints.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton_datapoints.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_datapoints.Image")));
            this.toolStripButton_datapoints.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_datapoints.Name = "toolStripButton_datapoints";
            this.toolStripButton_datapoints.Size = new System.Drawing.Size(69, 22);
            this.toolStripButton_datapoints.Text = "数据点维护";
            this.toolStripButton_datapoints.Click += new System.EventHandler(this.toolStripButton_datapoints_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton_ViewDesign
            // 
            this.toolStripButton_ViewDesign.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton_ViewDesign.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_ViewDesign.Image")));
            this.toolStripButton_ViewDesign.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_ViewDesign.Name = "toolStripButton_ViewDesign";
            this.toolStripButton_ViewDesign.Size = new System.Drawing.Size(57, 22);
            this.toolStripButton_ViewDesign.Text = "视图设计";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton_FrameDesign
            // 
            this.toolStripButton_FrameDesign.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton_FrameDesign.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_FrameDesign.Image")));
            this.toolStripButton_FrameDesign.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_FrameDesign.Name = "toolStripButton_FrameDesign";
            this.toolStripButton_FrameDesign.Size = new System.Drawing.Size(57, 22);
            this.toolStripButton_FrameDesign.Text = "页面设计";
            // 
            // ToolMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(622, 340);
            this.Controls.Add(this.toolStrip1);
            this.Name = "ToolMain";
            this.Text = "IFrame 开发工具";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton_DataSource;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButton_datapoints;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButton_ViewDesign;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolStripButton_FrameDesign;
    }
}