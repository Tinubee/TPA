﻿using Cognex.VisionPro;
using Cognex.VisionPro.Display;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CogUtils
{
    public class DisplayStatusBar : CogDisplayStatusBarV2
    {
        public void Init(CogDisplay Display)
        {
            this.Display = Display;
        }
    }
}