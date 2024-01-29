using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace TPA.Schemas
{
    public enum COMMAND
    {
        ALL_READ,
        ALL_READ_AND_STATUS,
        SINGLE_READ,
        SINGLE_WRITE,
        SINGLE_READ_FLOAT,
    };
    
    public class DLEN1 : 변위센서장치
    {
        public DLEN1()
        {
            this.주소 = "192.168.3.100";
            this.포트 = 64000;
        }
    }
}
