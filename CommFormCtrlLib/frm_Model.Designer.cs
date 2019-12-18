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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_Model));
            this.panel_Title = new System.Windows.Forms.Panel();
            this.btn_close = new System.Windows.Forms.PictureBox();
            this.tlb_Title = new WolfInv.Com.XPlatformCtrlLib.XWinForm_Label();
            this.panel_bottom = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label_buttom = new System.Windows.Forms.Label();
            this.panel_main = new WolfInv.Com.XPlatformCtrlLib.XWinForm_Panel();
            this.toolStrip1 = new WolfInv.Com.CommFormCtrlLib.ToolBarStrip();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.panel_Title.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btn_close)).BeginInit();
            this.panel_bottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_Title
            // 
            this.panel_Title.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel_Title.Controls.Add(this.btn_close);
            this.panel_Title.Controls.Add(this.tlb_Title);
            this.panel_Title.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_Title.Location = new System.Drawing.Point(0, 0);
            this.panel_Title.Margin = new System.Windows.Forms.Padding(6);
            this.panel_Title.Name = "panel_Title";
            this.panel_Title.Size = new System.Drawing.Size(1200, 42);
            this.panel_Title.TabIndex = 0;
            this.panel_Title.DoubleClick += new System.EventHandler(this.panel1_DoubleClick);
            // 
            // btn_close
            // 
            this.btn_close.Dock = System.Windows.Forms.DockStyle.Right;
            this.btn_close.Image = ((System.Drawing.Image)(resources.GetObject("btn_close.Image")));
            this.btn_close.Location = new System.Drawing.Point(1158, 0);
            this.btn_close.Margin = new System.Windows.Forms.Padding(10);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(42, 42);
            this.btn_close.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.btn_close.TabIndex = 1;
            this.btn_close.TabStop = false;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // tlb_Title
            // 
            this.tlb_Title.AutoSize = true;
            this.tlb_Title.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tlb_Title.ForeColor = System.Drawing.Color.White;
            this.tlb_Title.Location = new System.Drawing.Point(12, 6);
            this.tlb_Title.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.tlb_Title.Name = "tlb_Title";
            this.tlb_Title.Size = new System.Drawing.Size(102, 27);
            this.tlb_Title.TabIndex = 0;
            this.tlb_Title.Text = "label1";
            // 
            // panel_bottom
            // 
            this.panel_bottom.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel_bottom.Controls.Add(this.label1);
            this.panel_bottom.Controls.Add(this.label_buttom);
            this.panel_bottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel_bottom.Location = new System.Drawing.Point(0, 848);
            this.panel_bottom.Margin = new System.Windows.Forms.Padding(6);
            this.panel_bottom.Name = "panel_bottom";
            this.panel_bottom.Size = new System.Drawing.Size(1200, 52);
            this.panel_bottom.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 24);
            this.label1.TabIndex = 1;
            this.label1.Text = "项目";
            this.label1.Visible = false;
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
            // panel_main
            // 
            this.panel_main.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_main.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel_main.CurrMainControl = null;
            this.panel_main.InForm = false;
            this.panel_main.Location = new System.Drawing.Point(0, 105);
            this.panel_main.Margin = new System.Windows.Forms.Padding(6);
            this.panel_main.Name = "panel_main";
            this.panel_main.Size = new System.Drawing.Size(1194, 741);
            this.panel_main.TabIndex = 1;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.CanOverflow = false;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip1.Location = new System.Drawing.Point(17, 52);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.toolStrip1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.toolStrip1.Size = new System.Drawing.Size(1177, 47);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // frm_Model
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.panel_bottom);
            this.Controls.Add(this.panel_main);
            this.Controls.Add(this.panel_Title);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "frm_Model";
            this.Size = new System.Drawing.Size(1200, 900);
            this.Load += new System.EventHandler(this.frm_Model_Load);
            this.panel_Title.ResumeLayout(false);
            this.panel_Title.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btn_close)).EndInit();
            this.panel_bottom.ResumeLayout(false);
            this.panel_bottom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Panel panel_Title;
        public XWinForm_Panel panel_main;
        public System.Windows.Forms.Panel panel_bottom;
        //public System.Windows.Forms.Label lb_Title;
        
        public ToolBarStrip toolStrip1;
        public System.Windows.Forms.Label label_buttom;
        private PictureBox btn_close;
        private Label label1;
        protected ContextMenuStrip contextMenuStrip1;
        //public Label lb_Title { get; set; }
    }
}
