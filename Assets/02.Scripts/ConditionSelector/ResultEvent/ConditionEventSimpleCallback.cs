namespace GSSC.Condition
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;
    using GSSC.Signal;
    using System;
    [AddComponentMenu("GSSC/콜백이벤트심플")]
    [RequireComponent(typeof(ConditionListComponet))]
    public class ConditionEventSimpleCallback : ConditionEventCallbackBase
    {
        [TextArea]
        public string Description;
        [Header("자동등록여부")]
        [Tooltip("활성화 된 프레임의 마지막에 등록됩니다.")]
        public bool AutoRegister = false;
        [SerializeField]
        [Header("완료여부")]
        private bool isCompleted = false; // 완료여부
        //[Header("체크시 완료 후에도 재실행 가능.")] //< 생각보다 무리네. 
        //[SerializeField]
        //private bool isCanContinuityAcive = false;
        //[Header("조건리스트")]
        //[SerializeField]
        ConditionListComponet conditionList;
        [Header("완료시 실행 이벤트")]
        public UnityEvent Event;
        public Action action;

        private void Awake()
        {
            conditionList = GetComponent<ConditionListComponet>();
        }

        public void OnEnable()
        {
            if (AutoRegister)
                StartCoroutine(RegiLater());
        }

        IEnumerator RegiLater()
        {
            yield return new WaitForEndOfFrame();
            RegEv();
        }

        public void ResetComplete()
        => isCompleted = false;

        public override void RegEv()
        {
            Debug.Log($"{Description} 초기화, 재등록");
            ResetComplete();
            ConditionManager.Instance.RegiCallback(this);
        }
        private void OnDisable()
        {
            ConditionManager.Instance.RemoveCallback(this);
            //Event = null;
            //action = null;
        }
        public override void Call()
        {
            if (isCompleted)
                return;
            if (!conditionList.GetResult())
                return;
            Event?.Invoke();
            action?.Invoke();
            Debug.Log($"{Description} 완료");
            isCompleted = true;
        }
    }

}
