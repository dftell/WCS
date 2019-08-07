using WolfInv.Com.XPlatformCtrlLib;
using System.Windows.Forms;
namespace WolfInv.Com.CommFormCtrlLib
{
    partial class frm_Model
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_close = new System.Windows.Forms.Button();
            
            this.panel_bottom = new System.Windows.Forms.Panel();
            this.label_buttom = new System.Windows.Forms.Label();
            this.toolStrip1 = new WolfInv.Com.CommFormCtrlLib.ToolBarStrip();
            this.panel1.SuspendLayout();
            this.panel_bottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel1.Controls.Add(this.btn_close);
            this.panel1.Controls.Add(this.lb_Title);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1200, 42);
            this.panel1.TabIndex = 0;
            this.panel1.DoubleClick += new System.EventHandler(this.panel1_DoubleClick);
            // 
            // btn_close
            // 
            this.btn_close.BackColor = System.Drawing.SystemColors.ControlDark;
            this.btn_close.Dock = System.Windows.Forms.DockStyle.Right;
            this.btn_close.Location = new System.Drawing.Point(1160, 0);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(40, 42);
            this.btn_close.TabIndex = 1;
            this.btn_close.Text = "X";
            this.btn_close.UseVisualStyleBackColor = false;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // lb_Title
            // 
            this.lb_Title.AutoSize = true;
            this.lb_Title.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lb_Title.ForeColor = System.Drawing.Color.White;
            this.lb_Title.Location = new System.Drawing.Point(12, 6);
            this.lb_Title.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lb_Title.Name = "lb_Title";
            this.lb_Title.Size = new System.Drawing.Size(102, 27);
            this.lb_Title.TabIndex = 0;
            this.lb_Title.Text = "label1";
            // 
            // panel_main
            // 
            (this.panel_main as Panel).Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            (this.panel_main as Panel).BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            (this.panel_main as Panel).Location = new System.Drawing.Point(8, 98);
            (this.panel_main as Panel).Margin = new System.Windows.Forms.Padding(6);
            (this.panel_main as Panel).Name = "panel_main";
            (this.panel_main as Panel).Size = new System.Drawing.Size(1174, 756);
            (this.panel_main as Panel).TabIndex = 1;
            // 
            // panel_bottom
            // 
            this.panel_bottom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_bottom.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel_bottom.Controls.Add(this.label_buttom);
            this.panel_bottom.Location = new System.Drawing.Point(12, 880);
            this.panel_bottom.Margin = new System.Windows.Forms.Padding(6);
            this.panel_bottom.Name = "panel_bottom";
            this.panel_bottom.Size = new System.Drawing.Size(1178, 52);
            this.panel_bottom.TabIndex = 2;
            // 
            // label_buttom
            // 
            this.label_buttom.AutoSize = true;
            this.label_buttom.Location = new System.Drawing.Point(16, 16);
            this.label_buttom.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label_buttom.Name = "label_buttom";
            this.label_buttom.Size = new System.Drawing.Size(0, 24);
            this.label_buttom.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip1.Location = new System.Drawing.Point(0, 42);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.toolStrip1.Size = new System.Drawing.Size(1200, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // frm_Model
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.panel_bottom);
            this.Controls.Add(this.panel_main as System.Windows.Forms.Panel);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "frm_Model";
            this.Size = new System.Drawing.Size(1200, 940);
            this.Load += new System.EventHandler(this.frm_Model_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel_bottom.ResumeLayout(false);
            this.panel_bottom.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Panel panel1;
        public IXPanel panel_main;
        public System.Windows.Forms.Panel panel_bottom;
        //public System.Windows.Forms.Label lb_Title;
        
        public ToolBarStrip toolStrip1;
        public System.Windows.Forms.Label label_buttom;
        private System.Windows.Forms.Button btn_close;
    }
}
