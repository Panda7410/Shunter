using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace testerrr
{
    public class testerrrr : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            
            //dataParser.Print(TrainDataParser.  , dataParser.   );
        }


        [ContextMenu("아이템 값 로그")]
        void SETTEST()
        {
            TrainDataParser dataParser = FindObjectOfType<TrainDataParser>();

            dataParser.Print(TrainDataParser.ItemNameInstallVehicle, dataParser.InstallVehicle());

            dataParser.Print(TrainDataParser.ItemNameMoveVehicle, dataParser.MoveVehicle());

            dataParser.Print(TrainDataParser.ItemNameDestroyVehicle, dataParser.DestroyVehicle());
            dataParser.Print(TrainDataParser.ItemNameInstallTrain, dataParser.InstallTrain());
            dataParser.Print(TrainDataParser.ItemNameAddVehicleFront, dataParser.AddCarFront());
            dataParser.Print(TrainDataParser.ItemNameAddVehicleBack, dataParser.AddCarBack());
            dataParser.Print(TrainDataParser.ItemNameMoveTrain, dataParser.MoveTrain());
            dataParser.Print(TrainDataParser.ItemNameRemoveVehicle, dataParser.RemoveVehicle());
            dataParser.Print(TrainDataParser.ItemNameSetTrainSpeed, dataParser.SetTrainSpeed());
            dataParser.Print(TrainDataParser.ItemNameDestroyTrain, dataParser.DestryTrain());
        }
    }

}