using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using GSSC.Signal;
using GSSC.Condition;

public class ConditionManager : SimpleSingleton<ConditionManager>
{
    //public void 
    public List<ConditionEventCallbackBase> Callbacks = new List<ConditionEventCallbackBase>();

    public void RegiCallback(ConditionEventCallbackBase callback)
    {
        if (!Callbacks.Contains(callback))
            Callbacks.Add(callback);
    }
    public void RemoveCallback(ConditionEventCallbackBase callback)
    {
        if (Callbacks.Contains(callback))
            Callbacks.Remove(callback);
    }

    public void Call()
    {
        for (int i = 0; i < Callbacks.Count; i++)
        {
            Callbacks[i].Call();
        }
    }
}
