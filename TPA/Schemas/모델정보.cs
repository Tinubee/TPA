﻿using MvUtils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPA.Schemas
{
    public enum 모델구분
    {
        [ListBindable(false)]
        None,
        [DXDescription("VDA590 UFA"), Description("VDA590 UFA")]
        VDA590UFA = 1,
        [DXDescription("VDA590 TPA"), Description("VDA590 TPA")]
        VDA590TPA = 2,
    }
    public class 모델정보
    {
        [JsonProperty("type"), Translation("Model", "모델")]
        public 모델구분 모델구분 { get; set; } = 모델구분.None;
        [JsonProperty("desc"), Translation("Description", "설명")]
        public String 모델설명 { get; set; } = String.Empty;
        [JsonProperty("date"), Translation("Date", "일자")]
        public DateTime 양산일자 { get; set; } = DateTime.Today;
        [JsonProperty("OK"), Translation("OK", "양품")]
        public Int32 양품갯수 { get; set; } = 0;
        [JsonProperty("NG"), Translation("NG", "불량")]
        public Int32 불량갯수 { get; set; } = 0;
        [JsonIgnore, Translation("Total", "전체")]
        public Int32 전체갯수 { get { return 양품갯수 + 불량갯수; } }
        [JsonIgnore, Translation("Yield", "양품율")]
        public Double 양품수율 { get { return 전체갯수 > 0 ? (Double)양품갯수 / (Double)전체갯수 * (Double)100 : (Double)100; } }
        [JsonIgnore, Translation("OK", "양품")]
        public String 양품갯수표현 { get { return Utils.FormatNumeric(양품갯수, "{0:#,0}"); } }
        [JsonIgnore, Translation("NG", "불량")]
        public String 불량갯수표현 { get { return Utils.FormatNumeric(불량갯수, "{0:#,0}"); } }
        [JsonIgnore, Translation("Total", "전체")]
        public String 전체갯수표현 { get { return Utils.FormatNumeric(전체갯수, "{0:#,0}"); } }
        [JsonIgnore, Translation("Yield", "양품율")]
        public String 양품수율표현 { get { return Utils.FormatNumeric(양품수율, "{0:#,0}%"); } }
        [JsonIgnore]
        public Int32 모델번호 { get { return (Int32)this.모델구분; } }
        [JsonIgnore]
        public String 모델코드 { get { return Utils.GetDescription(this.모델구분); } }
        [JsonIgnore]
        public String 모델사진 { get { return Path.Combine(Global.환경설정.사진저장경로, 모델번호.ToString("d2") + ".png"); } }
        [JsonProperty("StartIndex")]
        public Int32 시작번호 { get; set; } = 0; // 투입 Index 번호
        [JsonProperty("EndIndex")]
        public Int32 종료번호 { get; set; } = 0; // 결과 Index 번호
        [JsonIgnore]
        public 검사설정자료 검사설정 = null;

        public 모델정보() { }
        public 모델정보(모델구분 구분)
        {
            this.모델구분 = 구분;
            this.모델설명 = GetModelDescription(구분);
            this.Init();
        }

        public void Init()
        {
            this.검사설정 = new 검사설정자료(this);
        }
        public void Close() { }

        public static String GetModelDescription(모델구분 구분)
        {
            DXDescriptionAttribute d = Utils.GetAttribute<DXDescriptionAttribute>(구분);
            if (d == null) return String.Empty;
            return d.Description;
        }

        public void 수량리셋()
        {
            this.양품갯수 = 0;
            this.불량갯수 = 0;
            this.양산일자 = DateTime.Today;
        }
        public void 수량추가(결과구분 구분)
        {
            if (구분 == 결과구분.PS) return;
            if (구분 == 결과구분.OK) this.양품갯수++;
            else this.불량갯수++;
        }
        public void 날짜변경()
        {
            this.수량리셋();
            this.시작번호 = 0;
            this.종료번호 = 0;
        }
        public void 검사시작(Int32 제품인덱스)
        {
            if (this.시작번호 >= 제품인덱스) return;
            this.시작번호 = 제품인덱스;
        }
        public void 검사종료(Int32 제품인덱스)
        {
            if (this.종료번호 >= 제품인덱스) return;
            this.종료번호 = 제품인덱스;
        }
    }

    public class 모델자료 : BindingList<모델정보>
    {
        public static TranslationAttribute 로그영역 = new TranslationAttribute("Models", "모델관리");
        private String 저장파일 { get { return Path.Combine(Global.환경설정.기본경로, $"Models.json"); } }
        public 모델정보 선택모델 { get { return this.GetItem(Global.환경설정.선택모델); } }
        public event Global.BaseEvent 검사수량변경;
        public void Init()
        {
            this.Load();
            this.BaseModel();
        }

        public void Close()
        {
            this.Save();
            foreach (모델정보 모델 in this)
                모델.Close();
        }

        private void Load()
        {
            if (!File.Exists(저장파일)) {
                Global.정보로그(로그영역.GetString(), "자료로드", "저장파일이 없습니다.", false);
                return;
            }
            try {
                List<모델정보> 자료 = JsonConvert.DeserializeObject<List<모델정보>>(File.ReadAllText(저장파일));
                if (자료 == null) return;
                자료.Sort((a, b) => a.모델번호.CompareTo(b.모델번호));
                자료.ForEach(e => this.Add(e));
            }
            catch (Exception ex) {
                Global.오류로그(로그영역.GetString(), "자료로드", ex.Message, false);
            }

            if (this.GetItem(Global.환경설정.선택모델) == null)
                if (this.Count > 0) Global.환경설정.선택모델 = this[0].모델구분;

            foreach (모델정보 정보 in this) {
                if (정보.양산일자 == DateTime.Today) continue;
                정보.양산일자 = DateTime.Today;
                정보.수량리셋();
            }
            SettingLoad();
        }

        public 모델정보 GetItem(모델구분 모델코드)
        {
            return this.Where(e => e.모델구분 == 모델코드).FirstOrDefault();
        }

        private void BaseModel()
        {
            foreach (모델구분 구분 in typeof(모델구분).GetEnumValues()) {
                if (구분 == 모델구분.None) continue;
                모델정보 모델 = this.GetItem(구분);
                if (모델 == null) this.Add(new 모델정보(구분));
            }
            if (this.선택모델 == null) Global.환경설정.선택모델 = 모델구분.None;
        }

        public void Save()
        {
            File.WriteAllText(저장파일, JsonConvert.SerializeObject(this, Formatting.Indented));
        }

        public void 수량리셋()
        {
            this.선택모델.수량리셋();
            this.검사수량변경?.Invoke();
        }

        public void 수량추가(모델구분 모델, 결과구분 결과)
        {
            this.GetItem(모델)?.수량추가(결과);
            this.검사수량변경?.Invoke();
        }

        public void SettingLoad()
        {
            foreach (모델정보 정보 in this)
                SettingLoad(정보);
        }

        public void SettingLoad(모델정보 정보)
        {
            if (정보 == null) return;
            if (정보.검사설정 == null) 정보.Init();
            정보.검사설정.Load();
        }

        public void SettingSave(모델정보 정보)
        {
            정보?.검사설정?.Save();
        }
    }

    public class 검사설정자료 : BindingList<검사정보>
    {
        public static TranslationAttribute 로그영역 = new TranslationAttribute("Inspection Settings", "검사설정");
        private 모델정보 모델정보;
        private 모델구분 모델구분 { get { return 모델정보.모델구분; } }
        private Int32 모델번호 { get { return 모델정보.모델번호; } }
        private String 저장파일 { get { return Path.Combine(Global.환경설정.기본경로, $"Model.{모델번호.ToString("d2")}.json"); } }
        public 검사설정자료(모델정보 모델) { this.모델정보 = 모델; }

        public void Init() { this.Load(); }

        public void Load()
        {
            this.Clear();

            if (!File.Exists(저장파일)) {
                Global.정보로그(로그영역.GetString(), "자료로드", $"[{Utils.GetDescription(모델구분)}] 검사설정 파일이 없습니다.", false);
                foreach (검사항목 항목 in typeof(검사항목).GetEnumValues()) {
                    if (항목 == 검사항목.None) continue;
                    ResultAttribute a = Utils.GetAttribute<ResultAttribute>(항목);
                    this.Add(new 검사정보() { 검사항목 = 항목, 검사그룹 = a.검사그룹, 검사장치 = a.장치구분, 결과분류 = a.결과분류 });
                }
                this.Save();
                return;
            }
            try {
                List<검사정보> 자료 = JsonConvert.DeserializeObject<List<검사정보>>(File.ReadAllText(저장파일));
                자료.Sort((a, b) => a.검사항목.CompareTo(b.검사항목));
                if (자료 == null) {
                    Global.정보로그(로그영역.GetString(), "자료로드", "저장된 설정자료가 올바르지 않습니다.", false);
                    return;
                }

                자료.ForEach(e => {
                    검사정보 정보 = new 검사정보(e);
                    if (정보 != null)
                    {
                        정보.Set(e);
                        this.Add(정보);
                    }
                });
            }
            catch (Exception ex) {
                Global.오류로그(로그영역.GetString(), "자료로드", ex.Message, false);
            }
        }
        public 검사정보 GetItem(검사항목 항목) => this.Where(e => e.검사항목 == 항목).FirstOrDefault();
        public Boolean Save()
        {
            try {
                if (File.Exists(저장파일)) {
                    String path = Path.Combine(Global.환경설정.기본경로, "backup");
                    if (Common.DirectoryExists(path, true))
                        File.Copy(저장파일, Path.Combine(path, $"검사설정.{모델번호.ToString("d2")}.{Utils.FormatDate(DateTime.Now, "{0:yymmddhhmmss}")}.json"));
                }

                File.WriteAllText(저장파일, JsonConvert.SerializeObject(this, Formatting.Indented));
                Debug.WriteLine(저장파일, "티칭저장");
                return true;
            }
            catch (Exception ex) {
                Global.오류로그(로그영역.GetString(), "자료저장", ex.Message, false);
                return false;
            }
        }
    }
}
