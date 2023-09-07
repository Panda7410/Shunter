using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LogDisplay : MonoBehaviour
{
    static LogDisplay _Log;

    public TextMeshProUGUI logWindow;
    public GameObject logCanvas;


    static LogDisplay mainLog
    {
        get
        {
            if (_Log == null)
                _Log = FindObjectOfType<LogDisplay>();
            if (_Log == null)
            {
                Managers.Resource.Instantiate("UI/LogCanvas");
                _Log = FindObjectOfType<LogDisplay>();
            }
            if (_Log == null)
                Debug.LogError("커스텀 로그 설정에 에러가 존재합니다. ");

            return _Log;
        }
    }


    public static void Log(object message)
    {
#if UNITY_EDITOR
        Debug.Log(message);
#endif

        string Message = message.ToString();

        if (mainLog.logWindow.text.Length > 20000)
            mainLog.logWindow.text = null;
        mainLog.logWindow.text = System.DateTime.Now.ToString("hh:mm:ss") + " : " + Message + "\n" + mainLog.logWindow.text;
    }
    public static void LogWarning(object message)
    {
#if UNITY_EDITOR
        Debug.LogWarning(message);
#endif
        
        string Message = message.ToString();

        if (mainLog.logWindow.text.Length > 20000)
            mainLog.logWindow.text = null;
        mainLog.logWindow.text = System.DateTime.Now.ToString("hh:mm:ss") + " : " + "<color=yellow>" + Message + "</color>" + "\n" + mainLog.logWindow.text;
    }
    public static void LogError(object message)
    {
#if UNITY_EDITOR
        Debug.LogError(message);
#endif
       
        string Message = message.ToString();

        if (mainLog.logWindow.text.Length > 20000)
            mainLog.logWindow.text = null;
        mainLog.logWindow.text = System.DateTime.Now.ToString("hh:mm:ss") + " : " + "<color=red>" + Message + "</color>" + "\n" + mainLog.logWindow.text;
    }




    // Start is called before the first frame update
    void Start()
    {
        Managers.Input.KeyAction += () =>
        {
            if (Input.GetKeyDown(KeyCode.F2))
                logCanvas.SetActive(logCanvas.activeSelf ? false : true);
            if (Input.GetKeyDown(KeyCode.L))
                logCanvas.SetActive(logCanvas.activeSelf ? false : true);
        };
    }
    private void OnDestroy()
    {
        
    }
}
