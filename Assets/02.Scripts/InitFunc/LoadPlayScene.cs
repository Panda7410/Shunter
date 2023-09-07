using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadPlayScene
{
    const string LineInfo = "LineInfo";
    const string BackGround = "BackGround";
    const string Play = "Play";

    public Action OnLoadCompleted;
    public Action OnUnLoadCompleted;



    public void LoadScene()
    {
        if (!isLoaded(LineInfo))
        {
            Managers.SceneChanger.LoadSceneAsyncAdditive(LineInfo);
        }
        else
            LogDisplay.LogWarning($"{LineInfo} 씬이 이미 로드 되어있습니다.");

        if (!isLoaded(Play))
        {
            Managers.SceneChanger.LoadSceneAsyncAdditive(Play);
        }
        else
            LogDisplay.LogWarning($"{Play} 씬이 이미 로드 되어있습니다.");

        if (!isLoaded(BackGround))
        {
            Managers.SceneChanger.LoadSceneAsyncAdditive(BackGround, LoadCompleted);
        }
        else
            LogDisplay.LogWarning($"{BackGround} 씬이 이미 로드 되어있습니다.");



        bool isLoaded(string ScneName)
        {
            Scene scene = SceneManager.GetSceneByName(ScneName);
            return scene.isLoaded;
        }
    }

    void LoadCompleted()
    {
        //백그라운드 로드 완료.
        OnLoadCompleted?.Invoke();
    }

    public void UnLoadScene()
    {
        // 열차 데이터를 지운다. 
        TrainManager.Instance.DestroyAllTrain();
        TrainManager.Instance.DestroyAllModule();

        //언로드 개시
        Managers.SceneChanger.UnloadSceneAsync(Play);
        Managers.SceneChanger.UnloadSceneAsync(LineInfo);
        Managers.SceneChanger.UnloadSceneAsync(BackGround, UnLoadCompleted);
    }

    void UnLoadCompleted()
    {
        //백그라운드 언로드 완료.
        OnUnLoadCompleted?.Invoke();
    }

}
