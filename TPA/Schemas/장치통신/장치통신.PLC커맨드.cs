using ActUtlType64Lib;
using Newtonsoft.Json;
using MvUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 커맨드값 = System.Int32;
using System.Diagnostics;

namespace TPA.Schemas
{
    public partial class 장치통신
    {
        public enum PLC커맨드목록 : Int32
        {
            [CommAddress("W200")]
            통신핑퐁,
            [CommAddress("W100")]
            자동수동,
            [CommAddress("W101")]
            시작정지,
            [CommAddress("W102")]
            재검사,

            //[CommAddress("W12C", "W22C", "W22D")]
            //진행여부트리거,
            [CommAddress("W110", "W210", "W211")]
            하부큐알트리거,
            [CommAddress("W112", "W212", "W213")]
            바닥평면트리거,
            [CommAddress("W114", "W214", "W215")]
            측상촬영트리거,
            [CommAddress("W116", "W216", "W217")]
            상부큐알트리거,
            [CommAddress("W118", "W218", "W219")]
            하부촬영트리거,
            [CommAddress("W12A", "W22A", "W22B")]
            커넥터촬영트리거,
            [CommAddress("W120", "W220", "W221")]
            커버조립트리거,
            [CommAddress("W122", "W222", "W223")]
            커버들뜸트리거,
            [CommAddress("W126", "W226", "W227")]
            라벨발행트리거,
            [CommAddress("W128", "W228", "W229")]
            결과요청트리거,

            [CommAddress("W310")]
            셔틀01제품인덱스,
            [CommAddress("W311")]
            셔틀02제품인덱스,
            [CommAddress("W312")]
            셔틀03제품인덱스,
            [CommAddress("W313")]
            셔틀04제품인덱스,
            [CommAddress("W314")]
            셔틀05제품인덱스,
            [CommAddress("W315")]
            셔틀06제품인덱스,
            [CommAddress("W316")]
            셔틀07제품인덱스,
            [CommAddress("W317")]
            셔틀08제품인덱스,
            [CommAddress("W318")]
            셔틀09제품인덱스,
            [CommAddress("W319")]
            셔틀10제품인덱스,

            //[CommAddress("W400")]
            //하부큐알정보F_1,
            //[CommAddress("W401")]
            //하부큐알정보F_2,
            //[CommAddress("W402")]
            //하부큐알정보F_3,
            //[CommAddress("W403")]
            //하부큐알정보F_4,
            //[CommAddress("W404")]
            //하부큐알정보F_5,
            //[CommAddress("W405")]
            //하부큐알정보F_6,
            //[CommAddress("W406")]
            //하부큐알정보F_7,
            //[CommAddress("W407")]
            //하부큐알정보F_8,
            //[CommAddress("W408")]
            //하부큐알정보R_1,
            //[CommAddress("W409")]
            //하부큐알정보R_2,
            //[CommAddress("W40A")]
            //하부큐알정보R_3,
            //[CommAddress("W40B")]
            //하부큐알정보R_4,
            //[CommAddress("W40C")]
            //하부큐알정보R_5,
            //[CommAddress("W40D")]
            //하부큐알정보R_6,
            //[CommAddress("W40E")]
            //하부큐알정보R_7,
            //[CommAddress("W40F")]
            //하부큐알정보R_8,

            // [CommAddress("W100", "W200", "W300", 200)]
            // 양품결과,
            // [CommAddress("W101", "W201", "W301", 200)]
            // 불량결과,

            // [CommAddress("B107", "B207", "B307")]
            // 결과요청,
            // [CommAddress("B108", "B208", "B308")]
            // 번호리셋,
            // [CommAddress("B109", "B209", "B309")]
            // 전체불량,
            // [CommAddress("B110", "B210", "B310")]
            // 양품여부,
            // [CommAddress("B111", "B211", "B311")]
            // 불량여부,
            // [CommAddress("B114", "B214", "B314")]
            // 피씨알람,
            // [CommAddress("B115", "B215", "B315")]
            // 비상정지,
        }

        private Boolean 신호읽기(PLC커맨드목록 커맨드) { return ToBool(this.PLC커맨드.Get(커맨드)); }
        private 커맨드값 정보읽기(PLC커맨드목록 커맨드) { return this.PLC커맨드.Get(커맨드); }
        private void 정보쓰기(PLC커맨드목록 커맨드, Int32 값) { this.PLC커맨드.Set(커맨드, 값); }
        private void 정보쓰기(PLC커맨드목록 커맨드, Boolean 값) { this.PLC커맨드.Set(커맨드, ToInt(값)); }

