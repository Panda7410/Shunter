namespace GSSC.Condition
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using GSSC.Signal;

    [AddComponentMenu("GSSC/변수&&이벤트 초기화")] 
    public class InitializeConditonValue : MonoBehaviour
    {
        [TextArea]
        public string Discription;

        [Header("활성시 값 초기화")]
        public bool isInitStart = false;
        [Header("초기화 할 태그 수치")]
        public List<InitValue> InitValues = new List<InitValue>();
        [Header("초기화 할 태그 콜백이벤트")]
        public List<ConditionEventCallbackBase> Callbacks = new List<ConditionEventCallbackBase>();

        private void OnEnable()
        {
            if (isInitStart)
                Call();
        }

        public void Call()
        {
            InitValues.ForEach(x => 
            {
                ConditionDatas.ConditionData data = ConditionDatas.Instance.GetConditionData(x.ValueName);
                data.Count = x.Count;
                Debug.Log($"{data.Tag.Group}:{data.Tag.Tag} 초기값 설정 => {data.Count}");
            });
            Callbacks.ForEach(x => x.RegEv());
        }
        [System.Serializable]
        public class InitValue
        {
            public SignalTag ValueName;
            public int Count;

            public bool Match(int count)
            {
                if (Count == count)
                    return true;
                return false;
            }
        }
    }

}