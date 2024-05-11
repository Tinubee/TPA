using Cognex.VisionPro;
using DevExpress.XtraEditors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MvUtils;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 제품인덱스 = System.Int32;

namespace TPA.Schemas
{
    public class 검사자료 : BindingList<검사결과>
    {
        [JsonIgnore]
        public static TranslationAttribute 로그영역 = new TranslationAttribute("Inspection", "검사내역");

        public delegate void 검사진행알림(검사결과 결과);
        public event 검사진행알림 검사완료알림;
        private Dictionary<제품인덱스, 검사결과> 검사스플 = new Dictionary<제품인덱스, 검사결과>();
        public 검사결과 수동검사;
        private 검사결과테이블 테이블 = null;
        private TranslationAttribute 저장오류 = new TranslationAttribute("An error occurred while saving the data.", "자료 저장중 오류가 발생하였습니다.");
        public String 문서저장 { get; set; } = @"C:\IVM\VDA590TPA\SaveData";

        public void Init()
        {
            this.AllowEdit = true;
            this.AllowRemove = true;
            this.테이블 = new 검사결과테이블();
            this.Load();
            this.수동검사초기화();
            Global.환경설정.모델변경알림 += 모델변경알림;
        }

        public Boolean Close()
        {
            if (this.테이블 == null) return true;
            this.테이블.Save();
            this.테이블.자료정리(Global.환경설정.결과보관);
            return this.SaveJson();
        }

        private void 수동검사초기화()
        {
            this.수동검사 = new 검사결과();
            this.수동검사.검사코드 = 9999;
            this.수동검사.Reset();
        }

        private String 저장파일(DateTime 날짜) => Path.Combine(Global.환경설정.문서저장경로, Utils.FormatDate(날짜, "{0:yyyyMMdd}") + ".json");

        public void Save() => this.테이블.Save();

        public void SaveAsync() => this.테이블.SaveAsync();
        private Boolean SaveJson()
        {
            DateTime 날짜 = DateTime.Today;
            try {
                List<검사결과> 자료 = this.테이블.Select(날짜, 날짜);
                if (자료.Count < 1) return true;
                File.WriteAllText(this.저장파일(날짜), JsonConvert.SerializeObject(자료, Formatting.Indented));
                return true;
            }
            catch (Exception ex) {
                Global.오류로그(로그영역.GetString(), Localization.저장.GetString(), $"{저장오류.GetString()}\r\n{ex.Message}", true);
            }
            return false;
        }

        public void Load() => this.Load(DateTime.Today, DateTime.Today);
    
        public void Load(DateTime 시작, DateTime 종료)
        {
            this.Clear();
            this.RaiseListChangedEvents = false;
            List<검사결과> 자료 = this.테이블.Select(시작, 종료);

            List<Int32> 대상 = Global.장치통신.검사중인항목();
            자료.ForEach(검사 => {
                this.Add(검사);
                if (검사.측정결과 < 결과구분.ER && 대상.Contains(검사.검사코드) && !this.검사스플.ContainsKey(검사.검사코드))
                    this.검사스플.Add(검사.검사코드, 검사);
            });
            this.RaiseListChangedEvents = true;
            this.ResetBindings();
        }

        public List<검사결과> GetData(DateTime 시작, DateTime 종료, 모델구분 모델) => this.테이블.Select(시작, 종료, 모델);

        private void 모델변경알림(모델구분 모델코드) => this.수동검사초기화();

        private void 자료추가(검사결과 결과)
        {
            this.Insert(0, 결과);
            if (Global.장치통신.자동수동여부)
                this.테이블.Add(결과);
            // 저장은 State 에서
        }

        public 검사결과 현재검사찾기()
        {
            if (!Global.장치상태.자동수동) return this.수동검사;
            if (this.검사스플.Count < 1) return this.수동검사;
            return this.검사스플.Last().Value;
        }

        public Boolean 결과삭제(검사결과 정보)
        {
            this.Remove(정보);
            return this.테이블.Delete(정보) > 0;
        }

        public Boolean 결과삭제(검사결과 결과, 검사정보 정보)
        {
            결과.검사내역.Remove(정보);
            return this.테이블.Delete(정보) > 0;
        }

