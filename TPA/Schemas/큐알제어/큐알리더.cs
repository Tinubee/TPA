using MvUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TPA.Schemas
{
    public enum 큐알리더모델
    {
        None,
        SR2000,
        SR710,
    }

    public enum 큐알동작커맨드
    {
        [Description("None")]
        명령없음,
        [Description("LON")]
        리딩시작,
        [Description("LOFF")]
        리딩종료,
        [Description("RESET")]
        리더리셋,
        [Description("ERRSTAT")]
        오류여부,
        [Description("BUSYSTAT")]
        리딩가능,
    }

    public abstract class 큐알리더 : 큐알장치
    {
        protected String 로그영역 = "큐알리더";
        public delegate void Communication(통신구분 통신, 큐알동작커맨드 커맨드, String mesg);
        public event Communication 송신수신알림;
        public abstract 큐알리딩결과자료 커맨드분석(큐알동작커맨드 커맨드, String 결과);

        public 큐알리더모델 모델 { get; }
        public String 큐알내용 { get; set; }

        public 큐알리더(큐알리더모델 모델)
        {
            this.모델 = 모델;
            this.큐알내용 = String.Empty;
        }

        protected void 송신수신알림발생(통신구분 통신, 큐알동작커맨드 커맨드, String mesg) => this.송신수신알림?.Invoke(통신, 커맨드, mesg);

        public virtual Boolean 커맨드전송(큐알동작커맨드 커맨드, Int32 대기시간 = 500) => this.커맨드전송(커맨드, Utils.GetDescription(커맨드), 대기시간);

        public virtual Boolean 커맨드전송(큐알동작커맨드 커맨드, String command, Int32 대기시간 = 500)
        {
            송신수신알림발생(통신구분.TX, 커맨드, command.Trim());
            return this.SendData(command, 대기시간);
        }

        public virtual 큐알리딩결과 커맨드수신(큐알동작커맨드 커맨드, Int32 대기시간 = 500)
        {
            String 수신데이터 = this.ReceiveData(대기시간);
            송신수신알림발생(통신구분.RX, 커맨드, 수신데이터.Trim());
            큐알리딩결과 결과 = new 큐알리딩결과(this, 커맨드, 수신데이터);
            return 결과;
        }

        public virtual 큐알리딩결과 커맨드전송및응답확인(큐알동작커맨드 커맨드, Int32 대기시간 = 500) => this.커맨드전송및응답확인(커맨드, Utils.GetDescription(커맨드), 대기시간);

        public virtual 큐알리딩결과 커맨드전송및응답확인(큐알동작커맨드 커맨드, String command, Int32 대기시간 = 500)
        {
            if (!커맨드전송(커맨드, command, 대기시간)) {
                Global.오류로그(로그영역, "통신", $"커맨드전송에러", true);
                return null;
            }
            큐알리딩결과 결과 = 커맨드수신(커맨드, 대기시간);
            if (!결과.결과자료.정상여부 && String.IsNullOrEmpty(결과.결과자료.오류내용)) {
                String error = String.IsNullOrEmpty(결과.결과자료.오류내용) ? "커맨드전송실패" : 결과.결과자료.오류내용;
                Global.오류로그(로그영역, "통신", $"커맨드수행에러 : {error}", true);
            }
            return 결과;
        }

        public virtual void 리딩테스트(검사결과 검사결과, 검사항목 구분, Int32 뱅크주소 = 1, Int32 대기시간 = 500)
        {
            큐알리딩결과 리딩결과 = 커맨드전송및응답확인(큐알동작커맨드.리딩시작, $"{Utils.GetDescription(큐알동작커맨드.리딩시작)}{뱅크주소.ToString("d2")}", 대기시간);
            if (리딩결과 != null && 리딩결과.결과자료.정상여부) {
                this.큐알내용 = 리딩결과.결과자료.응답내용;
            }
            else 리딩종료();
            if (검사결과 != null) {
                검사결과.큐알정보검사(구분, this.큐알내용);
            }
        }

        public virtual 큐알리딩결과 리딩종료() => 커맨드전송및응답확인(큐알동작커맨드.리딩종료);

        public virtual Boolean 오류여부()
        {   
            큐알리딩결과 리딩결과 = 커맨드전송및응답확인(큐알동작커맨드.오류여부);
            if (리딩결과 == null) return false;
            if (!String.IsNullOrEmpty(리딩결과.결과자료.오류내용)) {
                Global.오류로그(로그영역, "통신", $"장치오류 : {리딩결과.결과자료.오류내용}", true);
                return false;
            }
            return 리딩결과.결과자료.정상여부;
        }

        public virtual Boolean 리딩가능()
        {
            큐알리딩결과 결과 = 커맨드전송및응답확인(큐알동작커맨드.리딩가능);
            if (결과 == null) return false;
            return 결과.결과자료.정상여부;
        }

        public virtual Boolean 장치리셋() => false;

        public class 큐알리딩결과
        {
            public 큐알리딩결과자료 결과자료;
            protected 큐알리더 큐알리더;

            public 큐알리딩결과(큐알리더 큐알리더, 큐알동작커맨드 커맨드, String 결과)
            {
                this.큐알리더 = 큐알리더;
                this.결과자료 = new 큐알리딩결과자료();
                this.결과자료.모델 = 큐알리더.모델;
                this.응답분석(커맨드, 결과);
            }

            public void 응답분석(큐알동작커맨드 커맨드, String 결과)
            {
                if (String.IsNullOrEmpty(결과.Trim())) {
                    this.결과자료.정상여부 = false;
                    this.결과자료.오류내용 = "수신된 정보가 없습니다.";
                    return;
                }
                //Debug.WriteLine($"{커맨드} : {결과.Trim()}", this.결과자료.모델.ToString());
                커맨드분석(커맨드, 결과);
            }

            public void 커맨드분석(큐알동작커맨드 커맨드, String 결과)
            {
                결과자료 = 큐알리더.커맨드분석(커맨드, 결과);
            }
        }

        public class 큐알리딩결과자료
        {
            public 큐알리더모델 모델;
            public 큐알동작커맨드 커맨드;
            public String 응답내용 = String.Empty;
            public String 오류내용 = String.Empty;
            public Int32 오류번호 = -1;
            public Boolean 정상여부 = true;
        }
    }
}
