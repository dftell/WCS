﻿using System;
using System.Collections.Generic;
using System.Xml;
using System.Data;
using System.IO;
using WolfInv.Com.MetaDataCenter;
using System.Linq;
using WolfInv.Com.WCSExtraDataInterface;

namespace WolfInv.Com.WCS_Process
{
    
    public class WCSExtraDataAdapter
    {
        bool WData ;
        XmlNode setting;
        List<DataTranMapping> mps = new List<DataTranMapping>();
        Dictionary<string, DataTranMapping> dic;
        public WCSExtraDataAdapter( XmlNode convertNode,bool WriteData=false)
        {
            WData = WriteData;
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
                //dic = mps.ToDictionary(a => a.FromDataPoint.Name, a => a);
            }
            //dic = 
            ////dic = new Dictionary<string, DataTranMapping>();
            ////mps.ForEach(a=> {
            ////if (!dic.ContainsKey(a.FromDataPoint.Name))
            ////        dic.Add(a.FromDataPoint.Name, a);
                
            ////});

        }

        DataSet ConvertDataSet(DataSet ds)
        {
            mps.ForEach(a => {
                string from = a.FromDataPoint.Name;
                string to = a.ToDataPoint;
                for(int i=0;i<ds.Tables.Count;i++)
                {
                    if(ds.Tables[i].Columns.Contains(from))
                    {
                        ds.Tables[i].Columns[from].ColumnName = to;
                    }
                    else
                    {
                        if (ds.Tables[i].Columns.Contains(to))
                        {
                            ds.Tables[i].Columns[to].ColumnName = from;
                        }
                    }
                }
            });
            return ds;
        }

        public bool getData(string assembly,string classname,XmlNode callguider,  string type,ref DataSet ret,ref string msg)
        {
            ret = null;
            msg = null;
            try
            {
                switch (type.ToLower())
                {
                    case "json":
                    case "text":
                        {
                            break;
                        }
                    case "dataset":
                        {
                            WCSExtraDataClass wdc = new WCSExtraDataClass(assembly, classname, type, callguider);
                            DataSet ds = null;
                            if (!wdc.getExtraData(ref ds, ref msg))
                            {
                                msg = "无法获取到数据！";
                                return false;
                            }
                            ;
                            if (ds == null || ds.Tables.Count == 0)
                            {
                                msg = "数据为空！";
                                return false;
                            }
                            ret = ds;
                            msg = null;
                            break;
                        }
                    case "xml":

                    default:
                        {
                            WCSExtraDataClass wdc = new WCSExtraDataClass(assembly, classname, type, callguider);
                            XmlDocument node = null;
                            XmlDocument schema = null;
                            if (!wdc.getExtraData(ref node, ref schema,ref msg))
                            {
                                return false;
                            }
                            ret = getDataSet(node, schema, ref msg);
                            break;
                        }
                }
            }
            catch(Exception e)
            {
                msg = e.Message;
                return false;
            }
            ret = ConvertDataSet(ret);
            return true;
        }

        public bool writeData(string assembly, string classname, XmlNode callguider, string type,string updateype, DataSet inputds,ref DataSet ret, ref string msg)
        {
            ret = null;
            msg = null;
            try
            {
                inputds = ConvertDataSet(inputds);
                switch (type.ToLower())
                {
                    case "json":
                    case "text":
                        {
                            msg = "json方法没有实现！";
                            return false;
                            break;
                        }
                    case "dataset":
                        {
                            WCSExtraDataClass wdc = new WCSExtraDataClass(assembly, classname, type, callguider);
                            DataSet ds = null;
                            if (!wdc.writeExtraData(inputds,ref ds, ref msg, updateype))
                            {
                                msg = "无法获取到数据！";
                                return false;
                            }
                            ;
                            if (ds == null || ds.Tables.Count == 0)
                            {
                                msg = "数据为空！";
                                return false;
                            }
                            ret = ds;
                            msg = null;
                            break;
                        }
                    case "xml":

                    default:
                        {
                            WCSExtraDataClass wdc = new WCSExtraDataClass(assembly, classname, type, callguider);
                            XmlDocument node = null;
                            XmlDocument schema = null;
                            if (!wdc.writeExtraData(inputds,ref node, ref schema, ref msg,updateype))
                            {
                                return false;
                            }
                            ret = getDataSet(node, schema, ref msg);
                            break;
                        }
                }
            }
            catch (Exception e)
            {
                msg = e.Message;
                return false;
            }
            ret = ConvertDataSet(ret);
            return true;
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

                ////for(int i=0;i<dt.Columns.Count;i++)
                ////{
                ////    string name = dt.Columns[i].ColumnName;
                ////    if(dic.ContainsKey(name))
                ////    {
                ////        dt.Columns[i].ColumnName = dic[name].ToDataPoint;
                ////    }
                ////}
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
