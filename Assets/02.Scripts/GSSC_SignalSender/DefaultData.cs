using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace GSSC
{
    [CreateAssetMenu(fileName = "DefaultData", menuName = "GSSC/DataSet/DefaultData")]
    public class DefaultData : ScriptableObject
    {
        private const string SettingFileDirectory = "Assets/Resources/Data";
        private const string SettingFilePath = "Assets/Resources/Data/DefaultData.asset";
        private static DefaultData _instance;
        public static DefaultData Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }
                _instance = Resources.Load<DefaultData>(path: "Data/DefaultData");
#if UNITY_EDITOR

                if (_instance == null)
                {
                    if (!AssetDatabase.IsValidFolder(path: SettingFileDirectory))
                    {
                        AssetDatabase.CreateFolder("Data", "Resources");
                    }
                    _instance = AssetDatabase.LoadAssetAtPath<DefaultData>(SettingFilePath);
                    if (_instance == null)
                    {
                        _instance = CreateInstance<DefaultData>();
                        AssetDatabase.CreateAsset(_instance, SettingFilePath);
                    }
                }
#endif
                return _instance;
            }
        }

        [SerializeField]
        private List<TagSet> tagSets = new List<TagSet>();
        private List<string> _signalGroupTag = new List<string>();
        private List<string> _signalTag = new List<string>();


        //프로퍼티
        //public List<string> SignalTag => _signalTag;


        //그룹태그

        public List<string> SignalGroupTag
        {
            get
            {
                _signalGroupTag.Clear();
                for (int i = 0; i < tagSets.Count; i++)
                    _signalGroupTag.Add(tagSets[i].TagGroupName);
                return _signalGroupTag;
            }
        }

        public List<string> GetTags(string GroupName)
        {
            TagSet Set = tagSets.Find(t => t.MatchTag(GroupName));
            if (Set != null)
                _signalTag = Set.SignalTag;

            return _signalTag;
        }
    }
    [System.Serializable]
    public class TagSet
    {
        [SerializeField]
        string _TagGroupName;
        [SerializeField]
        List<string> _signalTag = new List<string>();

        public bool MatchTag(string TagGroupName)
        => this.TagGroupName == TagGroupName ? true : false;
        public string TagGroupName => _TagGroupName;
        public List<string> SignalTag => _signalTag;
    }

}