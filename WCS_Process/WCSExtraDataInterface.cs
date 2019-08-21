using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Data;
using System.Reflection;
using System.IO;
using WolfInv.Com.MetaDataCenter;
namespace WolfInv.Com.WCS_Process
{
    public enum ExtraDataType
    {
        Xml,
        Excel,
        DataSet,
        Json
    }
    public interface WCSExtraDataInterface
    {
        string getJsonData(XmlNode config);
        XmlDocument getXmlData(XmlNode config,ref XmlDocument xmlshema);

        DataSet getDataSet(XmlNode config);
    }

    public class WCSExtraDataClass
    {
        WCSExtraDataInterface obj;
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
            obj = Activator.CreateInstance(t) as WCSExtraDataInterface;
    
        }
        public bool getExtraData(ref XmlDocument ret,ref XmlDocument xmlschema)
        {
            if (obj == null)
                return false;
            ret = obj.getXmlData(confignode,ref xmlschema);
            return true;
        }

        public bool getExtraData(ref DataSet ret)
        {
            DataSet ds = obj.getDataSet(confignode);
            ret = ds;
            return true;
        }
    }

    public class WCSExtraDataAdapter
    {
        XmlNode setting;
        List<DataTranMapping> mps = new List<DataTranMapping>();
        Dictionary<string, DataTranMapping> dic;
        public WCSExtraDataAdapter( XmlNode convertNode)
        {
            setting = convertNode;
            mps = new List<DataTranMapping>();
            XmlNodeList mapnodes = convertNode.SelectNodes("Maps/Map");
            if (mapnodes.Count > 0)
            {
                //mps = new DataTranMapping();
                foreach (XmlNode mapnode in mapnodes)
                {
                    DataTranMapping dtm = new DataTranMapping();
                    dtm.LoadXml(mapnode);
                    mps.Add(dtm);
                }
                dic = mps.ToDictionary(a => a.FromDataPoint, a => a);
            }
            dic = mps.ToDictionary(a => a.FromDataPoint, a => a);

        }

        public DataSet getData(string assembly,string classname,XmlNode callguider,  string type,ref string msg)
        {
            DataSet ret = new DataSet();
            msg = null; 
            switch(type.ToLower())
            {
                case "json":
                case "text":
                    {
                        break;
                    }
                case   "dataset":
                    {
                        WCSExtraDataClass wdc = new WCSExtraDataClass(assembly, classname, type, callguider);
                        DataSet ds = null;
                        if (!wdc.getExtraData(ref ds))
                        {
                            msg = "无法获取到数据！";
                            return ret;
                        }
                        ;
                        if (ds == null || ds.Tables.Count == 0)
                        {
                            msg = "数据为空！";
                            return ret;
                        }
                        DataTable dt = ds.Tables[0];
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            string name = dt.Columns[i].ColumnName;
                            if (dic.ContainsKey(name))
                            {
                                dt.Columns[i].ColumnName = dic[name].ToDataPoint;
                            }
                        }
                        ret = new DataSet();
                        ret.Tables.Add(dt.Copy());
                        msg = null;
                        break;
                    }
                case "xml":
               
                default:
                    {
                        WCSExtraDataClass wdc = new WCSExtraDataClass(assembly, classname, type, callguider);
                        XmlDocument node = null;
                        XmlDocument schema = null;
                        if(wdc.getExtraData(ref node,ref schema))
                        {
                            ret = getDataSet(node,schema, ref msg);
                        }
                        
                        break;
                    }
            }
            return ret;
        }

        DataSet getDataSet(DataSet input, ref string msg)
        {
            DataSet ret = input.Clone();
            return ret;
        }



        DataSet getDataSet(XmlDocument xmlnode,XmlDocument xmlschema, ref string msg)
        {
            MemoryStream ms = new MemoryStream();
            MemoryStream msch = new MemoryStream();
            DataSet ret = new DataSet();
            try
            {
                DataTable dt = new DataTable();
                if (1 == 0)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        dt.Columns.Add("col1111" + i.ToString(), typeof(string));
                    }

                    for (int i = 0; i < 5; i++)
                    {
                        DataRow dr = dt.NewRow();
                        dr[0] = "ldfkdjldf";
                        dr[1] = "kldfjldf";
                        dr[2] = "ldjdfl";
                        dr[3] = "jljls";
                        dr[4] = null;

                        dt.Rows.Add(dr);

                    }
                    dt.WriteXmlSchema("c:\\test.txt");
                    dt.WriteXml("c:\\test.xml", XmlWriteMode.WriteSchema);
                }
                ////msch.Flush();
                //////ms.Seek(0, SeekOrigin.Begin);
                ////string txt  = new StreamReader(msch).ReadToEnd();
                //再存入内存流

                xmlnode.Save(ms);
                xmlschema.Save(msch);
                msch.Seek(0, SeekOrigin.Begin);
                ms.Seek(0, SeekOrigin.Begin);
                //dt.TableName = "table1";
                dt.ReadXmlSchema(msch);
                dt.ReadXml(ms);

                for(int i=0;i<dt.Columns.Count;i++)
                {
                    string name = dt.Columns[i].ColumnName;
                    if(dic.ContainsKey(name))
                    {
                        dt.Columns[i].ColumnName = dic[name].ToDataPoint;
                    }
                }
                ret.Tables.Add(dt);
                msg = null;
                return ret;
            }
            catch(Exception e)
            {
                msg = e.Message;
                return ret;
            }
            finally
            {
                ms.Dispose();
                msch.Dispose();
            }
        }
    }
}
