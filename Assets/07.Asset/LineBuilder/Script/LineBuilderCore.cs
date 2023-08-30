using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class LineBuilderCore : MonoBehaviour
{
    #region 변수
    [SerializeField]
    SaveSlot saveSlot = SaveSlot.Save1;

    [Header("애니메이션 라인을 넣습니다.")]
    public Animation SelecLine;
    [Header("애니메이션 될 객체를 넣습니다.")]
    public GameObject MainCar;
    [Header("부모객체")]
    public GameObject RotObj;
    [Header("생성할 프리팹")]
    public GameObject CreatObj;
    [Header("기존오브젝트 삭제")]
    public bool DestroyObj = false;
    [Header("=======================")]
    [Space]
    [Header("시작거리")]
    public float StartDist;
    [Header("끝거리")]
    public float EndDist;
    [Header("생성간격")]
    public float CreatInterver;
    
    [Header("옵셋 값")]
    public Vector3 Positon;
    [Header("추가 회전값")]
    [SerializeField]
    RotType RotateType = RotType.일반;
    public Vector3 Rotation;
    [Header("스케일값")]
    public Vector3 Scale = new Vector3(1,1,1);

    //public string saveName = "DefName";

    [Space]
    [Header("======================")]
    [Header("이하 라인정보")]
    [Space]
    [SerializeField]
    private float 라인길이;
   

    private List<GameObject> ObjList = new List<GameObject>();

    enum SaveSlot {Save1, Save2, Save3, Save4, Save5 }
    enum RotType {일반, 캔트제거, Y업}

    #endregion

#if UNITY_EDITOR
    [MenuItem("LineBuilder/MakeMaster")]
#endif
    static void CreatMaster()
    {
        Debug.Log("Builder");

        //if (GameObject.Find("Build_Master")) {
        //    //Debug.Log("이미 생성된 Buider 객체가 존재합니다.");
            
        //    if(!GameObject.Find("Build_Master").GetComponent< LineBuilderCore>())
        //    GameObject.Find("Build_Master").AddComponent<LineBuilderCore>();
        //    return;
        //}
            

        GameObject gameObject = new GameObject("Creat Build_Master");

        gameObject.AddComponent<LineBuilderCore>();

    }
#if UNITY_EDITOR
    [MenuItem("LineBuilder/ObjSwap")]
#endif
    static void CreatSwap()
    {
        Debug.Log("Builder_ObjSwap");

        //if (GameObject.Find("Build_Master"))
        //{
        //    //Debug.Log("이미 생성된 Buider 객체가 존재합니다.");
        //    if (!GameObject.Find("Build_Master").GetComponent<ObjSwapInEdit>())
        //        GameObject.Find("Build_Master").AddComponent<ObjSwapInEdit>();
        //    return;
        //}


        GameObject gameObject = new GameObject("Creat ObjSwap Obj");

        gameObject.AddComponent<ObjSwapInEdit>();

    }






    #region MyRegion

    private void SetDefault()
    {
        라인길이 = LengthToMeter(SelecLine["Take 001"].length);

        StartCoroutine(LinePointInstall(라인길이));

        gameObject.AddComponent<LineBuilderCamControl>().SetCamControl(SelecLine, MainCar);


    }

    void SetViewCam()
    {
        if (!SelecLine) return;

        GameObject LineAnime = Instantiate(SelecLine.gameObject);

    }

    IEnumerator LinePointInstall(float LineMeter)
    {
        yield return null;

        if (!SelecLine) yield break;

        Animation LineAnime = Instantiate(SelecLine.gameObject).GetComponent<Animation>();
        GameObject minCar = LineAnime.transform.Find(MainCar.name).gameObject;
        GameObject MeterPointMother = new GameObject("PointMother");
        int CreatCount = (int)LineMeter / 10;
        float NowMeter = 0;


        LineAnime.Play();
        LineAnime["Take 001"].speed = 0;

        GameObject DefPoint = null;
        for (int i = 0; i <= CreatCount; i++)
        {
            LineAnime["Take 001"].time = MeterToLenght(NowMeter);
            yield return null;
            yield return null;
            GameObject meterObj = new GameObject((i * 10).ToString() + "M");
            meterObj.transform.position = minCar.transform.position;
            meterObj.transform.SetParent(MeterPointMother.transform);
            meterObj.AddComponent<LineGizmoPoint>().SetNextPoint(DefPoint);

            DefPoint = meterObj;
            
            
            NowMeter = (i+1) * 10f;
        }

        Destroy(LineAnime.gameObject);

    }

    public void SetOBJ()
    {
        StartCoroutine(SetObj());
    }

    IEnumerator SetObj()
    {
        //항목체크
        if (SelecLine == null || MainCar == null || RotObj == null || CreatObj == null)
        {
            Debug.LogError("누락된 필수항목이 있습니다.");

            yield break;
        }
        //생성 횟수 계산.
        if(EndDist - StartDist < 0 || CreatInterver <= 0)
        {
            Debug.LogError("설정된 거리 값에 오류가 존재합니다.");
            yield break;
        }
        ///항목체크 끝////////

        float Dist = EndDist - StartDist;
        int produceCount = (int)System.Math.Truncate(Dist / CreatInterver);

        Debug.Log("생성횟수"+ produceCount);

        SelecLine.Play();
        SelecLine["Take 001"].speed = 0f;
        //SelecLine["Take 001"].time =;
        if (DestroyObj)
        {
            foreach (GameObject gameObject in ObjList)
            {
                Destroy(gameObject);
            }
        }
        

        ObjList.Clear();
        


        for (int i = 0; i < produceCount; i++)
        {
            
            float CreatDist = StartDist + (CreatInterver * i);

            SelecLine["Take 001"].time = MeterToLenght(CreatDist);

            yield return null;
            yield return null;

            GameObject CreatObj = Instantiate(this.CreatObj);
            CreatObj.transform.position = MainCar.transform.position;
            CreatObj.transform.eulerAngles =
                RotateType == RotType.일반
                ? MainCar.transform.eulerAngles
                : RotateType == RotType.캔트제거
                ? new Vector3(MainCar.transform.eulerAngles.x, MainCar.transform.eulerAngles.y, 0)
                : new Vector3(0, MainCar.transform.eulerAngles.y, 0);
            CreatObj.transform.Translate(Positon, Space.Self);
            CreatObj.transform.Rotate(Rotation, Space.Self);
            CreatObj.transform.localScale = Scale;
            CreatObj.transform.SetParent(RotObj.transform);

            ObjList.Add(CreatObj);//리스트에 오브젝트를 추가한다.

        }



        yield return null;

    }



    public void SaveObjData()
    {
        for(int i = 0; i < ObjList.Count; i ++)
        {
            string KeyName = gameObject.GetInstanceID().ToString() +  saveSlot.ToString() + i;

            string ObjValue = ObjList[i].transform.position.x.ToString() + ":"
                + ObjList[i].transform.position.y.ToString() + ":"
                + ObjList[i].transform.position.z.ToString() + ":"
                
                + ObjList[i].transform.rotation.x.ToString() + ":"
                + ObjList[i].transform.rotation.y.ToString() + ":"
                + ObjList[i].transform.rotation.z.ToString() + ":"
                + ObjList[i].transform.rotation.w.ToString() + ":"

                + ObjList[i].transform.localScale.x.ToString() + ":"
                + ObjList[i].transform.localScale.y.ToString() + ":"
                + ObjList[i].transform.localScale.z.ToString();
           
            PlayerPrefs.SetString(KeyName, ObjValue);
        }

        PlayerPrefs.SetString(gameObject.GetInstanceID().ToString() + saveSlot.ToString(), ObjList[0].name + ":" + ObjList.Count);
        Debug.Log(saveSlot + "슬롯에 세이브를 실시했습니다.");

    }
    public void LoadObjData()
    {
        string[] ObjParam = PlayerPrefs.GetString(gameObject.GetInstanceID().ToString() + saveSlot.ToString()).Split(':');
        string KeyName = gameObject.GetInstanceID().ToString() + saveSlot.ToString();
        int ObjCount = int.Parse(ObjParam[1]);

        for(int i = 0; i < ObjCount; i++)
        {
            //string[] ObjParamTemp = PlayerPrefs.GetString(KeyName + i).Split(' ');
            string[] ObjValue = PlayerPrefs.GetString(KeyName + i).Split(':');

            Vector3 ObjPos = new Vector3(float.Parse(ObjValue[0]), float.Parse(ObjValue[1]), float.Parse(ObjValue[2]));
            Quaternion ObjRot = new Quaternion(float.Parse(ObjValue[3]), float.Parse(ObjValue[4]), float.Parse(ObjValue[5]), float.Parse(ObjValue[6]));
            Vector3 ObjScale = new Vector3(float.Parse(ObjValue[7]), float.Parse(ObjValue[8]), float.Parse(ObjValue[9]));

            GameObject TartgetObj = Instantiate(CreatObj, ObjPos, ObjRot, RotObj.transform);
            TartgetObj.transform.localScale = ObjScale;

        }

        Debug.Log(saveSlot + "슬롯 로드를 실시했습니다.");

    }




    #endregion


    #region 단위 환산 

    /// <summary>
    /// 애니메이션 렌스를 실거리 미터로 환산한다.
    /// </summary>
    /// <param name="length">애니메이션의 렌스</param>
    /// <returns></returns>
    public float LengthToMeter(float length)
    {
        return length / 0.036f;
    }

    /// <summary>
    /// 실거리 미터를 애니메이션 렌스로 환산한다.
    /// </summary>
    /// <param name="meter">실제 미터</param>
    /// <returns></returns>
    public float MeterToLenght(float meter)
    {
        return meter * 0.036f;
    }

    /// <summary>
    /// 속도를 넣으면 애니메이션 재생 속도로 변환하여 리턴
    /// </summary>
    /// <param name="KmSpeed">Km/H</param>
    /// <returns></returns>
    public float KmSpeedToAniSpeed(float KmSpeed)
    {
        return KmSpeed / 100.0f;
    }

    /// <summary>
    /// 애니메이션 재생속도를 넣으면 Km/H로 환산하여 리턴
    /// </summary>
    /// <param name="AniSpeed">애니메이션 재생속도</param>
    /// <returns></returns>
    public float AniSpeedToKmSpeed(float AniSpeed)
    {
        return AniSpeed * 100f;
    }



    #endregion


    // Start is called before the first frame update
    void Start()
    {
        SetDefault();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
