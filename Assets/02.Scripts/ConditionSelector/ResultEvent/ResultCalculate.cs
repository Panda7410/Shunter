using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GSSC.Condition
{
    public abstract class ResultCalculate : ScriptableObject
    {
        public string description;
        public abstract int SetValue(int original, int valueToCalculate);
    }
}