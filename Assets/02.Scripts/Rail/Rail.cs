using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : MonoBehaviour
{
    //필드
    [SerializeField]
    private float RailLenth;
    //[SerializeField]
    //private List<Vector3> positions = new List<Vector3>();
    [SerializeField]
    private string railID;
    [SerializeField]
    public List<Transform> PositionPoint = new List<Transform>();
    [SerializeField]
    private List<RailPosSet> posSets = new List<RailPosSet>();

    //프로퍼티.
    public string RailID { get => railID; private set => railID = value; }
    //public List<Vector3> Positions { get => positions; private set => positions = value; }


    //설정
    public void SetRailID(string railID)
    => RailID = railID;

    #region 인스펙터용
    [ContextMenu("레일포즈 셋")]
    public void SetPosByPositionPoint()
    {
        List<Vector3> PosList = new List<Vector3>();
        for (int i = 0; i < PositionPoint.Count; i++)
            PosList.Add(PositionPoint[i].position);
        SetPositions(PosList);
    }

    #endregion

    public void SetPositions(List<Vector3> Positions)
    {
        //this.Positions = Positions;
        posSets.Clear();//레일 포즈 셋 클리어.
        if (Positions.Count < 2)
        {
            Debug.LogError($"{RailID} 좌표 구성이 잘못되었습니다.");
            return;
        }
        //좌표 부여.
        //총길이 부여.
        float totalLenth = 0;
        for (int i = 0; i < Positions.Count -1; i++)
        {
            //레일 포즈셋 추가.
            posSets.Add(new RailPosSet(Positions[i], Positions[i + 1], totalLenth));
            totalLenth += Vector3.Distance(Positions[i], Positions[i + 1]);
        }
        RailLenth = totalLenth;
        Debug.Log($"{RailID} 설정이 완료되었습니다.");

    }

    /// <summary>
    /// 레일위에 배치 가능여부 판단. 
    /// </summary>
    /// <param name="dist">현재 위치 하여야 하는 거리.</param>
    /// <returns></returns>
    public bool IsPlaced(float dist)
    {
        if (RailLenth < dist || dist < 0)
            return false;
        else
            return true;
    }

    public Vector3 GetPos(float dist)
    {
        RailPosSet posSet = posSets.Find(x => x.isPlaced(dist));
        if (posSet == null)
        {
            Debug.LogError($"{RailID} 거리 설정이 잘못됨.");
            return Vector3.zero;
        }
        return posSet.GetPos(dist);
    }

    private void OnEnable()
    {
        RailManager.Instance.AddRail(this);
    }
    private void OnDisable()
    {
        RailManager.Instance.RemoveRail(this);
    }

}
