using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GSSC
{
    using UnityEditor;

#if UNITY_EDITOR
    [CustomEditor(typeof(SaveObjPos))]

    public class SaveObjPosEdit : Editor
    {
        SaveObjPos targetRef;
        // GetTarget;
        SerializedProperty targetDiscription;
        SerializedProperty targetMainOBJProp;
        SerializedProperty targetListProp;

        GUILayoutOption[] btnOption;

        void OnEnable()
        {
            targetRef = (SaveObjPos)target;
            //GetTarget = new SerializedObject(targetRef);
            //ThisList = GetTarget.FindProperty("subTestClasses"); // Find the List in our script and create a refrence of it
            targetDiscription = serializedObject.FindProperty($"{nameof(SaveObjPos.Discription)}");
            targetMainOBJProp = serializedObject.FindProperty($"{nameof(SaveObjPos.MainOBJ)}");
            targetListProp = serializedObject.FindProperty($"{nameof(SaveObjPos.saveObjPosItemDatas)}");


            btnOption = new GUILayoutOption[]
            {
                GUILayout.Height(40f),
                //GUI.
            };
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            EditorGUILayout.LabelField("포지션 저장 시스템.", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(targetDiscription);

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();


            if (GUILayout.Button("SAVE", btnOption))
                targetRef.SavePos();
            if (GUILayout.Button("LOAD", btnOption))
                targetRef.LoadPos();

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("등록용 최상위 부모 객체");
            EditorGUILayout.PropertyField(targetMainOBJProp);
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("수동컨트롤 리스트");
            EditorGUILayout.PropertyField(targetListProp);
            if (GUILayout.Button("모든 객체 등록"))
                targetRef.SetAllObj();

            serializedObject.ApplyModifiedProperties();
        }

    }
#endif
}