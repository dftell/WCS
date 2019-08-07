using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using WolfInv.Com.WCS_Process;
using WolfInv.Com.MetaDataCenter;
using XmlProcess;
using WolfInv.Com.CommFormCtrlLib;
using WolfInv.Com.CommCtrlLib;
namespace WCS
{
    public partial class frm_EditView : Form,IUserData
    {
        DataSource ds;
        Dictionary<string,List<DataPoint>> DataPoints;
        Dictionary<string, DataPoint> srclist;
        Dictionary<string, ViewItem> dstlist;
        public frm_EditView()
        {
            InitializeComponent();
        }

        public frm_EditView(string dsname,frm_Model frm, Dictionary<string,ViewItem> orgViews)
        {
            InitializeComponent();
            if (GlobalShare.mapDataSource.ContainsKey(dsname))
            {
                ds = GlobalShare.mapDataSource[dsname];
            }
            this.Tag = ds;
            this.btn_Save.Tag = frm;
            dstlist = orgViews as Dictionary<string,ViewItem>;
        }

        private void frm_EditView_Load(object sender, EventArgs e)
        {
            if (ds == null) ds = this.Tag as DataSource;
            if (ds == null) return;
            DataPoints = GlobalShare.DataCenterClientObj.GetViewDataPointList(ds);
            this.comboBox_datasources.Tag = DataPoints;
            this.comboBox_datasources.Items.Clear();
            foreach (string name in DataPoints.Keys)
            {
                this.comboBox_datasources.Items.Add(name);
            }
            if (this.comboBox_datasources.Items.Count > 0)
            {
                this.comboBox_datasources.SelectedIndex = 0;
            }
            RefreshGrid(this.dstlist, this.listView_dist);
        }

        
        private void comboBox_datasources_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if(DataPoints == null)
                DataPoints = this.comboBox_datasources.Tag as Dictionary<string, List<DataPoint>>;
            if (DataPoints == null) return;
            this.label_tag.Tag = DataPoints[comboBox_datasources.Text];
            srclist = InitList(DataPoints[comboBox_datasources.Text]);
            RefreshGrid(srclist, this.listView_src);
        }

       
        Dictionary<string,DataPoint> InitList(List<DataPoint> dps)
        {
            Dictionary<string, DataPoint> ret = new Dictionary<string, DataPoint>();
            for (int i = 0; i < dps.Count; i++)
            {
                if (!GlobalShare.DataPointMappings.ContainsKey(dps[i].Name))
                {
                    continue;
                }
                DataPoint dp = GlobalShare.DataPointMappings[dps[i].Name];
                if (ret.ContainsKey(dp.Name)) continue;
                ret.Add(dp.Name, dp);
            }
            return ret;
        }

        void RefreshGrid(Dictionary<string,DataPoint> dps,ListView lv)
        {
            lv.Items.Clear();
            //srclist = new Dictionary<string, DataPoint>();
            foreach (DataPoint dp in dps.Values)
            {
                ListGridItem lvi = new ListGridItem();
                lvi.Text = dp.Text;
                lvi.Tag = dp;
                lv.Items.Add(lvi);
            }
            lv.Tag = dps;
        }

        void RefreshGrid(Dictionary<string, ViewItem> dps, ListView lv)
        {
            lv.Items.Clear();
            //srclist = new Dictionary<string, ViewItem>();
            foreach (ViewItem dp in dps.Values)
            {
                
                string[] grids = new string[5];
                grids[0] = dp.Text;
                grids[1] = dp.Width.ToString();
                grids[2] = dp.IsKeyText?"是":"否";
                grids[3] = dp.IsKeyValue ? "是" : "否";
                grids[4] = dp.Visable ? "是" : "否";
                ListGridItem lvi = new ListGridItem(grids);
                lvi.Text = dp.Text;
                lvi.Tag = dp;
                lv.Items.Add(lvi);
            }
            lv.Tag = dps;
        }


        private void btn_Add_Click(object sender, EventArgs e)
        {
            if (this.listView_src.SelectedItems.Count == 0) return;
            List<ListViewItem> lvis = new List<ListViewItem>();
            for (int i = 0; i < this.listView_src.SelectedItems.Count; i++)
            {
                lvis.Add(this.listView_src.SelectedItems[i]);
            }
            DataPointToView(lvis);
        }

        void DataPointToView(List<ListViewItem> list)
        {
            Dictionary<string,DataPoint> dps = this.listView_src.Tag as Dictionary<string,DataPoint>;
            Dictionary<string,ViewItem> vis = this.listView_dist.Tag as Dictionary<string,ViewItem>;
            for (int i = 0; i < list.Count; i++)
            {
                DataPoint dpt = list[i].Tag as DataPoint;
                dps.Remove(dpt.Name);

                dps.Remove(list[i].Name);
                ViewItem vi = new ViewItem();
                vi.GetItem(dpt);
                if (vis.ContainsKey(vi.Name)) continue;
                vis.Add(vi.Name, vi);
            }
            RefreshGrid(dps, this.listView_src);
            RefreshGrid(vis, this.listView_dist);
        }

        private void btn_AddAll_Click(object sender, EventArgs e)
        {
            if (this.listView_src.Items.Count == 0) return;
            List<ListViewItem> lvis = new List<ListViewItem>();
            for (int i = 0; i < this.listView_src.Items.Count; i++)
            {
                lvis.Add(this.listView_src.Items[i]);
            }
            DataPointToView(lvis);
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            frm_Model frm = this.btn_Save.Tag as frm_Model;
            string strFilePath = "";
   
            strFilePath = string.Format("{0}\\{1}\\frm_{1}_{2}_{3}.xml", "", frm.strModule, frm.strScreen, frm.strTarget);

            XmlDocument xmldoc = GlobalShare.UpdateWithUseInfo(GlobalShare.GetXmlFile(strFilePath),strUid) as XmlDocument;
            XmlNode node = xmldoc.SelectSingleNode("root/Grid/cols");
            XmlNode nodegrid;
            if (node == null)
            {
                nodegrid = xmldoc.SelectSingleNode("root/Grid");
                if (nodegrid == null)
                {
                    nodegrid = xmldoc.CreateElement("Grid");
                    xmldoc.SelectSingleNode(".").AppendChild(nodegrid);
                }

            }
            else
            {
                nodegrid = node.ParentNode;
                nodegrid.RemoveChild(node);
            }
            node = XmlUtil.AddSubNode(nodegrid, "cols");
            for (int i = 0; i < this.listView_dist.Items.Count; i++)
            {
                ViewItem vi = this.listView_dist.Items[i].Tag as ViewItem;
                vi.ToXml(node);
            }
            xmldoc.Save(strFilePath);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btn_Cancle_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }



        #region IUserData 成员

        string _uid;
        public string strUid { get { return _uid; } set { _uid = value; } }
        #endregion
    }
}