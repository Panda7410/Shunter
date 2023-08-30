using GSSC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;


public class MainInitialize : MonoBehaviour
{
    //const string CheckItemName = "자기진단실시";

    void Start()
    {
        NetInit();
        SceneAdd();
    }

    /// <summary>
    /// 네트워크 기능 초기화
    /// </summary>
    void NetInit()
    {
        TrainManager.Instance.MsgAction -= NetworkFunc.SendMsg;
        TrainManager.Instance.MsgAction += NetworkFunc.SendMsg;

        ActionManager.Instance.AddAction(OrderKind.SET.ToString(), ItemList.CheckItemName, ObjectAction: HeartBeat);




        //Client.Instance.OnConnect += () => 
        //{
        //    Debug.LogWarning("컨넥트");
        //    TrainManager.Instance.MsgAction -= NetworkFunc.SendMsg;
        //    TrainManager.Instance.MsgAction += NetworkFunc.SendMsg;
        //};
        //Client.Instance.OnDisConnect += () =>
        //{
        //    Debug.LogWarning("디스컨넥트");

        //    TrainManager.Instance.MsgAction -= NetworkFunc.SendMsg;
        //};
    }

    /// <summary>
    /// 
    /// </summary>
    void SceneAdd()
    {
        //우선 라인정보
        //Managers.SceneChanger.LoadSceneAsyncAdditive("LineInfo");
    }

    void HeartBeat(object obj)
    {
        //string ItemName = "자기진단실시";
        int value = 1;
        Client.Instance.SendData(ItemList.CheckItemName, value);
    }

}
