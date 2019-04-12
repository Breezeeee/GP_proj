using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ResultController : MonoBehaviour
{
    public Text Time;
    public Text FallDown;
    public Text BoxCollection;

    void Start()
    {
        GameObject.Find("RetryBtn").GetComponent<Button>().onClick.AddListener(() => { OnClickRetryBtn(); });
        GameObject.Find("ReturnBtn").GetComponent<Button>().onClick.AddListener(() => { OnClickReturnBtn(); });
        Time.text = "Time: " + PlayerPrefs.GetString("time");
        FallDown.text = "Fall down: " + PlayerPrefs.GetString("fd");
        BoxCollection.text = "Box Collection: " + PlayerPrefs.GetString("bc");
    }

    void OnClickRetryBtn()
    {
        SceneManager.LoadScene("Naive Maze");
    }

    void OnClickReturnBtn()
    {
        SceneManager.LoadScene("Menu");
    }

}