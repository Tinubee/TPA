using DevExpress.CodeParser.Xaml;
using DevExpress.Utils.Extensions;
using MvUtils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static TPA.Schemas.MES통신;
using static TPA.Schemas.장치통신;
using static TPA.Schemas.큐알제어;

namespace TPA.Schemas
{
    public class 제품검사수행
    {
        private string 로그영역 = "제품검사수행";
        private 재검사여부 재검사 = 재검사여부.FALSE;
        public Queue<Int32>[] 제품인덱스큐;

        public void Init()
        {
            Int32 카메라대수 = Enum.GetValues(typeof(카메라구분)).Length;
            제품인덱스큐 = new Queue<Int32>[카메라대수];
            for (Int32 구분 = 0; 구분 < 카메라대수; 구분++)
            {
                제품인덱스큐[구분] = new Queue<Int32>();
            }
        }

        public Int32 촬영위치별제품인덱스(카메라구분 촬영위치)
        {
            return this.제품인덱스큐[(Int32)촬영위치].Peek();
        }

        public void 전체테스트수행(PLC커맨드목록 커맨드, Int32 제품인덱스)
        {
            try
            {
                재검사여부확인(커맨드);
                하부큐알리딩수행(커맨드, 제품인덱스);
                바닥평면센서수행(커맨드, 제품인덱스);
                측면촬영수행(커맨드, 제품인덱스);
                상부촬영수행(커맨드, 제품인덱스);
                상부큐알리딩수행(커맨드, 제품인덱스);
                하부촬영수행(커맨드, 제품인덱스);
                커넥터설삽촬영수행(커맨드, 제품인덱스);
                커버조립여부요청수행(커맨드, 제품인덱스);
                커버들뜸수행(커맨드, 제품인덱스);
                //라벨부착수행(커맨드, 제품인덱스);
                검사결과전송수행(커맨드, 제품인덱스);
            }
            catch (Exception ee)
            {
                Debug.WriteLine($"--------------------------------전체검사수행 에러 : {ee.Message}--------------------------------");
            }

        }

        private void MES착공시작요청()
        {
            MESSAGE msg = new MESSAGE();
            msg.MSG_ID = EQPID.REQ_PROCESS_START.ToString();
            msg.SYSTEMID = "EQPID";
            msg.DATE_TIME = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fffff");
            msg.BARCODE_ID = $"";
            msg.KEY = "0001";

            //QR검사결과 데이터 전송 및 응답 대기 후 완료보고 받고 이후 검사 진행.
        }

        private void 재검사여부확인(PLC커맨드목록 커맨드)
        {
            if (Global.장치통신.재검사여부)
            {
                재검사 = 재검사여부.TRUE;
            }
        }

