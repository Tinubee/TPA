using Cognex.VisionPro;
using MvUtils;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPA.Schemas
{
    public static class 그랩이미지처리
    {
        public static Mat ExtendImage(Mat mat, int desWidth, int desHeight)
        {
            Rect source = new Rect(0, 0, mat.Width, mat.Height);
            Mat vMat = new Mat(desHeight, desWidth, MatType.CV_8U, Scalar.Black);
            mat[source].CopyTo(vMat);
            mat.Dispose();
            return vMat;
        }

        public static String SaveImagePath(String 사진저장경로, DateTime 검사시간, 카메라구분 카메라)
        {
            List<String> paths = new List<String> { 사진저장경로, Utils.FormatDate(검사시간, "{0:yyyy-MM-dd}") };
            return Common.CreateDirectory(paths);
        }

        public static String SaveImageFile(String 사진저장경로, 카메라구분 카메라, DateTime 검사시간, Int32 제품인덱스, ImageFormat 포맷)
        {
            String path = SaveImagePath(사진저장경로, 검사시간, 카메라);
            if (String.IsNullOrEmpty(path)) return String.Empty;
            String name;
            if (포맷 == ImageFormat.Jpeg) {
                name = $"{Utils.FormatDate(검사시간, "{0:HHmmss}")}_{카메라.ToString()}_{제품인덱스.ToString("d4")}.jpg";
            }
            else {
                name = $"{Utils.FormatDate(검사시간, "{0:HHmmss}")}_{카메라.ToString()}_{제품인덱스.ToString("d4")}.{포맷}";
            }
            return Path.Combine(path, name);
        }

        public static void ImageSave(String 사진저장경로, Mat 이미지, DateTime 검사시간, 카메라구분 카메라, Int32 제품인덱스, Boolean dispose)
        {
            String file = SaveImageFile(사진저장경로, 카메라, 검사시간, 제품인덱스, ImageFormat.Jpeg);
            if (String.IsNullOrEmpty(file)) {
                Debug.WriteLine("이미지 저장", $"이미지 저장 디렉토리를 만들 수 없습니다.");
                return;
            }

            Task.Run(() => {
                Common.ImageSaveJpeg(이미지, file, out String error, 70);
                if (dispose) 이미지.Dispose();
            });
        }
    }

    public class AcquisitionData : IDisposable
    {
        public 카메라구분 Camera = 카메라구분.None;
        public Mat Image = null;
        public IntPtr SufaceAddr = IntPtr.Zero;
        public Int32 Width = 0;
        public Int32 Height = 0;
        public String Error = String.Empty;

        public AcquisitionData(카메라구분 camera) => this.Camera = camera;
        public AcquisitionData(카메라구분 camera, IntPtr address, Int32 width, Int32 height)
        {
            this.Camera = camera;
            this.SetImage(address, width, height);
        }

        public AcquisitionData(카메라구분 camera, Mat image)
        {
            this.Camera = camera;
            this.Image = image;
            this.Width = image.Width;
            this.Height = image.Height;
        }

        public AcquisitionData(카메라구분 camera, String Error)
        {
            this.Camera = camera;
            this.Error = Error;
        }

        public void SetImage(IntPtr address, Int32 width, Int32 height)
        {
            this.SufaceAddr = address;
            this.Width = width;
            this.Height = height;
        }

        public ICogImage CogImage()
        {
            ICogImage image = null;
            try {
                if (this.SufaceAddr != IntPtr.Zero) {
                    image = new CogImage8Grey();
                    using (CogImage8Root cogImage8Root = new CogImage8Root()) {
                        cogImage8Root.Initialize(this.Width, this.Height, this.SufaceAddr, this.Width, null);
                        (image as CogImage8Grey).SetRoot(cogImage8Root);
                    }
                }
                // else if (this.Image != null) image = new CogImage24PlanarColor(this.Image.ToBitmap());
            }
            catch (Exception e) {
                Debug.WriteLine($"{this.Camera} => {e.Message}", "이미지 변환 오류");
            }
            return image;
        }
        public Mat MatImage()
        {
            if (this.SufaceAddr != IntPtr.Zero) return new Mat(Height, Width, MatType.CV_8UC1, SufaceAddr);
            return this.Image;
        }

        public void Dispose() => this.Image?.Dispose();
    }
}
