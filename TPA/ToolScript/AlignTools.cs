using System;
using System.Diagnostics;
using Cognex.VisionPro;
using Cognex.VisionPro.ToolBlock;
using Cognex.VisionPro.ToolGroup;
using Cognex.VisionPro.Caliper;
using Cognex.VisionPro.Dimensioning;
using Cognex.VisionPro.CalibFix;

public class AlignTools : CogToolBlockAdvancedScriptBase
{
    private CogToolBlock ToolBlock;
    private CogImage8Grey InputImage { get { return Input<CogImage8Grey>("InputImage"); } }

    private CogFixtureTool Fixture { get { return GetTool("Fixture") as CogFixtureTool; } }
    private CogTransform2DLinear Transform { get { return Fixture.RunParams.UnfixturedFromFixturedTransform as CogTransform2DLinear; } }
    private CogFindLineTool LineH { get { return GetTool("LineH") as CogFindLineTool; } }
    private CogFindLineTool LineV { get { return GetTool("LineV") as CogFindLineTool; } }
    private CogIntersectLineLineTool PointR { get { return GetTool("PointR") as CogIntersectLineLineTool; } } // 기준점
    private Double Width { get { return Input<Double>("Width"); } }
    private Double Height { get { return Input<Double>("Height"); } }
    private Double CalibX { get { return Input<Double>("CalibX"); } }
    private Double CalibY { get { return Input<Double>("CalibY"); } }
    private Double OriginX { get { return Input<Double>("OriginX"); } } // 마스터 기준 Transform 원점 X
    private Double OriginY { get { return Input<Double>("OriginY"); } } // 마스터 기준 Transform 원점 Y
    private Double OriginF { get { return Input<Double>("OriginF"); } } // 기준점 대비 X축 중심점을 찿는 방향

    private void SetFixture()
    {
        if (PointR.RunStatus.Result == CogToolResultConstants.Accept) {
            Double r1 = LineH.Results.GetLine().Rotation;
            Double r2 = LineV.Results.GetLine().Rotation + Math.PI / 2;
            Transform.Rotation = (r1 + r2) / 2; // 두 축의 틀어진 정도의 평균을 구함
            Transform.Rotation = (LineH.Results.GetLine().Rotation + LineV.Results.GetLine().Rotation + Math.PI / 2) / 2; // 두 축의 틀어진 정도의 평균을 구함
            Transform.TranslationX = Math.Round(PointR.X + OriginF * Width / CalibX / 2); // 임시
            Transform.TranslationY = Math.Round(PointR.Y + Height / CalibY / 2);
        }
        else {
            Transform.Rotation = 0;
            Transform.TranslationX = OriginX;
            Transform.TranslationY = OriginY;
        }
        Transform.Skew = 0;
        Output("Rotation", -Transform.Rotation);
    }

    public override Boolean GroupRun(ref string message, ref CogToolResultConstants result)
    {
        Output("Rotation", 0);
        foreach (ICogTool tool in ToolBlock.Tools) {
            ToolBlock.RunTool(tool, ref message, ref result);
            if (tool == PointR) SetFixture();
        }
        return false;
    }

    public override void Initialize(CogToolGroup host)
    {
        base.Initialize(host);
        ToolBlock = host as CogToolBlock;
    }

    #region Methods
    private Double ToAngle(Double radian) { return radian / Math.PI * 180; }
    private ICogTool GetTool(String name)
    {
        if (ToolBlock.Tools.Contains(name)) return ToolBlock.Tools[name];
        return null;
    }
    private T Input<T>(String name)
    {
        Object v = null;
        if (ToolBlock.Inputs.Contains(name)) v = ToolBlock.Inputs[name].Value;
        if (v == null) return default(T);
        return (T)v;
    }
    private Boolean Input(String name, Object value)
    {
        if (!ToolBlock.Inputs.Contains(name)) return false;
        ToolBlock.Inputs[name].Value = value;
        return true;
    }
    private T Output<T>(String name)
    {
        Object v = null;
        if (ToolBlock.Outputs.Contains(name)) v = ToolBlock.Outputs[name].Value;
        if (v == null) return default(T);
        return (T)v;
    }
    private Boolean Output(String name, Object value)
    {
        if (!ToolBlock.Outputs.Contains(name)) return false;
        ToolBlock.Outputs[name].Value = value;
        return true;
    }
    #endregion
}

