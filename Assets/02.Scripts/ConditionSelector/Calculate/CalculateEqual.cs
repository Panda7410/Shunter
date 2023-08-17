using System.Collections;
using System.Collections.Generic;
using GSSC.Signal;
using UnityEngine;
namespace GSSC.Condition
{

    [CreateAssetMenu(fileName = "Calculate_Equal", menuName = "GSSC/Condition/Calculate_Equal")]
    public class CalculateEqual : ConditionCalculate
    {
        public override bool GetResult(SignalTag SelectorTag, int Count)
        {
            ConditionDatas.ConditionData data = ConditionDatas.Instance.GetConditionData(SelectorTag);
            if (data.Count == Count)
                return true;
            else
                return false;
        }
    }
}
