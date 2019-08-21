using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WCS
{
    public partial class frm_Tool_SelectSource : Form
    {
        public frm_Tool_SelectSource()
        {
            InitializeComponent();
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {

        }

        private void frm_Tool_ViewDesigner_Load(object sender, EventArgs e)
        {
            this.treeView1.Nodes.Clear();
            string path  = AppDomain.CurrentDomain.BaseDirectory;
            string[] folds =  Directory.GetDirectories(path);
            for(int i=0;i<folds.Length;i++)
            {
                string strFolderName = folds[i];
                TreeNode tn = this.treeView1.Nodes.Add(folds[i].Replace(path,""));
                string[] xmlfiles =  Directory.GetFiles(strFolderName, "*.xml");
                for(int j=0;j<xmlfiles.Length;j++)
                {
                    TreeNode dn = tn.Nodes.Add(xmlfiles[j].Replace(strFolderName,""));
                    dn.Tag = xmlfiles[j];
                }
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag == null)
                return;
            this.lbl_EditFile.Text = e.Node.Tag.ToString();
        }
    }
}
