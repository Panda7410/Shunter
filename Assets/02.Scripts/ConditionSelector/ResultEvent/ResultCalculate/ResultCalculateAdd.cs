using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GSSC.Condition
{
    [CreateAssetMenu(fileName = "ResultCalculateAdd", menuName = "GSSC/ResultCalculate/ResultCalculateAdd")]

    public class ResultCalculateAdd : ResultCalculate
    {
        public override int SetValue(int original, int valueToCalculate)
        {
            return original + valueToCalculate;
        }
    }
}