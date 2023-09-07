using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 비동기 함수를 유니티 생명주기안에서 실행하기 위한 클래스. 
/// 해당 프레임의 마지막에 쌓인 모든 액션을 실행한다.
/// </summary>
public class ActionQueue : SimpleSingleton<ActionQueue>
{
    public Queue<Action> actions = new Queue<Action>();
    private object _lock = new object();
    private void Start()
    {
        StartCoroutine(Loop());
    }

    public static void EnQueue(Action action)
    {
        lock (Instance._lock)
        {
            Instance.actions.Enqueue(action);
        }
    }


    private IEnumerator Loop()
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();
        while (true)
        {
            yield return null;
            //프레임의 마지막까지 대기한다.
            yield return wait;
            while (actions.Count != 0)
            {
                lock (Instance._lock)
                {
                    actions.Dequeue()?.Invoke();
                }
            }
        }
    }
}
