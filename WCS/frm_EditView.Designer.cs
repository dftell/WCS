namespace WCS
{
    partial class frm_EditView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_EditView));
            this.listView_src = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.btn_Cancle = new System.Windows.Forms.Button();
            this.btn_Save = new System.Windows.Forms.Button();
            this.comboBox_datasources = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label_tag = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.listView_dist = new System.Windows.Forms.ListView();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.btn_MoveTop = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btn_Add = new System.Windows.Forms.Button();
            this.btn_AddAll = new System.Windows.Forms.Button();
            this.btn_MoveUp = new System.Windows.Forms.Button();
            this.btn_MoveDown = new System.Windows.Forms.Button();
            this.btn_MoveButtom = new System.Windows.Forms.Button();
            this.btn_setting = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // listView_src
            // 
            this.listView_src.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.listView_src.FullRowSelect = true;
            this.listView_src.Location = new System.Drawing.Point(12, 136);
            this.listView_src.Name = "listView_src";
            this.listView_src.Size = new System.Drawing.Size(405, 109);
            this.listView_src.TabIndex = 0;
            this.listView_src.UseCompatibleStateImageBehavior = false;
            this.listView_src.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "名称";
            this.columnHeader1.Width = 301;
            // 
            // btn_Cancle
            // 
            this.btn_Cancle.Location = new System.Drawing.Point(423, 466);
            this.btn_Cancle.Name = "btn_Cancle";
            this.btn_Cancle.Size = new System.Drawing.Size(72, 22);
            this.btn_Cancle.TabIndex = 1;
            this.btn_Cancle.Text = "取消";
            this.btn_Cancle.UseVisualStyleBackColor = true;
            this.btn_Cancle.Click += new System.EventHandler(this.btn_Cancle_Click);
            // 
            // btn_Save
            // 
            this.btn_Save.Location = new System.Drawing.Point(347, 466);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(72, 22);
            this.btn_Save.TabIndex = 2;
            this.btn_Save.Text = "保存";
            this.btn_Save.UseVisualStyleBackColor = true;
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // comboBox_datasources
            // 
            this.comboBox_datasources.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_datasources.FormattingEnabled = true;
            this.comboBox_datasources.Location = new System.Drawing.Point(59, 37);
            this.comboBox_datasources.Name = "comboBox_datasources";
            this.comboBox_datasources.Size = new System.Drawing.Size(358, 20);
            this.comboBox_datasources.TabIndex = 3;
            this.comboBox_datasources.SelectedIndexChanged += new System.EventHandler(this.comboBox_datasources_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "数据源";
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(59, 64);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(358, 20);
            this.comboBox2.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "分类";
            // 
            // label_tag
            // 
            this.label_tag.AutoSize = true;
            this.label_tag.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_tag.Location = new System.Drawing.Point(9, 12);
            this.label_tag.Name = "label_tag";
            this.label_tag.Size = new System.Drawing.Size(83, 12);
            this.label_tag.TabIndex = 7;
            this.label_tag.Text = "可用数据列表";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(12, 97);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "有效数据点";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 117);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "共计数据点：";
            // 
            // listView_dist
            // 
            this.listView_dist.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6});
            this.listView_dist.FullRowSelect = true;
            this.listView_dist.Location = new System.Drawing.Point(14, 316);
            this.listView_dist.Name = "listView_dist";
            this.listView_dist.Size = new System.Drawing.Size(405, 110);
            this.listView_dist.TabIndex = 10;
            this.listView_dist.UseCompatibleStateImageBehavior = false;
            this.listView_dist.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "名称";
            this.columnHeader2.Width = 96;
            // 
            // btn_MoveTop
            // 
            this.btn_MoveTop.ImageIndex = 0;
            this.btn_MoveTop.ImageList = this.imageList1;
            this.btn_MoveTop.Location = new System.Drawing.Point(423, 316);
            this.btn_MoveTop.Name = "btn_MoveTop";
            this.btn_MoveTop.Size = new System.Drawing.Size(72, 22);
            this.btn_MoveTop.TabIndex = 11;
            this.btn_MoveTop.UseVisualStyleBackColor = true;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Find_RemoveAll.gif");
            this.imageList1.Images.SetKeyName(1, "Find_Remove.gif");
            this.imageList1.Images.SetKeyName(2, "Find_Add.gif");
            this.imageList1.Images.SetKeyName(3, "Find_AddAll.gif");
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 293);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 12);
            this.label6.TabIndex = 16;
            this.label6.Text = "共计数据点：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(15, 273);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(70, 12);
            this.label7.TabIndex = 15;
            this.label7.Text = "选择数据点";
            // 
            // btn_Add
            // 
            this.btn_Add.Location = new System.Drawing.Point(197, 251);
            this.btn_Add.Name = "btn_Add";
            this.btn_Add.Size = new System.Drawing.Size(108, 22);
            this.btn_Add.TabIndex = 17;
            this.btn_Add.Text = "添加";
            this.btn_Add.UseVisualStyleBackColor = true;
            this.btn_Add.Click += new System.EventHandler(this.btn_Add_Click);
            // 
            // btn_AddAll
            // 
            this.btn_AddAll.Location = new System.Drawing.Point(311, 251);
            this.btn_AddAll.Name = "btn_AddAll";
            this.btn_AddAll.Size = new System.Drawing.Size(106, 22);
            this.btn_AddAll.TabIndex = 18;
            this.btn_AddAll.Text = "全部添加";
            this.btn_AddAll.UseVisualStyleBackColor = true;
            this.btn_AddAll.Click += new System.EventHandler(this.btn_AddAll_Click);
            // 
            // btn_MoveUp
            // 
            this.btn_MoveUp.ImageIndex = 1;
            this.btn_MoveUp.ImageList = this.imageList1;
            this.btn_MoveUp.Location = new System.Drawing.Point(423, 344);
            this.btn_MoveUp.Name = "btn_MoveUp";
            this.btn_MoveUp.Size = new System.Drawing.Size(72, 22);
            this.btn_MoveUp.TabIndex = 19;
            this.btn_MoveUp.UseVisualStyleBackColor = true;
            // 
            // btn_MoveDown
            // 
            this.btn_MoveDown.ImageIndex = 2;
            this.btn_MoveDown.ImageList = this.imageList1;
            this.btn_MoveDown.Location = new System.Drawing.Point(423, 372);
            this.btn_MoveDown.Name = "btn_MoveDown";
            this.btn_MoveDown.Size = new System.Drawing.Size(72, 22);
            this.btn_MoveDown.TabIndex = 20;
            this.btn_MoveDown.UseVisualStyleBackColor = true;
            // 
            // btn_MoveButtom
            // 
            this.btn_MoveButtom.ImageIndex = 3;
            this.btn_MoveButtom.ImageList = this.imageList1;
            this.btn_MoveButtom.Location = new System.Drawing.Point(423, 400);
            this.btn_MoveButtom.Name = "btn_MoveButtom";
            this.btn_MoveButtom.Size = new System.Drawing.Size(72, 22);
            this.btn_MoveButtom.TabIndex = 21;
            this.btn_MoveButtom.UseVisualStyleBackColor = true;
            // 
            // btn_setting
            // 
            this.btn_setting.Location = new System.Drawing.Point(83, 432);
            this.btn_setting.Name = "btn_setting";
            this.btn_setting.Size = new System.Drawing.Size(108, 22);
            this.btn_setting.TabIndex = 22;
            this.btn_setting.Text = "设置";
            this.btn_setting.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(311, 432);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(108, 22);
            this.button3.TabIndex = 23;
            this.button3.Text = "全部删除";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(197, 432);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(108, 22);
            this.button4.TabIndex = 24;
            this.button4.Text = "删除";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "宽度";
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "关键值";
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "关键文本";
            this.columnHeader5.Width = 72;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "可视";
            // 
            // frm_EditView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(507, 500);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.btn_setting);
            this.Controls.Add(this.btn_MoveButtom);
            this.Controls.Add(this.btn_MoveDown);
            this.Controls.Add(this.btn_MoveUp);
            this.Controls.Add(this.btn_AddAll);
            this.Controls.Add(this.btn_Add);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.btn_MoveTop);
            this.Controls.Add(this.listView_dist);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label_tag);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox_datasources);
            this.Controls.Add(this.btn_Save);
            this.Controls.Add(this.btn_Cancle);
            this.Controls.Add(this.listView_src);
            this.Name = "frm_EditView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frm_EditView";
            this.Load += new System.EventHandler(this.frm_EditView_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView_src;
        private System.Windows.Forms.Button btn_Cancle;
        private System.Windows.Forms.Button btn_Save;
        private System.Windows.Forms.ComboBox comboBox_datasources;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label_tag;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ListView listView_dist;
        private System.Windows.Forms.Button btn_MoveTop;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btn_Add;
        private System.Windows.Forms.Button btn_AddAll;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button btn_MoveUp;
        private System.Windows.Forms.Button btn_MoveDown;
        private System.Windows.Forms.Button btn_MoveButtom;
        private System.Windows.Forms.Button btn_setting;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
    }
}