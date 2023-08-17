namespace GSSC.Condition
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using GSSC.Signal;
    [CreateAssetMenu(fileName = "Calculate_Over", menuName = "GSSC/Condition/Calculate_Over")]
    public class CalculateOver : ConditionCalculate
    {
        
        public override bool GetResult(SignalTag SelectorTag, int Count)
        {
            ConditionDatas.ConditionData data = ConditionDatas.Instance.GetConditionData(SelectorTag);
            if (data.Count > Count)
                return true;
            else
                return false;
        }
    }
}