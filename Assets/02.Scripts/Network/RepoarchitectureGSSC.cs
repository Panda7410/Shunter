using Common.nsTcp;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using Newtonsoft.Json;

namespace Common.nsSocket
{

    public class RepoarchitectureGSSC
    {
        #region 데이터 수신 이벤트 ==+==+==+==+==+===+==+==+==+==+===+==+==+==+==+===+==+==+==+==+===+==+==+==+==+===+==+==
        public class RxDataEvent : EventArgs
        {
            private readonly DataModel strGetData;
            public RxDataEvent(DataModel strMessage)
            {
                try
                {
                    strGetData = strMessage;
                }
                catch
                {
                    throw;
                }
            }

            //이벤트 수신자용 속성.
            public DataModel Message
            {
                get
                {
                    return strGetData;
                }
            }
        }

        public event EventHandler<RxDataEvent> EventDataArrival;
        public virtual void DataArrival(DataModel strMessage)
        {
            EventHandler<RxDataEvent> ehandler = EventDataArrival;

            if (ehandler != null)
            {
                try
                {
                    System.Threading.Tasks.Task.Factory.StartNew(() =>
                        System.Threading.Tasks.Parallel.ForEach(ehandler.GetInvocationList(),
                            new System.Threading.Tasks.ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, delegateFunc =>
                            {
                                if (delegateFunc.Target != null)
                                {
                                    delegateFunc.DynamicInvoke(new object[] { this, new RxDataEvent(strMessage) });
                                }
                            })
                    );
                }
                catch (Exception se)
                {
                    Debug.WriteLine(se.Message);
                }
            }
        }
        #endregion

        #region 소켓연결 Status 이벤트 ==+==+==+==+==+===+==+==+==+==+===+==+==+==+==+===+==+==+==+==+===+==+==+==+==+===+==+==
        public class StatusEvent : EventArgs
        {
            private readonly bool boolStatus;
            private readonly string strIdxName;

            public StatusEvent(bool MsgStatus, string MsgIdx)
            {
                boolStatus = MsgStatus;
                strIdxName = MsgIdx;
            }

            //이벤트 수신자용 속성.
            public bool Status
            {
                get
                {
                    return boolStatus;
                }
            }
            public string Idx
            {
                get
                {
                    return strIdxName;
                }
            }
        }

        public event EventHandler<StatusEvent> EventStatus;
        public virtual void EventSocketStatus(Boolean bStatus, string IdxName)
        {
            EventHandler<StatusEvent> ehandler = EventStatus;

            if (ehandler != null)
            {
                try
                {
                    System.Threading.Tasks.Task.Factory.StartNew(() =>
                        System.Threading.Tasks.Parallel.ForEach(ehandler.GetInvocationList(),
                            new System.Threading.Tasks.ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, delegateFunc =>
                            {
                                if (delegateFunc.Target != null)
                                    delegateFunc.DynamicInvoke(new object[] { this, new StatusEvent(bStatus, IdxName) });
                            })
                    );
                }
                catch (Exception se)
                {
                    Debug.WriteLine(se.Message);
                }
            }
        }
        #endregion 이벤트 발생용 끝==+==+==+==+==+===+==+==+==+==+===+==+==+==+==+===+==+==+==+==+===+==+==+==+==+===+==+==+=
        private volatile Dictionary<string, string> gGSsDataList = new Dictionary<string, string>(); //Repository Architecture용 메모리 변수

        private BackgroundWorker bgwTcpConnect = new BackgroundWorker();                        //데이터 연결용 Thread
        private BackgroundWorker bgwThreadAdvise = new BackgroundWorker();                      //데이터 Advise Thread

        private System.Threading.Thread bgThreadTcpGetData = null;

        private System.Timers.Timer tmrTcpConnect = new System.Timers.Timer();                                              //소켓 연결용 타이머(6000)
        private System.Timers.Timer tmrTcpSend = new System.Timers.Timer();                                                 //데이터 전송 타이머(50)

        private readonly ConcurrentQueue<string> mQueue = new ConcurrentQueue<string>();                                               //데이터 수신용 Queue
        private readonly ConcurrentQueue<DataModel> mSendQueue = new ConcurrentQueue<DataModel>();                                           //데이터 송신용 Queue

        private TcpClientGSSC mSocket;                                      //통신소켓

        private volatile string strTcpIp = string.Empty;                                         //연결IP
        private volatile int iPort;                                                              //연결 port

