using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrainManager : SimpleSingleton<TrainManager>
{
    public Action<string> MsgAction;

    //public List<TrainModule> AllModules = new List<TrainModule>();
    public Dictionary<string, TrainModule> AllModules = new Dictionary<string, TrainModule>();

    //public List<Train> AllTrains = new List<Train>();
    public Dictionary<string, Train> AllTrains = new Dictionary<string, Train>();


    public void CreateModule(string trianType, string moduleId, string railId, float dist, int direct = 0)
    {
        //레일 검사.
        Rail rail;
        if(!RailManager.Instance.AllRails.TryGetValue(railId, out rail))
        {
            LogDisplay.LogError($"{railId}선로가 존재하지 않아 {moduleId} 를 배치할 수 없습니다.");
            Instance.MsgAction?.Invoke($"{railId}선로가 존재하지 않아 {moduleId} 를 배치할 수 없습니다.");
            return;
        }

        if (AllModules.ContainsKey(moduleId))
        {
            LogDisplay.LogError($"{moduleId}가 이미 존재합니다.");
            Instance.MsgAction?.Invoke($"{moduleId}가 이미 존재합니다.");

            return;
        }

        TrainModule module = CreateModule(trianType, moduleId);
        if (module == null)
        {
            LogDisplay.Log($"{moduleId} 생성실패.");
            Instance.MsgAction?.Invoke($"{moduleId} 생성실패.");

            return;
        }
        // 생성 혹은 찾기 완료.
        module.SetTrain(rail, dist);
        module.TrainDirect = direct == 0 ? false : true;
        AllModules.Add(moduleId,module);

        module.OnDestroyTrianAction += () => 
        {
            if (AllModules.ContainsValue(module))
                AllModules.Remove(module.ModuleID);
        };
        LogDisplay.Log($"{moduleId} 생성을 완료했습니다.");
        Instance.MsgAction?.Invoke($"{moduleId} 생성을 완료했습니다.");

    }
    public TrainModule CreateModule(string trianType, string moduleId)
    {
        TrainModule module;
        //이미 열차가 존재하는지 검사.
        //열차에 포함되어 있는경우.
        if (IsTrainContainModule(moduleId))
        {
            LogDisplay.LogError($"{moduleId}가 이미 다른열차에 포함되어있어 개별 위치를 지정할 수 없습니다.");
            Instance.MsgAction?.Invoke($"{moduleId}가 이미 다른열차에 포함되어있어 개별 위치를 지정할 수 없습니다.");

            return null;
        }
        //열차에는 없지만 생성된 된경우. 
        else if (AllModules.ContainsKey(moduleId))
        {
            LogDisplay.LogWarning($"{moduleId}가 이미 생성되어 있습니다. 동작에 주의하세요.");
            Instance.MsgAction?.Invoke($"{moduleId}가 이미 생성되어 있습니다. 동작에 주의하세요.");

            module = AllModules[moduleId];
        }
        //최종적으로 생성한다. 
        else
        {
            module = GSSC.TrainDataSet.Instance.CreatTainModule(trianType);
            if (module == null)
            {
                LogDisplay.LogError($"{trianType}에 해당하는 열차 데이터가 존재하지 않습니다.");
                Instance.MsgAction?.Invoke($"{trianType}에 해당하는 열차 데이터가 존재하지 않습니다.");

                return null;
            }
            module.SetID(moduleId);
        }
        return module;
    }
    public TrainModule GetModoule(string moduleId)
    {
        if (!AllModules.ContainsKey(moduleId))
        {
            Instance.MsgAction?.Invoke($"{moduleId}에 해당하는 차량 데이터가 존재하지 않습니다.");
            LogDisplay.LogError($"{moduleId}에 해당하는 차량 데이터가 존재하지 않습니다.");
            return null;
        }
        return AllModules[moduleId];
    }
    public Train CreatTrain(string trainId, TrainModule mainModule)
    {
        if(mainModule == null)
        {
            LogDisplay.LogError($"설정된 모듈이 존재하지 않습니다.");
            Instance.MsgAction?.Invoke($"설정된 모듈이 존재하지 않습니다.");
            return null;
        }
        if (AllTrains.ContainsKey(trainId))
        {
            LogDisplay.LogError($"{trainId} 동일 이름을 지닌 열차가 이미 생성되어 있습니다.");
            Instance.MsgAction?.Invoke($"{trainId} 동일 이름을 지닌 열차가 이미 생성되어 있습니다.");
            return null;
        }
        if (IsTrainContainModule(mainModule.ModuleID))
        {
            LogDisplay.LogError($"{mainModule.ModuleID} 메인차량이 이미 다른 열차에 포함되어 있습니다. ");
            Instance.MsgAction?.Invoke($"{mainModule.ModuleID} 메인차량이 이미 다른 열차에 포함되어 있습니다.");
            return null;
        }
        
        GameObject trainObj = new GameObject($"Train_{trainId}");
        Train train = trainObj.AddComponent<Train>();
        train.SetID(trainId);
        train.SetMainModule(mainModule);
        train.SendMsg += MsgAction;
        AllTrains.Add(trainId, train);
        LogDisplay.Log($"{trainId} 열차 생성완료");
        Instance.MsgAction?.Invoke($"{trainId} 열차 생성완료");

        return train;
    }
    public Train CreatTrain(string trainId, string mainModuleID)
    {
        if (!AllModules.ContainsKey(mainModuleID))
        {
            LogDisplay.LogError($"{mainModuleID}를 지닌 모듈이 존재하지 않습니다. ");
            Instance.MsgAction?.Invoke($"{mainModuleID}를 지닌 모듈이 존재하지 않습니다.");
            return null;
        }
        return CreatTrain(trainId, AllModules[mainModuleID]);
    }
    public (bool isContain, Train train) GetInstallTrain(string trainId)
    {
        Train train;
        return (AllTrains.TryGetValue(trainId, out train), train);
    }

    public void AttachFrontModule(string trainId, List<TrainModule> FrontTrainModules)
    {
        (bool isContain, Train train) = GetInstallTrain(trainId);
        if (!isContain)
        {
            LogDisplay.LogError($"{trainId} 에 해당하는 열차가 등록되어 있지 않습니다.");
            Instance.MsgAction?.Invoke($"{trainId} 에 해당하는 열차가 등록되어 있지 않습니다.");
            return;
        }
        train.AddFrontModule(FrontTrainModules);
    }
    public void AttachBackModule(string trainId, List<TrainModule> BackTrainModules)
    {
        (bool isContain, Train train) = GetInstallTrain(trainId);
        if (!isContain)
        {
            LogDisplay.LogError($"{trainId} 에 해당하는 열차가 등록되어 있지 않습니다.");
            Instance.MsgAction?.Invoke($"{trainId} 에 해당하는 열차가 등록되어 있지 않습니다.");
            return;
        }
        train.AddBackModule(BackTrainModules);
    }

    public void BreakTrainCoupling(string trainId, string moduleId)
    {
        (bool isContain, Train train) = GetInstallTrain(trainId);
        if (!isContain)
        {
            LogDisplay.LogWarning($"{trainId} 에 해당하는 열차가 등록되어 있지 않습니다.");
            Instance.MsgAction?.Invoke($"{trainId} 에 해당하는 열차가 등록되어 있지 않습니다.");
            return;
        }
        train.BreakTrainCoupling(moduleId);
        LogDisplay.Log("분리완료.");
    }


    public void DestroyModule(string ModuleId)
    {
        if(!AllModules.ContainsKey(ModuleId))
        {
            LogDisplay.LogWarning($"{ModuleId} 가 등록되어있지 않아 모듈을 제거할 수 없습니다.");
            Instance.MsgAction?.Invoke($"{ModuleId} 가 등록되어있지 않아 모듈을 제거할 수 없습니다.");
            return;
        }
        TrainModule module = AllModules[ModuleId];

        //모듈이 포함된 열차가 존재할경우.
        if (IsTrainContainModule(ModuleId)) // 열차에서 커플링 브레이킹.
            BreakTrainCoupling(GetTrainContainModule(ModuleId).TrainID, ModuleId);

        AllModules.Remove(ModuleId); // 딕셔너리에서 제거.

        Destroy(module.gameObject); // 오브젝트 제거.
    }
    public void DestroyTrain(string TrainId)
    {
        if (!AllTrains.ContainsKey(TrainId))
        {
            LogDisplay.LogWarning($"{TrainId} 가 등록되어있지 않아 열차를 제거할 수 없습니다.");
            Instance.MsgAction?.Invoke($"{TrainId} 가 등록되어있지 않아 열차를 제거할 수 없습니다.");
            return;
        }

        Train train = AllTrains[TrainId];
        Destroy(train.gameObject);// 오브젝트 제거
        //목록에서 제거. 
        AllTrains.Remove(TrainId);
    }

    public bool IsTrainContainModule(string moduleId)
    {
        List<string> keys = new List<string>(AllTrains.Keys);

        for (int i = 0; i < keys.Count; i++)
        {
            if(AllTrains[keys[i]].IsContainTrianModule(moduleId))
                return true;
        }

        return false;
    }

    public Train GetTrainContainModule(string moduleId)
    {
        List<string> keys = new List<string>(AllTrains.Keys);

        for (int i = 0; i < keys.Count; i++)
        {
            if (AllTrains[keys[i]].IsContainTrianModule(moduleId))
                return AllTrains[keys[i]];
        }

        return null;
    }

    public void SetTrainSpeed(string trainId, float dist)
    {
        GetTrainContainModule(trainId)?.SetSpeed(dist);
    }

    public void SetTrainPos(string trainId, string railId, float dist)
    {
        if (!AllTrains.ContainsKey(trainId))
        {
            LogDisplay.LogWarning($"{trainId} 가 현재 등록되어있지 않습니다.");
            Instance.MsgAction?.Invoke($"{trainId} 가 현재 등록되어있지 않습니다.");
            return;
        }
        if (!RailManager.Instance.AllRails.ContainsKey(railId))
        {
            LogDisplay.LogWarning($"{railId} 선로가 가 현재 등록되어있지 않습니다.");
            Instance.MsgAction?.Invoke($"{railId} 선로가 가 현재 등록되어있지 않습니다.");
            return;
        }

        AllTrains[trainId].SetDist(RailManager.Instance.AllRails[railId], dist);
    }
    public void SetModulePos(string ModuleId, string railId, float dist)
    {
        if (!AllModules.ContainsKey(ModuleId))
        {
            LogDisplay.LogWarning($"{ModuleId} 가 현재 등록되어있지 않습니다.");
            Instance.MsgAction?.Invoke($"{ModuleId} 가 현재 등록되어있지 않습니다.");
            return;
        }
        if (!RailManager.Instance.AllRails.ContainsKey(railId))
        {
            LogDisplay.LogWarning($"{railId} 선로가 가 현재 등록되어있지 않습니다.");
            Instance.MsgAction?.Invoke($"{railId} 선로가 가 현재 등록되어있지 않습니다.");
            return;
        }
        if(IsTrainContainModule(ModuleId))
        {
            LogDisplay.LogWarning($"{ModuleId} 가 이미 다른 열차에 포함되어 있습니다.");
            Instance.MsgAction?.Invoke($"{ModuleId} 가 이미 다른 열차에 포함되어 있습니다.");
            return;
        }
        AllModules[ModuleId].SetTrain(RailManager.Instance.AllRails[railId], dist);
    }

    public (string ModuleId, string railId, float dist)[] getAllTrainPos()
    {
        (string ModuleId, string railId, float dist)[] ps = new (string ModuleId, string railId, float dist)[AllModules.Count];

        //AllModules.Keys.ToList()    
        int index = 0;
        try
        {
            if(AllModules.Count != 0)
            foreach (KeyValuePair<string, TrainModule> item in AllModules)
            {
                ps[index] = (item.Value.ModuleID, item.Value.nowRail.RailID, item.Value.NowDist);
                index++;
            }
        }
        catch (System.Exception)
        {

            LogDisplay.LogError("열차 설정에 에러가 존재합니다.");
            Instance.MsgAction?.Invoke($"열차 설정에 에러가 존재합니다.");
            throw;
        }
        return ps;
    }

    public (string ModuleId, string railId, float dist) getTrainPos(string ModuleID)
    {
        string MID = ModuleID;
        string RID;
        float Dist = 0;

        TrainModule module = GetModoule(ModuleID);
        RID = module?.nowRail.RailID;
        if(module != null)
        Dist = module.NowDist;

        return (MID, RID, Dist);
    }

    public void DestroyAllTrain()
    {
        List<string> keys = AllTrains.Keys.ToList<string>();
        foreach (var trainID in keys)
        {
            DestroyTrain(trainID);
        }
    }
    public void DestroyAllModule()
    {
        List<string> keys = AllModules.Keys.ToList<string>();
        foreach (var ModuleID in keys)
        {
            DestroyModule(ModuleID);
        }
    }
}
