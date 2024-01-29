using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.IO;

namespace TPA.Schemas
{
    public abstract class 소켓통신기본클래스 : IDisposable
    {
        protected TcpClient 클라이언트;
        protected NetworkStream 네트워크스트림;
        public Boolean 소켓연결여부 => 클라이언트.Connected;
        public String 주소;
        public Int32 포트;
        
        protected String ACK메시지 { get; set; } = "ERRORSTAT";
        private Boolean 리소스해제여부 = false;

        public String 에러메시지 = String.Empty;

        public 소켓통신기본클래스()
        {
            클라이언트 = new TcpClient();
        }

        ~소켓통신기본클래스()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(Boolean 리소스해제)
        {
            if (!리소스해제여부) {
                if (리소스해제) {
                    네트워크스트림?.Close();
                    클라이언트?.Close();
                    에러메시지 = String.Empty;
                }
                리소스해제여부 = true;
            }
        }

        public virtual Boolean Connect(Int32 타임아웃시간 = 5000)
        {
            try
            {
                클라이언트.Connect(주소, 포트);
                네트워크스트림 = 클라이언트.GetStream();
                return true;
            }
            catch (Exception ex)
            {
                에러메시지 = $"소켓 연결 실패 : {ex.Message}, IP {주소.ToString()}, Port {포트}";
                return false;
            }
        }

        public virtual Boolean Connect(String 주소, Int32 포트)
        {
            try {
                if (this.소켓연결여부) return true;
                클라이언트.Connect(주소, 포트);
                네트워크스트림 = 클라이언트.GetStream();
                this.주소 = 주소;
                this.포트 = 포트;
                return true;
            }
            catch (Exception ex) {
                에러메시지 = $"소켓 연결 실패 : {ex.Message}, IP {주소.ToString()}, Port {포트}";
                return false;
            }
        }

        public virtual Boolean Connect(String 주소, Int32 포트, Int32 타임아웃시간 = 5000)
        {
            if (클라이언트.Client == null)
            {
                에러메시지 = $"소켓 연결 실패 (null) : IP {주소.ToString()}, Port {포트}";
                return false;
            }
            using (ManualResetEvent 연결완료 = new ManualResetEvent(false))
            {
                Stopwatch stopwatch = new Stopwatch();
                AsyncCallback callback = (ar) =>
                {
                    try {
                        클라이언트.EndConnect(ar);
                    }
                    catch (Exception) {
                        에러메시지 = $"소켓 연결 실패 (타임아웃 {타임아웃시간}) : IP {주소.ToString()}, Port {포트}";
                    }
                    finally {
                        stopwatch.Stop();
                        연결완료.Set();
                    }
                };

                stopwatch.Start();
                클라이언트.BeginConnect(주소, 포트, callback, 클라이언트);
                if (연결완료.WaitOne(타임아웃시간)){
                    네트워크스트림 = 클라이언트.GetStream();
                    this.주소 = 주소;
                    this.포트 = 포트;
                    return true;
                }
                else {
                    클라이언트.Close();
                    return false;
                }
            }
        }

        public virtual Boolean Disconnect()
        {
            try {
                네트워크스트림?.Close();
                클라이언트.Close();
                return true;
            }
            catch (Exception ex) {
                에러메시지 = $"소켓 해제 실패 : {ex.Message}";
                return false;
            }
        }

        protected virtual Boolean SendData(String data, Encoding encoding = null)
        {
            if (encoding == null) encoding = Encoding.UTF8;

            Byte[] 전송데이터 = encoding.GetBytes(data);
            try {
                if (네트워크스트림 == null)
                    throw new ArgumentNullException("stream", "네트워크 스트림이 null입니다.");

                네트워크스트림.Write(전송데이터, 0, 전송데이터.Length);
                네트워크스트림.Flush();
                return true;
            }
            catch (Exception ex) {
                에러메시지 = $"소켓통신 데이터 전송 실패 : {ex.Message}";
                return false;
            }
        }

