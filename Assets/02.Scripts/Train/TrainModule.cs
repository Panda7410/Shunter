using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainModule : MonoBehaviour
{
    public Action OnDestroyTrianAction;

    public Rail nowRail;


    [SerializeField]
    private string moduleID;
    [SerializeField]
    private float LengthBetweenBogies = 13.800f;
    [SerializeField]
    private float HeadFromBogie = 2.95f;
    [SerializeField]
    private float TailFromBogie = 2.85f;
    [SerializeField]// 0정방향 1역방향
    private bool trainDirect = false;

    public bool TrainDirect { get => trainDirect; set => trainDirect = value; }
    public string ModuleID { get => moduleID; private set => moduleID = value; }
    public float NowDist { get; private set; }

    public bool isPlaced(Rail rail, float dist)
    {
        float SeDist = (dist - LengthBetweenBogies);

        if (rail.IsPlaced(dist) && rail.IsPlaced(SeDist))
            return true;
        else
            return false;
    }

    public void SetID(string ID)
    {
    ModuleID = ID;
        gameObject.name = $"{gameObject.name}_{ID}";
    }
    public bool MatchID(string ID)
        => ModuleID == ID;

    public void SetTrainSpec(float HeadFromBogie, float LengthBetweenBogies, float TailFromBogie)
    {
        this.HeadFromBogie = HeadFromBogie;
        this.LengthBetweenBogies = LengthBetweenBogies;
        this.TailFromBogie = TailFromBogie;
    }

    public void SetTrain(Rail rail, float dist)
    {
        NowDist = dist;
        nowRail = rail;
        float SeDist = (dist - LengthBetweenBogies);
        Vector3 lookPos = Vector3.zero;
        Vector3 TargetPos = Vector3.zero;
        if (!trainDirect)
        {
            lookPos = rail.GetPos(dist);
            TargetPos = rail.GetPos(SeDist);
        }
        else
        {
            lookPos = rail.GetPos(SeDist);
            TargetPos = rail.GetPos(dist);
        }

        transform.position = TargetPos;

        Vector3 Nomal = lookPos - TargetPos;
        Nomal.Normalize();
        transform.rotation = Quaternion.LookRotation(Nomal);
    }

    public (float HeadFromBogie, float LengthBetweenBogies, float TailFromBogie) GetTrainSpec
        => (HeadFromBogie, LengthBetweenBogies, TailFromBogie);

    //public void DestroyTainModule()
    //{
    //    OnDestroyTrianAction?.Invoke();
    //}

    private void OnDestroy()
    {
        OnDestroyTrianAction?.Invoke();
    }

    [ContextMenu("수동등록")]
    private void CustomADD()
    =>TrainManager.Instance.AllModules.Add(moduleID, this);
    
}
