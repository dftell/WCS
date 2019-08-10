using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace WolfInv.Com.JsLib
{
    public class JsonableClass<T>
    {
        public T GetFromJson<T>(string json)
        {
            if (json == null)
                return default(T);
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Deserialize<T>(json);
        }

        public string ToJson()
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(this);
        }
    }

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
            string result = null;
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(str);
            XmlNode node = xmldoc.SelectSingleNode(nodename);
            result = JsonConvert.SerializeXmlNode(node);
            return result;
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
