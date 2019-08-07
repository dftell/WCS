using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace WolfInv.Com.WCS_Process
{
    public class ImportProcessClass
    {
        public ImportType FileType;
        public XmlDocument XmlFileModel;
        public ImportProcessClass(XmlDocument xmlmodel,ImportType type)
        {
            XmlFileModel = xmlmodel;
            FileType = type;
        }
        public XmlDocument getImportData(string ImpFilePath)
        {
            XmlDocument xmldoc = FileImportClass.CreateInstance(XmlFileModel, FileType)?.getDataFormImportFile(ImpFilePath);
            return xmldoc;

        }

        
    }

    public abstract class FileImportClass
    {
        public abstract XmlDocument getDataFormImportFile(string Path);

        public static FileImportClass CreateInstance(XmlDocument _XmlFileModel,ImportType type)
        {
            FileImportClass ret = null;
            switch(type)
            {
                case ImportType.XLS:
                default:
                    {
                        ret = new XlsImportClass(_XmlFileModel);
                        break;
                    }
            }
            return ret;
        }
    }

    public class XlsImportClass: FileImportClass
    {
        public XmlDocument XmlFileModel;
        public XlsImportClass(XmlDocument _XmlFileModel)
        {
            XmlFileModel = _XmlFileModel;
        }

        public override XmlDocument getDataFormImportFile(string Path)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml("<root/>");
            return xmldoc;
        }
    }


    public enum ImportType
    {
        XLS,
        TEXT
    }
}
