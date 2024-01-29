using MvUtils;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Windows.Forms;
using TPA.Schemas;
using System.Diagnostics;
using System.Linq;

namespace TPA.UI.Controls
{
    public partial class CamSettings : XtraUserControl
    {
        public CamSettings()
        {
            InitializeComponent();
        }

        public void Init()
        {
            this.GridView1.Init();
            this.GridView1.OptionsBehavior.Editable = true;
            this.GridView1.OptionsView.ShowAutoFilterRow = false;
            this.GridView1.OptionsView.ShowFooter = false;
            this.GridView1.OptionsView.NewItemRowPosition = NewItemRowPosition.Bottom;
            this.GridControl1.DataSource = Global.그랩제어.Values;

            this.GridView2.Init();
            this.GridView2.OptionsBehavior.Editable = true;
            this.GridView2.OptionsView.ShowAutoFilterRow = false;
            this.GridView2.OptionsView.ShowFooter = false;
            this.GridControl2.DataSource = Global.조명제어;

            Localization.SetColumnCaption(this.GridView1, typeof(카메라장치));
            Localization.SetColumnCaption(this.GridView2, typeof(조명정보));
            this.b켜기.Click += 모두켜기;
            this.b끄기.Click += 모두끄기;
            this.b저장.Text = Localization.저장.GetString();
            this.GridView1.CellValueChanged += GridView1_CellValueChanged;
            this.GridView2.CellValueChanged += GridView2_CellValueChanged;
            this.e조명켜짐.Toggled += E켜짐_Toggled;
        }

        public void Close()
        {
            
        }

        private void b저장_Click(object sender, EventArgs e)
        {
            this.GridControl1.EmbeddedNavigator.Buttons.DoClick(this.GridControl1.EmbeddedNavigator.Buttons.EndEdit);
            this.GridControl2.EmbeddedNavigator.Buttons.DoClick(this.GridControl2.EmbeddedNavigator.Buttons.EndEdit);
            Global.그랩제어.Save();
            Global.조명제어.Save();
            Global.정보로그("카메라 설정", "설정저장", "저장되었습니다.", this.FindForm());
        }

        private void GridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            GridView view = sender as GridView;
            HikeGigE 장치 = view.GetRow(e.RowHandle) as HikeGigE;
            if (장치 == null) return;
            GridControl1.EmbeddedNavigator.Buttons.DoClick(GridControl1.EmbeddedNavigator.Buttons.EndEdit);
            if (e.Column.FieldName == this.col밝기.FieldName) 장치.밝기적용();
        }

        private void GridView2_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName != this.col광량.FieldName) return;
            GridView view = sender as GridView;
            조명정보 정보 = view.GetRow(e.RowHandle) as 조명정보;
            정보?.Set();
            view.RefreshRow(e.RowHandle);
        }

        private void E켜짐_Toggled(object sender, EventArgs e)
        {
            조명정보 정보 = this.GridView2.GetRow(this.GridView2.FocusedRowHandle) as 조명정보;
            if (정보 == null) return;
            정보.OnOff();
        }

        private void 모두켜기(object sender, EventArgs e) => Global.조명제어.TurnOn();
        private void 모두끄기(object sender, EventArgs e) => Global.조명제어.TurnOff();
    }
}
