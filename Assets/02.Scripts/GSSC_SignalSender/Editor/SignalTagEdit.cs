namespace GSSC.Signal
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
#if UNITY_EDITOR
    using GSSC;
    using UnityEditor;
    [CustomPropertyDrawer(typeof(SignalTag))]
    public class SignalTagEdit : PropertyDrawer
    {
        int GroupFoldCount = 0;
        int FoldCount = 0;


        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Rect rect = new Rect(position.x, position.y, position.width, 16);
            SerializedProperty propertyGroup = property.FindPropertyRelative($"{nameof(SignalTag.Group)}");
            SerializedProperty propertyTag = property.FindPropertyRelative($"{nameof(SignalTag.Tag)}");

            EditorGUI.LabelField(rect, label);
            rect.y += 16;
            {
                //그룹
                rect.width = position.width / 2; // 크기, 위치 설정.
                                                 //이걸안해주면 강제로 초기값인 0으로 고정된다.
                if (DefaultData.Instance.SignalGroupTag.Contains(propertyGroup.stringValue))
                    GroupFoldCount = DefaultData.Instance.SignalGroupTag.IndexOf(propertyGroup.stringValue);
                //접히는 부분. 변경된 카운트도 넣어줘야한다.
                GroupFoldCount = EditorGUI.Popup(rect, GroupFoldCount, DefaultData.Instance.SignalGroupTag.ToArray());
                //실제 값을 부여하는부분.
                if (DefaultData.Instance.SignalGroupTag.Count > GroupFoldCount)
                    propertyGroup.stringValue = DefaultData.Instance.SignalGroupTag[GroupFoldCount];
                else
                    propertyGroup.stringValue = "";
            }
            {
                //태그
                rect.x += position.width / 2;
                //초기갱신값설정.
                if (DefaultData.Instance.GetTags(propertyGroup.stringValue).Contains(propertyTag.stringValue))
                    FoldCount = DefaultData.Instance.GetTags(propertyGroup.stringValue).IndexOf(propertyTag.stringValue);
                //접히는 부분. 변경된 카운트도 넣어줘야한다.
                FoldCount = EditorGUI.Popup(rect, FoldCount, DefaultData.Instance.GetTags(propertyGroup.stringValue).ToArray());
                //실제 값을 부여하는부분.
                if (DefaultData.Instance.GetTags(propertyGroup.stringValue).Count > FoldCount)
                    propertyTag.stringValue = DefaultData.Instance.GetTags(propertyTag.stringValue)[FoldCount];
                else
                    propertyTag.stringValue = "";
            }
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            //PropertyDrawer 사이즈 계산.
            int cnt = 0;
            foreach (var prop in property)
            {
                cnt++;
            }
            return EditorGUIUtility.singleLineHeight * (cnt + 1) + 6;
        }
    }
#endif
}