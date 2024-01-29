using System;
using System.Diagnostics;

namespace TPA.Schemas
{
    public class LCP100DC : 조명장치 // 하부, 2채널 필요
    {
        public override 조명포트 포트 { get; set; } = 조명포트.COM4;
        public override Int32 통신속도 { get; set; } = 19200;
        public override Int32 최대밝기 { get; } = 100;
        public override String STX { get; set; } = $"{Convert.ToChar(2)}";
        public override String ETX { get; set; } = $"{Convert.ToChar(3)}";
        public override Boolean Set(조명정보 정보) => false;
        public override Boolean Save(조명정보 정보) => false; // 커맨드가 있는지 모름
        public override Boolean TurnOn(조명정보 정보)
        {
            if (!SendCommand($"{정보.카메라} On", $"{(Int32)정보.채널}o")) {  
                return false;
            }
            if (!SendCommand($"{정보.카메라} Set", $"{(Int32)정보.채널}d{this.밝기변환(정보.밝기).ToString("d4")}")) {
                return false;
            }
            return true;
        }
        public override Boolean TurnOff(조명정보 정보)
        {
            if (!SendCommand($"{정보.카메라} Set", $"{(Int32)정보.채널}d0000"))
                return false;
            if (!SendCommand($"{정보.카메라} Off", $"{(Int32)정보.채널}f"))
                return false;
            return true;
        }
    }
}
