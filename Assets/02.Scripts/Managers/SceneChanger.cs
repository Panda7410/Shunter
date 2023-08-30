using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger
{

    #region 일반 씬 로드
    public void LoadScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
    public void LoadScene(int SceneIndex)
    {
        if (SceneManager.sceneCount > SceneIndex + 1)
        {
            Debug.LogError($"{SceneIndex} 로드하고자 하는 씬 인덱스 범위가 잘못되었습니다.");
            return;
        }
        SceneManager.LoadScene(SceneIndex);
    }
    #endregion

    #region 비동기 씬 로드(추가)

    /// <summary>
    /// 씬을 비동기로 추가한다.
    /// </summary>
    /// <param name="SceneName"></param>
    public AsyncOperation LoadSceneAsyncAdditive(string SceneName)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);
        return op;
    }

    /// <summary>
    /// 씬을 비동기로 추가하고 완료시 액션을 수행한다
    /// </summary>
    /// <param name="SceneName">씬 이름</param>
    /// <param name="OnCompletedAction">완료시 실행할 액션</param>
    public void LoadSceneAsyncAdditive(string SceneName, Action OnCompletedAction)
    {
        AsyncOperation op = LoadSceneAsyncAdditive(SceneName);
        if (op == null)
            return;

        Managers.ScForEveObj.StartCoroutine(OnLoadCompleted(op, OnCompletedAction));
    }

    IEnumerator OnLoadCompleted(AsyncOperation op, Action OnCompletedAction)
    {
        while (!op.isDone)
        {
            yield return null;
        }
        OnCompletedAction?.Invoke();
    }

    #endregion

    public AsyncOperation UnloadSceneAsync(string SceneName)
    {
        Scene activeScene = SceneManager.GetSceneByName(SceneName);
        if (activeScene == null)
        {
            Debug.LogWarning($"{SceneName} 씬이 현재 로드되어있지 않거나 빌드에 포함되어 있지 않습니다.");
            return null;
        }

        if (activeScene.isLoaded)
        {
            return SceneManager.UnloadSceneAsync(activeScene);
        }
        else
        {
            Debug.LogWarning($"{SceneName} 씬이 현재 로드되어있지 않습니다.");
            return null;
        }
    }

    public void UnloadSceneAsync(string SceneName, Action OnCompletedAction)
    {
        AsyncOperation op = UnloadSceneAsync(SceneName);
        if (op == null)
            return;
        Managers.ScForEveObj.StartCoroutine(OnLoadCompleted(op, OnCompletedAction));
    }
}
