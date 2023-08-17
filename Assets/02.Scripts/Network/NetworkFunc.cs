using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkFunc : MonoBehaviour
{
    const string ErrorMsgItemName = "영상메시지";

    public static void SendMsg(string msg)
    {
        Client.Instance.SendData(ErrorMsgItemName, msg);
    }
}
