using MvUtils;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TPA.Schemas
{
    public class 환경설정
    {
        public delegate void 모델변경(모델구분 모델코드);
        public event 모델변경 모델변경알림;

        [JsonIgnore]
        public const String 프로젝트번호 = "23-0034-004";
        [Description("프로그램 동작구분"), JsonProperty("RunType")]
        public 동작구분 동작구분 { get; set; } = 동작구분.Live;
        [JsonProperty("ConfigSavePath")]
        public String 기본경로 { get; set; } = @"C:\IVM\VDA590TPA\Config";
        [JsonProperty("DocumentSavePath")]
        public String 문서저장경로 { get; set; } = @"C:\IVM\VDA590TPA\SaveData";
        [JsonProperty("ImageSavePath")]
        public String 사진저장경로 { get; set; } = @"C:\IVM\VDA590TPA\SaveImage";
        [Description("비젼 Tools"), JsonIgnore]
        public String 비전도구경로 { get { return Path.Combine(기본경로, "Tools"); } }
        [Description("마스터 이미지"), JsonIgnore]
        public String 마스터사진경로 { get { return Path.Combine(기본경로, "Masters"); } }
        [JsonIgnore]
        public String 설정저장경로 { get { return Path.Combine(this.기본경로, "Config.json"); } }
        [JsonProperty("SaveOK")]
        public Boolean 사진저장OK { get; set; } = false;
        [JsonProperty("SaveNG")]
        public Boolean 사진저장NG { get; set; } = false;
        [JsonProperty("Decimals")]
        public Int32 결과자릿수 { get; set; } = 3;
        [JsonIgnore]
        public String Format { get { return "#,0." + String.Empty.PadLeft(this.결과자릿수, '0'); } }
        [JsonIgnore]
        public String 결과표현 { get { return "{0:" + Format + "}"; } }
        [JsonProperty("DaysToKeepResults")]
        public Int32 결과보관 { get; set; } = 900;
        [JsonProperty("DaysToKeepLogs")]
        public Int32 로그보관 { get; set; } = 60;
        [JsonIgnore, Description("이미지 저장 디스크 사용율")]
        public Int32 저장비율 { get { return 100 - this.SaveImageDriveFreeSpace(); } }
        [JsonIgnore, Description("사용자명")]
        public String 사용자명 { get; set; } = String.Empty;
        [JsonIgnore, Description("권한구분")]
        public 유저권한구분 사용권한 { get; set; } = 유저권한구분.없음;
        [JsonIgnore, Description("검사여부"), Translation("Use Inspect", "검사여부")]
        public Boolean 검사여부 { get; set; } = true;
        [JsonProperty("CurrentModel")]
        public 모델구분 선택모델 { get; set; } = 모델구분.VDA590TPA_Ohsung;
        [JsonProperty("TriggerBoardPort")]
        public String 트리거보드포트 { get; set; } = "COM6";
        [JsonProperty("CodeReader1Host")]
        public String 코드리더1주소 { get; set; } = "192.168.3.50";
        [JsonProperty("CodeReader1Port")]
        public Int32 코드리더1포트 { get; set; } = 9004;
        [JsonProperty("UseCodeReader1")]
        public Boolean 코드리더1사용여부 { get; set; }  = true;
        [JsonProperty("CodeReader2Host")]
        public String 코드리더2주소 { get; set; } = "192.168.3.51";
        [JsonProperty("CodeReader2Port")]
        public Int32 코드리더2포트 { get; set; } = 9004;
        [JsonProperty("UseCodeReader2")]
        public Boolean 코드리더2사용여부 { get; set; } = true;
        [JsonProperty("CodeReader3Host")]
        public String 코드리더3주소 { get; set; } = "192.168.3.52";
        [JsonProperty("CodeReader3Port")]
        public Int32 코드리더3포트 { get; set; } = 9004;
        [JsonProperty("UseCodeReader3")]
        public Boolean 코드리더3사용여부 { get; set; } = true;
        [JsonProperty("LabelPrinterHost")]
        public String 큐알인쇄주소 { get; set; } = "192.168.3.115";
        [JsonProperty("LabelPrinterPort")]
        public Int32 큐알인쇄포트 { get; set; } = 9100;
        [JsonProperty("UseLabelPrinter")]
        public Boolean 라벨기사용여부 { get; set; } = true;
        [JsonProperty("ForceOutputOK")]
        public Boolean 강제OK배출여부 { get; set; } = false;
        [JsonProperty("ForceOutputNG")]
        public Boolean 강제NG배출여부 { get; set; } = false;
        [JsonProperty("ForceCoverAssyOK")]
        public Boolean 강제커버조립O { get; set; } = false;
        [JsonProperty("ForceCoverAssyNG")]
        public Boolean 강제커버조립X { get; set; } = false;

        [JsonIgnore, Description("슈퍼유저")]
        public const String 시스템관리자 = "ivmadmin";

        public 유저권한구분 시스템관리자인증(string 사용자명, string 비밀번호)
        {
            if (사용자명 != 시스템관리자) return 유저권한구분.없음;
            String pass = $"{시스템관리자}";// {Utils.FormatDate(DateTime.Now, "{0:ddHH}")}";
            if (비밀번호 == pass) {
                this.시스템관리자로그인();
                return 유저권한구분.시스템;
            }
            return 유저권한구분.없음;
        }
        public void 시스템관리자로그인()
        {
            this.사용자명 = 시스템관리자;
            this.사용권한 = 유저권한구분.시스템;
        }
        public Boolean 권한여부(유저권한구분 요구권한) => (Int32)사용권한 >= (Int32)요구권한;
        [JsonIgnore]
        public Boolean 슈퍼유저 { get { return 사용권한 == 유저권한구분.시스템 && 사용자명 == 시스템관리자; } }

        [JsonIgnore]
        public static TranslationAttribute 로그영역 = new TranslationAttribute("Preferences", "환경설정");

        public Boolean Init() => this.Load();
        public void Close() => this.Save();

        public Boolean Load()
        {
            if (!CanDbConnect()) {
                Global.오류로그(로그영역.GetString(), "데이터베이스 연결실패", "데이터베이스에 연결할 수 없습니다.", true);
                return false;
            }
            Common.DirectoryExists(Path.Combine(Application.StartupPath, @"Views"), true);
            if (!Common.DirectoryExists(기본경로, true)) {
                Global.오류로그(로그영역.GetString(), "환경설정 초기화 실패", "기본설정 폴더를 생성할 수 없습니다.", true);
                return false;
            }
            if (!Common.DirectoryExists(사진저장경로, true))
                Global.오류로그(로그영역.GetString(), "환경설정 초기화 실패", "사진저장 폴더를 생성할 수 없습니다.", true);
            if (!Common.DirectoryExists(문서저장경로, true))
                Global.오류로그(로그영역.GetString(), "환경설정 초기화 실패", "문서저장 폴더를 생성할 수 없습니다.", true);
            if (!Common.DirectoryExists(비전도구경로, true))
                Global.오류로그(로그영역.GetString(), "환경설정 초기화 실패", "비전도구 폴더를 생성할 수 없습니다.", true);
            if (!Common.DirectoryExists(마스터사진경로, true))
                Global.오류로그(로그영역.GetString(), "환경설정 초기화 실패", "마스터사진 폴더를 생성할 수 없습니다.", true);

            if (File.Exists(설정저장경로)) {
                try {
                    환경설정 설정 = JsonConvert.DeserializeObject<환경설정>(File.ReadAllText(설정저장경로, Encoding.UTF8));
                    foreach (PropertyInfo p in 설정.GetType().GetProperties()) {
                        if (!p.CanWrite) continue;
                        Object v = p.GetValue(설정);
                        if (v == null) continue;
                        p.SetValue(this, v);
                    }
                }
                catch (Exception ex) {
                    Global.오류로그(로그영역.GetString(), "환경설정 초기화 실패", ex.Message, true);
                }
            }
            else {
                this.Save();
                Debug.WriteLine("환경설정 로드 실패 : 저장된 설정파일 없음");
            }
            Debug.WriteLine(this.동작구분, "동작구분");
            return true;
        }

        public void Save()
        {
            if (!Utils.WriteAllText(설정저장경로, JsonConvert.SerializeObject(this, Utils.JsonSetting())))
                Global.오류로그(로그영역.GetString(), "환경설정 저장", "환경설정 저장에 실패하였습니다.", true);
        }

        public void 모델변경요청(모델구분 모델구분)
        {
            if (this.선택모델 == 모델구분) return;
            this.선택모델 = 모델구분;
            this.모델변경알림?.Invoke(this.선택모델);
        }

        public static Color ResultColor(결과구분 구분)
        {
            if (구분 == 결과구분.IN) return DevExpress.LookAndFeel.DXSkinColors.ForeColors.DisabledText;
            if (구분 == 결과구분.ER) return DevExpress.LookAndFeel.DXSkinColors.ForeColors.Question;
            if (구분 == 결과구분.OK) return DevExpress.LookAndFeel.DXSkinColors.ForeColors.Information;
            if (구분 == 결과구분.NG) return DevExpress.LookAndFeel.DXSkinColors.ForeColors.Critical;
            return DevExpress.LookAndFeel.DXSkinColors.ForeColors.ControlText;
        }

        private DriveInfo ImageSaveDrive = null;
        private DriveInfo GetSaveImageDrive()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo drive in drives) {
                if (this.사진저장경로.StartsWith(drive.Name)) {
                    this.ImageSaveDrive = drive;
                    return this.ImageSaveDrive;
                }
            }
            if (drives.Length > 0)
                this.ImageSaveDrive = drives[0];
            return this.ImageSaveDrive;
        }

        public Int32 SaveImageDriveFreeSpace()
        {
            DriveInfo drive = this.GetSaveImageDrive();
            double FreeSpace = drive.AvailableFreeSpace / (double)drive.TotalSize * 100;
            return Convert.ToInt32(FreeSpace);
        }

        public static NpgsqlConnection CreateDbConnection()
        {
            // org NpgsqlConnectionStringBuilder b = new NpgsqlConnectionStringBuilder() { Host = "192.168.3.5", Port = 5432, Username = "postgres", Password = "ivmadmin", Database = "vda590tpa_ohsung" };
            NpgsqlConnectionStringBuilder b = new NpgsqlConnectionStringBuilder() { Host = "localhost", Port = 5432, Username = "postgres", Password = "ivmadmin", Database = "vda590tpa_ohsung" };
            return new NpgsqlConnection(b.ConnectionString);
        }

        public Boolean CanDbConnect()
        {
            Boolean can = false;
            try {
                NpgsqlConnection conn = CreateDbConnection();
                conn.Open();
                can = conn.ProcessID > 0;
                conn.Close();
                conn.Dispose();
            }
            catch (Exception ex) {
                Global.오류로그(로그영역.GetString(), "데이터베이스 연결실패", ex.Message, true);
            }
            return can;
        }
    }
}
