using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Web;
using System.Data;
using System.Xml;
using System.IO;
using WolfInv.Com.MetaDataCenter;
namespace WolfInv.Com.WCS_Process
{
    public class AccessWebService
    {
        protected string WebPath;
        protected ConnectObject connobj;
        public AccessWebService(ConnectObject conn)
        {
            if (WebPath == null)
            {
                WebPath = conn.ConnectString;
            }
            connobj = conn;
        }

        public DataSet GetDataSet(string funcname,XmlNode xml,out string msg)
        {
            string xmlresult = GetResult(funcname,xml.OuterXml);
            DataSet ds = new DataSet();
            XmlDocument xmldoc = new XmlDocument();
            try
            {
                xmldoc.LoadXml(xmlresult);
            }
            catch
            {
                msg = xmlresult;
                return null;
            }
            TextReader tr = new StringReader(xmlresult);
            //ds.ReadXml(tr);
            ds.ReadXml(tr, XmlReadMode.ReadSchema);
            tr.ReadToEnd();
            msg = null;
            tr.Close();
            return ds;
        }

        public string GetString(string funcname, XmlNode xml)
        {
            return GetResult(funcname, xml.OuterXml);
        }

        public XmlNode GetXml(string funcname, XmlNode xml, out string msg)
        {
            string xmlresult = GetResult(funcname, xml.OuterXml);
            XmlDocument xmldoc = new XmlDocument();
            try
            {
                xmldoc.LoadXml(xmlresult);
            }
            catch
            {
                msg = xmlresult;
                return null;
            }
            msg = null;
            return xmldoc;
        }

        public string GetResult(string funcname, string xml)
        {
            string furl = string.Format("{0}?op={1}&system={2}&uid={3}",WebPath ,funcname,connobj.id,"" );
            HttpWebRequest WebReq = HttpWebRequest.Create(furl) as HttpWebRequest;
            WebReq.Method = "POST";
            WebReq.KeepAlive = false;
            //wq.Headers 
            string data = string.Format("{0}", xml);
            //            wq.ContentType = "application/x-www-form-urlencoded";
            WebReq.ContentType = "text/xml";
            //WebReq.ContentLength = data.Length;
            try
            {


                Stream strReq = WebReq.GetRequestStream();
                StreamWriter ReqWrite = new StreamWriter(strReq);
                
                ReqWrite.Write(data);
                ReqWrite.Flush();
                //strReq.Flush();
                ReqWrite.Close();
                strReq.Close();

           
                
                WebResponse WebResp = WebReq.GetResponse();
                
                Stream strResp = WebResp.GetResponseStream();
                Encoding encode = System.Text.Encoding.UTF8;
                StreamReader RespReader = new StreamReader(strResp, encode);
                string rethtml = RespReader.ReadToEnd();
                RespReader.Close();
                strResp.Close();
                
                strReq = null;
                strResp = null;
                WebResp = null;
                WebReq = null;
                
                return rethtml;
            }
            catch (Exception ce)
            {
                return string.Format("发送请求失败!原因:{0}.",ce.Message);
            }
            return "无法接收到数据！";
        }
    }
}
