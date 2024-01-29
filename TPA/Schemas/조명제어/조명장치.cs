using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO.Ports;

namespace TPA.Schemas
{
    public enum 조명포트
    {
        None,
        COM1,
        COM2,
        COM3,
        COM4,
        COM5,
        COM6,
        COM7,
        COM8,
        COM9,
    }

    public enum 조명채널
    {
        CH01,
        CH02,
        CH03,
        CH04,
        CH05,
    }

    public class 조명정보
    {
        [JsonProperty("Camera")]
        public 카메라구분 카메라 { get; set; } = 카메라구분.None;
        [JsonProperty("Port")]
        public 조명포트 포트 { get; set; } = 조명포트.None;
        [JsonProperty("Channel")]
        public 조명채널 채널 { get; set; } = 조명채널.CH01;
        [JsonProperty("Brightness")]
        public Int32 밝기 { get; set; } = 100;
        [JsonProperty("Description")]
        public String 설명 { get; set; } = String.Empty;
        [JsonIgnore]
        public Boolean 상태 { get; set; } = false;
        [JsonIgnore]
        public 조명장치 컨트롤러;
        public 조명정보() { }
        public 조명정보(카메라구분 카메라, 조명장치 컨트롤)
        {
            this.카메라 = 카메라;
            this.컨트롤러 = 컨트롤;
            this.포트 = 컨트롤.포트;
        }
        public Boolean Set()
        {
            this.상태 = this.컨트롤러.Set(this);
            return this.상태;
        }
        public Boolean TurnOn()
        {
            if (this.상태) return true;
            this.상태 = this.컨트롤러.TurnOn(this);
            return this.상태;
        }
        public Boolean TurnOff()
        {
            if (!this.상태) return true;
            this.상태 = false;
            return this.컨트롤러.TurnOff(this);
        }
        public Boolean OnOff()
        {
            if (this.상태) return this.TurnOff();
            else return this.TurnOn();
        }

        public void Set(조명정보 정보)
        {
            this.밝기 = 정보.밝기;
            this.설명 = 정보.설명;
        }
    }

    public abstract class 조명장치
    {
        public String 로그영역 = "조명제어";
        public abstract 조명포트 포트 { get; set; }
        public abstract Int32 통신속도 { get; set; }
        public abstract Int32 최대밝기 { get; }
        public abstract String STX { get; set; }
        public abstract String ETX { get; set; }
        public SerialPort 통신포트;

        public virtual void Init()
        {
            통신포트 = new SerialPort();
            통신포트.PortName = this.포트.ToString();
            통신포트.BaudRate = 통신속도;
            통신포트.DataBits = (Int32)8;
            통신포트.StopBits = StopBits.One;
            통신포트.Parity = Parity.None;
            통신포트.DataReceived += DataReceived;
            통신포트.ErrorReceived += ErrorReceived;
        }

        public virtual Boolean IsOpen() => 통신포트 != null && 통신포트.IsOpen;

        public virtual Boolean Open()
        {
            if (통신포트 == null) return false;
            try
            {
                통신포트.Open();
                return 통신포트.IsOpen;
            }
            catch (Exception ex)
            {
                통신포트.Dispose();
                통신포트 = null;
                Debug.WriteLine($"조명 제어 포트에 연결할 수 없습니다. {ex.Message}");
                return false;
            }
        }

        public virtual void Close()
        {
            if (통신포트 == null || !통신포트.IsOpen) return;
            통신포트.Close();
            통신포트.Dispose();
            통신포트 = null;
        }

        public virtual Int32 밝기변환(Int32 밝기) => Convert.ToInt32(Math.Round((Double)this.최대밝기 * 밝기 / 100));
        public abstract Boolean Set(조명정보 정보);
        public abstract Boolean Save(조명정보 정보);
        public abstract Boolean TurnOn(조명정보 정보);
        public abstract Boolean TurnOff(조명정보 정보);

        public virtual void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            String data = sp.ReadExisting();
            Debug.WriteLine($"DataReceived 포트={this.포트}, {data}");
        }
        public virtual void ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            Debug.WriteLine($"ErrorReceived 포트={this.포트}, {e.EventType.ToString()}");
            Debug.WriteLine(e.ToString());
        }
        public virtual Boolean SendCommand(String 구분, String Command)
        {
            if (!IsOpen()) {
                Global.오류로그(로그영역, 구분, "조명컨트롤러 포트에 연결할 수 없습니다.", true);
                return false;
            }
            try {
                통신포트.Write($"{STX}{Command}{ETX}");
                return true;
            }
            catch (Exception ex) {
                Global.오류로그(로그영역, 구분, ex.Message, true);
                return false;
            }
        }
    }
}
