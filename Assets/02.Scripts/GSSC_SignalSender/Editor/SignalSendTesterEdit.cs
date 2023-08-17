using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(SignalSendTester))]
public class SignalSendTesterEdit : Editor
{
    SignalSendTester targetRef;

    SerializedProperty targetSignalProp;
    SerializedProperty targetBoolProp;
    SerializedProperty targetIntProp;
    SerializedProperty targetFloatProp;
    SerializedProperty targetStringProp;

    private void OnEnable()
    {
        targetRef = (SignalSendTester)target;
        targetSignalProp = serializedObject.FindProperty($"{nameof(SignalSendTester.Signal)}");
        targetBoolProp = serializedObject.FindProperty($"{nameof(SignalSendTester.ActionBool)}");
        targetIntProp = serializedObject.FindProperty($"{nameof(SignalSendTester.ActionInt)}");
        targetFloatProp = serializedObject.FindProperty($"{nameof(SignalSendTester.ActionFloat)}");
        targetStringProp = serializedObject.FindProperty($"{nameof(SignalSendTester.ActionString)}");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(targetSignalProp);
        if (GUILayout.Button("Send")) targetRef.Send();
        EditorGUILayout.PropertyField(targetBoolProp);
        if (GUILayout.Button("SendBool")) targetRef.SendBool();
        EditorGUILayout.PropertyField(targetIntProp);
        if (GUILayout.Button("SendInt")) targetRef.SendInt();
        EditorGUILayout.PropertyField(targetFloatProp);
        if (GUILayout.Button("SendFloat")) targetRef.SendFloat();
        EditorGUILayout.PropertyField(targetStringProp);
        if (GUILayout.Button("SendString")) targetRef.SendString();
        serializedObject.ApplyModifiedProperties();
    }

}
#endif