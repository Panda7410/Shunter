using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GsDefaultModule;

public class Managers : MonoBehaviour
{
    static Managers s_instance; // 유일성이 보장된다
    static Managers Instance { get { Init(); return s_instance; } } // 유일한 매니저를 갖고온다


    InputManager _input = new InputManager();
    ResourceManager _resource = new ResourceManager();
    //SceneChanger _sceneChanger = new SceneChanger();
    StatusManager _statusManager = new StatusManager();
    ScForEveObj _scForEveObj;
    SoundManager _soundManager;
    //UiPopupManager _uiPopupManager;

    public static InputManager Input { get { return Instance._input; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    //public static SceneChanger SceneChanger { get { return Instance._sceneChanger; } }
    public static Transform ManagerTr { get { return Instance.gameObject.transform; } }
    public static StatusManager StatusManager { get { return Instance._statusManager; } } 
    public static SoundManager SoundManager
    {
        get
        {
            if (Instance._soundManager == null)
                Instance._soundManager = Instance.gameObject.AddComponent<SoundManager>();
            return Instance._soundManager;
        }
    }
    public static ScForEveObj ScForEveObj
    {
        get
        {
            if (Instance._scForEveObj == null)
                Instance._scForEveObj = ScForEveObj.GetScForEveObj;
            return Instance._scForEveObj;
        }
    }
    //public static UiPopupManager UiPopupManager
    //{
    //    get
    //    {
    //        if (Instance._uiPopupManager == null)
    //            Instance._uiPopupManager = Resource.Instantiate("UI/PopUpCanvas").GetComponent<UiPopupManager>();
    //        return Instance._uiPopupManager;
    //    }
    //}
    
    
    void Start()
    {
        Init(this.gameObject);
    }
    void Update()
    {
        _input.OnUpdate();
    }
    static void Init(GameObject ManagerObj = null)
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();
        }
        else
        {
            if (ManagerObj != null && s_instance.gameObject != ManagerObj)
            { Destroy(ManagerObj); }
        }
    }
    public static bool ManagerIsNull
    {
        get
        {
            if (s_instance == null)
                return true;
            else
                return false;
        }
    }
}
