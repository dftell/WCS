using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Net;
namespace WolfInv.Com.HtmlExp
{
    public class WebReqProcess
    {
        public static string GetResult(string furl,bool Post,string postxml)
        {
            //string furl = string.Format("{0}?op={1}&system={2}&uid={3}", WebPath, funcname, connobj.id, "");
            HttpWebRequest WebReq = HttpWebRequest.Create(furl) as HttpWebRequest;
       
            WebReq.Method = Post?"POST":"GET";
            WebReq.KeepAlive = false;
            //wq.Headers 
            string data = string.Format("{0}", postxml);
            //            wq.ContentType = "application/x-www-form-urlencoded";
            WebReq.ContentType = "text/xml";
            //WebReq.ContentLength = data.Length;
            try
            {


                
     
                if (Post)
                {
                    Stream strReq = WebReq.GetRequestStream();
                    StreamWriter ReqWrite = new StreamWriter(strReq);
                    ReqWrite.Write(data);
                    ReqWrite.Close();
                    strReq.Close();
                    WebReq.GetRequestStream();
                    strReq = null;
                }
                WebResponse WebResp = WebReq.GetResponse();
                Stream strResp = WebResp.GetResponseStream();
                Encoding encode = System.Text.Encoding.UTF8;
                StreamReader RespReader = new StreamReader(strResp, encode);
                string rethtml = RespReader.ReadToEnd();
                RespReader.Close();
                strResp.Close();
                
                strResp = null;
                WebResp = null;
                WebReq = null;

                return rethtml;
            }
            catch (Exception ce)
            {
                return string.Format("发送请求失败!原因:{0}.", ce.Message);
            }
            return "无法接收到数据！";
        }

  
        public static string GetResult(string furl)
        {

            //XMLHTTP xmlhttp = new XMLHTTP();
            WebRequest xmlhttp = HttpWebRequest.Create(furl);
            try
            {
               WebResponse wr = xmlhttp.GetResponse();
                wr.GetResponseStream();
                string ret = new StreamReader(wr.GetResponseStream(), Encoding.UTF8).ReadToEnd();
                wr.Close();
                return ret;
            }
            catch(Exception ce)
            {
                return null;
            }

        }

        
    }

    

}
