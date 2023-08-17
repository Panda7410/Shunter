namespace GSSC.Condition
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using GSSC.Signal;
    using UnityEngine;

    public abstract class ConditionBaseSelector : ScriptableObject
    {
        public Action<bool> ResultAction;
        [TextArea]
        public string Description;
        public abstract bool GetResult();
    }
}