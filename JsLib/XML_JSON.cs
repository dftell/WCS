using System;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WolfInv.Com.JsLib
{
    public class XML_JSON
    {
        /// <summary>
        /// 返回指定节点下信息的JSON格式字符串
        /// </summary>
        /// <param name="str">xml字符串</param>
        /// <param name="nodename">节点名称，应从根节点开始</param>
        /// <returns></returns>
        public static string XML2Json(string str, string nodename)
        {

            try
            {
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(str);
               
                return XML2Json(str,nodename);
            }
            catch
            {
                return null;
            }
        }

        public static string XML2Json(XmlDocument xmldoc, string nodename,bool OnlyInnerText=false)
        {
            string result = null;
            try
            {
                XmlNode node = xmldoc.SelectSingleNode(nodename);
                result = JsonConvert.SerializeXmlNode(node);
                if(OnlyInnerText)//去掉最外面的花括号
                {
                    string nopi = result.Substring(1, result.Length - 2);//2边        花括号
                    return nopi.Substring(node.Name.Length + 3);//+2个引号+1个冒号
                }
                return result;
            }
            catch
            {
                return null;
            }
        }

        public static string Json2XML(string str)
        {
            string result = null;
            XmlDocument xml =JsonConvert.DeserializeXmlNode(str);
            result = xml.OuterXml;
            return result;
        }

        public static JObject ToJsonObj(string str)
        {
            JObject ret = null;
            try
            {
                ret = (JObject)JsonConvert.DeserializeObject(str);
                return ret;
            }
            catch(Exception ce)
            {

            }
            return ret;
        }
    }
}
