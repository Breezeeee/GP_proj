using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleController : MonoBehaviour
{

    public Transform head;

    [Range(0, 10)]
    public float maxMovingSpeed = 3.0f;

    [HideInInspector]
    public float movingSpeed = 0.0f;

    private Vector3 _head_right;
    private Vector3 _head_right_on_xy;
    private Vector3 _head_right_on_xz;

    private float _speed1, _speed2;

    public static float SignedAngle(Vector3 from, Vector3 to, Vector3 axis)
    {
        float angle = Vector3.Angle(from, to);
        int sign = Vector3.Dot(Vector3.Cross(from, to), axis) > 0 ? 1 : -1;
        return sign * angle;
    }

    public static float Angle2Speed(float angle, float maxMovingSpeed)
    {
        angle = angle > 45f ? 45f : angle;
        angle = angle < -45f ? -45f : angle;
        return angle / 45f * maxMovingSpeed;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _head_right = head.right;
        _head_right_on_xy = _head_right;
        _head_right_on_xy.z = 0;
        _head_right_on_xz = _head_right;
        _head_right_on_xz.y = 0;

        _speed1 = Angle2Speed(SignedAngle(transform.right, _head_right_on_xz, transform.up), maxMovingSpeed);
        _speed2 = Angle2Speed(SignedAngle(transform.right, _head_right_on_xy, -transform.forward), maxMovingSpeed);
        movingSpeed = _speed1 + _speed2;
        movingSpeed = movingSpeed > maxMovingSpeed ? maxMovingSpeed : movingSpeed;
        movingSpeed = movingSpeed < -maxMovingSpeed ? -maxMovingSpeed : movingSpeed;
    }
}
