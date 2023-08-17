using UnityEngine;

[System.Serializable]
public class RailPosSet
{

    [SerializeField]
    private Vector3 Pos1;
    [SerializeField]
    private Vector3 Pos2;
    [SerializeField]
    private float StartDist;
    [SerializeField]
    private float EndDist;
    [SerializeField]
    private float LineLenth;

    public RailPosSet(Vector3 pos1, Vector3 pos2, float StartDist)
    {
        Pos1 = pos1;
        Pos2 = pos2;
        this.StartDist = StartDist;
        LineLenth = Vector3.Distance(pos1, pos2);
        EndDist = this.StartDist + LineLenth;
    }

    public bool isPlaced(float dist)
    {
        if (StartDist <= dist && dist <= EndDist)
            return true;
        else
            return false;
    }

    public Vector3 GetPos(float dist)
    {
        float magnification = 1 - (EndDist - dist) / LineLenth;
        return Vector3.Lerp(Pos1, Pos2, magnification);
    }
}

