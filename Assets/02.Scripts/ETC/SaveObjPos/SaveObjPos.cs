using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GSSC
{
    public class SaveObjPos : MonoBehaviour
    {
        [TextArea]
        public string Discription;
        public GameObject MainOBJ;
        [SerializeField]
        public List<SaveObjPosItemData> saveObjPosItemDatas = new List<SaveObjPosItemData>();


        public void SetAllObj()
        {
            if(MainOBJ == null)
            {
                Debug.LogError("메인객체가 존재하지 않습니다.");
                return;
            }
            saveObjPosItemDatas.Clear();
            Transform[] trs = MainOBJ.GetComponentsInChildren<Transform>();
            for (int i = 1; i < trs.Length; i++)
            {
                SaveObjPosItemData data = new SaveObjPosItemData()
                {
                    TargetObj = trs[i].gameObject,
                    //TargetPos = trs[i].localPosition,
                    //TargetRot = trs[i].localRotation,
                    //TargetActive = trs[i].gameObject.activeSelf
                };
                saveObjPosItemDatas.Add(data);
            }

            Debug.Log("세팅 완료");
        }

        public void SavePos()
        {
            for (int i = 0; i < saveObjPosItemDatas.Count; i++)
            {
                saveObjPosItemDatas[i].Save();
            }
            Debug.Log("세이브가 완료되었습니다.");
        }

        public void LoadPos()
        {
            for (int i = 0; i < saveObjPosItemDatas.Count; i++)
            {
                saveObjPosItemDatas[i].Load();
            }
            Debug.Log("로드가 완료되었습니다.");
        }




        [System.Serializable]
        public class SaveObjPosItemData
        {
            public GameObject TargetObj;
            public Vector3 TargetPos;
            public Quaternion TargetRot;
            public bool TargetActive;

            public void Save()
            {
                if (TargetObj == null)
                {
                    Debug.LogWarning("대상객체가 비어있습니다.");
                    return;
                }
                TargetPos = TargetObj.transform.localPosition;
                TargetRot = TargetObj.transform.rotation;
                TargetActive = TargetObj.activeSelf;
            }

            public void Load()
            {
                if (TargetObj == null)
                {
                    Debug.LogWarning("대상객체가 비어있습니다.");
                    return;
                }
                TargetObj.transform.localPosition = TargetPos;
                TargetObj.transform.rotation = TargetRot;
                TargetObj.SetActive(TargetActive);

            }
        }
    }
}
