using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float rotSpeed;
    public Text countText;
    public Text downText;
    public Text timeText;
    public Text winText;

    private int minute;
    private int second;
    private int millisecond;
    private float timeSpend;
    private int downCount;
    private int count;
    private int win;

    void Start()
    {
        downCount = 0;
        count = 0;
        win = 0;
        timeSpend = 0.0f;
        winText.text = "";
        SetCountText();
    }

    void Update()
    {
        if (win == 0)
        {
            timeSpend += Time.deltaTime;

            minute = ((int)timeSpend) / 60;
            second = (int)timeSpend - minute * 60;
            millisecond = (int)((timeSpend - (int)timeSpend) * 1000);

            timeText.text = string.Format("Time: {0:D2}:{1:D2}.{2:D3}", minute, second, millisecond);
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(Vector3.up * Time.deltaTime * rotSpeed);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(Vector3.up * Time.deltaTime * (-rotSpeed));
        }
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("Menu");
        }
        if ((this.transform.eulerAngles.x > 90 && this.transform.eulerAngles.x < 270) || (this.transform.eulerAngles.z > 90 && this.transform.eulerAngles.z < 270))
        {
            this.transform.eulerAngles = new Vector3(0, 0, 0);
            downCount += 1;
            SetCountText();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
        }
        if (other.gameObject.CompareTag("Terminal"))
        {
            win = 1;
            PlayerPrefs.SetString("time", string.Format("{0:D2}:{1:D2}.{2:D3}", minute, second, millisecond));
            PlayerPrefs.SetString("fd", downCount.ToString());
            PlayerPrefs.SetString("bc", count.ToString());
            SceneManager.LoadScene("Result");
        }
    }

    void SetCountText()
    {
        countText.text = "Box Collection: " + count.ToString();
        downText.text = "Fall Down: " + downCount.ToString();

        //if (win == 1)
        //{
        //
        //   winText.text = "You win!\nYour time is:" + string.Format("{0:D2}:{1:D2}.{2:D3}", minute, second, millisecond);
        //}

    }
}
