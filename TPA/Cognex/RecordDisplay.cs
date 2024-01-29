using Cognex.VisionPro;
using Cognex.VisionPro.Display;
using System;
using System.Drawing;

namespace CogUtils
{
    public class RecordDisplay : CogRecordDisplay
    {
        public static Color 배경색상 = DevExpress.LookAndFeel.DXSkinColors.IconColors.Black;

        public void Init(Boolean showScrollBar = true)
        {
            this.AutoFit = true;
            this.HorizontalScrollBar = showScrollBar;
            this.VerticalScrollBar = showScrollBar;
            this.BackColor = 배경색상;
            this.MouseMode = CogDisplayMouseModeConstants.Pan;
        }

        public void SetImage(ICogImage image)
        {
            this.Image = image;
            this.SetBackground();
            this.Fit(true);
        }

        public void SetBackground() => this.BackColor = 배경색상;

        public void ToScroll(Point p)
        {
            if (p.IsEmpty) return;
            this.AutoScrollOffset = p;
        }
    }
}
