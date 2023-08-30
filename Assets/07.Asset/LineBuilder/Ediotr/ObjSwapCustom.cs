using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
#if UNITY_EDITOR
[CustomEditor(typeof(ObjSwapInEdit))]
[CanEditMultipleObjects]

public class ObjSwapCustom : Editor
{
    static ObjSwapInEdit ObjSwapInEdit;

    private void OnEnable()
    {
        if (AssetDatabase.Contains(target))
        {
            ObjSwapInEdit = null;
        }
        else
        {
            ObjSwapInEdit = (ObjSwapInEdit)target;
        }
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("교체", EditorStyles.miniButton))
        {

            ObjSwapInEdit.SwapGameObj();

        }




    }
}
#endif
