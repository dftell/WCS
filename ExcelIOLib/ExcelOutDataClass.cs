using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WolfInv.Com.WCS_Process;
using System.Data;
using System.IO;
using XmlProcess;
using WolfInv.Com.JsLib;
namespace WolfInv.Com.ExcelIOLib
{
    public class ExcelOutDataClass : WCSExtraDataInterface
    {
        public string getFilePath(string name, string folder = "json", string type = ".json")
        {
            string strPath = string.Format("{0}{1}\\{2}{3}", AppDomain.CurrentDomain.BaseDirectory, folder,name, type);
            if (File.Exists(strPath))
            {
                return strPath;
            }
            return null;
        }
        public string getText(string filepath, string folder, string type)
        {
            string file = getFilePath(filepath, folder, type);
            if (!File.Exists(file))
            {
                return null;
            }
            try
            {
                return File.ReadAllText(file);
            }
            catch
            {
                return null;
            }

        }
        public DataSet getDataSet(XmlNode config)
        {
            DataSet ds = new DataSet();
            ExcelSheetDefineClass obj = new ExcelSheetDefineClass();
            string strDefinedfile = XmlUtil.GetSubNodeText(config, "@definepath");
            string strDefinedType = "";
            string fPath = XmlUtil.GetSubNodeText(config, "@excelpath");
            string sheetname = XmlUtil.GetSubNodeText(config, "@excelsheet");
            //string strPath = string.Format("{0}\\json\\Imp.Excel.SaleOrder.List.json", AppDomain.CurrentDomain.BaseDirectory);
            
            obj = obj.GetFromJson<ExcelSheetDefineClass>(getText(strDefinedfile, "json", strDefinedType));
            
            if (obj == null)
            {
                return ds;
            }
            if (sheetname.Trim().Length > 0)
            {
                obj.SheetName = sheetname;
            }
            //ExcelSheetDefineClass useObj = new ExcelSheetDefineClass(obj.QuickTitleList, obj.QuickTitleRefList, 1, 2);
            if (fPath == null)
            {
                //MessageBox.Show("请先选择需要导入的文件！");
                return ds;
            }

            ExcelDefineReader edr = new ExcelDefineReader(obj);
            ReadResult ret = edr.GetResult(fPath);
            if (ret.Succ == false)
            {
                
                return ds;
            }
            ds = ret.ReData;
            return ds;
        }

        public string getJsonData(XmlNode config)
        {
            throw new NotImplementedException();
        }

        public XmlDocument getXmlData(XmlNode config, ref XmlDocument xmlshema)
        {
            throw new NotImplementedException();
        }
    }
}
