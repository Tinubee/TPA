using DevExpress.Utils.Extensions;
using MvUtils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DevExpress.Pdf.ContentGeneration;
using System.Diagnostics;
using TPA.UI.Controls;

namespace TPA.Schemas
{
    public class 변위센서제어 : Dictionary<센서구분, 변위센서장치>
    {
        public String 로그영역 = "변위센서제어";
        private string 저장파일 { get { return Path.Combine(Global.환경설정.기본경로, "FlatSensor.json"); } }
        private 변위센서장치 센서모듈1;
        private 변위센서장치 센서모듈2;
        private 변위센서장치 센서모듈3;
        private 변위센서장치 센서모듈4;

        public void Init()
        {
            센서모듈1 = new DLEN1() { 주소 = "192.168.3.100", 포트 = 64000 };
            센서모듈2 = new DLEN1() { 주소 = "192.168.3.101", 포트 = 64000 };
            센서모듈3 = new DLEN1() { 주소 = "192.168.3.102", 포트 = 64000 };
            센서모듈4 = new DLEN1() { 주소 = "192.168.3.103", 포트 = 64000 };

            this.Add(센서구분.FRONT1, 센서모듈1);
            this.Add(센서구분.FRONT2, 센서모듈2);
            this.Add(센서구분.REAR1, 센서모듈3);
            this.Add(센서구분.REAR2, 센서모듈4);

            if (Global.환경설정.동작구분 != 동작구분.Live) return;
            this.Load();
            this.Open();
        }

        public 변위센서장치 GetItem(센서구분 구분)
        {
            return this[구분];
        }

        public 변위센서장치 GetItem(센서구분 구분, String 주소, Int32 포트)
        {
            foreach (변위센서장치 센서 in this.Values)
                if (센서.구분 == 구분 && 센서.주소 == 주소 && 센서.포트 == 포트)
                    return 센서;
            return null;
        }

        public void Load()
        {
            if (!File.Exists(this.저장파일)) return;
            try {
                List<변위센서장치> 자료 = JsonConvert.DeserializeObject<List<변위센서장치>>(File.ReadAllText(this.저장파일), Utils.JsonSetting());
                foreach (변위센서장치 정보 in 자료) {
                    변위센서장치 변위센서 = this.GetItem(정보.구분, 정보.주소, 정보.포트);
                    if (변위센서 == null) continue;
                    변위센서.Set(정보);
                }
            }
            catch (Exception ex) {
                Global.오류로그(로그영역, "자료로드", $"센서 설정 로드 실패 : {ex.Message}", true);
            }
        }

        public void Open()
        {
            foreach (센서구분 구분 in this.Keys) {
                if (!this[구분].Init()) {
                    Global.오류로그(로그영역, "초기화", $"센서 통신 연결 실패", true);
                }
            }
        }

        public void Close()
        {
            foreach (센서구분 구분 in this.Keys)
            {
                if (!this[구분].Close())
                {
                    Global.오류로그(로그영역, "초기화", $"센서 통신 해제 실패", true);
                }
            }
        }

        public Boolean Read(센서구분 구분, out Dictionary<변위센서구분, Single> 결과)
        {
            결과 = new Dictionary<변위센서구분, Single>();
            변위센서장치 장치 = GetItem(구분);
            장치.SendData("M0");
            Thread.Sleep(100);
            String[] 센서리딩값 = 장치.ReceiveData().Split(',');

            if (구분 == 센서구분.FRONT1) {
                결과.Add(변위센서구분.변위센서a6, (Single)(Double.Parse(센서리딩값.ElementAt(1)) * 0.001));
                결과.Add(변위센서구분.데이텀A3_F, (Single)(Double.Parse(센서리딩값.ElementAt(2)) * 0.001));
                결과.Add(변위센서구분.변위센서a7, (Single)(Double.Parse(센서리딩값.ElementAt(3)) * 0.001));
                결과.Add(변위센서구분.데이텀A4_F, (Single)(Double.Parse(센서리딩값.ElementAt(4)) * 0.001));
                결과.Add(변위센서구분.변위센서a8, (Single)(Double.Parse(센서리딩값.ElementAt(5)) * 0.001));
                결과.Add(변위센서구분.변위센서a4, (Single)(Double.Parse(센서리딩값.ElementAt(6)) * 0.001));
            }
            else if (구분 == 센서구분.FRONT2) {
                결과.Add(변위센서구분.변위센서a5, (Single)(Double.Parse(센서리딩값.ElementAt(1)) * 0.001));
                결과.Add(변위센서구분.변위센서a1, (Single)(Double.Parse(센서리딩값.ElementAt(2)) * 0.001));
                결과.Add(변위센서구분.데이텀A1_F, (Single)(Double.Parse(센서리딩값.ElementAt(3)) * 0.001));
                결과.Add(변위센서구분.변위센서a2, (Single)(Double.Parse(센서리딩값.ElementAt(4)) * 0.001));
                결과.Add(변위센서구분.데이텀A2_F, (Single)(Double.Parse(센서리딩값.ElementAt(5)) * 0.001));
                결과.Add(변위센서구분.변위센서a3, (Single)(Double.Parse(센서리딩값.ElementAt(6)) * 0.001));
            }
            else if (구분 == 센서구분.REAR1) {
                결과.Add(변위센서구분.No4_3_커버상m3, (Single)(Double.Parse(센서리딩값.ElementAt(1)) * 0.001));
                결과.Add(변위센서구분.No4_2_커버상m2, (Single)(Double.Parse(센서리딩값.ElementAt(2)) * 0.001));
                결과.Add(변위센서구분.No4_8_커버들뜸k5, (Single)(Double.Parse(센서리딩값.ElementAt(3)) * 0.001));
                결과.Add(변위센서구분.No4_6_커버들뜸k3, (Single)(Double.Parse(센서리딩값.ElementAt(4)) * 0.001));
                결과.Add(변위센서구분.데이텀A4_R, (Single)(Double.Parse(센서리딩값.ElementAt(5)) * 0.001));
                결과.Add(변위센서구분.No4_7_커버들뜸k4, (Single)(Double.Parse(센서리딩값.ElementAt(6)) * 0.001));
                결과.Add(변위센서구분.데이텀A3_R, (Single)(Double.Parse(센서리딩값.ElementAt(7)) * 0.001));
            }
            else if (구분 == 센서구분.REAR2) {
                결과.Add(변위센서구분.데이텀A2_R, (Single)(Double.Parse(센서리딩값.ElementAt(1)) * 0.001));
                결과.Add(변위센서구분.No4_1_커버상m1,   (Single)(Double.Parse(센서리딩값.ElementAt(2)) * 0.001));
                결과.Add(변위센서구분.데이텀A1_R, (Single)(Double.Parse(센서리딩값.ElementAt(3)) * 0.001));
                결과.Add(변위센서구분.No4_9_커버들뜸k6, (Single)(Double.Parse(센서리딩값.ElementAt(4)) * 0.001));
                결과.Add(변위센서구분.No4_10_커버들뜸k7, (Single)(Double.Parse(센서리딩값.ElementAt(5)) * 0.001));
                결과.Add(변위센서구분.No4_11_커버들뜸k8, (Single)(Double.Parse(센서리딩값.ElementAt(6)) * 0.001));
                결과.Add(변위센서구분.No4_4_커버들뜸k1, (Single)(Double.Parse(센서리딩값.ElementAt(7)) * 0.001));
                결과.Add(변위센서구분.No4_5_커버들뜸k2, (Single)(Double.Parse(센서리딩값.ElementAt(8)) * 0.001));
            }
            return true;   
        }
    }
}
