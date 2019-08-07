using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WolfInv.Com.ExcelIOLib;
using WolfInv.Com.JsLib;
using jdyInterfaceLib;
using System.Threading;

namespace WCS_Plugin_For_GMX
{
    public partial class Form1 : Form
    {
        Dictionary<string, string> AllProduct;
        Dictionary<string, string> AllUnits;
        Dictionary<string, string> AllWareHouses;
        Dictionary<string, string> AllCustomers;
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_save_saleOrder_Click(object sender, EventArgs e)
        {
            backgroundWorker1.DoWork += sync_SaleOrderList ;
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker1.RunWorkerAsync();
        }

        private void btn_sync_productlist_Click(object sender, EventArgs e)
        {
            backgroundWorker1.DoWork += sync_productlist;
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker1.RunWorkerAsync();
        }
        string fPath = null;
        void OpenDialog()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                fPath = ofd.FileName;
            }
        }
        public void sync_SaleOrderList(object worker, DoWorkEventArgs e)
        {
            BackgroundWorker sender = worker as BackgroundWorker;
            ExcelSheetDefineClass obj = new ExcelSheetDefineClass();
            string strPath = string.Format("{0}\\json\\Imp.Excel.SaleOrder.List.json", AppDomain.CurrentDomain.BaseDirectory);
            if (File.Exists(strPath))
            {
                string str = File.ReadAllText(strPath);
                obj = obj.GetFromJson<ExcelSheetDefineClass>(str);
            }
            if (obj == null)
            {
                MessageBox.Show(string.Format("文件{0}错误！", strPath));
                return;
            }
            ExcelSheetDefineClass useObj = new ExcelSheetDefineClass(obj.QuickTitleList, obj.QuickTitleRefList, 1, 2);
            Thread InvokeThread = new Thread(new ThreadStart(OpenDialog));
            InvokeThread.SetApartmentState(ApartmentState.STA);
            InvokeThread.Start();
            InvokeThread.Join();
            if (fPath == null)
            {
                MessageBox.Show("请先选择需要导入的文件！");
                return;
            }

            ExcelDefineReader edr = new ExcelDefineReader(useObj);
            ReadResult ret = edr.GetResult(fPath);

            if (ret.Succ == false)
            {
                sender.ReportProgress(0);
                sender.CancelAsync();
                MessageBox.Show(ret.Message);
                return;
            }
            sender.ReportProgress(50);
            if (MessageBox.Show(string.Format("已成功读取Excel表中{0}个表数据，确认将这些数据导入到金蝶系统中？", ret.ReData.Tables.Count), "确认导入数据", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                sender.ReportProgress(0);
                sender.CancelAsync();
                return;
            }
            List<JDYSCM_SaleOrder_Add_Class.SaleOrderProduct> list = new DisplayAsTableClass().FillByTable<JDYSCM_SaleOrder_Add_Class.SaleOrderProduct>(ret.ReData.Tables[0]);
            int cnt = 0;
            JDYSCM_SaleOrder_Add_Class pdc = new JDYSCM_SaleOrder_Add_Class();
            pdc.InitClass(jdy_GlbObject.mlist[pdc.GetType().Name]);
            pdc.items = new List<JDYSCM_SaleOrder_Add_Class.JDYSCM_SaleOrder_List_Item_Class>();
            JDYSCM_SaleOrder_Add_Class.JDYSCM_SaleOrder_List_Item_Class Order = new JDYSCM_SaleOrder_Add_Class.JDYSCM_SaleOrder_List_Item_Class();
            Order.date = DateTime.Now.ToString("yyyy-MM-dd");
            Order.number = "";
            Order.employeeNumber = "";
            Order.customerNumber = jdy_GlbObject.AllCustomers.Where(a => a.Key.IndexOf("大疆无人机") >= 0).First().Value;
            Order.entries = new List<JDYSCM_SaleOrder_Add_Class.SaleOrderProduct>();
            Dictionary<string, JDYSCM_SaleOrder_Add_Class.SaleOrderProduct> AllProductQty = new Dictionary<string, JDYSCM_SaleOrder_Add_Class.SaleOrderProduct>();
            list.ForEach(a =>
            {
                
                JDYSCM_SaleOrder_Add_Class.SaleOrderProduct sale_prd = new JDYSCM_SaleOrder_Add_Class.SaleOrderProduct();
                if(AllProductQty.ContainsKey(a.productNumber))
                {
                    sale_prd = AllProductQty[a.productNumber];
                }
                sale_prd.productNumber = a.productNumber;
                sale_prd.location = jdy_GlbObject.AllWareHouses.Values.First();
                if(sale_prd.price == null)
                    sale_prd.price = a.price;
                sale_prd.qty += a.qty;
                if (jdy_GlbObject.UnitList.ContainsKey(a.unit))
                {
                    sale_prd.unit = jdy_GlbObject.UnitList[a.unit];
                }
                else
                {
                    sale_prd.unit = "";
                }
                if(!AllProductQty.ContainsKey(a.productNumber))
                {
                    AllProductQty.Add(a.productNumber, sale_prd);
                }

            });
            List<string> NoInList = new List<string>();
            foreach(string pid in AllProductQty.Keys)
            {
                JDYSCM_SaleOrder_Add_Class.SaleOrderProduct sale_prd = AllProductQty[pid];
                if (!jdy_GlbObject.ProductList.ContainsKey(pid))
                {
                    NoInList.Add(pid);
                    continue;
                }
                Order.entries.Add(sale_prd);
                
            }
            if(NoInList.Count>0)
            {
                
                if(MessageBox.Show(string.Format("以下物料[{0}]未存在在系统的物料清单中！建议先同步最新的物料清单！如果您一定需要导入，请选是，我们同步时将跳过这些物料！否则，终止同步", string.Join(",",NoInList.ToArray())),"忽略无编号物料",MessageBoxButtons.YesNo)== DialogResult.No)
                {
                    sender.ReportProgress(0);
                    sender.CancelAsync();
                    return;
                }
            }

            List<JDYSCM_SaleOrder_Add_Class.SaleOrderProduct> NoPriceList = Order.entries.Where(a =>
            {
                if(a.price == null || a.price.Trim().Length == 0)
                {
                    return true;
                }
                return false;
            }).ToList();
            List < JDYSCM_SaleOrder_Add_Class.SaleOrderProduct > HasPriceList = Order.entries.Where(a =>
            {
                if (a.price == null || a.price.Trim().Length == 0)
                {
                    return false;
                }
                return true;
            }).ToList();
            if (NoPriceList.Count()>0)
            {
                if (MessageBox.Show(string.Format("以下物料[{0}]价格未提供！建议先提供价格！如果您一定需要导入，请选是，我们同步时将跳过这些物料！否则，终止同步！", string.Join(",", NoPriceList.Select(a=>a.productNumber).ToArray())), "忽略无价格物料", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    sender.ReportProgress(0);
                    sender.CancelAsync();
                    return;
                }
                else
                {
                    Order.entries.Clear();
                    HasPriceList.ForEach(a=>Order.entries.Add(a));
                }
            }
            pdc.items.Clear();
            pdc.items.Add(Order);
            pdc.InitRequestJson();
            string strPostJson = "";
            int i = 0;
            pdc.items.ForEach(a =>
            {
                strPostJson += string.Format("{0}{1}",i==0?"":",",a.ToJson());
                i++;
            });
            strPostJson = "{\"items\":[{0}]}".Replace("{0}", strPostJson);
            pdc.Req_PostData = strPostJson.Replace("null","\"\"");
            string strRet = pdc.PostRequest();
            JDYSCM_SaleOrder_Add_Class res = new JDYSCM_SaleOrder_Add_Class().GetFromJson<JDYSCM_SaleOrder_Add_Class>(strRet);
            sender.ReportProgress(100);
            sender.CancelAsync();
            if (res.code == "0")
            {
                MessageBox.Show(string.Format("成功保存{0}条记录！", pdc.items.Count));
            }
            else
            {
                MessageBox.Show(string.Format("保存记录失败！{0}", pdc.msg));
            }
            
            
        }
        public void sync_productlist(object worker, DoWorkEventArgs e)
        {
            BackgroundWorker sender = worker as BackgroundWorker;
            ExcelSheetDefineClass obj = new ExcelSheetDefineClass();
            string strPath = string.Format("{0}\\json\\Imp.Excel.Product.List.json", AppDomain.CurrentDomain.BaseDirectory);
            if (File.Exists(strPath))
            {
                string str = File.ReadAllText(strPath);
                obj = obj.GetFromJson<ExcelSheetDefineClass>(str);
            }
            if (obj == null)
            {
                sender.ReportProgress(0);
                sender.CancelAsync();
                MessageBox.Show(string.Format("文件{0}错误！", strPath));

                return;
            }
            ExcelSheetDefineClass useObj = new ExcelSheetDefineClass(obj.QuickTitleList, obj.QuickTitleRefList, 1, 2);

            Thread InvokeThread = new Thread(new ThreadStart(OpenDialog));
            InvokeThread.SetApartmentState(ApartmentState.STA);
            InvokeThread.Start();
            InvokeThread.Join();
            
            if (fPath == null)
            {
                sender.ReportProgress(0);
                sender.CancelAsync();
                MessageBox.Show("请先选择需要导入的文件！");
                return;
            }
            Application.DoEvents();
            //this.Cursor = Cursors.WaitCursor;

            ExcelDefineReader edr = new ExcelDefineReader(useObj);
            ReadResult ret = edr.GetResult(fPath);
            //this.Cursor = Cursors.Default;
            if (ret.Succ == false)
            {
                MessageBox.Show(ret.Message);
                sender.ReportProgress(0);
                sender.CancelAsync();
                return;
            }
            if (MessageBox.Show(string.Format("已成功读取Excel表中{0}个表数据，确认将这些数据导入到金蝶系统中？", ret.ReData.Tables.Count), "确认导入数据", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                sender.CancelAsync();
                return;
            }
            //this.Cursor = Cursors.WaitCursor;
            List<JDYSCM_Product_Add_Class.JDYSCM_Bussiness_Item_Product_Add_Class> list = new DisplayAsTableClass().FillByTable<JDYSCM_Product_Add_Class.JDYSCM_Bussiness_Item_Product_Add_Class>(ret.ReData.Tables[0]);
            int cnt = 0;
            sender.ReportProgress(50);
            int i = 0;
            list.ForEach(a =>
            {
                if (!jdy_GlbObject.ProductList.ContainsKey(a.productNumber))
                {


                    JDYSCM_Product_Add_Class pdc = new JDYSCM_Product_Add_Class();
                    pdc.items = new List<JDYSCM_Product_Add_Class.JDYSCM_Bussiness_Item_Product_Add_Class>();
                    pdc.items.Add(a);
                    pdc.InitClass(jdy_GlbObject.mlist[pdc.GetType().Name]);
                    pdc.InitRequestJson();

                    pdc.access_token = jdy_GlbObject.Access_token;
                    pdc.dbId = jdy_GlbObject.dbId;
                    if (a.unit == null && a.unit.Trim().Length == 0)
                    {
                        MessageBox.Show(string.Format("记录[{0}]的单位为空，仍将继续导入！", a.ToJson()));
                        a.unit = "";
                    }
                    if (!jdy_GlbObject.UnitList.ContainsKey(a.unit))
                    {
                        MessageBox.Show(string.Format("记录[{0}]的单位无法识别，仍将继续导入，请导入后手工增加该单位然后修改物料单位！", a.ToJson()));
                        a.unit = "";
                    }
                    else
                    {
                        a.unit = jdy_GlbObject.UnitList[a.unit];
                    }
                    a.primaryStock = jdy_GlbObject.AllWareHouses.Keys.First();
                    pdc.Req_PostData = "{\"items\":[{0}]}".Replace("{0}", a.ToJson()).Replace("null", "\"\"");
                    string res = pdc.PostRequest();
                    JDYSCM_Product_Add_Class tmp = new JDYSCM_Product_Add_Class().GetFromJson<JDYSCM_Product_Add_Class>(res);

                    
                    if (tmp.code == "0")
                    {
                        cnt++;
                    }
                }
                i++;
                sender.ReportProgress(50 +(50*i/list.Count));
            });
            //this.Cursor = Cursors.Default;
            sender.ReportProgress(100);
            sender.CancelAsync();
            MessageBox.Show(string.Format("合计{1}条记录，成功保存{0}条记录！", cnt,list.Count));
            sender.ReportProgress(0);
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.AllCustomers = jdy_GlbObject.AllCustomers;
            this.AllUnits = jdy_GlbObject.UnitList;
            this.AllWareHouses = jdy_GlbObject.AllWareHouses;
            this.AllProduct = jdy_GlbObject.ProductList;
            //this.toolStripProgressBar1.
            
            this.toolStripProgressBar1.Tag = this.AllProduct;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            //还原worker对象
            BackgroundWorker worker = sender as BackgroundWorker;
            //开始工作
            //DoSomething(worker, e);
            //sync_productlist(worker, e);
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.toolStripProgressBar1.Value = e.ProgressPercentage;
            //resultLabel.Text = (e.ProgressPercentage.ToString() + "%");
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                this.toolStripStatusLabel1.Text = "Canceled!";
            }
            else if (e.Error != null)
            {
                toolStripStatusLabel1.Text = "Error: " + e.Error.Message;
            }
            else
            {
                toolStripStatusLabel1.Text = "Done!";
            }
        }

        

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.backgroundWorker1.CancelAsync();
            this.backgroundWorker1 = null;
        }
    }
}
