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
using MvUtils;
using TPA.Schemas;
using System.Web.UI.WebControls;

namespace TPA.UI.Controls
{
    public partial class EnvSettingViewer : XtraUserControl
    {
        private int limitline = 100;
        private int removeline = 50;

        public EnvSettingViewer()
        {
            InitializeComponent();
        }

        public void Init()
        {
            this.Bind환경설정정보.DataSource = Global.환경설정;
            //this.e디버깅로그창.ResetText();
            //this.Refresh();

            //Global.로그알람 += 로그출력;
        }

        private void 로그출력(String 내용)
        {
            if (this.e디버깅로그창.InvokeRequired) {
                this.e디버깅로그창.BeginInvoke(new Action(() => { 로그출력(내용); }));
                return;
            }
            this.e디버깅로그창.Items.Add(내용);
            this.e디버깅로그창.SelectedIndex = this.e디버깅로그창.Items.Count - 1;
            this.e디버깅로그창.TopIndex = this.e디버깅로그창.SelectedIndex;

            while (this.e디버깅로그창.Items.Count > limitline) {
                for (int i = 0; i < removeline; i++)
                    this.e디버깅로그창.Items.RemoveAt(0);
            }
        }
    }
}
