using System.Collections.Generic;

namespace TPA.UI.Controls
{
    partial class QrValidate
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QrValidate));
            this.Bind검증정보 = new System.Windows.Forms.BindingSource(this.components);
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.e최소길이 = new DevExpress.XtraEditors.TextEdit();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.e최대길이 = new DevExpress.XtraEditors.TextEdit();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.e검증여부 = new DevExpress.XtraEditors.ToggleSwitch();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.e중복체크 = new DevExpress.XtraEditors.ToggleSwitch();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.textEdit2 = new DevExpress.XtraEditors.TextEdit();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.b설정저장 = new DevExpress.XtraEditors.SimpleButton();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.e샘플보기 = new DevExpress.XtraEditors.ButtonEdit();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            this.e코드검증 = new DevExpress.XtraEditors.ButtonEdit();
            this.layoutControlItem8 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.Bind검증자료 = new System.Windows.Forms.BindingSource(this.components);
            this.GridControl1 = new MvUtils.CustomGrid();
            this.GridView1 = new MvUtils.CustomView();
            this.col인쇄순번 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col코드구분 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col표현형식 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col검증유무 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col중복체크 = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.Bind검증정보)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.e최소길이.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.e최대길이.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.e검증여부.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.e중복체크.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.e샘플보기.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.e코드검증.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Bind검증자료)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // Bind검증정보
            // 
            this.Bind검증정보.DataSource = typeof(System.Collections.Generic.List<TPA.Schemas.큐알검증정보>);
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.e코드검증);
            this.layoutControl1.Controls.Add(this.e샘플보기);
            this.layoutControl1.Controls.Add(this.b설정저장);
            this.layoutControl1.Controls.Add(this.textEdit2);
            this.layoutControl1.Controls.Add(this.e중복체크);
            this.layoutControl1.Controls.Add(this.e검증여부);
            this.layoutControl1.Controls.Add(this.e최대길이);
            this.layoutControl1.Controls.Add(this.e최소길이);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(1305, 238, 672, 746);
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(1418, 137);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.emptySpaceItem1,
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutControlItem4,
            this.layoutControlItem5,
            this.layoutControlItem6,
            this.layoutControlItem7,
            this.layoutControlItem8,
            this.emptySpaceItem2});
            this.Root.Name = "Root";
            this.Root.Padding = new DevExpress.XtraLayout.Utils.Padding(4, 4, 4, 4);
            this.Root.Size = new System.Drawing.Size(1418, 137);
            this.Root.TextVisible = false;
            // 
            // e최소길이
            // 
            this.e최소길이.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.Bind검증자료, "최소길이", true));
            this.e최소길이.EnterMoveNextControl = true;
            this.e최소길이.Location = new System.Drawing.Point(68, 8);
            this.e최소길이.Name = "e최소길이";
            this.e최소길이.Properties.Appearance.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.e최소길이.Properties.Appearance.Options.UseFont = true;
            this.e최소길이.Properties.Appearance.Options.UseTextOptions = true;
            this.e최소길이.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.e최소길이.Size = new System.Drawing.Size(152, 28);
            this.e최소길이.StyleController = this.layoutControl1;
            this.e최소길이.TabIndex = 1;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.e최소길이;
            this.layoutControlItem1.CustomizationFormText = "최소길이";
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.MaxSize = new System.Drawing.Size(220, 30);
            this.layoutControlItem1.MinSize = new System.Drawing.Size(220, 30);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Padding = new DevExpress.XtraLayout.Utils.Padding(4, 4, 4, 4);
            this.layoutControlItem1.Size = new System.Drawing.Size(220, 36);
            this.layoutControlItem1.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem1.Text = "최소길이";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(48, 15);
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 36);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(1410, 21);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // e최대길이
            // 
            this.e최대길이.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.Bind검증자료, "최대길이", true));
            this.e최대길이.EnterMoveNextControl = true;
            this.e최대길이.Location = new System.Drawing.Point(288, 8);
            this.e최대길이.Name = "e최대길이";
            this.e최대길이.Properties.Appearance.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.e최대길이.Properties.Appearance.Options.UseFont = true;
            this.e최대길이.Properties.AutoHeight = false;
            this.e최대길이.Size = new System.Drawing.Size(152, 28);
            this.e최대길이.StyleController = this.layoutControl1;
            this.e최대길이.TabIndex = 4;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.e최대길이;
            this.layoutControlItem2.CustomizationFormText = "최대길이";
            this.layoutControlItem2.Location = new System.Drawing.Point(220, 0);
            this.layoutControlItem2.MaxSize = new System.Drawing.Size(220, 36);
            this.layoutControlItem2.MinSize = new System.Drawing.Size(220, 36);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Padding = new DevExpress.XtraLayout.Utils.Padding(4, 4, 4, 4);
            this.layoutControlItem2.Size = new System.Drawing.Size(220, 36);
            this.layoutControlItem2.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem2.Text = "최대길이";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(48, 15);
            // 
            // e검증여부
            // 
            this.e검증여부.EnterMoveNextControl = true;
            this.e검증여부.Location = new System.Drawing.Point(508, 8);
            this.e검증여부.Name = "e검증여부";
            this.e검증여부.Properties.Appearance.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.e검증여부.Properties.Appearance.Options.UseFont = true;
            this.e검증여부.Properties.AutoHeight = false;
            this.e검증여부.Properties.OffText = "Off";
            this.e검증여부.Properties.OnText = "On";
            this.e검증여부.Size = new System.Drawing.Size(152, 22);
            this.e검증여부.StyleController = this.layoutControl1;
            this.e검증여부.TabIndex = 5;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.e검증여부;
            this.layoutControlItem3.CustomizationFormText = "검증여부";
            this.layoutControlItem3.Location = new System.Drawing.Point(440, 0);
            this.layoutControlItem3.MaxSize = new System.Drawing.Size(220, 30);
            this.layoutControlItem3.MinSize = new System.Drawing.Size(220, 30);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Padding = new DevExpress.XtraLayout.Utils.Padding(4, 4, 4, 4);
            this.layoutControlItem3.Size = new System.Drawing.Size(220, 36);
            this.layoutControlItem3.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem3.Text = "검증여부";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(48, 15);
            // 
            // e중복체크
            // 
            this.e중복체크.EnterMoveNextControl = true;
            this.e중복체크.Location = new System.Drawing.Point(728, 8);
            this.e중복체크.Name = "e중복체크";
            this.e중복체크.Properties.Appearance.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.e중복체크.Properties.Appearance.Options.UseFont = true;
            this.e중복체크.Properties.AutoHeight = false;
            this.e중복체크.Properties.OffText = "Off";
            this.e중복체크.Properties.OnText = "On";
            this.e중복체크.Size = new System.Drawing.Size(152, 22);
            this.e중복체크.StyleController = this.layoutControl1;
            this.e중복체크.TabIndex = 6;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.e중복체크;
            this.layoutControlItem4.CustomizationFormText = "중복체크";
            this.layoutControlItem4.Location = new System.Drawing.Point(660, 0);
            this.layoutControlItem4.MaxSize = new System.Drawing.Size(220, 30);
            this.layoutControlItem4.MinSize = new System.Drawing.Size(220, 30);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Padding = new DevExpress.XtraLayout.Utils.Padding(4, 4, 4, 4);
            this.layoutControlItem4.Size = new System.Drawing.Size(220, 36);
            this.layoutControlItem4.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem4.Text = "중복체크";
            this.layoutControlItem4.TextSize = new System.Drawing.Size(48, 15);
            // 
            // textEdit2
            // 
            this.textEdit2.EnterMoveNextControl = true;
            this.textEdit2.Location = new System.Drawing.Point(948, 8);
            this.textEdit2.Name = "textEdit2";
            this.textEdit2.Properties.Appearance.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.textEdit2.Properties.Appearance.Options.UseFont = true;
            this.textEdit2.Properties.AutoHeight = false;
            this.textEdit2.Size = new System.Drawing.Size(152, 24);
            this.textEdit2.StyleController = this.layoutControl1;
            this.textEdit2.TabIndex = 7;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.textEdit2;
            this.layoutControlItem5.CustomizationFormText = "체크기간";
            this.layoutControlItem5.Location = new System.Drawing.Point(880, 0);
            this.layoutControlItem5.MaxSize = new System.Drawing.Size(220, 32);
            this.layoutControlItem5.MinSize = new System.Drawing.Size(220, 32);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Padding = new DevExpress.XtraLayout.Utils.Padding(4, 4, 4, 4);
            this.layoutControlItem5.Size = new System.Drawing.Size(220, 36);
            this.layoutControlItem5.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem5.Text = "체크기간";
            this.layoutControlItem5.TextSize = new System.Drawing.Size(48, 15);
            // 
            // b설정저장
            // 
            this.b설정저장.Appearance.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.b설정저장.Appearance.Options.UseFont = true;
            this.b설정저장.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("simpleButton1.ImageOptions.SvgImage")));
            this.b설정저장.ImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.b설정저장.Location = new System.Drawing.Point(1238, 8);
            this.b설정저장.Name = "b설정저장";
            this.b설정저장.Size = new System.Drawing.Size(172, 26);
            this.b설정저장.StyleController = this.layoutControl1;
            this.b설정저장.TabIndex = 8;
            this.b설정저장.Text = "설정 저장";
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.b설정저장;
            this.layoutControlItem6.Location = new System.Drawing.Point(1230, 0);
            this.layoutControlItem6.MaxSize = new System.Drawing.Size(180, 34);
            this.layoutControlItem6.MinSize = new System.Drawing.Size(180, 34);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Padding = new DevExpress.XtraLayout.Utils.Padding(4, 4, 4, 4);
            this.layoutControlItem6.Size = new System.Drawing.Size(180, 36);
            this.layoutControlItem6.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem6.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem6.TextVisible = false;
            // 
            // e샘플보기
            // 
            this.e샘플보기.EnterMoveNextControl = true;
            this.e샘플보기.Location = new System.Drawing.Point(68, 65);
            this.e샘플보기.Name = "e샘플보기";
            this.e샘플보기.Properties.Appearance.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.e샘플보기.Properties.Appearance.Options.UseFont = true;
            this.e샘플보기.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.e샘플보기.Size = new System.Drawing.Size(1342, 28);
            this.e샘플보기.StyleController = this.layoutControl1;
            this.e샘플보기.TabIndex = 9;
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this.e샘플보기;
            this.layoutControlItem7.CustomizationFormText = "샘플코드";
            this.layoutControlItem7.Location = new System.Drawing.Point(0, 57);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Padding = new DevExpress.XtraLayout.Utils.Padding(4, 4, 4, 4);
            this.layoutControlItem7.Size = new System.Drawing.Size(1410, 36);
            this.layoutControlItem7.Text = "샘플코드";
            this.layoutControlItem7.TextSize = new System.Drawing.Size(48, 15);
            // 
            // e코드검증
            // 
            this.e코드검증.EnterMoveNextControl = true;
            this.e코드검증.Location = new System.Drawing.Point(68, 101);
            this.e코드검증.Name = "e코드검증";
            this.e코드검증.Properties.Appearance.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.e코드검증.Properties.Appearance.Options.UseFont = true;
            this.e코드검증.Properties.AutoHeight = false;
            this.e코드검증.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.e코드검증.Size = new System.Drawing.Size(1342, 28);
            this.e코드검증.StyleController = this.layoutControl1;
            this.e코드검증.TabIndex = 10;
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.Control = this.e코드검증;
            this.layoutControlItem8.CustomizationFormText = "코드검증";
            this.layoutControlItem8.Location = new System.Drawing.Point(0, 93);
            this.layoutControlItem8.MaxSize = new System.Drawing.Size(0, 36);
            this.layoutControlItem8.MinSize = new System.Drawing.Size(200, 36);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Padding = new DevExpress.XtraLayout.Utils.Padding(4, 4, 4, 4);
            this.layoutControlItem8.Size = new System.Drawing.Size(1410, 36);
            this.layoutControlItem8.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem8.Text = "코드검증";
            this.layoutControlItem8.TextSize = new System.Drawing.Size(48, 15);
            // 
            // emptySpaceItem2
            // 
            this.emptySpaceItem2.AllowHotTrack = false;
            this.emptySpaceItem2.Location = new System.Drawing.Point(1100, 0);
            this.emptySpaceItem2.Name = "emptySpaceItem2";
            this.emptySpaceItem2.Size = new System.Drawing.Size(130, 36);
            this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
            // 
            // Bind검증자료
            // 
            this.Bind검증자료.DataSource = typeof(TPA.Schemas.큐알검증);
            // 
            // GridControl1
            // 
            this.GridControl1.DataSource = this.Bind검증정보;
            this.GridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GridControl1.Location = new System.Drawing.Point(0, 137);
            this.GridControl1.MainView = this.GridView1;
            this.GridControl1.Name = "GridControl1";
            this.GridControl1.Size = new System.Drawing.Size(1418, 584);
            this.GridControl1.TabIndex = 1;
            this.GridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GridView1});
            // 
            // GridView1
            // 
            this.GridView1.AllowColumnMenu = true;
            this.GridView1.AllowCustomMenu = true;
            this.GridView1.AllowExport = true;
            this.GridView1.AllowPrint = true;
            this.GridView1.AllowSettingsMenu = false;
            this.GridView1.AllowSummaryMenu = true;
            this.GridView1.ApplyFocusedRow = true;
            this.GridView1.Caption = "";
            this.GridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.col코드구분,
            this.col인쇄순번,
            this.col표현형식,
            this.col검증유무,
            this.col중복체크});
            this.GridView1.FooterPanelHeight = 21;
            this.GridView1.GridControl = this.GridControl1;
            this.GridView1.GroupRowHeight = 21;
            this.GridView1.IndicatorWidth = 44;
            this.GridView1.MinColumnRowHeight = 24;
            this.GridView1.MinRowHeight = 18;
            this.GridView1.Name = "GridView1";
            this.GridView1.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.Click;
            this.GridView1.OptionsCustomization.AllowColumnMoving = false;
            this.GridView1.OptionsCustomization.AllowFilter = false;
            this.GridView1.OptionsCustomization.AllowSort = false;
            this.GridView1.OptionsFilter.UseNewCustomFilterDialog = true;
            this.GridView1.OptionsNavigation.EnterMoveNextColumn = true;
            this.GridView1.OptionsPrint.AutoWidth = false;
            this.GridView1.OptionsPrint.UsePrintStyles = false;
            this.GridView1.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.False;
            this.GridView1.OptionsView.ShowGroupPanel = false;
            this.GridView1.RowHeight = 20;
            this.GridView1.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.col인쇄순번, DevExpress.Data.ColumnSortOrder.Ascending)});
            // 
            // col인쇄순번
            // 
            this.col인쇄순번.AppearanceCell.Options.UseTextOptions = true;
            this.col인쇄순번.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.col인쇄순번.AppearanceHeader.Options.UseTextOptions = true;
            this.col인쇄순번.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.col인쇄순번.FieldName = "인쇄순번";
            this.col인쇄순번.Name = "col인쇄순번";
            this.col인쇄순번.Visible = true;
            this.col인쇄순번.VisibleIndex = 1;
            // 
            // col코드구분
            // 
            this.col코드구분.AppearanceHeader.Options.UseTextOptions = true;
            this.col코드구분.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.col코드구분.FieldName = "코드구분";
            this.col코드구분.Name = "col코드구분";
            this.col코드구분.OptionsColumn.AllowEdit = false;
            this.col코드구분.Visible = true;
            this.col코드구분.VisibleIndex = 0;
            // 
            // col표현형식
            // 
            this.col표현형식.AppearanceHeader.Options.UseTextOptions = true;
            this.col표현형식.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.col표현형식.FieldName = "표현형식";
            this.col표현형식.Name = "col표현형식";
            this.col표현형식.Visible = true;
            this.col표현형식.VisibleIndex = 2;
            // 
            // col검증유무
            // 
            this.col검증유무.AppearanceHeader.Options.UseTextOptions = true;
            this.col검증유무.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.col검증유무.FieldName = "검증유무";
            this.col검증유무.Name = "col검증유무";
            this.col검증유무.Visible = true;
            this.col검증유무.VisibleIndex = 3;
            // 
            // col중복체크
            // 
            this.col중복체크.AppearanceHeader.Options.UseTextOptions = true;
            this.col중복체크.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.col중복체크.FieldName = "중복체크";
            this.col중복체크.Name = "col중복체크";
            this.col중복체크.Visible = true;
            this.col중복체크.VisibleIndex = 4;
            // 
            // QrValidate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.GridControl1);
            this.Controls.Add(this.layoutControl1);
            this.Name = "QrValidate";
            this.Size = new System.Drawing.Size(1418, 721);
            ((System.ComponentModel.ISupportInitialize)(this.Bind검증정보)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.e최소길이.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.e최대길이.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.e검증여부.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.e중복체크.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.e샘플보기.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.e코드검증.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Bind검증자료)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource Bind검증자료;
        private System.Windows.Forms.BindingSource Bind검증정보;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraEditors.ToggleSwitch e검증여부;
        private DevExpress.XtraEditors.TextEdit e최대길이;
        private DevExpress.XtraEditors.TextEdit e최소길이;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraEditors.ButtonEdit e코드검증;
        private DevExpress.XtraEditors.ButtonEdit e샘플보기;
        private DevExpress.XtraEditors.SimpleButton b설정저장;
        private DevExpress.XtraEditors.TextEdit textEdit2;
        private DevExpress.XtraEditors.ToggleSwitch e중복체크;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem8;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
        private MvUtils.CustomGrid GridControl1;
        private MvUtils.CustomView GridView1;
        private DevExpress.XtraGrid.Columns.GridColumn col인쇄순번;
        private DevExpress.XtraGrid.Columns.GridColumn col코드구분;
        private DevExpress.XtraGrid.Columns.GridColumn col표현형식;
        private DevExpress.XtraGrid.Columns.GridColumn col검증유무;
        private DevExpress.XtraGrid.Columns.GridColumn col중복체크;
    }
}
