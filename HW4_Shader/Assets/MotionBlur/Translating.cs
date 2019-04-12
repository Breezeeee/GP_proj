using UnityEngine;
using System.Collections;

public class Translating : MonoBehaviour {

	public float Speed = 20.0f;
    public float StartX = 0.0f;
    public float EndX = 0.0f;

    private float movespeed, speed;
    private float startX, endX;
    private bool moveStatus;

	void Start () {
        startX = StartX > EndX ? StartX : EndX;
        endX = StartX > EndX ? EndX : StartX;
        transform.position = new Vector3(startX, 20.0f, -50.0f);
        moveStatus = false;
        movespeed = -Speed;
        speed = 0.0f;
	}
	
	void Update () {
        transform.Translate(Time.deltaTime * speed, 0, 0, Space.Self);


        if (Input.GetKeyDown(KeyCode.Space))
        {
            moveStatus = moveStatus ? false : true;
        }
        if (moveStatus)
        {
            if (transform.position.x > startX)
                movespeed = -Speed;
            else if (transform.position.x < endX)
                movespeed = Speed;
            speed = movespeed;
        }
        else
        {
            speed = 0.0f;
        }
    }
}