        public 검사결과 검사시작(Int32 제품인덱스)
        {
            검사결과 검사 = 검사결과찾기(제품인덱스, true);
            if (Global.장치통신.자동수동여부) {
                if (검사 == null) {
                    검사 = new 검사결과() { 검사코드 = 제품인덱스 };
                    검사.Reset();
                    검사.측정결과 = 결과구분.IN; // 검사중으로 바꿈
                    this.자료추가(검사);
                    this.검사스플.Add(검사.검사코드, 검사);
                    Global.정보로그(로그영역.GetString(), $"검사시작", $"[{(Int32)Global.환경설정.선택모델} - {검사.검사코드}] 신규검사 시작.", false);
                }
            }
            else {
                검사.Reset();
                검사.측정결과 = 결과구분.IN; // 검사중으로 바꿈
            }
            return 검사;
        }

        public 검사결과 검사결과찾기(Int32 제품인덱스, Boolean 신규여부 = false)
        {
            if (!Global.장치통신.자동수동여부) return this.수동검사;
            검사결과 검사 = null;
            if (제품인덱스 > 0 && this.검사스플.ContainsKey(제품인덱스))
                검사 = this.검사스플[제품인덱스];
            if (검사 == null && !신규여부)
                Debug.WriteLine($"{제품인덱스} : 해당 검사가 없습니다.");
            return 검사;
        }

        public 검사결과 검사결과계산(Int32 제품인덱스)
        {
            검사결과 검사 = null;
            if (Global.장치통신.자동수동여부) {
                검사 = this.검사결과찾기(제품인덱스);
                if (검사 == null) {
                    Global.오류로그(로그영역.GetString(), "검사결과계산", $"{제품인덱스} : 해당 검사 없음", true);
                    return null;
                }
                검사.결과계산();
                Global.모델자료.수량추가(검사.모델구분, 검사.측정결과);
            }
            else {
                검사 = this.수동검사;
                검사.결과계산();
            }
            this.검사완료알림?.Invoke(검사);
            return 검사;
        }

        public 검사결과 검사완료(Int32 제품인덱스)
        {
            검사결과 검사 = null;
            if (Global.장치통신.자동수동여부) {
                검사 = this.검사결과찾기(제품인덱스);
                if (검사 == null) {
                    Global.오류로그(로그영역.GetString(), "검사완료", $"{제품인덱스} : 해당 검사 없음", true);
                    return null;
                }
                
                this.검사스플.Remove(제품인덱스);
            }
            else {
                검사 = this.수동검사;
            }
            return 검사;
        }

