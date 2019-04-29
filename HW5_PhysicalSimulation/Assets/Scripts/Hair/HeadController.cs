using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadController : MonoBehaviour
{
    private float speed = 5.0f;
    private float rotSpeed = 120.0f;
    private GameObject[] gameObjects;

    float angle;
    Vector3 axis = Vector3.zero;

    void Start()
    {
        gameObjects = GameObject.FindGameObjectsWithTag("HairRoot");
        //Debug.Log(gameObjects.Length);
    }

    void Update()
    {
        if (gameObjects.Length == 1)
        {
            gameObjects = GameObject.FindGameObjectsWithTag("HairRoot");
            Debug.Log(gameObjects.Length);
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * speed * Time.deltaTime;
            for (int i = 0; i < gameObjects.Length; i++)
            {
                gameObjects[i].GetComponent<HairPoint>().rootPos += transform.forward * speed * Time.deltaTime;
            }
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= transform.forward * speed * Time.deltaTime;
            for (int i = 0; i < gameObjects.Length; i++)
            {
                gameObjects[i].GetComponent<HairPoint>().rootPos -= transform.forward * speed * Time.deltaTime;
            }
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= transform.right * speed * Time.deltaTime;
            for (int i = 0; i < gameObjects.Length; i++)
            {
                gameObjects[i].GetComponent<HairPoint>().rootPos -= transform.right * speed * Time.deltaTime;
            }
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += transform.right * speed * Time.deltaTime;
            for (int i = 0; i < gameObjects.Length; i++)
            {
                gameObjects[i].GetComponent<HairPoint>().rootPos += transform.right * speed * Time.deltaTime;
            }
        }
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(Vector3.up * Time.deltaTime * rotSpeed);
            for (int i = 0; i < gameObjects.Length; i++)
            {
                Vector3 center = new Vector3(transform.position.x, gameObjects[i].GetComponent<HairPoint>().rootPos.y, transform.position.z);
                Vector3 point = Quaternion.AngleAxis(rotSpeed * Time.deltaTime, new Vector3(0, 1, 0)) * (gameObjects[i].GetComponent<HairPoint>().rootPos - center);
                gameObjects[i].GetComponent<HairPoint>().rootPos = point + center;
            }

        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(Vector3.up * Time.deltaTime * -rotSpeed);
            for (int i = 0; i < gameObjects.Length; i++)
            {
                Vector3 center = new Vector3(transform.position.x, gameObjects[i].GetComponent<HairPoint>().rootPos.y, transform.position.z);
                Vector3 point = Quaternion.AngleAxis(rotSpeed * Time.deltaTime, new Vector3(0, -1, 0)) * (gameObjects[i].GetComponent<HairPoint>().rootPos - center);
                gameObjects[i].GetComponent<HairPoint>().rootPos = point + center;
            }
        }
    }
}
