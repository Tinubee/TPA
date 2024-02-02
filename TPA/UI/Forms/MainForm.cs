using DevExpress.XtraWaitForm;
using MvUtils;
using Npgsql;
using OpenCvSharp.Dnn;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TPA.Schemas;
using TPA.UI.Controls;
using static TPA.Schemas.장치통신;

namespace TPA
{
    public partial class MainForm : DevExpress.XtraBars.TabForm
    {
        private LocalizationMain 번역 = new LocalizationMain();
        private UI.Controls.WaitForm WaitForm;
        public SynchronizationContext uiContext;

        public MainForm()
        {
            InitializeComponent();
            this.ShowWaitForm();
            this.e프로젝트.Caption = $"IVM : {환경설정.프로젝트번호}";
            this.SetLocalization();
            this.TabFormControl.SelectedPage = this.p검사하기;
            this.Shown += MainFormShown;
            this.FormClosing += MainFormClosing;

            this.타이틀.ItemDoubleClick += 타이틀_ItemDoubleClick;
        }

        private void 타이틀_ItemDoubleClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            List<string> resultRow = new List<string>();
            var connectionString = "Host=localhost;Port=5432;Username=postgres;Password=ivmadmin;Database=vda590tpa_ohsung";

            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                // 첫 번째 쿼리: public.inspl 테이블에서 ilwdt 값을 가져옵니다.
                var ilwdtValues = new List<DateTime>();
                using (var cmd = new NpgsqlCommand("SELECT ilwdt FROM public.inspl ORDER BY ilwdt DESC LIMIT 30", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ilwdtValues.Add(reader.GetDateTime(0));
                        }
                    }
                }

