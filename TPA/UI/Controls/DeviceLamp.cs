﻿using DevExpress.XtraEditors;
using MvUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TPA.UI.Controls
{
    public partial class DeviceLamp : XtraUserControl
    {
        public DeviceLamp()
        {
            InitializeComponent();
        }

        private 장치상태 PLC통신;
        private 장치상태 큐알리더1;
        private 장치상태 큐알리더2;
        private 장치상태 큐알리더3;
        private 장치상태 조명장치;
        private 장치상태 카메라1;
        private 장치상태 카메라2;
        private 장치상태 카메라3;
        private 장치상태 카메라4;
        private 장치상태 카메라5;
        private 장치상태 카메라6;
        private 장치상태 카메라7;
        //private 장치상태 라벨출력;

        public void Init()
        {
            this.PLC통신 = new 장치상태(this.e장치통신, true);
            this.큐알리더1 = new 장치상태(this.e큐알리더1);
            this.큐알리더2 = new 장치상태(this.e큐알리더2);
            this.큐알리더3 = new 장치상태(this.e큐알리더3);
            this.조명장치 = new 장치상태(this.e조명장치);
            this.카메라1 = new 장치상태(this.e카메라1);
            this.카메라2 = new 장치상태(this.e카메라2);
            this.카메라3 = new 장치상태(this.e카메라3);
            this.카메라4 = new 장치상태(this.e카메라4);
            this.카메라5 = new 장치상태(this.e카메라5);
            this.카메라6 = new 장치상태(this.e카메라6);
            this.카메라7 = new 장치상태(this.e카메라7);
            //this.라벨출력 = new 장치상태(this.e라벨기);
            Global.장치통신.통신상태알림 += 통신상태알림;
            this.통신상태알림();
        }

        private void 통신상태알림()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(통신상태알림));
                return;
            }

            if (Global.장치통신.정상여부) this.PLC통신.Set(Global.장치통신.통신확인핑퐁 ? 상태구분.정상 : 상태구분.대기);
            //if (Global.장치통신.정상여부) this.PLC통신.Set(상태구분.정상);
            else this.PLC통신.Set(상태구분.오류);
            if (Global.환경설정.동작구분 != 동작구분.Live) return;

            this.큐알리더1.Set(Global.장치상태.하부큐알1리더);
            this.큐알리더2.Set(Global.장치상태.하부큐알2리더);
            this.큐알리더3.Set(Global.장치상태.상부큐알리더);
            this.조명장치.Set(Global.장치상태.조명장치);
            this.카메라1.Set(Global.장치상태.측면카메라1);
            this.카메라2.Set(Global.장치상태.측면카메라2);
            this.카메라3.Set(Global.장치상태.상부카메라);
            this.카메라4.Set(Global.장치상태.하부카메라1);
            this.카메라5.Set(Global.장치상태.하부카메라2);
            this.카메라6.Set(Global.장치상태.커넥터설삽카메라1);
            this.카메라7.Set(Global.장치상태.커넥터설삽카메라2);
            //this.라벨출력.Set(Global.장치상태.라벨기);
        }

        private enum 상태구분
        {
            없음,
            대기,
            정상,
            오류,
        }
        private class 장치상태
        {
            private SvgImageBox 도구;
            private 상태구분 현재상태 = 상태구분.없음;
            private DevExpress.Utils.Svg.SvgImage 대기 = null;
            private DevExpress.Utils.Svg.SvgImage 정상 = null;
            private DevExpress.Utils.Svg.SvgImage 오류 = null;

            public 장치상태(SvgImageBox tool, Boolean HasWait = false)
            {
                this.도구 = tool;
                this.정상 = Utils.SetSvgStyle(tool.SvgImage, Utils.SvgStyles.Green);
                this.오류 = Utils.SetSvgStyle(tool.SvgImage, Utils.SvgStyles.Red);
                if (HasWait) this.대기 = Utils.SetSvgStyle(tool.SvgImage, Utils.SvgStyles.Blue);
                this.도구.SvgImage = this.오류;
            }

            public void Set(Boolean 상태) => this.Set(상태 ? 상태구분.정상 : 상태구분.오류);
            public void Set(상태구분 상태)
            {
                if (this.현재상태 == 상태) return;
                this.현재상태 = 상태;
                if (상태 == 상태구분.정상) this.도구.SvgImage = this.정상;
                else if (상태 == 상태구분.오류) this.도구.SvgImage = this.오류;
                else if (상태 == 상태구분.대기) this.도구.SvgImage = this.대기;
            }
        }
    }
}
