using Common;
using GSSC;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainDataParser : MonoBehaviour
{



    //구분용 분류 아이템이름.===========
    //차량설치
    public const string ItemNameInstallVehicle = "차량설치";
    //열차설치
    public const string ItemNameInstallTrain = "열차설치";
    //차량이동
    public const string ItemNameMoveVehicle = "차량이동";
    //열차이동
    public const string ItemNameMoveTrain = "열차이동";
    //차량삭제
    public const string ItemNameDestroyVehicle = "차량삭제";
    //열차삭제
    public const string ItemNameDestroyTrain = "열차삭제";
    //추가편성앞
    public const string ItemNameAddVehicleFront = "추가편성앞";
    //추가편성뒤
    public const string ItemNameAddVehicleBack = "추가편성뒤";
    //추가편성 해제
    public const string ItemNameRemoveVehicle = "편성해제";
    //열차 속력제어
    public const string ItemNameSetTrainSpeed = "속력제어";


    //공용.
    //거리
    public const string ItemNameDist = "세팅거리";
    //레일 이름
    public const string ItemNameRailID = "레일ID";
    //차량 타입
    public const string ItemNameVehicleType = "차량타입";
    //차량 ID
    public const string ItemNameVehicleID = "차량ID";
    //차량 방향
    public const string ItemNameVehicleDirection = "차량방향";
    //열차 ID
    public const string ItemNameTrainID = "열차ID";
    //추가차량 앞
    public const string ItemNameAddFrontVehicleList = "추가차량리스트";
    //추가차량 뒤
    //public const string ItemNameAddBackVehicleList = "";
    //열차 속력
    public const string ItemNameTrainSpeed = "세팅속력";


    //모든열차거리
    public const string ItemNameGetAllModuleDist = "모든열차위치";

    public const string ItemNameGetModuleDist = "열차위치";

    //public const string ItemNameDist = "";
    //public const string ItemNameDist = "";





    // Start is called before the first frame update
    void OnEnable()
    {
        {// 어드바이스
            var AdviseList = Client.Instance.AdviseList;
            AdviseList.Add(ItemNameInstallVehicle);
            AdviseList.Add(ItemNameInstallTrain);
            AdviseList.Add(ItemNameMoveVehicle);
            AdviseList.Add(ItemNameDestroyVehicle);
            AdviseList.Add(ItemNameAddVehicleFront);
            AdviseList.Add(ItemNameAddVehicleBack);
            AdviseList.Add(ItemNameRemoveVehicle);
            AdviseList.Add(ItemNameSetTrainSpeed);
        }
        ////모듈 설치 시.
        ActionManager.Instance.AddAction(OrderKind.SET.ToString(), ItemNameInstallVehicle, ObjectAction: ParsInstallVehicle);
        //차량이동
        ActionManager.Instance.AddAction(OrderKind.SET.ToString(), ItemNameMoveVehicle, ObjectAction: ParsMoveVehicle);
        //차량 삭제
        ActionManager.Instance.AddAction(OrderKind.SET.ToString(), ItemNameDestroyVehicle, ObjectAction: ParsDestroyVehicle);
        //열차 설치
        ActionManager.Instance.AddAction(OrderKind.SET.ToString(), ItemNameInstallTrain, ObjectAction: ParsInstallTrain);
        //차량 앞 추가
        ActionManager.Instance.AddAction(OrderKind.SET.ToString(), ItemNameAddVehicleFront, ObjectAction: ParsAddCarFront);
        //차량 뒤 추가
        ActionManager.Instance.AddAction(OrderKind.SET.ToString(), ItemNameAddVehicleBack, ObjectAction: ParsAddCarBack);
        //열차이동
        ActionManager.Instance.AddAction(OrderKind.SET.ToString(), ItemNameMoveTrain, ObjectAction: ParsMoveTrain);
        //추가편성해제
        ActionManager.Instance.AddAction(OrderKind.SET.ToString(), ItemNameRemoveVehicle, ObjectAction: ParsRemoveVehicle);
        //열차 속도 할당
        ActionManager.Instance.AddAction(OrderKind.SET.ToString(), ItemNameSetTrainSpeed, ObjectAction: ParsSetTrainSpeed);
        //열차 삭제
        ActionManager.Instance.AddAction(OrderKind.SET.ToString(), ItemNameDestroyTrain, ObjectAction: ParsDestryTrain);



        //열차위치 반환
        ActionManager.Instance.AddAction(OrderKind.SET.ToString(), ItemNameGetAllModuleDist, ObjectAction: SendAllVehiclePos);
        //ItemNameGetModuleDist 열차 단일위치 반환
        ActionManager.Instance.AddAction(OrderKind.SET.ToString(), ItemNameGetModuleDist, ObjectAction: SendVehiclePos);

    }
    void OnDisable()
    {
        ////모듈 설치 시.
        ActionManager.Instance.RemoveAction(OrderKind.SET.ToString(), ItemNameInstallVehicle, ObjectAction: ParsInstallVehicle);
        //차량이동
        ActionManager.Instance.RemoveAction(OrderKind.SET.ToString(), ItemNameMoveVehicle, ObjectAction: ParsMoveVehicle);
        //차량 삭제             
        ActionManager.Instance.RemoveAction(OrderKind.SET.ToString(), ItemNameDestroyVehicle, ObjectAction: ParsDestroyVehicle);
        //열차 설치             
        ActionManager.Instance.RemoveAction(OrderKind.SET.ToString(), ItemNameInstallTrain, ObjectAction: ParsInstallTrain);
        //차량 앞 추가          
        ActionManager.Instance.RemoveAction(OrderKind.SET.ToString(), ItemNameAddVehicleFront, ObjectAction: ParsAddCarFront);
        //차량 뒤 추가          
        ActionManager.Instance.RemoveAction(OrderKind.SET.ToString(), ItemNameAddVehicleBack, ObjectAction: ParsAddCarBack);
        //열차이동              
        ActionManager.Instance.RemoveAction(OrderKind.SET.ToString(), ItemNameMoveTrain, ObjectAction: ParsMoveTrain);
        //추가편성해제          
        ActionManager.Instance.RemoveAction(OrderKind.SET.ToString(), ItemNameRemoveVehicle, ObjectAction: ParsRemoveVehicle);
        //열차 속도 할당        
        ActionManager.Instance.RemoveAction(OrderKind.SET.ToString(), ItemNameSetTrainSpeed, ObjectAction: ParsSetTrainSpeed);
        //열차 삭제             
        ActionManager.Instance.RemoveAction(OrderKind.SET.ToString(), ItemNameDestroyTrain, ObjectAction: ParsDestryTrain);
    }


    #region 샌드 예시-----------
    public JObject InstallVehicle() //설치 시. 
    {
        //ItemNameInstallVehicle
        JObject @object = new JObject();
        @object.Add(ItemNameVehicleType, "value(string)");
        @object.Add(ItemNameVehicleID, "value(string)");
        @object.Add(ItemNameRailID, "value(string)");
        @object.Add(ItemNameDist, "value(float)");
        @object.Add(ItemNameVehicleDirection, "value(int)");

        //리턴
        return @object;
    }

    public JObject MoveVehicle() //차량이동
    {
        //ItemNameMoveVehicle
        JObject @object = new JObject();
        @object.Add(ItemNameVehicleID, "value(string)");
        @object.Add(ItemNameRailID, "value(string)");
        @object.Add(ItemNameDist, "value(float)");

        //리턴
        return @object;
    }

    public JObject DestroyVehicle() // 차량삭제
    {
        //ItemNameDestroyVehicle
        JObject @object = new JObject();
        @object.Add(ItemNameVehicleID, "value(string)");

        //리턴
        return @object;
    }
    
    public JObject InstallTrain()
    {
        //ItemNameInstallTrain
        JObject @object = new JObject();
        @object.Add(ItemNameTrainID, "value(string)");
        @object.Add(ItemNameVehicleID, "value(string)");
        @object.Add(ItemNameRailID, "value(string)");
        @object.Add(ItemNameDist, "value(float)");
        //리턴
        return @object;
    }

    public JObject AddCarFront()
    {
        //ItemNameAddVehicleFront
        JObject @object = new JObject();
        @object.Add(ItemNameTrainID, "value(string)");

        JArray jArray = new JArray();

        for (int i = 0; i < 3; i++)
        {
            JObject @object2 = new JObject();
            @object2.Add(ItemNameVehicleID, $"value(string) {i} 번");
            jArray.Add(@object2);
        }


        @object.Add(ItemNameAddFrontVehicleList, jArray);
        //리턴
        return @object;
    }

    public JObject AddCarBack()
    {
        //ItemNameAddVehicleBack
        JObject @object = new JObject();
        @object.Add(ItemNameTrainID, "value(string)");

        JArray jArray = new JArray();

        for (int i = 0; i < 3; i++)
        {
            JObject @object2 = new JObject();
            @object2.Add(ItemNameVehicleID, $"value(string) {i} 번");
            jArray.Add(@object2);
        }


        @object.Add(ItemNameAddFrontVehicleList, jArray);
        //리턴
        return @object;
    }

    public JObject MoveTrain()
    {
        //ItemNameMoveTrain
        JObject @object = new JObject();
        @object.Add(ItemNameTrainID, "value(string)");
        @object.Add(ItemNameRailID, "value(string)");
        @object.Add(ItemNameDist, "value(float)");
        //리턴
        return @object;
    }

    public JObject RemoveVehicle() //추가편성해제
    {
        //ItemNameRemoveVehicle
        JObject @object = new JObject();
        @object.Add(ItemNameTrainID, "value(string)");
        @object.Add(ItemNameVehicleID, "value(string)");
        //리턴
        return @object;

    }

    public JObject SetTrainSpeed()
    {
        //ItemNameSetTrainSpeed
        JObject @object = new JObject();
        @object.Add(ItemNameTrainID, "value(string)");
        @object.Add(ItemNameTrainSpeed, "value(float)");
        //리턴
        return @object;
    }

    public JObject DestryTrain()
    {
        //ItemNameDestroyTrain
        JObject @object = new JObject();
        @object.Add(ItemNameTrainID, "value(string)");
        //리턴
        return @object;
    }
    #endregion

    #region 열차관련데이터 파싱 && 실행

    public void ParsInstallVehicle(object value) //설치 시. 
    {
        //ItemNameInstallVehicle
        JObject @object = (JObject)value;

        TrainManager.Instance.CreateModule(
            @object[ItemNameVehicleType].ToString(),
            @object[ItemNameVehicleID].ToString(),
            @object[ItemNameRailID].ToString(),
            @object[ItemNameDist].Value<float>(),
            @object[ItemNameVehicleDirection].Value<int>()
            );
    }

    public void ParsMoveVehicle(object value) //차량이동
    {
        //ItemNameMoveVehicle
        JObject @object = (JObject)value;

        TrainManager.Instance.SetModulePos(
            @object[ItemNameVehicleID].ToString(),
            @object[ItemNameRailID].ToString(),
            @object[ItemNameDist].Value<float>()
            );
    }

    public void ParsDestroyVehicle(object value) // 차량삭제
    {
        //ItemNameDestroyVehicle
        JObject @object = (JObject)value;

        TrainManager.Instance.DestroyModule(
            @object[ItemNameVehicleID].ToString()
            );
    }


    public void ParsInstallTrain(object value) // 열차 설치
    {
        //ItemNameInstallTrain
        JObject @object = (JObject)value;
        //생성
        TrainManager.Instance.CreatTrain(
            @object[ItemNameTrainID].ToString(),
            @object[ItemNameVehicleID].ToString()
            );
        //위치 변경.
        TrainManager.Instance.SetTrainPos(
            @object[ItemNameTrainID].ToString(),
            @object[ItemNameRailID].ToString(),
            @object[ItemNameDist].Value<float>()
            );

        //@object.Add(ItemNameTrainID, "value(string)");
        //@object.Add(ItemNameVehicleID, "value(string)");
        //@object.Add(ItemNameRailID, "value(string)");
        //@object.Add(ItemNameDist, "value(float)");

    }

    public void ParsAddCarFront(object value)
    {
        //ItemNameAddVehicleFront
        JObject @object = (JObject)value;

        List<TrainModule> modules = new List<TrainModule>();

        //ItemNameAddFrontVehicleList
        JArray jArray = (JArray)@object[ItemNameAddFrontVehicleList];
        for (int i = 0; i < jArray.Count; i++)
        {
            //어레이에서 J오브젝트 추출.
            JObject @object2 = jArray[i].Value<JObject>();
            //J오브젝트 에서 모듈 네임 추출
            modules.Add(TrainManager.Instance.GetModoule(@object2[ItemNameVehicleID].ToString()));
        }

        TrainManager.Instance.AttachFrontModule(@object[ItemNameTrainID].ToString(), modules);




        //@object.Add(ItemNameTrainID, "value(string)");

        //for (int i = 0; i < 3; i++)
        //{
        //    JObject @object2 = new JObject();
        //    @object2.Add(ItemNameVehicleID, $"value(string) {i} 번");
        //    jArray.Add(@object2);
        //}

        //@object.Add(ItemNameAddFrontVehicleList, jArray);

    }

    public void ParsAddCarBack(object value)
    {
        //ItemNameAddVehicleBack


        JObject @object = (JObject)value;

        List<TrainModule> modules = new List<TrainModule>();

        JArray jArray = (JArray)@object[ItemNameAddFrontVehicleList];
        for (int i = 0; i < jArray.Count; i++)
        {
            //어레이에서 J오브젝트 추출.
            JObject @object2 = jArray[i].Value<JObject>();
            //J오브젝트 에서 모듈 네임 추출
            modules.Add(TrainManager.Instance.GetModoule(@object2[ItemNameVehicleID].ToString()));
        }

        TrainManager.Instance.AttachBackModule(@object[ItemNameTrainID].ToString(), modules);


        //JObject @object = (JObject)value;
        //@object.Add(ItemNameTrainID, "value(string)");

        //JArray jArray = new JArray();
        //{
        //    JObject @object2 = new JObject();
        //    @object2.Add(ItemNameVehicleID, "value(string) 0 번");
        //    @object2.Add(ItemNameVehicleID, "value(string) 1 번");
        //    @object2.Add(ItemNameVehicleID, "value(string) 2 번");
        //}

        //@object.Add(ItemNameAddBackVehicleList, jArray);

    }

    public void ParsMoveTrain(object value)
    {
        //ItemNameMoveTrain
        JObject @object = (JObject)value;

        //위치 변경.
        TrainManager.Instance.SetTrainPos(
            @object[ItemNameTrainID].ToString(),
            @object[ItemNameRailID].ToString(),
            @object[ItemNameDist].Value<float>()
            );
    }

    public void ParsRemoveVehicle(object value) //추가편성해제
    {
        //ItemNameRemoveVehicle
        JObject @object = (JObject)value;

        TrainManager.Instance.BreakTrainCoupling(@object[ItemNameTrainID].ToString(), @object[ItemNameVehicleID].ToString());

        //@object.Add(ItemNameVehicleID, "value(string)");

    }

    public void ParsSetTrainSpeed(object value) // 스피드 할당.
    {
        //ItemNameSetTrainSpeed
        JObject @object = (JObject)value;
        TrainManager.Instance.SetTrainSpeed(@object[ItemNameTrainID].ToString(), @object[ItemNameTrainSpeed].Value<float>());

        //@object.Add(ItemNameTrainID, "value(string)");
        //@object.Add(ItemNameTrainSpeed, "value(float)");

    }

    public void ParsDestryTrain(object value)
    {
        //ItemNameDestroyTrain
        JObject @object = (JObject)value;
        TrainManager.Instance.DestroyTrain(@object[ItemNameTrainID].ToString());

        //@object.Add(ItemNameTrainID, "value(string)");

    }

    #endregion

    [ContextMenu("비클s포즈 샌드")]
    public void SendAllVehiclePos(object value)
    {
        //ItemNameGetAllModuleDist
        { //비클 포지션
            //DataModel dataEvent = new DataModel()
            //{
            //    Order = OrderKind.SET
            //};

            JObject @object = new JObject();
            

            JArray jArray = new JArray();

            (string ModuleId, string railId, float dist)[] ps = TrainManager.Instance.getAllTrainPos();


            for (int i = 0; i < ps.Length; i++)
            {
                //jArray
                JObject @object2 = new JObject();
                @object2.Add(ItemNameVehicleID, ps[i].ModuleId);
                @object2.Add(ItemNameRailID, ps[i].railId);
                @object2.Add(ItemNameDist, ps[i].dist);

                jArray.Add(@object2);
            }


            @object.Add(ItemNameGetModuleDist, jArray);


            //dataEvent.Items.Add(new Item()
            //{
            //    Name = ItemNameGetAllModuleDist,
            //    Value = @object
            //});

            Debug.Log(@object);
            Client.Instance.SendData(ItemNameGetAllModuleDist, @object);
        }
    }
    [ContextMenu("비클포즈 샌드")]
    public void SendVehiclePos(object value)
    {
        string ID = value.ToString();
        (string ModuleId, string railId, float dist) ps = TrainManager.Instance.getTrainPos(ID);
        JObject @object = new JObject();
        @object.Add(ItemNameVehicleID, ps.ModuleId);
        @object.Add(ItemNameRailID, ps.railId);
        @object.Add(ItemNameDist, ps.dist);
        Debug.Log(@object);
        Client.Instance.SendData(ItemNameGetModuleDist, @object);

    }

    [ContextMenu("파싱테스트 01")]
    public void ParsTest()
    {
        {
            DataModel dataEvent = new DataModel()
            {
                Order = OrderKind.SET
            };

            JObject @object = new JObject();
            @object.Add(ItemNameVehicleType, "기관차");
            @object.Add(ItemNameVehicleID, "value(string)");
            @object.Add(ItemNameRailID, "인상선-Line1");
            @object.Add(ItemNameDist, 50f);
            @object.Add(ItemNameVehicleDirection, 0);

            dataEvent.Items.Add(new Item()
            {
                Name = ItemNameInstallVehicle,
                Value = @object
            });

            Client.Instance.EnqueDataModelOutSide(dataEvent);
        }
        { //열차설치
            DataModel dataEvent = new DataModel()
            {
                Order = OrderKind.SET
            };

            JObject @object = new JObject();
            @object.Add(ItemNameTrainID, "value(string)");
            @object.Add(ItemNameVehicleID, "value(string)");
            @object.Add(ItemNameRailID, "인상선-Line1");
            @object.Add(ItemNameDist, 50f);

            dataEvent.Items.Add(new Item()
            {
                Name = ItemNameInstallTrain,
                Value = @object
            });

            Client.Instance.EnqueDataModelOutSide(dataEvent);
        }
        { //앞에 추가 설치
            DataModel dataEvent = new DataModel()
            {
                Order = OrderKind.SET
            };

            JObject @object = new JObject();
            //ItemNameAddVehicleFront
            @object.Add(ItemNameTrainID, "value(string)");

            JArray jArray = new JArray();

            for (int i = 0; i < 3; i++)
            {
                JObject @object2 = new JObject();
                @object2.Add(ItemNameVehicleID, $"{i+1}");
                jArray.Add(@object2);
            }


            @object.Add(ItemNameAddFrontVehicleList, jArray);

            dataEvent.Items.Add(new Item()
            {
                Name = ItemNameAddVehicleFront,
                Value = @object
            });

            Client.Instance.EnqueDataModelOutSide(dataEvent);
        }
    }

    [ContextMenu("파싱테스트 스피드")]

    public void parsTest2()
    {
        { //앞에 추가 설치
            DataModel dataEvent = new DataModel()
            {
                Order = OrderKind.SET
            };

            JObject @object = new JObject();
            //ItemNameAddVehicleFront
            @object.Add(ItemNameTrainID, "value(string)");
            @object.Add(ItemNameTrainSpeed, 20);

            

            dataEvent.Items.Add(new Item()
            {
                Name = ItemNameSetTrainSpeed,
                Value = @object
            });

            Client.Instance.EnqueDataModelOutSide(dataEvent);
        }
    }


    public void Print(string ItemName, JObject @object)
    {
        Debug.Log("ItemName : " + ItemName);
        Debug.Log(@object);
    }
}
