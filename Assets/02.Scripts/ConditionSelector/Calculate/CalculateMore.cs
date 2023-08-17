namespace GSSC.Condition
{
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GSSC.Signal;
//using UnityEngine;
    [CreateAssetMenu(fileName = "Calculate_OrMore", menuName = "GSSC/Condition/Calculate_OrMore")]

    public class CalculateMore : ConditionCalculate
    {
        public override bool GetResult(SignalTag SelectorTag, int Count)
        {
            ConditionDatas.ConditionData data = ConditionDatas.Instance.GetConditionData(SelectorTag);
            if (data.Count >= Count)
                return true;
            else
                return false;
        }
    }
}
