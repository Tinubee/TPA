using DevExpress.Xpo.Exceptions;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraRichEdit.Model;
using Npgsql;
using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using TPA.Schemas;

namespace TPA.UI.Controls
{
    public partial class MasterCalib : XtraUserControl
    {
        BindingList<마스터Calib정보> list = new BindingList<마스터Calib정보>();
        private bool isCalculating = false;

        public MasterCalib()
        {
            InitializeComponent();
        }

        public void Init()
        {
            this.GridView1.Init(this.barManager1);
            this.GridView1.OptionsBehavior.Editable = true;
            this.GridView1.OptionsSelection.MultiSelect = true;
            this.GridView1.OptionsView.NewItemRowPosition = NewItemRowPosition.Bottom;
            this.GridView1.AddEditSelectionMenuItem();
            this.GridView1.AddSelectPopMenuItems();
            this.GridView1.CellValueChanged += 교정값계산;
            //this.GridView1.AddDeleteMenuItem(DeleteClick);
            this.col측정값.DisplayFormat.FormatString = Global.환경설정.결과표현;
            this.colCMM측정값.DisplayFormat.FormatString = Global.환경설정.결과표현;
            this.col교정값.DisplayFormat.FormatString = "0.#########";

            this.col검사장치.OptionsColumn.AllowEdit = false;
            this.col검사장치.AppearanceCell.BackColor = System.Drawing.Color.Gray;
            this.col검사장치.AppearanceCell.ForeColor = System.Drawing.Color.Black;

            this.col검사항목.OptionsColumn.AllowEdit = false;
            this.col검사항목.AppearanceCell.BackColor = System.Drawing.Color.Gray;
            this.col검사항목.AppearanceCell.ForeColor = System.Drawing.Color.Black;

            this.col교정값.OptionsColumn.AllowEdit = false;
            this.col교정값.AppearanceCell.BackColor = System.Drawing.Color.Gray;
            this.col교정값.AppearanceCell.ForeColor = System.Drawing.Color.Black;

            this.col측정단위.OptionsColumn.AllowEdit = false;
            this.col측정단위.AppearanceCell.BackColor = System.Drawing.Color.Gray;
            this.col측정단위.AppearanceCell.ForeColor = System.Drawing.Color.Black;

            this.col측정값.OptionsColumn.AllowEdit = false;
            this.col측정값.AppearanceCell.BackColor = System.Drawing.Color.Gray;
            this.col측정값.AppearanceCell.ForeColor = System.Drawing.Color.Black;

            this.e일자.DateTime = DateTime.Now;
            this.b조회.Click += 자료조회;
            this.b적용.Click += 교정값적용;
            this.col측정값.DisplayFormat.FormatString = Global.환경설정.결과표현;
            this.GridControl1.DataSource = this.list;
        }

        public void Close() { }

        private void 교정값계산(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (isCalculating) return;

            int rowIndex = e.RowHandle;

            object measvalue = GridView1.GetRowCellValue(rowIndex, "측정값");
            object cmmvalue = GridView1.GetRowCellValue(rowIndex, "CMM측정값");

            double measdvalue, cmmdvalue;

            if (measvalue != null && cmmvalue != null && 
                    double.TryParse(measvalue.ToString(), out measdvalue) &&
                        double.TryParse(cmmvalue.ToString(), out cmmdvalue))
            {
                if (measdvalue != 0)
                {
                    isCalculating = true;

                    double calvalue = cmmdvalue / measdvalue;
                    GridView1.SetRowCellValue(rowIndex, "교정값", calvalue);

                    isCalculating = false;
                }
            }
        }

        private void 교정값적용(object sender,  EventArgs e)
        {
            모델구분 모델 = Global.환경설정.선택모델;
            검사설정자료 자료 = Global.모델자료.GetItem(모델)?.검사설정;

            for (int rowIndex = 0; rowIndex < GridView1.DataRowCount; rowIndex++)
            {
                object 검사항목 = GridView1.GetRowCellValue(rowIndex, "검사항목");
                object 교정값 = GridView1.GetRowCellValue(rowIndex, "교정값");

                double calDValue;

                if (교정값 != null && double.TryParse(교정값.ToString(), out calDValue))
                {
                    if (calDValue != 0)
                    {
                        검사정보 정보 = 자료.FirstOrDefault(item => item.검사항목.ToString() == 검사항목.ToString());
                        if (정보 != null)
                        {
                            정보.교정값 = (Decimal)calDValue;
                            Debug.WriteLine($"검사항목 : {검사항목.ToString()}, 교정값 : {교정값.ToString()}");
                        }
                    }
                }
            }
        }

        private void 자료조회(object sender, EventArgs e)
        {
            string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=ivmadmin;Database=vda590tpa_ohsung";

            NpgsqlConnection conn = new NpgsqlConnection(connectionString);

            conn.Open();

            DateTime starttime = this.e일자.DateTime.Date;
            DateTime endtime = this.e일자.DateTime.Date.AddDays(1);

            string sindex = this.e제품인덱스.Text;
            Int32 index = int.Parse(sindex);

            string sql = $"SELECT * FROM public.inspd INNER JOIN inspl ON public.inspd.idwdt = public.inspl.ilwdt WHERE ilwdt >= @starttime AND ilwdt <= @endtime AND ilnum = @{index}";

            NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@starttime", starttime);
            cmd.Parameters.AddWithValue("@endtime", endtime);
            cmd.Parameters.AddWithValue("@ilnum", index);

            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            da.Fill(dt);
            list.Clear();
            // 결과를 처리하거나 출력할 수 있습니다.

            foreach (DataRow row in dt.Rows)
            {
                마스터Calib정보 정보 = new 마스터Calib정보();
                foreach (DataColumn col in dt.Columns)
                {
                    string columnName = col.ColumnName;
                    object columnValue = row[col];

                    Debug.WriteLine($"{columnName}: {columnValue}");

                    if (columnName.Contains("iddev")) // 검사장치
                    {
                        정보.검사장치 = (장치구분)columnValue;
                    }
                    else if (columnName.Contains("iditm")) // 검사항목
                    {
                        정보.검사항목 = (검사항목)columnValue;
                    }
                    else if (columnName.Contains("iduni")) // 측정단위
                    {
                        정보.측정단위 = (단위구분)columnValue;
                    }
                    else if (columnName.Contains("idmes")) // 측정값
                    {
                        정보.측정값 = (Decimal)columnValue;
                    }
                    else if (columnName.Contains("idcal")) // 교정값
                    {
                        정보.교정값 = (Decimal)columnValue;
                    }
                }
                list.Add(정보);
            }
            GridControl1.DataSource = null;
            GridControl1.DataSource = list;
        }
    }

    public class 마스터Calib정보
    {
        public 장치구분 검사장치 { get; set; } = 장치구분.None;
        public 검사항목 검사항목 { get; set; } = 검사항목.None;
        public 단위구분 측정단위 { get; set; } = 단위구분.mm;
        public Decimal 측정값 { get; set; } = 0m;
        public Decimal CMM측정값 { get; set; } = 0m;
        public Decimal 교정값 { get; set; } = 1m;
    }
}
