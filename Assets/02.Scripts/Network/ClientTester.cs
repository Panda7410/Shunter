using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.nsTcp;
using Common.nsSocket;
using Common;
using GSSC;
using UnityEngine.UI;
using TMPro;

public class ClientTester : MonoBehaviour
{
    public TextMeshProUGUI text;
    // Start is called before the first frame update
    RepoarchitectureGSSC tcpClient;
    void Start()
    {
        //tcpClient = new RepoarchitectureGSSC();


        //tcpClient.New("127.0.0.1", 7000);
        //tcpClient.EventStatus += TcpClient_EventStatus;
        //tcpClient.EventDataArrival += TcpClient_EventDataArrival;

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

        ActionManager.Instance.AddAction("SET", "test2", ObjectAction:(t) => 
        {
            Debug.Log(t.ToString());
            text.text = t.ToString();
        });

    }

    [ContextMenu("샌드 테스트")]
    void SendTest()
    {
        Client.Instance.SendData("테스트", "테스트");
    }


    private void TcpClient_EventDataArrival(object sender, RepoarchitectureGSSC.RxDataEvent e)
    {
        foreach(var item in e.Message.Items)
        {
            Debug.Log(item.Name);
            Debug.Log(item.Value);
        }

    }

    private void TcpClient_EventStatus(object sender, RepoarchitectureGSSC.StatusEvent e)
    {
        if (e.Status)
        {
            Debug.Log("접속성공");
        }
        else
        {
            Debug.Log("접속실패");
        }
    }

    private void OnDisable()
    {
        tcpClient?.Dispose();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
