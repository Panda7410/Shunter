namespace GSSC.Condition
{
    using GSSC.Signal;
    using UnityEngine;

    //[CreateAssetMenu(fileName = "Calculate", menuName = "GSSC/Condition/Calculate")]

    public abstract class ConditionCalculate : ScriptableObject
    {
        public string description;
        public abstract bool GetResult(SignalTag SelectorTag, int Count);

    }
}