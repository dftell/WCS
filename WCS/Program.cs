using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WolfInv.Com.WCS_Process;
using DataCenter;
using System.Text.RegularExpressions;
using System.Reflection;

namespace WCS
{
    static class Program
    {
        
        
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {

            ////Regex reg = new Regex(@"\{[A-Z].{3}[0-9]\}");
            ////string str ="{BC003}营业部{BC002}类型";
            ////MatchCollection mths =  reg.Matches(str);

            ////return;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //            Application.Run(new Frm_Test());
            //
            try
            {
                GlobalShare.MainAssem = Assembly.GetExecutingAssembly(); 
                GlobalShare.AppDllPath = Application.StartupPath;
                GlobalShare.Init(Application.StartupPath);
            }
            catch (Exception ce)
            {
                MessageBox.Show(ce.Message);
                return;
            }
            
            Application.Run(new Form1());
        }


    }
}