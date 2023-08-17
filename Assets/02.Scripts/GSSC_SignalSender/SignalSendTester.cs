using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GSSC.Signal;
public class SignalSendTester : MonoBehaviour
{
    public SignalSend Signal = new SignalSend();

    public bool ActionBool;
    public int ActionInt;
    public float ActionFloat;
    public string ActionString;
    public object ActionObect;

    public void Send() => Signal.SendAction();
    public void SendBool() => Signal.SendAction(ActionBool: ActionBool);
    public void SendInt() => Signal.SendAction(ActionInt:ActionInt);
    public void SendFloat() => Signal.SendAction(ActionFloat: ActionFloat);
    public void SendString() => Signal.SendAction(ActionString: ActionString);

}
