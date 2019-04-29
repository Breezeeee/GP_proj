﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidpointController : MonoBehaviour
{
    public float g = 9.8f;
    private float l;
    private float theta;
    [System.NonSerialized] public float omega;
    private float transAngle;

    private Transform Top;
    private Transform Sphere;

    void Start()
    {
        Top = GameObject.FindGameObjectWithTag("Top").GetComponent<Transform>();
        Sphere = this.GetComponent<Transform>();

        l = Vector3.Distance(Top.position, Sphere.position);
        theta = Vector3.Angle(Sphere.position - Top.position, new Vector3(0, -1, 0));
        transAngle = 0.0f;
        omega = 0.0f;
    }

    void Update()
    {
        Sphere.RotateAround(Top.position, new Vector3(0, 0, -1), transAngle);
        theta = Vector3.Angle(Sphere.position - Top.position, new Vector3(0, -1, 0));

        float deltaOmega = g / l * Mathf.Sin(Mathf.Deg2Rad * theta) * Time.deltaTime;
        float tmpOmega = omega;
        if (Top.position.x > Sphere.position.x)
        {
            omega = omega - deltaOmega;
        }
        else
        {
            omega = omega + deltaOmega;
        }
        if (omega > 0)
        {
            transAngle = Mathf.Sqrt((Mathf.Pow(omega, 2) + Mathf.Pow(tmpOmega, 2)) / 2);
        }
        else
        {
            transAngle = -Mathf.Sqrt((Mathf.Pow(omega, 2) + Mathf.Pow(tmpOmega, 2)) / 2);
        }
    }
}
