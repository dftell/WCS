using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WolfInv.Com.jdyInterfaceLib;
namespace testWCS
{
    public partial class form2 : Form
    {
        public form2()
        {
            InitializeComponent();
        }

        private void btn_request_Click(object sender, EventArgs e)
        {
            this.txt_url.Text = "";
            this.txt_result.Text = "正在准备请求！";
            ////jdy_GlbObject.ResetAccess();
            ////string ret = jdy_GlbObject.Access_token;
            ////this.txt_req_name.Text = ret;
            ////this.txt_bdId.Text = jdy_GlbObject.bdId.ToString();

            JDYSCM_Bussiness_Class jdy = ddl_className.Tag as JDYSCM_Bussiness_Class;
            if(jdy == null)
            {
                return;
            }
            jdy.InitRequestJson();
            jdy.Req_PostData = this.txt_PostData.Text;
            this.txt_url.Text = jdy.getUrl();
            this.txt_result.Text = this.chkbox_Post.Checked?jdy.PostRequest():jdy.GetRequest();
            
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            string ret = jdy_GlbObject.Access_token;
            this.txt_req_name.Text = ret;
            this.txt_bdId.Text = jdy_GlbObject.dbId;
            DataTable dt = new DataTable();
            dt.Columns.Add("text");
            dt.Columns.Add("value");
            jdy_GlbObject.AllModuleClass.Values.ToList().ForEach(a =>
            {
                DataRow dr = dt.NewRow();
                dr["text"] = a.Name;
                dr["value"] = a;
                dt.Rows.Add(dr);
            });
            this.ddl_className.DisplayMember = "text";
            this.ddl_className.ValueMember = "value";
            this.ddl_className.DataSource = dt;
            this.ddl_className.Tag = dt;

        }

        private void ddl_className_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_className.SelectedIndex < 0)
            {
                return;
            }
            Type cls = jdy_GlbObject.AllModuleClass[ddl_className.Text];
            object obj = Activator.CreateInstance(cls);// as;
            JDYSCM_Bussiness_Class jdy = obj as JDYSCM_Bussiness_Class;// as
            //JDYSCM_Class jdy = jdy_GlbObject.AllModuleClass[ddl_className.SelectedValue.ToString()];
            if (jdy == null)
                return;
            jdy.InitClass(jdy_GlbObject.mlist[ddl_className.Text]);
            jdy.InitRequestJson();
            if (jdy is JDYSCM_Bussiness_List_Class)
            {
                (jdy as JDYSCM_Bussiness_List_Class).filter = new JDYSCM_Bussiness_List_Class.JDYSCM_Bussiness_Filter_Class();
                (jdy as JDYSCM_Bussiness_List_Class).filter.pageSize = int.Parse(txt_PageSize.Text);
                (jdy as JDYSCM_Bussiness_List_Class).filter.page = int.Parse(txt_PageNo.Text);
                jdy.Req_PostData = "{\"filter\":" + (jdy as JDYSCM_Bussiness_List_Class).filter.ToJson().Replace("null", "\"\"") + "}";
            }
            this.txt_url.Text = jdy.getUrl();
            this.txt_PostData.Text = jdy.Req_PostData;
            this.ddl_className.Tag = jdy;
        }
    }
}
