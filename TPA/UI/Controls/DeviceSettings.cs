using DevExpress.XtraEditors;
using MvUtils;
using System;
using TPA.Schemas;
using VDA590TPA.Schemas;

namespace TPA.UI.Controls
{
    public partial class DeviceSettings : XtraUserControl
    {
        public DeviceSettings()
        {
            InitializeComponent();
        }

        public void Init()
        {
            this.e카메라.Init();
            this.e큐알장치.Init();
            this.e기본설정.Init();
            this.b캠트리거리셋.Click += 캠트리거리셋;
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
    }
}
