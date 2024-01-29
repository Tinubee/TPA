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
        public QrControls()
        {
            InitializeComponent();
        }

        public void Init()
        {
            this.b큐알리딩오류1.Click += 큐알리딩오류1;
            this.b큐알리딩가능여부1.Click += 큐알리딩가능판별1;
            this.b큐알리더1장치리셋.Click +=  큐알리더1장치리셋;
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

            EnumToList 라벨모델 = new EnumToList(typeof(모델구분));
            라벨모델.SetLookUpEdit(this.e라벨모델);
            this.e라벨모델.EditValue = Global.환경설정.선택모델;
            this.e라벨날짜.DateTime = DateTime.Today;
            this.e라벨번호.EditValue = 9999;
            this.b라벨출력.Click += 라벨출력;
            this.b라벨부착.Click += 라벨부착;
            this.g큐알인쇄.CustomButtonClick += 큐알인쇄_CustomButtonClick;
            this.e라벨모델.CustomDisplayText += 모델CustomDisplayText;

            if (Global.환경설정.동작구분 == 동작구분.Live)
            {
                Global.큐알제어.하부큐알리더1.송신수신알림 += 큐알리더1송신수신알림;
                Global.큐알제어.하부큐알리더2.송신수신알림 += 큐알리더2송신수신알림;
                Global.큐알제어.상부큐알리더.송신수신알림 += 큐알리더3송신수신알림;
            }
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
            if (this.e큐알통신내역1.InvokeRequired) {
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
            if (this.e큐알통신내역2.InvokeRequired) {
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
            if (this.e큐알통신내역3.InvokeRequired) {
                this.e큐알통신내역3.BeginInvoke(new Action(() => 큐알리더3송신수신알림(통신, 커맨드, mesg)));
                return;
            }
            this.e큐알통신내역3.Items.Insert(0, $"{Utils.FormatDate(DateTime.Now, "{0:HH:mm:ss}")}  {통신.ToString()}: {mesg}");
            this.e큐알통신내역3.SelectedIndex = 0;
            while (this.e큐알통신내역3.Items.Count > 100)
                this.e큐알통신내역3.Items.RemoveAt(this.e큐알통신내역3.Items.Count - 1);
        }
        #endregion

        #region 큐알각인관련메서드
        private void 라벨출력(object sender, EventArgs e)
        {
            Global.큐알인쇄.자료전송(this.e라벨날짜.DateTime, (모델구분)this.e라벨모델.EditValue, MvUtils.Utils.IntValue(this.e라벨번호.EditValue));
        }

        private void 라벨부착(object sender, EventArgs e)
        {
            // org Global.제품검사수행.라벨부착수행(장치통신.PLC커맨드목록.큐알라벨부착, Utils.IntValue(this.e라벨번호.EditValue));
            Global.제품검사수행.라벨부착수행(장치통신.PLC커맨드목록.라벨발행트리거, Utils.IntValue(this.e라벨번호.EditValue));
        }
        private void 큐알인쇄_CustomButtonClick(object sender, DevExpress.XtraBars.Docking2010.BaseButtonEventArgs e)
        {
            Global.큐알인쇄.장치리셋();
        }
        private void 모델CustomDisplayText(object sender, DevExpress.XtraEditors.Controls.CustomDisplayTextEventArgs e)
        {
            if (e.Value == null) return;
            모델구분 모델 = (모델구분)e.Value;
            e.DisplayText = $"{(Int32)모델}. {MvUtils.Utils.GetDescription(모델)}";
        }
        #endregion
    }
}