        private string[] mItemList = new string[] { "" };
        private string[] sBaseItems = new string[] { "자기진단실시", "훈련모드", "시스템제어", "훈련제어", "SHUTDOWN_CODE" };
        string mItemCommand = "Advise";

        private volatile bool mtcpStatus;                                                       //tcp연결상태
        private int pSystemControl;              //시스템제어
        private int pExerciseControl;            //훈련제어

        private bool bFirstAdvise = false;              //전체 Advise 실행여부

        private string IndexName = string.Empty;
        public string GetName
        {
            get
            {
                return IndexName;
            }
        }

        public string SetName
        {
            set
            {
                IndexName = value;
            }
        }

        //현재 값 가져오기
        public int GetSystemControl
        {
            get
            {
                return pSystemControl;
            }
        }

        public int GetExerciseControl
        {
            get
            {
                return pExerciseControl;
            }
        }

        //생성 메서드 정의
        public RepoarchitectureGSSC()
        {
        }

        //종료 메서드 정의
        ~RepoarchitectureGSSC()
        {
            mtcpStatus = false;
            if (bgThreadTcpGetData != null)
            {
                if (bgThreadTcpGetData == null)
                    return;
                //bgThreadTcpGetData.Abort();
                bgThreadTcpGetData.Join();
                System.Threading.Thread.Sleep(100);

                while (bgThreadTcpGetData.IsAlive == true)
                {
                    tmrTcpConnect.Enabled = false;
                    mtcpStatus = false;
                    System.Threading.Thread.Sleep(50);
                }
            }
        }

