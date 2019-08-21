using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WolfInv.Com.WCS_Process;
using WolfInv.Com.DataCenter;
using System.Text.RegularExpressions;
using System.Reflection;
using Xilium.CefGlue;
using System.IO;
using System.Linq;
using Xilium.CefGlue.WindowsForms;
namespace WCS
{
    static class Program
    {

        public static CefWebBrowser WebView;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            try
            {
                CefRuntime.Load();
            }
            catch (DllNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 1;
            }
            catch (CefRuntimeException ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 2;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 3;
            }

            var mainArgs = new CefMainArgs(args);
            var app = new DemoApp();

            var exitCode = CefRuntime.ExecuteProcess(mainArgs, app);
            if (exitCode != -1)
                return exitCode;

            var settings = new CefSettings
            {
                // BrowserSubprocessPath = @"D:\fddima\Projects\Xilium\Xilium.CefGlue\CefGlue.Demo\bin\Release\Xilium.CefGlue.Demo.exe",
                SingleProcess = false,
                MultiThreadedMessageLoop = true,
                LogSeverity = CefLogSeverity.Disable,
                LogFile = "CefGlue.log",
            };

            CefRuntime.Initialize(mainArgs, settings, app);

            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);


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
                return 0;
            }

            Application.Run(new Form1());
            CefRuntime.Shutdown();
            return 0;
        }

        
    }

    internal sealed class DemoApp : CefApp
    {
        protected override void OnBeforeCommandLineProcessing(string processType, CefCommandLine commandLine)
        {
            ;
        }
    }
}