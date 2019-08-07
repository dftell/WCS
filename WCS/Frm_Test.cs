using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WCS
{
    public partial class Frm_Test : Form
    {
        public Frm_Test()
        {
            InitializeComponent();
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            Button btn = new Button();
            btn.Text = string.Format("{0},{1}",txt_col.Text ,txt_row.Text);
            this.tableLayoutPanel1.Controls.Add(btn, int.Parse(this.txt_col.Text), int.Parse(this.txt_row.Text));
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            this.tableLayoutPanel1.Controls.Clear();
            this.tableLayoutPanel1.RowCount = 9;
            this.tableLayoutPanel1.ColumnCount = 9;
        }
    }
}