namespace TPA.UI.Controls
{
    partial class PLCMonitor
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
            this.Bind센서자료 = new System.Windows.Forms.BindingSource(this.components);
            this.customGrid1 = new MvUtils.CustomGrid();
            this.customView1 = new MvUtils.CustomView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.Bind센서자료)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.customGrid1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.customView1)).BeginInit();
            this.SuspendLayout();
            // 
            // Bind센서자료
            // 
            this.Bind센서자료.DataSource = typeof(TPA.UI.Controls.PLCAddress);
            // 
            // customGrid1
            // 
            this.customGrid1.DataSource = this.Bind센서자료;
            this.customGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customGrid1.Location = new System.Drawing.Point(0, 0);
            this.customGrid1.MainView = this.customView1;
            this.customGrid1.Name = "customGrid1";
            this.customGrid1.Size = new System.Drawing.Size(674, 703);
            this.customGrid1.TabIndex = 0;
            this.customGrid1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.customView1});
            // 
            // customView1
            // 
            this.customView1.AllowColumnMenu = true;
            this.customView1.AllowCustomMenu = true;
            this.customView1.AllowExport = true;
            this.customView1.AllowPrint = true;
            this.customView1.AllowSettingsMenu = false;
            this.customView1.AllowSummaryMenu = true;
            this.customView1.ApplyFocusedRow = true;
            this.customView1.Caption = "";
            this.customView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4});
            this.customView1.FooterPanelHeight = 21;
            this.customView1.GridControl = this.customGrid1;
            this.customView1.GroupRowHeight = 21;
            this.customView1.IndicatorWidth = 44;
            this.customView1.MinColumnRowHeight = 24;
            this.customView1.MinRowHeight = 18;
            this.customView1.Name = "customView1";
            this.customView1.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.customView1.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.customView1.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.customView1.OptionsCustomization.AllowColumnMoving = false;
            this.customView1.OptionsCustomization.AllowGroup = false;
            this.customView1.OptionsCustomization.AllowQuickHideColumns = false;
            this.customView1.OptionsFilter.UseNewCustomFilterDialog = true;
            this.customView1.OptionsNavigation.EnterMoveNextColumn = true;
            this.customView1.OptionsPrint.AutoWidth = false;
            this.customView1.OptionsPrint.UsePrintStyles = false;
            this.customView1.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.False;
            this.customView1.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom;
            this.customView1.OptionsView.ShowGroupPanel = false;
            this.customView1.RowHeight = 20;
            // 
            // gridColumn1
            // 
            this.gridColumn1.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.Caption = "구분";
            this.gridColumn1.FieldName = "AddrName";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            // 
            // gridColumn2
            // 
            this.gridColumn2.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn2.Caption = "트리거상태";
            this.gridColumn2.FieldName = "CmdStatus";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            // 
            // gridColumn3
            // 
            this.gridColumn3.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn3.Caption = "Busy상태";
            this.gridColumn3.FieldName = "BusyStatus";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 2;
            // 
            // gridColumn4
            // 
            this.gridColumn4.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn4.Caption = "완료상태";
            this.gridColumn4.FieldName = "CompleteStatus";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 3;
            // 
            // PLCMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.customGrid1);
            this.Name = "PLCMonitor";
            this.Size = new System.Drawing.Size(674, 703);
            ((System.ComponentModel.ISupportInitialize)(this.Bind센서자료)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.customGrid1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.customView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource Bind센서자료;
        private MvUtils.CustomGrid customGrid1;
        private MvUtils.CustomView customView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
    }
}
