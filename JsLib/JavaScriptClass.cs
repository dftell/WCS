using Microsoft.JScript;
using Microsoft.JScript.Vsa;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Web.Script.Serialization;
using System.IO;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Xml;
using Convert = System.Convert;

namespace WolfInv.Com.JsLib
{
    public class JavaScriptClass
    {
        public static object getEval(string str)
        {
            VsaEngine ve = VsaEngine.CreateEngine();
            return Eval.JScriptEvaluate(string.Format("({0})", str), ve);
        }
    }

    public class DisplayAsTableClass : DetailStringClass
    {
        Dictionary<string, List<MemberInfo>> TableStructure = new Dictionary<string, List<MemberInfo>>();

        public static DataTable ToTable<T>(List<T> list, bool UseDisplayNameAsColumnName, bool OnlyDisplayDefName)
        {
            Dictionary<string, MemberInfo> dic = null;
            return ToTable<T>(list, UseDisplayNameAsColumnName, OnlyDisplayDefName, ref dic);
        }

        public static DataTable ToTable<T>(List<T> list, bool OnlyDisplayDefName)
        {
            Dictionary<string, MemberInfo> dic = null;
            return ToTable<T>(list, false, OnlyDisplayDefName, ref dic);
        }

        public static DataTable ToTable<T>(List<T> list)
        {
            return ToTable<T>(list, false);
        }

        public static DataTable ToTable<T>(List<T> list, bool UseDisplayNameAsColumnName, bool OnlyDisplayDefName, ref Dictionary<string, MemberInfo> DisIs)
        {
            DataTable ret = new DataTable();
            int lasti = 0;
            try
            {
                MemberInfo[] pis = typeof(T).GetMembers();
                //List<string> Titles;
                if (DisIs == null)
                {
                    DisIs = new Dictionary<string, MemberInfo>();
                    for (int i = 0; i < pis.Length; i++)
                    {
                        if (pis[i].MemberType != MemberTypes.Property && pis[i].MemberType != MemberTypes.Field)
                        {
                            continue;
                        }
                        Type memtype = (pis[i].MemberType == MemberTypes.Property) ? (pis[i] as PropertyInfo).PropertyType : (pis[i] as FieldInfo).FieldType;
                        ////if (pis[i].GetType().IsClass)//如果是对象，不载入
                        ////    continue;
                        //if (pis[i] is PropertyInfo)
                        //{
                        if ((memtype.IsClass && memtype != typeof(string)))//类对象跳过
                        {
                            continue;
                        }
                        //}
                        DisplayNameAttribute DNA = Attribute.GetCustomAttribute(pis[i], typeof(DisplayNameAttribute)) as DisplayNameAttribute;
                        string strTitle = pis[i].Name;
                        if (DNA != null && UseDisplayNameAsColumnName)
                        {
                            strTitle = DNA.DisplayName;
                        }
                        if (OnlyDisplayDefName && DNA == null)
                        {
                            continue;
                        }
                        if (DisIs.ContainsKey(strTitle))
                        {
                            //ToLog("错误", "转换为表", string.Format("存在相同的关键字.{0}", strTitle));
                            continue;
                        }
                        DisIs.Add(strTitle, pis[i]);
                    }
                }
                foreach (string key in DisIs.Keys)
                {
                    Type memtype = (DisIs[key].MemberType == MemberTypes.Property) ? (DisIs[key] as PropertyInfo).PropertyType : (DisIs[key] as FieldInfo).FieldType;
                    Type genericTypeDefinition = memtype;
                    if (genericTypeDefinition.IsGenericType)
                    {
                        if (genericTypeDefinition.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            genericTypeDefinition = genericTypeDefinition.GetGenericArguments()[0];
                        }
                    }
                    DataColumn dc = new DataColumn(key, genericTypeDefinition);
                    ret.Columns.Add(dc);
                }
                for (int i = 0; i < list.Count; i++)
                {
                    lasti = i;
                    DataRow dr = ret.NewRow();
                    foreach (string key in DisIs.Keys)
                    {
                        T obj = (T)list[i];
                        Type memtype = (DisIs[key].MemberType == MemberTypes.Property) ? (DisIs[key] as PropertyInfo).PropertyType : (DisIs[key] as FieldInfo).FieldType;
                        Type genericTypeDefinition = memtype;
                        if (genericTypeDefinition.IsGenericType)
                        {
                            if (genericTypeDefinition.GetGenericTypeDefinition() == typeof(Nullable<>))
                            {
                                genericTypeDefinition = genericTypeDefinition.GetGenericArguments()[0];
                            }
                        }
                        object val = DisIs[key].MemberType == MemberTypes.Property ? (DisIs[key] as PropertyInfo).GetValue(obj, null) : (DisIs[key] as FieldInfo).GetValue(obj);
                        if (val == null) continue;
                        val = ConvertionExtensions.ConvertTo((IConvertible)val, genericTypeDefinition);
                        if (DisIs[key] is PropertyInfo)
                        {
                            dr[key] = val;
                        }
                        else
                        {
                            dr[key] = val;
                        }
                    }
                    ret.Rows.Add(dr);
                }
            }
            catch (Exception e)
            {
                //ToLog("错误", "将对象列表转换为数据表错误！", string.Format("[{2}]{0}:{1}", e.Message, e.StackTrace, list[lasti].ToString()));
            }
            return ret;
        }

