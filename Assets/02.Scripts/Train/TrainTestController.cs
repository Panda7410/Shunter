using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainTestController : MonoBehaviour
{
    //public TrainModule trainModule;

    //public Rail nowRail;


    //public bool go;

    //[ContextMenu("열차세팅테스트")]
    //public void Set()
    //{
    //    //놓을수있는지 검사. 
    //    if (!trainModule.isPlaced(nowRail, dist))
    //    {
    //        Debug.Log("안되는데요. 막혔어요.");return; 
    //    }
    //    trainModule.SetTrain(nowRail, dist);
    //}
    public float dist = 30f;


    [Space]
    [Header("==========")]
    [Header("앞뒤로 붙일 리스트")]
    public List<TrainModule> FrontMo = new List<TrainModule>();
    public List<TrainModule> BackMo = new List<TrainModule>();
    [Space]
    [Header("==========")]
    [Header("레일세팅 ")]

    public string RailName;
    public float setDist = 100f;
    [Space]

    [Header("==========")]
    //생성부분.
    [Header("일단 모듈 생성부분.")]
    public string CreatMoName;
    public string CreatMoType;
    [ContextMenu("열차모듈생성.")]
    public void MakeModule()
    {
        TrainManager.Instance.CreateModule(CreatMoType, CreatMoName, RailName, setDist);
    }
    [Space]
    [Header("==========")]
    [Header("모듈탐색")]
    public string FindMoName;
    [ContextMenu("목록에모듈붙이기")]
    public void ADDFrontMo()
    {
        TrainModule module;// = TrainManager.Instance.AllModules.Find(x => x.MatchID(FindMoName));
        if(TrainManager.Instance.AllModules.TryGetValue(FindMoName, out module))
        FrontMo.Add(module);
        else
            Debug.LogWarning($"{FindMoName}를 찾을수 없음. 등록실패 ");
    }
    [Space]
    [Header("==========")]
    [Header("열차생성")]
    public string CreatTrainNmae;
    public TrainModule mainModule;

    [ContextMenu("열차생성")]
    public void CreatTrain()
    {
        TrainManager.Instance.CreatTrain(CreatTrainNmae, mainModule);
    }
    [Space]
    [Header("==========")]
    [Header("열차위치 조절")]

    public string MoveTrainName;
    public string MoveRailName;
    public float MoveDist = 100f;
    [ContextMenu("열차위치조절")]
    public void SetTrain()
    {
        TrainManager.Instance.SetTrainPos(MoveTrainName, MoveRailName, MoveDist);
    }
    [ContextMenu("앞에 장착")]
    public void FrontAttach()
    {
        TrainManager.Instance.AttachFrontModule(MoveTrainName, FrontMo);
    }
    [ContextMenu("뒤에 장착")]
    public void backAttach()
    {
        TrainManager.Instance.AttachBackModule(MoveTrainName, BackMo);
    }
    [Space]
    [Header("==========")]
    [Header("커플링 제거")]
    public string BreakTrainName;
    public string BreakModuleName;
    
    [ContextMenu("커플링제거")]

    public void BreakCoupling()
    {
        TrainManager.Instance.BreakTrainCoupling(BreakTrainName, BreakModuleName);
    }
    [Space]
    [Header("==========")]
    [Header("모듈제거")]
    public string DestroyModuleName;
    [ContextMenu("모듈제거")]

    public void DestroyModule()
    {
        TrainManager.Instance.DestroyModule(DestroyModuleName);
    }

}
