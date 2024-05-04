using Euresys.MultiCam;
using MvUtils;
using Newtonsoft.Json;
using DevExpress.XtraEditors.Drawing;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.ComponentModel;
using System.Windows.Forms;

namespace TPA.Schemas
{
    public enum Connector
    {
        M,
        A,
        B,
    }

    public enum AcquisitionMode
    {
        PAGE,
        LONGPAGE,
        WEB,
    }

    public enum TrigMode
    {
        IMMEDIATE,
        HARD,
        SOFT,
        COMBINED,
    }


    public class EuresysLink : 카메라장치
    {
        public delegate void ImageCallbackEvent(카메라구분 카메라, IntPtr intPtr, Int32 width, Int32 height);
        //public event ImageCallbackEvent OnImageCallbackReceived;

        public UInt32 Channel;
        public string CamFile { get; set; } = "P3-80-16k40.cam";
        public UInt32 DriverIndex { get; set; } = 0;
        public Connector Connector { get; set; } = Connector.M;
        public AcquisitionMode AcquisitionMode { get; set; } = AcquisitionMode.PAGE;
        public TrigMode TrigMode { get; set; } = TrigMode.HARD;
        public Int32 SeqLength_pg { get; set; } = 1;
        public Int32 PageLength_Ln { get; set; } = 40000;
        private MC.CALLBACK CamCallBack;
        private UInt32 currentSurface;

        public EuresysLink(카메라구분 구분) {
            this.구분 = 구분;
            this.상태 = Init();
        }

        public override Boolean Init()
        {
            MC.Create("CHANNEL", out this.Channel);
            MC.SetParam(this.Channel, "DriverIndex", this.DriverIndex);
            MC.SetParam(this.Channel, "Connector", this.Connector.ToString());
            MC.SetParam(this.Channel, "CamFile", Path.Combine(Global.환경설정.기본경로, this.CamFile));
            MC.SetParam(this.Channel, "TrigMode", this.TrigMode.ToString());
            MC.SetParam(this.Channel, "PageLength_Ln", this.PageLength_Ln);
            MC.SetParam(this.Channel, "SeqLength_pg", this.SeqLength_pg);
            
            this.CamCallBack = new MC.CALLBACK(MultiCamCallback);
            MC.RegisterCallback(this.Channel, this.CamCallBack, this.Channel);
            // Enable the signals corresponding to the callback functions
            MC.SetParam(this.Channel, MC.SignalEnable + MC.SIG_SURFACE_PROCESSING, "ON");
            MC.SetParam(this.Channel, MC.SignalEnable + MC.SIG_ACQUISITION_FAILURE, "ON");
            MC.SetParam(this.Channel, "ChannelState", ChannelState.READY);
            Debug.WriteLine("카메라 연결", $"[{this.구분}] 카메라 연결 성공!", false);

            this.상태 = true;
            return this.상태;
        }

        public override Boolean Close()
        {
            this.Free();
            return true;
        }

        public override Boolean Start()
        {
            this.Ready();
            return true;
        }

        public override Boolean Stop()
        {
            if (this.CurrentState() != ChannelState.READY)
                MC.SetParam(this.Channel, "ChannelState", ChannelState.READY);
            return true;
        }

        public override Boolean Ready()
        {
            if (this.CurrentState() != ChannelState.ACTIVE)
                MC.SetParam(this.Channel, "ChannelState", ChannelState.ACTIVE);
            return true;
        }

        public void Free()
        {
            MC.SetParam(this.Channel, "ChannelState", ChannelState.FREE);
        }

        public string CurrentState()
        {
            String State;
            MC.GetParam(this.Channel, "ChannelState", out State);
            return State;
        }

        private void MultiCamCallback(ref MC.SIGNALINFO signalInfo)
        {
            switch (signalInfo.Signal) {
                case MC.SIG_SURFACE_PROCESSING:
                    ProcessingCallback(signalInfo);
                    break;

                case MC.SIG_ACQUISITION_FAILURE:
                    AcqFailureCallback(signalInfo);
                    break;

                default:
                    Debug.WriteLine(signalInfo.Signal, "SIGNALINFO");
                    throw new Euresys.MultiCamException("Unknown signal");
            }
        }

        private void ProcessingCallback(MC.SIGNALINFO signalInfo)
        {
            currentSurface = signalInfo.SignalInfo;
            try {
                UInt32 currentChannel = (UInt32)signalInfo.Context;
                Int32 ImageSizeX, ImageSizeY, BufferPitch;
                IntPtr SurfaceAddr;

                MC.GetParam(currentChannel, "ImageSizeX", out ImageSizeX);
                MC.GetParam(currentChannel, "ImageSizeY", out ImageSizeY);
                MC.GetParam(currentChannel, "BufferPitch", out BufferPitch);
                MC.GetParam(currentSurface, "SurfaceAddr", out SurfaceAddr);
                this.AcquisitionFinished(SurfaceAddr, ImageSizeX, ImageSizeY);
                //Global.그랩제어.그랩완료(this.구분, SurfaceAddr, ImageSizeX, ImageSizeY);
            }
            catch (Euresys.MultiCamException ex) {
                Utils.MessageBox("영상획득", ex.ToString(), 2000);
            }
        }

        private void AcqFailureCallback(MC.SIGNALINFO signalInfo)
        {
            Utils.MessageBox("영상획득", "유레시스영상획득 실패", 2000);
        }

        public static class ChannelState
        {
            [Description("채널은 그래버를 소유하고 있지만 잠금상태는 아님.")]
            public const string IDLE = "IDLE";

            [Description("채널은 그래버를 사용합니다.")]
            public const string ACTIVE = "ACTIVE";

            [Description("채널에 그래버가 없습니다.")]
            public const string ORPHAN = "ORPHAN";

            [Description("채널은 그래버를 잠그고 acquisition sequence를 시작할 준비가 됨.")]
            public const string READY = "READY";

            [Description("채널의 상태를 ORPHAN으로 설정합니다.")]
            public const string FREE = "FREE";
        }
    }
}
