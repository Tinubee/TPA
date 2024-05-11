using Cognex.VisionPro;
using Cognex.VisionPro.Display;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public void SetImage(ICogImage image, ICogRecord record, List<ICogGraphic> graphics) //(ICogImage image)
        {
            Debug.WriteLine($"**************************************************{this.Name}RecodDisplay SetImage 들어옴**************************************************");
            try
            {
                if (image == null || !image.Allocated) return;
                if (this.InvokeRequired) { this.BeginInvoke(new Action(() => { SetImage(image, record, graphics); })); return; }
               
                this.Image = null;
                Debug.WriteLine($"************************************************** {this.Name}this.Image = null; 완료**************************************************");
                this.InteractiveGraphics.Clear();
                Debug.WriteLine($"**************************************************{this.Name}this.InteractiveGraphics.Clear(); 완료**************************************************");
                this.StaticGraphics.Clear();
                Debug.WriteLine($"**************************************************{this.Name}this.StaticGraphics.Clear(); 완료**************************************************");
                this.Image = image;
                Debug.WriteLine($"************************************************** {this.Name}this.Image = image; 완료**************************************************");
                this.Record = record;
                Debug.WriteLine($"************************************************** {this.Name}this.Record = record; 완료**************************************************");
                foreach (ICogGraphic graphic in graphics)
                {
                    this.StaticGraphics.Add(graphic, "Results");
                    Debug.WriteLine($"************************************************** {this.Name}this.StaticGraphics.Add(graphic, Results) 완료**************************************************");
                }
                    
                this.SetBackground();
                Debug.WriteLine($"**************************************************{this.Name}RecodDisplay SetImage 완료**************************************************");
                //GC.Collect();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"**************************************************RecodDisplay SetImage Error**************************************************");
                Debug.WriteLine($"{ex.Message}");
                Debug.WriteLine($"**************************************************RecodDisplay SetImage Error**************************************************");
            }
        }

        public void SetBackground() => this.BackColor = 배경색상;

        public void ToScroll(Point p)
        {
            if (p.IsEmpty) return;
            this.AutoScrollOffset = p;
        }
    }
}
