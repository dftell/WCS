namespace WCS
{
    partial class frm_WebPage
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.webview = new Xilium.CefGlue.WindowsForms.CefWebBrowser();
            this.panel_main.SuspendLayout();
            this.panel_bottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_Title
            // 
            this.panel_Title.Size = new System.Drawing.Size(1043, 42);
            // 
            // panel_main
            // 
            this.panel_main.Controls.Add(this.webview);
            this.panel_main.Size = new System.Drawing.Size(1029, 981);
            // 
            // panel_bottom
            // 
            this.panel_bottom.Location = new System.Drawing.Point(0, 1092);
            this.panel_bottom.Size = new System.Drawing.Size(1043, 52);
            // 
            // webview
            // 
            this.webview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webview.Location = new System.Drawing.Point(0, 0);
            this.webview.Name = "webview";
            this.webview.Size = new System.Drawing.Size(1025, 977);
            this.webview.TabIndex = 0;
            this.webview.Text = "cefWebBrowser1";
            // 
            // frm_WebPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "frm_WebPage";
            this.Size = new System.Drawing.Size(1043, 1144);
            this.Load += new System.EventHandler(this.frm_WebPage_Load);
            this.DockChanged += new System.EventHandler(this.frm_WebPage_DockChanged);
            this.Controls.SetChildIndex(this.panel_Title, 0);
            this.Controls.SetChildIndex(this.panel_main, 0);
            this.Controls.SetChildIndex(this.panel_bottom, 0);
            this.panel_main.ResumeLayout(false);
            this.panel_bottom.ResumeLayout(false);
            this.panel_bottom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Xilium.CefGlue.WindowsForms.CefWebBrowser webview;
    }
}