                // 두 번째 쿼리: 가져온 ilwdt 값들을 사용하여 public.inspd 테이블에서 iditm이 5인 데이터를 찾습니다.
                foreach (var ilwdt in ilwdtValues)
                {

                    // int 형의 List 생성
                    List<int> iditmValues = new List<int>();

                    iditmValues.Add(54);
                    iditmValues.Add(55);
                    iditmValues.Add(56);
                    iditmValues.Add(57);
                    iditmValues.Add(47);
                    iditmValues.Add(48);
                    iditmValues.Add(49);
                    iditmValues.Add(50);
                    iditmValues.Add(41);
                    iditmValues.Add(42);
                    iditmValues.Add(43);
                    iditmValues.Add(44);
                    iditmValues.Add(53);
                    iditmValues.Add(52);
                    for (int i = 58; i < 76; i++) iditmValues.Add(i);
                    for (int i = 23; i < 39; i++) iditmValues.Add(i);
                    for (int i = 81; i < 97; i++) iditmValues.Add(i);
                    iditmValues.Add(5);
                    iditmValues.Add(6);
                    iditmValues.Add(9);
                    iditmValues.Add(8);

                    for (int i = 10; i < 14; i++) iditmValues.Add(i);
                    for (int i = 15; i < 23; i++) iditmValues.Add(i);

                    for (int i = 97; i < 101; i++) iditmValues.Add(i);
                    for (int i = 103; i < 114; i++) iditmValues.Add(i);

                    iditmValues.Add(14);
                    iditmValues.Add(101);
                    iditmValues.Add(102);

                    //iditmValues.Add(0);
                    //iditmValues.Add(0);
                    //iditmValues.Add(0);
                    //iditmValues.Add(0);
                    //iditmValues.Add(0);
                    //iditmValues.Add(0);


                    // List에 값 추가
                    //
                    //
                    //
                    //

                    foreach (int i in iditmValues)
                    {

                        //Debug.WriteLine(i);
                        using (var cmd = new NpgsqlCommand($"SELECT * FROM public.inspd WHERE idwdt = @ilwdt AND iditm ={i}", conn))
                        {
                            cmd.Parameters.AddWithValue("@ilwdt", ilwdt);

                            using (var reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    // 여기에서 public.inspd 테이블의 각 열의 데이터를 읽고 처리합니다.
                                    // 예: Console.WriteLine(reader["column_name"]);
                                    resultRow.Add(reader["idval"].ToString());
                                    //resultRow.Add(reader["idmes"].ToString());
                                    //Debug.WriteLine(reader["idmes"]);
                                }
                            }
                        }
                    }
                }


                // 4개씩 데이터를 묶어서 row로 변환
                List<List<string>> rows = new List<List<string>>();
                for (int i = 0; i < resultRow.Count; i += 98)
                {
                    List<string> row = resultRow.Skip(i).Take(98).ToList();
                    rows.Add(row);
                }

                // CSV 파일 경로
                string filePath = "C:\\IVM\\resultRow.csv";

                // StreamWriter를 사용하여 CSV 파일 생성 또는 열기
                using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
                {

                    // 컬럼명 작성
                    writer.WriteLine("DistanceCntL,DistanceCntR,DistanceCntR,DistanceCntL");

                    // 데이터 작성
                    foreach (var row in rows)
                    {
                        string csvLine = string.Join(",", row);
                        writer.WriteLine(csvLine);
                    }
                }

                Console.WriteLine("CSV 파일이 생성되었습니다.");
            }
        }

        private void ShowWaitForm()
        {
            WaitForm = new UI.Controls.WaitForm() { ShowOnTopMode = ShowFormOnTopMode.AboveAll };
            WaitForm.Show(this);
        }

        private void HideWaitForm()
        {
            if (this.WaitForm.InvokeRequired)
            {
                this.BeginInvoke(new Action(HideWaitForm));
                return;
            }

            if (Global.환경설정.동작구분 != 동작구분.Live)
            {
                WaitForm.Close();
                return;
            }

            Task.Run(() =>
            {
                while (!Global.조명제어.초기점멸작업완료여부)
                {
                    Task.Delay(500).Wait();
                }
                this.Invoke(new Action(() =>
                {
                    WaitForm.Close();
                }));
            });
        }

        private void MainFormShown(object sender, EventArgs e)
        {
            Global.Initialized += GlobalInitialized;
            uiContext = SynchronizationContext.Current;
            Task.Run(() => { Global.Init(); });
        }

        private void MainFormClosing(object sender, FormClosingEventArgs e)
        {
            if (Global.환경설정.사용권한 == 유저권한구분.없음) this.CloseForm();
            else
            {
                e.Cancel = !Utils.Confirm(this, 번역.종료확인, Localization.확인.GetString());
                if (!e.Cancel) this.CloseForm();
            }
        }

        private void GlobalInitialized(object sender, Boolean e)
        {
            this.BeginInvoke(new Action(() => GlobalInitialized(e)));
        }

        private void GlobalInitialized(Boolean e)
        {
            Global.Initialized -= GlobalInitialized;
            if (!e) { this.Close(); return; }
            this.HideWaitForm();
            Common.SetForegroundWindow(this.Handle.ToInt32());

            Global.환경설정.시스템관리자로그인();
            Global.DxLocalization();
            this.Init();
            Global.Start();
        }

        private void Init()
        {
            this.SetLocalization();

            this.e결과뷰어.Init();
            this.e검사설정.Init();
            //this.e변수설정.Init();
            this.e장치설정.Init();
            this.e검사내역.Init();
            this.e검사피봇.Init();
            this.e상태뷰어.Init();
            this.e로그내역.Init();
            this.e큐알검증.Init();
            this.e셋팅뷰어.Init();
            this.e모델정보뷰어.Init();
            this.ePLC모니터.Init();

            this.p환경설정.Enabled = Global.환경설정.권한여부(유저권한구분.시스템);
            this.p검사내역.Enabled = Global.환경설정.권한여부(유저권한구분.관리자);
            this.TabFormControl.AllowMoveTabs = false;
            this.TabFormControl.AllowMoveTabsToOuterForm = false;

            Global.비전검사.SetDisplay(카메라구분.Cam01, this.e결과뷰어.GetDisplayControl(DisplayUI.좌측면));
            Global.비전검사.SetDisplay(카메라구분.Cam02, this.e결과뷰어.GetDisplayControl(DisplayUI.우측면));
            Global.비전검사.SetDisplay(카메라구분.Cam03, this.e결과뷰어.GetDisplayControl(DisplayUI.상부면));
            Global.비전검사.SetDisplay(카메라구분.Cam04, this.e결과뷰어.GetDisplayControl(DisplayUI.좌측하부));
            Global.비전검사.SetDisplay(카메라구분.Cam05, this.e결과뷰어.GetDisplayControl(DisplayUI.우측하부));
            Global.비전검사.SetDisplay(카메라구분.Cam06, this.e결과뷰어.GetDisplayControl(DisplayUI.커넥터설삽상부));
            Global.비전검사.SetDisplay(카메라구분.Cam07, this.e결과뷰어.GetDisplayControl(DisplayUI.커넥터설삽하부));

            if (Global.환경설정.동작구분 == 동작구분.Live)
                this.WindowState = FormWindowState.Maximized;
        }

        private void CloseForm()
        {
            this.e장치설정.Close();
            this.e로그내역.Close();
            this.e상태뷰어.Close();
            Global.Close();
        }

        private void SetLocalization()
        {
            this.Text = this.번역.타이틀;
            this.타이틀.Caption = this.번역.타이틀;
            this.p검사하기.Text = this.번역.검사하기;
            //this.p그랩뷰어.Text = this.번역.그랩장치;
            this.p검사내역.Text = this.번역.검사내역;
            this.p환경설정.Text = this.번역.환경설정;
            this.t검사설정.Text = this.번역.검사설정;
            this.t장치설정.Text = this.번역.장치설정;
            //this.t변수설정.Text = this.번역.변수설정;
            this.t큐알검증.Text = this.번역.큐알검증;
            this.t로그내역.Text = this.번역.로그내역;
        }

        private class LocalizationMain
        {
            private enum Items
            {
                [Translation("Inspection", "검사하기", "Inšpekcia")]
                검사하기,
                [Translation("History", "검사내역", "História")]
                검사내역,
                [Translation("Preferences", "환경설정", "Predvoľby")]
                환경설정,
                [Translation("Settings", "검사설정", "Nastavenie")]
                검사설정,
                [Translation("Devices", "장치설정", "Zariadenia")]
                장치설정,
                [Translation("Cameras", "카메라", "Kamery")]
                그랩장치,
                [Translation("Variables", "변수설정", "Variables")]
                변수설정,
                [Translation("QR Validate", "큐알검증", "QR Validate")]
                큐알검증,
                [Translation("Logs", "로그내역", "Denníky")]
                로그내역,
                [Translation("Are you want to exit the program?", "프로그램을 종료하시겠습나까?", "Naozaj chcete ukončiť program?")]
                종료확인,
            }
            private String GetString(Items item) { return Localization.GetString(item); }

            public String 타이틀 { get => Localization.제목.GetString(); }
            public String 검사하기 { get => GetString(Items.검사하기); }
            public String 검사내역 { get => GetString(Items.검사내역); }
            public String 환경설정 { get => GetString(Items.환경설정); }
            public String 검사설정 { get => GetString(Items.검사설정); }
            public String 장치설정 { get => GetString(Items.장치설정); }
            public String 그랩장치 { get => GetString(Items.그랩장치); }
            public String 변수설정 { get => GetString(Items.변수설정); }
            public String 큐알검증 { get => GetString(Items.큐알검증); }
            public String 로그내역 { get => GetString(Items.로그내역); }
            public String 종료확인 { get => GetString(Items.종료확인); }
        }

        public delegate void 제품검사수행델리게이트(PLC커맨드목록 커맨드, Int32 제품인덱스);
        //public delegate void 제품검사수행델리게이트();
        public event 제품검사수행델리게이트 제품검사수행이벤트;

        private void e제품수행이벤트발생_Click(object sender, EventArgs e)
        {
            Dictionary<PLC커맨드목록, Int32> values = new Dictionary<PLC커맨드목록, Int32>();
            foreach (PLC커맨드목록 PLC커맨드 in typeof(PLC커맨드목록).GetEnumValues())
            {
                // org if (PLC커맨드 < PLC커맨드목록.검사위치01 || PLC커맨드 > PLC커맨드목록.검사위치09)
                if (PLC커맨드 < PLC커맨드목록.하부큐알트리거 || PLC커맨드 > PLC커맨드목록.결과요청트리거)
                    continue;
                values.Add(PLC커맨드, 0);
            }

            foreach (KeyValuePair<PLC커맨드목록, Int32> item in values)
            {
                제품검사수행이벤트?.Invoke(item.Key, item.Value);
            }
        }

        private void bBusyLow_Click(object sender, EventArgs e)
        {
            foreach (PLC커맨드목록 구분 in typeof(PLC커맨드목록).GetEnumValues())
            {
                Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[구분].Busy주소, 0);
            }
        }

        private void b모든CpltLow_Click(object sender, EventArgs e)
        {
            foreach (PLC커맨드목록 구분 in typeof(PLC커맨드목록).GetEnumValues())
            {
                Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[구분].완료주소, 0);
            }
        }

        private void bTrigger하부큐알_Click(object sender, EventArgs e)
        {
            Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[장치통신.PLC커맨드목록.하부큐알트리거].요청주소, 1);
        }

        private void bTrigger바닥평면_Click(object sender, EventArgs e)
        {
            Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[장치통신.PLC커맨드목록.바닥평면트리거].요청주소, 1);
        }

        private void bTrigger상부큐알_Click(object sender, EventArgs e)
        {
            Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[장치통신.PLC커맨드목록.상부큐알트리거].요청주소, 1);
        }

        private void bTrigger커버들뜸_Click(object sender, EventArgs e)
        {
            Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[장치통신.PLC커맨드목록.라벨발행트리거].요청주소, 1);
        }

        private void bTriggerLow_Click(object sender, EventArgs e)
        {
            foreach (PLC커맨드목록 구분 in typeof(PLC커맨드목록).GetEnumValues())
            {
                Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[구분].요청주소, 0);
            }
        }

        private void bForceAutoMode_Click(object sender, EventArgs e)
        {
            Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[장치통신.PLC커맨드목록.자동수동].요청주소, 1);
        }

        private void b커버센서읽기_Click(object sender, EventArgs e)
        {
            Dictionary<변위센서구분, Single> 자료1 = new Dictionary<변위센서구분, Single>();
            Dictionary<변위센서구분, Single> 자료2 = new Dictionary<변위센서구분, Single>();
            Dictionary<변위센서구분, Single> 전체센서자료 = new Dictionary<변위센서구분, Single>();

            Global.변위센서제어.Read(센서구분.REAR1, out 자료1);
            foreach (var s in 자료1)
            {
                전체센서자료[s.Key] = s.Value;
             
            }

            Global.변위센서제어.Read(센서구분.REAR2, out 자료2);
            foreach (var s in 자료2)
            {
                전체센서자료[s.Key] = s.Value;
            }

            var Sort전체센서자료 = 전체센서자료.OrderBy(s => s.Key).ToList();

            //Single averageA = ((Single)Sort전체센서자료[0].Value + (Single)Sort전체센서자료[1].Value + (Single)Sort전체센서자료[2].Value + (Single)Sort전체센서자료[3].Value) / 4;
            //Debug.WriteLine($"averageA : {averageA}");
            //foreach (var s in Sort전체센서자료)
            //{
            //    if((Int32)s.Key > 15)
            //    {
            //        //16 = 4
            //        전체센서자료[s.Key] = s.Value - averageA;
            //    }
                
            //}

            Sort전체센서자료 = 전체센서자료.OrderBy(s => s.Key).ToList();
            foreach (var s in Sort전체센서자료)
            {
                Debug.WriteLine($"item{(Int32)s.Key}: {s.Key} value: {s.Value}");
            }
            //foreach (var s in 자료2)
            //{
            //    Debug.WriteLine($"item{(Int32)s.Key}: {s.Key} value: {s.Value}");
            //}


            Single[,] 기준위치 = {
                    {  90,  200, (Single)Sort전체센서자료[0].Value },
                    { -90,  200, (Single)Sort전체센서자료[1].Value },
                    {  90, -230, (Single)Sort전체센서자료[2].Value },
                    { -90, -230, (Single)Sort전체센서자료[3].Value },
                };

            Single[,] 커버들뜸위치 = { // 커버상m1, 커버상m2, 커버상m3
                    { 0,   40,  Sort전체센서자료[12].Value },
                    { 0,  -60,  Sort전체센서자료[13].Value },
                    { 0, -125,  Sort전체센서자료[14].Value},
                };

            Single[,] 커버윤곽위치 = {
                    {  26.7f,   74.68f, Sort전체센서자료[4].Value},
                    {  26.7f,  -12.62f, Sort전체센서자료[5].Value },
                    {  26.7f,  -85.92f, Sort전체센서자료[6].Value},
                    {  26.7f, -175.42f,Sort전체센서자료[7].Value },
                    { -26.7f, -175.42f,Sort전체센서자료[8].Value  },
                    { -26.7f,  -36.52f, Sort전체센서자료[9].Value},
                    { -26.7f,   33.38f, Sort전체센서자료[10].Value},
                    { -26.7f,   85.48f, Sort전체센서자료[11].Value},
                };


            Single[] 커버들뜸편차 = PlaneDistanceCalculator.CalculateDistances(3, 기준위치, 커버들뜸위치);
            Single 커버들뜸높이 = PlaneDistanceCalculator.FindAbsMaxDiff(커버들뜸편차);


            Single[] 커버윤곽편차 = PlaneDistanceCalculator.CalculateDistances(8, 기준위치, 커버윤곽위치);
            Single 커버윤곽높이 = PlaneDistanceCalculator.FindAbsMaxDiff(커버윤곽편차);

            Debug.WriteLine($"커버들뜸 : {커버들뜸높이}");
            Debug.WriteLine($"커버윤곽 : {커버윤곽높이}");

        }
    }
}