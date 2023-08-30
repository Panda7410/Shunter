using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
#if UNITY_EDITOR
[CustomEditor(typeof(LineBuilderCore))]
[CanEditMultipleObjects]
public class LineBuilderEdit : Editor
{

    /// <summary>
    /// 원본 연결
    /// </summary>
    static LineBuilderCore CoreDummy;


    private void OnEnable()
    {
        if (AssetDatabase.Contains(target))
        {
            CoreDummy = null;
        }
        else
        {
            CoreDummy = (LineBuilderCore)target;
        }
    }
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("오브젝트 설치(플레이)", EditorStyles.miniButton)) {
            CoreDummy.SetOBJ();
        }
        if (GUILayout.Button("슬롯 세이브(플레이)", EditorStyles.miniButton)) {
            CoreDummy.SaveObjData();
        }
        if (GUILayout.Button("슬롯 로드(논 플레이)", EditorStyles.miniButton)) {
            CoreDummy.LoadObjData();
        }


        base.OnInspectorGUI();
    }


        // Start is called before the first frame update
        void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
#endif