        public 검사결과 카메라검사(카메라구분 카메라, Dictionary<String, Object> results)
        {
            Int32 제품인덱스 = Global.제품검사수행.촬영위치별제품인덱스(카메라);
            검사결과 검사 = Global.검사자료.검사결과찾기(제품인덱스);
            if (검사 == null) return null;
            검사.SetResults(카메라, results);

            // // 하드코딩해보고 되면 정상코드로 기입
            // if (Global.비전검사[카메라].CogCaliperTool1.RunStatus.Result != CogToolResultConstants.Accept) {
            //     검사.SetResult(검사항목.가공부높이f1, (Single)Double.NaN);
            // }

            if (카메라 == 카메라구분.Cam03) {
                검사정보 H1 = 검사.GetItem(검사항목.No2_1_거리측정h1);
                검사정보 H2 = 검사.GetItem(검사항목.No2_2_거리측정h2);
                검사정보 H3 = 검사.GetItem(검사항목.No3_1_거리측정h3);
                검사정보 H4 = 검사.GetItem(검사항목.No3_2_거리측정h4);

                검사정보 J1 = 검사.GetItem(검사항목.No10_1_1_거리측정J1);
                검사정보 J2 = 검사.GetItem(검사항목.No10_1_2_거리측정J2);
                검사정보 J3 = 검사.GetItem(검사항목.No10_2_1_거리측정J3);
                검사정보 J4 = 검사.GetItem(검사항목.No10_2_2_거리측정J4);

                검사정보 F1 = 검사.GetItem(검사항목.No8_1_거리측정L);
                검사정보 F2 = 검사.GetItem(검사항목.No8_2_거리측정R);
                검사.SetResult(검사항목.No2_선윤곽도H_F, (Single)(Math.Max(Math.Abs(H1.기준값 - H1.결과값), Math.Abs(H2.기준값 - H2.결과값)) * 2));
                검사.SetResult(검사항목.No3_선윤곽도H_R, (Single)(Math.Max(Math.Abs(H3.기준값 - H3.결과값), Math.Abs(H4.기준값 - H4.결과값)) * 2));
                검사.SetResult(검사항목.No10_1_선윤곽도J_F, (Single)(Math.Max(Math.Abs(J1.기준값 - J1.결과값), Math.Abs(J2.기준값 - J2.결과값)) * 2));
                검사.SetResult(검사항목.No10_2_선윤곽도J_R, (Single)(Math.Max(Math.Abs(J3.기준값 - J3.결과값), Math.Abs(J4.기준값 - J4.결과값)) * 2));
                검사.SetResult(검사항목.No8_선윤곽도_F, (Single)(Math.Max(Math.Abs(F1.기준값 - F1.결과값), Math.Abs(F2.기준값 - F2.결과값)) * 2));

                검사정보 각인정보 = 검사.GetItem(검사항목.노멀미러);
                if ((Single)각인정보.결과값 == 0)
                    검사.노멀미러 = 노멀미러.None;
                else if ((Single)각인정보.결과값 == 1)
                    검사.노멀미러 = 노멀미러.Normal;
                else if ((Single)각인정보.결과값 == 2)
                    검사.노멀미러 = 노멀미러.Mirror;
                검사정보 역투입정보 = 검사.GetItem(검사항목.역투입);
                if ((Single)역투입정보.결과값 == 0)
                    검사.역투입여부 = 역투입여부.None;
                else if ((Single)역투입정보.결과값 == 1)
                    검사.역투입여부 = 역투입여부.Normal;
                else if ((Single)역투입정보.결과값 == 2)
                    검사.역투입여부 = 역투입여부.Reverse;
            }
            else if (카메라 == 카메라구분.Cam06) {
                검사정보 CntL = 검사.GetItem(검사항목.커넥터설삽상부L);
                검사정보 CntR = 검사.GetItem(검사항목.커넥터설삽상부R);

                if ((Single)CntL.결과값 > (Single)CntL.최소값 && (Single)CntL.결과값 < (Single)CntL.최대값 && (Single)CntR.결과값 > (Single)CntR.최소값 && (Single)CntR.결과값 < (Single)CntR.최대값) {
                    검사.커넥터설삽상부 = 커넥터설삽여부.OK;
                }
                else {
                    검사.커넥터설삽상부 = 커넥터설삽여부.NG;
                }
                검사.SetResult(검사항목.커넥터설삽상부, (Single)검사.커넥터설삽상부);
            }
            else if (카메라 == 카메라구분.Cam07) {
                검사정보 CntL = 검사.GetItem(검사항목.커넥터설삽하부L);
                검사정보 CntR = 검사.GetItem(검사항목.커넥터설삽하부R);

                if ((Single)CntL.결과값 > (Single)CntL.최소값 && (Single)CntL.결과값 < (Single)CntL.최대값 && (Single)CntR.결과값 > (Single)CntR.최소값 && (Single)CntR.결과값 < (Single)CntR.최대값)
                    검사.커넥터설삽하부 = 커넥터설삽여부.OK;
                else
                    검사.커넥터설삽하부 = 커넥터설삽여부.NG;
                검사.SetResult(검사항목.커넥터설삽하부, (Single)검사.커넥터설삽하부);
            }

            return 검사;
        }
        public Dictionary<Int32, Int32> 큐알중복횟수(String 큐알코드, Int32[] indexs) => this.테이블.큐알중복횟수(큐알코드, indexs);
    }