        public List<T> FillByTable<T>(DataTable dt)
        {
            List<MemberInfo> ret = null;
            return FillByTable<T>(dt, ref ret);
        }



        public List<T> FillByTable<T>(DataTable dt, ref List<MemberInfo> TableBuffs)
        {

            List<T> ret = new List<T>();
            Type t = typeof(T);
            if (TableStructure != null && TableStructure.ContainsKey(typeof(T).ToString()))
            {
                TableBuffs = TableStructure[typeof(T).ToString()];
            }
            if (dt == null || dt.Columns.Count == 0)
            {
                //Log("错误", "数据源错误", "为空/或者列为空");
            }
            if (TableBuffs == null)
            {
                TableBuffs = new List<MemberInfo>();
                MemberInfo[] mis = t.GetMembers();
                for (int i = 0; i < mis.Length; i++)
                {
                    if (dt.Columns.Contains(mis[i].Name))
                    {
                        if (!TableBuffs.Contains(mis[i]))
                        {
                            TableBuffs.Add(mis[i]);
                        }
                        else
                        {
                            //Log("错误", "表结构", string.Format("存在相同的列{0}.", mis[i].Name));
                        }
                    }
                }
            }
            //Log("输入表结构列表数量", TableBuffs.Count.ToString());
            if (TableStructure != null && TableBuffs != null)
            {
                if (!TableStructure.ContainsKey(typeof(T).ToString()))
                {
                    TableStructure.Add(typeof(T).ToString(), TableBuffs);
                }
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                T cc = (T)Activator.CreateInstance(typeof(T));
                DataRow dr = dt.Rows[i];
                for (int j = 0; j < TableBuffs.Count; j++)
                {


                    //return Convert.ChangeType(value, t);

                    MemberInfo mi = TableBuffs[j];
                    Type ty = (mi is PropertyInfo) ? (mi as PropertyInfo).PropertyType : (mi as FieldInfo).FieldType;
                    object val = dr[mi.Name];
                    if (val == null) continue;
                    val = ConvertionExtensions.ConvertTo((IConvertible)val, ty);
                    if (mi is PropertyInfo)
                    {
                        (mi as PropertyInfo).SetValue(cc, val, null);
                    }
                    if (mi is FieldInfo)
                    {
                        (mi as FieldInfo).SetValue(cc, val);
                    }
                }
                ret.Add(cc);
            }
            return ret;
        }


    }

    public interface iDetailListParamsable
    {
        string ToDetailString();
    }

    [Serializable]
    public class InnerClass<T> : DetailStringClass
    {
        public List<T> list;
    }

