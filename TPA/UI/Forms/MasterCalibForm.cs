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

namespace TPA.UI.Forms
{
    public partial class MasterCalibForm : XtraForm
    {
        private Controls.MasterCalib masterCalib1;

        public MasterCalibForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.masterCalib1 = new Controls.MasterCalib();
            this.SuspendLayout();
            // 
            // masterCalib1
            // 
            this.masterCalib1.Dock = DockStyle.Fill;
            this.masterCalib1.Location = new Point(0, 0);
            this.masterCalib1.Name = "masterCalib1";
            this.masterCalib1.Size = new Size(679, 519);
            this.masterCalib1.TabIndex = 0;
            // 
            // MasterCalibForm
            // 
            this.ClientSize = new Size(679, 519);
            this.Controls.Add(this.masterCalib1);
            this.Name = "MasterCalibForm";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        public void Init()
        {
            this.masterCalib1.Init();
        }
        
    }
}