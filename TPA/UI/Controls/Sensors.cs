using DevExpress.XtraEditors;
using System;

namespace TPA.UI.Controls
{
    public partial class Sensors : XtraUserControl
    {
        public Sensors()
        {
            InitializeComponent();
        }

        public void Init()
        {
            this.GridView1.Init();
            this.GridView1.OptionsBehavior.Editable = true;
            this.GridView1.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.None;
            // backup this.GridControl1.DataSource = Global.장치통신.센서보정.Values;
            this.b저장.Click += 설정저장;
        }
        public void Close() { }

        private void 설정저장(object sender, EventArgs e)
        {
            if (!MvUtils.Utils.Confirm("센서 보정 정보를 저장하시겠습니까?")) return;
            // backup Global.장치통신.센서보정.Save();
        }
    }
}
