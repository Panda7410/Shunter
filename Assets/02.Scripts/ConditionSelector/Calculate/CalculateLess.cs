namespace GSSC.Condition
{
using System.Collections;
using System.Collections.Generic;
using GSSC.Signal;
using UnityEngine;
    [CreateAssetMenu(fileName = "Calculate_OrLess", menuName = "GSSC/Condition/Calculate_OrLess")]

    public class CalculateLess : ConditionCalculate
    {
        public override bool GetResult(SignalTag SelectorTag, int Count)
        {
            ConditionDatas.ConditionData data = ConditionDatas.Instance.GetConditionData(SelectorTag);
            if (data.Count <= Count)
                return true;
            else
                return false;
        }
    }
}
