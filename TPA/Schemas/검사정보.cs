using Cognex.VisionPro;
using DevExpress.DataProcessing.InMemoryDataProcessor.GraphGenerator;
using DevExpress.Xpf.LayoutControl;
using MvUtils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace TPA.Schemas
{
    public class CameraAttribute : Attribute
    {
        public Boolean Whether = true;
        public CameraAttribute(Boolean cam) { Whether = cam; }

        public static Boolean IsCamera(장치구분 구분)
        {
            CameraAttribute a = Utils.GetAttribute<CameraAttribute>(구분);
            if (a == null) return false;
            return a.Whether;
        }
    }


    public class ResultAttribute : Attribute
    {
        public 검사그룹 검사그룹 = 검사그룹.None;
        public 결과분류 결과분류 = 결과분류.None;
        public 장치구분 장치구분 = 장치구분.None;
        public String 변수명칭 = String.Empty;

        public ResultAttribute() { }
        public ResultAttribute(검사그룹 그룹, 결과분류 결과, 장치구분 장치) { 검사그룹 = 그룹; 장치구분 = 장치; 결과분류 = 결과; }
        public ResultAttribute(검사그룹 그룹, 결과분류 결과, 장치구분 장치, String 변수) { 검사그룹 = 그룹; 장치구분 = 장치; 결과분류 = 결과; 변수명칭 = 변수; }
        public static String VarName(검사항목 항목)
        {
            ResultAttribute a = Utils.GetAttribute<ResultAttribute>(항목);
            if (a == null) return String.Empty;
            return a.변수명칭;
        }
    }


    public enum 장치구분
    {
        [Description("None"), DXDescription("NO"), Camera(false)]
        None,
        [Description("Cam01"), DXDescription("C1"), Camera(true)]
        Cam01 = 카메라구분.Cam01,
        [Description("Cam02"), DXDescription("C2"), Camera(true)]
        Cam02 = 카메라구분.Cam02,
        [Description("Cam03"), DXDescription("C3"), Camera(true)]
        Cam03 = 카메라구분.Cam03,
        [Description("Cam04"), DXDescription("C4"), Camera(true)]
        Cam04 = 카메라구분.Cam04,
        [Description("Cam05"), DXDescription("C5"), Camera(true)]
        Cam05 = 카메라구분.Cam05,
        [Description("Cam06"), DXDescription("C6"), Camera(true)]
        Cam06 = 카메라구분.Cam06,
        [Description("Cam07"), DXDescription("C7"), Camera(true)]
        Cam07 = 카메라구분.Cam07,
        [Description("Cam08"), DXDescription("C8"), Camera(true)]
        Cam08 = 카메라구분.Cam08,
        [Description("Flatness"), DXDescription("FL"), Camera(false)]
        Flatness,
        [Description("Spacing"), DXDescription("SP"), Camera(false)]
        Spacing,
        [Description("QrCode"), DXDescription("QC"), Camera(false)]
        QRCode,
        [Description("QrMark"), DXDescription("QM"), Camera(false)]
        QRMark,
    }


    public enum 노멀미러
    {
        [ListBindable(false)]
        None = 0,
        Normal = 1,
        Mirror = 2,
    }


    public enum 결과분류
    {
        None,
        Summary,
        Detail,
    }


    public enum 검사그룹
    {
        [Description("None"), Translation("None", "없음")]
        None,
        [Description("CTQ"), Translation("CTQ")]
        CTQ,
        [Description("Surface"), Translation("Surface", "외관검사")]
        Surface,
    }


    public enum 검사항목 : Int32
    {
        [Result(), ListBindable(false)]
        None,
        [Result(검사그룹.Surface, 결과분류.Summary, 장치구분.QRCode)]
        하부큐알코드1 = 101,
        [Result(검사그룹.Surface, 결과분류.Summary, 장치구분.QRCode)]
        하부큐알코드2 = 102,

        [Result(검사그룹.Surface, 결과분류.Summary, 장치구분.Cam03, "InputDirection")]
        역투입 = 103,
        [Result(검사그룹.Surface, 결과분류.Summary, 장치구분.Cam06)]
        커넥터설삽상부 = 95,
        [Result(검사그룹.Surface, 결과분류.Detail, 장치구분.Cam06, "DistanceCntL")]
        커넥터설삽상부L = 96,
        [Result(검사그룹.Surface, 결과분류.Detail, 장치구분.Cam06, "DistanceCntR")]
        커넥터설삽상부R = 97,
        [Result(검사그룹.Surface, 결과분류.Summary, 장치구분.Cam07)]
        커넥터설삽하부 = 98,
        [Result(검사그룹.Surface, 결과분류.Detail, 장치구분.Cam07, "DistanceCntL")]
        커넥터설삽하부L = 99,
        [Result(검사그룹.Surface, 결과분류.Detail, 장치구분.Cam07, "DistanceCntR")]
        커넥터설삽하부R = 100,

        [Result(검사그룹.CTQ, 결과분류.Detail, 장치구분.Flatness)]
        데이텀A1_F = 56,
        [Result(검사그룹.CTQ, 결과분류.Detail, 장치구분.Flatness)]
        데이텀A2_F = 57,
        [Result(검사그룹.CTQ, 결과분류.Detail, 장치구분.Flatness)]
        데이텀A3_F = 58,
        [Result(검사그룹.CTQ, 결과분류.Detail, 장치구분.Flatness)]
        데이텀A4_F = 59,

        [Result(검사그룹.CTQ, 결과분류.Summary, 장치구분.Flatness)]
        바닥평면도 = 55,
        [Result(검사그룹.CTQ, 결과분류.Detail, 장치구분.Flatness)]
        변위센서a1 = 60,
        [Result(검사그룹.CTQ, 결과분류.Detail, 장치구분.Flatness)]
        변위센서a2 = 61,
        [Result(검사그룹.CTQ, 결과분류.Detail, 장치구분.Flatness)]
        변위센서a3 = 62,
        [Result(검사그룹.CTQ, 결과분류.Detail, 장치구분.Flatness)]
        변위센서a4 = 63,
        [Result(검사그룹.CTQ, 결과분류.Detail, 장치구분.Flatness)]
        변위센서a5 = 64,
        [Result(검사그룹.CTQ, 결과분류.Detail, 장치구분.Flatness)]
        변위센서a6 = 65,
        [Result(검사그룹.CTQ, 결과분류.Detail, 장치구분.Flatness)]
        변위센서a7 = 66,
        [Result(검사그룹.CTQ, 결과분류.Detail, 장치구분.Flatness)]
        변위센서a8 = 67,

        [Result(검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam01, "ThicknessF01")]
        가공부높이f1 = 23,
        [Result(검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam01, "ThicknessF02")]
        가공부높이f2 = 24,
        [Result(검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam01, "ThicknessF03")]
        가공부높이f3 = 25,
        [Result(검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam01, "ThicknessF04")]
        가공부높이f4 = 26,
        [Result(검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam01, "ThicknessF05")]
        가공부높이f5 = 27,
        [Result(검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam01, "ThicknessF06")]
        가공부높이f6 = 28,
        [Result(검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam01, "ThicknessF07")]
        가공부높이f7 = 29,
        [Result(검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam01, "ThicknessF08")]
        가공부높이f8 = 30,
        [Result(검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam02, "ThicknessF09")]
        가공부높이f9 = 31,
        [Result(검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam02, "ThicknessF10")]
        가공부높이f10 = 32,
        [Result(검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam02, "ThicknessF11")]
        가공부높이f11 = 33,
        [Result(검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam02, "ThicknessF12")]
        가공부높이f12 = 34,
        [Result(검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam02, "ThicknessF13")]
        가공부높이f13 = 35,
        [Result(검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam02, "ThicknessF14")]
        가공부높이f14 = 36,
        [Result(검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam02, "ThicknessF15")]
        가공부높이f15 = 37,
        [Result(검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam02, "ThicknessF16")]
        가공부높이f16 = 38,

        [Result(검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam03, "TrimHF")]
        선윤곽도H_F = 5,
        [Result(검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam03, "TrimHR")]
        선윤곽도H_R = 6,
        [Result(검사그룹.CTQ, 결과분류.Detail, 장치구분.Cam03, "TrimH01")]
        거리측정h1 = 7,
        [Result(검사그룹.CTQ, 결과분류.Detail, 장치구분.Cam03, "TrimH02")]
        거리측정h2 = 8,
        [Result(검사그룹.CTQ, 결과분류.Detail, 장치구분.Cam03, "TrimH03")]
        거리측정h3 = 9,
        [Result(검사그룹.CTQ, 결과분류.Detail, 장치구분.Cam03, "TrimH04")]
        거리측정h4 = 10,

        [Result(검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam03, "TrimJF")]
        선윤곽도J_F = 71,
        [Result(검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam03, "TrimJR")]
        선윤곽도J_R = 72,
        [Result(검사그룹.CTQ, 결과분류.Detail, 장치구분.Cam03, "TrimJ01")]
        거리측정J1 = 73,
        [Result(검사그룹.CTQ, 결과분류.Detail, 장치구분.Cam03, "TrimJ02")]
        거리측정J2 = 74,
        [Result(검사그룹.CTQ, 결과분류.Detail, 장치구분.Cam03, "TrimJ03")]
        거리측정J3 = 75,
        [Result(검사그룹.CTQ, 결과분류.Detail, 장치구분.Cam03, "TrimJ04")]
        거리측정J4 = 76,

        [Result(검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam03, "TrimF")]
        선윤곽도_F = 68,
        [Result(검사그룹.CTQ, 결과분류.Detail, 장치구분.Cam03, "TrimL")]
        거리측정L = 69,
        [Result(검사그룹.CTQ, 결과분류.Detail, 장치구분.Cam03, "TrimR")]
        거리측정R = 70,

        [Result(검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam03, "Width01")]
        측면부폭1 = 1,
        [Result(검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam03, "Width02")]
        측면부폭2 = 2,
        [Result(검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam03, "Width03")]
        측면부폭3 = 3,
        [Result(검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam03, "Width04")]
        측면부폭4 = 4,

        [Result(검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam03, "CenterNotchL_F")]
        중심노치L반폭F = 79,
        [Result(검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam03, "CenterNotchL_R")]
        중심노치L반폭R = 80,
        [Result(검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam03, "CenterNotchR_F")]
        중심노치R반폭F = 81,
        [Result(검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam03, "CenterNotchR_R")]
        중심노치R반폭R = 82,
        [Result(검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam03, "JNotchHalfWidth01_F")]
        노치J01반폭F = 87,
        [Result(검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam03, "JNotchHalfWidth01_R")]
        노치J01반폭R = 88,
        [Result(검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam03, "JNotchHalfWidth02_F")]
        노치J02반폭F = 89,
        [Result(검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam03, "JNotchHalfWidth02_R")]
        노치J02반폭R = 90,
        [Result(검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam03, "JNotchHalfWidth03_F")]
        노치J03반폭F = 91,
        [Result(검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam03, "JNotchHalfWidth03_R")]
        노치J03반폭R = 92,
        [Result(검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam03, "JNotchHalfWidth04_F")]
        노치J04반폭F = 93,
        [Result(검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam03, "JNotchHalfWidth04_R")]
        노치J04반폭R = 94,

        [Result(검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam03, "CenterNotchLDepth")]
        중심노치L깊이 = 77,
        [Result(검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam03, "CenterNotchRDepth")]
        중심노치R깊이 = 78,
        [Result(검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam03, "JNotchDepth01")]
        노치J01깊이 = 83,
        [Result(검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam03, "JNotchDepth02")]
        노치J02깊이 = 84,
        [Result(검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam03, "JNotchDepth03")]
        노치J03깊이 = 85,
        [Result(검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam03, "JNotchDepth04")]
        노치J04깊이 = 86,

        [Result(검사그룹.Surface, 결과분류.Summary, 장치구분.Cam03)]
        장축용접불량 = 111,
        [Result(검사그룹.Surface, 결과분류.Summary, 장치구분.Cam03)]
        단축용접불량 = 112,
        [Result(검사그룹.Surface, 결과분류.Summary, 장치구분.Cam03)]
        바닥표면불량 = 113,

        [Result(검사그룹.Surface, 결과분류.Summary, 장치구분.QRCode)]
        상부큐알코드1 = 109,
        [Result(검사그룹.Surface, 결과분류.Summary, 장치구분.QRCode)]
        상부큐알코드2 = 110,

        [Result(검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam04, "ThicknessG01")]
        가공부높이g1 = 39,
        [Result(검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam04, "ThicknessG02")]
        가공부높이g2 = 40,
        [Result(검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam04, "ThicknessG03")]
        가공부높이g3 = 41,
        [Result(검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam04, "ThicknessG04")]
        가공부높이g4 = 42,
        [Result(검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam04, "ThicknessG05")]
        가공부높이g5 = 43,
        [Result(검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam04, "ThicknessG06")]
        가공부높이g6 = 44,
        [Result(검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam04, "ThicknessG07")]
        가공부높이g7 = 45,
        [Result(검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam04, "ThicknessG08")]
        가공부높이g8 = 46,
        [Result(검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam05, "ThicknessG09")]
        가공부높이g9 = 47,
        [Result(검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam05, "ThicknessG10")]
        가공부높이g10 = 48,
        [Result(검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam05, "ThicknessG11")]
        가공부높이g11 = 49,
        [Result(검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam05, "ThicknessG12")]
        가공부높이g12 = 50,
        [Result(검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam05, "ThicknessG13")]
        가공부높이g13 = 51,
        [Result(검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam05, "ThicknessG14")]
        가공부높이g14 = 52,
        [Result(검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam05, "ThicknessG15")]
        가공부높이g15 = 53,
        [Result(검사그룹.CTQ, 결과분류.Summary, 장치구분.Cam05, "ThicknessG16")]
        가공부높이g16 = 54,



        [Result(검사그룹.CTQ, 결과분류.Detail, 장치구분.Flatness)]
        데이텀A1_R = 105,
        [Result(검사그룹.CTQ, 결과분류.Detail, 장치구분.Flatness)]
        데이텀A2_R = 106,
        [Result(검사그룹.CTQ, 결과분류.Detail, 장치구분.Flatness)]
        데이텀A3_R = 107,
        [Result(검사그룹.CTQ, 결과분류.Detail, 장치구분.Flatness)]
        데이텀A4_R = 108,



        [Result(검사그룹.CTQ, 결과분류.Summary, 장치구분.Flatness)]
        면윤곽도 = 104,
        [Result(검사그룹.CTQ, 결과분류.Summary, 장치구분.Flatness)]
        커버들뜸 = 11,



        [Result(검사그룹.CTQ, 결과분류.Detail, 장치구분.Flatness)]
        커버상m1 = 12,
        [Result(검사그룹.CTQ, 결과분류.Detail, 장치구분.Flatness)]
        커버상m2 = 13,
        [Result(검사그룹.CTQ, 결과분류.Detail, 장치구분.Flatness)]
        커버상m3 = 14,
        [Result(검사그룹.CTQ, 결과분류.Detail, 장치구분.Flatness)]
        커버들뜸k1 = 15,
        [Result(검사그룹.CTQ, 결과분류.Detail, 장치구분.Flatness)]
        커버들뜸k2 = 16,
        [Result(검사그룹.CTQ, 결과분류.Detail, 장치구분.Flatness)]
        커버들뜸k3 = 17,
        [Result(검사그룹.CTQ, 결과분류.Detail, 장치구분.Flatness)]
        커버들뜸k4 = 18,
        [Result(검사그룹.CTQ, 결과분류.Detail, 장치구분.Flatness)]
        커버들뜸k5 = 19,
        [Result(검사그룹.CTQ, 결과분류.Detail, 장치구분.Flatness)]
        커버들뜸k6 = 20,
        [Result(검사그룹.CTQ, 결과분류.Detail, 장치구분.Flatness)]
        커버들뜸k7 = 21,
        [Result(검사그룹.CTQ, 결과분류.Detail, 장치구분.Flatness)]
        커버들뜸k8 = 22,



        [Result(검사그룹.Surface, 결과분류.Summary, 장치구분.Cam03, "MarkingResult")]
        노멀미러 = 114,




        [Result(검사그룹.Surface, 결과분류.Summary, 장치구분.Cam03, "Dent")]
        찍힘불량 = 115,
        [Result(검사그룹.Surface, 결과분류.Detail, 장치구분.Cam03, "DentB")]
        찍힘불량B = 116,
        [Result(검사그룹.Surface, 결과분류.Detail, 장치구분.Cam03, "DentTL")]
        찍힘불량TL = 117,
        [Result(검사그룹.Surface, 결과분류.Detail, 장치구분.Cam03, "DentTR")]
        찍힘불량TR = 118,
        [Result(검사그룹.Surface, 결과분류.Detail, 장치구분.Cam03, "DentL")]
        찍힘불량L = 119,
        [Result(검사그룹.Surface, 결과분류.Detail, 장치구분.Cam03, "DentR")]
        찍힘불량R = 120,



        [Result(검사그룹.Surface, 결과분류.Summary, 장치구분.Cam03, "Scratch")]
        스크레치 = 121,
        [Result(검사그룹.Surface, 결과분류.Detail, 장치구분.Cam03, "ScratchB")]
        스크레치B = 122,
        [Result(검사그룹.Surface, 결과분류.Detail, 장치구분.Cam03, "ScratchTL")]
        스크레치TL = 123,
        [Result(검사그룹.Surface, 결과분류.Detail, 장치구분.Cam03, "ScratchTR")]
        스크레치TR = 124,
        [Result(검사그룹.Surface, 결과분류.Detail, 장치구분.Cam03, "ScratchL")]
        스크레치L = 125,
        [Result(검사그룹.Surface, 결과분류.Detail, 장치구분.Cam03, "ScratchR")]
        스크레치R = 126,



        [Result(검사그룹.Surface, 결과분류.Summary, 장치구분.None)]
        큐알검증 = 127,
        [Result(검사그룹.Surface, 결과분류.Summary, 장치구분.None)]
        중복여부 = 128,



        [Result(검사그룹.Surface, 결과분류.Summary, 장치구분.Cam03, "PlusMarkingResult")]
        플러스각인 = 1201,
        [Result(검사그룹.Surface, 결과분류.Summary, 장치구분.Cam03, "MinusMarkingResult")]
        마이너스각인 = 1202,
        
        [Result(검사그룹.Surface, 결과분류.Summary, 장치구분.Cam03, "BraketMarkingResult1")]
        브라켓각인1 = 1203,
        [Result(검사그룹.Surface, 결과분류.Summary, 장치구분.Cam03, "BraketMarkingResult2")]
        브라켓각인2 = 1204,
        [Result(검사그룹.Surface, 결과분류.Summary, 장치구분.Cam03, "BraketMarkingResult3")]
        브라켓각인3 = 1205,
        [Result(검사그룹.Surface, 결과분류.Summary, 장치구분.Cam03, "BraketMarkingResult4")]
        브라켓각인4 = 1206,
        [Result(검사그룹.Surface, 결과분류.Summary, 장치구분.Cam03, "BraketMarkingResult5")]
        브라켓각인5 = 1207,
        [Result(검사그룹.Surface, 결과분류.Summary, 장치구분.Cam03, "BraketMarkingResult6")]
        브라켓각인6 = 1208,
        
        [Result(검사그룹.Surface, 결과분류.Summary, 장치구분.Cam03, "MarkingResult1")]
        직선각인1 = 1209,
        [Result(검사그룹.Surface, 결과분류.Summary, 장치구분.Cam03, "MarkingResult2")]
        직선각인2 = 1210,


    }
    public enum 단위구분
    {
        [Description("mm")]
        mm = 0,
        [Description("ø")]
        ø = 1,
        [Description("µm")]
        µm = 2,
        [Description("px")]
        px = 3,
    }

    public enum 결과구분
    {
        [Description("Waiting"), Translation("Waiting", "대기중")]
        NO = 0,
        [Description("Measuring"), Translation("Measuring", "검사중")]
        IN = 1,
        [Description("PS"), Translation("Pass", "통과")]
        PS = 2,
        [Description("ER"), Translation("Error", "오류")]
        ER = 3,
        [Description("NG"), Translation("NG", "불량")]
        NG = 5,
        [Description("OK"), Translation("OK", "양품")]
        OK = 7,
    }


    public enum 역투입여부
    {
        None,
        Normal,
        Reverse,
    }


    public enum 각인유무
    {
        None,
        Absence,
        Presence,
    }


    public enum 커넥터설삽여부
    {
        None,
        OK,
        NG,
    }



    public enum MES응답
    {
        None,
        OK,
        NG,
    }



    public enum 재검사여부
    {
        FALSE,
        TRUE,
    }



    [Table("inspd")]
    public class 검사정보
    {
        [Column("idwdt", Order = 0), Required, Key, JsonProperty("idwdt"), Translation("Time", "검사일시")]
        public DateTime 검사일시 { get; set; } = DateTime.Now;
        [Column("iditm", Order = 1), Required, Key, JsonProperty("iditm"), Translation("Item", "검사항목")]
        public 검사항목 검사항목 { get; set; } = 검사항목.None;
        [Column("idgrp"), JsonProperty("idgrp"), Translation("Group", "검사그룹"),     BatchEdit(true)]
        public 검사그룹 검사그룹 { get; set; } = 검사그룹.None;
        [Column("iddev"), JsonProperty("iddev"), Translation("Device", "검사장치"),    BatchEdit(true)]
        public 장치구분 검사장치 { get; set; } = 장치구분.None;
        [Column("idcat"), JsonProperty("idcat"), Translation("Category", "결과유형"),  BatchEdit(true)]
        public 결과분류 결과분류 { get; set; } = 결과분류.None;
        [Column("iduni"), JsonProperty("iduni"), Translation("Unit", "단위"),         BatchEdit(true)]
        public 단위구분 측정단위 { get; set; } = 단위구분.mm;
        [Column("idstd"), JsonProperty("idstd"), Translation("Standard", "기준값"),    BatchEdit(true)]
        public Decimal 기준값 { get; set; } = 0m;
        [Column("idmin"), JsonProperty("idmin"), Translation("Min", "최소값"),         BatchEdit(true)]
        public Decimal 최소값 { get; set; } = 0m;
        [Column("idmax"), JsonProperty("idmax"), Translation("Max", "최대값"),         BatchEdit(true)]
        public Decimal 최대값 { get; set; } = 0m;
        [Column("idoff"), JsonProperty("idoff"), Translation("Offset", "보정값"),      BatchEdit(true)]
        public Decimal 보정값 { get; set; } = 0m;
        [Column("idcal"), JsonProperty("idcal"), Translation("Calib(µm)", "교정(µm)"), BatchEdit(true)]
        public Decimal 교정값 { get; set; } = 1m;
        [Column("idmes"), JsonProperty("idmes"), Translation("Measure", "측정값")]
        public Decimal 측정값 { get; set; } = 0m;
        [Column("idval"), JsonProperty("idval"), Translation("Value", "결과값")]
        public Decimal 결과값 { get; set; } = 0m;
        [Column("idres"), JsonProperty("idres"), Translation("Result", "판정")]
        public 결과구분 측정결과 { get; set; } = 결과구분.NO;
        [NotMapped, JsonProperty("iduse"), Translation("Used", "검사"), BatchEdit(true)]
        public Boolean 검사여부 { get; set; } = true;
        [NotMapped, JsonIgnore]
        public Double 검사시간 = 0;
        [NotMapped, JsonIgnore]
        public String 변수명칭 = String.Empty;
        [NotMapped, JsonIgnore]
        public Boolean 카메라여부 = false;
        public 검사정보() { }
        public 검사정보(검사정보 정보) { this.Set(정보); }

        public void Reset(DateTime? 일시 = null)
        {
            if (일시 != null) this.검사일시 = (DateTime)일시;
            this.측정값 = 0;
            this.결과값 = 0;
            this.측정결과 = 결과구분.IN;
        }

        public void Set(검사정보 정보)
        {
            if (정보 == null) return;
            foreach (PropertyInfo p in typeof(검사정보).GetProperties())
            {
                if (!p.CanWrite) continue;
                Object v = p.GetValue(정보);
                if (v == null) continue;
                p.SetValue(this, v);
            }

            this.측정값 = 0;
            this.결과값 = 0;
            this.측정결과 = 결과구분.NO;

            this.카메라여부 = CameraAttribute.IsCamera(this.검사장치);
            this.변수명칭 = ResultAttribute.VarName(this.검사항목);
        }
    }


    [Table("inspl")]
    public class 검사결과
    {
        [Column("ilwdt"), Required, Key, JsonProperty("ilwdt"), Translation("Time", "일시")]
        public DateTime 검사일시 { get; set; } = DateTime.Now;
        [Column("ilmcd"), JsonProperty("ilmcd"), Translation("Model", "모델")]
        public 모델구분 모델구분 { get; set; } = 모델구분.None;
        [Column("ilqrffcR"), JsonProperty("ilqrffcR"), Translation("QR_FCC_R", "QR_FCC_R")]
        public String 하부큐알코드1 { get; set; } = String.Empty;
        [Column("ilqrffcF"), JsonProperty("ilqrffcF"), Translation("QR_FCC_F", "QR_FCC_F")]
        public String 하부큐알코드2 { get; set; } = String.Empty;
        [Column("ilqrcsc1"), JsonProperty("ilqrcsc1"), Translation("QR_CSC1", "QR_CSC1")]
        public String 상부큐알코드1 { get; set; } = String.Empty;
        [Column("ilqrcsc2"), JsonProperty("ilqrcsc2"), Translation("QR_CSC2", "QR_CSC2")]
        public String 상부큐알코드2 { get; set; } = String.Empty;
        [Column("ilinputDir"), JsonProperty("ilinputDir"), Translation("InputDir", "투입방향")]
        public 역투입여부 역투입여부 { get; set; } = 역투입여부.None;
        [Column("ilnum"), JsonProperty("ilnum"), Translation("Index", "제품인덱스")]
        public Int32 검사코드 { get; set; } = 0;
        [Column("ilres"), JsonProperty("ilres"), Translation("Result", "측정결과")]
        public 결과구분 측정결과 { get; set; } = 결과구분.NO;
        [Column("ilctq"), JsonProperty("ilctq"), Translation("CTQResult", "CTQ결과")]
        public 결과구분 CTQ결과 { get; set; } = 결과구분.NO;
        [Column("ilsurf"), JsonProperty("ilsurf"), Translation("SurfaceResult", "외관결과")]
        public 결과구분 외관결과 { get; set; } = 결과구분.NO;
        [Column("ilnom"), JsonProperty("ilnom"), Translation("NormalMirror", "노멀미러")]
        public 노멀미러 노멀미러 { get; set; } = 노멀미러.None;
        [Column("ilngs"), JsonProperty("ilngs"), Translation("NGInfo.", "불량정보")]
        public String 불량정보 { get; set; } = String.Empty;
        [Column("ilcnt1"), JsonProperty("ilcnt1"), Translation("Cnt1", "커넥터설삽상부")]
        public 커넥터설삽여부 커넥터설삽상부 { get; set; } = 커넥터설삽여부.None;
        [Column("ilcnt2"), JsonProperty("ilcnt2"), Translation("Cnt2", "커넥터설삽하부")]
        public 커넥터설삽여부 커넥터설삽하부 { get; set; } = 커넥터설삽여부.None;
        [Column("ilmes"), JsonProperty("ilmes"), Translation("MESResponce", "MES응답")]
        public MES응답 MES응답 { get; set; } = MES응답.None;
        [Column("ilret"), JsonProperty("ilret"), Translation("Retest", "재검사여부")]
        public 재검사여부 재검사여부 { get; set; } = 재검사여부.FALSE;

        [NotMapped, JsonProperty("inspd")]
        public List<검사정보> 검사내역 { get; set; } = new List<검사정보>();
        [NotMapped, JsonIgnore]
        public List<String> 불량내역 = new List<String>();
        [NotMapped, JsonProperty("ilreg")]
        public List<불량영역> 표면불량 { get; set; } = new List<불량영역>();


        public 검사결과()
        {
            this.검사일시 = DateTime.Now;
            this.모델구분 = Global.환경설정.선택모델;
        }


        public void Reset()
        {
            this.검사일시 = DateTime.Now;
            this.모델구분 = Global.환경설정.선택모델;
            this.측정결과 = 결과구분.NO;
            this.CTQ결과 = 결과구분.NO;
            this.외관결과 = 결과구분.NO;
            this.노멀미러 = 노멀미러.None;
            this.역투입여부 = 역투입여부.None;
            this.하부큐알코드1 = String.Empty;
            this.하부큐알코드2 = String.Empty;
            this.상부큐알코드1 = String.Empty;
            this.상부큐알코드2 = String.Empty;
            this.커넥터설삽상부 = 커넥터설삽여부.None;
            this.커넥터설삽하부 = 커넥터설삽여부.None;
            this.불량정보 = String.Empty;
            this.불량내역.Clear();
            this.검사내역.Clear();

            검사설정자료 자료 = Global.모델자료.GetItem(this.모델구분)?.검사설정;
            foreach (검사정보 정보 in 자료)
            {
                if (!정보.검사여부) continue;
                this.검사내역.Add(new 검사정보(정보) { 검사일시 = this.검사일시 });
            }
        }

        public 검사정보 GetItem(장치구분 장치, String name) => 검사내역.Where(e => e.검사장치 == 장치 && e.변수명칭 == name).FirstOrDefault();
        public 검사정보 GetItem(검사항목 항목) => 검사내역.Where(e => e.검사항목 == 항목).FirstOrDefault();

        // 카메라 검사결과 적용
        public 검사정보 SetResult(String name, Double value) => SetResult(검사내역.Where(e => e.검사항목.ToString() == name).FirstOrDefault(), value);
        public Boolean SetResult(String name, Single value, Boolean ok) => SetResult(검사내역.Where(e => e.검사항목.ToString() == name).FirstOrDefault(), value, ok);
        public Boolean SetResult(검사항목 항목, Single value, Boolean ok) => SetResult(검사내역.Where(e => e.검사항목 == 항목).FirstOrDefault(), value, ok);
        public 검사정보 SetResult(검사정보 검사, Double value)
        {
            if (검사 == null) return null;
            if (Double.IsNaN(value)) { 검사.측정결과 = 결과구분.ER; return 검사; }

            검사.측정값 = (Decimal)Math.Round(value, Global.환경설정.결과자릿수);
            if (검사.카메라여부)
            {
                if (검사.검사항목 == 검사항목.노멀미러 || 검사.검사항목 == 검사항목.역투입)
                {
                    검사.결과값 = 검사.측정값;
                }
                else if (검사.검사항목 == 검사항목.마이너스각인 || 검사.검사항목 == 검사항목.플러스각인)
                {
                    검사.결과값 = 검사.측정값;
                }
                else
                {
                    if (검사.교정값 > 0) 검사.결과값 = Math.Round(검사.측정값 * 검사.교정값, Global.환경설정.결과자릿수);
                    else 검사.결과값 = 검사.측정값;
                    검사.결과값 += 검사.보정값;
                }
            }
            else
            {
                검사.결과값 = 검사.측정값 + 검사.보정값;
                if (검사.검사장치 == 장치구분.Flatness) 검사.결과값 += 검사.교정값;
            }

            Boolean ok = 검사.결과값 >= 검사.최소값 && 검사.결과값 <= 검사.최대값;
            검사.측정결과 = ok ? 결과구분.OK : 결과구분.NG;
            return 검사;
        }
        public Boolean SetResult(검사정보 검사, Single value, Boolean ok)
        {
            if (검사 == null) return false;
            if (Single.IsNaN(value))
            {
                검사.측정결과 = 결과구분.ER;
                return false;
            }
            검사.결과값 = (Decimal)Math.Round(value, Global.환경설정.결과자릿수);
            검사.측정값 = 검사.결과값;
            검사.측정결과 = ok ? 결과구분.OK : 결과구분.NG;
            return true;
        }
        public void SetResults(카메라구분 카메라, Dictionary<String, Object> results)
        {
            //불량영역제거(카메라);
            //Debug.WriteLine("1");
            //String scratch = ResultAttribute.VarName(검사항목.스크레치);
            //String dent = ResultAttribute.VarName(검사항목.찍힘불량);
            //Debug.WriteLine("2");
            String markingResult = ResultAttribute.VarName(검사항목.노멀미러);
            String inputdir = ResultAttribute.VarName(검사항목.역투입);
            //.WriteLine("3");
            foreach (var result in results)
            {
                //if (result.Key.Equals(scratch) || result.Key.Equals(dent))
                //{
                //    this.표면불량.AddRange(result.Value as List<불량영역>);
                //    continue;
                //}

                검사정보 정보 = GetItem((장치구분)카메라, result.Key); // result.Key : 검사항목명
                if (정보 == null) continue;

                if (result.Key.Equals(markingResult))
                {
                    if (result.Value.Equals("None"))
                        SetResult(정보, 0);
                    else if (result.Value.Equals("Normal"))
                        SetResult(정보, 1);
                    else if (result.Value.Equals("Mirror"))
                        SetResult(정보, 2);
                }
                else if (result.Key.Equals(inputdir))
                {
                    Double value = result.Value == null ? Double.NaN : (Double)result.Value;
                    if (value > 0.4)
                        SetResult(정보, 1);
                    else
                        SetResult(정보, 2);
                }
                else
                {
                    Double value = result.Value == null ? Double.NaN : (Double)result.Value;
                    SetResult(정보, value);
                }
            }
        }

        public List<불량영역> 불량영역(카메라구분 카메라) => this.표면불량.Where(e => e.장치구분 == (장치구분)카메라).ToList();
        public void 불량영역제거(카메라구분 카메라)
        {
            List<불량영역> 불량 = 불량영역(카메라);
            불량.ForEach(e => this.표면불량.Remove(e));
        }

        // 일반 검사결과 적용
        public 검사정보 SetResult(검사항목 항목, Single value) => SetResult(검사내역.Where(e => e.검사항목 == 항목).FirstOrDefault(), value);
        public 검사정보 SetResult(String name, Single value) => SetResult(검사내역.Where(e => e.검사항목.ToString() == name).FirstOrDefault(), value);
        public 검사정보 SetResult(검사정보 검사, Single value)
        {
            if (검사 == null) return null;

            if (검사.검사장치 == 장치구분.QRCode)
            {
                검사.측정결과 = (value == 1) ? 결과구분.OK : 결과구분.NG;
            }
            else if (검사.검사항목 == 검사항목.노멀미러)
            {
                if (value == 0)
                    검사.측정결과 = 결과구분.NG;
                else if (value == 1)
                    검사.측정결과 = 결과구분.OK;
                else if (value == 2)
                    검사.측정결과 = 결과구분.OK;
                검사.결과값 = (Decimal)value;
            }
            else if (검사.검사항목 == 검사항목.역투입)
            {
                if (value == 0)
                    검사.측정결과 = 결과구분.NG;
                else if (value == 1)
                    검사.측정결과 = 결과구분.OK;
                else if (value == 2)
                    검사.측정결과 = 결과구분.NG;
                검사.결과값 = (Decimal)value;
            }
            else if (검사.검사항목 == 검사항목.커넥터설삽상부 || 검사.검사항목 == 검사항목.커넥터설삽하부)
            {
                if (value == 0)
                    검사.측정결과 = 결과구분.NG;
                else if (value == 1)
                    검사.측정결과 = 결과구분.OK;
                else if (value == 2)
                    검사.측정결과 = 결과구분.NG;
                검사.결과값 = (Decimal)value;
            }
            else
            {
                if (Single.IsNaN(value))
                {
                    검사.측정결과 = 결과구분.ER;
                    return 검사;
                }
                검사.측정값 = (Decimal)Math.Round(value, Global.환경설정.결과자릿수);
                검사.결과값 = 검사.측정값 + 검사.보정값;
                // org if (검사.검사장치 == 장치구분.Flatness || 검사.검사장치 == 장치구분.Spacing)
                // org     검사.결과값 += 검사.교정값;
                Boolean ok = 검사.결과값 >= 검사.최소값 && 검사.결과값 <= 검사.최대값;
                검사.측정결과 = ok ? 결과구분.OK : 결과구분.NG;
            }
            return 검사;
        }

        public void AddRange(List<검사정보> 자료)
        {
            this.검사내역.AddRange(자료);
        }

        public void 큐알정보검사(검사항목 구분, String 코드)
        {
            if (구분 == 검사항목.하부큐알코드1)
                this.하부큐알코드1 = 코드;
            else if (구분 == 검사항목.하부큐알코드2)
                this.하부큐알코드2 = 코드;
            else if (구분 == 검사항목.상부큐알코드1)
                this.상부큐알코드1 = 코드;
            else if (구분 == 검사항목.상부큐알코드2)
                this.상부큐알코드2 = 코드;
            else
            {
                Global.오류로그("검사정보", "큐알검사", $"파라미터 확인 필요", false);
                return;
            }

            this.SetResult(구분, String.IsNullOrEmpty(코드) ? 0 : 1);

            Boolean r = Global.큐알검증.검증수행(코드, out String 오류내용, out Int32[] indexs);

            // 큐알코드 검증, 중복여부 체크
            if (!Global.큐알검증.코드검증 || r)
                this.SetResult(검사항목.큐알검증, 0);
            else
            {
                this.SetResult(검사항목.큐알검증, 1);
                this.불량내역.Add(오류내용);
            }
            if (r && Global.큐알검증.중복체크)
            {
                if (!Global.큐알검증.중복검사(코드, indexs, out String 중복오류))
                {
                    this.불량내역.Add(중복오류);
                    this.SetResult(검사항목.중복여부, 1);
                }
                else this.SetResult(검사항목.중복여부, 0);
                Debug.WriteLine(중복오류, "중복오류");
            }
            else this.SetResult(검사항목.중복여부, 0);
        }

        public 결과구분 결과계산()
        {
            if (this.검사내역.Any(e => e.측정결과 == 결과구분.ER)) this.측정결과 = 결과구분.ER;
            else if (this.검사내역.Any(e => e.측정결과 != 결과구분.PS && e.측정결과 != 결과구분.OK)) this.측정결과 = 결과구분.NG;
            else this.측정결과 = 결과구분.OK;
            if (this.측정결과 == 결과구분.OK)
            {
                this.CTQ결과 = 결과구분.OK;
                this.외관결과 = 결과구분.OK;
            }
            else
            {
                if (this.검사내역.Any(e => e.검사그룹 == 검사그룹.CTQ && e.측정결과 == 결과구분.ER)) this.CTQ결과 = 결과구분.ER;
                else this.CTQ결과 = 결과구분.NG;
                if (this.검사내역.Any(e => e.검사그룹 == 검사그룹.Surface && e.측정결과 == 결과구분.ER)) this.외관결과 = 결과구분.ER;
                else this.외관결과 = 결과구분.NG;

                List<검사정보> 불량내역 = this.검사내역.Where(e => e.결과분류 == 결과분류.Summary && e.측정결과 == 결과구분.NG).ToList();
                if (불량내역.Count > 0)
                {
                    foreach (검사정보 정보 in 불량내역)
                        this.불량내역.Add(정보.검사항목.ToString());
                }
            }
            this.불량정보 = String.Join(",", this.불량내역.ToArray());
            return this.측정결과;
        }
    }


    [Table("insurf")]
    public class 불량영역
    {
        [Column("iswdt"), Required, Key, JsonProperty("iswdt"), Translation("Time", "일시")]
        public DateTime 검사일시 { get; set; } = DateTime.Now;
        [Column("isdev"), JsonProperty("isdev"), Translation("Model", "모델")]
        public 장치구분 장치구분 { get; set; } = 장치구분.None;
        [Column("isitm"), JsonProperty("isitm"), Translation("Item", "검사항목")]
        public 검사항목 검사항목 { get; set; } = 검사항목.스크레치;
        [Column("islef"), JsonProperty("islef"), Translation("X", "X")]
        public Int32 시작가로 { get; set; } = 0;
        [Column("istop"), JsonProperty("istop"), Translation("Y", "Y")]
        public Int32 시작세로 { get; set; } = 0;
        [Column("iswid"), JsonProperty("iswid"), Translation("Width", "Width")]
        public Int32 가로길이 { get; set; } = 0;
        [Column("ishei"), JsonProperty("ishei"), Translation("Height", "Height")]
        public Int32 세로길이 { get; set; } = 0;

        public 불량영역() { }
        public 불량영역(카메라구분 카메라, 검사항목 항목, CogRectangleAffine 영역)
        {
            장치구분 = (장치구분)카메라;
            검사항목 = 항목;
            시작가로 = (Int32)Math.Round(영역.CenterX - 영역.SideXLength / 2);
            시작세로 = (Int32)Math.Round(영역.CenterY - 영역.SideYLength / 2);
            가로길이 = (Int32)Math.Round(영역.SideXLength);
            세로길이 = (Int32)Math.Round(영역.SideYLength);
        }
        public CogRectangle GetRectangle() => new CogRectangle() { X = 시작가로, Y = 시작세로, Width = 가로길이, Height = 세로길이 };
        public CogRectangle GetRectangle(CogColorConstants color) { var r = GetRectangle(); r.Color = color; return r; }
        public CogRectangle GetRectangle(CogColorConstants color, Int32 lineWidth) { var r = GetRectangle(color); r.LineWidthInScreenPixels = lineWidth; return r; }
    }
}
