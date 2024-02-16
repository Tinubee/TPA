using MvCamCtrl.NET;
using MvCamCtrl.NET.CameraParams;
using MvUtils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace TPA.Schemas
{
    public class HikeGigE : 카메라장치
    {
        private CCamera Camera = null;
        private CCameraInfo Device;
        private cbOutputExdelegate ImageCallBackDelegate;
        public delegate void ImageCallBackEvent(카메라구분 카메라, IntPtr intPtr, Int32 width, Int32 height);
        public event ImageCallBackEvent OnImageCallbackReceived;

        public Boolean Init(CGigECameraInfo info)
        {
            try {
                this.Camera = new CCamera();
                this.Device = info;
                this.ImageCallBackDelegate = new cbOutputExdelegate(ImageCallBack);
                this.명칭 = info.chManufacturerName + " " + info.chModelName;
                UInt32 ip1 = (info.nCurrentIp & 0xff000000) >> 24;
                UInt32 ip2 = (info.nCurrentIp & 0x00ff0000) >> 16;
                UInt32 ip3 = (info.nCurrentIp & 0x0000ff00) >> 8;
                UInt32 ip4 = info.nCurrentIp & 0x000000ff;
                this.주소 = $"{ip1}.{ip2}.{ip3}.{ip4}";
                this.상태 = this.Init();
            }
            catch (Exception ex) {
                Debug.WriteLine($"초기화 오류 : {ex.Message}");
                this.상태 = false;
            }
            Debug.WriteLine($"{this.명칭}, {this.코드}, {this.주소}, {this.상태}");
            return this.상태;
        }

        public CCamera GetCamera()
        {
            return this.Camera;
        }

        public override Boolean Init()
        {
            Int32 nRet = this.Camera.CreateHandle(ref Device);
            if (!Validate($"[{this.구분}] 카메라 초기화에 실패하였습니다.", nRet, true)) return false;
            nRet = this.Camera.OpenDevice();
            if (!Validate($"[{this.구분}] 카메라 연결 실패!", nRet, true)) return false;

            Validate("", this.Camera.SetBoolValue("BlackLevelEnable", true), false);
            this.밝기적용();

            if (this.구분 == 카메라구분.Cam06 || this.구분 == 카메라구분.Cam07) {
                Validate("AcquisitionMode", this.Camera.SetEnumValue("AcquisitionMode", (UInt32)MV_CAM_ACQUISITION_MODE.MV_ACQ_MODE_CONTINUOUS), false);
                Validate("TriggerMode", this.Camera.SetEnumValue("TriggerMode", (UInt32)MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_ON), false);
                Validate("TriggerSource", this.Camera.SetEnumValue("TriggerSource", (UInt32)MV_CAM_TRIGGER_SOURCE.MV_TRIGGER_SOURCE_SOFTWARE), false);
            }
            Validate("RegisterImageCallBackEx", this.Camera.RegisterImageCallBackEx(this.ImageCallBackDelegate, IntPtr.Zero), false);

            return true;
        }

        public override Boolean Start()
        {
            return Validate($"{this.구분} 그래버 시작 오류!", Camera.StartGrabbing(), true);
        }

        public override Boolean Ready()
        {
           this.Camera.ClearImageBuffer();
            return Start();
        }

        public override Boolean Close()
        {
            if (this.Camera == null || !this.상태) return false;
            return Validate($"{this.구분} 종료오류!", Camera.CloseDevice(), false);
        }

        public override Boolean Triggering()
        {
            if (this.Camera == null || !this.상태) return false;
            return Validate($"{this.구분} 소프트 트리거", this.Camera.SetCommandValue("TriggerSoftware"), true);
        }

        public override Boolean Stop()
        {
            Camera.ClearImageBuffer();
            return Validate($"{this.구분} 정지오류!", Camera.StopGrabbing(), false);
        }

        public void 밝기적용() // Black Level : 0 ~ 4095
        {
            if (this.Camera == null) return;
            Int32 nRet = this.Camera.SetIntValue("BlackLevel", this.밝기);
            Validate($"[{this.구분}] 밝기 설정에 실패하였습니다.", nRet, true);
        }

        private void ImageCallBack(IntPtr data, ref MV_FRAME_OUT_INFO_EX frameInfo, IntPtr user)
        {
            try {
                //Global.그랩제어.그랩완료(this.구분, data, frameInfo.nWidth, frameInfo.nHeight);
                this.AcquisitionFinished(data, frameInfo.nWidth, frameInfo.nHeight);
                this.Stop();
            }
            catch (Exception ex) {
                Debug.WriteLine("Camera Call Back Error", $"[{this.구분}] {ex.Message}");
                return;
            }
        }

        public static Boolean Validate(String message, Int32 errorNum, Boolean show)
        {
            if (errorNum == CErrorDefine.MV_OK) return true;

            String errorMsg = String.Empty;
            switch (errorNum) {
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
            return false;
        }
    }
}
