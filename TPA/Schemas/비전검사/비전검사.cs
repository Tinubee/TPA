using Cognex.VisionPro;
using Cognex.VisionPro.QuickBuild;
using CogUtils;
using DevExpress.Utils.Extensions;
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
    public class 비전검사 : Dictionary<카메라구분, 비전도구>
    {
        #region 도구초기화
        private CogJobManager Manager = null;
        public 비전검사() { InitPath(); }

        private void InitPath()
        {
            foreach (모델구분 모델 in Enum.GetValues(typeof(모델구분))) {
                if (모델 == 모델구분.None) continue;
                String path = Path.Combine(Global.환경설정.비전도구경로, ((Int32)모델).ToString("d2"));
                if (Directory.Exists(path)) continue;
                Directory.CreateDirectory(path);
            }
        }

        public void Init() => this.Init(Global.환경설정.선택모델);

        public void Init(모델구분 모델)
        {
            this.Close();
            this.Manager = new CogJobManager("JobManager") { GarbageCollection = true, GarbageCollectionInterval = 5 };

            List<카메라구분> 대상카메라 = new List<카메라구분>() {
                    카메라구분.Cam01, 카메라구분.Cam02, 카메라구분.Cam03, 카메라구분.Cam04,
                    카메라구분.Cam05, 카메라구분.Cam06, 카메라구분.Cam07, 카메라구분.Cam08 };
            foreach (카메라구분 구분 in 대상카메라) {
                비전도구 도구 = new 비전도구(모델, 구분);
                도구.Init();
                this.Add(구분, 도구);
                this.Manager.JobAdd(도구.Job);
            }
        }

        public void Close()
        {
            foreach (비전도구 도구 in this.Values)
                도구.Job?.Shutdown();
            this.Manager?.Shutdown();
            this.Manager = null;
            this.Clear();
            GC.Collect();
        }

        // public void SetCalib() => this.Values.ForEach(e => e.SetCalib());

        public void SetDisplay(카메라구분 카메라, RecordDisplay display)
        {
            if (!this.ContainsKey(카메라)) return;
            this[카메라].Display = display;
        }

        public static void 도구설정(비전도구 도구)
        {
            UI.Forms.CogToolEdit viewForm = new UI.Forms.CogToolEdit();
            viewForm.Init(도구);
            viewForm.Show(Global.MainForm);
        }
        public void 도구설정(카메라구분 구분)
        {
            if (!this.ContainsKey(구분)) return;
            도구설정(this[구분]);
        }
        #endregion

        #region Run
        public static String SaveImagePath(카메라구분 카메라, DateTime 검사시간, String 사진저장경로, 모델구분 선택모델)
        {
            List<String> paths = new List<String> { 사진저장경로, Utils.FormatDate(검사시간, "{0:yyyy-MM-dd}"), 선택모델.ToString(), 카메라.ToString() };
            return Common.CreateDirectory(paths);

        }
        public static String SaveImageFile(카메라구분 카메라, DateTime 검사시간, String 사진저장경로, 모델구분 선택모델, Int32 검사번호, ImageFormat 포맷, 결과구분 결과 = 결과구분.NO)
        {
            String path = SaveImagePath(카메라, 검사시간, 사진저장경로, 선택모델);
            if (String.IsNullOrEmpty(path)) return String.Empty;
            String name;
            if (포맷 == ImageFormat.Jpeg) {
                name = $"{검사번호.ToString("d4")}_{Utils.FormatDate(검사시간, "{0:HHmmss}")}.jpg";
            }
            else {
                name = $"{검사번호.ToString("d4")}_{Utils.FormatDate(검사시간, "{0:HHmmss}")}.{포맷}";
            }
            return Path.Combine(path, name);
        }

        public Boolean Run(카메라장치 장치, 검사결과 결과)
        {
            Int32 result;
            Debug.WriteLine($"{Utils.FormatDate(DateTime.Now, "{0:HHmmss.fff}")} [검사수행] {장치.구분}");
            if (결과 == null) return false;
            Boolean r = Run(장치.구분, 장치.CogImage(), 결과);
            Global.사진자료.SaveImage(장치, 결과);
            if (!Global.제품검사수행.제품인덱스큐[(Int32)장치.구분].TryDequeue(out result))
            {
                // Handle the case where the dequeue operation fails, if necessary
                Global.오류로그("비전검사", "Run", "Failed to dequeue an item.", false);
                Debug.WriteLine("Failed to dequeue an item.");
            }
            else
            {
                Debug.WriteLine($"{result} 인덱스큐 제거완료.");
            }
            return r;
        }

        public Boolean Run(카메라구분 카메라, ICogImage image, 검사결과 검사)
        {
            if (image == null)
            {
                Global.오류로그("비전검사", "이미지없음", $"{카메라} 검사할 이미지가 없습니다.", true);
                return false;
            }
            if (!this.ContainsKey(카메라)) return false;
            비전도구 도구 = this[카메라];
            return 도구.Run(image, 검사);
        }

        #endregion
    }
}
