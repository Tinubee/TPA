using MvUtils;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using TPA.Schemas;

namespace TPA
{
    public enum 동작구분
    {
        Live = 0,
        LocalTest = 2,
    }

    public static class Global
    {
        public static MainForm MainForm = null;
        public delegate void BaseEvent();
        public static event EventHandler<Boolean> Initialized;
        public delegate void 로그이벤트(String 내용);
        public static event 로그이벤트 로그알람;

        public const String SkinName = "The Bezier";
        public const String BlackPalette = "Office Black";
        public const String ColorPalette = "Office Colorful";

        private const String 로그영역 = "프로그램";
        public static 환경설정 환경설정;
        public static 로그자료 로그자료;
        public static 유저자료 유저자료;
        public static 모델자료 모델자료;
        public static 장치통신 장치통신;
        public static 조명제어 조명제어;
        public static 그랩제어 그랩제어;
        public static 비전검사 비전검사;
        public static 큐알제어 큐알제어;
        public static 검사자료 검사자료;
        public static 큐알검증 큐알검증;
        public static 큐알인쇄 큐알인쇄;
        public static 제품검사수행 제품검사수행;
        public static 변위센서제어 변위센서제어;
        public static 사진자료 사진자료;

        public static class 장치상태
        {
            public static Boolean 정상여부 { get { return true; } }
            public static Boolean 측면카메라1 { get { return Global.그랩제어.측면카메라1.상태; } }
            public static Boolean 측면카메라2 { get { return Global.그랩제어.측면카메라2.상태; } }
            public static Boolean 상부카메라 { get { return Global.그랩제어.상부카메라.상태; } }
            public static Boolean 하부카메라1 { get { return Global.그랩제어.하부카메라1.상태; } }
            public static Boolean 하부카메라2 { get { return Global.그랩제어.하부카메라2.상태; } }
            public static Boolean 커넥터설삽카메라1 { get { return Global.그랩제어.커넥터설삽카메라1.상태; } }
            public static Boolean 커넥터설삽카메라2 { get { return Global.그랩제어.커넥터설삽카메라2.상태; } }
            public static Boolean 하부큐알1리더 { get { return Global.큐알제어.하부큐알리더1.연결여부; } }
            public static Boolean 하부큐알2리더 { get { return Global.큐알제어.하부큐알리더2.연결여부; } }
            public static Boolean 상부큐알리더 { get { return Global.큐알제어.상부큐알리더.연결여부; } }
            public static Boolean 라벨기 { get { return Global.큐알인쇄.연결여부; } }
            public static Boolean 조명장치 { get { return Global.조명제어.정상여부; } }
            public static Boolean 자동수동 { get => Global.장치통신.자동수동여부; }
        }

        public static Boolean Init()
        {
            try {
                로그자료 = new 로그자료();
                환경설정 = new 환경설정();
                유저자료 = new 유저자료();
                장치통신 = new 장치통신();
                조명제어 = new 조명제어();
                모델자료 = new 모델자료();
                그랩제어 = new 그랩제어();
                비전검사 = new 비전검사();
                검사자료 = new 검사자료();
                큐알제어 = new 큐알제어();
                큐알검증 = new 큐알검증();
                큐알인쇄 = new 큐알인쇄();
                제품검사수행 = new 제품검사수행();
                변위센서제어 = new 변위센서제어();
                사진자료 = new 사진자료();

                로그자료.Init();
                환경설정.Init();
                유저자료.Init();
                모델자료.Init();
                검사자료.Init();
                장치통신.Init();
                비전검사.Init();
                큐알검증.Init();
                제품검사수행.Init();
                사진자료.Init();

                if (환경설정.동작구분 == 동작구분.Live)
                {  
                    if (!장치통신.Open()) new Exception("PLC 서버 연결 실패");
                    if (!그랩제어.Init()) new Exception("카메라 초기화 실패");
                    큐알제어.Init();
                    //큐알인쇄.Init();
                    조명제어.Init();
                    변위센서제어.Init();
                }

                Global.정보로그(로그영역, "초기화", "시스템을 초기화 합니다.", false);
                Initialized?.Invoke(null, true);
                return true;
            }
            catch (Exception ex) {
                Utils.DebugException(ex, 3);
                Global.오류로그(로그영역, "초기화 오류", "시스템 초기화에 실패하였습니다.\n" + ex.Message, true);
            }
            Initialized.Invoke(null, false);
            return false;
        }

