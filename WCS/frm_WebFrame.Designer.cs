namespace WCS
{
    using System.Windows.Forms;
    partial class frm_WebFrame
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.wb_main = new System.Windows.Forms.WebBrowser();
            this.panel_Title.SuspendLayout();
            (this.panel_main as Panel).SuspendLayout();
            this.panel_bottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel_Title.Size = new System.Drawing.Size(1273, 42);
            // 
            // panel_main
            // 
            //this.panel_main.Controls.Add(this.wb_main);
            //this.panel_main.Size = new System.Drawing.Size(1259, 756);
            // 
            // wb_main
            // 
            this.wb_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wb_main.Location = new System.Drawing.Point(0, 0);
            this.wb_main.MinimumSize = new System.Drawing.Size(20, 20);
            this.wb_main.Name = "wb_main";
            this.wb_main.Size = new System.Drawing.Size(1255, 752);
            this.wb_main.TabIndex = 0;
            // 
            // frm_WebFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "frm_WebFrame";
            this.Size = new System.Drawing.Size(1273, 902);
            this.panel_Title.ResumeLayout(false);
            this.panel_Title.PerformLayout();
            (this.panel_main as Panel).ResumeLayout(false);
            this.panel_bottom.ResumeLayout(false);
            this.panel_bottom.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.WebBrowser wb_main;
    }
}