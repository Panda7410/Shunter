using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(SignalRecv))]
public class SignalRecvEdit : Editor
{
    SignalRecv targetRef;

    private void OnEnable()
    {
        targetRef = (SignalRecv)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
        if (GUILayout.Button("재등록")) targetRef.ReReg();
        serializedObject.ApplyModifiedProperties();
    }
}
#endif