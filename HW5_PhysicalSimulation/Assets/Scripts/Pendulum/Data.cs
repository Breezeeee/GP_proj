using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Data : MonoBehaviour
{
    public float g = 9.8f;
    private StreamWriter sw1, sw2;
    public Transform EulerSphere;
    public Transform MidpointSphere;
    public Transform TrapzoidSphere;
    public Transform Top;

    private float l;
    private float theta1, theta2, theta3;
    private float omega1, omega2, omega3;

    void Start()
    {
        l = Vector3.Distance(Top.position, EulerSphere.position);
        FileStream angleFile = new FileStream(@"D:/Proj/UnityProj/PhysicsAndHair/Data/Angle.txt", FileMode.Create);
        sw1 = new StreamWriter(angleFile);

        FileStream energyFile = new FileStream(@"D:/Proj/UnityProj/PhysicsAndHair/Data/Energy.txt", FileMode.Create);
        sw2 = new StreamWriter(energyFile);

        sw1.WriteLine("Time\tEuler\tMidpoint\tTrapzoid");
        sw2.WriteLine("Time\tEuler\tMidpoint\tTrapzoid");
    }

    void Update()
    {
        theta1 = Vector3.Angle(EulerSphere.position - Top.position, new Vector3(0, -1, 0)) * Mathf.Deg2Rad;
        theta2 = Vector3.Angle(MidpointSphere.position - Top.position, new Vector3(0, -1, 0)) * Mathf.Deg2Rad;
        theta3 = Vector3.Angle(TrapzoidSphere.position - Top.position, new Vector3(0, -1, 0)) * Mathf.Deg2Rad;
        theta1 = Top.position.x > EulerSphere.position.x ? -theta1 : theta1;
        theta2 = Top.position.x > MidpointSphere.position.x ? -theta2 : theta2;
        theta3 = Top.position.x > TrapzoidSphere.position.x ? -theta3 : theta3;

        omega1 = EulerSphere.GetComponent<EulerController>().omega;
        omega2 = MidpointSphere.GetComponent<MidpointController>().omega;
        omega3 = TrapzoidSphere.GetComponent<TrapzoidController>().omega;
        float energy1 = Mathf.Pow(omega1 * l, 2) / 2 + g * l * (1 - Mathf.Cos(theta1));
        float energy2 = Mathf.Pow(omega2 * l, 2) / 2 + g * l * (1 - Mathf.Cos(theta2));
        float energy3 = Mathf.Pow(omega3 * l, 2) / 2 + g * l * (1 - Mathf.Cos(theta3));

        float time = Time.time;
        sw1.WriteLine(time.ToString("f2") + "\t" + 
            theta1.ToString("f2") + "\t" +
            theta2.ToString("f2") + "\t" + 
            theta3.ToString("f2"));

        sw2.WriteLine(time.ToString("f2") + "\t" +
            energy1.ToString("f2") + "\t" +
            energy2.ToString("f2") + "\t" +
            energy3.ToString("f2"));
    }
}