    public class 검사결과테이블 : Data.BaseTable
    {
        private TranslationAttribute 로그영역 = new TranslationAttribute("Inspection Data", "검사자료");
        private TranslationAttribute 삭제오류 = new TranslationAttribute("An error occurred while deleting data.", "자료 삭제중 오류가 발생하였습니다.");
        private DbSet<검사결과> 검사결과 { get; set; }
        private DbSet<검사정보> 검사정보 { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<검사결과>().Property(e => e.모델구분).HasConversion(new EnumToNumberConverter<모델구분, Int32>());
            modelBuilder.Entity<검사결과>().Property(e => e.역투입여부).HasConversion(new EnumToNumberConverter<역투입여부, Int32>());
            modelBuilder.Entity<검사결과>().Property(e => e.측정결과).HasConversion(new EnumToNumberConverter<결과구분, Int32>());
            modelBuilder.Entity<검사결과>().Property(e => e.CTQ결과).HasConversion(new EnumToNumberConverter<결과구분, Int32>());
            modelBuilder.Entity<검사결과>().Property(e => e.외관결과).HasConversion(new EnumToNumberConverter<결과구분, Int32>());
            modelBuilder.Entity<검사결과>().Property(e => e.노멀미러).HasConversion(new EnumToNumberConverter<노멀미러, Int32>());
            modelBuilder.Entity<검사결과>().Property(e => e.커넥터설삽상부).HasConversion(new EnumToNumberConverter<커넥터설삽여부, Int32>());
            modelBuilder.Entity<검사결과>().Property(e => e.커넥터설삽하부).HasConversion(new EnumToNumberConverter<커넥터설삽여부, Int32>());
            modelBuilder.Entity<검사결과>().Property(e => e.재검사여부).HasConversion(new EnumToNumberConverter<재검사여부, Int32>());

            modelBuilder.Entity<검사정보>().HasKey(e => new { e.검사일시, e.검사항목 });
            modelBuilder.Entity<검사정보>().Property(e => e.검사그룹).HasConversion(new EnumToNumberConverter<검사그룹, Int32>());
            modelBuilder.Entity<검사정보>().Property(e => e.검사항목).HasConversion(new EnumToNumberConverter<검사항목, Int32>());
            modelBuilder.Entity<검사정보>().Property(e => e.검사장치).HasConversion(new EnumToNumberConverter<장치구분, Int32>());
            modelBuilder.Entity<검사정보>().Property(e => e.결과분류).HasConversion(new EnumToNumberConverter<결과분류, Int32>());
            modelBuilder.Entity<검사정보>().Property(e => e.측정단위).HasConversion(new EnumToNumberConverter<단위구분, Int32>());
            modelBuilder.Entity<검사정보>().Property(e => e.측정결과).HasConversion(new EnumToNumberConverter<결과구분, Int32>());
            
            base.OnModelCreating(modelBuilder);
        }

        public void Save()
        {
            try { this.SaveChanges(); }
            //try { this.SaveChangesAsync(); }
            catch (Exception ex) { Debug.WriteLine(ex.ToString(), "자료저장"); }
        }

        public void SaveAsync()
        {
            try { this.SaveChangesAsync(); }
            catch (Exception ex) { Debug.WriteLine(ex.ToString(), "자료저장"); }
        }

        public void Add(검사결과 정보)
        {
            this.검사결과.Add(정보);
            this.검사정보.AddRange(정보.검사내역);
        }

        public void Remove(List<검사정보> 자료)
        {
            this.검사정보.RemoveRange(자료);
        }

        public List<검사결과> Select()
        {
            return this.Select(DateTime.Today);
        }
        public List<검사결과> Select(DateTime 날짜)
        {
            DateTime 시작 = new DateTime(날짜.Year, 날짜.Month, 날짜.Day);
            return this.Select(시작, 시작);
        }
        public List<검사결과> Select(DateTime 시작, DateTime 종료, 모델구분 모델 = 모델구분.None, Int32 코드 = 0)
        {
            IQueryable<검사결과> query1 = (
                from l in 검사결과
                where l.검사일시 >= 시작 && l.검사일시 < 종료.AddDays(1)
                where (코드 <= 0 || l.검사코드 == 코드)
                where (모델 == 모델구분.None || l.모델구분 == 모델)
                orderby l.검사일시 descending
                select l);
            List<검사결과> 자료 = query1.AsNoTracking().ToList();

            IQueryable<검사정보> query2 = (
                from d in 검사정보
                join l in 검사결과 on d.검사일시 equals l.검사일시
                where l.검사일시 >= 시작 && l.검사일시 < 종료.AddDays(1)
                where (코드 <= 0 || l.검사코드 == 코드)
                where (모델 == 모델구분.None || l.모델구분 == 모델)
                orderby d.검사일시 descending
                orderby d.검사항목 ascending
                select d);
            List<검사정보> 정보 = query2.AsNoTracking().ToList();

            자료.ForEach(l => {
                l.AddRange(정보.Where(d => d.검사일시 == l.검사일시).ToList());
            });

            return 자료;
        }

        public 검사결과 Select(DateTime 일자, 모델구분 모델, Int32 코드)
        {
            return this.Select(일자, 일자, 모델, 코드).FirstOrDefault();
        }

