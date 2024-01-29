using ActUtlType64Lib;
using DevExpress.XtraPrinting.Native;
using MvUtils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TPA.Schemas
{
    public partial class 장치통신
    {
        public delegate void BaseEvent();
        public event BaseEvent 동작상태알림;
        public event BaseEvent 통신상태알림;
        public delegate void 제품검사수행델리게이트(PLC커맨드목록 커맨드, Int32 제품인덱스);
        public event 제품검사수행델리게이트 제품검사수행이벤트;

        public void Init()
        {
            this.PLC = new ActUtlType64();
            if (Global.환경설정.동작구분 == 동작구분.Live) {
                this.PLC커맨드.Init(new Action<PLC커맨드목록, Int32>((커맨드주소, 커맨드값) => PLC커맨드전송(커맨드주소, 커맨드값)));
            }
            else {
                this.PLC커맨드.Init(null);
            }
        }

        public void Close()
        {
            this.Stop();
        }

        public void Start()
        {
            if (this.작업여부)  return; 
            this.작업여부 = true; 
            this.정상여부 = true; 
            this.시작일시 = DateTime.Now;
            if (Global.환경설정.동작구분 == 동작구분.Live) {
                this.PLC커맨드정보갱신();
                this.PLC커맨드정보초기화();
                this.검사위치별제품인덱스버퍼초기화();
                this.동작상태알림?.Invoke();
            }
            new Thread(장치통신작업) { Priority = ThreadPriority.Highest }.Start();
        }

        private void 장치통신작업()
        {
            Global.정보로그(로그영역, "PLC 통신", $"통신을 시작합니다.", false);
            while (this.작업여부) {
                PLC커맨드자료분석();
                Thread.Sleep(PLC통신주기);
            }

            Global.정보로그(로그영역, "PLC 통신", $"통신이 종료되었습니다.", false);
            this.연결종료();
        }

        public void Stop()
        {
            this.작업여부 = false;
            Global.정보로그(로그영역, "PLC 통신", $"통신을 중단합니다.", false);
        }

        public Boolean Open()
        {
            this.정상여부 = (PLC.Open() == 0);
            return this.정상여부;
        }

        private void 연결종료()
        {
            try {
                PLC.Close();
                Global.정보로그(로그영역, "PLC 연결종료", $"서버에 연결을 종료하였습니다.", false);
            }
            catch (Exception ex) {
                Global.오류로그(로그영역, "PLC 연결종료", $"서버 연결을 종료하는 중 오류가 발생하였습니다.\r\n{ex.Message}", false);
            }
        }
    }
}
