using DevExpress.Utils.Extensions;
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

namespace TPA.UI.Controls
{
    public partial class PLCMonitor : XtraUserControl
    {
        BindingList<PLCAddress> plcAddrList = new BindingList<PLCAddress>();
        public PLCMonitor()
        {
            InitializeComponent();
        }

        public void Init()
        {
            foreach (장치통신.PLC커맨드목록 구분 in typeof(장치통신.PLC커맨드목록).GetEnumValues())
            {
                PLCAddress plcAddress = new PLCAddress();
                plcAddress.AddrName = 구분.ToString();
                plcAddress.CmdStatus = Global.장치통신.PLC커맨드.Get(구분);
                plcAddress.BusyStatus = Global.장치통신.정보읽기(Global.장치통신.PLC커맨드[구분].Busy주소);
                plcAddress.CompleteStatus = Global.장치통신.정보읽기(Global.장치통신.PLC커맨드[구분].완료주소);
                plcAddrList.Add(plcAddress);
            }
            customGrid1.DataSource = plcAddrList;
        }
    }

    public class PLCAddress
    {
        public String AddrName { get; set; }
        public Int32 CmdStatus { get; set; }
        public Int32 BusyStatus { get; set; }
        public Int32 CompleteStatus { get; set; }
    }
}
