using Cognex.VisionPro;
using Cognex.VisionPro.Blob;
using Cognex.VisionPro.Display;
using Cognex.VisionPro.QuickBuild;
using Cognex.VisionPro.ToolBlock;
using Cognex.VisionPro.ToolGroup;
//using OpenCvSharp;
using CogUtils;
using MvUtils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;

namespace TPA.Schemas
{
    public class 비전도구
    {
        private static String 로그영역 = "검사도구";

        public 모델구분 모델구분 = Global.환경설정.선택모델;
        public 카메라구분 카메라 = 카메라구분.None;
        public String 도구명칭 { get => this.카메라.ToString(); }
        public String 도구경로 { get => Path.Combine(Global.환경설정.비전도구경로, ((Int32)모델구분).ToString("d2"), $"{도구명칭}.vpp"); }
        public String 마스터경로 { get => Path.Combine(Global.환경설정.마스터사진경로, $"{((Int32)모델구분).ToString("d2")}.{도구명칭}.png"); }

        public CogJob Job = null;
        public CogToolBlock ToolBlock = null;
        public CogToolBlock AlignTools { get { return this.GetTool("AlignTools") as CogToolBlock; } }
        public CogToolBlock InspectionTools = null;

        public ICogImage InputImage { get => this.GetInput<ICogImage>("InputImage"); set => this.SetInput("InputImage", value); }
        public ICogImage OutputImage { get { return GetOutput<ICogImage>(this.AlignTools, "OutputImage"); } }
        public String ViewerRecodName { get { return "AlignTools.Fixture.OutputImage"; } }
        public String InputImageName { get { return "AlignTools.CogPMAlignTool1.InputImage"; } }

        private DateTime 검사시작 = DateTime.Today;
        private DateTime 검사종료 = DateTime.Today;

        public Double 검사시간 { get { return (this.검사종료 - this.검사시작).TotalMilliseconds; } }
        public RecordDisplay Display = null;

        public 비전도구(모델구분 모델, 카메라구분 카메라)
        {
            this.모델구분 = 모델;
            this.카메라 = 카메라;
        }

        public static ICogTool GetTool(CogToolBlock tool, String name)
        {
            if (tool.Tools.Contains(name)) return tool.Tools[name];
            return null;
        }
        public static T GetInput<T>(CogToolBlock tool, String name)
        {
            if (tool == null) return default(T);
            if (tool.Inputs.Contains(name)) return (T)tool.Inputs[name].Value;
            return default(T);
        }
        public static Boolean SetInput(CogToolBlock tool, String name, Object value)
        {
            if (tool == null) return false;
            if (!tool.Inputs.Contains(name)) return false;
            tool.Inputs[name].Value = value;
            return true;
        }
        public static T GetOutput<T>(CogToolBlock tool, String name)
        {
            if (tool == null) return default(T);
            if (tool.Outputs.Contains(name)) return (T)tool.Outputs[name].Value;
            return default(T);
        }

        public ICogTool GetTool(String name) { return GetTool(this.ToolBlock, name); }
        public T GetInput<T>(String name) { return GetInput<T>(this.ToolBlock, name); }
        public Boolean SetInput(String name, Object value) { return SetInput(this.ToolBlock, name, value); }
        public T GetOutput<T>(String name) { return GetOutput<T>(this.ToolBlock, name); }

        public static void AddInput(CogToolBlock block, String name, Type type)
        {
            if (block == null || block.Inputs.Contains(name)) return;
            block.Inputs.Add(new CogToolBlockTerminal(name, type));
        }
        public static void AddInput(CogToolBlock block, String name, Object value)
        {
            if (block == null || block.Inputs.Contains(name)) return;
            block.Inputs.Add(new CogToolBlockTerminal(name, value));
        }
        public static void AddOutput(CogToolBlock block, String name, Type type)
        {
            if (block == null || block.Outputs.Contains(name)) return;
            block.Outputs.Add(new CogToolBlockTerminal(name, type));
        }
        public static void AddOutput(CogToolBlock block, String name, Object value)
        {
            if (block == null || block.Outputs.Contains(name)) return;
            block.Outputs.Add(new CogToolBlockTerminal(name, value));
        }

        public void Init() => this.Load();

