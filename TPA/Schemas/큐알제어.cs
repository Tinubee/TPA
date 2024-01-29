using MvUtils;
using Newtonsoft.Json;
using OpenCvSharp.Aruco;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPA.Schemas
{
    public enum 큐알리더구분
    {
        하부큐알1,
        하부큐알2,
        상부큐알,
    }

    public class 큐알제어 : Dictionary<큐알리더구분, 큐알장치>
    {
        public SR710 하부큐알리더1 = null;
        public SR710 하부큐알리더2 = null;
        public SR2000 상부큐알리더 = null;

        public Boolean Init()
        {
            this.하부큐알리더1 = new SR710() { 주소 = "192.168.3.50", 포트 = 9004 };
            this.하부큐알리더2 = new SR710() { 주소 = "192.168.3.51", 포트 = 9004 };
            this.상부큐알리더 = new SR2000(큐알생성수:2) { 주소 = "192.168.3.52", 포트 = 9004 };

            if (this.하부큐알리더1.Init()) this.Add(큐알리더구분.하부큐알1, 하부큐알리더1);
            if (this.하부큐알리더2.Init()) this.Add(큐알리더구분.하부큐알2, 하부큐알리더2);
            if (this.상부큐알리더.Init()) this.Add(큐알리더구분.상부큐알, 상부큐알리더);

            return true;
        }

        public void Start()
        {
            foreach (큐알리더구분 구분 in typeof(큐알리더구분).GetEnumValues())
                this[구분].Start();
        }

        public void Close()
        {
            foreach (큐알리더구분 구분 in typeof(큐알리더구분).GetEnumValues()) {
                this[구분].Close();
            }
        }
    }
}
