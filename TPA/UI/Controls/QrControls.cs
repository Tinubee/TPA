using DevExpress.CodeParser;
using DevExpress.XtraEditors;
using MvUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TPA.Schemas;

namespace TPA.UI.Controls
{
    public partial class QrControls : XtraUserControl
    {
        private LocalizationQrControl 번역 = new LocalizationQrControl();

        public QrControls()
        {
            InitializeComponent();
        }

        public void Init()
        {
            this.SetLocalization();
            this.b큐알리딩오류1.Click += 큐알리딩오류1;
            this.b큐알리딩가능여부1.Click += 큐알리딩가능판별1;
            this.b큐알리더1장치리셋.Click += 큐알리더1장치리셋;
            this.b큐알리딩시작1.Click += 큐알리딩시작1;
            this.b큐알리딩종료1.Click += 큐알리딩종료1;
            this.g큐알통신내역1.CustomButtonClick += 큐알통신내역1_클릭;

            this.b큐알리딩오류2.Click += 큐알리딩오류2;
            this.b큐알리딩가능여부2.Click += 큐알리딩가능판별2;
            this.b큐알리더2장치리셋.Click += 큐알리더2장치리셋;
            this.b큐알리딩시작2.Click += 큐알리딩시작2;
            this.b큐알리딩종료2.Click += 큐알리딩종료2;
            this.g큐알통신내역2.CustomButtonClick += 큐알통신내역2_클릭;

            this.b큐알리딩오류3.Click += 큐알리딩오류3;
            this.b큐알리딩가능여부3.Click += 큐알리딩가능판별3;
            this.b큐알리더3장치리셋.Click += 큐알리더3장치리셋;
            this.b큐알리딩시작3.Click += 큐알리딩시작3;
            this.b큐알리딩종료3.Click += 큐알리딩종료3;
            this.g큐알통신내역3.CustomButtonClick += 큐알통신내역3_클릭;

            if (Global.환경설정.동작구분 == 동작구분.Live)
            {
                Global.큐알제어.하부큐알리더1.송신수신알림 += 큐알리더1송신수신알림;
                Global.큐알제어.하부큐알리더2.송신수신알림 += 큐알리더2송신수신알림;
                Global.큐알제어.상부큐알리더.송신수신알림 += 큐알리더3송신수신알림;
            }
        }
        private void SetLocalization()
        {
            this.b큐알리딩오류1.Text = 번역.오류상태;
            this.b큐알리딩가능여부1.Text = 번역.리딩가능;
            this.b큐알리더1장치리셋.Text = 번역.장치리셋;
            this.b큐알리딩시작1.Text = 번역.리딩시작;
            this.b큐알리딩종료1.Text = 번역.리딩종료;

            this.b큐알리딩오류2.Text = 번역.오류상태;
            this.b큐알리딩가능여부2.Text = 번역.리딩가능;
            this.b큐알리더2장치리셋.Text = 번역.장치리셋;
            this.b큐알리딩시작2.Text = 번역.리딩시작;
            this.b큐알리딩종료2.Text = 번역.리딩종료;

            this.b큐알리딩오류3.Text = 번역.오류상태;
            this.b큐알리딩가능여부3.Text = 번역.리딩가능;
            this.b큐알리더3장치리셋.Text = 번역.장치리셋;
            this.b큐알리딩시작3.Text = 번역.리딩시작;
            this.b큐알리딩종료3.Text = 번역.리딩종료;

            this.g큐알통신내역1.Text = 번역.통신내역;
            this.g큐알통신내역2.Text = 번역.통신내역;
            this.g큐알통신내역3.Text = 번역.통신내역;
        }
        public void Close()
        {

        }

        #region 큐알리더1관련메서드
        private void 큐알리딩오류1(object sender, EventArgs e) => Global.큐알제어.하부큐알리더1.오류여부();
        private void 큐알리딩가능판별1(object sender, EventArgs e) => Global.큐알제어.하부큐알리더1.리딩가능();
        private void 큐알리더1장치리셋(object sender, EventArgs e) => Global.큐알제어.하부큐알리더1.장치리셋();
        private void 큐알리딩시작1(object sender, EventArgs e) => Global.큐알제어.하부큐알리더1.리딩테스트(null, 검사항목.하부큐알코드1);
        private void 큐알리딩종료1(object sender, EventArgs e) => Global.큐알제어.하부큐알리더1.리딩종료();
        private void 큐알통신내역1_클릭(object sender, DevExpress.XtraBars.Docking2010.BaseButtonEventArgs e) => this.e큐알통신내역1.Items.Clear();
        private void 큐알리더1송신수신알림(큐알장치.통신구분 통신, 큐알동작커맨드 커맨드, String mesg)
        {
            if (this.e큐알통신내역1.InvokeRequired)
            {
                this.e큐알통신내역1.BeginInvoke(new Action(() => 큐알리더1송신수신알림(통신, 커맨드, mesg)));
                return;
            }
            this.e큐알통신내역1.Items.Insert(0, $"{Utils.FormatDate(DateTime.Now, "{0:HH:mm:ss}")}  {통신.ToString()}: {mesg}");
            this.e큐알통신내역1.SelectedIndex = 0;
            while (this.e큐알통신내역1.Items.Count > 100)
                this.e큐알통신내역1.Items.RemoveAt(this.e큐알통신내역1.Items.Count - 1);
        }
        #endregion

