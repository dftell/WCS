namespace WCS
{
    partial class frm_Tool_ColumnDefine
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem_xml_datacolumns = new System.Windows.Forms.ToolStripMenuItem();
            this.XmlToolStripMenuItem_xml_datapoint = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_xml_datasource = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_xml_grid = new System.Windows.Forms.ToolStripMenuItem();
            this.PanelToolStripMenuItem_xml_editpanel = new System.Windows.Forms.ToolStripMenuItem();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.groupBox1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "表";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.comboBox1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(491, 100);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "查询内容";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(417, 54);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(51, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "查找";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(83, 56);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(304, 21);
            this.textBox1.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 12);
            this.label2.TabIndex = 2;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(82, 23);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(306, 20);
            this.comboBox1.TabIndex = 1;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // listView1
            // 
            this.listView1.CheckBoxes = true;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.listView1.ContextMenuStrip = this.contextMenuStrip1;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(14, 130);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(488, 182);
            this.listView1.TabIndex = 2;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_xml_datacolumns,
            this.XmlToolStripMenuItem_xml_datapoint,
            this.ToolStripMenuItem_xml_datasource,
            this.ToolStripMenuItem_xml_grid,
            this.PanelToolStripMenuItem_xml_editpanel});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(149, 114);
            // 
            // toolStripMenuItem_xml_datacolumns
            // 
            this.toolStripMenuItem_xml_datacolumns.Name = "toolStripMenuItem_xml_datacolumns";
            this.toolStripMenuItem_xml_datacolumns.Size = new System.Drawing.Size(148, 22);
            this.toolStripMenuItem_xml_datacolumns.Text = "生成表列Xml";
            this.toolStripMenuItem_xml_datacolumns.Click += new System.EventHandler(this.toolStripMenuItem_xml_datacolumns_Click);
            // 
            // XmlToolStripMenuItem_xml_datapoint
            // 
            this.XmlToolStripMenuItem_xml_datapoint.Name = "XmlToolStripMenuItem_xml_datapoint";
            this.XmlToolStripMenuItem_xml_datapoint.Size = new System.Drawing.Size(148, 22);
            this.XmlToolStripMenuItem_xml_datapoint.Text = "生成数据点Xml";
            this.XmlToolStripMenuItem_xml_datapoint.Click += new System.EventHandler(this.XmlToolStripMenuItem_xml_datapoint_Click);
            // 
            // ToolStripMenuItem_xml_datasource
            // 
            this.ToolStripMenuItem_xml_datasource.Name = "ToolStripMenuItem_xml_datasource";
            this.ToolStripMenuItem_xml_datasource.Size = new System.Drawing.Size(148, 22);
            this.ToolStripMenuItem_xml_datasource.Text = "生成数据源";
            // 
            // ToolStripMenuItem_xml_grid
            // 
            this.ToolStripMenuItem_xml_grid.Name = "ToolStripMenuItem_xml_grid";
            this.ToolStripMenuItem_xml_grid.Size = new System.Drawing.Size(148, 22);
            this.ToolStripMenuItem_xml_grid.Text = "生成Grid";
            // 
            // PanelToolStripMenuItem_xml_editpanel
            // 
            this.PanelToolStripMenuItem_xml_editpanel.Name = "PanelToolStripMenuItem_xml_editpanel";
            this.PanelToolStripMenuItem_xml_editpanel.Size = new System.Drawing.Size(148, 22);
            this.PanelToolStripMenuItem_xml_editpanel.Text = "生成EditPanel";
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "表名";
            this.columnHeader1.Width = 250;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "视图";
            this.columnHeader2.Width = 100;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "自增长键";
            this.columnHeader3.Width = 100;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "主键";
            // 
            // frm_Tool_ColumnDefine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(526, 340);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.groupBox1);
            this.Name = "frm_Tool_ColumnDefine";
            this.Text = "frm_Tool_ColumnDefine";
            this.Load += new System.EventHandler(this.frm_Tool_ColumnDefine_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_xml_datacolumns;
        private System.Windows.Forms.ToolStripMenuItem XmlToolStripMenuItem_xml_datapoint;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_xml_datasource;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_xml_grid;
        private System.Windows.Forms.ToolStripMenuItem PanelToolStripMenuItem_xml_editpanel;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
    }
}