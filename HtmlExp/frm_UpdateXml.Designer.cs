namespace WolfInv.Com.HtmlExp
{
    partial class frm_UpdateXml
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
            this.txt_xml = new System.Windows.Forms.TextBox();
            this.button_ok = new System.Windows.Forms.Button();
            this.btn_skip = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txt_xml
            // 
            this.txt_xml.Location = new System.Drawing.Point(7, 8);
            this.txt_xml.Multiline = true;
            this.txt_xml.Name = "txt_xml";
            this.txt_xml.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txt_xml.Size = new System.Drawing.Size(616, 355);
            this.txt_xml.TabIndex = 0;
            // 
            // button_ok
            // 
            this.button_ok.Location = new System.Drawing.Point(562, 369);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(61, 29);
            this.button_ok.TabIndex = 1;
            this.button_ok.Text = "确定";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // btn_skip
            // 
            this.btn_skip.Location = new System.Drawing.Point(499, 370);
            this.btn_skip.Name = "btn_skip";
            this.btn_skip.Size = new System.Drawing.Size(61, 29);
            this.btn_skip.TabIndex = 2;
            this.btn_skip.Text = "跳过";
            this.btn_skip.UseVisualStyleBackColor = true;
            this.btn_skip.Click += new System.EventHandler(this.btn_skip_Click);
            // 
            // frm_UpdateXml
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(628, 404);
            this.Controls.Add(this.btn_skip);
            this.Controls.Add(this.button_ok);
            this.Controls.Add(this.txt_xml);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_UpdateXml";
            this.Text = "修改Xml内容";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_xml;
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.Button btn_skip;
    }
}