﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairGenerate : MonoBehaviour
{
    public GameObject backhair;
    public List<GameObject> hairSet;
    public int hairSize = 240;

    void Start()
    {
        hairSet = new List<GameObject>();
        for (int idx = 0; idx < hairSize; idx++)
        {
            GameObject hair = Instantiate(backhair) as GameObject;
            hair.transform.SetParent(transform);
            hair.transform.RotateAround(new Vector3(0, hair.transform.position.y, 0), new Vector3(0, -1, 0), 240 / hairSize * (idx + 1));
        }
    }

    void Update()
    {

    }
}
