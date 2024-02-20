using ActUtlType64Lib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using 제품인덱스 = System.Int32;
using PLC커맨드값 = System.Int32;
using DevExpress.CodeParser;
using System.ComponentModel;

namespace TPA.Schemas
{
    [Description("MELSEC Q03UDV")]
    partial class 장치통신
    {
        private static String 로그영역 = "PLC 통신";
        private ActUtlType64 PLC = null;
        private const Int32 PLC통신주기 = 20;
        private DateTime 시작일시 = DateTime.Now;
        private Boolean 작업여부 = false;
        public Boolean 정상여부 = false;
        private DateTime 오류알림시간 = DateTime.Today.AddDays(-1);
        private Int32 오류알림간격 = 30; // 초

        public Int32 testcount = 0;

        public PLC커맨드통신 PLC커맨드 = new PLC커맨드통신();
        public Dictionary<PLC커맨드목록, 제품인덱스> 검사위치별제품인덱스버퍼 = new Dictionary<PLC커맨드목록, 제품인덱스>();

        public static Boolean ToBool(Int32 val) { return val != 0; }
        public static Int32 ToInt(Boolean val) { return val ? 1 : 0; }

        private class CommAddressAttribute : System.Attribute
        {
            public String cmdAddress = String.Empty;
            public String busyAddress = String.Empty;
            public String cpltAddress = String.Empty;
            public Int32 Delay = 0;
            public CommAddressAttribute(String address) => this.cmdAddress = address;
            public CommAddressAttribute(String address, Int32 delay)
            {
                this.cmdAddress = address;
                this.Delay = delay;
            }
            public CommAddressAttribute(String cmdAddr, String busyAddr, String cpltAddr, Int32 delay = 0)
            {
                this.cmdAddress = cmdAddr;
                this.busyAddress = busyAddr;
                this.cpltAddress = cpltAddr;
                this.Delay = delay;
            }
        }

        private class SensorAddressAttribute : System.Attribute
        {
            public String sensorAddr = String.Empty;
            public Int32 Delay = 0;

            public SensorAddressAttribute(String sensorAddr) => this.sensorAddr = sensorAddr;
            public SensorAddressAttribute(String sensorAddr, Int32 delay = 0)
            {
                this.sensorAddr = sensorAddr;
                this.Delay = delay;
            }
        }


        private Boolean SetDevice(String Address, Int32 Data, out Int32 오류코드)
        {
            오류코드 = PLC.SetDevice(Address, Data);
            return 오류코드 == 0;
        }

        private Int32[] ReadDeviceRandom(String[] 주소, out Int32 오류코드)
        {
            Int32[] 자료 = new Int32[주소.Length];
            오류코드 = PLC.ReadDeviceRandom(String.Join("\n", 주소), 주소.Length, out 자료[0]);
            return 자료;
        }

        private void PLC커맨드전송(PLC커맨드목록 주소, PLC커맨드값 값)
        {
            DateTime 시간 = DateTime.Now;
            Int32 오류 = 0;
            SetDevice(PLC커맨드.Address(주소), 값, out 오류);
            통신오류알림(오류);
        }

        public void 통신오류알림(Int32 오류코드)
        {
            if (오류코드 == 0) {
                this.정상여부 = true;
                return;
            }
            if ((DateTime.Now - this.오류알림시간).TotalSeconds < this.오류알림간격) return;
            this.오류알림시간 = DateTime.Now;
            this.정상여부 = false;
            Global.오류로그(로그영역, "PLC 통신", $"[{오류코드.ToString("X8")}] 통신 오류가 발생하였습니다.", false);
        }
    }
}
