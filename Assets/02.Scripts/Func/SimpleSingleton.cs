using UnityEngine;

public class SimpleSingleton<T> : MonoBehaviour where T : Component
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if(_instance == null)
            {
                GameObject obj = new GameObject();
                obj.name = typeof(T).Name;
                obj.hideFlags = HideFlags.DontSave;
                _instance = obj.AddComponent<T>();
            }
            return _instance;
        }
    }
    protected virtual void OnDestroy()
    {
        if(_instance == this)
        {
            _instance = null;
        }
    }
    protected virtual void Awake()
    {
        if(_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

}
