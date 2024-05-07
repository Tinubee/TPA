using DevExpress.XtraEditors;
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
    public partial class ResultPreview : XtraUserControl
    {
        LocalizationResultPreview 번역 = new LocalizationResultPreview();
        public ResultPreview()
        {
            InitializeComponent();
        }

        public void Init()
        {
            this.edit하부큐알1.Text = string.Empty;
            this.edit하부큐알2.Text = string.Empty;
            this.sedit바닥평면도.Text = "0";
            this.edit역투입.Text = string.Empty;
            this.edit각인.Text = string.Empty;
            this.sedit선윤곽도F.Text = "0";
            this.sedit선윤곽도H_F.Text = "0";
            this.sedit선윤곽도H_R.Text = "0";
            this.sedit선윤곽도J_F.Text = "0";
            this.sedit선윤곽도J_R.Text = "0";
            this.edit상부큐알1.Text = string.Empty;
            this.edit상부큐알2.Text = string.Empty;
            this.edit커넥터설삽상부.Text = string.Empty;
            this.edit커넥터설삽하부.Text = string.Empty;
            this.sedit면윤곽도.Text = "0";
            this.sedit커버들뜸.Text = "0";
            라벨이름설정();
            Global.검사자료.검사완료알림 += 검사완료알림;
        }

        private void 라벨이름설정()
        {
            lb하부큐알1.Text = this.번역.하부큐알1;
            lb하부큐알2.Text = this.번역.하부큐알2;
            lb바닥평면도.Text = this.번역.바닥평면도;
            lb역투입.Text = this.번역.역투입;
            lb노멀각인.Text = this.번역.각인내용;
            lb선윤곽도F.Text = this.번역.선윤곽도_상하;
            lb선윤곽도H_F.Text = this.번역.선윤곽도_H1H2;
            lb선윤곽도H_R.Text = this.번역.선윤곽도_H3H4;
            lb선윤곽도J_F.Text = this.번역.선윤곽도_J1J2;
            lb선윤곽도J_R.Text = this.번역.선윤곽도_J3J4;
            lb상부큐알1.Text = this.번역.상부큐알1;
            lb상부큐알2.Text = this.번역.상부큐알2;
            lb커넥터설삽상부.Text = this.번역.설삽상부;
            lb커넥터설삽하부.Text = this.번역.설삽하부;
            lb면윤곽도.Text = this.번역.커버윤곽도;
            lb커버들뜸.Text = this.번역.커버들뜸;
        }

        private void 검사완료알림(검사결과 결과)
        {
            if (this.InvokeRequired) { this.BeginInvoke(new Action(() => { 검사완료알림(결과); })); return; }
            SetValue(edit하부큐알1, 결과.GetItem(검사항목.하부큐알코드1), 결과.하부큐알코드1);
            SetValue(edit하부큐알2, 결과.GetItem(검사항목.하부큐알코드2), 결과.하부큐알코드2);
            SetValue(sedit바닥평면도, 결과.GetItem(검사항목.No7_바닥평면도));
            SetValue(edit역투입, 결과.GetItem(검사항목.역투입), 결과.역투입여부);
            SetValue(edit각인, 결과.GetItem(검사항목.노멀미러), 결과.노멀미러);
            SetValue(sedit선윤곽도F, 결과.GetItem(검사항목.No8_선윤곽도_F));
            SetValue(sedit선윤곽도H_F, 결과.GetItem(검사항목.No2_선윤곽도H_F));
            SetValue(sedit선윤곽도H_R, 결과.GetItem(검사항목.No3_선윤곽도H_R));
            SetValue(sedit선윤곽도J_F, 결과.GetItem(검사항목.No10_1_선윤곽도J_F));
            SetValue(sedit선윤곽도J_R, 결과.GetItem(검사항목.No10_2_선윤곽도J_R));
            SetValue(edit상부큐알1, 결과.GetItem(검사항목.상부큐알코드1), 결과.상부큐알코드1);
            SetValue(edit상부큐알2, 결과.GetItem(검사항목.상부큐알코드2), 결과.상부큐알코드2);
            SetValue(edit커넥터설삽상부, 결과.GetItem(검사항목.커넥터설삽상부), 결과.커넥터설삽상부);
            SetValue(edit커넥터설삽하부, 결과.GetItem(검사항목.커넥터설삽하부), 결과.커넥터설삽하부);
            SetValue(sedit면윤곽도, 결과.GetItem(검사항목.면윤곽도));
            SetValue(sedit커버들뜸, 결과.GetItem(검사항목.No4_커버들뜸));
        }

        private void SetValue(SpinEdit e, 검사정보 정보)
        {
            if (정보 == null) { e.Value = 0; return; }
            e.Value = 정보.결과값;
            e.Properties.Appearance.ForeColor = 환경설정.ResultColor(정보.측정결과);
        }

        private void SetValue(TextEdit e, 검사정보 정보, Object value)
        {
            e.EditValue = value;
            if (정보 == null) return;
            e.Properties.Appearance.ForeColor = 환경설정.ResultColor(정보.측정결과);
        }

        private class LocalizationResultPreview
        {
            private enum Items
            {
                [Translation("FFC QR (Rear)", "FFC QR (Rear)")]
                하부큐알1,
                [Translation("FFC QR (Front)", "FFC QR (Rear)")]
                하부큐알2,
                [Translation("Floor plan", "바닥평면도")]
                바닥평면도,
                [Translation("Reverse input", "역투입")]
                역투입,
                [Translation("Engraving details", "각인내용")]
                각인내용,
                [Translation("Line outline diagram(Top & Bottom)", "선윤곽도(상하)")]
                선윤곽도_상하,
                [Translation("Line outline diagram(H1H2)", "선윤곽도(H1H2)")]
                선윤곽도_H1H2,
                [Translation("Line outline diagram(H3H4)", "선윤곽도(H3H4)")]
                선윤곽도_H3H4,
                [Translation("Line outline diagram(J1J2)", "선윤곽도(J1J2)")]
                선윤곽도_J1J2,
                [Translation("Line outline diagram(J3J4)", "선윤곽도(J3J4)")]
                선윤곽도_J3J4,
                [Translation("CSC QR 1", "CSC QR 1")]
                상부큐알1,
                [Translation("CSC QR 2", "CSC QR 2")]
                상부큐알2,
                [Translation("Upper part of connector", "커넥터설삽상부")]
                설삽상부,
                [Translation("Lower part of connector", "커넥터설삽하부")]
                설삽하부,
                [Translation("Cover outline diagram", "커버윤곽도")]
                커버윤곽도,
                [Translation("Cover lifting", "커버들뜸")]
                커버들뜸,
            }
            private String GetString(Items item) { return Localization.GetString(item); }

            public String 하부큐알1 { get { return GetString(Items.하부큐알1); } }
            public String 하부큐알2 { get { return GetString(Items.하부큐알2); } }
            public String 바닥평면도 { get { return GetString(Items.바닥평면도); } }
            public String 역투입 { get { return GetString(Items.역투입); } }
            public String 각인내용 { get { return GetString(Items.각인내용); } }
            public String 선윤곽도_상하 { get { return GetString(Items.선윤곽도_상하); } }
            public String 선윤곽도_H1H2 { get { return GetString(Items.선윤곽도_H1H2); } }
            public String 선윤곽도_H3H4 { get { return GetString(Items.선윤곽도_H3H4); } }
            public String 선윤곽도_J1J2 { get { return GetString(Items.선윤곽도_J1J2); } }
            public String 선윤곽도_J3J4 { get { return GetString(Items.선윤곽도_J3J4); } }
            public String 상부큐알1 { get { return GetString(Items.상부큐알1); } }
            public String 상부큐알2 { get { return GetString(Items.상부큐알2); } }
            public String 설삽상부 { get { return GetString(Items.설삽상부); } }
            public String 설삽하부 { get { return GetString(Items.설삽하부); } }
            public String 커버윤곽도 { get { return GetString(Items.커버윤곽도); } }
            public String 커버들뜸 { get { return GetString(Items.커버들뜸); } }

        }
    }
}
