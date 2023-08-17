namespace GSSC.Condition
{
    using System.Collections;
    using System.Collections.Generic;
    using GSSC.Signal;
    using UnityEngine;
    [CreateAssetMenu(fileName = "Condition_", menuName = "GSSC/ConditionSelector")]

    public class ConditionSelector : ConditionBaseSelector
    {

        [SerializeField]
        private SignalTag Tag;
        [SerializeField]
        private int Count;
        public enum IfType { AllTrue, AllFalse, SomeTrue, SomeFalse }
        [Header("조건")]
        public IfType ifType = IfType.AllTrue;
        //public ConditionCalculate Calculate;
        public List<ConditionCalculate> Calculates = new List<ConditionCalculate>();

        public override bool GetResult()
        {
            if (Calculates.Count == 0)
            {
                Debug.LogWarning($"{Description} 의 조건이 비어있습니다.");
                return false;
            }
            bool result = true;
            //썸트루 썸폴스 일경우 디폴트는 false
            if (ifType == IfType.SomeFalse || ifType == IfType.SomeTrue)
                result = false;
            for (int i = 0; i < Calculates.Count; i++)
            {
                switch (ifType)
                {
                    case IfType.AllTrue:// 하나라도 false 일경우 false
                        if (!Calculates[i].GetResult(Tag, Count))
                            result = false;
                        break;
                    case IfType.AllFalse:// 하나라도 true 일경우 false
                        if (Calculates[i].GetResult(Tag, Count))
                            result = false;
                        break;
                    case IfType.SomeTrue://하나라도 true 면 true
                        if (Calculates[i].GetResult(Tag, Count))
                            result = true;
                        break;
                    case IfType.SomeFalse:// 하나라도 False 면 true
                        if (!Calculates[i].GetResult(Tag, Count))
                            result = true;
                        break;
                    default:
                        break;
                }
            }
            return result; 
        }
    }
}
