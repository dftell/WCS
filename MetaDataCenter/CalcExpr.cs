using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using XmlProcess;
using System.Text.RegularExpressions;
namespace WolfInv.Com.MetaDataCenter
{
    public class CalcExpr
    {
        HandleMethod HdlMethod;
        string EvalExpress;
        HandleBase HandleCalss;
        public string type;


        public HandleBase GetHandleClass(XmlNode node)
        {
            LoadXml(node);
            Init();
            return HandleCalss;
        }

        public HandleBase GetHandleClass(string streval,string strtype)
        {
            EvalExpress = streval;
            type = strtype;
            Init();
            return HandleCalss;
        }

        public void LoadXml(XmlNode node)
        {
            //this.TargetField = XmlUtil.GetSubNodeText(node, "@f");
            this.EvalExpress = XmlUtil.GetSubNodeText(node, "@expr");
            type = XmlUtil.GetSubNodeText(node, "@method");
           
        }

        public string getValue(string streval, string strtype, string datapoint, string val, string splitor = "|")
        {
            return getValue(streval, strtype, new string[] { datapoint }, new string[] { val }, splitor);
        }
        public string getValue(string streval, string strtype, string[] datapoints,string[] vals, string splitor = "|")
        {
            int cnt = Math.Min(datapoints.Length, vals.Length);
            UpdateData ud = new UpdateData();
            for(int i=0;i<cnt;i++)
            {
                UpdateItem ui = new UpdateItem(datapoints[i], vals[i]);
                if (!ud.Items.ContainsKey(datapoints[i]))
                    ud.Items.Add(datapoints[i],ui);
            }
            return getValue(streval, strtype, ud, splitor);
        }
        public string getValue(string streval, string strtype,UpdateData inputData, string splitor = "|")
        {

            string[] cols = streval.Split(splitor.ToCharArray());
            if (cols.Length == 0)
                return null;
            //List<string> strparam = new List<string>();
            for (int i = 1; i < cols.Length; i++)
            {
                string dpt = cols[i];
                string val = dpt;//默认为常量
                if (inputData.Items.ContainsKey(dpt))//field pc
                {
                    val = inputData.Items[dpt].value;
                    if (val == null)//引用的其中一个值为空，暂时不计算结果
                        return null;
                }
                string strindex = "{" + string.Format("{0}", i - 1) + "}";
                cols[0] = cols[0].Replace(strindex, val);
                //strparam.Add(val);
            }
            
            HandleBase hb = GetHandleClass(cols[0], strtype);
            return hb.Handle();
        }



        void Init()
        {
            switch (type.ToLower())
            {
                
                
                case "date":
                    {
                        HdlMethod = HandleMethod.Date;
                        HandleCalss = new DateHandle(EvalExpress);
                        break;
                    }
                case "string":
                
                    {
                        HdlMethod = HandleMethod.String;
                        HandleCalss = new StringHandle(EvalExpress);
                        break;
                    }
                case "math":
                default:
                    {
                        HdlMethod = HandleMethod.Math;
                        HandleCalss = new MathHandle(EvalExpress);
                        break;
                    }
            }
        }
    }

    public enum HandleMethod
    {
        String,
        Math,
        
        Date,
        Logic
    }
    
    public abstract class HandleBase
    {
        protected string InputExpr;
        public abstract string Handle();
    }

    public class MathHandle:HandleBase 
    {
        
        public MathHandle(string expr)
        {
            InputExpr = expr;
        }

        public override string Handle()
        {
            ScriptEngine src = new ScriptEngine(ScriptLanguage.JScript);
            //src.Language = "ENU";
            try
            {
                object str = src.eval_r(InputExpr, "");
                if (str == null)
                    return null;
                return str.ToString();
            }
            catch (Exception ce)
            {
                return null;
            }

            //return src.Eval(InputExpr);
        }
    }

    public class StringHandle : HandleBase
    {
        public StringHandle(string expr)
        {
            InputExpr = expr;
        }
        public override string Handle()
        {
            ScriptEngine src = new ScriptEngine(ScriptLanguage.JScript);
            //src.Language = "ENU";
            try
            {
                object str = src.eval_r(InputExpr, "");
                if (str == null)
                    return null;
                return str.ToString().Trim();
            }
            catch (Exception ce)
            {
                return null;
            }
        }
    }

    public class DateHandle : HandleBase
    {
        public DateHandle(string expr)
        {
            InputExpr = expr;
        }
        public override string Handle()
        {
            return InputExpr;
        }
    }

    public class DataPointReg
    {
        public static string GetExpress(string str)
        {
            Regex reg = new Regex(@"\{[A-Z].{3}[0-9]\}");
            
            if(!reg.IsMatch(str))
            {
                return null;
            }
            Match m = reg.Match(str);
            if (m.Length > 2)
            {
                return str.Substring(m.Index + 1, m.Length - 2);
            }
            return null;
        }

        public static List<string> GetExpresses(string str)
        {
            Regex reg = new Regex(@"\{[A-Z].{4}\}");

            if (!reg.IsMatch(str))
            {
                return null;
            }
            List<string> ret = new List<string>();
            MatchCollection mes = reg.Matches(str);
            for (int i = 0; i < mes.Count; i++)
            {
                Match m = mes[i];
                if (m.Length > 2)
                {
                    ret.Add(str.Substring(m.Index + 1, m.Length - 2));
                    //return str.Substring(m.Index + 1, m.Length - 2);
                }
            }
            return ret ;
        }

        public static List<string> GetWideExpresses(string str)
        {
            Regex reg = new Regex(@"\{[A-Z].{n}\}");

            if (!reg.IsMatch(str))
            {
                return null;
            }
            List<string> ret = new List<string>();
            MatchCollection mes = reg.Matches(str);
            for (int i = 0; i < mes.Count; i++)
            {
                Match m = mes[i];
                if (m.Length > 2)
                {
                    ret.Add(str.Substring(m.Index + 1, m.Length - 2));
                    //return str.Substring(m.Index + 1, m.Length - 2);
                }
            }
            return ret;
        }
    
    }
}
