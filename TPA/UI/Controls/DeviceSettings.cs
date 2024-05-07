using DevExpress.XtraEditors;
using MvUtils;
using System;
using TPA.Schemas;
using VDA590TPA.Schemas;

namespace TPA.UI.Controls
{
    public partial class DeviceSettings : XtraUserControl
    {
        private LocalizationDeviceSetting 번역 = new LocalizationDeviceSetting();

        public DeviceSettings()
        {
            InitializeComponent();
        }

        public void Init()
        {
            this.SetLocalization();
            this.e카메라.Init();
            this.e큐알장치.Init();
            this.e기본설정.Init();
            this.b캠트리거리셋.Click += 캠트리거리셋;
        }
        private void SetLocalization()
        {
            this.t기타.Text = 번역.기타;
            this.t환경설정.Text = 번역.환경설정;
            this.b캠트리거리셋.Text = 번역.카메라트리거리셋;
        }
        public void Close()
        {
            this.e카메라.Close();
            this.e큐알장치.Close();
            this.e기본설정.Close();
        }

        private void 캠트리거리셋(object sender, EventArgs e)
        {
            if (!Utils.Confirm("트리거 보드의 위치를 초기화 하시겠습니까?")) return;
            Enc852 트리거보드 = new Enc852(Global.환경설정.트리거보드포트);
            트리거보드.Init();
            트리거보드.Close();
            Global.정보로그("트리거보드", "초기화", "초기화 되었습니다.", true);
        }

        private class LocalizationDeviceSetting
        {
            private enum Items
            {
                [Translation("Other", "기타")]
                기타,
                [Translation("Config", "환경설정")]
                환경설정,
                [Translation("Camera Trigger Reset", "카메라 트리거 리셋")]
                카메라트리거리셋,
                [Translation("Are you want to exit the program?", "프로그램을 종료하시겠습나까?")]
                종료확인,
            }
            private String GetString(Items item) { return Localization.GetString(item); }

            public String 타이틀 { get => Localization.제목.GetString(); }
            public String 기타 { get => GetString(Items.기타); }
            public String 환경설정 { get => GetString(Items.환경설정); }
            public String 종료확인 { get => GetString(Items.종료확인); }
            public String 카메라트리거리셋 { get => GetString(Items.카메라트리거리셋); }
        }
    }
}