        protected virtual Boolean SendData(String data, Int32 타임아웃시간, Encoding encoding = null)
        {
            if (encoding == null) encoding = Encoding.UTF8;
            Byte[] 전송데이터 = encoding.GetBytes(data);
            
            try {
                if (네트워크스트림 == null)
                    throw new ArgumentNullException("stream", "네트워크 스트림이 null입니다.");

                네트워크스트림.WriteTimeout = 타임아웃시간;
                네트워크스트림.Write(전송데이터, 0, 전송데이터.Length);
                네트워크스트림.Flush();
                return true;
            }
            catch (IOException ex) when (ex.InnerException is SocketException socketEx && socketEx.SocketErrorCode == SocketError.TimedOut)
            {
                에러메시지 = $"소켓통신 데이터 전송 실패 (타임아웃) : {ex.Message}";
                return false;
            }
            catch (Exception ex) {
                에러메시지 = $"소켓통신 데이터 전송 실패 : {ex.Message}";
                return false;
            }
        }

        protected virtual String ReceiveData(Encoding encoding = null)
        {
            if (encoding == null) encoding = Encoding.UTF8;

            try {
                if (네트워크스트림 == null)
                    throw new ArgumentNullException("stream", "네트워크 스트림이 null입니다.");

                Byte[] 수신버퍼 = new Byte[클라이언트.Available];
                Int32 수신버퍼크기 = 네트워크스트림.Read(수신버퍼, 0, 클라이언트.Available);
                return encoding.GetString(수신버퍼, 0, 수신버퍼크기);
            }
            catch (Exception ex) {
                에러메시지 = $"소켓통신 데이터 수신 실패 : {ex.Message}";
                return String.Empty;
            }
        }

        protected virtual String ReceiveData(Int32 타임아웃시간, Int32 재시도횟수, Encoding encoding = null)
        {
            if (encoding == null) encoding = Encoding.UTF8;
            Int32 시도횟수 = 0;
            while (시도횟수 < 재시도횟수) {
                if (네트워크스트림.CanRead) {
                    Byte[] 수신버퍼 = new Byte[클라이언트.Available];
                    try {
                        if (네트워크스트림 == null)
                            throw new ArgumentNullException("stream", "네트워크 스트림이 null입니다.");
                        네트워크스트림.ReadTimeout = 타임아웃시간;
                        Int32 수신데이터크기 = 네트워크스트림.Read(수신버퍼, 0, 클라이언트.ReceiveBufferSize);
                        if (수신데이터크기 > 0) {
                            return encoding.GetString(수신버퍼, 0, 수신데이터크기);
                        }
                    }
                    catch (IOException) {
                        시도횟수++;
                    }
                    catch (Exception ex) {
                        에러메시지 = $"소켓통신 데이터 수신 실패 : {ex.Message}";
                        return String.Empty;
                    }
                }
                else {
                    시도횟수++;
                    Thread.Sleep(타임아웃시간);
                }
            }
            에러메시지 = $"소켓통신 데이터 수신 실패 : {재시도횟수}번의 재시도에도 데이터 수신 실패";
            return String.Empty;
        }

        protected virtual Boolean ACK메시지확인(Int32 Ack대기시간 = 100, Encoding encoding = null)
        {
            if (encoding == null) encoding = Encoding.UTF8;
            Byte[] 버퍼 = new Byte[encoding.GetByteCount(ACK메시지)];

            try {
                if (네트워크스트림 == null)
                    throw new ArgumentNullException("stream", "네트워크 스트림이 null입니다.");

                네트워크스트림.ReadTimeout = Ack대기시간;
                Int32 수신메시지길이 = 네트워크스트림.Read(버퍼, 0, 버퍼.Length);
                String 수신메시지 = encoding.GetString(버퍼, 0, 수신메시지길이);

                if (수신메시지 == ACK메시지) return true;
                else {
                    에러메시지 = $"ACK 메시지 불일치 : ACK메시지 {ACK메시지}, 수신한 메시지 {수신메시지}";
                    return false;
                }
            }
            catch (IOException ex) when (ex.InnerException is SocketException socketEx && socketEx.SocketErrorCode == SocketError.TimedOut)
            {
                에러메시지 = "ACK메시지 수신 타임아웃";
                return false;
            }
            catch (Exception ex) {
                에러메시지 = $"ACK메시지 수신 중 예외발생 {ex.Message}";
                return false;
            }
        }
    }
}
