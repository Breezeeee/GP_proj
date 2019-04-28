using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidpointController : MonoBehaviour
{
    public float g = 9.8f;
    public float mass = 0.01f;
    private float l;
    private float theta;
    private float omega;

    private Transform Top;
    private Transform Sphere;

    void Start()
    {
        Top = GameObject.FindGameObjectWithTag("MidpointTop").GetComponent<Transform>();
        Sphere = this.GetComponent<Transform>();

        l = Vector3.Distance(Top.position, Sphere.position);
        theta = Vector3.Angle(Sphere.position - Top.position, new Vector3(0, -1, 0));
        omega = 0.0f;
    }

    void Update()
    {
        Sphere.RotateAround(Top.position, new Vector3(0, 0, -1), omega);
        theta = Vector3.Angle(Sphere.position - Top.position, new Vector3(0, -1, 0));
        float deltaOmega = g / l * Mathf.Sin(Mathf.Deg2Rad * theta) * mass;
        float tmpOmega;
        if (Top.position.x > Sphere.position.x)
        {
            tmpOmega = omega - deltaOmega;

        }
        else
        {
            tmpOmega = omega + deltaOmega;
        }
        
    }
}
