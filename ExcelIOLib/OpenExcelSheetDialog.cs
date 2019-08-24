using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;

namespace WolfInv.Com.ExcelIOLib
{
    public partial class OpenExcelSheetDialog:Form
    {
        public string ExcelFileName
        {
            get
            {
                return openFileDialog1.FileName;
            }
        }

        public DialogResult DlgRes = DialogResult.Cancel;

        public string SheetName
        {
            get {
                return lbl_SelectSheet.Text;
            }
        }
        string excelfile = null;
        public OpenExcelSheetDialog()
        {

            InitializeComponent();
            openFileDialog1.Filter = "Excel文件|*.xls;*.xlsx;";
            //ShowDialog();
        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            if(this.listView2.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选择Sheet!");
                return;
            }
            this.lbl_SelectSheet.Text = this.listView2.SelectedItems[0].Text;
            DlgRes = DialogResult.OK;
            this.DialogResult = DlgRes;
            this.Close();
        }

        void readAllSheets(string strFileName)
        {
            object missing = System.Reflection.Missing.Value;
            Microsoft.Office.Interop.Excel.Application excel;// = new Application(); 
            _Workbook xBk;
            excel = new ApplicationClass();
            
            excel.Visible = false;
            excel.UserControl = true;
            Workbook wb = null;
            // 以只读的形式打开EXCEL文件  
            try
            {
                wb = excel.Application.Workbooks.Open(strFileName, missing, true, missing, missing, missing,
                                       missing, missing, missing, true, missing, missing, missing, missing, missing);
                excel.DisplayAlerts = false;
            }
            catch (Exception ce)
            {
                //wb.Close(false);
                //excel.Quit();
                excel = null;
                MessageBox.Show(ce.Message);
                return;
            }
            //取得第一个工作薄  
            Worksheet MainWs = null;// (Worksheet)wb.Worksheets.get_Item(1);//默认是第一个
            if (wb.Worksheets.Count == 0)
            {
                excel.Quit();
                excel = null;
                MessageBox.Show("空Excel!");
                this.Close();
                return;
            }
            
            this.listView2.Items.Clear();
            for (int i = 0; i < wb.Worksheets.Count; i++)
            {
                Worksheet st = (Worksheet)wb.Worksheets[i + 1];
                string mcs = st.Name;
                this.listView2.Items.Add(mcs);
            }
            excel.DisplayAlerts = false;
            wb.Close(false);
            excel.Quit();
            ExcelCommLib.Kill(excel);
            excel = null;
        }

        private void listView2_DoubleClick(object sender, EventArgs e)
        {
            if (this.listView2.SelectedItems.Count == 0) return;


            ListViewItem lvi = this.listView2.SelectedItems[0];
            this.lbl_SelectSheet.Text = lvi.Text;
            DlgRes = DialogResult.OK;
            this.DialogResult = DlgRes;
            this.Close();
        }

        private void OpenExcelSheetDialog_Load(object sender, EventArgs e)
        {
            this.Visible = false;
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr == DialogResult.Cancel)
            {
                this.Close();
                this.Dispose();
                DlgRes = dr;
                this.Close();
                return;
            }
            this.Cursor = Cursors.WaitCursor;
            readAllSheets(openFileDialog1.FileName);
            this.Visible = true;
            this.Cursor = Cursors.Default;
            if(this.listView2.Items.Count == 1)
            {
                this.listView2.Items[0].Selected = true;
                this.button1_Click(null, null);
            }
            //System.Windows.Forms.Application.Run(this);
        }
    }
}