        public void Load()
        {
            Debug.WriteLine(this.도구경로, this.카메라.ToString());
            if (File.Exists(this.도구경로))
            {
                #pragma warning disable
                this.Job = CogSerializer.LoadObjectFromFile(this.도구경로) as CogJob;
                this.ToolBlock = (this.Job.VisionTool as CogToolGroup).Tools[1] as CogToolBlock; // Cam01
            }
            else
            {
                this.Job = new CogJob($"Job{도구명칭}");
                CogToolGroup group = new CogToolGroup() { Name = $"Group{도구명칭}" };
                this.ToolBlock = new CogToolBlock();
                this.ToolBlock.Name = this.도구명칭;
                group.Tools.Add(this.ToolBlock);
                this.Job.VisionTool = group;
                this.ToolBlock.Tools.Add(new CogToolBlock() { Name = "AlignTools" });
                this.Save();
            }

            if (this.ToolBlock != null) this.ToolBlock.DataBindings.Clear();
            else this.ToolBlock = new CogToolBlock();
            this.ToolBlock.Name = this.도구명칭;

            // 파라미터 체크
            AddInput(this.ToolBlock, "InputImage", typeof(CogImage8Grey));
            AddInput(this.ToolBlock, "CalibX", 0.020d);
            AddInput(this.ToolBlock, "CalibY", 0.020d);
            AddInput(this.AlignTools, "InputImage", typeof(CogImage8Grey));
            AddInput(this.AlignTools, "CalibX", 0.020d);
            AddInput(this.AlignTools, "CalibY", 0.020d);
            AddInput(this.AlignTools, "Width", 107.7d);
            AddInput(this.AlignTools, "Height", 538d);
            AddInput(this.AlignTools, "OriginX", 1024d);
            AddInput(this.AlignTools, "OriginY", 10000d);
            AddInput(this.AlignTools, "OriginF", 1d); // X Factor
            AddOutput(this.AlignTools, "OutputImage", typeof(CogImage8Grey));
            AddOutput(this.AlignTools, "Rotation", 0d);
            // SetCalib();

            
            // Output 파라메터 설정, 일단 CTQ 항목만
            검사설정자료 자료 = Global.모델자료.GetItem(this.모델구분)?.검사설정;
            if (자료 == null) return;
            List<검사정보> 목록 = 자료.Where(e => (Int32)e.검사장치 == (Int32)this.카메라 && !String.IsNullOrEmpty(e.변수명칭)).ToList();
            foreach (검사정보 검사 in 목록)
            {
                if (검사.검사그룹 == 검사그룹.CTQ) { AddOutput(this.ToolBlock, 검사.변수명칭, typeof(Double)); }
                else {  }
            }
        }

        public void Save()
        {
            CogSerializer.SaveObjectToFile(this.Job, this.도구경로, typeof(BinaryFormatter), CogSerializationOptionsConstants.Minimum);
            Debug.WriteLine(this.도구경로, "도구저장");
        }

        // public void SetCalib() => SetCalib(Global.그랩제어.GetItem(this.카메라));
        // public void SetCalib(그랩장치 장치)
        // {
        //     if (장치 == null) return;
        //     SetInput(this.ToolBlock, "CalibX", 장치.교정X / 1000);
        //     SetInput(this.ToolBlock, "CalibY", 장치.교정Y / 1000);
        // }

        #region Run
        public Boolean IsAccepted()
        {
            try
            {
                foreach (ICogTool tool in this.AlignTools.Tools)
                    if (tool.RunStatus.Result != CogToolResultConstants.Accept) return false;
                foreach (ICogTool tool in this.ToolBlock.Tools)
                    if (tool.RunStatus.Result != CogToolResultConstants.Accept) return false;
                return true;
            }
            catch (Exception ee)
            {
                Debug.WriteLine($"*****************************IsAccepted Error*****************************");
                Debug.WriteLine($"{ee.Message}");
                Debug.WriteLine($"*****************************IsAccepted Error*****************************");
                return false;
            }
          
        }

        public Boolean Run(ICogImage image, 검사결과 검사)
        {
            Boolean accepted = false;
            try
            {
                if (image != null) this.InputImage = image;
                this.검사시작 = DateTime.Now;
                this.ToolBlock.Run();

                this.검사종료 = DateTime.Now;
                accepted = this.IsAccepted();
                if (this.카메라 == 카메라구분.Cam08)
                {
                    //DisplayResult(null);
                }
                else
                {
                    Global.검사자료.카메라검사(this.카메라, GetResults());
                    DisplayResult(검사);
                }
                return this.IsAccepted();
            }
            catch (Exception ex)
            {
                Global.로그기록(로그영역, 로그구분.오류, "Cognex", $"Cognex Run Fails : {ex.Message}");
                return false;
            }
        }

        public void SetInputs(Dictionary<String, Object> inputs)
        {
            if (inputs == null) return;
            foreach (KeyValuePair<String, Object> r in inputs)
                this.SetInput(r.Key, r.Value);
        }

        public Dictionary<String, Object> GetResults()
        {
            Dictionary<String, Object> results = new Dictionary<String, Object>();
            foreach (CogToolBlockTerminal terminal in this.ToolBlock.Outputs) {
                if (terminal.ValueType == typeof(Double))
                    results.Add(terminal.Name, terminal.Value == null ? Double.NaN : (Double)terminal.Value);
                else if (terminal.ValueType == typeof(String))
                    results.Add(terminal.Name, terminal.Value == null ? String.Empty : (String)terminal.Value);
            }
            //results.Add(ResultAttribute.VarName(검사항목.스크레치), SufaceResults(검사항목.스크레치));
            //results.Add(ResultAttribute.VarName(검사항목.찍힘불량), SufaceResults(검사항목.찍힘불량));
            return results;
        }

