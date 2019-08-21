using System;
using System.Collections.Generic;
using System.ComponentModel;
//using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using XmlProcess;
using WolfInv.Com.MetaDataCenter;
using Microsoft.VisualBasic;
using WolfInv.Com.AccessDataBase;
using WolfInv.Com.DataCenter;

namespace WCS
{
    public partial class frm_Tool_ColumnDefine : Form
    {
        public frm_Tool_ColumnDefine()
        {
            InitializeComponent();
        }

        private void toolStripMenuItem_xml_datacolumns_Click(object sender, EventArgs e)
        {
          
            ListViewItem lvi = this.listView1.SelectedItems[0];
            if (lvi == null) return;
            string aliasname = Microsoft.VisualBasic.Interaction.InputBox("表的缩写:", "别称", null,Screen.PrimaryScreen.WorkingArea.X ,Screen.PrimaryScreen.WorkingArea.Y);
           
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml("<flds/>");
            XmlNode root = xmldoc.SelectSingleNode("flds");
            DataIdenTable dit =lvi.Tag as DataIdenTable;
            dit.AName = aliasname;
            XmlNode node = xmldoc.CreateComment(string.Format("Table: {0} Alias Name:{1}", dit.TableName, dit.AName));
            root.AppendChild(node);
            foreach (DataColumn col in dit.Values)
            {
                string merge = string.Format("00000{0}", col.Index);
                col.DataPoint = col.OwnTable.AName.ToUpper() + merge.Substring(merge.Length - 5 + col.OwnTable.AName.Length );
                col.ToXml(root);
                //XmlUtil.AddAttribute(node, "id", this.OwnTable.AName + string.Format("00000{0}", this.Index).Substring(5 - this.OwnTable.AName.Length));

            }
            Clipboard.Clear();
            Clipboard.SetText(xmldoc.InnerXml);
            //SaveFile(xmldoc);
            Form frm = new Form();
            frm.Size = new Size(800, 600);
            TextBox tb = new TextBox();
            tb.Multiline = true;
            tb.Text = xmldoc.OuterXml;
            tb.Dock = DockStyle.Fill;
            frm.Controls.Add(tb);
            frm.ShowDialog(this);
        }

