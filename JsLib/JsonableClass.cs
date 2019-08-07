using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

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
}
