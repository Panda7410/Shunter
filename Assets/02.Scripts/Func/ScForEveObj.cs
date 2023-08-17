using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScForEveObj : MonoBehaviour
{
    public Action DesrtoyAction;

    public static ScForEveObj GetScForEveObj
    {
        get { return Managers.Resource.Instantiate("EvePrefabs/EveObj", Managers.ManagerTr).GetComponent<ScForEveObj>(); }
    }
   
    public void OnDestroy()
    {
        StopAllCoroutines();
        if (DesrtoyAction != null)
            DesrtoyAction.Invoke();
        DesrtoyAction = null;
    }
}
