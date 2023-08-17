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
            TrainDataParser dataParser = FindObjectOfType<TrainDataParser>();

            dataParser.Print(TrainDataParser.ItemNameInstallVehicle , dataParser.InstallVehicle());

            dataParser.Print(TrainDataParser.ItemNameMoveVehicle, dataParser.MoveVehicle());

            dataParser.Print(TrainDataParser.ItemNameDestroyVehicle, dataParser.DestroyVehicle());
            dataParser.Print(TrainDataParser.ItemNameInstallTrain, dataParser.InstallTrain());
            dataParser.Print(TrainDataParser.ItemNameAddVehicleFront, dataParser.AddCarFront());
            dataParser.Print(TrainDataParser.ItemNameAddVehicleBack, dataParser.AddCarBack());
            dataParser.Print(TrainDataParser.ItemNameMoveTrain, dataParser.MoveTrain());
            dataParser.Print(TrainDataParser.ItemNameRemoveVehicle, dataParser.RemoveVehicle());
            dataParser.Print(TrainDataParser.ItemNameSetTrainSpeed, dataParser.SetTrainSpeed());
            dataParser.Print(TrainDataParser.ItemNameDestroyTrain, dataParser.DestryTrain());
            //dataParser.Print(TrainDataParser.  , dataParser.   );
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}