        #region PLC커맨드신호
        public Boolean 자동수동여부 { get { return 신호읽기(PLC커맨드목록.자동수동); } set { 정보쓰기(PLC커맨드목록.자동수동, value); } }
        public Boolean 시작정지여부 { get { return 신호읽기(PLC커맨드목록.시작정지); } set { 정보쓰기(PLC커맨드목록.시작정지, value); } }
        public Boolean 재검사여부 { get { return 신호읽기(PLC커맨드목록.재검사); } set { 정보쓰기(PLC커맨드목록.재검사, value); } }
        #endregion

        #region 검사현황
        // org public Boolean 검사번호리셋 { get { return 신호읽기(PLC커맨드목록.번호리셋); } set { 정보쓰기(PLC커맨드목록.번호리셋, value); } }
        // org public Boolean 전체불량요청 { get { return 신호읽기(PLC커맨드목록.전체불량); } set { 정보쓰기(PLC커맨드목록.전체불량, value); } }
        // org public Boolean 양품여부요청 { get { return 신호읽기(PLC커맨드목록.양품여부); } set { 정보쓰기(PLC커맨드목록.양품여부, value); } }
        // org public Boolean 불량여부요청 { get { return 신호읽기(PLC커맨드목록.불량여부); } set { 정보쓰기(PLC커맨드목록.불량여부, value); } }
        public Boolean 통신확인핑퐁 { get { return 신호읽기(PLC커맨드목록.통신핑퐁); } set { 정보쓰기(PLC커맨드목록.통신핑퐁, value); } }
        // org public Int32 양품결과쓰기 { get { return this.PLC커맨드.Get(PLC커맨드목록.양품결과); } set { this.PLC커맨드.Set(PLC커맨드목록.양품결과, value); } }
        // org public Int32 불량결과쓰기 { get { return this.PLC커맨드.Get(PLC커맨드목록.불량결과); } set { this.PLC커맨드.Set(PLC커맨드목록.불량결과, value); } }
        // org public Int32 당일시쓰기 { get { return this.PLC커맨드.Get(PLC커맨드목록.당일시); } set { this.PLC커맨드.Set(PLC커맨드목록.당일시, value); } }
        // org public Int32 당일분쓰기 { get { return this.PLC커맨드.Get(PLC커맨드목록.당일분); } set { this.PLC커맨드.Set(PLC커맨드목록.당일분, value); } }
        // org public Int32 당일초쓰기 { get { return this.PLC커맨드.Get(PLC커맨드목록.당일초); } set { this.PLC커맨드.Set(PLC커맨드목록.당일초, value); } }
        // org public Int32 결과쓰기 { get { return this.PLC커맨드.Get(PLC커맨드목록.요구); } set { this.PLC커맨드.Set(PLC커맨드목록.요구, value); } }
        // org public Int32 결과쓰기 { get { return this.PLC커맨드.Get(PLC커맨드목록.검사위치08); } set { this.PLC커맨드.Set(PLC커맨드목록.검사위치08, value); } }
        #endregion

        public class PLC커맨드정보
        {
            public PLC커맨드목록 PLC커맨드;
            public Int32 PLC커맨드정수형 = 0;
            public Int32 데이터값 = 0;
            public DateTime 시간 = DateTime.MinValue;
            public String 요청주소 = String.Empty;
            public String Busy주소 = String.Empty;
            public String 완료주소 = String.Empty;
            public Int32 지연 = 0;
            public Boolean 데이터값변경여부 = false;

            public PLC커맨드정보(PLC커맨드목록 PLC커맨드)
            {
                this.PLC커맨드 = PLC커맨드;
                this.PLC커맨드정수형 = (Int32)PLC커맨드;
                CommAddressAttribute a = Utils.GetAttribute<CommAddressAttribute>(PLC커맨드);
                this.요청주소 = a.cmdAddress;
                this.Busy주소 = a.busyAddress;
                this.완료주소 = a.cpltAddress;
                this.지연 = a.Delay;
            }

            public Boolean Passed()
            {
                if (this.지연 <= 0) return true;
                return (DateTime.Now - 시간).TotalMilliseconds >= this.지연;
            }

            public Boolean Set(Int32 val, Boolean force = false)
            {
                if (this.데이터값.Equals(val) || !force && !this.Passed()) {
                    this.데이터값변경여부 = false;
                    return false;
                }

                this.데이터값 = val;
                this.시간 = DateTime.Now;
                this.데이터값변경여부 = true;
                return true;
            }

            public Boolean Set(Int32 val, PLC커맨드목록 PLC커맨드, Boolean force = false)
            {
                if (this.데이터값.Equals(val) || !force && !this.Passed())
                {
                    this.데이터값변경여부 = false;
                    return false;
                }

                this.데이터값 = val;
                this.시간 = DateTime.Now;
                this.데이터값변경여부 = true;

                if (PLC커맨드 == PLC커맨드목록.통신핑퐁) return true;
                return true;
            }
        }
        public class PLC커맨드통신 : Dictionary<PLC커맨드목록, PLC커맨드정보>
        {
            private Action<PLC커맨드목록, Int32> Transmit;
            public String[] PLC커맨드주소목록;

