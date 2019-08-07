using System;
using Microsoft.JScript;
using Microsoft.JScript.Vsa;
using System.Text;
namespace WolfInv.Com.MetaDataCenter
{

    /// <summary>
    /// �ű�����
    /// </summary>
    public enum ScriptLanguage
    {

        /// <summary>
        /// JScript�ű�����
        /// </summary>
        JScript,
        /// <summary>
        /// VBscript�ű�����
        /// </summary>
        VBscript,
        /// <summary>
        /// JavaScript�ű�����
        /// </summary>
        JavaScript

    }
    /// <summary>
    /// �ű����д������
    /// </summary>
    public delegate void RunErrorHandler();
    /// <summary>
    /// �ű����г�ʱ����
    /// </summary>
    public delegate void RunTimeoutHandler();
    /// <summary>
    /// ScriptEngine��
    /// </summary>
    public class ScriptEngine
    {

        //private ScriptControl msc;
        VsaEngine msc;
        /// <summary>
        /// ����ű����д����¼�
        /// </summary>
        public event RunErrorHandler RunError;
        /// <summary>
        /// ����ű����г�ʱ�¼�
        /// </summary>
        public event RunTimeoutHandler RunTimeout;

        /// <summary>
        ///���캯�� Ĭ��Ϊ VBscript �ű�����
        /// </summary>
        public ScriptEngine()
            : this(ScriptLanguage.VBscript)
        { }
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="language">�ű�����</param>
        public ScriptEngine(ScriptLanguage language)
        {
            msc = VsaEngine.CreateEngine();
            ////this.msc = new ScriptControlClass();

            ////this.msc.UseSafeSubset = true;
            ////this.msc.Language = language.ToString();
            ////((DScriptControlSource_Event)this.msc).Error += new DScriptControlSource_ErrorEventHandler(OnError);
            ////((DScriptControlSource_Event)this.msc).Timeout += new DScriptControlSource_TimeoutEventHandler(OnTimeout);
        }


        /// <summary>
        /// ����Eval����
        /// </summary>
        /// <param name="expression">���ʽ</param>
        /// <param name="codeBody">������</param>
        /// <returns>����ֵobject</returns>
        public object eval_r(string expression, string codeBody)
        {
            return Eval.JScriptEvaluate(expression, msc);
            //msc.AddCode(codeBody);
            //return msc.Eval(expression);
        }
        /// <summary>
        /// ����Eval����
        /// </summary>
        /// <param name="language">�ű�����</param>
        /// <param name="expression">���ʽ</param>
        /// <param name="codeBody">������</param>
        /// <returns>����ֵobject</returns>
        public object eval_r(ScriptLanguage language, string expression, string codeBody)
        {
            //if (this.Language != language)
            //    this.Language = language;
            return eval_r(expression, codeBody);
        }
        /// <summary>
        /// ����Run����
        /// </summary>
        /// <param name="mainFunctionName">��ں�������</param>
        /// <param name="parameters">����</param>
        /// <param name="codeBody">������</param>
        /// <returns>����ֵobject</returns>
        public object Run(string mainFunctionName, object[] parameters, string codeBody)
        {
            return null;
            ////msc.AppDomain =
            ////this.msc.PushScriptObject(ScriptBlock.);
            ////return msc.Run(mainFunctionName, ref parameters);
        }

        /// <summary>
        /// ����Run����
        /// </summary>
        /// <param name="language">�ű�����</param>
        /// <param name="mainFunctionName">��ں�������</param>
        /// <param name="parameters">����</param>
        /// <param name="codeBody">������</param>
        /// <returns>����ֵobject</returns>
        public object Run(ScriptLanguage language, string mainFunctionName, object[] parameters, string codeBody)
        {
            //if (this.Language != language)
            //    this.Language = language;
            return Run(mainFunctionName, parameters, codeBody);
        }

        /// <summary>
        /// ���������Ѿ���ӵ� ScriptControl �е� Script ����Ͷ���
        /// </summary>
        public void Reset()
        {
            this.msc.Reset();
        }
        /// <summary>
        /// ��ȡ�����ýű�����
        /// </summary>
        ////public ScriptLanguage Language
        ////{
        ////    get { return (ScriptLanguage)Enum.Parse(typeof(ScriptLanguage), this.msc.Language, false); }
        ////    set { this.msc.Language = value.ToString(); }
        ////}

        /// <summary>
        /// ��ȡ�����ýű�ִ��ʱ�䣬��λΪ����
        /// </summary>
        public int Timeout
        {
            get { return 0; }
        }

        /// <summary>
        /// �����Ƿ���ʾ�û�����Ԫ��
        /// </summary>
        ////public bool AllowUI
        ////{
        ////    get { return this.msc.AllowUI; }
        ////    set { this.msc.AllowUI = value; }
        ////}

        /// <summary>
        /// ����Ӧ�ó����Ƿ��б�����Ҫ��
        /// </summary>
        ////public bool UseSafeSubset
        ////{
        ////    get { return this.msc.UseSafeSubset; }
        ////    set { this.msc.UseSafeSubset = true; }
        ////}

        /// <summary>
        /// RunError�¼�����
        /// </summary>
        private void OnError()
        {
            if (RunError != null)
                RunError();
        }

        /// <summary>
        /// OnTimeout�¼�����
        /// </summary>
        private void OnTimeout()
        {
            if (RunTimeout != null)
                RunTimeout();
        }
    }
}
