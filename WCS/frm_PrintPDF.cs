using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WolfInv.Com.CommFormCtrlLib;
using WolfInv.Com.CommCtrlLib;
using WolfInv.Com.MetaDataCenter;
using WolfInv.Com.WCS_Process;
using System.Xml;
using System.IO;
using System.Drawing.Printing;
using System.Drawing.Design;
namespace WCS
{
    public partial class frm_PrintPDF :frm_Model, IMutliDataInterface
    {
        public frm_PrintPDF()
        {
            InitializeComponent();
            
        }

        
        ////List<UpdateData> _injectedatas;
        ////public List<UpdateData> InjectedDatas
        ////{
        ////    get { return _injectedatas; }
        ////    set { _injectedatas = value; }
        ////}
        private void frm_PrintPDF_Load(object sender, EventArgs e)
        {
            Application.DoEvents();
            string msg = null;
            XmlDocument xmldoc = new XmlDocument();
            this.BringToFront();
            string[] strImports = this.DetailSource.Split(':');
            if (strImports.Length > 1 && strImports[0] == "Import")//
            {
                string strXmlPath = string.Format("{0}\\{1}\\{4}_{1}_{2}_{3}.xml", "", this.strModule, this.strScreen, this.strTarget, "imp");
                XmlDocument xmlfile = GlobalShare.UpdateWithUseInfo(GlobalShare.GetXmlFile(strXmlPath), strUid) as XmlDocument;
                OpenFileDialog fd = new OpenFileDialog();
                fd.Multiselect = false;
                fd.CheckPathExists = true;
                //fd.Filter = "*.xls;*.xlsx";
                if(fd.ShowDialog() == DialogResult.OK)
                {
                    string selectfile = fd.FileName;
                    ImportProcessClass ipc = new ImportProcessClass(xmlfile, ImportType.XLS);
                    xmldoc = ipc.getImportData(selectfile);
                   
                }
                else
                {
                    MessageBox.Show("必须指定打印文件！");
                    return;
                }


            }
            else
            {


                if (this.InjectedDatas == null || this.InjectedDatas.Count == 0)
                {
                    DataSet GridData = InitDataSource(this.DetailSource, out msg);
                    if (GridData != null)
                    {
                        xmldoc.LoadXml(GridData.GetXml());
                    }
                }
                else
                {
                    xmldoc.LoadXml("<root/>");
                    XmlNode root = xmldoc.SelectSingleNode("root");
                    InjectedDatas.ForEach(
                        a =>
                        {
                            XmlNode node = a.ToXml(root);

                        }
                        );

                }
            }
            //this.Refresh();
            //for test
            xmldoc = GlobalShare.GetXmlFile("Tools/test.xml");
            if(xmldoc == null)
            {
                MessageBox.Show("数据为空！");
                return;
            }
            string strXslPath = string.Format("{0}\\{1}\\{4}_{1}_{2}_{3}.xsl", "", this.strModule, this.strScreen, this.strTarget, "pdf");
            XmlDocument xsldoc = GlobalShare.UpdateWithUseInfo(GlobalShare.GetXmlFile(strXslPath), strUid) as XmlDocument;
            string pdfpath = string.Format("{0}.html",Path.GetTempFileName());
            if (!PDFGenerator.XMLToPDF(xmldoc, xsldoc, pdfpath))
            {
                MessageBox.Show("打印文件失败！");
            }

            ////////PrintService ps = new PrintService();
            ////////StreamReader sr = new StreamReader(pdfpath);
            ////////FileStream fs = new FileStream(pdfpath, FileMode.Open,FileAccess.Read);

            ////////ps.startPrint(fs,"pdf");
            ////////fs.Close();
            
            webBrowser1.Navigate(pdfpath);
        }
    }

    public class PrintService
    {
        public PrintService()
        {
            // 
            // todo: 在此处添加构造函数逻辑 
            // 
            this.doctoprint.PrintPage += new PrintPageEventHandler(doctoprint_printpage);
        }//将事件处理函数添加到printdocument的printpage中 

        // declare the printdocument object. 
        private PrintDocument doctoprint = new PrintDocument();//创建一个printdocument的实例 

        private Stream streamtoprint;
        string streamtype;

        // this method will set properties on the printdialog object and 
        // then display the dialog. 
        public void startPrint(Stream InStream, string InType)
        {

            this.streamtoprint = InStream;
            this.streamtype = InType;
            // allow the user to choose the page range he or she would 
            // like to print. 
            PrintDialog printdialog1 = new PrintDialog();//创建一个printdialog的实例。 
            printdialog1.AllowSomePages = true;

            // show the help button. 
            printdialog1.ShowHelp = true;

            // set the document property to the printdocument for 
            // which the printpage event has been handled. to display the 
            // dialog, either this property or the printersettings property 
            // must be set 
            printdialog1.Document = doctoprint;//把printdialog的document属性设为上面配置好的printdocument的实例 

            DialogResult result = printdialog1.ShowDialog();//调用printdialog的showdialog函数显示打印对话框 

            // if the result is ok then print the document. 
            if (result == DialogResult.OK)
            {
                doctoprint.Print();//开始打印 
            }

        }

        // the printdialog will print the document 
        // by handling the document’s printpage event. 
        private void doctoprint_printpage(object sender, PrintPageEventArgs e)//设置打印机开始打印的事件处理函数 
        {

            // insert code to render the page here. 
            // this code will be called when the control is drawn. 

            // the following code will render a simple 
            // message on the printed document 
            switch (this.streamtype)
            {
                case "txt":
                    string text = null;
                    System.Drawing.Font printfont = new System.Drawing.Font(new FontFamily("arial"),1, FontStyle.Regular);

                    // draw the content. 
                    StreamReader streamreader = new StreamReader(this.streamtoprint);
                    text = streamreader.ReadToEnd();
                    e.Graphics.DrawString(text, printfont, Brushes.Black, e.MarginBounds.X, e.MarginBounds.Y);
                    break;
                case "pdf":
                case "image":
                    Image image = Image.FromStream(this.streamtoprint);
                    int x = e.MarginBounds.X;
                    int y = e.MarginBounds.Y;
                    int width = image.Width;
                    int height = image.Height;
                    if ((width / e.MarginBounds.Width) > (height / e.MarginBounds.Height))
                    {
                        width = e.MarginBounds.Width;
                        height = image.Height * e.MarginBounds.Width / image.Width;
                    }
                    else
                    {
                        height = e.MarginBounds.Height;
                        width = image.Width * e.MarginBounds.Height / image.Height;
                    }
                    Rectangle destrect = new Rectangle(x, y, width, height);
                    e.Graphics.DrawImage(image, destrect, 0, 0, image.Width, image.Height,GraphicsUnit.Pixel);
                    break;
                default:
                    break;
            }

        }

    }
}
