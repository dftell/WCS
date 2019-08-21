using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using WolfInv.Com.MetaDataCenter;
using System.IO;
using WolfInv.Com.DataCenter;

namespace WCS
{
    public partial class frm_Tool_SourceTest : Form
    {
        public frm_Tool_SourceTest()
        {
            InitializeComponent();
        }

        private void btn_GetSql_Click(object sender, EventArgs e)
        {
            this.txt_sql.Text = "";
            this.txt_sql.Text = GetSql(this.txt_datasource.Text);
        }

        

        string GetSql(string req)
        {
            XmlDocument xmldoc = new XmlDocument();
            try
            {
                xmldoc.LoadXml(req);
                SqlBuilder sb = new SqlBuilder(new DataRequest(xmldoc));
                string strSql = sb.GenerateQuerySql();
                return strSql;
            }
            catch (Exception ce)
            {
                return ce.Message;
            }
            
        }

        private void Btn_GetXml_Click(object sender, EventArgs e)
        {
            this.txt_sql.Text = "";
            string msg = null;
            XmlDocument xmldoc = new XmlDocument();
            try
            {
                xmldoc.LoadXml(this.txt_datasource.Text);
            }
            catch (Exception ce)
            {
                this.txt_sql.Text = ce.Message;
                return;
            }
            DataSet ds = DataAccessCenter.GetDataList(xmldoc, out msg);
            if (msg != null)
            {
                this.txt_sql.Text = msg;
                return;
            }
            StringBuilder strbld = new StringBuilder();
            TextWriter txtWrt = new StringWriter(strbld);
            try
            {
                ds.WriteXml(txtWrt, XmlWriteMode.WriteSchema);
                this.txt_sql.Text =  strbld.ToString();
            }
            catch (Exception ce)
            {
                msg = ce.Message;
                this.txt_sql.Text = msg;
                return;
            }

        }
    }
}