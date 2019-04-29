using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct HairNode
{
    public Vector3 prePos;
    public Vector3 curPos;
    public Transform PointTrans;
    public float length;
}

public class HairPoint : MonoBehaviour
{
    public List<HairNode> hairNodes;
    void Start()
    {
        hairNodes = new List<HairNode>();

    }

    void Update()
    {
        
    }
}
