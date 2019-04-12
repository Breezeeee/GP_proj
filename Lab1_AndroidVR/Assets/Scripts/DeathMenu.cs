using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//// UI controller for DeathMenu
public class DeathMenu : MonoBehaviour
{
    public Text endScoreText;
    public Text highScoreText;
    public Image bgImage;
    private bool isShowned;

    private float bgTransition;

    void Start()
    {
        //// hide the Death Menu at first
        gameObject.SetActive(false);
        isShowned = false;
        bgTransition = 0.0f;
    }
    void Update()
    {
        if(!isShowned)
            return;

        
        //// fading effect
        bgTransition += Time.deltaTime;
        bgImage.color = Color.Lerp(new Color(0,0,0,0), Color.black, bgTransition);
    }

    public void Show(float score){
        //// TODO: Your Implementation:
        //// - show Death Menu
        //// - show score information
        ///

        gameObject.SetActive(true);
        isShowned = true;

        endScoreText.text = "End Score: " + ((int)score).ToString();
        highScoreText.text = "High Score: " + ((int)PlayerPrefs.GetFloat("highScore")).ToString();
    }
}