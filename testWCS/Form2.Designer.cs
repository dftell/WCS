namespace testWCS
{
    partial class form2
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
            this.btn_request = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_req_name = new System.Windows.Forms.TextBox();
            this.txt_bdId = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_result = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ddl_className = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txt_PageSize = new System.Windows.Forms.TextBox();
            this.txt_PageNo = new System.Windows.Forms.TextBox();
            this.txt_PostData = new System.Windows.Forms.TextBox();
            this.txt_url = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.chkbox_Post = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btn_request
            // 
            this.btn_request.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_request.Location = new System.Drawing.Point(640, 5);
            this.btn_request.Margin = new System.Windows.Forms.Padding(2);
            this.btn_request.Name = "btn_request";
            this.btn_request.Size = new System.Drawing.Size(37, 26);
            this.btn_request.TabIndex = 0;
            this.btn_request.Text = "请求";
            this.btn_request.UseVisualStyleBackColor = true;
            this.btn_request.Click += new System.EventHandler(this.btn_request_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 12);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "Access_Token";
            // 
            // txt_req_name
            // 
            this.txt_req_name.Location = new System.Drawing.Point(97, 9);
            this.txt_req_name.Margin = new System.Windows.Forms.Padding(2);
            this.txt_req_name.Name = "txt_req_name";
            this.txt_req_name.Size = new System.Drawing.Size(287, 21);
            this.txt_req_name.TabIndex = 2;
            // 
            // txt_bdId
            // 
            this.txt_bdId.Location = new System.Drawing.Point(97, 33);
            this.txt_bdId.Margin = new System.Windows.Forms.Padding(2);
            this.txt_bdId.Name = "txt_bdId";
            this.txt_bdId.Size = new System.Drawing.Size(287, 21);
            this.txt_bdId.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 36);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "bdId";
            // 
            // txt_result
            // 
            this.txt_result.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_result.Location = new System.Drawing.Point(18, 226);
            this.txt_result.Margin = new System.Windows.Forms.Padding(2);
            this.txt_result.Multiline = true;
            this.txt_result.Name = "txt_result";
            this.txt_result.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txt_result.Size = new System.Drawing.Size(659, 245);
            this.txt_result.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 61);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "dataTypes";
            // 
            // ddl_className
            // 
            this.ddl_className.FormattingEnabled = true;
            this.ddl_className.Location = new System.Drawing.Point(97, 58);
            this.ddl_className.Margin = new System.Windows.Forms.Padding(2);
            this.ddl_className.Name = "ddl_className";
            this.ddl_className.Size = new System.Drawing.Size(287, 20);
            this.ddl_className.TabIndex = 7;
            this.ddl_className.SelectedIndexChanged += new System.EventHandler(this.ddl_className_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(522, 37);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "页大小";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(522, 65);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "页数";
            // 
            // txt_PageSize
            // 
            this.txt_PageSize.Location = new System.Drawing.Point(568, 33);
            this.txt_PageSize.Margin = new System.Windows.Forms.Padding(2);
            this.txt_PageSize.Name = "txt_PageSize";
            this.txt_PageSize.Size = new System.Drawing.Size(61, 21);
            this.txt_PageSize.TabIndex = 10;
            this.txt_PageSize.Text = "1000";
            // 
            // txt_PageNo
            // 
            this.txt_PageNo.Location = new System.Drawing.Point(568, 61);
            this.txt_PageNo.Margin = new System.Windows.Forms.Padding(2);
            this.txt_PageNo.Name = "txt_PageNo";
            this.txt_PageNo.Size = new System.Drawing.Size(61, 21);
            this.txt_PageNo.TabIndex = 11;
            this.txt_PageNo.Text = "1";
            // 
            // txt_PostData
            // 
            this.txt_PostData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_PostData.Location = new System.Drawing.Point(18, 112);
            this.txt_PostData.Margin = new System.Windows.Forms.Padding(2);
            this.txt_PostData.Multiline = true;
            this.txt_PostData.Name = "txt_PostData";
            this.txt_PostData.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txt_PostData.Size = new System.Drawing.Size(659, 108);
            this.txt_PostData.TabIndex = 12;
            // 
            // txt_url
            // 
            this.txt_url.Location = new System.Drawing.Point(97, 83);
            this.txt_url.Margin = new System.Windows.Forms.Padding(2);
            this.txt_url.Name = "txt_url";
            this.txt_url.Size = new System.Drawing.Size(532, 21);
            this.txt_url.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 86);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(23, 12);
            this.label6.TabIndex = 14;
            this.label6.Text = "URL";
            // 
            // chkbox_Post
            // 
            this.chkbox_Post.AutoSize = true;
            this.chkbox_Post.Checked = true;
            this.chkbox_Post.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkbox_Post.Location = new System.Drawing.Point(523, 6);
            this.chkbox_Post.Name = "chkbox_Post";
            this.chkbox_Post.Size = new System.Drawing.Size(48, 16);
            this.chkbox_Post.TabIndex = 15;
            this.chkbox_Post.Text = "Post";
            this.chkbox_Post.UseVisualStyleBackColor = true;
            // 
            // form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(689, 477);
            this.Controls.Add(this.chkbox_Post);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txt_url);
            this.Controls.Add(this.txt_PostData);
            this.Controls.Add(this.txt_PageNo);
            this.Controls.Add(this.txt_PageSize);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ddl_className);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txt_result);
            this.Controls.Add(this.txt_bdId);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txt_req_name);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_request);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "form2";
            this.Text = "Form2";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_request;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_req_name;
        private System.Windows.Forms.TextBox txt_bdId;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_result;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox ddl_className;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txt_PageSize;
        private System.Windows.Forms.TextBox txt_PageNo;
        private System.Windows.Forms.TextBox txt_PostData;
        private System.Windows.Forms.TextBox txt_url;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chkbox_Post;
    }
}