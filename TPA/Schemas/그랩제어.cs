using Euresys.MultiCam;
using MvCamCtrl.NET;
using MvUtils;
using Newtonsoft.Json;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using static TPA.Schemas.장치통신;

namespace TPA.Schemas
{
    public class 그랩제어 : Dictionary<카메라구분, 카메라장치>
    {
        [JsonIgnore]
        private const string 로그영역 = "카메라";

        public static List<카메라구분> 대상카메라 = new List<카메라구분>()
                            { 카메라구분.Cam01, 카메라구분.Cam02, 카메라구분.Cam03, 카메라구분.Cam04,
                              카메라구분.Cam05, 카메라구분.Cam06, 카메라구분.Cam07, 카메라구분.Cam08 };
        public delegate void 그랩완료이벤트(카메라구분 카메라, IntPtr intPtr, Int32 width, Int32 height);
        public event 그랩완료이벤트 OnGrabbedImage;

        public HikeGigE 측면카메라1 = null; // Side Left
        public HikeGigE 측면카메라2 = null; // Side Right
        public EuresysLink 상부카메라 = null; // Top
        public HikeGigE 하부카메라1 = null; // Bottom Left
        public HikeGigE 하부카메라2 = null; // Bottom Right
        public HikeGigE 커넥터설삽카메라1 = null;
        public HikeGigE 커넥터설삽카메라2 = null;

        [JsonIgnore]
        private string 저장파일 { get { return Path.Combine(Global.환경설정.기본경로, "Camera.json"); } }
        [JsonIgnore]
        public Boolean 정상여부 { get { return !this.Values.Any(e => !e.상태); } }

        public Boolean Init()
        {
            
            MC.OpenDriver(); // Euresys (Multicam)

            // backup this.측면카메라1 = new HikeGigE() { 구분 = 카메라구분.Cam01, 코드 = "DA1698484", 교정X = 0.014d, 교정Y = 0.014d };
            // backup this.측면카메라2 = new HikeGigE() { 구분 = 카메라구분.Cam02, 코드 = "DA1698487", 교정X = 0.014d, 교정Y = 0.014d };
            // backup this.상부카메라 = new EuresysLink(카메라구분.Cam03) { 코드 = "", 교정X = 0.015398409d, 교정Y = 0.016d };
            // backup this.하부카메라1 = new HikeGigE() { 구분 = 카메라구분.Cam04, 코드 = "DA1698488", 교정X = 0.014d, 교정Y = 0.014d };
            // backup this.하부카메라2 = new HikeGigE() { 구분 = 카메라구분.Cam05, 코드 = "DA1698486", 교정X = 0.014d, 교정Y = 0.014d };
            // backup this.커넥터설삽카메라1 = new HikeGigE() { 구분 = 카메라구분.Cam06, 코드 = "DA1278379", 교정X = 0.014640d, 교정Y = 0.014640d };
            // backup this.커넥터설삽카메라2 = new HikeGigE() { 구분 = 카메라구분.Cam07, 코드 = "DA0652350", 교정X = 0.014658d, 교정Y = 0.014658d };

            this.측면카메라1 = new HikeGigE() { 구분 = 카메라구분.Cam01, 코드 = "DA1698484", 교정X = 0.014d, 교정Y = 0.014d };
            this.측면카메라2 = new HikeGigE() { 구분 = 카메라구분.Cam02, 코드 = "DA1698487", 교정X = 0.014d, 교정Y = 0.014d };
            this.상부카메라 = new EuresysLink(카메라구분.Cam03) { 코드 = "", 교정X = 0.016d, 교정Y = 0.016d };
            this.하부카메라1 = new HikeGigE() { 구분 = 카메라구분.Cam04, 코드 = "DA1698488", 교정X = 0.014d, 교정Y = 0.014d };
            this.하부카메라2 = new HikeGigE() { 구분 = 카메라구분.Cam05, 코드 = "DA1698486", 교정X = 0.014d, 교정Y = 0.014d };
            this.커넥터설삽카메라1 = new HikeGigE() { 구분 = 카메라구분.Cam06, 코드 = "DA1278379", 교정X = 0.014d, 교정Y = 0.014d };
            this.커넥터설삽카메라2 = new HikeGigE() { 구분 = 카메라구분.Cam07, 코드 = "DA0652350", 교정X = 0.014d, 교정Y = 0.014d };

            this.Add(카메라구분.Cam01, this.측면카메라1);
            this.Add(카메라구분.Cam02, this.측면카메라2);
            this.Add(카메라구분.Cam03, this.상부카메라);
            this.Add(카메라구분.Cam04, this.하부카메라1);
            this.Add(카메라구분.Cam05, this.하부카메라2);
            this.Add(카메라구분.Cam06, this.커넥터설삽카메라1);
            this.Add(카메라구분.Cam07, this.커넥터설삽카메라2);

            카메라장치 정보;
            List<카메라장치> 자료 = Load();
            if (자료 != null) {
                foreach (카메라장치 설정 in 자료) {
                    정보 = this.GetItem(설정.구분);
                    if (정보 == null) continue;
                    정보.Set(설정);
                }
            }

            if (Global.환경설정.동작구분 != 동작구분.Live) return true;

            List<CCameraInfo> 카메라들 = new List<CCameraInfo>();
            Int32 nRet = CSystem.EnumDevices(CSystem.MV_GIGE_DEVICE, ref 카메라들);
            if (!Validate("Enumerate devices fail!", nRet, true)) return false;

            for (int i = 0; i < 카메라들.Count; i++) {
                CGigECameraInfo gigeInfo = 카메라들[i] as CGigECameraInfo;
                HikeGigE gige = this.GetItem(gigeInfo.chSerialNumber) as HikeGigE;
                if (gige == null) continue;
                gige.Init(gigeInfo);
            }

            Debug.WriteLine($"카메라 갯수: {this.Count}");
            Global.정보로그(로그영역, "카메라 연결", $"카메라 초기화 성공", false);
            GC.Collect();
            return true;
        }

