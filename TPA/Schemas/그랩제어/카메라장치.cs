using Cognex.VisionPro;
using Newtonsoft.Json;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TPA.Schemas
{
    public enum 카메라구분
    {
        None,
        [Description("Side1"), Translation("Side1", "측면1")]
        Cam01,
        [Description("Side2"),Translation("Side2", "측면2")]
        Cam02,
        [Description("Top"),Translation("Top", "상부")]
        Cam03,
        [Description("Bottom1"),Translation("Bottom1", "하부1")]
        Cam04,
        [Description("Bottom2"), Translation("Bottom2", "하부2")]
        Cam05,
        [Description("Connector Top"), Translation("Connector Top", "설삽상부")]
        Cam06,
        [Description("Connector Bottom"), Translation("Connector Bottom", "설삽하부")]
        Cam07,
        [Description("Normal"), Translation("Normal", "노멀각인")]
        Cam08,
    }

    public class 카메라장치 : IDisposable
    {
        [JsonProperty("Camera"), Translation("Camera", "카메라")]
        public virtual 카메라구분 구분 { get; set; } = 카메라구분.None;
        [JsonIgnore, Translation("Camera", "카메라이름")]
        public virtual String 카메라이름 { get; set; } = String.Empty;
        [JsonIgnore]
        public virtual Int32 번호 { get; set; } = 0;
        [JsonProperty("Serial"), Translation("Serial", "시리얼번호")]
        public virtual String 코드 { get; set; } = String.Empty;
        [JsonIgnore]
        public virtual String 명칭 { get; set; } = String.Empty;
        [JsonProperty("Description"), Translation("Description", "설명")]
        public virtual String 설명 { get; set; } = String.Empty;
        [JsonProperty("IpAddress"), Translation("IP Address", "IP 주소")]
        public virtual String 주소 { get; set; } = String.Empty;
        [JsonProperty("BlackLevel"), Translation("BlackLevel", "밝기")]
        public virtual UInt32 밝기 { get; set; } = 0;
        [JsonProperty("Contrast"), Translation("Contrast", "대비")]
        public virtual Single 대비 { get; set; } = 10;
        [JsonProperty("Width"), Translation("Width", "가로")]
        public virtual Int32 가로 { get; set; } = 0;
        [JsonProperty("Height"), Translation("Height", "세로")]
        public virtual Int32 세로 { get; set; } = 0;
        [JsonProperty("OffsetX"), Translation("OffsetX", "OffsetX")]
        public virtual Int32 OffsetX { get; set; } = 0;
        [JsonProperty("CalibX"), Translation("X Calibration Value", "X 교정값")]
        public virtual Double 교정X { get; set; } = 0;
        [JsonProperty("CalibY"), Translation("Y Calibration Value", "Y 교정값")]
        public virtual Double 교정Y { get; set; } = 0;
        [JsonIgnore, Translation("Status", "상태")]
        public virtual Boolean 상태 { get; set; } = false;
        [JsonIgnore]
        internal virtual MatType ImageType => MatType.CV_8UC1;
        [JsonIgnore]
        internal virtual Boolean UseMemoryCopy => false;
        [JsonIgnore]
        internal Int32 ImageWidth = 0;
        [JsonIgnore]
        internal Int32 ImageHeight = 0;
        [JsonIgnore]
        internal Object BufferLock = new Object();
        [JsonIgnore]
        internal UInt32 BufferSize = 0;
        [JsonIgnore]
        internal IntPtr BufferAddress = IntPtr.Zero;
        [JsonIgnore]
        internal Queue<Mat> Images = new Queue<Mat>();
        [JsonIgnore]
        internal Mat Image => Images.LastOrDefault<Mat>();
        [JsonIgnore]
        public static TranslationAttribute 로그영역 = new TranslationAttribute("Camera", "카메라");


        public void Dispose()
        {
            while (this.Images.Count > 3)
                this.Dispose(this.Images.Dequeue());
        }
        internal void Dispose(Mat image)
        {
            if (image == null || image.IsDisposed) return;
            image.Dispose();
        }

        public virtual void Set(카메라장치 장치)
        {
            if (장치 == null) return;
            this.코드 = 장치.코드;
            this.설명 = 장치.설명;
            this.밝기 = 장치.밝기;
            this.가로 = 장치.가로;
            this.세로 = 장치.세로;
            this.OffsetX = 장치.OffsetX;
        }

        public virtual Boolean Init() => false;
        public virtual Boolean Ready() => false;
        public virtual Boolean Start() => false;
        public virtual Boolean Stop() => false;
        public virtual Boolean Close()
        {
            while (this.Images.Count > 0)
                this.Dispose(this.Images.Dequeue());
            return true;
        }
        public virtual Boolean Triggering() => false;
        public virtual void TurnOn() { }
        public virtual void TurnOff() { }


        #region 이미지그랩
        internal void InitBuffers(Int32 width, Int32 height)
        {
            if (width == 0 || height == 0) return;
            Int32 channels = ImageType == MatType.CV_8UC3 ? 3 : 1;
            Int32 imageSize = width * height * channels;
            if (BufferAddress != IntPtr.Zero && imageSize == BufferSize) return;
            this.ImageWidth = width; this.ImageHeight = height;
            if (BufferAddress != IntPtr.Zero)
            {
                Marshal.Release(BufferAddress);
                BufferAddress = IntPtr.Zero;
                BufferSize = 0;
            }

            BufferAddress = Marshal.AllocHGlobal(imageSize);
            if (BufferAddress == IntPtr.Zero) return;
            BufferSize = (UInt32)imageSize;
            Debug.WriteLine(this.구분.ToString(), "InitBuffers");
        }

        internal void CopyMemory(IntPtr surfaceAddr, Int32 width, Int32 height)
        {
            // 메모리 복사
            lock (this.BufferLock)
            {
                try
                {
                    this.InitBuffers(width, height);
                    Common.CopyMemory(BufferAddress, surfaceAddr, BufferSize);
                }
                catch (Exception e)
                {
                    Global.오류로그(로그영역.GetString(), "Acquisition", $"[{this.구분.ToString()}] {e.Message}", true);
                }
            }
        }

        internal void AcquisitionFinished(IntPtr surfaceAddr, Int32 width, Int32 height)
        {
            if (surfaceAddr == IntPtr.Zero) { AcquisitionFinished("Failed."); return; }
            try
            {
                if (this.UseMemoryCopy) this.CopyMemory(surfaceAddr, width, height);
                else
                {
                    this.BufferAddress = surfaceAddr;
                    this.ImageWidth = width;
                    this.ImageHeight = height;
                }
                Global.그랩제어.그랩완료(this);
            }
            catch (Exception ex)
            {
                Global.오류로그(로그영역.GetString(), "Acquisition", $"[{this.구분}] {ex.Message}", true);
            }
        }

        internal void AcquisitionFinished(String error) =>
            Global.오류로그(로그영역.GetString(), "Acquisition", $"[{this.구분.ToString()}] {error}", true);
        internal void AcquisitionFinished(Mat image)
        {
            if (image == null)
            {
                AcquisitionFinished("Failed.");
                return;
            }
            this.Images.Enqueue(image);
            this.Dispose();
            Global.그랩제어.그랩완료(this);
        }

        public ICogImage CogImage()
        {
            try
            {
                if (this.Image != null) return Common.ToCogImage(this.Image);
                if (this.BufferAddress == IntPtr.Zero) return null;
                using (CogImage8Root cogImage8Root = new CogImage8Root())
                {
                    CogImage8Grey image = new CogImage8Grey();
                    cogImage8Root.Initialize(ImageWidth, ImageHeight, BufferAddress, ImageWidth, null);
                    image.SetRoot(cogImage8Root);
                    return image;
                }
            }
            catch (Exception e)
            {
                Global.오류로그(로그영역.GetString(), "Acquisition", $"[{this.구분.ToString()}] {e.Message}", true);
            }
            return null;
        }
        public Mat MatImage()
        {
            if (this.Image != null) return this.Image;
            if (BufferAddress == IntPtr.Zero) return null;
            return new Mat(ImageHeight, ImageWidth, ImageType, BufferAddress);
        }
        #endregion
    }
}