        public List<불량영역> SufaceResults(검사항목 항목)
        {
            String name = ResultAttribute.VarName(항목);
            List<불량영역> list = new List<불량영역>();
            CogToolBlock sufacetools = this.GetTool("SurfaceTools") as CogToolBlock;
            if (sufacetools == null) return list;
            foreach (ICogTool tool in sufacetools.Tools)
            {
                if (!tool.Name.StartsWith(name)) continue;
                list.AddRange(SufaceResults(항목, tool as CogBlobTool));
            }
            return list;
        }
        private List<불량영역> SufaceResults(검사항목 항목, CogBlobTool tool)
        {
            List<불량영역> list = new List<불량영역>();
            if (tool == null || tool.RunStatus.Result != CogToolResultConstants.Accept || tool.Results == null) return list;
            CogBlobResultCollection blobs = tool.Results.GetBlobs();
            foreach (CogBlobResult blob in blobs)
                list.Add(new 불량영역(this.카메라, 항목, blob.GetBoundingBoxAtAngle(0)));
            return list;
        }
        #endregion

        #region 도구설정, 마스터
        public void 도구설정() => 비전검사.도구설정(this);
        public Boolean 마스터저장()
        {
            if (this.InputImage == null) return false;
            Boolean r = Common.ImageSavePng(this.InputImage, this.마스터경로, out String error);
            if (!r) Utils.WarningMsg("마스터 이미지 등록실패!!!\n" + error);
            return r;
        }
        public Boolean 마스터로드() => 이미지로드(this.마스터경로);
        public Boolean 이미지로드() => 이미지로드(Common.GetImageFile());
        public Boolean 이미지로드(String path)
        {
            if (!File.Exists(path)) return false;
            return 이미지검사(Common.LoadImage(path));
        }
        public Boolean 이미지검사(ICogImage image)
        {
            if (Global.장치상태.자동수동) return false;
            if (image == null) return false;
            this.Run(image, Global.검사자료.수동검사);
            return true;
        }
        #endregion

        #region Display
        public void DisplayResult(검사결과 검사) => this.DisplayResult(this.Display, 검사);
        public void DisplayResult(RecordDisplay display, 검사결과 검사)
        {
            if (display == null) return;
            ICogRecord records = this.ToolBlock.CreateLastRunRecord();
            try
            {
                ICogRecord record = null;
                if (records != null && records.SubRecords != null && records.SubRecords.ContainsKey(this.ViewerRecodName))
                    record = records.SubRecords[this.ViewerRecodName];
                List<ICogGraphic> graphics = new List<ICogGraphic>();
                if (검사 != null)
                    검사.불량영역(this.카메라).ForEach(e => display.StaticGraphics.Add(e.GetRectangle(GraphicColor(결과구분.NG), 2), "Results"));
                if (this.OutputImage != null) display.SetImage(this.OutputImage, record, graphics);
                else
                {
                    record = records.SubRecords[this.InputImageName];
                    display.SetImage(this.InputImage, record, graphics);
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message, "DisplayResult"); }
        }

        public void DisplayResult(RecordDisplay display, ICogImage image)
        {
            //display.SetImage(image);
        }

        private static CogColorConstants GraphicColor(결과구분 판정)
        {
            if (판정 == 결과구분.OK) return CogColorConstants.DarkGreen;
            if (판정 == 결과구분.NG) return CogColorConstants.Red;
            if (판정 == 결과구분.ER) return CogColorConstants.Magenta;
            return CogColorConstants.LightGrey;
        }

        public static CogGraphicLabel CreateLabel(String text, Double x, Double y, Single size, CogColorConstants color, String spaceName, CogGraphicLabelAlignmentConstants alignment = CogGraphicLabelAlignmentConstants.TopCenter)
        {
            CogGraphicLabel label = new CogGraphicLabel() { Text = text, X = x, Y = y, Color = color, SelectedSpaceName = spaceName, Alignment = alignment };
            label.Font = new Font(label.Font.FontFamily, size, FontStyle.Bold);
            return label;
        }
        #endregion

        #region 이미지 저장
        public void SaveDisplayImage(String path)
        {
            if (this.Display == null) return;
            try {
                this.Display.CreateContentBitmap(CogDisplayContentBitmapConstants.Custom).Save(path, ImageFormat.Jpeg);
            }
            catch (Exception ex) {
                Global.로그기록(로그영역, 로그구분.오류, "자료저장", $"결과 이미지 저장 오류 : {ex.Message}");
            }
        }
        #endregion
    }
}
