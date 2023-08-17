namespace GSSC.Condition
{
    using GSSC.Signal;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    [AddComponentMenu("GSSC/조건확인용")]
    public class ConditionListComponet : MonoBehaviour
    {
        
        [Header("조건을 만족하는지, Bool값 확인용 컴포넌트 입니다. ")]
        [Header("조건리스트")]
        public List<ConditionBaseSelector> Selectors = new List<ConditionBaseSelector>();
        public List<ConditionData> conditions = new List<ConditionData>();
        public bool GetResult()
        {
            //bool isPass = true;
            for (int i = 0; i < Selectors.Count; i++)
            {
                if (Selectors[i] == null)
                    continue;
                if (!Selectors[i].GetResult())
                {
                    return false;
                }
            }
            for (int i = 0; i < conditions.Count; i++)
            {
                if (!conditions[i].GetResult())
                    return false;
            }
            return true;
        }


        [System.Serializable]
        public class ConditionData
        {
            public SignalTag ValueName;
            public ConditionCalculate Calculate;
            public int Count;

            public bool GetResult()
            {
                if (Calculate == null)
                    return false;
                return Calculate.GetResult(ValueName, Count);
            }
        }
    }
}