namespace WolfInv.Com.DataCenter
{
    partial class frm_Tool_SourceTest
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
            this.txt_datasource = new System.Windows.Forms.TextBox();
            this.txt_sql = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_GetSql = new System.Windows.Forms.Button();
            this.Btn_GetXml = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txt_datasource
            // 
            this.txt_datasource.Location = new System.Drawing.Point(12, 12);
            this.txt_datasource.Multiline = true;
            this.txt_datasource.Name = "txt_datasource";
            this.txt_datasource.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txt_datasource.Size = new System.Drawing.Size(667, 133);
            this.txt_datasource.TabIndex = 0;
            // 
            // txt_sql
            // 
            this.txt_sql.Location = new System.Drawing.Point(12, 179);
            this.txt_sql.Multiline = true;
            this.txt_sql.Name = "txt_sql";
            this.txt_sql.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txt_sql.Size = new System.Drawing.Size(667, 245);
            this.txt_sql.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 154);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "执行结果";
            // 
            // btn_GetSql
            // 
            this.btn_GetSql.Location = new System.Drawing.Point(553, 149);
            this.btn_GetSql.Name = "btn_GetSql";
            this.btn_GetSql.Size = new System.Drawing.Size(60, 25);
            this.btn_GetSql.TabIndex = 3;
            this.btn_GetSql.Text = "返回Sql";
            this.btn_GetSql.UseVisualStyleBackColor = true;
            this.btn_GetSql.Click += new System.EventHandler(this.btn_GetSql_Click);
            // 
            // Btn_GetXml
            // 
            this.Btn_GetXml.Location = new System.Drawing.Point(619, 149);
            this.Btn_GetXml.Name = "Btn_GetXml";
            this.Btn_GetXml.Size = new System.Drawing.Size(60, 25);
            this.Btn_GetXml.TabIndex = 4;
            this.Btn_GetXml.Text = "返回Xml";
            this.Btn_GetXml.UseVisualStyleBackColor = true;
            this.Btn_GetXml.Click += new System.EventHandler(this.Btn_GetXml_Click);
            // 
            // frm_Tool_SourceTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(691, 436);
            this.Controls.Add(this.Btn_GetXml);
            this.Controls.Add(this.btn_GetSql);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_sql);
            this.Controls.Add(this.txt_datasource);
            this.Name = "frm_Tool_SourceTest";
            this.Text = "数据源处理";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_datasource;
        private System.Windows.Forms.TextBox txt_sql;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_GetSql;
        private System.Windows.Forms.Button Btn_GetXml;
    }
}