        public Int32 Delete(검사결과 정보)
        {
            String Sql = $"DELETE FROM inspd WHERE idwdt = @idwdt;\nDELETE FROM inspl WHERE ilwdt = @ilwdt;";
            try {
                int AffectedRows = 0;
                using (NpgsqlCommand cmd = new NpgsqlCommand(Sql, this.DbConn)) {
                    cmd.Parameters.Add(new NpgsqlParameter("@idwdt", 정보.검사일시));
                    cmd.Parameters.Add(new NpgsqlParameter("@ilwdt", 정보.검사일시));
                    if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();
                    AffectedRows = cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                }
                return AffectedRows;
            }
            catch (Exception ex) {
                Global.오류로그(로그영역.GetString(), Localization.삭제.GetString(), $"{삭제오류.GetString()}\r\n{ex.Message}", true);
            }
            return 0;
        }

        public Int32 Delete(검사정보 정보)
        {
            String Sql = $"DELETE FROM inspd WHERE idwdt = @idwdt AND idnum = @idnum";
            try {
                int AffectedRows = 0;
                using (NpgsqlCommand cmd = new NpgsqlCommand(Sql, this.DbConn))
                {
                    cmd.Parameters.Add(new NpgsqlParameter("@idwdt", 정보.검사일시));
                    cmd.Parameters.Add(new NpgsqlParameter("@idnum", 정보.검사항목));
                    if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();
                    AffectedRows = cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                }
                return AffectedRows;
            }
            catch (Exception ex) {
                Global.오류로그(로그영역.GetString(), Localization.삭제.GetString(), $"{삭제오류.GetString()}\r\n{ex.Message}", true);
            }
            return 0;
        }

        public Int32 자료정리(Int32 일수)
        {
            DateTime 일자 = DateTime.Today.AddDays(-일수);
            String day = Utils.FormatDate(일자, "{0:yyyy-MM-dd}");
            // String sql = $"DELETE FROM inspd WHERE idwdt < DATE('{day}');\nDELETE FROM inspl WHERE ilwdt < DATE('{day}');DELETE FROM inserial WHERE isday < CURRENT_DATE;";
            String sql = $"DELETE FROM inspd WHERE idwdt < DATE('{day}');\nDELETE FROM inspl WHERE ilwdt < DATE('{day}');";
            try {
                int AffectedRows = 0;
                using (NpgsqlCommand cmd = new NpgsqlCommand(sql, this.DbConn))
                {
                    if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();
                    AffectedRows = cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                }
                return AffectedRows;
            }
            catch (Exception ex) {
                Debug.WriteLine(ex.Message);
                Global.오류로그(로그영역.GetString(), "Remove Datas", ex.Message, false);
            }
            return -1;
        }

        public Dictionary<Int32, Int32> 큐알중복횟수(String qrcode, Int32[] indexs)
        {
            Dictionary<Int32, Int32> result = new Dictionary<Int32, Int32>();
            if (!Global.큐알검증.중복체크 || indexs.Length < 1) return result;
            DateTime 시작 = DateTime.Today.AddDays(-100);// (-Global.큐알검증.중복일수);
            String Sql = $"SELECT * FROM qr_duplicated(@code, ARRAY[{String.Join(",", indexs)}]::integer[], @sday);";
            Debug.WriteLine(Sql, "중복쿼리");
            try {
                DateTime sday = new DateTime(시작.Year, 시작.Month, 시작.Day);
                using (NpgsqlCommand cmd = new NpgsqlCommand(Sql, this.DbConn))
                {
                    cmd.Parameters.Add(new NpgsqlParameter("@sday", sday));
                    cmd.Parameters.Add(new NpgsqlParameter("@code", qrcode));
                    if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();
                    using (NpgsqlDataReader reader = cmd.ExecuteReader()) {
                        while (reader.Read()) {
                            if (!result.ContainsKey(reader.GetInt32(0)))
                                result.Add(reader.GetInt32(0), reader.GetInt32(1));
                            else
                                Global.오류로그(로그영역.GetString(), "중복검사", $"해당 키는 이미 존재합니다. key : {reader.GetInt32(0)}", false);
                        }
                    }
                    cmd.Connection.Close();
                }
            }
            catch (Exception ex) {
                Global.오류로그(로그영역.GetString(), "중복검사", $"{ex.Message}", true);
            }
            return result;
        }
    }
}
