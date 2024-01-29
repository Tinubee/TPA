using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPA.Schemas
{
    public enum 카메라구분
    {
        None,
        [Description("측면1")]
        Cam01,
        [Description("측면2")]
        Cam02,
        [Description("상부")]
        Cam03,
        [Description("하부1")]
        Cam04,
        [Description("하부2")]
        Cam05,
        [Description("설삽상부")]
        Cam06,
        [Description("설삽하부")]
        Cam07,
        [Description("노멀각인")]
        Cam08,
    }

    public class 카메라장치
    {
        [JsonProperty("Camera")]
        public virtual 카메라구분 구분 { get; set; } = 카메라구분.None;
        [JsonIgnore]
        public virtual Int32 번호 { get; set; } = 0;
        [JsonProperty("Serial")]
        public virtual String 코드 { get; set; } = String.Empty;
        [JsonIgnore]
        public virtual String 명칭 { get; set; } = String.Empty;
        [JsonProperty("Description")]
        public virtual String 설명 { get; set; } = String.Empty;
        [JsonProperty("IpAddress")]
        public virtual String 주소 { get; set; } = String.Empty;
        [JsonProperty("BlackLevel")]
        public virtual UInt32 밝기 { get; set; } = 0;
        [JsonProperty("Contrast")]
        public virtual Single 대비 { get; set; } = 10;
        [JsonProperty("Width")]
        public virtual Int32 가로 { get; set; } = 0;
        [JsonProperty("Height")]
        public virtual Int32 세로 { get; set; } = 0;
        [JsonProperty("OffsetX")]
        public virtual Int32 OffsetX { get; set; } = 0;
        [JsonProperty("CalibX")]
        public virtual Double 교정X { get; set; } = 0;
        [JsonProperty("CalibY")]
        public virtual Double 교정Y { get; set; } = 0;
        [JsonIgnore]
        public virtual Boolean 상태 { get; set; } = false;

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
        public virtual Boolean Close() => false;
        public virtual Boolean Triggering() => false;
        public virtual void TurnOn() { }
        public virtual void TurnOff() { }
    }
}
