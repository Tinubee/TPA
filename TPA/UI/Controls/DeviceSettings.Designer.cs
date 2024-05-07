namespace TPA.UI.Controls
{
    partial class DeviceSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeviceSettings));
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.e카메라 = new TPA.UI.Controls.CamSettings();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.t기타 = new DevExpress.XtraTab.XtraTabPage();
            this.b캠트리거리셋 = new DevExpress.XtraEditors.SimpleButton();
            this.e큐알장치 = new TPA.UI.Controls.QrControls();
            this.t환경설정 = new DevExpress.XtraTab.XtraTabPage();
            this.e기본설정 = new TPA.UI.Controls.Config();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1.Panel1)).BeginInit();
            this.splitContainerControl1.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1.Panel2)).BeginInit();
            this.splitContainerControl1.Panel2.SuspendLayout();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.t기타.SuspendLayout();
            this.t환경설정.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.FixedPanel = DevExpress.XtraEditors.SplitFixedPanel.None;
            this.splitContainerControl1.Location = new System.Drawing.Point(0, 0);
            this.splitContainerControl1.Name = "splitContainerControl1";
            // 
            // splitContainerControl1.Panel1
            // 
            this.splitContainerControl1.Panel1.Controls.Add(this.e카메라);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            // 
            // splitContainerControl1.Panel2
            // 
            this.splitContainerControl1.Panel2.Controls.Add(this.xtraTabControl1);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(1920, 1040);
            this.splitContainerControl1.SplitterPosition = 1288;
            this.splitContainerControl1.TabIndex = 0;
            // 
            // e카메라
            // 
            this.e카메라.Dock = System.Windows.Forms.DockStyle.Fill;
            this.e카메라.Location = new System.Drawing.Point(0, 0);
            this.e카메라.Name = "e카메라";
            this.e카메라.Size = new System.Drawing.Size(1288, 1040);
            this.e카메라.TabIndex = 0;
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.t기타;
            this.xtraTabControl1.Size = new System.Drawing.Size(622, 1040);
            this.xtraTabControl1.TabIndex = 0;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.t기타,
            this.t환경설정});
            // 
            // t기타
            // 
            this.t기타.Controls.Add(this.b캠트리거리셋);
            this.t기타.Controls.Add(this.e큐알장치);
            this.t기타.Name = "t기타";
            this.t기타.Size = new System.Drawing.Size(620, 1009);
            this.t기타.Text = "Others";
            // 
            // b캠트리거리셋
            // 
            this.b캠트리거리셋.Appearance.Font = new System.Drawing.Font("맑은 고딕", 18F, System.Drawing.FontStyle.Bold);
            this.b캠트리거리셋.Appearance.Options.UseFont = true;
            this.b캠트리거리셋.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("b캠트리거리셋.ImageOptions.SvgImage")));
            this.b캠트리거리셋.Location = new System.Drawing.Point(12, 13);
            this.b캠트리거리셋.Name = "b캠트리거리셋";
            this.b캠트리거리셋.Size = new System.Drawing.Size(323, 40);
            this.b캠트리거리셋.TabIndex = 1;
            this.b캠트리거리셋.Text = "Camera Trigger Reset";
            // 
            // e큐알장치
            // 
            this.e큐알장치.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.e큐알장치.Location = new System.Drawing.Point(0, 146);
            this.e큐알장치.Name = "e큐알장치";
            this.e큐알장치.Size = new System.Drawing.Size(620, 863);
            this.e큐알장치.TabIndex = 0;
            // 
            // t환경설정
            // 
            this.t환경설정.Controls.Add(this.e기본설정);
            this.t환경설정.Name = "t환경설정";
            this.t환경설정.Size = new System.Drawing.Size(620, 1009);
            this.t환경설정.Text = "Config";
            // 
            // e기본설정
            // 
            this.e기본설정.Dock = System.Windows.Forms.DockStyle.Fill;
            this.e기본설정.Location = new System.Drawing.Point(0, 0);
            this.e기본설정.Name = "e기본설정";
            this.e기본설정.Size = new System.Drawing.Size(620, 1009);
            this.e기본설정.TabIndex = 0;
            // 
            // DeviceSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainerControl1);
            this.Name = "DeviceSettings";
            this.Size = new System.Drawing.Size(1920, 1040);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1.Panel1)).EndInit();
            this.splitContainerControl1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1.Panel2)).EndInit();
            this.splitContainerControl1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.t기타.ResumeLayout(false);
            this.t환경설정.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage t기타;
        private CamSettings e카메라;
        private DevExpress.XtraTab.XtraTabPage t환경설정;
        private Config e기본설정;
        private QrControls e큐알장치;
        private DevExpress.XtraEditors.SimpleButton b캠트리거리셋;
    }
}