        private void 하부큐알리딩수행(PLC커맨드목록 커맨드, Int32 제품인덱스)
        {
            if (커맨드 != PLC커맨드목록.하부큐알트리거) return;

            제품인덱스 = Global.장치통신.검사위치별제품인덱스버퍼[PLC커맨드목록.셔틀01제품인덱스];

            if (제품인덱스 <= 0)
            {
                Global.오류로그(로그영역, "하부큐알리딩", $"제품인덱스가 없습니다.", true);
                return;
            }

            Global.모델자료.선택모델.검사시작(제품인덱스);

            new Thread(() =>
            {
                검사결과 검사 = Global.검사자료.검사시작(제품인덱스);
                검사.재검사여부 = this.재검사;

                큐알리더.큐알리딩결과 하부큐알1결과 = Global.큐알제어.하부큐알리더1.커맨드전송및응답확인(큐알동작커맨드.리딩시작, 3000);
                큐알리더.큐알리딩결과 하부큐알2결과 = Global.큐알제어.하부큐알리더2.커맨드전송및응답확인(큐알동작커맨드.리딩시작, 3000);

                if (하부큐알1결과.결과자료.정상여부)
                {
                    검사.큐알정보검사(검사항목.하부큐알코드1, 하부큐알1결과.결과자료.응답내용);
                    //Global.정보로그(로그영역, "하부큐알1리딩수행", $"제품인덱스 {제품인덱스}, 결과 : {하부큐알1결과.결과자료.응답내용}", false);
                }
                else
                {
                    //Global.정보로그(로그영역, "하부큐알1리딩수행", $"큐알리딩실패 : {하부큐알1결과.결과자료.오류내용}", false);
                    Global.큐알제어.하부큐알리더1.리딩종료();
                }

                if (하부큐알2결과.결과자료.정상여부)
                {
                    검사.큐알정보검사(검사항목.하부큐알코드2, 하부큐알2결과.결과자료.응답내용);
                    //Global.정보로그(로그영역, "하부큐알2리딩수행", $"제품인덱스 {제품인덱스}, 결과 : {하부큐알2결과.결과자료.응답내용}", false);

                    Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[커맨드].완료주소, 1);
                    Task.Delay(50);
                    Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[커맨드].Busy주소, 0);
                    Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[커맨드].완료주소, 0);
                }
                else
                {
                    //Global.정보로그(로그영역, "하부큐알2리딩수행", $"큐알리딩실패 : {하부큐알2결과.결과자료.오류내용}", false);
                    Global.큐알제어.하부큐알리더2.리딩종료();
                }
            })
            { Priority = ThreadPriority.Highest }.Start();
        }

        private void 바닥평면센서수행(PLC커맨드목록 커맨드, Int32 제품인덱스)
        {
            if (커맨드 != PLC커맨드목록.바닥평면트리거) return;

            제품인덱스 = Global.장치통신.검사위치별제품인덱스버퍼[PLC커맨드목록.셔틀02제품인덱스];

            if (제품인덱스 <= 0)
            {
                Global.오류로그(로그영역, "바닥평면센서수행", $"제품인덱스가 없습니다.", true);
                return;
            }

            if (Global.환경설정.Only어퍼하우징검사)
            {
                Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[커맨드].완료주소, 1);
                Thread.Sleep(50);
                Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[커맨드].Busy주소, 0);
                Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[커맨드].완료주소, 0);
                return;
            }

            new Thread(() =>
            {
                검사결과 검사 = Global.검사자료.검사결과찾기(제품인덱스);
                if (검사 == null) return;
                Dictionary<변위센서구분, Single> 자료 = new Dictionary<변위센서구분, Single>();
                if (Global.변위센서제어.Read(센서구분.FRONT1, out 자료))
                {
                    foreach (var s in 자료)
                    {
                        검사.SetResult(s.Key.ToString(), s.Value);
                    }
                }
                else
                {
                    Global.오류로그(로그영역, "바닥평면센서수행", "변위센서 읽기 오류", true);
                    return;
                }

                자료 = new Dictionary<변위센서구분, Single>();
                if (Global.변위센서제어.Read(센서구분.FRONT2, out 자료))
                {
                    foreach (var s in 자료)
                    {
                        검사.SetResult(s.Key.ToString(), s.Value);
                    }
                }
                else
                {
                    Global.오류로그(로그영역, "바닥평면센서수행", "변위센서 읽기 오류", true);
                    return;
                }

                Single[,] 기준위치 = {
                    { 90,  200, (Single)검사.GetItem(검사항목.데이텀A1_F).결과값},
                    {-90,  200, (Single)검사.GetItem(검사항목.데이텀A2_F).결과값},
                    { 90, -230, (Single)검사.GetItem(검사항목.데이텀A3_F).결과값},
                    {-90, -230, (Single)검사.GetItem(검사항목.데이텀A4_F).결과값},
                };
                Single[,] 검사위치 = {
                    { 105,   200, (Single)검사.GetItem(검사항목.변위센서a1).결과값},
                    {   0,   200, (Single)검사.GetItem(검사항목.변위센서a2).결과값},
                    {-105,   200, (Single)검사.GetItem(검사항목.변위센서a3).결과값},
                    {  90,     0, (Single)검사.GetItem(검사항목.변위센서a4).결과값},
                    { -90,     0, (Single)검사.GetItem(검사항목.변위센서a5).결과값},
                    { 105,  -230, (Single)검사.GetItem(검사항목.변위센서a6).결과값},
                    {   0,  -230, (Single)검사.GetItem(검사항목.변위센서a7).결과값},
                    { -105, -230, (Single)검사.GetItem(검사항목.변위센서a8).결과값},
                };

                try
                {
                    Single[] 위치편차 = PlaneDistanceCalculator.CalculateDistances(8, 기준위치, 검사위치);
                    Single 바닥평면 = PlaneDistanceCalculator.FindMinMaxDiff(위치편차);
                    검사.SetResult(검사항목.No7_바닥평면도, 바닥평면);

                    Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[커맨드].완료주소, 1);
                    Task.Delay(50);
                    Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[커맨드].Busy주소, 0);
                    Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[커맨드].완료주소, 0);
                }
                catch (Exception e) { Utils.DebugException(e, 0); }
            }).Start();
        }

        private void 측면촬영수행(PLC커맨드목록 커맨드, Int32 제품인덱스)
        {
            try
            {
                if (커맨드 != PLC커맨드목록.측상촬영트리거) return;

                제품인덱스 = Global.장치통신.검사위치별제품인덱스버퍼[PLC커맨드목록.셔틀02제품인덱스];

                if (제품인덱스 <= 0)
                {
                    Global.오류로그(로그영역, "측면촬영수행", $"제품인덱스가 없습니다.", true);
                    return;
                }

                this.제품인덱스큐[(Int32)카메라구분.Cam01].Enqueue(제품인덱스);
                this.제품인덱스큐[(Int32)카메라구분.Cam02].Enqueue(제품인덱스);
                Debug.WriteLine($"제품인덱스큐 {this.제품인덱스큐.Length} 개");
                new Thread(() =>
                {
                    Global.조명제어.TurnOn(카메라구분.Cam01);
                    Global.그랩제어.Ready(카메라구분.Cam01);

                    Debug.WriteLine($"측면촬영수행(L) : 제품인덱스 {제품인덱스}");
                }).Start();
                Task.Delay(20);
                new Thread(() =>
                {
                    Global.조명제어.TurnOn(카메라구분.Cam02);
                    Global.그랩제어.Ready(카메라구분.Cam02);

                    Debug.WriteLine($"측면촬영수행(R) : 제품인덱스 {제품인덱스}");
                }).Start();
            }
            catch(Exception ee)
            {
                Debug.WriteLine("--------------------------------------측면촬영수행 에러--------------------------------------");
                Debug.WriteLine($"{ee.Message}");
                Debug.WriteLine("--------------------------------------측면촬영수행 에러--------------------------------------");
            }
        }

        private void 상부촬영수행(PLC커맨드목록 커맨드, Int32 제품인덱스)
        {
            if (커맨드 != PLC커맨드목록.측상촬영트리거) return;

            제품인덱스 = Global.장치통신.검사위치별제품인덱스버퍼[PLC커맨드목록.셔틀02제품인덱스];

            if (제품인덱스 <= 0)
            {
                Global.오류로그(로그영역, "상부촬영수행", $"제품인덱스가 없습니다.", true);
                return;
            }

            this.제품인덱스큐[(Int32)카메라구분.Cam03].Enqueue(제품인덱스);
            this.제품인덱스큐[(Int32)카메라구분.Cam08].Enqueue(제품인덱스);

            new Thread(() =>
            {
                Global.조명제어.TurnOn(카메라구분.Cam03);
                Global.그랩제어.Ready(카메라구분.Cam03);
                Debug.WriteLine($"상부촬영수행 : 제품인덱스 {제품인덱스}");
            }).Start();
        }

        private void 상부큐알리딩수행(PLC커맨드목록 커맨드, Int32 제품인덱스)
        {
            try
            {
                if (커맨드 != PLC커맨드목록.상부큐알트리거) return;

                제품인덱스 = Global.장치통신.검사위치별제품인덱스버퍼[PLC커맨드목록.셔틀03제품인덱스];

                if (제품인덱스 <= 0)
                {
                    Global.오류로그(로그영역, "상부큐알리딩수행", $"제품인덱스가 없습니다.", true);
                    return;
                }

                if (Global.환경설정.Only어퍼하우징검사)
                {
                    Task.Run(() =>
                    {
                        Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[커맨드].완료주소, 1);
                        Task.Delay(50);
                        Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[커맨드].Busy주소, 0);
                        Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[커맨드].완료주소, 0);
                        return;
                    });
                }
                else
                {
                    new Thread(() =>
                    {
                        검사결과 검사 = Global.검사자료.검사결과찾기(제품인덱스);
                        if (검사 == null) return;

                        SR2000.SR2000리딩결과 상부큐알결과 = (SR2000.SR2000리딩결과)Global.큐알제어.상부큐알리더.커맨드전송및응답확인(큐알동작커맨드.리딩시작, 3000);

                        if (상부큐알결과.결과자료.정상여부)
                        {
                            검사.큐알정보검사(검사항목.상부큐알코드1, 상부큐알결과.결과자료.큐알별내용[0]);
                            검사.큐알정보검사(검사항목.상부큐알코드2, 상부큐알결과.결과자료.큐알별내용[1]);
                            Global.정보로그(로그영역, "상부큐알리딩수행", $"제품인덱스 {제품인덱스}, 결과1 : {상부큐알결과.결과자료.큐알별내용[0]}, 결과2 : {상부큐알결과.결과자료.큐알별내용[1]}", false);

                            Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[커맨드].완료주소, 1);
                            Thread.Sleep(50);
                            Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[커맨드].Busy주소, 0);
                            Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[커맨드].완료주소, 0);
                        }
                        else
                        {
                            Global.정보로그(로그영역, "상부큐알리딩수행", $"큐알리딩실패 : {상부큐알결과.결과자료.오류내용}", false);
                            Global.큐알제어.상부큐알리더.리딩종료();
                            //리딩실패시 NG로 빼자.
                            Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[커맨드].완료주소, 1);
                            Task.Delay(50);
                            Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[커맨드].Busy주소, 0);
                            Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[커맨드].완료주소, 0);
                        }
                    })
                    { Priority = ThreadPriority.Highest }.Start();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"-------------------------------------상부큐알리딩수행에러 : {ex.Message}-------------------------------------");
                Global.큐알제어.상부큐알리더.리딩종료();
                //리딩실패시 NG로 빼자.
                Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[커맨드].완료주소, 1);
                Task.Delay(50);
                Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[커맨드].Busy주소, 0);
                Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[커맨드].완료주소, 0);
            }

        }

        private void 하부촬영수행(PLC커맨드목록 커맨드, Int32 제품인덱스)
        {
            try
            {
                if (커맨드 != PLC커맨드목록.하부촬영트리거) return;

                제품인덱스 = Global.장치통신.검사위치별제품인덱스버퍼[PLC커맨드목록.셔틀03제품인덱스];

                if (제품인덱스 <= 0)
                {
                    Global.오류로그(로그영역, "하부촬영수행", $"제품인덱스가 없습니다.", true);
                    return;
                }

                this.제품인덱스큐[(Int32)카메라구분.Cam04].Enqueue(제품인덱스);
                this.제품인덱스큐[(Int32)카메라구분.Cam05].Enqueue(제품인덱스);
                Debug.WriteLine($"제품인덱스큐 {this.제품인덱스큐.Length} 개");
                new Thread(() =>
                {
                    Global.조명제어.TurnOn(카메라구분.Cam04);
                    Global.그랩제어.Ready(카메라구분.Cam04);
                    Debug.WriteLine($"하부촬영수행(L) : 제품인덱스 {제품인덱스}");
                }).Start();
                Task.Delay(20);
                new Thread(() =>
                {
                    Global.조명제어.TurnOn(카메라구분.Cam05);
                    Global.그랩제어.Ready(카메라구분.Cam05);
                    Debug.WriteLine($"하부촬영수행(R) : 제품인덱스 {제품인덱스}");
                }).Start();
            }
            catch(Exception ee)
            {
                Debug.WriteLine("--------------------------------------하부촬영수행 에러--------------------------------------");
                Debug.WriteLine($"{ee.Message}");
                Debug.WriteLine("--------------------------------------하부촬영수행 에러--------------------------------------");
            }
        }

        private void 커넥터설삽촬영수행(PLC커맨드목록 커맨드, Int32 제품인덱스)
        {
            if (커맨드 != PLC커맨드목록.커넥터촬영트리거) return;

            제품인덱스 = Global.장치통신.검사위치별제품인덱스버퍼[PLC커맨드목록.셔틀04제품인덱스];

            if (제품인덱스 <= 0)
            {
                Global.오류로그(로그영역, "커넥터설삽촬영수행", $"제품인덱스가 없습니다.", true);
                return;
            }

            this.제품인덱스큐[(Int32)카메라구분.Cam06].Enqueue(제품인덱스);
            this.제품인덱스큐[(Int32)카메라구분.Cam07].Enqueue(제품인덱스);

            new Thread(() =>
            {
                Global.조명제어.TurnOn(카메라구분.Cam06);
                Global.조명제어.TurnOn(카메라구분.Cam07);
                Global.그랩제어.Ready(카메라구분.Cam06);
                Global.그랩제어.Ready(카메라구분.Cam07);
                Global.그랩제어.Triggering(카메라구분.Cam06);
                Global.그랩제어.Triggering(카메라구분.Cam07);
                Debug.WriteLine($"커넥터촬영수행 : 제품인덱스 {제품인덱스}");
            }).Start();
        }

        private void 커버조립여부요청수행(PLC커맨드목록 커맨드, Int32 제품인덱스)
        {
            if (커맨드 != PLC커맨드목록.커버조립트리거) return;

            제품인덱스 = Global.장치통신.검사위치별제품인덱스버퍼[PLC커맨드목록.셔틀05제품인덱스];

            if (제품인덱스 <= 0)
            {
                Global.오류로그(로그영역, "커버조립여부요청수행", $"제품인덱스가 없습니다.", true);
                return;
            }

            검사결과 검사 = Global.검사자료.검사결과찾기(제품인덱스);

            if (Global.환경설정.강제커버조립O)
            {
                Debug.WriteLine("강제커버조립O 들어옴");
                Debug.WriteLine($"강제커버조립O PLC커맨드 : {Global.장치통신.PLC커맨드[커맨드].완료주소}");
                Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[커맨드].완료주소, 1);
                Task.Delay(50);
                Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[커맨드].완료주소, 0);
                return;
            }
            else if (Global.환경설정.강제커버조립X)
            {
                Debug.WriteLine("강제커버조립X 들어옴");
                Debug.WriteLine($"강제커버조립X PLC커맨드 : {Global.장치통신.PLC커맨드[커맨드].Busy주소}");
                Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[커맨드].Busy주소, 1);
                Task.Delay(50);
                Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[커맨드].Busy주소, 0);
                return;
            }

            if (검사.커넥터설삽상부 == 커넥터설삽여부.OK && 검사.커넥터설삽하부 == 커넥터설삽여부.OK && !String.IsNullOrEmpty(검사.상부큐알코드1) && !String.IsNullOrEmpty(검사.상부큐알코드2))
            {
                // 커버조립 O
                Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[커맨드].완료주소, 1);
                Task.Delay(50);
                Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[커맨드].완료주소, 0);
            }
            else
            {
                // 커버조립 X
                Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[커맨드].Busy주소, 1);
                Task.Delay(50);
                Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[커맨드].Busy주소, 0);
            }
        }

        private void 커버들뜸수행(PLC커맨드목록 커맨드, Int32 제품인덱스)
        {
            if (커맨드 != PLC커맨드목록.커버들뜸트리거) return;

            제품인덱스 = Global.장치통신.검사위치별제품인덱스버퍼[PLC커맨드목록.셔틀06제품인덱스];

            if (제품인덱스 <= 0)
            {
                Global.오류로그(로그영역, "커버들뜸수행", $"제품인덱스가 없습니다.", true);
                return;
            }

            new Thread(() =>
            {
                검사결과 검사 = Global.검사자료.검사결과찾기(제품인덱스);
                if (검사 == null) return;

                Dictionary<변위센서구분, Single> 자료 = new Dictionary<변위센서구분, Single>();
                Dictionary<변위센서구분, Single> 자료2 = new Dictionary<변위센서구분, Single>();
                List<Single> 커버들뜸값 = new List<Single>();
                if (Global.변위센서제어.Read(센서구분.REAR1, out 자료))
                {
                    foreach (var s in 자료)
                    {
                        검사.SetResult(s.Key.ToString(), s.Value);
                    }
                }
                else
                {
                    Global.오류로그(로그영역, "커버센서수행", "변위센서 읽기 오류", true);
                    return;
                }
                if (Global.변위센서제어.Read(센서구분.REAR2, out 자료2))
                {
                    foreach (var s in 자료2)
                    {
                        검사.SetResult(s.Key.ToString(), s.Value);
                    }
                }
                else
                {
                    Global.오류로그(로그영역, "커버센서수행", "변위센서 읽기 오류", true);
                    return;
                }

                Single[,] 기준위치 = {
                    {  90,  200, (Single)검사.GetItem(검사항목.데이텀A1_R).결과값 },
                    { -90,  200, (Single)검사.GetItem(검사항목.데이텀A2_R).결과값 },
                    {  90, -230, (Single)검사.GetItem(검사항목.데이텀A3_R).결과값 },
                    { -90, -230, (Single)검사.GetItem(검사항목.데이텀A4_R).결과값 },
                };

                커버들뜸값.Add((Single)검사.GetItem(검사항목.No4_1_커버상m1).결과값);
                커버들뜸값.Add((Single)검사.GetItem(검사항목.No4_2_커버상m2).결과값);
                커버들뜸값.Add((Single)검사.GetItem(검사항목.No4_3_커버상m3).결과값);

                //Single[,] 커버들뜸위치 = { // 커버상m1, 커버상m2, 커버상m3
                //    { 0,   40, (Single)검사.GetItem(검사항목.커버상m1).결과값 },
                //    { 0,  -60, (Single)검사.GetItem(검사항목.커버상m2).결과값 },
                //    { 0, -125, (Single)검사.GetItem(검사항목.커버상m3).결과값 },
                //};

                Single[,] 커버윤곽위치 = {
                    {  26.7f,   74.68f, (Single)검사.GetItem(검사항목.No4_4_커버들뜸k1).결과값 },
                    {  26.7f,  -12.62f, (Single)검사.GetItem(검사항목.No4_5_커버들뜸k2).결과값 },
                    {  26.7f,  -85.92f, (Single)검사.GetItem(검사항목.No4_6_커버들뜸k3).결과값 },
                    {  26.7f, -175.42f, (Single)검사.GetItem(검사항목.No4_7_커버들뜸k4).결과값 },
                    { -26.7f, -175.42f, (Single)검사.GetItem(검사항목.No4_8_커버들뜸k5).결과값 },
                    { -26.7f,  -36.52f, (Single)검사.GetItem(검사항목.No4_9_커버들뜸k6).결과값 },
                    { -26.7f,   33.38f, (Single)검사.GetItem(검사항목.No4_10_커버들뜸k7).결과값 },
                    { -26.7f,   85.48f, (Single)검사.GetItem(검사항목.No4_11_커버들뜸k8).결과값 },
                };

                try
                {
                    Single[] 커버들뜸편차 = PlaneDistanceCalculator.편차계산(커버들뜸값);
                    Single 커버들뜸높이 = PlaneDistanceCalculator.FindAbsMaxDiff2(커버들뜸편차);
                    Debug.WriteLine($"커버들뜸 높이 : {커버들뜸높이}");
                    검사.SetResult(검사항목.No4_커버들뜸, 커버들뜸높이);

                    Single[] 커버윤곽편차 = PlaneDistanceCalculator.CalculateDistances(8, 기준위치, 커버윤곽위치);
                    Single 커버윤곽높이 = PlaneDistanceCalculator.FindAbsMaxDiff(커버윤곽편차);
                    검사.SetResult(검사항목.면윤곽도, 커버윤곽높이);

                    Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[커맨드].완료주소, 1);
                    Task.Delay(50);
                    Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[커맨드].Busy주소, 0);
                    Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[커맨드].완료주소, 0);
                }
                catch (Exception e) { Utils.DebugException(e, 0); }

                Global.모델자료.선택모델.검사종료(제품인덱스);
                Global.검사자료.검사결과계산(제품인덱스);
            })
            { Priority = ThreadPriority.Highest }.Start();
        }

        public void 라벨부착수행(PLC커맨드목록 커맨드, Int32 제품인덱스)
        {
            if (커맨드 != PLC커맨드목록.라벨발행트리거) return;
            제품인덱스 = Global.장치통신.검사위치별제품인덱스버퍼[PLC커맨드목록.셔틀08제품인덱스];

            if (제품인덱스 <= 0)
            {
                Global.오류로그(로그영역, "라벨부착수행", $"제품인덱스가 없습니다.", true);
                return;
            }

            if (Global.환경설정.라벨기사용여부)
            {
                검사결과 검사 = Global.검사자료.검사결과찾기(제품인덱스);
                if (검사 != null)
                {
                    if (Global.큐알인쇄.명령가능())
                    {
                        if (검사.측정결과 == 결과구분.NG)
                        {
                            //Global.큐알인쇄.자료전송(검사.검사일시, 검사.모델구분, 검사.검사코드);
                            Global.큐알인쇄.라벨발행(검사.검사일시, 검사.모델구분, 검사.검사코드);
                            Task.Delay(50);
                            Global.정보로그(로그영역, "라벨부착수행", $"라벨부착커맨드 전송", false);

                            // 라벨부착수행 O
                            Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[커맨드].Busy주소, 1);
                            Task.Delay(50);
                            Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[커맨드].Busy주소, 0);
                        }
                        else
                        {
                            // 라벨부착수행 X
                            Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[커맨드].완료주소, 1);
                            Task.Delay(50);
                            Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[커맨드].완료주소, 0);
                        }
                    }
                    else
                    {
                        Global.정보로그(로그영역, "라벨부착수행", $"이전작업수행중", false);
                    }
                }
                else
                {
                    Global.오류로그(로그영역, "라벨부착수행", $"검사결과(제품인덱스:{제품인덱스})를 찾을 수 없습니다.", true);
                }

                // 라벨부착수행 O
                Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[커맨드].Busy주소, 1);
                Task.Delay(50);
                Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[커맨드].Busy주소, 0);
            }
            else
            {
                // 라벨부착수행 X
                Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[커맨드].완료주소, 1);
                Task.Delay(50);
                Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[커맨드].완료주소, 0);
            }
        }

        private void 검사결과전송수행(PLC커맨드목록 커맨드, Int32 제품인덱스)
        {
            if (커맨드 != PLC커맨드목록.결과요청트리거) return;

            제품인덱스 = Global.장치통신.검사위치별제품인덱스버퍼[PLC커맨드목록.셔틀10제품인덱스];

            if (제품인덱스 <= 0)
            {
                Global.오류로그(로그영역, "검사결과전송수행", $"제품인덱스가 없습니다.", true);
                return;
            }

            검사결과 검사 = Global.검사자료.검사완료(제품인덱스);
            if (검사 == null) return;

            if (Global.환경설정.강제OK배출여부)
            {
                Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[커맨드].완료주소, 1);
                Task.Delay(50);
                Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[커맨드].완료주소, 0);
                return;
            }
            else if (Global.환경설정.강제NG배출여부)
            {
                Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[커맨드].Busy주소, 1);
                Task.Delay(50);
                Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[커맨드].Busy주소, 0);
                return;
            }

            Boolean ok = 검사.측정결과 == 결과구분.OK;

            if (ok) // 검사결과 OK
            {
                Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[커맨드].완료주소, 1);
                Task.Delay(50);
                Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[커맨드].완료주소, 0);
            }
            else // 검사결과 NG
            {
                Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[커맨드].Busy주소, 1);
                Task.Delay(50);
                Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[커맨드].Busy주소, 0);
            }
        }
    }
}
