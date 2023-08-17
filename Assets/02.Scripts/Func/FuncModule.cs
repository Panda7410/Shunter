using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FuncModule : MonoBehaviour
{
    public Action FuncAction;
    public UnityEvent FuncEvent;

    public static void AddModule(Transform obj, Action action)
    {
        FuncModule funcModule = obj.GetComponent<FuncModule>();
        if (funcModule == null)
            funcModule = obj.gameObject.AddComponent<FuncModule>();
        funcModule.FuncAction += action;
    }
    public void ModuleInvoke()
    => InvokeAction();
    public static void ModuleInvoke(Transform tr)
    => tr.GetComponent<FuncModule>()?.InvokeAction();
    public static void ModuleInvoke(GameObject obj)
    => obj.GetComponent<FuncModule>()?.InvokeAction();
    public static void ModuleInvoke(RaycastHit hit)
    => hit.collider?.GetComponent<FuncModule>()?.InvokeAction();
    public static void ModuleInvoke(Collider collider)
    => collider?.GetComponent<FuncModule>()?.InvokeAction();



    void InvokeAction()
    {
        FuncAction?.Invoke();
        FuncEvent?.Invoke();
    }
}
