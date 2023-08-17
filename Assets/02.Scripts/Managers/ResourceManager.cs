using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GsDefaultModule
{
    public class ResourceManager
    {
        public T Load<T>(string path) where T : Object
        {
            return Resources.Load<T>(path);
        }

        public GameObject Instantiate(string path, Transform parent = null)
        {
            GameObject prefab = Load<GameObject>($"Prefabs/{path}");
            if (prefab == null)
            {
#if UNITY_EDITOR
            Debug.Log($"프리팹 로드에 실패했습니다. : {path}");
#endif
                return null;
            }
            return Object.Instantiate(prefab, parent);
        }

        public void Destroy(GameObject go)
        {
            if (go == null)
                return;
            Object.Destroy(go);
        }
    }
}
