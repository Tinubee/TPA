using CogUtils;
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
using TPA.Schemas;
using static TPA.Schemas.검사자료;

namespace TPA.UI.Controls
{
    public enum DisplayUI
    {
        좌측면,
        우측면,
        상부면,
        노멀각인,
        좌측하부,
        우측하부,
        커넥터설삽상부,
        커넥터설삽하부,
        배면각인,
    }

    public partial class CamViewer : System.Windows.Forms.UserControl
    {
        public CamViewer()
        {
            InitializeComponent();
        }

        public void Init()
        {
            this.e좌측면.Init(false);
            this.e우측면.Init(false);
            this.e상부면.Init(false);
            this.e노멀각인.Init(false);
            this.e좌측하부.Init(false);
            this.e우측하부.Init(false);
            this.e커넥터설삽상부.Init(false);
            this.e커넥터설삽하부.Init(false);
            this.eResultPreview.Init();

            Global.비전검사.SetDisplay(카메라구분.Cam01, this.e좌측면);
            Global.비전검사.SetDisplay(카메라구분.Cam02, this.e우측면);
            Global.비전검사.SetDisplay(카메라구분.Cam03, this.e상부면);
            Global.비전검사.SetDisplay(카메라구분.Cam04, this.e좌측하부);
            Global.비전검사.SetDisplay(카메라구분.Cam05, this.e우측하부);
            Global.비전검사.SetDisplay(카메라구분.Cam06, this.e커넥터설삽상부);
            Global.비전검사.SetDisplay(카메라구분.Cam07, this.e커넥터설삽하부);
            Global.비전검사.SetDisplay(카메라구분.Cam08, this.e노멀각인);

            Global.검사자료.검사완료알림 += 검사완료알림;
            검사완료알림(Global.검사자료.현재검사찾기());
        }

        public RecordDisplay GetDisplayControl(DisplayUI 구분)
        {
            if (구분 == DisplayUI.좌측면) return this.e좌측면;
            else if (구분 == DisplayUI.우측면) return this.e우측면;
            else if (구분 == DisplayUI.상부면) return this.e상부면;
            else if (구분 == DisplayUI.노멀각인) return this.e노멀각인;
            else if (구분 == DisplayUI.좌측하부) return this.e좌측하부;
            else if (구분 == DisplayUI.우측하부) return this.e우측하부;
            else if (구분 == DisplayUI.커넥터설삽상부) return this.e커넥터설삽상부;
            else if (구분 == DisplayUI.커넥터설삽하부) return this.e커넥터설삽하부;
            else return null;
        }

        private void 검사완료알림(검사결과 결과)
        {
            // if (this.GridControl1.InvokeRequired)
            // {
            //     this.GridControl1.BeginInvoke(new Action(() => { 검사완료알림(결과); }));
            //     return;
            // }
            // this.GridControl1.DataSource = 결과.검사내역;
            // this.GridView1.RefreshData();
        }
    }
}
