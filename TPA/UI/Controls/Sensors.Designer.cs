namespace TPA.UI.Controls
{
    partial class Sensors
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Sensors));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.b저장 = new DevExpress.XtraEditors.SimpleButton();
            this.GridControl1 = new MvUtils.CustomGrid();
            this.GridView1 = new MvUtils.CustomView();
            this.colSensor = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSlope = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colIntercept = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.b저장);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(0, 485);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(801, 36);
            this.panelControl1.TabIndex = 0;
            // 
            // b저장
            // 
            this.b저장.Appearance.Font = new System.Drawing.Font("맑은 고딕", 12F);
            this.b저장.Appearance.Options.UseFont = true;
            this.b저장.Dock = System.Windows.Forms.DockStyle.Right;
            this.b저장.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("b저장.ImageOptions.SvgImage")));
            this.b저장.Location = new System.Drawing.Point(648, 2);
            this.b저장.Name = "b저장";
            this.b저장.Size = new System.Drawing.Size(151, 32);
            this.b저장.TabIndex = 0;
            this.b저장.Text = "설정저장";
            // 
            // GridControl1
            // 
            this.GridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GridControl1.Location = new System.Drawing.Point(0, 0);
            this.GridControl1.MainView = this.GridView1;
            this.GridControl1.Name = "GridControl1";
            this.GridControl1.Size = new System.Drawing.Size(801, 485);
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
            this.colSensor,
            this.colSlope,
            this.colIntercept});
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
            this.GridView1.OptionsCustomization.AllowGroup = false;
            this.GridView1.OptionsCustomization.AllowQuickHideColumns = false;
            this.GridView1.OptionsCustomization.AllowSort = false;
            this.GridView1.OptionsFilter.UseNewCustomFilterDialog = true;
            this.GridView1.OptionsNavigation.EnterMoveNextColumn = true;
            this.GridView1.OptionsPrint.AutoWidth = false;
            this.GridView1.OptionsPrint.UsePrintStyles = false;
            this.GridView1.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.False;
            this.GridView1.OptionsView.ShowGroupPanel = false;
            this.GridView1.RowHeight = 20;
            // 
            // colSensor
            // 
            this.colSensor.AppearanceHeader.Options.UseTextOptions = true;
            this.colSensor.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colSensor.FieldName = "Sensor";
            this.colSensor.Name = "colSensor";
            this.colSensor.OptionsColumn.AllowEdit = false;
            this.colSensor.Visible = true;
            this.colSensor.VisibleIndex = 0;
            // 
            // colSlope
            // 
            this.colSlope.AppearanceHeader.Options.UseTextOptions = true;
            this.colSlope.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colSlope.DisplayFormat.FormatString = "{0:#,0.000000000000}";
            this.colSlope.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colSlope.FieldName = "Slope";
            this.colSlope.Name = "colSlope";
            this.colSlope.Visible = true;
            this.colSlope.VisibleIndex = 1;
            // 
            // colIntercept
            // 
            this.colIntercept.AppearanceHeader.Options.UseTextOptions = true;
            this.colIntercept.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colIntercept.DisplayFormat.FormatString = "{0:#,0.000000000000}";
            this.colIntercept.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colIntercept.FieldName = "Intercept";
            this.colIntercept.Name = "colIntercept";
            this.colIntercept.Visible = true;
            this.colIntercept.VisibleIndex = 2;
            // 
            // Sensors
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.GridControl1);
            this.Controls.Add(this.panelControl1);
            this.Name = "Sensors";
            this.Size = new System.Drawing.Size(801, 521);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton b저장;
        private MvUtils.CustomGrid GridControl1;
        private MvUtils.CustomView GridView1;
        private DevExpress.XtraGrid.Columns.GridColumn colSensor;
        private DevExpress.XtraGrid.Columns.GridColumn colSlope;
        private DevExpress.XtraGrid.Columns.GridColumn colIntercept;
    }
}
