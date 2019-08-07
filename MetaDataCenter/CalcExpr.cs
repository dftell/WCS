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


        public HandleBase GetHandleCalss(XmlNode node)
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
            string type = XmlUtil.GetSubNodeText(node, "@method");
            
           
        }

        void Init()
        {
            switch (type)
            {
                case "String":
                    {
                        HdlMethod = HandleMethod.String;
                        HandleCalss = new StringHandle(EvalExpress);
                        break;
                    }
                case "Date":
                    {
                        HdlMethod = HandleMethod.Date;
                        HandleCalss = new DateHandle(EvalExpress);
                        break;
                    }
                case "Math":
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
        Math,
        String,
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
            return InputExpr;
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
