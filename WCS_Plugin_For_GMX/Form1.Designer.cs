namespace WCS_Plugin_For_GMX
{
    partial class Form1
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.btn_save_saleOrder = new System.Windows.Forms.Button();
            this.btn_sync_productlist = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(142, 31);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(128, 34);
            this.button1.TabIndex = 0;
            this.button1.Text = "转换销售计划表支持企业一号";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            // 
            // btn_save_saleOrder
            // 
            this.btn_save_saleOrder.Location = new System.Drawing.Point(272, 31);
            this.btn_save_saleOrder.Margin = new System.Windows.Forms.Padding(2);
            this.btn_save_saleOrder.Name = "btn_save_saleOrder";
            this.btn_save_saleOrder.Size = new System.Drawing.Size(130, 34);
            this.btn_save_saleOrder.TabIndex = 1;
            this.btn_save_saleOrder.Text = "同步出货单";
            this.btn_save_saleOrder.UseVisualStyleBackColor = true;
            this.btn_save_saleOrder.Click += new System.EventHandler(this.btn_save_saleOrder_Click);
            // 
            // btn_sync_productlist
            // 
            this.btn_sync_productlist.Location = new System.Drawing.Point(22, 31);
            this.btn_sync_productlist.Margin = new System.Windows.Forms.Padding(2);
            this.btn_sync_productlist.Name = "btn_sync_productlist";
            this.btn_sync_productlist.Size = new System.Drawing.Size(117, 34);
            this.btn_sync_productlist.TabIndex = 3;
            this.btn_sync_productlist.Text = "同步物料清单";
            this.btn_sync_productlist.UseVisualStyleBackColor = true;
            this.btn_sync_productlist.Click += new System.EventHandler(this.btn_sync_productlist_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripProgressBar1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 184);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(423, 22);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(32, 17);
            this.toolStripStatusLabel1.Text = "状态";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(300, 16);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // Form1
            // 
            this.AcceptButton = this.btn_save_saleOrder;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(423, 206);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btn_sync_productlist);
            this.Controls.Add(this.btn_save_saleOrder);
            this.Controls.Add(this.button1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "果美香临时补丁";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btn_save_saleOrder;
        private System.Windows.Forms.Button btn_sync_productlist;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}

