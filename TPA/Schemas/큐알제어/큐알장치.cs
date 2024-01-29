using DevExpress.Xpo.Logger.Transport;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace TPA.Schemas
{
    public abstract class 큐알장치 : 소켓통신기본클래스
    {
        public enum 통신구분
        {
            TX,
            RX,
        }
        public virtual String 로그영역 { get { return "큐알장치"; } }
        public virtual String STX { get { return String.Empty; } }
        public virtual Char ETX { get { return '\r'; } }

        public Boolean 동작여부 = false;
        public Boolean 연결여부 { get { return this.소켓연결여부; } }
        public Int32 응답분석주기 = 50; // ms

        public virtual Boolean Init() => Ping();
        public virtual void Close() { this.Stop(); this.클라이언트?.Close(); this.Dispose(); }
        public virtual void Start() { this.동작여부 = true; Task.Run(DoWork); }
        public virtual void Stop() => this.동작여부 = false;

        protected virtual void DoWork()
        {
            //while (this.동작여부) {
            //    if (!Connect(this.주소, this.포트))
            //        Global.오류로그(로그영역, "통신체크", this.에러메시지, true);
            //    Task.Delay(5000).Wait();
            //}

            Connect(this.주소, this.포트);
            Task.Delay(5000).Wait();
        }

        public Int32 불량알림간격 = 30;
        public DateTime 연결불량알림 = DateTime.Today.AddDays(-1);
        public Boolean Ping()
        {
            if (Global.환경설정.동작구분 != 동작구분.Live) return true;
            if (!Common.Ping(주소)) {
                if ((DateTime.Now - 연결불량알림).TotalSeconds >= 불량알림간격) {
                    this.연결불량알림 = DateTime.Now;
                    Global.오류로그(로그영역, "통신체크", $"[{주소}] 장치와 통신할 수 없습니다.", true);
                    return false;
                }
            }
            return true;
        }

        protected override Boolean SendData(String data, Int32 타임아웃시간, Encoding encoding = null)
        {
            return base.SendData(STX + data + ETX, 타임아웃시간, encoding);
        }

        protected String ReceiveData(Int32 대기시간)
        {
            DateTime 종료시간 = DateTime.Now.AddMilliseconds(대기시간);
            while (DateTime.Now <= 종료시간) {
                if (this.클라이언트.Available > 0) return base.ReceiveData();
                Task.Delay(응답분석주기).Wait();
            }
            return String.Empty;
        }
    }
}