    [Serializable]
    public class DetailStringClass : iDetailListParamsable
    {

        public DetailStringClass()
        {
            SetDefaultValueAttribute();
        }

        public static List<T> getObjectListByXml<T>(string strXml)
        {
            InnerClass<T> innerObj = new InnerClass<T>();

            List<T> ret = new List<T>();
            if (strXml == null || strXml.Trim().Length == 0) return ret;
            //(T)Convert.ChangeType(
            innerObj = (InnerClass<T>)GetObjectByXml(strXml, innerObj.GetType());
            if (ret == null)
                return null;
            ret = innerObj.list;
            ////}
            ////catch(Exception e)
            ////{
            ////    return null;
            ////}
            return ret;
        }

        public static string getXmlByObjectList<T>(List<T> list)
        {
            InnerClass<T> ic = new InnerClass<T>();
            ic.list = list;
            if (ic.list.Count == 0)
                return "";
            return ic.ToXml();
        }

        public string ToDetailString()
        {
            string ret = "";
            Type MyType = this.GetType();
            PropertyInfo[] pps = MyType.GetProperties();
            for (int i = 0; i < pps.Length; i++)
            {
                if (pps[i].PropertyType.IsValueType || pps[i].PropertyType == typeof(string))
                {
                    ret = string.Format("{0}{3}\"{1}\":\"{2}\"", ret, pps[i].Name, pps[i].GetValue(this, null), ret.Length == 0 ? "" : ",");
                }
                else
                {
                    if (pps[i] is iDetailListParamsable)
                    {
                        ret = string.Format("{0}{3}\"{1}\":{2}", ret, pps[i].Name, "{" + (pps[i] as iDetailListParamsable).ToDetailString() + "}", ret.Length == 0 ? "" : ",");
                    }
                }
            }
            FieldInfo[] mems = MyType.GetFields(BindingFlags.Public | BindingFlags.Instance);
            for (int i = 0; i < mems.Length; i++)
            {
                if (mems[i].FieldType.IsValueType || mems[i].FieldType == typeof(string))
                {
                    ret = string.Format("{0}{3}\"{1}\":\"{2}\"", ret, mems[i].Name, mems[i].GetValue(this), ret.Length == 0 ? "" : ",");
                }
                else
                {
                    if (mems[i] is iDetailListParamsable)
                    {
                        ret = string.Format("{0}{3}\"{1}\":{2}", ret, mems[i].Name, "{" + (mems[i] as iDetailListParamsable).ToDetailString() + "}", ret.Length == 0 ? "" : ",");
                    }
                }
            }
            return "{" + ret + "}";
        }

        public override string ToString()
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(this);
            //return ToDetailString();
        }

        public string ToXml()
        {
            return XmlHelper.XmlSerialize(this, Encoding.UTF8);
        }

        public object getValue(string strFieldName)
        {
            Type t = this.GetType();
            FieldInfo fi = t.GetField(strFieldName);
            if (fi == null)
            {
                PropertyInfo pi = t.GetProperty(strFieldName);
                if (pi == null)
                {
                    return null;
                }
                return pi.GetValue(this);
            }
            return fi.GetValue(this);
        }
        public static T GetObjectByXml<T>(string str)
        {
            return XmlHelper.XmlDeserialize<T>(str, Encoding.UTF8);
        }

        public static object GetObjectByXml(string str, Type objtpye)
        {
            return XmlHelper.XmlDeserialize(str, Encoding.UTF8, objtpye);
        }

        protected void SetDefaultValueAttribute()
        {
            PropertyInfo[] pps = this.GetType().GetProperties();
            for (int i = 0; i < pps.Length; i++)
            {
                PropertyInfo pi = pps[i];
                //Attribute[] atts = Attribute.GetCustomAttributes(
                DefaultValueAttribute att = Attribute.GetCustomAttribute(pi as MemberInfo, typeof(DefaultValueAttribute), true) as DefaultValueAttribute;
                if (att != null)
                {
                    if (pi.CanWrite)
                    {
                        object val = ConvertionExtensions.ConvertTo((IConvertible)att.Value, pi.PropertyType);
                        pi.SetValue(this, val, null);
                    }
                }
            }
        }

