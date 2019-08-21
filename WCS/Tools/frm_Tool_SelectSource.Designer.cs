namespace WCS
{
    partial class frm_Tool_SelectSource
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.panel_buttons = new System.Windows.Forms.Panel();
            this.lbl_EditFile = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_Save = new System.Windows.Forms.Button();
            this.panel_main = new System.Windows.Forms.Panel();
            this.btn_AllCancel = new System.Windows.Forms.Button();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.btn_To = new System.Windows.Forms.Button();
            this.btn_AllTo = new System.Windows.Forms.Button();
            this.listView_Save = new System.Windows.Forms.ListView();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.listView_From = new System.Windows.Forms.ListView();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel_buttons.SuspendLayout();
            this.panel_main.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeView1);
            this.splitContainer1.Panel1MinSize = 50;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel_buttons);
            this.splitContainer1.Panel2.Controls.Add(this.panel_main);
            this.splitContainer1.Size = new System.Drawing.Size(1751, 1086);
            this.splitContainer1.SplitterDistance = 300;
            this.splitContainer1.TabIndex = 0;
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(300, 1086);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // panel_buttons
            // 
            this.panel_buttons.Controls.Add(this.lbl_EditFile);
            this.panel_buttons.Controls.Add(this.label1);
            this.panel_buttons.Controls.Add(this.btn_Save);
            this.panel_buttons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel_buttons.Location = new System.Drawing.Point(0, 996);
            this.panel_buttons.Name = "panel_buttons";
            this.panel_buttons.Size = new System.Drawing.Size(1447, 90);
            this.panel_buttons.TabIndex = 1;
            // 
            // lbl_EditFile
            // 
            this.lbl_EditFile.AutoSize = true;
            this.lbl_EditFile.Location = new System.Drawing.Point(100, 28);
            this.lbl_EditFile.Name = "lbl_EditFile";
            this.lbl_EditFile.Size = new System.Drawing.Size(82, 24);
            this.lbl_EditFile.TabIndex = 6;
            this.lbl_EditFile.Text = "未定义";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 24);
            this.label1.TabIndex = 5;
            this.label1.Text = "文件：";
            // 
            // btn_Save
            // 
            this.btn_Save.Location = new System.Drawing.Point(1274, 3);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(165, 75);
            this.btn_Save.TabIndex = 4;
            this.btn_Save.Text = "保存";
            this.btn_Save.UseVisualStyleBackColor = true;
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // panel_main
            // 
            this.panel_main.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_main.Controls.Add(this.comboBox1);
            this.panel_main.Controls.Add(this.label2);
            this.panel_main.Controls.Add(this.btn_AllCancel);
            this.panel_main.Controls.Add(this.btn_Cancel);
            this.panel_main.Controls.Add(this.btn_To);
            this.panel_main.Controls.Add(this.btn_AllTo);
            this.panel_main.Controls.Add(this.listView_Save);
            this.panel_main.Controls.Add(this.listView_From);
            this.panel_main.Location = new System.Drawing.Point(3, 3);
            this.panel_main.Name = "panel_main";
            this.panel_main.Size = new System.Drawing.Size(1444, 987);
            this.panel_main.TabIndex = 0;
            // 
            // btn_AllCancel
            // 
            this.btn_AllCancel.Location = new System.Drawing.Point(667, 614);
            this.btn_AllCancel.Name = "btn_AllCancel";
            this.btn_AllCancel.Size = new System.Drawing.Size(113, 70);
            this.btn_AllCancel.TabIndex = 5;
            this.btn_AllCancel.Text = "<<";
            this.btn_AllCancel.UseVisualStyleBackColor = true;
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Location = new System.Drawing.Point(667, 508);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(113, 70);
            this.btn_Cancel.TabIndex = 4;
            this.btn_Cancel.Text = "<";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            // 
            // btn_To
            // 
            this.btn_To.Location = new System.Drawing.Point(667, 403);
            this.btn_To.Name = "btn_To";
            this.btn_To.Size = new System.Drawing.Size(113, 70);
            this.btn_To.TabIndex = 3;
            this.btn_To.Text = ">";
            this.btn_To.UseVisualStyleBackColor = true;
            // 
            // btn_AllTo
            // 
            this.btn_AllTo.Location = new System.Drawing.Point(667, 297);
            this.btn_AllTo.Name = "btn_AllTo";
            this.btn_AllTo.Size = new System.Drawing.Size(113, 70);
            this.btn_AllTo.TabIndex = 2;
            this.btn_AllTo.Text = ">>";
            this.btn_AllTo.UseVisualStyleBackColor = true;
            // 
            // listView_Save
            // 
            this.listView_Save.Location = new System.Drawing.Point(786, 94);
            this.listView_Save.Name = "listView_Save";
            this.listView_Save.Size = new System.Drawing.Size(650, 885);
            this.listView_Save.TabIndex = 1;
            this.listView_Save.UseCompatibleStateImageBehavior = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(130, 24);
            this.label2.TabIndex = 6;
            this.label2.Text = "DataSource";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(145, 36);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(508, 32);
            this.comboBox1.TabIndex = 7;
            // 
            // listView_From
            // 
            this.listView_From.Location = new System.Drawing.Point(13, 94);
            this.listView_From.Name = "listView_From";
            this.listView_From.Size = new System.Drawing.Size(640, 888);
            this.listView_From.TabIndex = 0;
            this.listView_From.UseCompatibleStateImageBehavior = false;
            // 
            // frm_Tool_ViewDesigner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1751, 1086);
            this.Controls.Add(this.splitContainer1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_Tool_ViewDesigner";
            this.Text = "frm_Tool_ViewDesigner";
            this.Load += new System.EventHandler(this.frm_Tool_ViewDesigner_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel_buttons.ResumeLayout(false);
            this.panel_buttons.PerformLayout();
            this.panel_main.ResumeLayout(false);
            this.panel_main.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Panel panel_buttons;
        private System.Windows.Forms.Panel panel_main;
        private System.Windows.Forms.Button btn_AllCancel;
        private System.Windows.Forms.Button btn_Cancel;
        private System.Windows.Forms.Button btn_To;
        private System.Windows.Forms.Button btn_AllTo;
        private System.Windows.Forms.ListView listView_Save;
        private System.Windows.Forms.Button btn_Save;
        private System.Windows.Forms.Label lbl_EditFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ListView listView_From;
    }
}