        private List<카메라장치> Load()
        {
            if (!File.Exists(this.저장파일)) return null;
            return JsonConvert.DeserializeObject<List<카메라장치>>(File.ReadAllText(this.저장파일), Utils.JsonSetting());
        }

        public void Save()
        {
            if (!Utils.WriteAllText(저장파일, JsonConvert.SerializeObject(this.Values, Utils.JsonSetting())))
                Global.오류로그(로그영역, "카메라 설정 저장", "카메라 설정 저장에 실패하였습니다.", true);
        }

        public void Close()
        {
            this.Save();
            if (Global.환경설정.동작구분 != 동작구분.Live) return;
            foreach (카메라장치 장치 in this.Values)
                장치?.Close();
        }

        public void Ready(카메라구분 카메라) => this.GetItem(카메라)?.Ready();

        public void Triggering(카메라구분 카메라) => this.GetItem(카메라)?.Triggering();

        public void 그랩완료(카메라구분 카메라, IntPtr intPtr, Int32 width, Int32 height)
        {
            if (Global.장치통신.자동수동여부)
            {
                // org Int32 제품인덱스 = Global.장치통신.촬영위치별제품인덱스(카메라, false);
                Int32 제품인덱스 = Global.제품검사수행.촬영위치별제품인덱스(카메라);
                검사결과 검사 = Global.검사자료.검사결과찾기(제품인덱스);
                DateTime 검사시간 = 검사 != null ? 검사.검사일시 : DateTime.Now;
                Global.정보로그("카메라", "그랩완료", $"{카메라} 제품인덱스 {제품인덱스}", false);
                if (제품인덱스 > 0)
                {
                    Task.Run(() =>
                    {
                        AcquisitionData data = new AcquisitionData(카메라);
                        data.SetImage(intPtr, width, height);

                        if (카메라 == 카메라구분.Cam03)
                        {
                            Task.Run(() =>
                            {
                                Global.비전검사.Run(data);
                            });
                        }
                        else {
                            Global.비전검사.Run(data);
                        }
                
                        if (카메라 == 카메라구분.Cam03)
                        {
                            Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[PLC커맨드목록.측상촬영트리거].완료주소, 1);
                            Thread.Sleep(50);
                            Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[PLC커맨드목록.측상촬영트리거].Busy주소, 0);
                            Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[PLC커맨드목록.측상촬영트리거].완료주소, 0);
                
                            return;
                        }
                        else if (카메라 == 카메라구분.Cam05)
                        {
                            Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[PLC커맨드목록.하부촬영트리거].완료주소, 1);
                            Thread.Sleep(50);
                            Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[PLC커맨드목록.하부촬영트리거].Busy주소, 0);
                            Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[PLC커맨드목록.하부촬영트리거].완료주소, 0);
                        }
                        else if (카메라 == 카메라구분.Cam07)
                        {
                            Debug.WriteLine($"{카메라구분.Cam07} 그랩완료시 들어오는 else if문 들어옴");
                            Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[PLC커맨드목록.커넥터촬영트리거].완료주소, 1);
                            Thread.Sleep(50);
                            Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[PLC커맨드목록.커넥터촬영트리거].Busy주소, 0);
                            Global.장치통신.강제쓰기(Global.장치통신.PLC커맨드[PLC커맨드목록.커넥터촬영트리거].완료주소, 0);
                        }
                        Mat mat = new Mat(height, width, MatType.CV_8U, intPtr);
                        그랩이미지처리.ImageSave(Global.환경설정.사진저장경로, mat, 검사시간, 카메라, 제품인덱스, true);
                    });
                }
                else
                {
                    Global.경고로그("카메라", "제품인덱스", $"카메라 [{Utils.GetDescription(카메라)}]의 검사 Index가 없습니다.", false);
                }
                this[카메라].Stop();
            }
            Global.조명제어.TurnOff(카메라);
        }

        public 카메라장치 GetItem(카메라구분 구분)
        {
            if (this.ContainsKey(구분)) return this[구분];
            return null;
        }

        private 카메라장치 GetItem(String serial) => this.Values.Where(e => e.코드 == serial).FirstOrDefault();

        public static Boolean Validate(String message, Int32 errorNum, Boolean show)
        {
            if (errorNum == CErrorDefine.MV_OK) return true;

            String errorMsg = String.Empty;
            switch (errorNum)
            {
                case CErrorDefine.MV_E_HANDLE: errorMsg = "Error or invalid handle"; break;
                case CErrorDefine.MV_E_SUPPORT: errorMsg = "Not supported function"; break;
                case CErrorDefine.MV_E_BUFOVER: errorMsg = "Cache is full"; break;
                case CErrorDefine.MV_E_CALLORDER: errorMsg = "Function calling order error"; break;
                case CErrorDefine.MV_E_PARAMETER: errorMsg = "Incorrect parameter"; break;
                case CErrorDefine.MV_E_RESOURCE: errorMsg = "Applying resource failed"; break;
                case CErrorDefine.MV_E_NODATA: errorMsg = "No data"; break;
                case CErrorDefine.MV_E_PRECONDITION: errorMsg = "Precondition error, or running environment changed"; break;
                case CErrorDefine.MV_E_VERSION: errorMsg = "Version mismatches"; break;
                case CErrorDefine.MV_E_NOENOUGH_BUF: errorMsg = "Insufficient memory"; break;
                case CErrorDefine.MV_E_UNKNOW: errorMsg = "Unknown error"; break;
                case CErrorDefine.MV_E_GC_GENERIC: errorMsg = "General error"; break;
                case CErrorDefine.MV_E_GC_ACCESS: errorMsg = "Node accessing condition error"; break;
                case CErrorDefine.MV_E_ACCESS_DENIED: errorMsg = "No permission"; break;
                case CErrorDefine.MV_E_BUSY: errorMsg = "Device is busy, or network disconnected"; break;
                case CErrorDefine.MV_E_NETER: errorMsg = "Network error"; break;
                default: errorMsg = "Unknown error"; break;
            }

            Global.오류로그("Camera", "Error", $"[{errorNum}] {message} {errorMsg}", show);
            return false;
        }
    }
}
