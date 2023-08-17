using System.Collections.Generic;
using UnityEngine;
using GSSC.Signal;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace GSSC.Condition
{
    [CreateAssetMenu(fileName = "ConditionDatas", menuName = "GSSC/DataSet/ConditionDatas")]

    public class ConditionDatas : DataScriptable<ConditionDatas>
    {
        public List<ConditionData> datas = new List<ConditionData>();

        public ConditionData GetConditionData(SignalTag Tag)
        {
            ConditionData data = datas.Find(t => t.Match(Tag));
            if (data == null)
            {
                data = new ConditionData(Tag);
                datas.Add(data);
            }
            return data;
        }

        [System.Serializable]
        public class ConditionData
        {
            [SerializeField]
            private SignalTag tag = new SignalTag();
            [SerializeField]
            private int _Count;

            //프로퍼티
            public SignalTag Tag { get { return tag; } }
            public int Count
            {
                get { return _Count; }
                set { _Count = value; }
            }

            

            public ConditionData() { }
            public ConditionData(SignalTag tag)
            {
                if(this.tag == null)
                this.tag = new SignalTag();
                this.tag.Group = tag.Group;
                this.tag.Tag = tag.Tag;
            }

            public bool Match(SignalTag tag)
            {
                if (this.tag.Group == tag.Group && this.tag.Tag == tag.Tag)
                    return true;
                else
                    return false;
            }
        }
    }
}