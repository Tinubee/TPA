using MvUtils;
using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.UserSkins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace TPA
{
    internal static class Program
    {
        private const String 로그영역 = "프로그램";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Boolean createdNew = false;
            Mutex mtx = new Mutex(true, Global.GetGuid(), out createdNew);
            if (!createdNew) {
                Utils.ErrorMsg("프로그램이 이미 실행중입니다.");
                Application.Exit();
                return;
            }

#pragma warning disable
            DevExpress.Data.CurrencyDataController.DisableThreadingProblemsDetection = true;

            if (!String.IsNullOrEmpty(Properties.Settings.Default.SkinName))
                DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle("The Bezier", Properties.Settings.Default.SvgPaletteName);//Properties.Settings.Default.SkinName
            DevExpress.LookAndFeel.UserLookAndFeel.Default.StyleChanged += Default_StyleChanged;

            Application.ThreadException += Application_ThreadException;
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.Automatic);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Global.환경설정 = new Schemas.환경설정();
            Global.환경설정.Init();
            Global.MainForm = new MainForm();
            Application.Run(Global.MainForm);
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            Global.오류로그(로그영역, "예외발생", $"Application_ThreadException " + e.Exception.Message, true);
            Global.Close();
            Application.Exit(new System.ComponentModel.CancelEventArgs(false));
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Global.오류로그(로그영역, "예외발생", $"CurrentDomain_UnhandledException " + ((Exception)e.ExceptionObject).Message + " Is Terminatiog : " + e.IsTerminating.ToString(), true);
            Global.Close();
        }

        private static void Default_StyleChanged(object sender, EventArgs e)
        {
            Debug.WriteLine(DevExpress.LookAndFeel.UserLookAndFeel.Default.ActiveSvgPaletteName, DevExpress.LookAndFeel.UserLookAndFeel.Default.ActiveSkinName);
            Properties.Settings.Default.SkinName = DevExpress.LookAndFeel.UserLookAndFeel.Default.ActiveSkinName;
            Properties.Settings.Default.SvgPaletteName = DevExpress.LookAndFeel.UserLookAndFeel.Default.ActiveSvgPaletteName;
            Properties.Settings.Default.Save();
        }
    }
}
