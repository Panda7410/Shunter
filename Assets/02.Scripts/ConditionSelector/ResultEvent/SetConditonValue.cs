namespace GSSC.Condition
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using GSSC.Signal;

    [AddComponentMenu("GSSC/변수값부여")] 
    public class SetConditonValue : MonoBehaviour
    {
        [Header("변경할 태그")]
        public SignalTag Tag;
        [Header("메모")]
        private string Description;
        [Header("적용할 계산식")]
        public ResultCalculate Calculate;

        [Header("변경해줄값")]
        public int Value;
        [ContextMenu("call")]
        public void Call()
        {
            //여기부터 작성할것. 데이터에 수치 넣는중. 어.. 이거 겟 리절트할수있어?
            //컨티뉴 불능으로 변경함. ㅇㅇ.
            Call(Value);
        }
        public void Call(int value)
        {
            if (Calculate == null)
            {
                Debug.LogWarning($"{Tag.Group} {Tag.Tag} 의 계산값이 존재하지 않습니다.");
                return;
            }

            ConditionDatas.ConditionData data = ConditionDatas.Instance.GetConditionData(Tag);
            int BF = data.Count;
            //계산
            data.Count = Calculate.SetValue(data.Count, value);
            Debug.Log($"{data.Tag.Group}:{data.Tag.Tag} 값변동 {BF} => {data.Count}  {Description}");

            ConditionManager.Instance.Call();
            //수치가 변경될때마다 검사. 이렇게 하고싶지는 않은데 조금 복잡해져서 그냥 처리함.
            //자주 변경되는 부분은 아니니까. ...
        }
    }

}