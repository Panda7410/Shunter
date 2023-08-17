using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GSSC.Condition
{
    [CreateAssetMenu(fileName = "ResultCalculateEqual", menuName = "GSSC/ResultCalculate/ResultCalculateEqual")]

    public class ResultCalculateEqual : ResultCalculate
    {
        public override int SetValue(int original, int valueToCalculate)
        {
            return valueToCalculate;
        }
    }
}