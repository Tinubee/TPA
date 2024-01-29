using MvUtils;
using Cognex.VisionPro.ToolBlock;
using Cognex.VisionPro.ToolGroup;
using System;
using System.Windows.Forms;
using TPA.Schemas;
using System.Diagnostics;

namespace TPA.UI.Forms
{
    public partial class CogToolEdit : DevExpress.XtraBars.ToolbarForm.ToolbarForm
    {
        public CogToolEdit()
        {
            InitializeComponent();
            this.LookAndFeel.SetSkinStyle(Global.SkinName);
            this.LookAndFeel.SetSkinStyle(Global.ColorPalette);
            this.FormClosed += CogToolEdit_FormClosed;
            this.FormClosing += CogToolEdit_FormClosing;
            this.b사진열기.ItemClick += 이미지로드;
            this.b마스터로드.ItemClick += 마스터로드;
            this.b마스터저장.ItemClick += 마스터저장;
            this.b설정저장.ItemClick += 설정저장;
        }

        private const String 로그영역 = "비젼도구";
        private CogToolGroupEditV2 CogControl = null;
        비전도구 검사도구 = null;

        private void CogToolEdit_FormClosed(object sender, FormClosedEventArgs e) => this.CogControl?.Dispose();

        private void CogToolEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            String paletteName = String.IsNullOrEmpty(Properties.Settings.Default.SvgPaletteName) ? Global.BlackPalette : Properties.Settings.Default.SvgPaletteName;
            this.LookAndFeel.SetSkinStyle(paletteName);
        }

        public void Init(비전도구 도구)
        {
            this.검사도구 = 도구;
            this.Text = 도구.도구명칭;
            this.CogControl = new CogToolBlockEditV2() { Subject = this.검사도구.ToolBlock, Dock = DockStyle.Fill };
            this.Controls.Add(this.CogControl);
        }

        private Boolean 수행가능()
        {
            if (Global.장치상태.자동수동)
                return Utils.WarningMsg("자동모드에서는 수행하실 수 없습니다.");
            return !Global.장치상태.자동수동;
        }

        private void 이미지로드(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!수행가능()) return;
            this.검사도구.이미지로드();
        }

        private void 마스터로드(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!수행가능()) return;
            this.검사도구.마스터로드();
        }

        private void 마스터저장(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!MvUtils.Utils.Confirm("현재 이미지를 마스터로 저장하시겠습니까?")) return;
            if (this.검사도구.마스터저장()) Global.정보로그(로그영역, "마스터 저장", $"저장하였습니다.", true);
        }

        private void 설정저장(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!MvUtils.Utils.Confirm("비전도구 설정을 저장하시겠습니까?")) return;
            try { this.검사도구.Save(); }
            catch (Exception ex) {
                Global.오류로그(로그영역, "저장오류", $"오류가 발생하였습니다.\n{ex.Message}", this);
            }
        }
    }
}