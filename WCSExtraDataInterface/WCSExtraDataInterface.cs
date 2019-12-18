using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Data;
using System.Reflection;
namespace WolfInv.Com.WCSExtraDataInterface
{
    public enum ExtraDataType
    {
        Xml,
        Excel,
        DataSet,
        Json
    }
    public interface IWCSExtraDataInterface
    {
        bool getJsonData(XmlNode config,ref string strJson,ref string msg, XmlNode condition = null);
        bool getXmlData(XmlNode config, ref XmlDocument ret,ref XmlDocument xmlshema,ref string msg,XmlNode condition = null);

        bool getDataSet(XmlNode config,ref DataSet ds,ref string msg, XmlNode condition = null);

        bool writeJsonData(XmlNode config, DataSet data,  ref string strJson,ref string msg, string writetype="Add");
        bool writeXmlData(XmlNode config,DataSet data, ref XmlDocument ret, ref XmlDocument xmlshema,ref string msg, string writetype = "Add");

        bool writeDataSet(XmlNode config,DataSet data,ref DataSet ret,ref string msg, string writetype = "Add");
    }

    public class WCSExtraDataClass
    {
        IWCSExtraDataInterface obj;
        XmlNode confignode = null;
        public WCSExtraDataClass(string assem,string classname,string datatype,XmlNode config)
        {
            string asspath = string.Format("{0}\\{1}.dll",AppDomain.CurrentDomain.BaseDirectory,assem);
            confignode = config;
            Assembly ass = Assembly.LoadFrom(asspath);
            if(ass == null)
            {
                return;
            }
            
            Type t = ass.GetType(string.Format("{0}.{1}",assem,classname));
            if (t == null)
                return;
            obj = Activator.CreateInstance(t) as IWCSExtraDataInterface;
    
        }
        public bool getExtraData(ref XmlDocument ret,ref XmlDocument xmlschema,ref string msg, XmlNode condition = null)
        {
            if (obj == null)
                return false;
            return obj.getXmlData(confignode,ref ret,ref xmlschema,ref msg, condition);

        }
        
        public bool writeExtraData(DataSet ds, ref XmlDocument ret, ref XmlDocument xmlschema, ref string msg,string updateype)
        {
            return obj.writeXmlData(confignode, ds, ref ret, ref xmlschema, ref msg,updateype);
        }

        public bool getExtraData(ref DataSet ret,ref string msg,XmlNode condition = null)
        {
            DataSet ds = null;
            bool succ =  obj.getDataSet(confignode,ref ds,ref msg, condition);
            ret = ds;
            return succ;
        }

        public bool writeExtraData(DataSet ds,ref DataSet ret,ref string msg,string updateype)
        {
            return obj.writeDataSet(confignode, ds, ref ret, ref msg,updateype);
        }
    }
}
