using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
    [SerializeField]
    private string trainID;
    
    [SerializeField]
    private TrainModule MainTrainModule;
    [SerializeField]
    private List<TrainModule> trainBackModules = new List<TrainModule>();
    [SerializeField]
    private List<TrainModule> trainFrontModules = new List<TrainModule>();
    [SerializeField]// 0정방향 1역방향
    private bool trainDirect = false;
    [SerializeField]
    private float trainSpeed = 0;

    public float BackLenth = 0;
    public float FrontLenth = 0;

    //public Action<string> OnFailAddModule;
    public Action<string> OnFailInstallTrain; // 열차 배치 실패. 거리 없음 
    public Action<string> SendMsg;

    //프로퍼티
    public bool TrainDirect { get => trainDirect; private set => trainDirect = value; }
    public float LastDist { get; private set; }
    public Rail LastRail { get; private set; }
    public string TrainID { get => trainID; private set => trainID = value; }
    public float TrainSpeed { get => trainSpeed; private set => trainSpeed = value; }

    public void SetID(string ID)
    => trainID = ID;

    public bool MatchID(string ID)
    => trainID == ID;

    public void SetDist(Rail rail, float dist)
    {
        if (!IsPlaced(rail, dist))
        {
            Debug.LogWarning($"{trainID}배치 불능");
            SendMsg?.Invoke($"{trainID}배치 불능");
            return;
        }

        float AddBackLenth = 0;
        float AddFrontLenth = 0;

        LastDist = dist; // 최종위치 갱신.
        LastRail = rail; // 최종 노선 갱신
        //메인열차 위치 세팅. 
        MainTrainModule.SetTrain(rail, dist);
        //뒤쪽은 메인 열차 길이를 포함한다. 
        AddBackLenth += MainTrainModule.GetTrainSpec.LengthBetweenBogies;
        
        if (!MainTrainModule.TrainDirect)
        {
            AddBackLenth += MainTrainModule.GetTrainSpec.TailFromBogie;
            AddFrontLenth += MainTrainModule.GetTrainSpec.HeadFromBogie;
        }
        else
        {
            AddBackLenth += MainTrainModule.GetTrainSpec.HeadFromBogie;
            AddFrontLenth += MainTrainModule.GetTrainSpec.TailFromBogie;
        }


        //뒤로 붙는 열차. 0부터시작.
        for (int i = 0; i < trainBackModules.Count; i++)
        {
            //
            if (!trainBackModules[i].TrainDirect)
                AddBackLenth += trainBackModules[i].GetTrainSpec.HeadFromBogie;
            else
                AddBackLenth += trainBackModules[i].GetTrainSpec.TailFromBogie;

            trainBackModules[i].SetTrain(rail, dist - AddBackLenth);

            if (!trainBackModules[i].TrainDirect)
                AddBackLenth += trainBackModules[i].GetTrainSpec.TailFromBogie;
            else
                AddBackLenth += trainBackModules[i].GetTrainSpec.HeadFromBogie;
            AddBackLenth += trainBackModules[i].GetTrainSpec.LengthBetweenBogies;
        }
        //앞으로 붙는 열차. 0부터시작.
        for (int i = 0; i < trainFrontModules.Count; i++)
        {
            //방향이 역이기 때문에 먼저 열차 길이를 더한다. 
            AddFrontLenth += trainFrontModules[i].GetTrainSpec.LengthBetweenBogies;
            //
            if (!trainFrontModules[i].TrainDirect)
                AddFrontLenth += trainFrontModules[i].GetTrainSpec.TailFromBogie;
            else
                AddFrontLenth += trainFrontModules[i].GetTrainSpec.HeadFromBogie;

            trainFrontModules[i].SetTrain(rail, dist + AddFrontLenth);

            if (!trainFrontModules[i].TrainDirect)
                AddFrontLenth += trainFrontModules[i].GetTrainSpec.HeadFromBogie;
            else
                AddFrontLenth += trainFrontModules[i].GetTrainSpec.TailFromBogie;
        }
    }

    /// <summary>
    /// 배치 가능여부
    /// </summary>
    /// <param name="rail">설치하려는 레일</param>
    /// <param name="dist">설치하려는 거리</param>
    /// <returns></returns>
    public bool IsPlaced(Rail rail, float dist)
    {
        if (MainTrainModule == null)
        {
            Debug.LogError($"{trainID} 의 메인차량이 비어있습니다.");
            SendMsg?.Invoke($"{trainID} 의 메인차량이 비어있습니다.");

            return false;
        }

        if (BackLenth == 0 && FrontLenth == 0) // 열차길이 연산.
            CalculateTrainLenth();

        if (rail.IsPlaced(dist + FrontLenth + 0.5f) && rail.IsPlaced(dist - BackLenth - 0.5f))
            return true;
        else
            return false;
    }


    public bool IsContainTrianModule(string moduleID)
    {
        if (MainTrainModule.MatchID(moduleID))
            return true;
        for (int i = 0; i < trainBackModules.Count; i++)
        {
            if (trainBackModules[i].MatchID(moduleID))
                return true;
        }
        for (int i = 0; i < trainFrontModules.Count; i++)
        {
            if (trainFrontModules[i].MatchID(moduleID))
                return true;
        }
        return false;
    }
    public TrainModule GetTrainModule(string moduleID)
    {
        if (MainTrainModule.MatchID(moduleID))
            return MainTrainModule;
        TrainModule module;

        module = trainBackModules.Find(x => x.MatchID(moduleID));
        if (module != null)
            return module;
        module = trainBackModules.Find(x => x.MatchID(moduleID));
        return module;
    }

    
    //public void SetAllTrainModule(TrainModule MainTrainModule, List<TrainModule> BackTrainModules, List<TrainModule> FrontTrainModules, bool RemoveTrain = false)
    //{
    //    if (RemoveTrain) // 기존 설치 열차 제거.
    //    {
    //        MainTrainModule.DestroyTainModule();
    //        for (int i = 0; i < trainBackModules.Count; i++)
    //            trainBackModules[i].DestroyTainModule();
    //        for (int i = 0; i < trainFrontModules.Count; i++)
    //            trainFrontModules[i].DestroyTainModule();
    //        MainTrainModule = null;
    //        trainBackModules.Clear();
    //        trainFrontModules.Clear();
    //    }
    //    this.MainTrainModule = MainTrainModule;
    //    trainBackModules = BackTrainModules;
    //    trainFrontModules = FrontTrainModules;

    //    //열차 길이 재연산
    //    CalculateTrainLenth();

    //    if(LastRail != null) // 위치 재연산
    //    {
    //        if (IsPlaced(LastRail, LastDist))
    //            SetDist(LastRail, LastDist);
    //        else
    //            OnFailInstallTrain?.Invoke(trainID);
    //    }
    //}

    public void SetMainModule(TrainModule MainTrainModule)
        => this.MainTrainModule = MainTrainModule;

    public void AddFrontModule(List<TrainModule> FrontTrainModules)
    {
        //열차 배치 실패 - 이미 모듈이 열차에 포함되어있음. 
        for (int i = 0; i < FrontTrainModules.Count; i++)
        {
            try
            {
                if (IsContainTrianModule(FrontTrainModules[i].ModuleID))
                {
                    //OnFailAddModule?.Invoke(FrontTrainModules[i].ModuleID);
                    Debug.LogError($"{FrontTrainModules[i].ModuleID} 가 이미 열차에 포함되어있습니다.");
                    SendMsg?.Invoke($"{FrontTrainModules[i].ModuleID} 가 이미 열차에 포함되어있습니다.");

                    return;
                }
            }
            catch (Exception)
            {
                Debug.LogError($"열차 구성에 에러가 있습니다.");
                SendMsg?.Invoke($"열차 구성에 에러가 있습니다.");


                //throw;
            }
        }
        trainFrontModules.AddRange(FrontTrainModules);
        //열차 길이 재연산
        CalculateTrainLenth();
    }
    public void AddBackModule(List<TrainModule> BackTrainModules)
    {
        //열차 배치 실패- 이미 모듈이 열차에 포함되어있음. 
        for (int i = 0; i < BackTrainModules.Count; i++)
        {
            if (IsContainTrianModule(BackTrainModules[i].ModuleID))
            {
                //OnFailAddModule?.Invoke(BackTrainModules[i].ModuleID);
                Debug.LogError($"{BackTrainModules[i].ModuleID} 가 이미 열차에 포함되어있습니다.");
                SendMsg?.Invoke($"열차 구성에 에러가 있습니다.");

                return;
            }
        }
        trainBackModules.AddRange(BackTrainModules);
        //열차 길이 재연산
        CalculateTrainLenth();
    }

    /// <summary>
    /// 연결차량 연결 해제
    /// </summary>
    /// <param name="moduleId"></param>
    public void BreakTrainCoupling(string moduleId)
    {
        if (!IsContainTrianModule(moduleId))
        {
            Debug.LogWarning($"{TrainID} 에 {moduleId} 차량이 포함되어있지 않아 연결을 해제 할 수 없습니다.");
            return;
        }
        if (MainTrainModule.MatchID(moduleId))
        {
            Debug.LogWarning($"{TrainID} - {moduleId} 메인모듈은 해제할 수 없습니다.");
            return;
        }

        TrainModule module = GetTrainModule(moduleId);
        if (trainBackModules.Contains(module))
        {
            int A = trainBackModules.IndexOf(module);
            trainBackModules.RemoveRange(A, trainBackModules.Count - A);
        }
        else if (trainFrontModules.Contains(module))
        {
            int A = trainFrontModules.IndexOf(module);
            trainFrontModules.RemoveRange(A, trainFrontModules.Count - A);
        }
        //열차 길이 재연산
        CalculateTrainLenth();
    }

    public void CalculateTrainLenth()
    {
        try
        {
            BackLenth = TrainCalculate.GetBackLenth(trainBackModules, MainTrainModule);
            FrontLenth = TrainCalculate.GetFrontLenth(trainFrontModules, MainTrainModule);
        }
        catch (Exception)
        {
            Debug.LogError("열차 길이 계산오류.");
            
        }
    }

    public void SetSpeed(float speed)
        => TrainSpeed = speed;

    private void OnEnable()
    {
        StartCoroutine(SetSpeed());
    }

    public IEnumerator SetSpeed()
    {
        //자료 초기화 까지 대기.
        yield return null;

        bool isPass = false;
        while (!isPass) // 배치 성공 전까지는 속력을 받지 않는다. 
        {
            if(LastRail != null) // 레일이 없으면 체크를 변경시도하지 않음. 
            isPass = IsPlaced(LastRail, LastDist);
            yield return null;
        }

        while (true)
        {
            float NextDist = TrainCalculate.GetNextDist(LastDist, TrainSpeed);
            SetDist(LastRail, NextDist);
            yield return null;
        }
    }

    public Rail Testrail;
    public float dist = 0;
    public bool go;
    private void Update()
    {
        if (go)
            SetDist(Testrail, dist);
        if (Input.GetKey(KeyCode.Plus))
            dist += Time.deltaTime * 10;
        if(Input.GetKey(KeyCode.Minus))
            dist -= Time.deltaTime * 10;


    }

}
