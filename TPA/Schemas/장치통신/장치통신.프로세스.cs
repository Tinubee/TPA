using MvUtils;
using ActUtlType64Lib;
using DevExpress.XtraEditors.Popup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using 제품인덱스 = System.Int32;
using 커맨드값 = System.Int32;
using System.Diagnostics;
using OpenCvSharp.Flann;
using System.Runtime.Remoting.Messaging;

namespace TPA.Schemas
{
    partial class 장치통신
    {
        public Boolean 센서읽기 = false;

        private Boolean PLC커맨드정보갱신()
        {
            DateTime 현재 = DateTime.Now;
            Int32 오류 = 0;
            Int32[] 자료 = ReadDeviceRandom(PLC커맨드.PLC커맨드주소목록, out 오류);
            if (오류 != 0)
            {
                통신오류알림(오류);
                return false;
            }
            this.PLC커맨드.Set(자료);

            return true;
        }

        private void PLC커맨드정보초기화()
        {
        }

        private void 검사위치별제품인덱스버퍼초기화()
        {
            this.검사위치별제품인덱스버퍼.Clear();

            this.검사위치별제품인덱스버퍼.Add(PLC커맨드목록.셔틀01제품인덱스, 0);
            this.검사위치별제품인덱스버퍼.Add(PLC커맨드목록.셔틀02제품인덱스, 0);
            this.검사위치별제품인덱스버퍼.Add(PLC커맨드목록.셔틀03제품인덱스, 0);
            this.검사위치별제품인덱스버퍼.Add(PLC커맨드목록.셔틀04제품인덱스, 0);
            this.검사위치별제품인덱스버퍼.Add(PLC커맨드목록.셔틀05제품인덱스, 0);
            this.검사위치별제품인덱스버퍼.Add(PLC커맨드목록.셔틀06제품인덱스, 0);
            this.검사위치별제품인덱스버퍼.Add(PLC커맨드목록.셔틀07제품인덱스, 0);
            this.검사위치별제품인덱스버퍼.Add(PLC커맨드목록.셔틀08제품인덱스, 0);
            this.검사위치별제품인덱스버퍼.Add(PLC커맨드목록.셔틀09제품인덱스, 0);
            this.검사위치별제품인덱스버퍼.Add(PLC커맨드목록.셔틀10제품인덱스, 0);
        }

        private Boolean PLC커맨드자료분석()
        {
            if (Global.환경설정.동작구분 == 동작구분.LocalTest) return 로컬테스트수행();
            if (!PLC커맨드정보갱신()) return false;
            장치상태확인();
            제품검사수행();
            통신핑퐁수행();
            return true;
        }

        private DateTime 테스트위치확인 = DateTime.Now;
        private Boolean 로컬테스트수행()
        {
            통신핑퐁수행();
            if ((DateTime.Now - 테스트위치확인).TotalSeconds < 5) return true;
            테스트위치확인 = DateTime.Now;
            Random rnd = new Random();
            List<Int32> 목록 = new List<Int32>();
            foreach (PLC커맨드목록 주소 in typeof(PLC커맨드목록).GetEnumValues())
            {
                Int32 val = 0;
                if (주소 >= PLC커맨드목록.셔틀01제품인덱스 && 주소 <= PLC커맨드목록.셔틀10제품인덱스)
                    val = rnd.Next(0, 32);
                목록.Add(val);
            }
            PLC커맨드.Set(목록.ToArray());

            Dictionary<PLC커맨드목록, Int32> 변경 = this.PLC커맨드.Changes(PLC커맨드목록.셔틀01제품인덱스, PLC커맨드목록.셔틀10제품인덱스);
            if (변경.Count < 1) return true;
            return true;
        }

