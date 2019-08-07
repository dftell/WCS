using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WolfInv.Com.ExcelIOLib;
using System.IO;
namespace testWCS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ExcelSheetDefineClass obj =  new ExcelSheetDefineClass();
            string strPath = this.txt_SavePath.Text.Trim();
            if(File.Exists(strPath))
            {
                string str = File.ReadAllText(strPath);
                obj = obj.GetFromJson<ExcelSheetDefineClass>(str);
            }
            this.propertyGrid1.SelectedObject = obj;

        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            ExcelSheetDefineClass esd = this.propertyGrid1.SelectedObject as ExcelSheetDefineClass;
            string str = esd.ToJson();
            StreamWriter sw = new StreamWriter(txt_SavePath.Text, false);
            try
            {
                
                sw.Write(str);
                sw.Flush();
            }
            catch (Exception ce)
            {
                MessageBox.Show(ce.Message);
                return;
            }
            finally
            {
                sw.Close();
            }
            
        }

        private void btn_Import_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            string fPath = null;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                fPath = ofd.FileName;
            }
            else
            {
                MessageBox.Show("请先选择需要导入的文件！");
                return;
            }
            this.Cursor = Cursors.WaitCursor;
            ExcelSheetDefineClass esdc = this.propertyGrid1.SelectedObject as ExcelSheetDefineClass;
            ExcelDefineReader edr = new ExcelDefineReader(esdc);
            ReadResult ret = edr.GetResult(fPath);
            this.Cursor = Cursors.Default;
            if (ret.Succ)
            {
                MessageBox.Show(ret.Result.OuterXml);
            }
            else
            {
                MessageBox.Show(ret.Message);
            }
        }
    }
}
