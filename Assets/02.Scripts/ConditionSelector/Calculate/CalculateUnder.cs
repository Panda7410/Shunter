namespace GSSC.Condition
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using GSSC.Signal;
    [CreateAssetMenu(fileName = "Calculate_Under", menuName = "GSSC/Condition/Calculate_Under")]
    public class CalculateUnder : ConditionCalculate
    {
        
        public override bool GetResult(SignalTag SelectorTag, int Count)
        {
            ConditionDatas.ConditionData data = ConditionDatas.Instance.GetConditionData(SelectorTag);
            if (data.Count < Count)
                return true;
            else
                return false;
        }
    }
}
