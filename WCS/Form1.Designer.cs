using System;
using WebKit;

namespace WCS
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.txt_user = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_pwd = new System.Windows.Forms.TextBox();
            this.Lbl_SystemName = new System.Windows.Forms.Label();
            this.btn_enter = new System.Windows.Forms.Button();
            this.btn_out = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(322, 284);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "用户名";
            // 
            // txt_user
            // 
            this.txt_user.Location = new System.Drawing.Point(416, 278);
            this.txt_user.Margin = new System.Windows.Forms.Padding(6);
            this.txt_user.Name = "txt_user";
            this.txt_user.Size = new System.Drawing.Size(240, 35);
            this.txt_user.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(322, 368);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 24);
            this.label2.TabIndex = 2;
            this.label2.Text = "密码";
            // 
            // txt_pwd
            // 
            this.txt_pwd.Location = new System.Drawing.Point(416, 362);
            this.txt_pwd.Margin = new System.Windows.Forms.Padding(6);
            this.txt_pwd.Name = "txt_pwd";
            this.txt_pwd.PasswordChar = '*';
            this.txt_pwd.Size = new System.Drawing.Size(240, 35);
            this.txt_pwd.TabIndex = 3;
            // 
            // Lbl_SystemName
            // 
            this.Lbl_SystemName.Dock = System.Windows.Forms.DockStyle.Top;
            this.Lbl_SystemName.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Lbl_SystemName.Location = new System.Drawing.Point(0, 0);
            this.Lbl_SystemName.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.Lbl_SystemName.Name = "Lbl_SystemName";
            this.Lbl_SystemName.Size = new System.Drawing.Size(996, 68);
            this.Lbl_SystemName.TabIndex = 4;
            this.Lbl_SystemName.Text = "XXX系统";
            this.Lbl_SystemName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_enter
            // 
            this.btn_enter.Location = new System.Drawing.Point(384, 470);
            this.btn_enter.Margin = new System.Windows.Forms.Padding(6);
            this.btn_enter.Name = "btn_enter";
            this.btn_enter.Size = new System.Drawing.Size(110, 38);
            this.btn_enter.TabIndex = 5;
            this.btn_enter.Text = "提交";
            this.btn_enter.UseVisualStyleBackColor = true;
            this.btn_enter.Click += new System.EventHandler(this.btn_enter_Click);
            // 
            // btn_out
            // 
            this.btn_out.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_out.Location = new System.Drawing.Point(494, 470);
            this.btn_out.Margin = new System.Windows.Forms.Padding(6);
            this.btn_out.Name = "btn_out";
            this.btn_out.Size = new System.Drawing.Size(110, 38);
            this.btn_out.TabIndex = 6;
            this.btn_out.Text = "退出";
            this.btn_out.UseVisualStyleBackColor = true;
            this.btn_out.Click += new System.EventHandler(this.btn_out_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.Lbl_SystemName);
            this.panel1.Location = new System.Drawing.Point(12, 84);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(996, 68);
            this.panel1.TabIndex = 7;
            // 
            // Form1
            // 
            this.AcceptButton = this.btn_enter;
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_out;
            this.ClientSize = new System.Drawing.Size(1592, 1264);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btn_out);
            this.Controls.Add(this.btn_enter);
            this.Controls.Add(this.txt_pwd);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txt_user);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "登录";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_user;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_pwd;
        private System.Windows.Forms.Label Lbl_SystemName;
        private System.Windows.Forms.Button btn_enter;
        private System.Windows.Forms.Button btn_out;
        private System.Windows.Forms.Panel panel1;
    }
}

