using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkFunc
{
    public static void SendMsg(string msg)
    {
        Client.Instance.SendData(ItemList.ErrorMsgItemName, msg);
    }
}
