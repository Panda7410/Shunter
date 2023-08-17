using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RailShower : MonoBehaviour
{
    public Rail rail;
    // Start is called before the first frame update
    [ContextMenu("Line")]

    void Start()
    {
        var render = GetComponent<LineRenderer>();


        Vector3[] vector3s = new Vector3[rail.PositionPoint.Count];
        render.positionCount = vector3s.Length;
        for (int i = 0; i < rail.PositionPoint.Count; i++)
        {
            vector3s[i] = rail.PositionPoint[i].position;
            //render.SetPosition(i, rail.PositionPoint[i].position);
        }
        render.SetPositions(vector3s);
    }
}
