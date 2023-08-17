using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainInitialize : MonoBehaviour
{
    void Start()
    {
        NetInit();
    }

    /// <summary>
    /// 네트워크 기능 초기화
    /// </summary>
    void NetInit()
    {
        TrainManager.Instance.MsgAction -= NetworkFunc.SendMsg;
        TrainManager.Instance.MsgAction += NetworkFunc.SendMsg;
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
}
