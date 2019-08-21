using System;
using System.IO;
using System.Net;
using System.Text;

namespace WolfInv.Com.AccessWebLib
{
    public class AccessWebServerClass
    {
        public static string GetData(string url)
        {
            return GetData(url, Encoding.UTF8);
        }
        public static string GetData(string url, Encoding Encode)
        {
            string ret = "";
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            req.Method = "Get";
            try
            {
                using (WebResponse wr = req.GetResponse())
                {
                    wr.GetResponseStream();
                    ret = new StreamReader(wr.GetResponseStream(), Encode).ReadToEnd();
                    wr.Close();
                }
            }
            catch (Exception ce)
            {
                return null;

                //throw ce;
            }
            return ret;
        }

        private static readonly string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
        
        public static string PostData(string url, string Data, Encoding Encode,RequestContentType reqtype=RequestContentType.Json)
        {
            string ret = "";
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            req.ProtocolVersion = HttpVersion.Version10;
            req.Method = "POST";
            //req.Timeout = 5 * 60 * 1000;
            req.ContentType =  requestTypeString(reqtype);// "application/json";// "application/x-www-form-urlencoded";
            req.UserAgent = DefaultUserAgent;
            try
            {
                byte[] byteArray = Encode.GetBytes(Data);
                using (Stream newStream = req.GetRequestStream())//创建一个Stream,赋值是写入HttpWebRequest对象提供的一个stream里面
                {
                    newStream.Write(byteArray, 0, byteArray.Length);
                    newStream.Flush();
                }


                using (WebResponse wr = req.GetResponse())
                {
                    Stream rs = wr.GetResponseStream();
                    ret = new StreamReader(rs, Encode).ReadToEnd();
                    wr.Close();
                }
                //newStream.Close();
            }
            catch (Exception ce)
            {
                return ce.Message;

                //throw ce;
            }
            return ret;
        }

        static string requestTypeString( RequestContentType type)
        {
            string ret = "";
            switch(type)
            {
                case RequestContentType.Json:
                    {
                        ret = "application/json";
                        break;
                    }
                case RequestContentType.Xml:
                    {
                        ret = "application/xml";
                        break;
                    }
                case RequestContentType.Html:
                    {
                        ret = "text/html";
                        break;
                    }
                case RequestContentType.Text:
                    {
                        ret = "text/plain";
                        break;
                    }
                case RequestContentType.Gif:
                case RequestContentType.Img:
                case RequestContentType.Png:
                default:
                    {
                        ret = "application/octet-stream";
                        break;
                    }
            }
            return ret;
                
        }
    }

    public enum RequestContentType
    {
        Html,
        Text,
        Json,
        Xml,
        Gif,
        Img,
        Png
    }

}
