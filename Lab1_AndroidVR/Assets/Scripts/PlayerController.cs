using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    public KeyCode keyRight;
    public KeyCode keyLeft;

    //// current velocity
    private float horizontalVel = 0.0f;
    private float forwardVel = 3.0f;
    private float maxSpeed = 9.0f;

    private float score = 0.0f;

    private AngleController angleController;


    void Start()
    {
        angleController = GetComponent<AngleController>();
    }

    void Update()
    {
        //// control with keyboard
        if (Input.GetKey(keyLeft))
            horizontalVel = -3.0f;
        else if (Input.GetKey(keyRight))
            horizontalVel = +3.0f;
        else
            horizontalVel = 0.0f;
        //// TODO: Your Implementation:
        //// - Update the horizontal velocity with angleController
        ///

        horizontalVel = angleController.movingSpeed;

        if (Time.time > 10)
        {
            forwardVel += (float)((Time.time - 10) * 0.001);
            forwardVel = forwardVel > maxSpeed ? maxSpeed : forwardVel;
        }

        //// When not dead, update velocity
        if (!GameManager.Instance.IsDead()) {
            
            this.transform.GetComponent<Rigidbody>().velocity = new Vector3(horizontalVel, 0.0f, forwardVel);
        }
        else
        {
            this.transform.GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
            TileManager.Instance.hideAll();
        }
    }
    
    void OnTriggerEnter(Collider other) {
        //// TODO: Your Implementation:
        //// - When collide with obj with tag 'CollisionWall' or 'FallWall', trigger OnDeath() in GameManager
        if (other.gameObject.CompareTag("CollisionWall"))
        {
            GameManager.Instance.OnDeath(true);
        }
        if (other.gameObject.CompareTag("FallWall"))
        {
            GameManager.Instance.OnDeath(false);
        }
    }
}
