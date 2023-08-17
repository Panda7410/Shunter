using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.nsSocket;
using Common;
using GSSC;
using System;

public class Client : SimpleSingleton<Client>
{
    public Action OnConnect;
    public Action OnDisConnect;


    public List<string> AdviseList = new List<string>();

    public string IP = "127.0.0.1";
    public int Port = 7000;
    [SerializeField]
    private bool isSendAdvise = false;
    RepoarchitectureGSSC tcpClient;
    private Queue<DataModel> DataEvents = new Queue<DataModel>();
    private Queue<(string, object)> SendDataQueue = new Queue<(string, object)>();
    object _lock = new object();

    private void Start()
    {
        tcpClient = new RepoarchitectureGSSC();


        tcpClient.New("127.0.0.1", 7000);
        tcpClient.EventStatus += TcpClient_EventStatus;
        tcpClient.EventDataArrival += TcpClient_EventDataArrival;



        StartCoroutine(LoopUpdate());
        //DataModel test = new DataModel()
        //{
        //    Order = OrderKind.SET
        //};
        //test.Items.Add(new Item()
        //{
        //    Name = "testname",
        //    Value = 1
        //});
        //tcpClient.SendData(test);
    }

    public void SendData(string Name, object value)
    {
        SendDataQueue.Enqueue((Name, value));
        //tcpClient.SendData(model);
    }

    private void TcpClient_EventDataArrival(object sender, RepoarchitectureGSSC.RxDataEvent e)
    {
        lock (_lock)
            DataEvents.Enqueue(e.Message);
    }

    public void EnqueDataModelOutSide(DataModel dataModel)
    {
        lock (_lock)
            DataEvents.Enqueue(dataModel);
    }

    private void TcpClient_EventStatus(object sender, RepoarchitectureGSSC.StatusEvent e)
    {
        if (e.Status)
        {
            Debug.Log("접속성공");
            if (!isSendAdvise)
            {
                ADVISEData();
            }
            OnConnect?.Invoke();
        }
        else
        {
            
            Debug.Log("접속종료");
            //try
            //{
            //    if (!Application.isPlaying)
            //    {
            //        tcpClient?.Dispose();
            //        tcpClient = null;
            //    }
            //}
            //catch
            //{
            //    tcpClient?.Dispose();
            //    tcpClient = null;
            //}
            //StopAllCoroutines();

            OnDisConnect?.Invoke();
        }
    }

    IEnumerator LoopUpdate()
    {
        WaitForEndOfFrame endF = new WaitForEndOfFrame();
        while (true)
        {

            DataModel data;
            lock (_lock)
            {
                if (DataEvents.TryDequeue(out data))
                {
                    foreach (var item in data.Items)
                    {
                        ActionManager.Instance.InvokeAction(data.Order.ToString(), item.Name, ActionObject: item.Value);
#if UNITY_EDITOR
                        Debug.Log($"{data.Order} : {item.Name} : {item.Value} - 이벤트 인보크");
# endif
                    }
                }
            }
            //프레임 종료 직전까지 대기.
            yield return endF;
            if(SendDataQueue.Count != 0) // 보낼 데이터가 있다면 전송.
            {
                DataModel SendData = new DataModel()
                {
                    Order = OrderKind.SET
                };
                while (SendDataQueue.Count != 0)
                {
                    var Item = SendDataQueue.Dequeue();
                    SendData.Items.Add(new Item()
                    {
                        Name = Item.Item1,
                        Value = Item.Item2
                    });
                }
                tcpClient.SendData(SendData);
            }
            //다음프레임까지 대기
            yield return null;
        }
    }
    IEnumerator HeartBeat()
    {
        WaitForSeconds waitSecond = new WaitForSeconds(1f);
        while (true)
        {
            yield return waitSecond;
        }
    }


    private void OnDisable()
    {
        //종료
        if (Instance == this)
#if UNITY_EDITOR
        Debug.Log("접속 종료 시도");
#endif
        {
        this.tcpClient?.Dispose();
        tcpClient = null;
        }

    }
    protected override void OnDestroy()
    {
        if (Instance == this)
        {
            Debug.Log("접속 종료 시도");
            this.tcpClient?.Dispose();
            tcpClient = null;
        }

        base.OnDestroy();
    }

    [ContextMenu("강제종료")]
    void forceDispose()
    {
        Debug.Log("강제 접속 종료 시도");
        tcpClient?.Dispose();
        tcpClient = null;
    }

    void ADVISEData()
    {
        //string[] dataStr = { "자기진단실시", "시스템제어", "훈련제어" , "SHUTDOWN_CODE" };
        DataModel SendData = new DataModel()
        {
            Order = OrderKind.ADVISE
        };
        //for (int i = 0; i < dataStr.Length; i++)
        //{
        //    SendData.Items.Add(new Item()
        //    {
        //        Name = dataStr[i],
        //        Value = 1
        //    });
        //}

        AdviseList.ForEach(t => 
        {
            SendData.Items.Add(new Item()
            {
                Name = t,
            });
        });

        isSendAdvise = true;
        tcpClient.SendData(SendData);
    }
}

