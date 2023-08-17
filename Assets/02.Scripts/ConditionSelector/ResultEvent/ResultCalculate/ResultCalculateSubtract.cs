using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GSSC.Condition
{
    [CreateAssetMenu(fileName = "ResultCalculateSubtract", menuName = "GSSC/ResultCalculate/ResultCalculateSubtract")]

    public class ResultCalculateSubtract : ResultCalculate
    {

        public override int SetValue(int original, int valueToCalculate)
        {
            return original - valueToCalculate;
        }
    }
}