        public static Boolean Close()
        {
            Global.정보로그(로그영역, "종료", "시스템을 종료 합니다.", false);
            try
            {
                if (환경설정.동작구분 == 동작구분.Live) {
                    큐알제어.Close();
                    큐알인쇄.Close();
                    조명제어.Close();
                    변위센서제어.Close();
                }

                장치통신.Close();
                유저자료.Close();
                환경설정.Close();
                그랩제어.Close();
                비전검사.Close();
                검사자료.Close();
                모델자료.Close();
                로그자료.Close();
                사진자료.Close();

                Properties.Settings.Default.Save();
                Debug.WriteLine("시스템 종료");
                return true;
            }
            catch (Exception ex) {
                return Utils.ErrorMsg("프로그램 종료 중 오류가 발생하였습니다.\n" + ex.Message);
            }
        }

        public static void Start()
        {
            장치통신.Start();
            if (Global.환경설정.동작구분 != 동작구분.Live) return;
            큐알제어.Start();
            큐알인쇄.Start();
        }

        public static void DxLocalization()
        {
            if (Localization.CurrentLanguage != Language.KO) return;
            MvUtils.Localization.CurrentLanguage = MvUtils.Localization.Language.KO;
            MvUtils.DxDataGridLocalizer.Enable();
            MvUtils.DxEditorsLocalizer.Enable();
            MvUtils.DxDataFilteringLocalizer.Enable();
            MvUtils.DxLayoutLocalizer.Enable();
            MvUtils.DxBarLocalizer.Enable();
        }

        public static String GetGuid()
        {
            Assembly assembly = typeof(Program).Assembly;
            GuidAttribute attribute = assembly.GetCustomAttributes(typeof(GuidAttribute), true)[0] as GuidAttribute;
            return attribute.Value;
        }

        #region 로그 / Alert
        private static AlertControl 알림화면 = new AlertControl() { AutoHeight = false, FormSize = new System.Drawing.Size(400, 150) };
        private delegate void ShowMessageDelegate(Form Owner, 로그정보 로그);
        private static void ShowMessage(Form Owner, 로그정보 로그)
        {
            if (Owner == null || Owner.IsDisposed) return;
            if (Owner.InvokeRequired) {
                Owner.BeginInvoke(new ShowMessageDelegate(ShowMessage), new object[] { Owner, 로그 });
                return;
            }
        
            if (로그.구분 == 로그구분.오류)
                알림화면.Show(AlertControl.AlertTypes.Invalid, 로그.제목, 로그.내용, Owner);
            else if (로그.구분 == 로그구분.경고)
                알림화면.Show(AlertControl.AlertTypes.Warning, 로그.제목, 로그.내용, Owner);
            else if (로그.구분 == 로그구분.정보)
                알림화면.Show(AlertControl.AlertTypes.Information, 로그.제목, 로그.내용, Owner);
        }
        public static void ShowMessage(로그정보 로그) => ShowMessage(MainForm, 로그);
        
        public static 로그정보 로그기록(string 영역, 로그구분 구분, string 제목, string 내용)
        {
            try {
                로그정보 로그 = 로그자료.Add(영역, 구분, 제목, 내용);                
                로그알람?.BeginInvoke($"{Utils.FormatDate(DateTime.Now, "{0:HH:mm:ss}")}    {구분.ToString()}    {영역}    {제목}    {내용}", null, null);
                Debug.WriteLine($"{Utils.FormatDate(DateTime.Now, "{0:HH:mm:ss}")}\t{구분.ToString()}\t{영역}\t{제목}\t{내용}");
                return 로그;
            }
            catch (Exception ex) {
                Debug.WriteLine(ex.Message, "로그기록 오류");
            }
            return null;
        }
        
        public static 로그정보 오류로그(string 영역, string 제목, string 내용, bool Show) =>
            오류로그(영역, 제목, 내용, Show ? MainForm : null);
        public static 로그정보 오류로그(string 영역, string 제목, string 내용, Form Owner)
        {
            로그정보 로그 = 로그기록(영역, 로그구분.오류, 제목, 내용);
            if (로그 != null && Owner != null) ShowMessage(Owner, 로그);
            return 로그;
        }
        
        public static 로그정보 경고로그(string 영역, string 제목, string 내용, bool Show) =>
            경고로그(영역, 제목, 내용, Show ? MainForm : null);
        public static 로그정보 경고로그(string 영역, string 제목, string 내용, Form Owner)
        {
            로그정보 로그 = 로그기록(영역, 로그구분.경고, 제목, 내용);
            if (로그 != null && Owner != null) ShowMessage(Owner, 로그);
            return 로그;
        }
        
        public static 로그정보 정보로그(string 영역, string 제목, string 내용, bool Show) =>
            정보로그(영역, 제목, 내용, Show ? MainForm : null);
        public static 로그정보 정보로그(string 영역, string 제목, string 내용, Form Owner)
        {
            로그정보 로그 = 로그기록(영역, 로그구분.정보, 제목, 내용);
            if (로그 != null && Owner != null) ShowMessage(Owner, 로그);
            return 로그;
        }

        #endregion
    }
}
