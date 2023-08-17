namespace GSSC.Condition
{
    using System.Collections;
    using System.Collections.Generic;
    using GSSC.Signal;
    using UnityEngine;
    [CreateAssetMenu(fileName = "ConditionList_", menuName = "GSSC/ConditionSelectorList")]

    public class ConditionListSelector : ConditionBaseSelector
    {
        static int loofCount = 0;
        public enum IfType { AllTrue, AllFalse, SomeTrue, SomeFalse }
        public IfType ifType = IfType.AllTrue;
        [Header("조건리스트")]
        public List<ConditionBaseSelector> Selectors = new List<ConditionBaseSelector>();

        public override bool GetResult()
        {
            if (Selectors.Count == 0)
                return false;
            if (Selectors.Contains(this))
            {
                Debug.LogError($"조건리스트엔 자신을 포함할수 없습니다. 구성을 확인해주세요.");
                return false;
            }
            if(loofCount > 1000)
            {
                // 무한루프 방지구문. 복합구문일경우 이래도 스택오버 플로우가 발생할수있다.. 주의할것. 
                Debug.LogError($"{Description} 연산 스택오버플로우가 발생했습니다. 구성을 확인해주세요.");
                return false;
            }
            loofCount++;

            bool result = true;
            //썸트루 썸폴스 일경우 디폴트는 false
            if (ifType == IfType.SomeFalse || ifType == IfType.SomeTrue)
                result = false;

            for (int i = 0; i < Selectors.Count; i++)
            {
                switch (ifType)
                {
                    case IfType.AllTrue: // 하나라도 false 일경우 false
                        if (!Selectors[i].GetResult())
                            result = false;
                        break;
                    case IfType.AllFalse: // 하나라도 true 일경우 false
                        if (Selectors[i].GetResult())
                            result = false;
                        break;
                    case IfType.SomeTrue: //하나라도 true 면 true
                        if (Selectors[i].GetResult())
                            result = true;
                        break;
                    case IfType.SomeFalse: // 하나라도 False 면 true
                        if (!Selectors[i].GetResult())
                            result = true;
                        break;
                    default:
                        break;
                }
            }


            //돌아 올수 있으면 (연산 1000회 미만) 루프카운트 초기화
            loofCount = 0;
            return result;
        }
    }
}