            public PLC커맨드통신()
            {
                List<String> PLC커맨드실제주소들 = new List<String>();
                foreach (PLC커맨드목록 커맨드 in typeof(PLC커맨드목록).GetEnumValues()) {
                    PLC커맨드정보 커맨드정보 = new PLC커맨드정보(커맨드);
                    if (커맨드정보.PLC커맨드정수형 < 0) continue;
                    this.Add(커맨드, 커맨드정보);
                    PLC커맨드실제주소들.Add(커맨드정보.요청주소);
                }
                this.PLC커맨드주소목록 = PLC커맨드실제주소들.ToArray();
            }

            public void Init(Action<PLC커맨드목록, Int32> transmit) => this.Transmit = transmit;

            public String Address(PLC커맨드목록 PLC커맨드)
            {
                if (!this.ContainsKey(PLC커맨드)) return String.Empty;
                return this[PLC커맨드].요청주소;
            }

            public Int32 Get(PLC커맨드목록 PLC커맨드)
            {
                if (!this.ContainsKey(PLC커맨드)) return 0;
                return this[PLC커맨드].데이터값;
            }

            public void Set(Int32[] 자료)
            {
                foreach (PLC커맨드정보 PLC커맨드정보 in this.Values)
                {
                    Int32 PLC커맨드값 = 자료[PLC커맨드정보.PLC커맨드정수형];
                    PLC커맨드정보.Set(PLC커맨드값);
                }
            }

            public Boolean Set(PLC커맨드목록 PLC커맨드, Int32 PLC커맨드값, Int32 resetTime = 0, Int32 resetValue = 0)
            {
                if (!this[PLC커맨드].Set(PLC커맨드값, PLC커맨드, true)) return false;
                this.Transmit?.Invoke(PLC커맨드, PLC커맨드값);
                if (resetTime > 0) {
                    Task.Run(() => {
                        Task.Delay(resetTime).Wait();
                        this[PLC커맨드].Set(resetValue, true);
                        this.Transmit?.Invoke(PLC커맨드, resetValue);
                    });
                }
                return true;
            }

            public void SetDelay(PLC커맨드목록 PLC커맨드, Int32 PLC커맨드값, Int32 resetTime)
            {
                if (resetTime <= 0) {
                    if (!this[PLC커맨드].Set(PLC커맨드값, true)) return;
                    this.Transmit?.Invoke(PLC커맨드, PLC커맨드값);
                }
                Task.Run(() => {
                    Task.Delay(resetTime).Wait();
                    if (this[PLC커맨드].Set(PLC커맨드값, true))
                        this.Transmit?.Invoke(PLC커맨드, PLC커맨드값);
                });
            }

            public Boolean Changed(PLC커맨드목록 PLC커맨드) => this[PLC커맨드].데이터값변경여부;
            public Boolean Firing(PLC커맨드목록 PLC커맨드, Boolean 상태) => this[PLC커맨드].데이터값변경여부 && ToBool(this[PLC커맨드].데이터값) == 상태;
            public Dictionary<PLC커맨드목록, 커맨드값> Changes(PLC커맨드목록 시작, PLC커맨드목록 종료) => this.Changes((커맨드값)시작, (커맨드값)종료);
            public Dictionary<PLC커맨드목록, 커맨드값> Changes(커맨드값 시작, 커맨드값 종료)
            {
                Dictionary<PLC커맨드목록, 커맨드값> 변경 = new Dictionary<PLC커맨드목록, 커맨드값>();
                foreach (PLC커맨드목록 PLC커맨드 in typeof(PLC커맨드목록).GetEnumValues()) {
                    커맨드값 번호 = (커맨드값)PLC커맨드;
                    if (번호 < 시작 || 번호 > 종료 || !this[PLC커맨드].데이터값변경여부) continue;
                    변경.Add(PLC커맨드, this[PLC커맨드].데이터값);
                }
                return 변경;
            }
        }

        public void 강제쓰기(String address, Int32 value)
        {
            if (this.PLC == null) { return; }
            Int32 rvalue = this.PLC.ReadDeviceBlock(address, 1, out rvalue);
            this.PLC.WriteDeviceBlock(address, 1, ref value);
            this.PLC.ReadDeviceBlock(address, 1, out rvalue);
        }

        public Int32 정보읽기(String address)
        {
            Int32 value;
            this.PLC.ReadDeviceBlock(address, 1, out value);
            return value;
        }
    }
}
