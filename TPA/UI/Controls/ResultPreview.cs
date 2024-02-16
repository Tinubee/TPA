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
    public partial class ResultPreview : DevExpress.XtraEditors.XtraUserControl
    {
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
            Global.검사자료.검사완료알림 += 검사완료알림;
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
    }
}
