using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Common.nsTcp
{

    public class TcpClientGSSC : IDisposable
    {
        //MSDN - [EventHandler<(Of <(TEventArgs>)>) 대리자] 참조로 작성됨.
        #region 소켓 수신데이터 이벤트 ==+==+==+==+==+===+==+==+==+==+===+==+==+==+==+===+==+==+==+==+===+==+==+==+==+===+==+==
        public class RxDataEvent : EventArgs
        {
            private readonly string strGetData;
            public RxDataEvent(string strMessage)
            {
                strGetData = strMessage;
            }

            //이벤트 수신자용 속성.
            public string Message
            {
                get
                {
                    return strGetData;
                }
                //set { strGetData = value; }
            }
        }

        public event EventHandler<RxDataEvent> EventDataArrival;
        public virtual void DataArrival(string strMessage)
        {
            EventHandler<RxDataEvent> ehandler = EventDataArrival;
            if (ehandler != null)
            {
                System.Threading.Tasks.Task.Factory.StartNew(() =>
                    System.Threading.Tasks.Parallel.ForEach(ehandler.GetInvocationList(), delegateFunc =>
                    {
                        if (delegateFunc.Target != null)
                        {
                            delegateFunc.DynamicInvoke(new object[] { this, new RxDataEvent(strMessage) });
                        }
                    })
                );
            }
        }
        #endregion 이벤트 발생용 끝==+==+==+==+==+===+==+==+==+==+===+==+==+==+==+===+==+==+==+==+===+==+==+==+==+===+==+==+=

        #region 소켓연결 에러 이벤트 ==+==+==+==+==+===+==+==+==+==+===+==+==+==+==+===+==+==+==+==+===+==+==+==+==+===+==+==
        public class ErrorEvent : EventArgs
        {
            private readonly string strError;
            public ErrorEvent(string ErrMsg)
            {
                strError = ErrMsg;
            }

            //이벤트 수신자용 속성.
            public string Message
            {
                get
                {
                    return strError;
                }
                //set { strError = value; }
            }
        }

        public event EventHandler<ErrorEvent> EventStatus;
        public virtual void EventSocketStatus(string strVal)
        {
            EventHandler<ErrorEvent> ehandler = EventStatus;

            if (ehandler != null)
            {

                try
                {
                    ////foreach (Delegate delegateFunc in ehandler.GetInvocationList())
                    ////{
                    ////    if (delegateFunc.Target != null)
                    ////        delegateFunc.DynamicInvoke(new object[] { this, new errEvent(strVal) });
                    ////}

                    System.Threading.Tasks.Task.Factory.StartNew(() =>
                        System.Threading.Tasks.Parallel.ForEach(ehandler.GetInvocationList(), delegateFunc =>
                        {
                            if (delegateFunc.Target != null)
                                delegateFunc.DynamicInvoke(new object[] { this, new ErrorEvent(strVal) });
                        })
                    );
                }
                catch { }
            }
        }
        #endregion 이벤트 발생용 끝==+==+==+==+==+===+==+==+==+==+===+==+==+==+==+===+==+==+==+==+===+==+==+==+==+===+==+==+=

        #region Socket용 정의==+==+==+==+==+===+==+==+==+==+===+==+==+==+==+===+==+==+==+==+===+==+==+==+==+===+==+==+==+=
        private const int iBufferSize = 8192;                           //버퍼사이즈 상수
        private TcpClient tcpSocket = new TcpClient();                  //클래스 소켓
        private byte[] bReadBuffer = new byte[iBufferSize];             //읽는 데이터 변수

        //readonly Encoding uniEncod = Encoding.GetEncoding("ks_c_5601");          //한국어는 기본codepage(949)로 설정한다.
        readonly Encoding uniEncod = Encoding.UTF8;

        public volatile bool mtcpStatus;
        public volatile bool bUsed = false;

        string m_BufIP = string.Empty;
        int m_BufPort;
        //소켓 생성 메서드 정의
        public string Name { get; set; } = string.Empty;

        //소켓 생성 메서드 정의
        public void New()
        {
            tcpSocket = null;                                           //소켓 초기화
                                                                        //netStream = null;                                           //스트리밍 초기화
            bReadBuffer = new byte[iBufferSize];                        //변수 초기화
        }

        public void New(string ip, int port)
        {
            m_BufIP = ip;
            m_BufPort = port;
            New();
        }

        #region Dispose 구현
        ~TcpClientGSSC()
        {
            Dispose(false);
        }

        private bool disposedValue = false; // 중복 호출을 검색하려면
        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (!disposedValue)
                {
                    if (disposing)
                    {
                        // IDisposable 인터페이스를 구현하는 멤버들을 여기서 정리합니다.
                    }

                    tcpSocket?.Close();                   //닫기.

                    Thread.Sleep(50);

                    tcpSocket = null;
                    mtcpStatus = false;

                    disposedValue = true;
                }
            }
            catch
            {
            }
        }

        // 삭제 가능한 패턴을 올바르게 구현하기 위해 추가된 코드입니다.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        //소켓 종료 메서드 정의
        ////public void Dispose()
        ////{
        ////    if (tcpSocket != null)
        ////        tcpSocket.Close();                   //닫기.
        ////    System.Threading.Thread.Sleep(50);

        ////    tcpSocket = null;
        ////    //netStream = null;

        ////    mtcpStatus = false;
        ////}

        //소켓 연결 메서드
        public string Connect()
        {
            return Connect(m_BufIP, m_BufPort);
        }

        public void DisConnect()
        {
            try
            {
                if (tcpSocket == null)
                    return;

                if (tcpSocket.Client == null)
                    return;

                if (tcpSocket.Client.Connected)
                    tcpSocket.Client.Disconnect(true);
            }
            catch { }
        }

        public string Connect(string sIPAddress, int iPortNo)
        {
            string sLocalIP = string.Empty;

            try
            {
                Close();

                if (sIPAddress == null || iPortNo == 0)
                    return sLocalIP;

                tcpSocket = new TcpClient(sIPAddress, iPortNo)
                {
                    ReceiveBufferSize = iBufferSize, // 1024;
                    SendBufferSize = iBufferSize //1024;
                };

                tcpSocket.Client.BeginReceive(bReadBuffer, 0, iBufferSize, SocketFlags.None, new AsyncCallback(DoRead), tcpSocket);

                //접속된 로칼IP정보를 처리한다.
                sLocalIP = ((System.Net.IPEndPoint)tcpSocket.Client.LocalEndPoint).Address.ToString();
                mtcpStatus = true;
                EventSocketStatus("true");             //연결 정상
            }
            catch (SocketException se)
            {
                mtcpStatus = false;
                EventSocketStatus(string.Concat("TcpClient Connect => ", se.Message));                     //스트림 데이터 읽기 오류
            }
            return sLocalIP;
        }

        //소켓 연결해제 메서드
        public bool Close()
        {
            try
            {
                //소켓 종료
                tcpSocket?.Close();

                Thread.Sleep(100);

            }
            catch (SocketException se)
            {
                EventSocketStatus(string.Concat("TcpClient Close => ", se.Message));
            }

            mtcpStatus = false;
            return false;
        }

        public bool SendData(string strSendData)
        {
            bool bResult = false;
            try
            {
                if (tcpSocket == null)
                    return false;

                if (tcpSocket.Connected)
                {
                    byte[] strmessage = uniEncod.GetBytes(strSendData);
                    int iRemainderByte = strmessage.Length;
                    int ioffsetByte = 0;

                    tcpSocket.Client.BeginSend(strmessage, ioffsetByte, iRemainderByte, 0, new AsyncCallback(SendCallback), tcpSocket);
                    bResult = true;
                }
            }
            catch (Exception e)
            {
                bResult = false;
                EventSocketStatus(string.Concat("TcpClient SendData Err => ", e.Message));
            }
            return bResult;
        }

        public bool SendData(byte[] strSendData)
        {
            bool bResult = false;

            try
            {
                if (tcpSocket == null)
                    return false;
                if (tcpSocket.Connected)
                {
                    int iRemainderByte = strSendData.Length;
                    int ioffsetByte = 0;

                    if (tcpSocket.Connected)
                    {
                        tcpSocket.Client.BeginSend(strSendData, ioffsetByte, iRemainderByte, 0, new AsyncCallback(SendCallback), tcpSocket);
                        bResult = true;
                    }


                    bResult = true;
                }
            }
            catch (Exception se)
            {
                EventSocketStatus(string.Concat("TcpClient SendData => ", se.Message));
                bResult = false;
            }
            return bResult;
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                int bytesSent = tcpSocket.Client.EndSend(ar);
                if (bytesSent == -1)
                {
                    Close();
                    mtcpStatus = false;
                    EventSocketStatus("Socket DisConnected");               //소켓연결 해제
                }
            }
            catch (Exception e)
            {
                EventSocketStatus(e.Message);
            }
        }

        //데이터 읽기.
        private void DoRead(IAsyncResult ar)
        {
            int iBytesRead = 0;
            string strmessage = string.Empty;

            try
            {
                iBytesRead = tcpSocket?.Client.EndReceive(ar) ?? -1;

                if (iBytesRead > 0)
                {
                    strmessage = uniEncod.GetString(bReadBuffer, 0, iBytesRead);        //문자열 변환(한글)
                    DataArrival(strmessage);                                            //읽은 데이터 전송 이벤트 발생처리.

                    //데이터를 읽고 다시 대기 상태로.
                    bReadBuffer = new byte[iBufferSize];

                    IAsyncResult beginRead = tcpSocket.Client.BeginReceive(bReadBuffer, 0, iBufferSize, SocketFlags.None, new AsyncCallback(DoRead), null);
                }
                else
                {
                    Close();
                    mtcpStatus = false;
                    EventSocketStatus("Socket DisConnected");               //소켓연결 해제
                }
            }
            catch (Exception se)
            {
                Close();
                mtcpStatus = false;
                EventSocketStatus(se.Message);                                  //읽기 오류 (메시지 참조)
            }
        }
        #endregion Socket용 정의 끝.==+==+==+==+==+===+==+==+==+==+===+==+==+==+==+===+==+==+==+==+===+==+==+==+==+===+==+==+==+=
    }
}
