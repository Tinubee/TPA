using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPA.Schemas
{
    public enum 센서구분
    {
        None,
        FRONT1,
        FRONT2,
        REAR1,
        REAR2,
    }

    public enum 변위센서구분
    {
        데이텀A1_F,
        데이텀A2_F,
        데이텀A3_F,
        데이텀A4_F,
        변위센서a1,
        변위센서a2,
        변위센서a3,
        변위센서a4,
        변위센서a5,
        변위센서a6,
        변위센서a7,
        변위센서a8,
        데이텀A1_R,
        데이텀A2_R,
        데이텀A3_R,
        데이텀A4_R,
        커버들뜸k1,
        커버들뜸k2,
        커버들뜸k3,
        커버들뜸k4,
        커버들뜸k5,
        커버들뜸k6,
        커버들뜸k7,
        커버들뜸k8,
        커버상m1,
        커버상m2,
        커버상m3,
    }

    public class 변위센서장치 : 소켓통신기본클래스
    {
        public virtual 센서구분 구분 { get; set; } = 센서구분.None;
        public virtual Int32 오프셋 { get; set; } = 0;
        public virtual Boolean 상태 { get; set; } = false;

        private String ETX = "\r\n";

        public Boolean Init()
        {
            return base.Connect(this.주소, this.포트);
        }

        public Boolean Close()
        {
            return base.Disconnect();
        }

        public Boolean SendData(String data)
        {
            return base.SendData(data + ETX);
        }

        public String ReceiveData()
        {
            return base.ReceiveData();
        }

        public virtual void Set(변위센서장치 장치)
        {
            if (장치 == null) return;
            this.구분 = 장치.구분;
            this.주소 = 장치.주소;
            this.포트 = 장치.포트;
            this.오프셋 = 장치.오프셋;
            this.상태 = 장치.상태;
        }
    }
}
