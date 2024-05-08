using DevExpress.XtraEditors;
using DevExpress.XtraRichEdit.Import.OpenXml;
using MvUtils;
using System;
using System.Windows.Forms;
using TPA.Schemas;

namespace TPA.UI.Controls
{
    public partial class Config : XtraUserControl
    {
        private LocalizationConfig 번역 = new LocalizationConfig();
        public Config()
        {
            InitializeComponent();
        }

        public void Init()
        {
            this.SetLocalization();
            this.Bind환경설정.DataSource = Global.환경설정;
            this.d기본경로.SelectedPath = Global.환경설정.기본경로;
            this.d문서저장.SelectedPath = Global.환경설정.문서저장경로;
            this.e기본경로.ButtonClick += E기본경로_ButtonClick;
            this.e문서저장.ButtonClick += E문서저장_ButtonClick;
            this.e사진저장.ButtonClick += E사진저장_ButtonClick;
            this.b설정저장.Click += B설정저장_Click;

            this.eOnly어퍼하우징.Toggled += EOnly어퍼하우징_Toggled;

            this.e유저관리.Init();
        }

        public void SetLocalization()
        {
            this.lb설정저장경로.Text = 번역.설정저장;
            this.lb문서저장경로.Text = 번역.문서경로;
            this.lb사진저장경로.Text = 번역.사진경로;
            this.lbOK이미지저장.Text = 번역.OK이미지저장;
            this.lbNG이미지저장.Text = 번역.NG이미지저장;
            this.lb검사자료보관일.Text = 번역.자료보관일;
            this.lb로그보관일.Text = 번역.로그보관일;
            this.lb검사결과자릿수.Text = 번역.결과자릿수;
            this.lb어퍼하우징검사.Text = 번역.어퍼하우징검사;
            this.lb강제OK배출.Text = 번역.강제OK;
            this.lb강제NG배출.Text = 번역.강제NG;
            this.lb강제커버조립O.Text = 번역.강제커버조립O;
            this.lb강제커버조립X.Text = 번역.강제커버조립X;
        }

        private void EOnly어퍼하우징_Toggled(object sender, EventArgs e)
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;
            Global.환경설정.Only어퍼하우징검사 = toggleSwitch.IsOn;
        }

        public void Close()
        {
            this.e유저관리.Close();
        }

        private void E기본경로_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (this.d기본경로.ShowDialog() == DialogResult.OK)
                this.e기본경로.Text = this.d기본경로.SelectedPath;
        }

        private void E문서저장_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (this.d문서저장.ShowDialog() == DialogResult.OK)
                this.e문서저장.Text = this.d문서저장.SelectedPath;
        }

        private void E사진저장_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (this.d사진저장.ShowDialog() == DialogResult.OK)
                this.e사진저장.Text = this.d사진저장.SelectedPath;
        }

        private void B설정저장_Click(object sender, EventArgs e)
        {

        }

        private class LocalizationConfig
        {
            private enum Items
            {
                [Translation("Setting Save Path", "설정 저장 경로")]
                기본경로,
                [Translation("Document Save Path", "문서 저장 경로")]
                문서경로,
                [Translation("Image Save Path", "사진 저장 경로")]
                사진경로,
                [Translation("OK Image Save", "OK 이미지 저장")]
                OK이미지저장,
                [Translation("NG Image Save", "NG 이미지 저장")]
                NG이미지저장,
                [Translation("InspectData Storage Date", "검사 자료 보관일")]
                자료보관일,
                [Translation("LogData Storage Date", "로그 자료 보관일")]
                로그보관일,
                [Translation("InspectData Decimal Places", "검사결과 자릿수")]
                결과자릿수,
                [Translation("Housing Inspection", "어퍼하우징 검사")]
                어퍼하우징검사,
                [Translation("Forced OK", "강제 OK 배출")]
                강제OK,
                [Translation("Forced NG", "강제 NG 배출")]
                강제NG,
                [Translation("Forced Cover Assembly O", "강제 커버조립 O")]
                강제커버조립O,
                [Translation("Forced Cover Assembly X", "강제 커버조립 X")]
                강제커버조립X,
                [Translation("Save", "설정저장")]
                설정저장,
                [Translation("It's saved.", "저장되었습니다.")]
                저장완료,
                [Translation("Save your preferences?", "환경설정을 저장하시겠습니까?")]
                저장확인,
            }
            private String GetString(Items item) { return Localization.GetString(item); }
            public String 기본경로 { get => GetString(Items.기본경로); }
            public String 문서경로 { get => GetString(Items.문서경로); }
            public String 사진경로 { get => GetString(Items.사진경로); }
            public String OK이미지저장 { get => GetString(Items.OK이미지저장); }
            public String NG이미지저장 { get => GetString(Items.NG이미지저장); }
            public String 자료보관일 { get => GetString(Items.자료보관일); }
            public String 로그보관일 { get => GetString(Items.로그보관일); }
            public String 결과자릿수 { get => GetString(Items.결과자릿수); }
            public String 어퍼하우징검사 { get => GetString(Items.어퍼하우징검사); }
            public String 강제OK { get => GetString(Items.강제OK); }
            public String 강제NG { get => GetString(Items.강제NG); }
            public String 강제커버조립O { get => GetString(Items.강제커버조립O); }
            public String 강제커버조립X { get => GetString(Items.강제커버조립X); }
            public String 설정저장 { get => GetString(Items.설정저장); }
            public String 저장완료 { get => GetString(Items.저장완료); }
            public String 저장확인 { get => GetString(Items.저장확인); }
        }

        private void e강제OK배출_Toggled(object sender, EventArgs e)
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;
            Global.환경설정.강제OK배출여부 = toggleSwitch.IsOn;
        }

        private void e강제NG배출_Toggled(object sender, EventArgs e)
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;
            Global.환경설정.강제NG배출여부 = toggleSwitch.IsOn;
        }

        private void e강제커버조립O_Toggled(object sender, EventArgs e)
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;
            Global.환경설정.강제커버조립O = toggleSwitch.IsOn;
        }

        private void e강제커버조립X_Toggled(object sender, EventArgs e)
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;
            Global.환경설정.강제커버조립X = toggleSwitch.IsOn;
        }
    }
}