        void SaveFile(XmlDocument xmldoc)
        {
            if (MessageBox.Show("是否保存？", "文件", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "*.xml|(*.xml)";
                if (sfd.ShowDialog() == DialogResult.Yes)
                {
                    xmldoc.Save(sfd.FileName);
                }
            }
        }

        private void XmlToolStripMenuItem_xml_datapoint_Click(object sender, EventArgs e)
        {
            ListViewItem lvi = this.listView1.SelectedItems[0];
            if (lvi == null) return;
            
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml("<flds/>");
            XmlNode root = xmldoc.SelectSingleNode("flds");
            DataIdenTable dit = lvi.Tag as DataIdenTable;
            string aliasname = dit.AName;
            if (aliasname == null || aliasname.Trim().Length == 0)
                   aliasname = Microsoft.VisualBasic.Interaction.InputBox("表的缩写:", "别称", null, Screen.PrimaryScreen.WorkingArea.X, Screen.PrimaryScreen.WorkingArea.Y);

            dit.AName = aliasname;
            XmlNode node = xmldoc.CreateComment(string.Format("Table: {0} Alias Name:{1}", dit.TableName, dit.AName));
            root.AppendChild(node);
            foreach (DataColumn col in dit.Values)
            {
                string merge = string.Format("00000{0}", col.Index);
                col.DataPoint = col.OwnTable.AName.ToUpper() + merge.Substring(merge.Length + col.OwnTable.AName.Length -5);

                DataPoint dpt = new DataPoint();
                dpt.Name = col.DataPoint;
                dpt.Text = col.Column;
                
                dpt.DataType = col.DataType;
                switch (dpt.DataType)
                {
                    case "smallint":
                    case "int":
                    case "money":
                    case "float":
                        {
                            dpt.Width = 50;
                            break;
                        }
                    case "smalldatetime":
                    case "datetime":
                        {
                            dpt.Width = 100;
                            break;
                        }
                    case "varchar":
                    case "nvarchar":
                    case "text":
                    default :
                        {
                            if (col.Length < 80)
                                dpt.Width = 100;
                            else
                                dpt.Width = 200;
                            break ;
                        }
                }
                dpt.ToXml(root);
                //col.ToXml(root);
                //XmlUtil.AddAttribute(node, "id", this.OwnTable.AName + string.Format("00000{0}", this.Index).Substring(5 - this.OwnTable.AName.Length));

            }
            Clipboard.Clear();
            Clipboard.SetText(xmldoc.InnerXml);
            Form frm = new Form();
            frm.Size = new Size(800, 600);
            TextBox tb = new TextBox();
            tb.Multiline = true;
            tb.Text = xmldoc.OuterXml;
            tb.Dock = DockStyle.Fill;
            frm.Controls.Add(tb);
            frm.ShowDialog(this);
            //SaveFile(xmldoc);
        }

        private void frm_Tool_ColumnDefine_Load(object sender, EventArgs e)
        {
            List<DataChoiceItem> dcs = new List<DataChoiceItem>();
            List<ListViewItem> lvis = new List<ListViewItem>();
            DBAccessClass db = new DBAccessClass(DataAccessCenter.strConn);
            
            string sql = "select * from vw_comm_getAllTableViews";
            string sqltab = "select * from  vw_Comm_getAllColumns where TableName in ({0}) order by TableName,colid";
            db.Connect();
            System.Data.DataSet ds = null;
            string msg = db.GetResult(sql,ref ds);
            if (msg != null)
            {
                MessageBox.Show(msg);
                return;
            }
            if (ds == null)
            {
                return;
            }
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string table = ds.Tables[0].Rows[i][0].ToString();
                if (!DataAccessCenter.DataTables.ContainsKey(table))
                {
                    sb.AppendFormat(",'{0}'", table);
                }
            }
            string tabs = sb.ToString();
            if(!tabs.StartsWith(",")) return;
            tabs = tabs.Substring(1);
            sqltab = string.Format(sqltab, tabs);
            db.Connect();

             msg =  db.GetResult(sqltab,ref ds);
             if (msg != null)
             {
                 MessageBox.Show(msg);
                 return;
             }
            if (ds == null)
            {
                return;
            }
            Dictionary<string, DataIdenTable> newtables = new Dictionary<string, DataIdenTable>();

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                 
                System.Data.DataRow dr = ds.Tables[0].Rows[i];
                DataColumn dc = new DataColumn();
                dc.Table = dr["TableName"].ToString();
                dc.Column = dr["ColumnName"].ToString();
                dc.Index = dr["ColId"].ToString();
                dc.DataType = dr["TypeName"].ToString();
                int.TryParse(dr["Length"].ToString(), out dc.Length);
                dc.IsIden = !(dr["IsIden"].ToString()=="0");
                dc.IsKey = !(dr["IsKey"].ToString()=="0");
                DataIdenTable dit = new DataIdenTable();
                if (newtables.ContainsKey(dc.Table))
                {
                    dit = newtables[dc.Table];
                }
                else
                {
                    dit.TableName = dc.Table;
                    newtables.Add(dc.Table ,dit);
                }
                if (dit.ContainsKey(dc.Column))
                {
                    continue;
                }
                dit.Add(dc.Column, dc);
                dc.OwnTable = dit;
                
            }
            this.Tag = newtables;
            foreach (string tab in newtables.Keys)
            {
                DataChoiceItem dci = new DataChoiceItem();
                dci.Value = tab;
                dci.Text = tab;
                dcs.Add(dci);
                DataIdenTable dit = newtables[tab];
                string[] grids = new string[4];
                grids[0] = dit.TableName;
                grids[1] = dit.IsView?"是":"否";
                grids[2] = dit.IdenColumn;
                grids[3] = dit.Key;
                ListViewItem lvi = new ListViewItem(grids);
                lvi.Tag = dit;
                lvis.Add(lvi);
            }
            this.listView1.Items.Clear();
            this.listView1.Items.AddRange(lvis.ToArray());
            this.comboBox1.DataSource = dcs;
            this.comboBox1.DisplayMember = "Text";
            this.comboBox1.ValueMember = "Value";
            this.comboBox1.Tag = dcs;


        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.frm_Tool_ColumnDefine_Load(null, null);
        }
    }
}