using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WolfInv.Com.WCSExtraDataInterface;
using System.Data;
using System.IO;
using XmlProcess;
using WolfInv.Com.JsLib;
namespace WolfInv.Com.ExcelIOLib
{
    public class ExcelOutDataClass : IWCSExtraDataInterface
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
        public bool getDataSet(XmlNode config,ref DataSet ds,ref string msg)
        {
            ds = null;
            ExcelSheetDefineClass obj = new ExcelSheetDefineClass();
            string strDefinedfile = XmlUtil.GetSubNodeText(config, "@definepath");
            string strDefinedType = "";
            string fPath = XmlUtil.GetSubNodeText(config, "@excelpath");
            string sheetname = XmlUtil.GetSubNodeText(config, "@excelsheet");
            //string strPath = string.Format("{0}\\json\\Imp.Excel.SaleOrder.List.json", AppDomain.CurrentDomain.BaseDirectory);
            
            obj = obj.GetFromJson<ExcelSheetDefineClass>(getText(strDefinedfile, "json", strDefinedType));
            
            if (obj == null)
            {
                msg = "导入选项未正常配置！";
                return false;
            }
            if (sheetname.Trim().Length > 0)
            {
                obj.SheetName = sheetname;
            }
            //ExcelSheetDefineClass useObj = new ExcelSheetDefineClass(obj.QuickTitleList, obj.QuickTitleRefList, 1, 2);
            if (fPath == null)
            {
                msg = ("请先选择需要导入的文件！");
                return false;
            }

            ExcelDefineReader edr = new ExcelDefineReader(obj);
            ReadResult ret = edr.GetResult(fPath);
            if (ret.Succ == false)
            {
                msg = ret.Message;
                return false;
            }
            ds = ret.ReData;
            return true;
        }

       

        

        public bool writeXmlData(XmlNode config, DataSet data,ref XmlDocument xmldoc, ref XmlDocument xmlshema,ref string msg, string writetype = "Add")
        {

            return false;
        }

        public bool getJsonData(XmlNode config, ref string strJson, ref string msg)
        {
            throw new NotImplementedException();
        }

        public bool getXmlData(XmlNode config, ref XmlDocument ret, ref XmlDocument xmlshema, ref string msg)
        {
            throw new NotImplementedException();
        }

        public bool writeJsonData(XmlNode config, DataSet data, ref string strJson, ref string msg, string writetype = "Add")
        {
            throw new NotImplementedException();
        }

        public bool writeDataSet(XmlNode config, DataSet data, ref DataSet ret, ref string msg, string writetype = "Add")
        {
            throw new NotImplementedException();
        }
    }
}
