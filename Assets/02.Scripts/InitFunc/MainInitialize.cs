using GSSC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using UnityEngine.Events;

public class MainInitialize : MonoBehaviour
{
    Dictionary<string, int> OldValule = new Dictionary<string, int>();
    public UnityEvent OnLoadCompleted = new UnityEvent();
    public UnityEvent OnUnLoadCompleted = new UnityEvent();



    void Start()
    {
        NetInit();
    }

    /// <summary>
    /// 네트워크 기능 초기화
    /// </summary>
    void NetInit()
    {
        TrainManager.Instance.MsgAction -= NetworkFunc.SendMsg;
        TrainManager.Instance.MsgAction += NetworkFunc.SendMsg;

        ActionManager.Instance.AddAction(OrderKind.SET.ToString(), ItemList.자기진단실시, ObjectAction: HeartBeat);
        ActionManager.Instance.AddAction(OrderKind.SET.ToString(), ItemList.훈련제어, ObjectAction: trainingControl);
        ActionManager.Instance.AddAction(OrderKind.SET.ToString(), ItemList.그래픽로딩, ObjectAction: graphicLoading);
        ActionManager.Instance.AddAction(OrderKind.SET.ToString(), ItemList.SHUTDOWN_CODE, ObjectAction: SHUTDOWN_CODE);


        

        //Client.Instance.OnConnect += () => 
        //{
        //    Debug.LogWarning("컨넥트");
        //    TrainManager.Instance.MsgAction -= NetworkFunc.SendMsg;
        //    TrainManager.Instance.MsgAction += NetworkFunc.SendMsg;
        //};
        //Client.Instance.OnDisConnect += () =>
        //{
        //    Debug.LogWarning("디스컨넥트");

        //    TrainManager.Instance.MsgAction -= NetworkFunc.SendMsg;
        //};
    }

    #region FUNC

    /// <summary>
    /// 자기진단실시
    /// </summary>
    void HeartBeat(object obj)
    {
        //ItemList.자기진단실시
        int value = 0;
        if (!isIntValue(obj, ref value))
        {
            LogDisplay.LogError($"{ItemList.자기진단실시} 파싱에러");
            return;
        }
        //이전값과 같으면 리턴.
        if (CompareOldValue(ItemList.자기진단실시, value))
            return;
        LogDisplay.Log($"SET {ItemList.APP_선로영상} 1");
        Client.Instance.SendData(ItemList.APP_선로영상, 1);
    }

    /// <summary>
    /// 훈련제어
    /// </summary>
    void trainingControl(object obj)
    {
        //ItemList.훈련제어
        
        int value = 0;
        if(!isIntValue(obj, ref value))
        {
            LogDisplay.LogError($"{ItemList.훈련제어} 파싱에러");
            return;
        }
        //이전값과 같으면 리턴.
        if (CompareOldValue(ItemList.훈련제어, value))
            return;
        if (value == 1)
        {

        }
        else if(value == 2)
        {
            //메인으로 이동.
            UnLoadPlayScene();
        }
    }

    /// <summary>
    /// 시스템제어
    /// </summary>
    void systemControl(object obj)
    {
        //ItemList.시스템제어
        int value = 0;
        if (!isIntValue(obj, ref value))
        {
            LogDisplay.LogError($"{ItemList.시스템제어} 파싱에러");
            return;
        }
        //이전값과 같으면 리턴.
        if (CompareOldValue(ItemList.시스템제어, value))
            return;


        if (value == 1)
        {
            //어드바이스 요청. //연결시 어드바이스 하기때문에 실제로는 암것도 안함.
        }
        else if (value == 2) // 이거 왜 시스템 제어인데 CG 로딩응답을해??
        {
            //CG 로딩응답 0
            LogDisplay.Log($"Set {ItemList.CG로딩응답} 0");
            Client.Instance.SendData(ItemList.CG로딩응답, 0);
        }
        else if (value == 3)
        {
            LogDisplay.Log($"{ItemList.시스템제어} 3 - ShutDown.exe 실행");
            Client.Instance.SendData(ItemList.APP_선로영상, 1);
            //프로그램 종료. 
            StartCoroutine(KillProgram());
        }
    }

    /// <summary>
    /// 그래픽로딩
    /// </summary>
    void graphicLoading(object obj)
    {
        int value = 0;
        if (!isIntValue(obj, ref value))
        {
            LogDisplay.LogError($"{ItemList.그래픽로딩} 파싱에러");
            return;
        }
        if (value == 1)
        {
            LogDisplay.Log($"Set {ItemList.CG로딩응답} 1");
            Client.Instance.SendData(ItemList.CG로딩응답, 1);

            // 여기서 실제 씬을 로드할것.
            LoadPlayscene();
        }
    }

    /// <summary>
    /// SHUTDOWN_CODE
    /// </summary>
    void SHUTDOWN_CODE(object obj)
    {
        string value = obj.ToString();
        if (value != ItemList.APP_선로영상) // 영상으로 온것이 아닐경우리턴.
            return;
        Client.Instance.SendData(ItemList.APP_선로영상, 1);
        //프로그램 종료. 
        StartCoroutine(KillProgram());

    }




    #endregion


    #region 씬로드

    public void LoadPlayscene()
    {
        LoadPlayScene loadScene = new LoadPlayScene();
        loadScene.OnLoadCompleted += () => { OnLoadCompleted?.Invoke(); };
        loadScene.LoadScene();
    }

    public void UnLoadPlayScene()
    {
        LoadPlayScene loadScene = new LoadPlayScene();
        loadScene.OnUnLoadCompleted += () => { OnUnLoadCompleted?.Invoke(); };
        loadScene.UnLoadScene();

    }
    #endregion


    #region 기본기능

    /// <summary>
    /// 같으면 트루를 반환. 
    /// </summary>
    /// <param name="Key"></param>
    /// <param name="Value"></param>
    /// <returns></returns>
    bool CompareOldValue(string Key, int Value)
    {
        if (!OldValule.ContainsKey(Key))
        {
            OldValule.Add(Key, Value); // 없을경우 값을 입력하고 같지않음 리턴. 
            return false;
        }
        if(OldValule[Key] == Value)
        {
            return true;
        }
        else
        {
            OldValule[Key] = Value; // 최근값으로 변경하고 같지 않음 리턴. 
            return false;
        }
    }

    /// <summary>
    /// obj 를 int 형식으로 변환. 실패시 false 반환
    /// </summary>
    /// <param name="obj">변환할 값</param>
    /// <param name="OutInt">출력값.</param>
    /// <returns></returns>
    bool isIntValue(object obj, ref int OutInt)
    {
        int value;
        if (obj == null)
        {
            return false;
        }
        if (!int.TryParse(obj.ToString(), out value))
        {
            return false;
        }
        OutInt = value;
        return true;
    }

    IEnumerator KillProgram()
    {
        Client.Instance.forceDispose();
        yield return new WaitForSeconds(3f);

        System.Diagnostics.Process.Start("ShutDown.exe", "-s -f -t 0");
        UnityEngine.Application.Quit();
    }

    #endregion
}
