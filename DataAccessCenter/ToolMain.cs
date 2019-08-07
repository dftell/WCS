using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WolfInv.Com.DataCenter
{
    public partial class ToolMain : Form
    {
        public ToolMain()
        {
            InitializeComponent();
        }

        private void toolStripButton_DataSource_Click(object sender, EventArgs e)
        {
            frm_Tool_SourceTest frm = new frm_Tool_SourceTest();
            frm.Show();
        }

        private void toolStripButton_datapoints_Click(object sender, EventArgs e)
        {
            frm_Tool_ColumnDefine frm = new frm_Tool_ColumnDefine();
            frm.Show();
        }
    }
}