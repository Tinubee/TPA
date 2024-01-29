using DevExpress.CodeParser;
using DevExpress.Diagram.Core.Layout;
using MvUtils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPA.Schemas
{
    public class SR710 : 큐알리더
    {
        public SR710(큐알리더모델 모델 = 큐알리더모델.SR710) : base(모델)
        {
        }

        public override 큐알리딩결과자료 커맨드분석(큐알동작커맨드 커맨드, String 결과)
        {
            SR710리딩결과자료 분석자료 = new SR710리딩결과자료();
            분석자료.커맨드 = 커맨드;
            분석자료.응답내용 = 결과;
            try {
                if (커맨드 == 큐알동작커맨드.리딩시작) {
                    if (결과.Contains("ER")) {
                        분석자료.정상여부 = false;
                        분석자료.오류내용 = "장치리딩 커맨드 실패";
                    }
                    else {
                        String[] r = 분석자료.응답내용.Split(";".ToCharArray());
                        분석자료.생산번호 = r[0];
                        분석자료.생산일자 = r[1];
                    }
                }
                else if (커맨드 == 큐알동작커맨드.리딩종료)
                {
                    if (결과.Contains("ER"))
                    {
                        분석자료.정상여부 = false;
                        분석자료.오류내용 = "장치종료 커맨드 실패";
                    }
                }
                else if (커맨드 == 큐알동작커맨드.리더리셋)
                {
                    if (결과.Contains("ER"))
                    {
                        분석자료.정상여부 = false;
                        분석자료.오류내용 = "장치리셋 커맨드 실패";
                    }
                }
                else {
                    String[] r = 분석자료.응답내용.Split(";".ToCharArray());
                    if (r.Length > 2) {
                        if (r[0] != "OK" && r[2] != "none") {
                            분석자료.정상여부 = false;
                            분석자료.오류내용 = "장치에 오류가 있습니다.";
                        }
                    }
                }
            }
            catch (Exception ex) {
                분석자료.정상여부 = false;
                분석자료.오류내용 = ex.Message;
            }
            return 분석자료;
        }

        public override bool 장치리셋()
        {
            if (!this.커맨드전송(큐알동작커맨드.리더리셋)) {
                Global.오류로그("큐알리더(SR-710)", "통신", "장치리셋 커맨드전송 실패", true);
                return false;
            }
            큐알리딩결과 결과 = this.커맨드수신(큐알동작커맨드.리더리셋);
            if (결과.결과자료.정상여부 == false)
            {
                Global.오류로그("큐알리더(SR-710)", "통신", $"장치리셋 커맨드수신 실패 {결과.결과자료.오류내용})", true);
                return false;
            }
            Global.정보로그("큐알리더(SR-710)", "장치리셋", $"장치리셋 완료 {결과.결과자료.모델.ToString()}", false);
            return true;
        }

        public static String 오류내용확인(Int32 오류번호)
        {
            if (오류번호 < 0) return String.Empty;
            if (오류번호 == 0) return $"[{오류번호}] 정의되지 않은 커맨드의 수신";
            if (오류번호 == 1) return $"[{오류번호}] 커맨드의 포맷이 다릅니다.";
            if (오류번호 == 2) return $"[{오류번호}] 파라미터1의 설정 범위를 초과했습니다.";
            if (오류번호 == 3) return $"[{오류번호}] 파라미터2의 설정 범위를 초과했습니다.";
            if (오류번호 == 4) return $"[{오류번호}] 파라미터2가 HEX(16진) 코드로 지정되지 않았습니다.";
            if (오류번호 == 5) return $"[{오류번호}] 파라미터2가 HEX(16진) 코드이나 설정 범위를 초과했습니다.";
            if (오류번호 == 10) return $"[{오류번호}] 프리셋 데이터가 올바르지 않습니다.";
            if (오류번호 == 11) return $"[{오류번호}] 영역 지정 데이터가 올바르지 않습니다.";
            if (오류번호 == 12) return $"[{오류번호}] 지정 파일이 존재하지 않습니다.";
            if (오류번호 == 13) return $"[{오류번호}] %Tmm-LON,bb 커맨드의 mm가 31을 초과했습니다.";
            if (오류번호 == 14) return $"[{오류번호}] %Tmm-KEYENCE 커맨드로 통신을 확인할 수 없습니다.";
            if (오류번호 == 20) return $"[{오류번호}] 현재 상태에서는 실행이 허가되지 않은 커맨드(실행 에러)";
            if (오류번호 == 21) return $"[{오류번호}] 버퍼 오버 중이므로 커맨드를 실행할 수 없습니다.";
            if (오류번호 == 22) return $"[{오류번호}] 파라미터의 로드, 세이브 중에 에러가 발생해서 커맨드를 실행할 수 없습니다.";
            if (오류번호 == 23) return $"[{오류번호}] AutoID Network Navigator와 접속 중이어서 RS-232C에서의 커맨드를 수신할 수 없습니다.";
            if (오류번호 == 99) return $"[{오류번호}] SR 시리즈의 이상으로 생각됩니다. KEYENCE로 연락해 주십시오.";
            return $"[{오류번호}] 알수없는 오류가 발생하였습니다.";
        }

        public class SR710리딩결과 : 큐알리딩결과
        {
            public new SR710리딩결과자료 결과자료;

            public SR710리딩결과(큐알리더 큐알리더, 큐알동작커맨드 커맨드, String 결과) : base(큐알리더, 커맨드, 결과)
            {
                this.큐알리더 = (SR710)base.큐알리더;
                this.결과자료 = new SR710리딩결과자료();
                this.결과자료.모델 = 큐알리더.모델;
                this.응답분석(커맨드, 결과);
            }
        }

        public class SR710리딩결과자료 : 큐알리딩결과자료
        {
            public String 생산일자 = String.Empty;
            public String 생산번호 = String.Empty;
        }
    }

    public class SR2000 : 큐알리더
    {
        public Int32 큐알생성수 { get; set; }

        public SR2000(Int32 큐알생성수, 큐알리더모델 모델 = 큐알리더모델.SR2000) : base(모델)
        {
            this.큐알생성수 = 큐알생성수;
        }

        public override 큐알리딩결과 커맨드전송및응답확인(큐알동작커맨드 커맨드, Int32 대기시간 = 500) => this.커맨드전송및응답확인(커맨드, Utils.GetDescription(커맨드), 대기시간);

        public override 큐알리딩결과 커맨드전송및응답확인(큐알동작커맨드 커맨드, String command, Int32 대기시간 = 500)
        {
            if (!커맨드전송(커맨드, command, 대기시간))
            {
                Global.오류로그(로그영역, "통신", $"커맨드전송에러", true);
                return null;
            }
            SR2000리딩결과 결과 = (SR2000리딩결과)this.커맨드수신(커맨드, 대기시간);
            if (!결과.결과자료.정상여부 && String.IsNullOrEmpty(결과.결과자료.오류내용))
            {
                String error = String.IsNullOrEmpty(결과.결과자료.오류내용) ? "커맨드전송실패" : 결과.결과자료.오류내용;
                Global.오류로그(로그영역, "통신", $"커맨드수행에러 : {error}", true);
            }
            return 결과;
        }

        public override 큐알리딩결과 커맨드수신(큐알동작커맨드 커맨드, Int32 대기시간 = 500)
        {
            String 수신데이터 = this.ReceiveData(대기시간);
            송신수신알림발생(통신구분.RX, 커맨드, 수신데이터.Trim());
            SR2000리딩결과 결과 = new SR2000리딩결과(this, 커맨드, 수신데이터);
            return 결과;
        }

        public override 큐알리딩결과자료 커맨드분석(큐알동작커맨드 커맨드, string 결과)
        {
            SR2000리딩결과자료 분석자료 = new SR2000리딩결과자료();
            분석자료.커맨드 = 커맨드;
            분석자료.응답내용 = 결과;
            분석자료.큐알별내용 = new String[this.큐알생성수];

            try {
                if (커맨드 == 큐알동작커맨드.리딩시작) {
                    if (결과.Contains("ER"))
                    {
                        분석자료.정상여부 = false;
                        분석자료.오류내용 = "장치리딩 커맨드 실패";
                    }
                    else {
                        String[] r = 분석자료.응답내용.Split(",".ToCharArray());
                        for (Int32 인덱스 = 0; 인덱스 < r.Length; 인덱스++) {
                            분석자료.큐알별내용[인덱스] = r[인덱스];
                        }
                    }
                }
                else if (커맨드 == 큐알동작커맨드.리딩종료) {
                    if (결과.Contains("ER"))
                    {
                        분석자료.정상여부 = false;
                        분석자료.오류내용 = "장치종료 커맨드 실패";
                    }
                }
                else if (커맨드 == 큐알동작커맨드.리더리셋) {
                    if (결과.Contains("ERR")) {
                        분석자료.정상여부 = false;
                        분석자료.오류내용 = "장치리셋 커맨드 실패";
                    }
                }
                else {
                    String[] r = 분석자료.응답내용.Split(";".ToCharArray());
                    if (r.Length > 2) {
                        if (r[0] != "OK" && r[2] != "none") {
                            분석자료.정상여부 = false;
                            분석자료.오류내용 = "장치에 오류가 있습니다.";
                        }
                    }
                }
            }
            catch (Exception ex) {
                분석자료.정상여부 = false;
                분석자료.오류내용 = ex.Message;
            }
            return 분석자료;
        }

        public override bool 장치리셋()
        {
            if (!this.커맨드전송(큐알동작커맨드.리더리셋)) {
                Global.오류로그("큐알리더(SR-2000)", "통신", "장치리셋 커맨드전송 실패", true);
                return false;
            }
            큐알리딩결과 결과 = this.커맨드수신(큐알동작커맨드.리더리셋);
            if (결과.결과자료.정상여부 == false) {
                Global.오류로그("큐알리더(SR-2000)", "통신", $"장치리셋 커맨드수신 실패 ({결과.결과자료.오류내용})", true);
                return false;
            }
            Global.정보로그("큐알리더(SR-2000)", "장치리셋", $"장치리셋 완료 {결과.결과자료.모델.ToString()}", false);
            return true;
        }

        public static String 오류내용확인(Int32 오류번호)
        {
            if (오류번호 < 0) return String.Empty;
            if (오류번호 == 0) return $"[{오류번호}] 정의되지 않은 커맨드의 수신";
            if (오류번호 == 1) return $"[{오류번호}] 커맨드의 포맷이 다릅니다.";
            if (오류번호 == 2) return $"[{오류번호}] 파라미터1의 설정 범위를 초과했습니다.";
            if (오류번호 == 3) return $"[{오류번호}] 파라미터2의 설정 범위를 초과했습니다.";
            if (오류번호 == 4) return $"[{오류번호}] 파라미터2가 HEX(16진) 코드로 지정되지 않았습니다.";
            if (오류번호 == 5) return $"[{오류번호}] 파라미터2가 HEX(16진) 코드이나 설정 범위를 초과했습니다.";
            if (오류번호 == 10) return $"[{오류번호}] 프리셋 데이터가 올바르지 않습니다.";
            if (오류번호 == 11) return $"[{오류번호}] 영역 지정 데이터가 올바르지 않습니다.";
            if (오류번호 == 12) return $"[{오류번호}] 지정 파일이 존재하지 않습니다.";
            if (오류번호 == 13) return $"[{오류번호}] %Tmm-LON,bb 커맨드의 mm 설정 범위를 초과했습니다.";
            if (오류번호 == 14) return $"[{오류번호}] %Tmm-KEYENCE 커맨드로 통신을 확인할 수 없습니다.";
            if (오류번호 == 20) return $"[{오류번호}] 현재 상태에서는 실행이 허가되지 않은 커맨드(실행 에러)";
            if (오류번호 == 21) return $"[{오류번호}] 버퍼 오버 중이므로 커맨드를 실행할 수 없습니다.";
            if (오류번호 == 22) return $"[{오류번호}] 파라미터의 로드, 세이브 중에 에러가 발생해서 커맨드를 실행할 수 없습니다.";
            if (오류번호 == 23) return $"[{오류번호}] AutoID Network Navigator와 접속 중이어서 RS-232C에서의 커맨드를 수신할 수 없습니다.";
            if (오류번호 == 99) return $"[{오류번호}] SR 시리즈의 이상으로 생각됩니다. KEYENCE로 연락해 주십시오.";
            return $"[{오류번호}] 알수없는 오류가 발생하였습니다.";
        }

        public class SR2000리딩결과 : 큐알리딩결과
        {
            public new SR2000리딩결과자료 결과자료;

            public SR2000리딩결과(큐알리더 큐알리더, 큐알동작커맨드 커맨드, String 결과) : base(큐알리더, 커맨드, 결과)
            {
                this.큐알리더 = (SR2000)base.큐알리더;
                this.결과자료 = new SR2000리딩결과자료();
                this.결과자료.모델 = base.큐알리더.모델;
                this.응답분석(커맨드, 결과);
            }

            public new void 응답분석(큐알동작커맨드 커맨드, String 결과)
            {
                if (String.IsNullOrEmpty(결과.Trim()))
                {
                    this.결과자료.정상여부 = false;
                    this.결과자료.오류내용 = "수신된 정보가 없습니다.";
                    return;
                }
                this.커맨드분석(커맨드, 결과);
            }

            public new void 커맨드분석(큐알동작커맨드 커맨드, String 결과)
            {
                this.결과자료 = (SR2000리딩결과자료)this.큐알리더.커맨드분석(커맨드, 결과);
            }
        }

        public class SR2000리딩결과자료 : 큐알리딩결과자료
        {
            public String[] 큐알별내용;
        }
    }
}
