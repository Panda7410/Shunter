using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace GSSC
{
    public class DataScriptable<T> : ScriptableObject where T : ScriptableObject
    {
        private const string SettingFileDirectory = "Assets/Resources/Data";
        private const string SettingFilePath = "Assets/Resources/Data/";
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }
                _instance = Resources.Load<T>(path: $"Data/{typeof(T).Name}");
#if UNITY_EDITOR

                if (_instance == null)
                {
                    if (!AssetDatabase.IsValidFolder(path: SettingFileDirectory))
                    {
                        AssetDatabase.CreateFolder("Data", "Resources");
                    }
                    string path = $"{SettingFilePath}{typeof(T).Name}.asset";
                    _instance = AssetDatabase.LoadAssetAtPath<T>(path);
                    if (_instance == null)
                    {
                        _instance = CreateInstance<T>();
                        AssetDatabase.CreateAsset(_instance, path);
                    }
                }
#endif
                return _instance;
            }
        }
    }
}

