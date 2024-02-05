namespace TPA
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tabFormControl1 = new DevExpress.XtraBars.TabFormControl();
            this.타이틀 = new DevExpress.XtraBars.BarStaticItem();
            this.e프로젝트 = new DevExpress.XtraBars.BarStaticItem();
            this.skinPaletteDropDownButtonItem1 = new DevExpress.XtraBars.SkinPaletteDropDownButtonItem();
            this.p검사하기 = new DevExpress.XtraBars.TabFormPage();
            this.tabFormContentContainer1 = new DevExpress.XtraBars.TabFormContentContainer();
            this.e결과뷰어 = new TPA.UI.Controls.CamViewer();
            this.e상태뷰어 = new TPA.UI.Controls.State();
            this.그랩뷰어 = new DevExpress.XtraBars.TabFormPage();
            this.tabFormContentContainer2 = new DevExpress.XtraBars.TabFormContentContainer();
            this.p검사내역 = new DevExpress.XtraBars.TabFormPage();
            this.tabFormContentContainer3 = new DevExpress.XtraBars.TabFormContentContainer();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.e검사내역 = new TPA.UI.Controls.Result();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.e검사피봇 = new TPA.UI.Controls.ResultPivot();
            this.p환경설정 = new DevExpress.XtraBars.TabFormPage();
            this.tabFormContentContainer4 = new DevExpress.XtraBars.TabFormContentContainer();
            this.t환경설정 = new DevExpress.XtraTab.XtraTabControl();
            this.t검사설정 = new DevExpress.XtraTab.XtraTabPage();
            this.e검사설정 = new TPA.UI.Controls.SetInspection();
            this.t변수설정 = new DevExpress.XtraTab.XtraTabPage();
            this.t장치설정 = new DevExpress.XtraTab.XtraTabPage();
            this.e장치설정 = new TPA.UI.Controls.DeviceSettings();
            this.t큐알검증 = new DevExpress.XtraTab.XtraTabPage();
            this.e큐알검증 = new TPA.UI.Controls.QrValidate();
            this.t로그내역 = new DevExpress.XtraTab.XtraTabPage();
            this.e로그내역 = new TPA.UI.Controls.LogViewer();
            this.t디버깅용페이지 = new DevExpress.XtraBars.TabFormPage();
            this.tabFormContentContainer5 = new DevExpress.XtraBars.TabFormContentContainer();
            this.lbK1 = new DevExpress.XtraEditors.LabelControl();
            this.b커버센서읽기 = new DevExpress.XtraEditors.SimpleButton();
            this.bForceAutoMode = new DevExpress.XtraEditors.SimpleButton();
            this.bTriggerLow = new DevExpress.XtraEditors.SimpleButton();
            this.bTrigger커버들뜸 = new DevExpress.XtraEditors.SimpleButton();
            this.bTrigger상부큐알 = new DevExpress.XtraEditors.SimpleButton();
            this.bTrigger바닥평면 = new DevExpress.XtraEditors.SimpleButton();
            this.b모든CpltLow = new DevExpress.XtraEditors.SimpleButton();
            this.bTrigger하부큐알 = new DevExpress.XtraEditors.SimpleButton();
            this.bBusyLow = new DevExpress.XtraEditors.SimpleButton();
            this.ePLC모니터 = new TPA.UI.Controls.PLCMonitor();
            this.e제품수행이벤트발생 = new DevExpress.XtraEditors.SimpleButton();
            this.e모델정보뷰어 = new TPA.UI.Controls.ModelSettingViewer();
            this.e셋팅뷰어 = new TPA.UI.Controls.EnvSettingViewer();
            ((System.ComponentModel.ISupportInitialize)(this.tabFormControl1)).BeginInit();
            this.tabFormContentContainer1.SuspendLayout();
            this.tabFormContentContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            this.xtraTabPage2.SuspendLayout();
            this.tabFormContentContainer4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.t환경설정)).BeginInit();
            this.t환경설정.SuspendLayout();
            this.t검사설정.SuspendLayout();
            this.t장치설정.SuspendLayout();
            this.t큐알검증.SuspendLayout();
            this.t로그내역.SuspendLayout();
            this.tabFormContentContainer5.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabFormControl1
            // 
            this.tabFormControl1.AllowMoveTabs = false;
            this.tabFormControl1.AllowMoveTabsToOuterForm = false;
            this.tabFormControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.타이틀,
            this.e프로젝트,
            this.skinPaletteDropDownButtonItem1});
            this.tabFormControl1.Location = new System.Drawing.Point(0, 0);
            this.tabFormControl1.Name = "tabFormControl1";
            this.tabFormControl1.Pages.Add(this.p검사하기);
            this.tabFormControl1.Pages.Add(this.그랩뷰어);
            this.tabFormControl1.Pages.Add(this.p검사내역);
            this.tabFormControl1.Pages.Add(this.p환경설정);
            this.tabFormControl1.Pages.Add(this.t디버깅용페이지);
            this.tabFormControl1.SelectedPage = this.t디버깅용페이지;
            this.tabFormControl1.ShowAddPageButton = false;
            this.tabFormControl1.ShowTabCloseButtons = false;
            this.tabFormControl1.ShowTabsInTitleBar = DevExpress.XtraBars.ShowTabsInTitleBar.True;
            this.tabFormControl1.Size = new System.Drawing.Size(1920, 30);
            this.tabFormControl1.TabForm = this;
            this.tabFormControl1.TabIndex = 0;
            this.tabFormControl1.TabLeftItemLinks.Add(this.타이틀);
            this.tabFormControl1.TabRightItemLinks.Add(this.e프로젝트);
            this.tabFormControl1.TabRightItemLinks.Add(this.skinPaletteDropDownButtonItem1);
            this.tabFormControl1.TabStop = false;
            // 
            // 타이틀
            // 
            this.타이틀.Caption = "LGES VDA590 TPA Total Inspection";
            this.타이틀.Id = 0;
            this.타이틀.ImageOptions.SvgImage = global::TPA.Properties.Resources.vision;
            this.타이틀.Name = "타이틀";
            this.타이틀.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            // 
            // e프로젝트
            // 
            this.e프로젝트.Caption = "IVM: 23-0034-004";
            this.e프로젝트.Id = 1;
            this.e프로젝트.Name = "e프로젝트";
            // 
            // skinPaletteDropDownButtonItem1
            // 
            this.skinPaletteDropDownButtonItem1.Id = 3;
            this.skinPaletteDropDownButtonItem1.Name = "skinPaletteDropDownButtonItem1";
            // 
            // p검사하기
            // 
            this.p검사하기.ContentContainer = this.tabFormContentContainer1;
            this.p검사하기.ImageOptions.SvgImage = global::TPA.Properties.Resources.enablesearch;
            this.p검사하기.Name = "p검사하기";
            this.p검사하기.Text = "Inspection";
            // 
            // tabFormContentContainer1
            // 
            this.tabFormContentContainer1.Controls.Add(this.e결과뷰어);
            this.tabFormContentContainer1.Controls.Add(this.e상태뷰어);
            this.tabFormContentContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabFormContentContainer1.Location = new System.Drawing.Point(0, 30);
            this.tabFormContentContainer1.Name = "tabFormContentContainer1";
            this.tabFormContentContainer1.Size = new System.Drawing.Size(1920, 1010);
            this.tabFormContentContainer1.TabIndex = 5;
            // 
            // e결과뷰어
            // 
            this.e결과뷰어.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.e결과뷰어.Dock = System.Windows.Forms.DockStyle.Fill;
            this.e결과뷰어.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.e결과뷰어.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.e결과뷰어.Location = new System.Drawing.Point(0, 104);
            this.e결과뷰어.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.e결과뷰어.Name = "e결과뷰어";
            this.e결과뷰어.Size = new System.Drawing.Size(1920, 906);
            this.e결과뷰어.TabIndex = 1;
            // 
            // e상태뷰어
            // 
            this.e상태뷰어.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.e상태뷰어.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.e상태뷰어.Appearance.Options.UseFont = true;
            this.e상태뷰어.Appearance.Options.UseForeColor = true;
            this.e상태뷰어.Dock = System.Windows.Forms.DockStyle.Top;
            this.e상태뷰어.Location = new System.Drawing.Point(0, 0);
            this.e상태뷰어.Name = "e상태뷰어";
            this.e상태뷰어.Size = new System.Drawing.Size(1920, 104);
            this.e상태뷰어.TabIndex = 0;
            // 
            // 그랩뷰어
            // 
            this.그랩뷰어.ContentContainer = this.tabFormContentContainer2;
            this.그랩뷰어.ImageOptions.SvgImage = global::TPA.Properties.Resources.electronics_photo;
            this.그랩뷰어.Name = "그랩뷰어";
            this.그랩뷰어.Text = "Camera";
            this.그랩뷰어.Visible = false;
            // 
            // tabFormContentContainer2
            // 
            this.tabFormContentContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabFormContentContainer2.Location = new System.Drawing.Point(0, 30);
            this.tabFormContentContainer2.Name = "tabFormContentContainer2";
            this.tabFormContentContainer2.Size = new System.Drawing.Size(1920, 1010);
            this.tabFormContentContainer2.TabIndex = 2;
            // 
            // p검사내역
            // 
            this.p검사내역.ContentContainer = this.tabFormContentContainer3;
            this.p검사내역.ImageOptions.SvgImage = global::TPA.Properties.Resources.employeesummary;
            this.p검사내역.Name = "p검사내역";
            this.p검사내역.Text = "History";
            // 
            // tabFormContentContainer3
            // 
            this.tabFormContentContainer3.Controls.Add(this.xtraTabControl1);
            this.tabFormContentContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabFormContentContainer3.Location = new System.Drawing.Point(0, 30);
            this.tabFormContentContainer3.Name = "tabFormContentContainer3";
            this.tabFormContentContainer3.Size = new System.Drawing.Size(1920, 1010);
            this.tabFormContentContainer3.TabIndex = 3;
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.Size = new System.Drawing.Size(1920, 1010);
            this.xtraTabControl1.TabIndex = 0;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2});
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.e검사내역);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(1918, 979);
            this.xtraTabPage1.Text = "List";
            // 
            // e검사내역
            // 
            this.e검사내역.Dock = System.Windows.Forms.DockStyle.Fill;
            this.e검사내역.Location = new System.Drawing.Point(0, 0);
            this.e검사내역.Name = "e검사내역";
            this.e검사내역.Size = new System.Drawing.Size(1918, 979);
            this.e검사내역.TabIndex = 0;
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add(this.e검사피봇);
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(1918, 979);
            this.xtraTabPage2.Text = "Pivot";
            // 
            // e검사피봇
            // 
            this.e검사피봇.Dock = System.Windows.Forms.DockStyle.Fill;
            this.e검사피봇.Location = new System.Drawing.Point(0, 0);
            this.e검사피봇.Name = "e검사피봇";
            this.e검사피봇.Size = new System.Drawing.Size(1918, 979);
            this.e검사피봇.TabIndex = 0;
            // 
            // p환경설정
            // 
            this.p환경설정.ContentContainer = this.tabFormContentContainer4;
            this.p환경설정.ImageOptions.SvgImage = global::TPA.Properties.Resources.yaxissettings;
            this.p환경설정.Name = "p환경설정";
            this.p환경설정.Text = "Preferences";
            // 
            // tabFormContentContainer4
            // 
            this.tabFormContentContainer4.Controls.Add(this.t환경설정);
            this.tabFormContentContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabFormContentContainer4.Location = new System.Drawing.Point(0, 30);
            this.tabFormContentContainer4.Name = "tabFormContentContainer4";
            this.tabFormContentContainer4.Size = new System.Drawing.Size(1920, 1010);
            this.tabFormContentContainer4.TabIndex = 4;
            // 
            // t환경설정
            // 
            this.t환경설정.Dock = System.Windows.Forms.DockStyle.Fill;
            this.t환경설정.Location = new System.Drawing.Point(0, 0);
            this.t환경설정.Name = "t환경설정";
            this.t환경설정.SelectedTabPage = this.t검사설정;
            this.t환경설정.Size = new System.Drawing.Size(1920, 1010);
            this.t환경설정.TabIndex = 0;
            this.t환경설정.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.t검사설정,
            this.t변수설정,
            this.t장치설정,
            this.t큐알검증,
            this.t로그내역});
            // 
            // t검사설정
            // 
            this.t검사설정.Controls.Add(this.e검사설정);
            this.t검사설정.Name = "t검사설정";
            this.t검사설정.Size = new System.Drawing.Size(1918, 979);
            this.t검사설정.Text = "검사설정";
            // 
            // e검사설정
            // 
            this.e검사설정.Dock = System.Windows.Forms.DockStyle.Fill;
            this.e검사설정.Location = new System.Drawing.Point(0, 0);
            this.e검사설정.Name = "e검사설정";
            this.e검사설정.Size = new System.Drawing.Size(1918, 979);
            this.e검사설정.TabIndex = 0;
            // 
            // t변수설정
            // 
            this.t변수설정.Name = "t변수설정";
            this.t변수설정.PageVisible = false;
            this.t변수설정.Size = new System.Drawing.Size(1918, 979);
            this.t변수설정.Text = "변수설정";
            // 
            // t장치설정
            // 
            this.t장치설정.Controls.Add(this.e장치설정);
            this.t장치설정.Name = "t장치설정";
            this.t장치설정.Size = new System.Drawing.Size(1918, 979);
            this.t장치설정.Text = "장치설정";
            // 
            // e장치설정
            // 
            this.e장치설정.Dock = System.Windows.Forms.DockStyle.Fill;
            this.e장치설정.Location = new System.Drawing.Point(0, 0);
            this.e장치설정.Name = "e장치설정";
            this.e장치설정.Size = new System.Drawing.Size(1918, 979);
            this.e장치설정.TabIndex = 0;
            // 
            // t큐알검증
            // 
            this.t큐알검증.Controls.Add(this.e큐알검증);
            this.t큐알검증.Name = "t큐알검증";
            this.t큐알검증.Size = new System.Drawing.Size(1918, 979);
            this.t큐알검증.Text = "큐알검증";
            // 
            // e큐알검증
            // 
            this.e큐알검증.Dock = System.Windows.Forms.DockStyle.Fill;
            this.e큐알검증.Location = new System.Drawing.Point(0, 0);
            this.e큐알검증.Name = "e큐알검증";
            this.e큐알검증.Size = new System.Drawing.Size(1918, 979);
            this.e큐알검증.TabIndex = 0;
            // 
            // t로그내역
            // 
            this.t로그내역.Controls.Add(this.e로그내역);
            this.t로그내역.Name = "t로그내역";
            this.t로그내역.Size = new System.Drawing.Size(1918, 979);
            this.t로그내역.Text = "로그내역";
            // 
            // e로그내역
            // 
            this.e로그내역.Dock = System.Windows.Forms.DockStyle.Fill;
            this.e로그내역.Location = new System.Drawing.Point(0, 0);
            this.e로그내역.Name = "e로그내역";
            this.e로그내역.Size = new System.Drawing.Size(1918, 979);
            this.e로그내역.TabIndex = 0;
            // 
            // t디버깅용페이지
            // 
            this.t디버깅용페이지.ContentContainer = this.tabFormContentContainer5;
            this.t디버깅용페이지.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("t디버깅용페이지.ImageOptions.SvgImage")));
            this.t디버깅용페이지.Name = "t디버깅용페이지";
            this.t디버깅용페이지.Text = "For Debugging";
            // 
            // tabFormContentContainer5
            // 
            this.tabFormContentContainer5.Controls.Add(this.lbK1);
            this.tabFormContentContainer5.Controls.Add(this.b커버센서읽기);
            this.tabFormContentContainer5.Controls.Add(this.bForceAutoMode);
            this.tabFormContentContainer5.Controls.Add(this.bTriggerLow);
            this.tabFormContentContainer5.Controls.Add(this.bTrigger커버들뜸);
            this.tabFormContentContainer5.Controls.Add(this.bTrigger상부큐알);
            this.tabFormContentContainer5.Controls.Add(this.bTrigger바닥평면);
            this.tabFormContentContainer5.Controls.Add(this.b모든CpltLow);
            this.tabFormContentContainer5.Controls.Add(this.bTrigger하부큐알);
            this.tabFormContentContainer5.Controls.Add(this.bBusyLow);
            this.tabFormContentContainer5.Controls.Add(this.ePLC모니터);
            this.tabFormContentContainer5.Controls.Add(this.e제품수행이벤트발생);
            this.tabFormContentContainer5.Controls.Add(this.e모델정보뷰어);
            this.tabFormContentContainer5.Controls.Add(this.e셋팅뷰어);
            this.tabFormContentContainer5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabFormContentContainer5.Location = new System.Drawing.Point(0, 30);
            this.tabFormContentContainer5.Name = "tabFormContentContainer5";
            this.tabFormContentContainer5.Size = new System.Drawing.Size(1920, 1010);
            this.tabFormContentContainer5.TabIndex = 5;
            // 
            // lbK1
            // 
            this.lbK1.Location = new System.Drawing.Point(1376, 84);
            this.lbK1.Name = "lbK1";
            this.lbK1.Size = new System.Drawing.Size(60, 15);
            this.lbK1.TabIndex = 26;
            this.lbK1.Text = "센서데이터";
            // 
            // b커버센서읽기
            // 
            this.b커버센서읽기.Location = new System.Drawing.Point(800, 416);
            this.b커버센서읽기.Name = "b커버센서읽기";
            this.b커버센서읽기.Size = new System.Drawing.Size(173, 36);
            this.b커버센서읽기.TabIndex = 21;
            this.b커버센서읽기.Text = "커버센서읽기";
            this.b커버센서읽기.Click += new System.EventHandler(this.b커버센서읽기_Click);
            // 
            // bForceAutoMode
            // 
            this.bForceAutoMode.Location = new System.Drawing.Point(1252, 428);
            this.bForceAutoMode.Name = "bForceAutoMode";
            this.bForceAutoMode.Size = new System.Drawing.Size(135, 23);
            this.bForceAutoMode.TabIndex = 20;
            this.bForceAutoMode.Text = "강제자동모드";
            this.bForceAutoMode.Click += new System.EventHandler(this.bForceAutoMode_Click);
            // 
            // bTriggerLow
            // 
            this.bTriggerLow.Location = new System.Drawing.Point(1252, 778);
            this.bTriggerLow.Name = "bTriggerLow";
            this.bTriggerLow.Size = new System.Drawing.Size(135, 23);
            this.bTriggerLow.TabIndex = 19;
            this.bTriggerLow.Text = "모든TriggerLow";
            this.bTriggerLow.Click += new System.EventHandler(this.bTriggerLow_Click);
            // 
            // bTrigger커버들뜸
            // 
            this.bTrigger커버들뜸.Location = new System.Drawing.Point(1252, 596);
            this.bTrigger커버들뜸.Name = "bTrigger커버들뜸";
            this.bTrigger커버들뜸.Size = new System.Drawing.Size(135, 23);
            this.bTrigger커버들뜸.TabIndex = 18;
            this.bTrigger커버들뜸.Text = "커버들뜸트리거";
            this.bTrigger커버들뜸.Click += new System.EventHandler(this.bTrigger커버들뜸_Click);
            // 
            // bTrigger상부큐알
            // 
            this.bTrigger상부큐알.Location = new System.Drawing.Point(1252, 556);
            this.bTrigger상부큐알.Name = "bTrigger상부큐알";
            this.bTrigger상부큐알.Size = new System.Drawing.Size(135, 23);
            this.bTrigger상부큐알.TabIndex = 17;
            this.bTrigger상부큐알.Text = "상부큐알트리거";
            this.bTrigger상부큐알.Click += new System.EventHandler(this.bTrigger상부큐알_Click);
            // 
            // bTrigger바닥평면
            // 
            this.bTrigger바닥평면.Location = new System.Drawing.Point(1252, 517);
            this.bTrigger바닥평면.Name = "bTrigger바닥평면";
            this.bTrigger바닥평면.Size = new System.Drawing.Size(135, 23);
            this.bTrigger바닥평면.TabIndex = 16;
            this.bTrigger바닥평면.Text = "바닥평면트리거";
            this.bTrigger바닥평면.Click += new System.EventHandler(this.bTrigger바닥평면_Click);
            // 
            // b모든CpltLow
            // 
            this.b모든CpltLow.Location = new System.Drawing.Point(1252, 735);
            this.b모든CpltLow.Name = "b모든CpltLow";
            this.b모든CpltLow.Size = new System.Drawing.Size(135, 23);
            this.b모든CpltLow.TabIndex = 15;
            this.b모든CpltLow.Text = "모든CompleteLow";
            this.b모든CpltLow.Click += new System.EventHandler(this.b모든CpltLow_Click);
            // 
            // bTrigger하부큐알
            // 
            this.bTrigger하부큐알.Location = new System.Drawing.Point(1252, 478);
            this.bTrigger하부큐알.Name = "bTrigger하부큐알";
            this.bTrigger하부큐알.Size = new System.Drawing.Size(135, 23);
            this.bTrigger하부큐알.TabIndex = 15;
            this.bTrigger하부큐알.Text = "하부큐알트리거";
            this.bTrigger하부큐알.Click += new System.EventHandler(this.bTrigger하부큐알_Click);
            // 
            // bBusyLow
            // 
            this.bBusyLow.Location = new System.Drawing.Point(1252, 688);
            this.bBusyLow.Name = "bBusyLow";
            this.bBusyLow.Size = new System.Drawing.Size(135, 23);
            this.bBusyLow.TabIndex = 15;
            this.bBusyLow.Text = "모든BusyLow";
            this.bBusyLow.Click += new System.EventHandler(this.bBusyLow_Click);
            // 
            // ePLC모니터
            // 
            this.ePLC모니터.Location = new System.Drawing.Point(616, 478);
            this.ePLC모니터.Name = "ePLC모니터";
            this.ePLC모니터.Size = new System.Drawing.Size(580, 520);
            this.ePLC모니터.TabIndex = 14;
            // 
            // e제품수행이벤트발생
            // 
            this.e제품수행이벤트발생.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("e제품수행이벤트발생.ImageOptions.SvgImage")));
            this.e제품수행이벤트발생.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.e제품수행이벤트발생.Location = new System.Drawing.Point(616, 416);
            this.e제품수행이벤트발생.Name = "e제품수행이벤트발생";
            this.e제품수행이벤트발생.Size = new System.Drawing.Size(161, 36);
            this.e제품수행이벤트발생.TabIndex = 2;
            this.e제품수행이벤트발생.Text = "제품수행이벤트발생";
            this.e제품수행이벤트발생.Click += new System.EventHandler(this.e제품수행이벤트발생_Click);
            // 
            // e모델정보뷰어
            // 
            this.e모델정보뷰어.Location = new System.Drawing.Point(606, 0);
            this.e모델정보뷰어.Name = "e모델정보뷰어";
            this.e모델정보뷰어.Size = new System.Drawing.Size(600, 400);
            this.e모델정보뷰어.TabIndex = 1;
            // 
            // e셋팅뷰어
            // 
            this.e셋팅뷰어.Dock = System.Windows.Forms.DockStyle.Left;
            this.e셋팅뷰어.Location = new System.Drawing.Point(0, 0);
            this.e셋팅뷰어.Name = "e셋팅뷰어";
            this.e셋팅뷰어.Size = new System.Drawing.Size(600, 1010);
            this.e셋팅뷰어.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1920, 1040);
            this.Controls.Add(this.tabFormContentContainer5);
            this.Controls.Add(this.tabFormControl1);
            this.IconOptions.SvgImage = global::TPA.Properties.Resources.vision;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TabFormControl = this.tabFormControl1;
            this.Text = "자동 치수 검사기";
            ((System.ComponentModel.ISupportInitialize)(this.tabFormControl1)).EndInit();
            this.tabFormContentContainer1.ResumeLayout(false);
            this.tabFormContentContainer3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            this.xtraTabPage2.ResumeLayout(false);
            this.tabFormContentContainer4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.t환경설정)).EndInit();
            this.t환경설정.ResumeLayout(false);
            this.t검사설정.ResumeLayout(false);
            this.t장치설정.ResumeLayout(false);
            this.t큐알검증.ResumeLayout(false);
            this.t로그내역.ResumeLayout(false);
            this.tabFormContentContainer5.ResumeLayout(false);
            this.tabFormContentContainer5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.TabFormControl tabFormControl1;
        private DevExpress.XtraBars.TabFormPage p검사하기;
        private DevExpress.XtraBars.BarStaticItem 타이틀;
        private DevExpress.XtraBars.BarStaticItem e프로젝트;
        private DevExpress.XtraBars.SkinPaletteDropDownButtonItem skinPaletteDropDownButtonItem1;
        private DevExpress.XtraBars.TabFormPage 그랩뷰어;
        private DevExpress.XtraBars.TabFormPage p검사내역;
        private DevExpress.XtraBars.TabFormContentContainer tabFormContentContainer3;
        private DevExpress.XtraBars.TabFormPage p환경설정;
        private DevExpress.XtraBars.TabFormContentContainer tabFormContentContainer4;
        private DevExpress.XtraTab.XtraTabControl t환경설정;
        private DevExpress.XtraTab.XtraTabPage t검사설정;
        private DevExpress.XtraTab.XtraTabPage t변수설정;
        private DevExpress.XtraTab.XtraTabPage t장치설정;
        private DevExpress.XtraTab.XtraTabPage t큐알검증;
        private DevExpress.XtraTab.XtraTabPage t로그내역;
        private UI.Controls.SetInspection e검사설정;
        private DevExpress.XtraBars.TabFormContentContainer tabFormContentContainer1;
        private UI.Controls.State e상태뷰어;
        private UI.Controls.DeviceSettings e장치설정;
        private UI.Controls.CamViewer e결과뷰어;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private UI.Controls.Result e검사내역;
        private UI.Controls.ResultPivot e검사피봇;
        private UI.Controls.LogViewer e로그내역;
        private UI.Controls.QrValidate e큐알검증;
        private DevExpress.XtraBars.TabFormContentContainer tabFormContentContainer2;
        private DevExpress.XtraBars.TabFormPage t디버깅용페이지;
        private DevExpress.XtraBars.TabFormContentContainer tabFormContentContainer5;
        private UI.Controls.EnvSettingViewer e셋팅뷰어;
        private UI.Controls.ModelSettingViewer e모델정보뷰어;
        private DevExpress.XtraEditors.SimpleButton e제품수행이벤트발생;
        private UI.Controls.PLCMonitor ePLC모니터;
        private DevExpress.XtraEditors.SimpleButton b모든CpltLow;
        private DevExpress.XtraEditors.SimpleButton bBusyLow;
        private DevExpress.XtraEditors.SimpleButton bTrigger하부큐알;
        private DevExpress.XtraEditors.SimpleButton bTrigger바닥평면;
        private DevExpress.XtraEditors.SimpleButton bTrigger상부큐알;
        private DevExpress.XtraEditors.SimpleButton bTrigger커버들뜸;
        private DevExpress.XtraEditors.SimpleButton bTriggerLow;
        private DevExpress.XtraEditors.SimpleButton bForceAutoMode;
        private DevExpress.XtraEditors.SimpleButton b커버센서읽기;
        private DevExpress.XtraEditors.LabelControl lbK1;
    }
}

