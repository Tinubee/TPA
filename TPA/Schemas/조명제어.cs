using DevExpress.Utils.Extensions;
using MvUtils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace TPA.Schemas
{
    public class 조명제어 : BindingList<조명정보>
    {
        public String 로그영역 = "조명제어";
        [JsonIgnore]
        private string 저장파일 { get { return Path.Combine(Global.환경설정.기본경로, "Lights.json"); } }
        public Boolean 정상여부 { get { return this.측면조명.IsOpen() && this.상부조명.IsOpen() && this.하부조명.IsOpen(); } }
        public Boolean 초기점멸작업완료여부 = false; // Task로 동작하기 때문에 WaitForm이 먼저 종료되어 추가한 플래그

        private 조명장치 측면조명;
        private 조명장치 상부조명;
        private 조명장치 하부조명;
        private 조명장치 커넥터조명;

        public void Init()
        {
            // org this.측면조명 = new LCP30DC() { 포트 = 조명포트.COM5 };
            this.측면조명 = new LCP100DC() { 포트 = 조명포트.COM5, 통신속도 = 19200 };
            this.상부조명 = new LCP200QC() { 포트 = 조명포트.COM3 };
            this.하부조명 = new LCP100DC() { 포트 = 조명포트.COM4, 통신속도 = 19200 };
            this.커넥터조명 = new LCP24_30Q() { 포트 = 조명포트.COM2 };

            this.측면조명.Init();
            this.상부조명.Init();
            this.하부조명.Init();
            this.커넥터조명.Init();

            this.Add(new 조명정보(카메라구분.Cam01, 측면조명) { 채널 = 조명채널.CH02, 밝기 = (int)(측면조명.최대밝기) });
            this.Add(new 조명정보(카메라구분.Cam02, 측면조명) { 채널 = 조명채널.CH03, 밝기 = (int)(측면조명.최대밝기) });
            this.Add(new 조명정보(카메라구분.Cam03, 상부조명) { 채널 = 조명채널.CH01, 밝기 = (int)(상부조명.최대밝기) });
            this.Add(new 조명정보(카메라구분.Cam03, 상부조명) { 채널 = 조명채널.CH02, 밝기 = (int)(상부조명.최대밝기) });
            this.Add(new 조명정보(카메라구분.Cam03, 상부조명) { 채널 = 조명채널.CH03, 밝기 = (int)(상부조명.최대밝기) });
            this.Add(new 조명정보(카메라구분.Cam03, 상부조명) { 채널 = 조명채널.CH04, 밝기 = (int)(상부조명.최대밝기) });
            this.Add(new 조명정보(카메라구분.Cam04, 하부조명) { 채널 = 조명채널.CH02, 밝기 = (int)(하부조명.최대밝기) });
            this.Add(new 조명정보(카메라구분.Cam05, 하부조명) { 채널 = 조명채널.CH03, 밝기 = (int)(하부조명.최대밝기) });
            this.Add(new 조명정보(카메라구분.Cam06, 커넥터조명) { 채널 = 조명채널.CH01, 밝기 = (int)(커넥터조명.최대밝기) });
            this.Add(new 조명정보(카메라구분.Cam07, 커넥터조명) { 채널 = 조명채널.CH02, 밝기 = (int)(커넥터조명.최대밝기) });

            this.Load();
            if (Global.환경설정.동작구분 != 동작구분.Live) return;
            this.Open();
            Task.Run(() => { this.Set(); });
        }

        public 조명정보 GetItem(카메라구분 카메라)
        {
            foreach (조명정보 조명 in this)
                if (조명.카메라 == 카메라) return 조명;
            return null;
        }
        public 조명정보 GetItem(카메라구분 카메라, 조명포트 포트, 조명채널 채널)
        {
            foreach (조명정보 조명 in this)
                if (조명.카메라 == 카메라 && 조명.포트 == 포트 && 조명.채널 == 채널) return 조명;
            return null;
        }

        public void Load()
        {
            if (!File.Exists(this.저장파일)) return;
            try
            {
                List<조명정보> 자료 = JsonConvert.DeserializeObject<List<조명정보>>(File.ReadAllText(this.저장파일), Utils.JsonSetting());
                foreach (조명정보 정보 in 자료)
                {
                    조명정보 조명 = this.GetItem(정보.카메라, 정보.포트, 정보.채널);
                    if (조명 == null) continue;
                    조명.Set(정보);
                }
            }
            catch (Exception ex)
            {
                Global.오류로그(로그영역, "자료로드", $"조명 설정 로드 실패 : {ex.Message}", true);
            }
        }

        public void Save()
        {
            if (!Utils.WriteAllText(저장파일, JsonConvert.SerializeObject(this, Utils.JsonSetting())))
            {
                Global.오류로그(로그영역, "자료저장", "조명 설정 저장 실패", true);
            }
        }

        public void Open()
        {
            if (!this.측면조명.Open())
            {
                this.측면조명.Close();
                Global.오류로그(로그영역, "Open", "측면조명에 연결할 수 없습니다.", true);
            }
            if (!this.상부조명.Open())
            {
                this.상부조명.Close();
                Global.오류로그(로그영역, "Open", "상부조명에 연결할 수 없습니다.", true);
            }
            if (!this.하부조명.Open())
            {
                this.하부조명.Close();
                Global.오류로그(로그영역, "Open", "하부조명에 연결할 수 없습니다.", true);
            }
            if (!this.커넥터조명.Open())
            {
                this.커넥터조명.Close();
                Global.오류로그(로그영역, "Open", "커넥터조명에 연결할 수 없습니다.", true);
            }
        }

        public void Close()
        {
            if (Global.환경설정.동작구분 != 동작구분.Live) return;
            this.TurnOff();
            Task.Delay(100).Wait();
            this.측면조명?.Close();
            this.상부조명?.Close();
            this.하부조명?.Close();
            this.커넥터조명?.Close();
        }

        public void Set()
        {
            //this.TurnOff();
            초기점멸작업완료여부 = true;
            //Task.Run(() =>
            //{
            foreach (조명정보 조명 in this)
            {
                if (!조명.Set()) 조명.TurnOn();
                Task.Delay(200).Wait();
                조명.TurnOff();
                Task.Delay(200).Wait();
            }
            초기점멸작업완료여부 = true;
            //Debug.WriteLine("조명끝");
            //});
        }

        public void Set(카메라구분 카메라)
        {
            foreach (조명정보 조명 in this)
            {
                if (조명.카메라 == 카메라)
                    조명.Set();
            }
        }

        public void Set(카메라구분 카메라, 조명포트 포트, Int32 밝기)
        {
            foreach (조명정보 정보 in this)
            {
                if (정보.카메라 == 카메라 && 정보.포트 == 포트)
                {
                    정보.밝기 = 밝기;
                    정보.Set();
                }
            }
        }

        public void TurnOn() => this.ForEach(e => e.TurnOn());
        public void TurnOff() => this.ForEach(e => e.TurnOff());


        public void TurnOnOff(카메라구분 카메라, Boolean IsOn)
        {
            if (IsOn) this.TurnOn(카메라);
            else this.TurnOff(카메라);
        }

        public void TurnOn(카메라구분 카메라)
        {
            foreach (조명정보 정보 in this)
                if (정보.카메라 == 카메라)
                    정보.TurnOn();
        }

        public void TurnOff(카메라구분 카메라)
        {
            foreach (조명정보 정보 in this)
                if (정보.카메라 == 카메라)
                    정보.TurnOff();
        }
    }
}
