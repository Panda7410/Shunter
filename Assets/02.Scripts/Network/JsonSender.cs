using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace GSSC
{
    public class JsonSender : MonoBehaviour
    {
        public bool TestBool;
        [Header("일단 모듈 생성부분.")]
        public string CreatMoName;
        public string CreatMoType;


        [ContextMenu("샘플테스트.")]
        public string GetJObj()
        {
            JObject @object = new JObject();
            @object.Add("CreatMoName", CreatMoName);
            @object.Add("Sample", 0);

            //Debug.Log(@object.ToString());

            JArray jArray = new JArray();

            for (int i = 0; i < 3; i++)
            {
                JObject @object2 = new JObject();
                @object2.Add("name", $"{i} 번째 이름");
                @object2.Add("value", $"{i} 번째 내용물");
                jArray.Add(@object2);
            }
            @object.Add("어레이",jArray);


            return @object.ToString();


        }
        [ContextMenu("파싱테스트.")]
        public void ParsTest()
        {

            JObject @object = JObject.Parse(GetJObj());

            //@object.

            Debug.Log(@object["CreatMoName"].ToString());
            Debug.Log(@object["Sample"].ToString());

            int B = (int)@object["Sample"];
            Debug.Log(B);
            Debug.Log("통짜로 읽기");
            Debug.Log(@object.ToString());
        }

        public void ParseJson(JObject @object)
        {
            JArray array = JArray.Parse(@object["items"].ToString());
            for (int i = 0; i < array.Count; i++)
            {
                string A = array[i]["name"].ToString();
                string B = array[i]["value"].ToString();

            }

        }
        [ContextMenu("샌드테스트.")]

        public void Sendnet()
        {
            JObject @object = JObject.Parse(GetJObj());
            Client.Instance.SendData("ㅇㅇㅇ", @object);
        }

    }
}