using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjSwapInEdit : MonoBehaviour
{
    [Header("교체될 오브젝트들")]
    public List<GameObject> SwapList;

    [Header("설치할 오브젝트")]
    public GameObject ChangeObj;

    [Header("부모가 될 객체")]
    public GameObject MotherObj;

    [Header("체크하면 기존오브젝트 삭제")]
    public bool DestroyBool;


    public void SwapGameObj()
    {
        foreach (GameObject ObjList in SwapList)
        {
            Vector3 위치 = new Vector3();
            Quaternion 각도 = new Quaternion();

            위치 = ObjList.GetComponent<Transform>().position;
            각도 = ObjList.GetComponent<Transform>().rotation;

            GameObject B =

            Instantiate(ChangeObj, 위치, 각도);

            B.transform.SetParent(MotherObj.transform);

            if(DestroyBool)
            DestroyImmediate(ObjList);


            B.transform.SetParent(MotherObj.transform);
        }

    }




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
