using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace GSSC
{
    [CreateAssetMenu(fileName = "TrainDataSet", menuName = "GSSC/DataSet/TrainDataSet")]
    public class TrainDataSet : DataScriptable<TrainDataSet>
    {
        public List<TrainData> trainDatas = new List<TrainData>();

        public TrainModule CreatTainModule(string TrainType)
        {
            TrainData trainData = trainDatas.Find(x => x.TrainType == TrainType);
            if (trainData == null)
                return null;
            GameObject @object = new GameObject($"TrainModule_{TrainType}");
            Instantiate(trainData.Model, @object.transform);
            TrainModule module = @object.AddComponent<TrainModule>();
            module.SetTrainSpec(trainData.HeadFromBogie, trainData.LengthBetweenBogies, trainData.TailFromBogie);
            return module;
        }

        [System.Serializable]
        public class TrainData
        {
            public string TrainType;
            public GameObject Model;
            public float LengthBetweenBogies = 13.800f;
            public float HeadFromBogie = 2.95f;
            public float TailFromBogie = 2.85f;
        }
    }
}