        #region 큐알리더2관련메서드
        private void 큐알리딩오류2(object sender, EventArgs e) => Global.큐알제어.하부큐알리더2.오류여부();
        private void 큐알리딩가능판별2(object sender, EventArgs e) => Global.큐알제어.하부큐알리더2.리딩가능();
        private void 큐알리더2장치리셋(object sender, EventArgs e) => Global.큐알제어.하부큐알리더2.장치리셋();
        private void 큐알리딩시작2(object sender, EventArgs e) => Global.큐알제어.하부큐알리더2.리딩테스트(null, 검사항목.하부큐알코드2);
        private void 큐알리딩종료2(object sender, EventArgs e) => Global.큐알제어.하부큐알리더2.리딩종료();
        private void 큐알통신내역2_클릭(object sender, DevExpress.XtraBars.Docking2010.BaseButtonEventArgs e) => this.e큐알통신내역2.Items.Clear();
        private void 큐알리더2송신수신알림(큐알장치.통신구분 통신, 큐알동작커맨드 커맨드, String mesg)
        {
            if (this.e큐알통신내역2.InvokeRequired)
            {
                this.e큐알통신내역2.BeginInvoke(new Action(() => 큐알리더2송신수신알림(통신, 커맨드, mesg)));
                return;
            }
            this.e큐알통신내역2.Items.Insert(0, $"{Utils.FormatDate(DateTime.Now, "{0:HH:mm:ss}")}  {통신.ToString()}: {mesg}");
            this.e큐알통신내역2.SelectedIndex = 0;
            while (this.e큐알통신내역2.Items.Count > 100)
                this.e큐알통신내역2.Items.RemoveAt(this.e큐알통신내역2.Items.Count - 1);
        }
        #endregion

        #region 큐알리더3관련메서드
        private void 큐알리딩오류3(object sender, EventArgs e) => Global.큐알제어.상부큐알리더.오류여부();
        private void 큐알리딩가능판별3(object sender, EventArgs e) => Global.큐알제어.상부큐알리더.리딩가능();
        private void 큐알리더3장치리셋(object sender, EventArgs e) => Global.큐알제어.상부큐알리더.장치리셋();
        private void 큐알리딩시작3(object sender, EventArgs e) => Global.큐알제어.상부큐알리더.리딩테스트(null, 검사항목.상부큐알코드1);
        private void 큐알리딩종료3(object sender, EventArgs e) => Global.큐알제어.상부큐알리더.리딩종료();
        private void 큐알통신내역3_클릭(object sender, DevExpress.XtraBars.Docking2010.BaseButtonEventArgs e) => this.e큐알통신내역3.Items.Clear();
        private void 큐알리더3송신수신알림(큐알장치.통신구분 통신, 큐알동작커맨드 커맨드, String mesg)
        {
            if (this.e큐알통신내역3.InvokeRequired)
            {
                this.e큐알통신내역3.BeginInvoke(new Action(() => 큐알리더3송신수신알림(통신, 커맨드, mesg)));
                return;
            }
            this.e큐알통신내역3.Items.Insert(0, $"{Utils.FormatDate(DateTime.Now, "{0:HH:mm:ss}")}  {통신.ToString()}: {mesg}");
            this.e큐알통신내역3.SelectedIndex = 0;
            while (this.e큐알통신내역3.Items.Count > 100)
                this.e큐알통신내역3.Items.RemoveAt(this.e큐알통신내역3.Items.Count - 1);
        }
        #endregion

        private class LocalizationQrControl
        {
            private enum Items
            {
                [Translation("Error Status", "오류상태")]
                오류상태,
                [Translation("Reading Possible", "리딩가능")]
                리딩가능,
                [Translation("Device Reset", "장치리셋")]
                장치리셋,
                [Translation("Start Reading", "리딩시작")]
                리딩시작,
                [Translation("End Reading", "리딩종료")]
                리딩종료,
                [Translation("Communication Details", "통신내역")]
                통신내역,
                //[Translation("Config", "환경설정")]
                //환경설정,
                //[Translation("Are you want to exit the program?", "프로그램을 종료하시겠습나까?")]
                //종료확인,
            }
            private String GetString(Items item) { return Localization.GetString(item); }
            public String 오류상태 { get => GetString(Items.오류상태); }
            public String 리딩가능 { get => GetString(Items.리딩가능); }
            public String 장치리셋 { get => GetString(Items.장치리셋); }
            public String 리딩시작 { get => GetString(Items.리딩시작); }
            public String 리딩종료 { get => GetString(Items.리딩종료); }
            public String 통신내역 { get => GetString(Items.통신내역); }
        }

    }
}