        //생성 메서드 정의
        public void New(string strIpAddress, int nPortNo, bool bExcuteTimer = false)
        {
            if (string.IsNullOrEmpty(strIpAddress))
                return;
            if (nPortNo < 1000)
                return;

            try
            {
                strTcpIp = strIpAddress;
                iPort = nPortNo;

                bgwTcpConnect = new BackgroundWorker();                     //데이터 연결용 Thread
                bgwThreadAdvise = new BackgroundWorker();                   //데이터 Advise Thread

                bgwTcpConnect.WorkerSupportsCancellation = true;
                bgwThreadAdvise.WorkerSupportsCancellation = true;

                mQueue.Clear();
                mSendQueue.Clear();                        //데이터 송신용 Queue                    
                gGSsDataList.Clear();
                gGSsDataList = new Dictionary<string, string>();

                mSocket = new TcpClientGSSC();                               //통신소켓
                mSocket.New();                                              //소켓 열기.

                bgwTcpConnect.DoWork += TcpConnect_DoWork;
                bgwThreadAdvise.DoWork += ThreadAdvise_DoWork;

                mSocket.EventStatus += Socket_EventStatus;
                mSocket.EventDataArrival += Socket_EventDataArrival;

                tmrTcpConnect = new System.Timers.Timer()
                {
                    Enabled = true,
                    Interval = 6000
                };                                //소켓 연결용 타이머(6000)
                tmrTcpConnect.Elapsed += TimerTcpConnect_Elapsed;

                tmrTcpSend = new System.Timers.Timer()
                {
                    Enabled = false,
                    Interval = 10
                };                                   //데이터 전송 타이머(50)
                tmrTcpSend.Elapsed += TimerTcpSend_Elapsed;

                tmrTcpConnect.Start();
                if (bExcuteTimer)
                    TimerTcpConnect_Elapsed(tmrTcpConnect, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        //종료 메서드 정의
        public void Dispose()
        {
            try
            {
                tmrTcpConnect.Enabled = false;

                if (bgwTcpConnect.IsBusy)
                    bgwTcpConnect.CancelAsync();
                if (bgwThreadAdvise.IsBusy)
                    bgwThreadAdvise.CancelAsync();

                bgwTcpConnect.DoWork -= TcpConnect_DoWork;
                bgwThreadAdvise.DoWork -= ThreadAdvise_DoWork;
                tmrTcpConnect.Elapsed -= TimerTcpConnect_Elapsed;
                tmrTcpSend.Elapsed -= TimerTcpSend_Elapsed;

                if (mSocket != null)
                {
                    mSocket.EventStatus -= Socket_EventStatus;
                    mSocket.EventDataArrival -= Socket_EventDataArrival;
                }

                System.Threading.Thread.Sleep(50);

                tmrTcpSend.Stop();

                mSocket?.Close();
                System.Threading.Thread.Sleep(50);

                mSocket?.Dispose();
                mQueue.Clear();
                mSendQueue.Clear();

                mtcpStatus = false;

                tmrTcpConnect.Enabled = false;

                if (bgThreadTcpGetData == null)
                    return;
                bgThreadTcpGetData.Join();
                System.Threading.Thread.Sleep(100);

                while (bgThreadTcpGetData.IsAlive == true)
                {
                    tmrTcpConnect.Enabled = false;
                    mtcpStatus = false;
                    System.Threading.Thread.Sleep(50);
                    bgThreadTcpGetData.Join();
                }
            }
            catch
            {

            }
        }


        //현재 값 가져오기
        public string GetData(string strKey)
        {
            string sResult = string.Empty;

            try
            {
                gGSsDataList.TryGetValue(strKey.ToUpper(), out sResult);

                sResult = string.IsNullOrEmpty(sResult) ? "0" : sResult;
                return sResult;
            }
            catch
            {
                return sResult;
            }
        }

        //현재 값 가져오기
        public string GetData(string strKey, int iDecimals)
        {
            string sResult = string.Empty;

            try
            {
                gGSsDataList.TryGetValue(strKey.ToUpper(), out sResult);

                sResult = string.IsNullOrEmpty(sResult) ? "0" : sResult;

                //소수점 자리를 처리하여 넘긴다.
                string iParameter = string.Format("f{0}", iDecimals);
                sResult = Math.Round(Convert.ToDouble(sResult), iDecimals).ToString(iParameter);
                return sResult;
            }
            catch
            {
                return sResult;
            }
        }

        //값 전송하기
        public bool SendData(DataModel data)
        {
            bool bResult = false;
            try
            {
                //데이터 추가.
                mSendQueue.Enqueue(data);

                if (!tmrTcpSend.Enabled)
                {
                    tmrTcpSend.Enabled = true;
                    bResult = true;
                }
            }
            catch
            {
                bResult = false;
            }
            return bResult;
        }

        public bool SendNonQueueData(DataModel data)
        {
            bool bResult = false;
            try
            {
                if (mSocket == null || data.Items.Count == 0)
                    return bResult;

                if (mSocket.mtcpStatus)
                {
                    string jsonString = Utility.DataModelToString(data);
                    bResult = mSocket.SendData(jsonString);
                }
            }
            catch
            {
                bResult = false;
            }
            return bResult;
        }

        //public bool SendNonQueueData_Double(string strData)
        //{
        //    bool bResult = false;
        //    try
        //    {
        //        if (string.IsNullOrWhiteSpace(strData))
        //        {
        //            bResult = false;
        //            return bResult;
        //        }

        //        int iFindEtx = strData.LastIndexOf(";", strData.Length);
        //        if (iFindEtx < strData.Length - 1)
        //            strData = string.Concat(strData, ";");

        //        //데이터 전송.
        //        if (mSocket?.mtcpStatus ?? false)
        //        {
        //            bool bSendDataStatus = mSocket.SendData(strData);
        //            bool bSendDataStatusTwo = mSocket.SendData(strData);
        //            bResult = bSendDataStatus || bSendDataStatusTwo;
        //        }
        //    }
        //    catch
        //    {
        //        bResult = false;
        //    }
        //    return bResult;
        //}

        void TimerTcpSend_Elapsed(object sender, EventArgs e)
        {
            try
            {
                int Cnt = -1;
                DataModel tempData = null;
                bool bSendDataStatus = false;
                bool brturnst = false;

                Cnt = mSendQueue.Count;

                if (Cnt > 0)
                {
                    if (mtcpStatus.Equals(false))
                        brturnst = true;
                    else
                    {
                        _ = mSendQueue.TryDequeue(out tempData);
                    }
                }
                else
                {
                    tempData = null;
                    tmrTcpSend.Enabled = false;
                }

                if (brturnst)
                    return;

                if (tempData != null)
                {
                    if (mSocket?.mtcpStatus ?? false)
                    {
                        string jsonString = Utility.DataModelToString(tempData);
                        bSendDataStatus = mSocket.SendData(jsonString);
                    }
                }
            }
            catch
            {
            }
        }

        public void SetAdviseData_Add(string strKey)
        {
            if (mtcpStatus.Equals(false))
                return;

            mItemCommand = strKey;

            try
            {
                if (mtcpStatus.Equals(true))
                {
                    if (bgwThreadAdvise.IsBusy != true)
                    {
                        bgwThreadAdvise.RunWorkerAsync();
                    }
                }
                else
                {
                    if (bgwThreadAdvise.IsBusy == true)
                    {
                        bgwThreadAdvise.CancelAsync();
                    }
                }
            }
            catch
            {
            }
        }

        [DescriptionAttribute("GSs BaseAdvise 항목 리스트"), CategoryAttribute("mBaseItems"), DisplayNameAttribute("BaseItems"), DefaultValueAttribute(false)]
        public string[] BaseItems
        {
            set
            {
                sBaseItems = new string[value.Length];
                Array.Copy(value, sBaseItems, value.Length);
            }
        }

        [DescriptionAttribute("GSs Advise 항목 리스트"), CategoryAttribute("stItems"), DisplayNameAttribute("Items"), DefaultValueAttribute(false)]
        public string[] AdviseItems
        {
            get
            {
                return mItemList;
            }
            set
            {
                mItemList = new string[value.Length];
                Array.Copy(value, mItemList, value.Length);
            }
        }

        public int CntIndex
        {
            get
            {
                return gGSsDataList.Count;
            }
        }

        [DescriptionAttribute("GSs Connection Status"), CategoryAttribute("stGSsConnectStatus"), DisplayNameAttribute("stGSsConnectStatus"), DefaultValueAttribute(false)]
        public bool ConnectStatus
        {
            get
            {
                return mtcpStatus;
            }
        }

        #region Tcp/IP 소켓 및 이벤트 수신 정의

        private void Socket_Connect()
        {
            //소켓 연결작업 루틴
            mSocket?.Connect(strTcpIp, iPort);
        }

        private void Socket_EventStatus(object sender, TcpClientGSSC.ErrorEvent e)
        {
            string strStatus = e.Message;

            try
            {
                //소켓연결됨.
                if (strStatus.Equals("true"))
                {
                    //무조건 Advise Thread 중지처리 (새로 연결 또는 끊어졌으므로)
                    if (bgwThreadAdvise.IsBusy == true)
                    {
                        bgwThreadAdvise.CancelAsync();
                    }

                    Thread.Sleep(100);
                    DataModel stbBsItems = new DataModel()
                    {
                        Order = OrderKind.ADVISE
                    };
                    for (int i = 0; i < sBaseItems.Length; i++)
                    {
                        stbBsItems.Items.Add(new Item { Name = sBaseItems[i] });

                        if (!gGSsDataList.ContainsKey(sBaseItems[i].ToUpper()))
                            gGSsDataList.Add(sBaseItems[i].ToUpper(), "0");
                    }
                    SendData(stbBsItems);

                    stbBsItems.Order = OrderKind.SET;
                    stbBsItems.Items.Clear();
                    stbBsItems.Items.Add(new Item { Name = "SHUTDOWN_CODE", Value = 0 });
                    SendData(stbBsItems);

                    mtcpStatus = true;

                    bgThreadTcpGetData = new System.Threading.Thread(Thread_TcpGetData)
                    {
                        IsBackground = true
                    };
                    bgThreadTcpGetData.Start();
                }
                else
                {
                    mSocket?.DisConnect();
                    mSendQueue.Clear();
                    mtcpStatus = false;
                    pSystemControl = 0;
                    pExerciseControl = 0;
                    gGSsDataList.Clear();

                    if (bgThreadTcpGetData == null)
                        return;
                    //bgThreadTcpGetData.Abort();
                    bgThreadTcpGetData.Join();
                    System.Threading.Thread.Sleep(10);

                    try
                    {
                        while (bgThreadTcpGetData.IsAlive == true)
                        {
                            System.Threading.Thread.Sleep(10);
                            //bgThreadTcpGetData.Abort();
                            bgThreadTcpGetData.Join();
                        }


                    }
                    catch (Exception ex1)
                    {
                        Debug.WriteLine(ex1.Message);
                    }
                }
            }
            catch (Exception ex2)
            {
                Debug.WriteLine(ex2.Message);
            }
            finally
            {
                EventSocketStatus(mtcpStatus, IndexName);
            }
        }

        //수신 이벤트
        private void Socket_EventDataArrival(object sender, TcpClientGSSC.RxDataEvent e)
        {
            try
            {
                //데이터 표시.
                mQueue.Enqueue(e.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public void SetReadDataSplit(string mValueData)
        {
            try
            {
                DataModel data = JsonConvert.DeserializeObject<DataModel>(mValueData);

                if (data != null && data.Order == OrderKind.SET)
                {
                    foreach (var item in data.Items)
                    {
                        if (!gGSsDataList.ContainsKey(item.Name))
                        {
                            gGSsDataList.Add(item.Name, item.Value.ToString());
                        }
                        else
                        {
                            //수신된 데이터를 넣는다.
                            gGSsDataList[item.Name] = item.Value.ToString();
                        }

                        if (item.Name == "시스템제어")
                        {
                            pSystemControl = Convert.ToInt32(item.Value.ToString());
                        }
                        else if (item.Name == "훈련제어")
                        {
                            pExerciseControl = Convert.ToInt32(item.Value.ToString());
                        }
                    }


                    //수신된 데이터 이벤트를 발생시킨다.
                    //읽은 데이터 전송 이벤트 발생처리.
                    DataArrival(data);
                }
            }
            catch
            {
            }
        }

        private void Thread_TcpGetData()
        {
            while (mtcpStatus)
            {
                try
                {
                    int iQueueCnt = 0;
                    iQueueCnt = mQueue.Count;

                    while (iQueueCnt > 0)
                    {

                        _ = mQueue.TryDequeue(out string strTempData);

                        try
                        {
                            SetReadDataSplit(strTempData ?? "");
                        }
                        catch
                        {

                        }

                        iQueueCnt = mQueue.Count;
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
                System.Threading.Thread.Sleep(10);
            }
        }

        //소켓 연결 타이머.
        private void TimerTcpConnect_Elapsed(object sender, EventArgs e)
        {
            try
            {
                if (mtcpStatus.Equals(false))
                {
                    if (bgwTcpConnect.IsBusy != true)
                    {
                        bgwTcpConnect.RunWorkerAsync();
                    }
                }
                else
                {
                    if (bgwTcpConnect.IsBusy == true)
                    {
                        bgwTcpConnect.CancelAsync();
                    }

                    if (bgThreadTcpGetData.IsAlive == false)
                        mtcpStatus = false;
                }
            }
            catch { }
        }

        private void TcpConnect_DoWork(object sender, DoWorkEventArgs e)
        {
            if (mtcpStatus == false)
                Socket_Connect();  //소켓연결
        }

        private void ThreadAdvise_DoWork(object sender, DoWorkEventArgs e)
        {
            //전송 Item이 없으면 빠져 나온다.
            if (mItemList.Length < 1)
                return;

            try
            {
                int iCnt = 0;
                DataModel sb = new DataModel();

                for (int i = 0; i < mItemList.Length; i++)
                {
                    iCnt++;

                    try
                    {
                        string tmpItem = mItemList[i].ToUpper();

                        switch (mItemList[i])
                        {
                            case "시스템제어":
                            case "훈련제어":
                            case "훈련모드":
                            case "자기진단실시":
                            case "SHUTDOWN_CODE":
                                break;

                            default:
                                if (string.IsNullOrEmpty(tmpItem))
                                    continue;

                                if (mItemCommand.ToUpper() == "ADVISE")
                                {
                                    sb.Order = OrderKind.ADVISE;
                                    sb.Items.Add(new Item { Name = tmpItem });
                                    if (!gGSsDataList.ContainsKey(tmpItem))
                                        gGSsDataList.Add(tmpItem, "0");
                                }
                                else if (mItemCommand.ToUpper() == "GET")
                                {
                                    sb.Order = OrderKind.GET;
                                    sb.Items.Add(new Item { Name = tmpItem });
                                }
                                else
                                {
                                    if (bFirstAdvise)
                                    {
                                        sb.Order = OrderKind.UNADVISE;
                                        sb.Items.Add(new Item { Name = tmpItem });
                                    }
                                }
                                break;
                        }
                    }
                    catch
                    {
                    }

                    //Unadvise인 경우는 신속히 처리한다(최대 48개 까지)
                    bool bSendTime = false;
                    if (mItemCommand == "Advise")
                    {
                        bSendTime = iCnt > 5;
                    }
                    else
                    {
                        //bSendTime = iCnt > 48;
                        bSendTime = iCnt > 15;
                    }

                    if (bSendTime)
                    {
                        SendData(sb);
                        iCnt = 0;
                        sb = new DataModel();
                    }
                }

                //마지막 Item을 처리한다.
                if (sb.Items.Count > 0)
                {
                    SendData(sb);
                }

                if (mItemCommand.ToUpper() == "ADVISE")
                    bFirstAdvise = true;
                else if (mItemCommand.ToUpper() == "UNADVISE")
                    bFirstAdvise = false;

            }
            catch
            {
            }
        }
        #endregion
    }
}