using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineGizmoPoint : MonoBehaviour
{
    GameObject NextPoint;
    public float MeterThis = 0;

    public void SetNextPoint(GameObject nextPoint)
    { NextPoint = nextPoint; }

    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        if (NextPoint)
            Gizmos.DrawLine(transform.position, NextPoint.transform.position);

    }

}