        public T CopyTo<T>()
        {
            return ConvertionExtensions.CopyTo<T>(this);
        }
    }

    public static class XmlHelper
    {
        private static void XmlSerializeInternal(Stream stream, object o, Encoding encoding)
        {
            if (o == null)
                throw new ArgumentNullException("o");
            if (encoding == null)
                throw new ArgumentNullException("encoding");

            XmlSerializer serializer = new XmlSerializer(o.GetType());

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.NewLineChars = "\r\n";
            settings.Encoding = encoding;
            settings.IndentChars = "    ";

            using (XmlWriter writer = XmlWriter.Create(stream, settings))
            {
                serializer.Serialize(writer, o);
                writer.Close();
            }
        }

        /// <summary>
        /// 将一个对象序列化为XML字符串
        /// </summary>
        /// <param name="o">要序列化的对象</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>序列化产生的XML字符串</returns>
        public static string XmlSerialize(object o, Encoding encoding)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                XmlSerializeInternal(stream, o, encoding);

                stream.Position = 0;
                using (StreamReader reader = new StreamReader(stream, encoding))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// 将一个对象按XML序列化的方式写入到一个文件
        /// </summary>
        /// <param name="o">要序列化的对象</param>
        /// <param name="path">保存文件路径</param>
        /// <param name="encoding">编码方式</param>
        public static void XmlSerializeToFile(object o, string path, Encoding encoding)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            using (FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                XmlSerializeInternal(file, o, encoding);
            }
        }

        /// <summary>
        /// 从XML字符串中反序列化对象
        /// </summary>
        /// <typeparam name="T">结果对象类型</typeparam>
        /// <param name="s">包含对象的XML字符串</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>反序列化得到的对象</returns>
        public static T XmlDeserialize<T>(string s, Encoding encoding)
        {
            if (string.IsNullOrEmpty(s))
                throw new ArgumentNullException("s");
            if (encoding == null)
                throw new ArgumentNullException("encoding");

            XmlSerializer mySerializer = new XmlSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream(encoding.GetBytes(s)))
            {
                using (StreamReader sr = new StreamReader(ms, encoding))
                {
                    return (T)mySerializer.Deserialize(sr);
                }
            }
        }

        public static object XmlDeserialize(string s, Encoding encoding, Type tp)
        {
            //s = s.Substring(1, s.Length - 1);
            s = "<" + s.Substring(s.IndexOf('<') + 1);
            if (string.IsNullOrEmpty(s))
                throw new ArgumentNullException("s");
            if (encoding == null)
                throw new ArgumentNullException("encoding");
            XmlDocument xmldoc = new XmlDocument();
            try
            {
                xmldoc.LoadXml(s);
                if (xmldoc == null)
                {
                    throw new Exception("");
                }
            }
            catch (Exception ce)
            {
                //LogableClass.ToLog("载入Xml错误！", ce.Message);
            }

            XmlSerializer mySerializer = new XmlSerializer(tp);
            using (MemoryStream ms = new MemoryStream(encoding.GetBytes(s)))
            {
                using (StreamReader sr = new StreamReader(ms, encoding))
                {
                    return mySerializer.Deserialize(sr);
                }
            }
        }

        /// <summary>
        /// 读入一个文件，并按XML的方式反序列化对象。
        /// </summary>
        /// <typeparam name="T">结果对象类型</typeparam>
        /// <param name="path">文件路径</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>反序列化得到的对象</returns>
        public static T XmlDeserializeFromFile<T>(string path, Encoding encoding)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");
            if (encoding == null)
                throw new ArgumentNullException("encoding");

            string xml = File.ReadAllText(path, encoding);
            return XmlDeserialize<T>(xml, encoding);
        }




    }

    public static class ConvertionExtensions
    {
        public static T ConvertTo<T>(this IConvertible convertibleValue)
        {
            if (string.IsNullOrEmpty(convertibleValue.ToString()))
            {
                return default(T);
            }
            if (!typeof(T).IsGenericType)
            {
                return (T)System.Convert.ChangeType(convertibleValue, typeof(T));
            }
            else
            {
                Type genericTypeDefinition = typeof(T).GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(Nullable<>))
                {
                    return (T)Convert.ChangeType(convertibleValue, Nullable.GetUnderlyingType(typeof(T)));
                }
            }
            throw new InvalidCastException(string.Format("Invalid cast from type \"{0}\" to type \"{1}\".", convertibleValue.GetType().FullName, typeof(T).FullName));
        }

        public static object ConvertTo(this IConvertible convertibleValue, Type T)
        {
            if (convertibleValue == null)
            {
                return null;
            }
            if (string.IsNullOrEmpty(convertibleValue.ToString()))
            {
                return null;
            }
            if (!T.IsGenericType)
            {
                return Convert.ChangeType(convertibleValue, T);
            }
            else
            {
                Type genericTypeDefinition = T.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(Nullable<>))
                {
                    return Convert.ChangeType(convertibleValue, Nullable.GetUnderlyingType(T));
                }
            }
            throw new InvalidCastException(string.Format("Invalid cast from type \"{0}\" to type \"{1}\".", convertibleValue.GetType().FullName, T.FullName));
        }


        public static TField GetValue<TDocument, TField>(TDocument tdoc, string keyname)
        {
            TField ret = default(TField);
            if (tdoc == null)
                return ret;
            Type t = tdoc.GetType();
            MemberInfo mi = t.GetProperty(keyname);
            if (mi != null)
            {
                object val = (mi as PropertyInfo).GetValue(tdoc);
                if (val == null)
                    return ret;
                ret = ConvertionExtensions.ConvertTo<TField>(val as IConvertible);
                return ret;
            }
            else
            {
                mi = t.GetField(keyname);
                if (mi == null)
                    return ret;
                object val = (mi as FieldInfo).GetValue(tdoc);
                if (val == null)
                    return ret;
                ret = ConvertionExtensions.ConvertTo<TField>(val as IConvertible);
                return ret;
            }
        }

        public static TField GetValue<TDocument, TSubDocument, TField>(TDocument Ptdoc, Func<TDocument, TSubDocument> keynameFunc, string keyname)
        {
            TField ret = default(TField);
            if (Ptdoc == null)
                return ret;
            TSubDocument tdoc = keynameFunc(Ptdoc);
            if (tdoc == null)
                return ret;
            Type t = tdoc.GetType();
            MemberInfo mi = t.GetProperty(keyname);
            if (mi != null)
            {
                object val = (mi as PropertyInfo).GetValue(tdoc);
                if (val == null)
                    return ret;
                ret = ConvertionExtensions.ConvertTo<TField>(val as IConvertible);
                return ret;
            }
            else
            {
                mi = t.GetField(keyname);
                if (mi == null)
                    return ret;
                object val = (mi as FieldInfo).GetValue(tdoc);
                if (val == null)
                    return ret;
                ret = ConvertionExtensions.ConvertTo<TField>(val as IConvertible);
                return ret;
            }
        }

        public static TField GetValue<TDocument, TField>(Func<TDocument> keynameFunc, string keyname)
        {
            TDocument Ptdoc = keynameFunc();
            TField ret = default(TField);
            if (Ptdoc == null)
                return ret;
            TDocument tdoc = Ptdoc;// keynameFunc(Ptdoc);
            if (tdoc == null)
                return ret;
            Type t = tdoc.GetType();
            MemberInfo mi = t.GetProperty(keyname);
            if (mi != null)
            {
                object val = (mi as PropertyInfo).GetValue(tdoc);
                if (val == null)
                    return ret;
                ret = ConvertionExtensions.ConvertTo<TField>(val as IConvertible);
                return ret;
            }
            else
            {
                mi = t.GetField(keyname);
                if (mi == null)
                    return ret;
                object val = (mi as FieldInfo).GetValue(tdoc);
                if (val == null)
                    return ret;
                ret = ConvertionExtensions.ConvertTo<TField>(val as IConvertible);
                return ret;
            }
        }

        public static object GetValue(object tdoc, string keyname)
        {
            object ret = null;
            if (tdoc == null)
                return ret;
            Type t = tdoc.GetType();
            MemberInfo mi = t.GetProperty(keyname);
            if (mi != null)
            {
                object val = (mi as PropertyInfo).GetValue(tdoc);
                return val;
            }
            else
            {
                mi = t.GetField(keyname);
                if (mi == null)
                    return null;
                object val = (mi as FieldInfo).GetValue(tdoc);
                return val;
            }
        }

        public static TField GetValue<TField>(object tdoc, string keyname)
        {
            TField ret = default(TField);
            if (tdoc == null)
                return ret;
            Type t = tdoc.GetType();
            MemberInfo mi = t.GetProperty(keyname);
            if (mi != null)
            {
                object val = (mi as PropertyInfo).GetValue(tdoc);
                if (val == null)
                    return ret;
                ret = ConvertionExtensions.ConvertTo<TField>(val as IConvertible);
                return ret;
            }
            else
            {
                mi = t.GetField(keyname);
                object val = (mi as FieldInfo).GetValue(tdoc);
                if (val == null)
                    return ret;
                ret = ConvertionExtensions.ConvertTo<TField>(val as IConvertible);
                return ret;
            }
        }
        public static bool SetValue<TDocument, TField>(TDocument tdoc, string keyname, TField Newval)
        {
            try
            {
                if (tdoc == null)
                    return false;
                Type t = tdoc.GetType();
                object val = ConvertionExtensions.ConvertTo<TField>(Newval as IConvertible);
                MemberInfo mi = t.GetProperty(keyname);
                if (mi != null)
                {

                    (mi as PropertyInfo).SetValue(tdoc, val);
                    return true;
                }
                else
                {
                    mi = t.GetField(keyname);
                    (mi as FieldInfo).SetValue(tdoc, val);
                    return true;
                }
            }
            catch (Exception ce)
            {
                //LogLib.LogableClass.ToLog("ConvertionExtensions填充数据错误！", ce.Message);
            }
            return false;
        }

        public static bool SetValue<TDocument, TSubDocument, TField>(TDocument Ptdoc, Func<TDocument, TSubDocument> func, string keyname, TField Newval)
        {
            try
            {
                if (Ptdoc == null)
                {
                    return false;
                }
                TSubDocument tdoc = func(Ptdoc);
                if (tdoc == null)
                {
                    tdoc = CreateInstance<TSubDocument>();
                }
                Type t = tdoc.GetType();
                object val = ConvertionExtensions.ConvertTo<TField>(Newval as IConvertible);
                MemberInfo mi = t.GetProperty(keyname);
                if (mi != null)
                {

                    (mi as PropertyInfo).SetValue(tdoc, val);
                    return true;
                }
                else
                {
                    mi = t.GetField(keyname);
                    (mi as FieldInfo).SetValue(tdoc, val);
                    return true;
                }
            }
            catch (Exception ce)
            {
                //LogLib.LogableClass.ToLog("ConvertionExtensions填充数据错误！", ce.Message);
            }
            return false;
        }

        public static bool Equal(object tdoc, string keyname, object val)
        {
            object objval = GetValue(tdoc, keyname);

            if (objval == null)//不管是不是真的为空，先干掉
            {
                return false;
            }
            if (objval.Equals(val) || objval == val)
            {
                //LogLib.LogableClass.ToLog("判定", string.Format("{3}>>>>{0}=><{4}>.{1}==<{5}>.{2}", keyname, objval, val, true, objval?.GetType(), val?.GetType()));
                return true;
            }
            //LogLib.LogableClass.ToLog("判定", string.Format("{3}>>>>{0}=><{4}>.{1}==<{5}>.{2}", keyname, objval, val, false, objval?.GetType(), val?.GetType()));
            return false;
        }

        public static bool Equal(object tdoc, string keyname, Func<object, object> act, object val)
        {
            object objval = GetValue(tdoc, keyname);

            if (objval == null)//不管是不是真的为空，先干掉
            {
                return false;
            }
            if (act(objval).Equals(val))
            {
                //LogLib.LogableClass.ToLog("判定", string.Format("{3}>>>>{0}=><{4}>.{1}==<{5}>.{2}", keyname, objval, val, true, objval?.GetType(), val?.GetType()));
                return true;
            }
            //LogLib.LogableClass.ToLog("判定", string.Format("{3}>>>>{0}=><{4}>.{1}==<{5}>.{2}", keyname, objval, val, false, objval?.GetType(), val?.GetType()));
            return false;
        }
        public static T CreateInstance<T>()
        {
            Type t = typeof(T);
            T obj = Activator.CreateInstance<T>();
            return obj;
        }

        public static object CreateInstance(Type t)
        {
            //Type t = typeof(T);
            object obj = Activator.CreateInstance(t);
            return obj;
        }

        public static Dictionary<string, Type> GetAllProperties<T>()
        {
            Dictionary<string, Type> ret = new Dictionary<string, Type>();
            Type t = typeof(T);
            PropertyInfo[] pis = t.GetProperties();
            foreach (PropertyInfo pi in pis)
            {
                ret.Add(pi.Name, pi.PropertyType);
            }
            return ret;
        }

        public static Dictionary<string, Type> GetAllProperties(Type T)
        {
            Dictionary<string, Type> ret = new Dictionary<string, Type>();
            Type t = T;
            PropertyInfo[] pis = t.GetProperties();
            foreach (PropertyInfo pi in pis)
            {
                if (!ret.ContainsKey(pi.Name))
                    ret.Add(pi.Name, pi.PropertyType);
            }
            return ret;
        }

        public static T Clone<T>(T obj)
        {
            if (typeof(T).IsGenericType)
            {
                return obj;
            }
            T ret = CreateInstance<T>();
            Type t = typeof(T);
            PropertyInfo[] pis = t.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty);
            foreach (PropertyInfo pi in pis)
            {
                try
                {
                    pi.SetValue(ret, GetValue(obj, pi.Name));
                }
                catch (Exception e)
                {

                }
            }
            return ret;
        }

        public static object Clone(object obj)
        {
            if (obj == null) return obj;
            Type t = obj.GetType();
            if (t.IsGenericType)
            {
                return obj;
            }
            object ret = null;
            try
            {
                ret = Activator.CreateInstance(t);
            }
            catch //如果无法实例化对象，就返回空，退出来
            {
                return null;
            }
            PropertyInfo[] pis = t.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty);
            foreach (PropertyInfo pi in pis)
            {
                try
                {
                    pi.SetValue(ret, GetValue(obj, pi.Name));
                }
                catch
                {

                }
            }
            return ret;
        }

        public static T CopyTo<T>(object obj)
        {
            if (obj == null)
                return default(T);
            Type t = typeof(T);
            Type tCopy = obj.GetType();
            T ret = CreateInstance<T>();
            Dictionary<string, Type> retPips = GetAllProperties(t);
            Dictionary<string, Type> copyPips = GetAllProperties(tCopy);
            foreach (string key in retPips.Keys)
            {
                if (!copyPips.ContainsKey(key))
                {
                    continue;
                }
                if (!retPips[key].Equals(copyPips[key]))
                {
                    continue;
                }
                try
                {
                    object val = tCopy.GetProperty(key).GetValue(obj);
                    t.GetProperty(key).SetValue(ret, val);
                }
                catch (Exception ce)
                {

                }
            }
            return ret;
        }
    }

}
