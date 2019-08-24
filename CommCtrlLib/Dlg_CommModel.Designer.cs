namespace WolfInv.Com.CommCtrlLib
{
    partial class Dlg_CommModel
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.btn_yes = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox_column = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox_datasources = new System.Windows.Forms.ComboBox();
            this.lbl_count = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.txt_searchkey = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_location = new System.Windows.Forms.TextBox();
            this.listView1 = new WolfInv.Com.CommCtrlLib.ListGrid();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btn_Cancel);
            this.panel1.Controls.Add(this.btn_yes);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 868);
            this.panel1.Margin = new System.Windows.Forms.Padding(6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(848, 64);
            this.panel1.TabIndex = 27;
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_Cancel.Location = new System.Drawing.Point(736, 16);
            this.btn_Cancel.Margin = new System.Windows.Forms.Padding(6);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(94, 42);
            this.btn_Cancel.TabIndex = 1;
            this.btn_Cancel.Text = "取消";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // btn_yes
            // 
            this.btn_yes.Location = new System.Drawing.Point(632, 16);
            this.btn_yes.Margin = new System.Windows.Forms.Padding(6);
            this.btn_yes.Name = "btn_yes";
            this.btn_yes.Size = new System.Drawing.Size(94, 42);
            this.btn_yes.TabIndex = 0;
            this.btn_yes.Text = "确定";
            this.btn_yes.UseVisualStyleBackColor = true;
            this.btn_yes.Click += new System.EventHandler(this.btn_yes_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(24, 252);
            this.label5.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(130, 24);
            this.label5.TabIndex = 26;
            this.label5.Text = "所有记录：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(24, 212);
            this.label4.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(110, 24);
            this.label4.TabIndex = 25;
            this.label4.Text = "可用记录";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 86);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 24);
            this.label2.TabIndex = 23;
            this.label2.Text = "查找";
            // 
            // comboBox_column
            // 
            this.comboBox_column.FormattingEnabled = true;
            this.comboBox_column.Location = new System.Drawing.Point(180, 80);
            this.comboBox_column.Margin = new System.Windows.Forms.Padding(6);
            this.comboBox_column.Name = "comboBox_column";
            this.comboBox_column.Size = new System.Drawing.Size(232, 32);
            this.comboBox_column.TabIndex = 22;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 32);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 24);
            this.label1.TabIndex = 21;
            this.label1.Text = "选择范围";
            // 
            // comboBox_datasources
            // 
            this.comboBox_datasources.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_datasources.FormattingEnabled = true;
            this.comboBox_datasources.Location = new System.Drawing.Point(180, 26);
            this.comboBox_datasources.Margin = new System.Windows.Forms.Padding(6);
            this.comboBox_datasources.Name = "comboBox_datasources";
            this.comboBox_datasources.Size = new System.Drawing.Size(640, 32);
            this.comboBox_datasources.TabIndex = 20;
            // 
            // lbl_count
            // 
            this.lbl_count.AutoSize = true;
            this.lbl_count.Location = new System.Drawing.Point(154, 252);
            this.lbl_count.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbl_count.Name = "lbl_count";
            this.lbl_count.Size = new System.Drawing.Size(82, 24);
            this.lbl_count.TabIndex = 28;
            this.lbl_count.Text = "label3";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(706, 140);
            this.button1.Margin = new System.Windows.Forms.Padding(6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(124, 48);
            this.button1.TabIndex = 29;
            this.button1.Text = "查找";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txt_searchkey
            // 
            this.txt_searchkey.Location = new System.Drawing.Point(180, 142);
            this.txt_searchkey.Margin = new System.Windows.Forms.Padding(6);
            this.txt_searchkey.Name = "txt_searchkey";
            this.txt_searchkey.Size = new System.Drawing.Size(502, 35);
            this.txt_searchkey.TabIndex = 30;
            this.txt_searchkey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_searchkey_KeyDown);
            this.txt_searchkey.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_searchkey_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 306);
            this.label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(130, 24);
            this.label3.TabIndex = 31;
            this.label3.Text = "跳到记录：";
            // 
            // txt_location
            // 
            this.txt_location.Location = new System.Drawing.Point(180, 294);
            this.txt_location.Margin = new System.Windows.Forms.Padding(6);
            this.txt_location.Name = "txt_location";
            this.txt_location.Size = new System.Drawing.Size(646, 35);
            this.txt_location.TabIndex = 32;
            // 
            // listView1
            // 
            this.listView1.AllowGroup = false;
            this.listView1.AllowSum = false;
            this.listView1.CheckBoxes = true;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.GroupBy = null;
            this.listView1.Location = new System.Drawing.Point(18, 354);
            this.listView1.Margin = new System.Windows.Forms.Padding(6);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(806, 470);
            this.listView1.SumItems = null;
            this.listView1.TabIndex = 33;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            // 
            // Dlg_CommModel
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_Cancel;
            this.ClientSize = new System.Drawing.Size(848, 932);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.txt_location);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txt_searchkey);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lbl_count);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox_column);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox_datasources);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Dlg_CommModel";
            this.Text = "O0o0哦“";
            this.Load += new System.EventHandler(this.Dlg_CommModel_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_yes;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox_column;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox_datasources;
        private System.Windows.Forms.Button btn_Cancel;
        private System.Windows.Forms.Label lbl_count;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txt_searchkey;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_location;
        private ListGrid listView1;
    }
}