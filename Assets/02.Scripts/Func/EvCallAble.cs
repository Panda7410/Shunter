using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//오브젝트 활성화 비활성화시 이벤트 할당. 
public class EvCallAble : MonoBehaviour
{
    public string Discription;
    public UnityEvent OnEnableEv;
    public UnityEvent OnDisableEv;
    [Header("=========")]
    public bool DelayOnEnable;
    public float EnableDelayTime = 0f;
    public bool DelayOndisable;
    public float DisableDelayTime = 0f;

    [ContextMenu("OnEnable")]
    private void OnEnable()
    {
        if (!DelayOnEnable)
            OnEnableEv?.Invoke();
        else
            Managers.ScForEveObj.StartCoroutine(InvokeOnEnableLater());
    }
    [ContextMenu("OnDisable")]
    private void OnDisable()
    {
        if (!DelayOndisable)
            OnDisableEv?.Invoke();
        else
            Managers.ScForEveObj.StartCoroutine(InvokeOnDisableLater());
    }
    IEnumerator InvokeOnEnableLater()
    {
        yield return new WaitForSeconds(EnableDelayTime);
        OnEnableEv?.Invoke();
    }
    IEnumerator InvokeOnDisableLater()
    {
        yield return new WaitForSeconds(DisableDelayTime);
        OnDisableEv?.Invoke();
    }
}
