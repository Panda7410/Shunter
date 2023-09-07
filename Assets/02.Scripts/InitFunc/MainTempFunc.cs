using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainTempFunc : MonoBehaviour
{
    [ContextMenu("씬로드")]
    public void LoadScene()
    {
        FindObjectOfType<MainInitialize>().LoadPlayscene();
    }
    [ContextMenu("씬언로드")]

    public void UnLoadScene()
    {
        FindObjectOfType<MainInitialize>().UnLoadPlayScene();
    }
}
