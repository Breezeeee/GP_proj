using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MenuController: MonoBehaviour
{
    void Start()
    {
        GameObject.Find("StartBtn").GetComponent<Button>().onClick.AddListener(() => { OnClickStartBtn(); });
        GameObject.Find("CloseBtn").GetComponent<Button>().onClick.AddListener(() => { OnCloseApp(); });
    }

    void OnClickStartBtn()
    {
        SceneManager.LoadScene("Naive Maze");
    }

    void OnCloseApp()
    {
        Application.Quit();
    }

}