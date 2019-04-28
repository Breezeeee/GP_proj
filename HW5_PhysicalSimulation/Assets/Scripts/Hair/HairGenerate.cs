using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairGenerate : MonoBehaviour
{
    public GameObject prefab;
    public List<GameObject> hairSet;
    public int hairSize = 200;

    void Start()
    {
        hairSet = new List<GameObject>();
        for (int idx = 0; idx < hairSize; idx++)
        {
            GameObject go = Instantiate(prefab) as GameObject;
            go.transform.SetParent(transform);
            go.transform.RotateAround(new Vector3(0, go.transform.position.y, 0), new Vector3(0, 1, 0), 240 / hairSize * (idx + 1));
        }
    }

    void Update()
    {
        
    }
}