        private void 제품검사수행()
        {
            Dictionary<PLC커맨드목록, Int32> 트리거변경 = this.PLC커맨드.Changes(PLC커맨드목록.하부큐알트리거, PLC커맨드목록.결과요청트리거);
            Dictionary<PLC커맨드목록, Int32> 제품인덱스변경 = this.PLC커맨드.Changes(PLC커맨드목록.셔틀01제품인덱스, PLC커맨드목록.셔틀10제품인덱스);

            foreach (PLC커맨드목록 구분 in typeof(PLC커맨드목록).GetEnumValues())
            {
                if (구분 >= PLC커맨드목록.셔틀01제품인덱스 && 구분 <= PLC커맨드목록.셔틀10제품인덱스)
                    this.검사위치별제품인덱스버퍼[구분] = 정보읽기(구분);
            }

            if (트리거변경.Count < 1) return;

            Debug.WriteLine($"하부큐알트리거=>{정보읽기(PLC커맨드목록.하부큐알트리거)}, 바닥평면트리거=>{정보읽기(PLC커맨드목록.바닥평면트리거)}, 측상스캔트리거=>{정보읽기(PLC커맨드목록.측상촬영트리거)}, 상부큐알트리거=>{정보읽기(PLC커맨드목록.상부큐알트리거)}, " +
                            $"하부스캔트리거=>{정보읽기(PLC커맨드목록.하부촬영트리거)}, 커넥터설삽트리거=>{정보읽기(PLC커맨드목록.커넥터촬영트리거)}, 커버조립여부요청트리거=>{정보읽기(PLC커맨드목록.커버조립트리거)}, 커버들뜸트리거=>{정보읽기(PLC커맨드목록.커버들뜸트리거)}, " +
                            $"라벨발행요청트리거=>{정보읽기(PLC커맨드목록.라벨발행트리거)}, 검사결과요구트리거=>{정보읽기(PLC커맨드목록.결과요청트리거)}" +
                            "  PLC Trigger index");

            Debug.WriteLine($"셔틀01제품인덱스=>{정보읽기(PLC커맨드목록.셔틀01제품인덱스)}, 셔틀02제품인덱스=>{정보읽기(PLC커맨드목록.셔틀02제품인덱스)}, 셔틀03제품인덱스=>{정보읽기(PLC커맨드목록.셔틀03제품인덱스)}, 셔틀04제품인덱스=>{정보읽기(PLC커맨드목록.셔틀04제품인덱스)}, " +
                            $"셔틀05제품인덱스=>{정보읽기(PLC커맨드목록.셔틀05제품인덱스)}, 셔틀06제품인덱스=>{정보읽기(PLC커맨드목록.셔틀06제품인덱스)}, 셔틀07제품인덱스=>{정보읽기(PLC커맨드목록.셔틀07제품인덱스)}, 셔틀08제품인덱스=>{정보읽기(PLC커맨드목록.셔틀08제품인덱스)}, " + 
                            $"셔틀09제품인덱스=>{정보읽기(PLC커맨드목록.셔틀09제품인덱스)}, 셔틀10제품인덱스=>{정보읽기(PLC커맨드목록.셔틀10제품인덱스)}" + "  PLC Product index \n");

            foreach (KeyValuePair<PLC커맨드목록, Int32> item in 트리거변경) {
                if (item.Value <= 0) continue;

                if (Global.장치통신.정보읽기(Global.장치통신.PLC커맨드[item.Key].요청주소) == 0) {
                    Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[item.Key].Busy주소, 0);
                    Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[item.Key].완료주소, 0);
                }
                if (Global.장치통신.정보읽기(Global.장치통신.PLC커맨드[item.Key].요청주소) == 1) {
                    // 20231201 PLC에서 값을 요청하는 주소들은 Busy/Complete이 아닌 NG/OK 개념이기에 아래 코드 진입하면 안됨
                    if (!(item.Key == PLC커맨드목록.커버조립트리거 || item.Key == PLC커맨드목록.라벨발행트리거 || item.Key == PLC커맨드목록.결과요청트리거))
                    {
                        Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[item.Key].Busy주소, 1);
                        Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[item.Key].완료주소, 0);
                    }
                }
                Global.제품검사수행.전체테스트수행(item.Key, GetIndex(item.Key));
            }
        }

        private Int32 GetIndex(PLC커맨드목록 커맨드)
        {
            if (커맨드 == PLC커맨드목록.하부큐알트리거) // 하부큐알2개
                return Global.장치통신.정보읽기(Global.장치통신.PLC커맨드[PLC커맨드목록.셔틀01제품인덱스].요청주소);
            else if (커맨드 == PLC커맨드목록.바닥평면트리거) // 바닥평면센서
                return Global.장치통신.정보읽기(Global.장치통신.PLC커맨드[PLC커맨드목록.셔틀02제품인덱스].요청주소);
            else if (커맨드 == PLC커맨드목록.측상촬영트리거) // 측상촬영
                return Global.장치통신.정보읽기(Global.장치통신.PLC커맨드[PLC커맨드목록.셔틀02제품인덱스].요청주소);
            else if (커맨드 == PLC커맨드목록.상부큐알트리거) // 상부큐알
                return Global.장치통신.정보읽기(Global.장치통신.PLC커맨드[PLC커맨드목록.셔틀03제품인덱스].요청주소);
            else if (커맨드 == PLC커맨드목록.하부촬영트리거) // 하부검사
                return Global.장치통신.정보읽기(Global.장치통신.PLC커맨드[PLC커맨드목록.셔틀03제품인덱스].요청주소);
            else if (커맨드 == PLC커맨드목록.커넥터촬영트리거) // 커넥터설삽
                return Global.장치통신.정보읽기(Global.장치통신.PLC커맨드[PLC커맨드목록.셔틀04제품인덱스].요청주소);
            else if (커맨드 == PLC커맨드목록.커버조립트리거) // 커버조립
                return Global.장치통신.정보읽기(Global.장치통신.PLC커맨드[PLC커맨드목록.셔틀05제품인덱스].요청주소);
            else if (커맨드 == PLC커맨드목록.커버들뜸트리거) // 커버들뜸
                return Global.장치통신.정보읽기(Global.장치통신.PLC커맨드[PLC커맨드목록.셔틀06제품인덱스].요청주소);
            else if (커맨드 == PLC커맨드목록.라벨발행트리거) // 라벨발행
                return Global.장치통신.정보읽기(Global.장치통신.PLC커맨드[PLC커맨드목록.셔틀08제품인덱스].요청주소);
            else if (커맨드 == PLC커맨드목록.결과요청트리거) // 결과컨베이어
                return Global.장치통신.정보읽기(Global.장치통신.PLC커맨드[PLC커맨드목록.셔틀09제품인덱스].요청주소);
            else {
                Debug.WriteLine($"[{커맨드.ToString()}] : Global.장치통신.프로세스의 GetIndex 함수에서 정의하지 않은 검사위치 정보 전달받음");
                return 0;
            }
        }

        private readonly object callbackLocker = new object();
        public List<Int32> 검사중인항목()
        {
            List<Int32> 대상 = new List<Int32>();
            Int32 시작 = (Int32)PLC커맨드목록.하부큐알트리거;
            Int32 종료 = (Int32)PLC커맨드목록.결과요청트리거;
            for (Int32 i = 종료; i >= 시작; i--) {
                PLC커맨드목록 구분 = (PLC커맨드목록)i;
                if (this.PLC커맨드[구분].데이터값 <= 0) continue;
                대상.Add(this.PLC커맨드[구분].데이터값);
            }
            return 대상;
        }

        private void 장치상태확인()
        {
            if (this.PLC커맨드.Changed(PLC커맨드목록.시작정지))
                Debug.WriteLine($"{Utils.FormatDate(DateTime.Now, "{0:HH:mm:ss.fff}")} => {this.PLC커맨드.Changed(PLC커맨드목록.시작정지)}", "시작정지");
            
            if (this.PLC커맨드.Changed(PLC커맨드목록.자동수동) || this.PLC커맨드.Changed(PLC커맨드목록.시작정지))
                this.동작상태알림?.Invoke();
        }

        private void 통신핑퐁수행()
        {
            if (!this.PLC커맨드[PLC커맨드목록.통신핑퐁].Passed()) return;
            if (this.시작일시.Day != DateTime.Today.Day) {
                this.시작일시 = DateTime.Now;
                // org this.검사번호리셋 = true;
                Global.모델자료.선택모델.날짜변경();
            }
            this.통신확인핑퐁 = !this.통신확인핑퐁;
            this.통신상태알림?.Invoke();
        }

        // public 제품인덱스 촬영위치별제품인덱스(카메라구분 촬영위치, Boolean testflag)
        // {
        //     if (!testflag) {
        //         if (촬영위치 == 카메라구분.Cam01 || 촬영위치 == 카메라구분.Cam02)
        //             return this.검사위치별제품인덱스버퍼[PLC커맨드목록.셔틀02제품인덱스];
        //         else if (촬영위치 == 카메라구분.Cam03 || 촬영위치 == 카메라구분.Cam08)
        //             return this.검사위치별제품인덱스버퍼[PLC커맨드목록.셔틀02제품인덱스];
        //         else if (촬영위치 == 카메라구분.Cam04 || 촬영위치 == 카메라구분.Cam05)
        //             return this.검사위치별제품인덱스버퍼[PLC커맨드목록.셔틀03제품인덱스];
        //         else if (촬영위치 == 카메라구분.Cam06 || 촬영위치 == 카메라구분.Cam07)
        //             return this.검사위치별제품인덱스버퍼[PLC커맨드목록.셔틀04제품인덱스];
        //         else
        //             return 0;
        //     }
        //     else {
        //         if (촬영위치 == 카메라구분.Cam01 || 촬영위치 == 카메라구분.Cam02)
        //             return this.검사위치별제품인덱스버퍼[PLC커맨드목록.셔틀02제품인덱스];
        //         else if (촬영위치 == 카메라구분.Cam03 || 촬영위치 == 카메라구분.Cam08)
        //             return this.검사위치별제품인덱스버퍼[PLC커맨드목록.셔틀03제품인덱스];
        //         else if (촬영위치 == 카메라구분.Cam04 || 촬영위치 == 카메라구분.Cam05)
        //             return this.검사위치별제품인덱스버퍼[PLC커맨드목록.셔틀03제품인덱스];
        //         else if (촬영위치 == 카메라구분.Cam06 || 촬영위치 == 카메라구분.Cam07)
        //             return this.검사위치별제품인덱스버퍼[PLC커맨드목록.셔틀04제품인덱스];
        //         else
        //             return 0;
        //     }
        // }
    }
}
