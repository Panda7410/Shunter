using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using GSSC.Signal;
using GSSC;

public class SignalRecv : MonoBehaviour
{
    public SignalTag SignalTag;

    public UnityEvent DefaultEvent;
    public UnityEvent<bool> BoolEvent;
    public UnityEvent<int> IntEvent;
    public UnityEvent<float> FloatEvent;
    public UnityEvent<string> StringEvent;
    [Header("실행하는 측과 보내는 측 데이터형식 일치해야함.")]
    [Header("형변환시 주의.")]
    public UnityEvent<object> ObjectEvent;



    private void InvokeEvent() => DefaultEvent?.Invoke();
    private void InvokeBoolEvent(bool ActonVar) => BoolEvent?.Invoke(ActonVar);
    private void InvokeIntEvent(int ActonVar) => IntEvent?.Invoke(ActonVar);
    private void InvokeFloatEvent(float ActonVar) => FloatEvent?.Invoke(ActonVar);
    private void InvokeStringEvent(string ActonVar) => StringEvent?.Invoke(ActonVar);
    private void InvokeObjectEvent(object ActonVar) => ObjectEvent?.Invoke(ActonVar);



    private void OnEnable()
    {
        ActionManager.Instance.AddAction(SignalTag.Group, SignalTag.Tag, InvokeEvent);
        ActionManager.Instance.AddAction(SignalTag.Group, SignalTag.Tag, BoolAction: InvokeBoolEvent);
        ActionManager.Instance.AddAction(SignalTag.Group, SignalTag.Tag, IntAction: InvokeIntEvent);
        ActionManager.Instance.AddAction(SignalTag.Group, SignalTag.Tag, FloatAction: InvokeFloatEvent);
        ActionManager.Instance.AddAction(SignalTag.Group, SignalTag.Tag, StringAction: InvokeStringEvent);
        ActionManager.Instance.AddAction(SignalTag.Group, SignalTag.Tag, ObjectAction: InvokeObjectEvent);
    }

    private void OnDisable()
    {
        ActionManager.Instance.RemoveAction(SignalTag.Group, SignalTag.Tag, InvokeEvent);
        ActionManager.Instance.RemoveAction(SignalTag.Group, SignalTag.Tag, BoolAction: InvokeBoolEvent);
        ActionManager.Instance.RemoveAction(SignalTag.Group, SignalTag.Tag, IntAction: InvokeIntEvent);
        ActionManager.Instance.RemoveAction(SignalTag.Group, SignalTag.Tag, FloatAction: InvokeFloatEvent);
        ActionManager.Instance.RemoveAction(SignalTag.Group, SignalTag.Tag, StringAction: InvokeStringEvent);
        ActionManager.Instance.RemoveAction(SignalTag.Group, SignalTag.Tag, ObjectAction: InvokeObjectEvent);
    }
    public void ReReg()
    {
        Debug.Log("이벤트를 재 등록했습니다.");
        OnDisable();
        OnEnable();
    }
}
