using MvUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPA.Schemas
{
    public class 큐알인쇄 : 큐알장치
    {
        public enum 제어명령
        {
            [Description("None")]
            명령없음,
            [Description("041")]
            자료전송,
            [Description("C1")]
            자료삭제,
            [Description("01B")]
            라벨발행,
            [Description("002")]
            장치리셋,
            [Description("00")]
            장치상태,
        }
        //public String 로그영역 = "라벨프린터";
        public override String STX { get { return Convert.ToString(2); } }
        public override Char ETX { get { return Convert.ToChar(13); } }
        private Char LF { get { return Convert.ToChar(10); } }
        private Char ETB { get { return Convert.ToChar(23); } }
        private 제어명령 현재명령 = 제어명령.명령없음;
        private const Int32 레이아웃 = 0;
        private const Int32 출력장수 = 1;
        private const String 날짜포맷 = "{0:yyyyMMdd}";
        public Boolean 수행완료 = true;
        public String 명령응답 = String.Empty;
        public Int32 기본대기시간 = 500; // ms

        public override Boolean Init()
        {
            this.주소 = Global.환경설정.큐알인쇄주소;
            this.포트 = Global.환경설정.큐알인쇄포트;
            this.응답분석주기 = 50;
            base.Init();

            return true;
        }

        public delegate void Communication(통신구분 통신, 제어명령 명령, String mesg);
        public event Communication 송신수신알림;

        public void 자료수신(String mesg)
        {
            this.수행완료 = true;
            this.명령응답 = mesg;
            this.송신수신알림?.Invoke(통신구분.RX, this.현재명령, mesg.Trim());
        }

        public Boolean 명령가능()
        {
            return this.수행완료 && this.현재명령 == 제어명령.명령없음;
        }

        public void 명령리셋()
        {
            this.수행완료 = true;
            this.명령응답 = String.Empty;
            this.현재명령 = 제어명령.명령없음;
        }

        private Boolean 명령전송(제어명령 명령, String command)
        {
            if (!명령가능())
            {
                Task.Run(() => Global.오류로그(로그영역, "명령수행", $"[{로그영역}] 이전 명령이 완료되지 않았습니다.", true));
                return false;
            }
            this.현재명령 = 명령;
            this.수행완료 = false;
            this.명령응답 = String.Empty;
            Boolean r = this.SendData(command, Encoding.ASCII);
            if (r) 송신수신알림?.Invoke(통신구분.TX, this.현재명령, command.Trim());
            else Task.Run(() => Global.오류로그(로그영역, "명령수행", $"[{로그영역}] 명령 전송에 실패하였습니다.", true));
            this.명령리셋();
            return r;
        }

        public Boolean 자료전송(DateTime 날짜, 모델구분 모델, Int32 번호)
        {
            this.명령리셋();
            //String 자료 = $"{Utils.GetDescription(제어명령.자료전송)}{Utils.FormatDate(날짜, 날짜포맷)}{LF}{Utils.GetDescription(모델)}{LF}{번호.ToString("d4")}??";
            //return this.명령전송(제어명령.자료전송, 자료);
            String 자료 = $"041C1E1Q0{ETB}D{Utils.GetDescription(모델)}{LF}{Utils.FormatDate(날짜, 날짜포맷)}{LF}{번호.ToString("d4")}??";
            return this.명령전송(제어명령.자료전송, 자료);
        }

        public Boolean 라벨발행(DateTime 날짜, 모델구분 모델, Int32 번호)
        {
            this.명령리셋();
            //String 자료 = $"{Utils.GetDescription(제어명령.라벨발행)}E{레이아웃}Q{출력장수}??";
            String 자료 = $"041C1E1Q0{ETB}D{Utils.GetDescription(모델)}{LF}{Utils.FormatDate(날짜, 날짜포맷)}{LF}{번호.ToString("d4")}??";
            return this.명령전송(제어명령.라벨발행, 자료);
        }

        public Boolean 장치리셋()
        {
            this.명령리셋();
            return this.명령전송(제어명령.장치리셋, "002??");
        }
    }
}
