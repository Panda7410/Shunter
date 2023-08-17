using System.Collections;
using System.Collections.Generic;
using UnityEngine;
class RailManager : SimpleSingleton<RailManager>
{
    [SerializeField]
    //private List<Rail> AllRails = new List<Rail>();
    public Dictionary<string, Rail> AllRails = new Dictionary<string, Rail>();


    public void AddRail(Rail rail)
    {
        if(AllRails.ContainsKey(rail.RailID))
        {
            Debug.LogError($"{rail.RailID}와 동일한 RailID를 지닌 선로가 이미 존재합니다.");
            return;
        }
        else if (AllRails.ContainsValue(rail))
        {
            Debug.LogError($"{rail.RailID}가 이미 존재합니다.");
            return;
        }
        AllRails.Add(rail.RailID, rail);
    }
    public void RemoveRail(string RailID)
    {
        if (AllRails.ContainsKey(RailID))
            AllRails.Remove(RailID);
    }
    public void RemoveRail(Rail RailID)
    {
        if (AllRails.ContainsValue(RailID))
            AllRails.Remove(RailID.RailID);
    }


}

