using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : UnitySingleton<GameManager>
{
    public GameObject player;
    public Text scoreText;

    private bool isDead;
    private float score = 0.0f;

    public DeathMenu deathMenu;

    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        isDead = false;
    }

    void Update()
    {
        if (!isDead) {
            //// TODO: Your Implementation:
            //// 1. update score (Hint: you can use running time as the score)
            //// 2. show score (Hint: show in Canvas/CurrentScore/Text)
            score += Time.deltaTime;
            scoreText.text = "Score: " + ((int)score).ToString();
        }
        else if (Input.touchCount > 0 || Input.GetKeyDown(KeyCode.Mouse0)) {
            Restart();
        }
    }

    public bool IsDead() {
        return isDead;
    }

    public void OnDeath(bool collision){
        isDead = true;
        print("GameOver");
        //// TODO: Your Implementation:
        //// 1. show DeathMenu (Hint: you can use Show() in DeathMenu.cs)
        //// 2. stop player
        //// 3. hide all tiles (Hint: call function in TileManager.cs)
        //// 4. record high score (Hint: use PlayerPrefs)
        ///
        
        float highScore = PlayerPrefs.GetFloat("highScore");
        highScore = score > highScore ? score : highScore;
        PlayerPrefs.SetFloat("highScore", highScore);

        deathMenu.Show(score);

    }

    public void Restart(){
        score = 0.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
