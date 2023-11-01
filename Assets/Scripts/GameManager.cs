using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set;  }

    private const float COIN_SCORE_AMOUNT = 5f;

    public bool isDead { set; get; }
    private bool isGameStarted = false;
    private PlayerMotor motor;


    public Animator gameCanvas;
    public TextMeshProUGUI scoreText, coinText, modifierText, highscoreText;
    private float score, coinScore, modifierScore;
    private int lastScore = 0;

    public Animator deathMenuAnim;
    public TextMeshProUGUI deadScoreText, deadCoinText;

    private void Awake()
    {
        modifierScore = 1;
        Instance = this;
        motor = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMotor>();

        scoreText.text = score.ToString("0");
        coinText.text = coinScore.ToString("0");
        modifierText.text = "x" + modifierScore.ToString("0.0");

        highscoreText.text = "Highscore: " + PlayerPrefs.GetInt("Highscore").ToString();
    }

    private void Update()
    {
        if (MobileInput.Instance.Tap && !isGameStarted)
        {
            isGameStarted = true;
            motor.StartRunning();
            FindObjectOfType<GlacierSpawner>().IsScrolling = true;
            FindObjectOfType<CameraMotor>().IsMoving = true;
            gameCanvas.SetTrigger("Show");
        }

        if (isGameStarted && !isDead)
        {
            //Bump the score up
            score += Time.deltaTime * modifierScore;
            if (lastScore != score)
            {
                lastScore = (int)score;
                scoreText.text = score.ToString("0");
            }
        }
    }

    public void GetCoin()
    {
        coinScore += 1f;
        score += COIN_SCORE_AMOUNT;
        coinText.text = coinScore.ToString("0");
        scoreText.text = scoreText.text = score.ToString("0");
    }

    public void UpdateModifier(float modAmount)
    {
        modifierScore = 1f + modAmount;
        modifierText.text = "x" + modifierScore.ToString("0.0");
    }

    public void OnPlayButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
    }

    public void OnDeath()
    {
        isDead = true;
        FindObjectOfType<GlacierSpawner>().IsScrolling = false;
        deadScoreText.text = score.ToString("0");
        deadCoinText.text = coinScore.ToString("0");
        deathMenuAnim.SetTrigger("Dead");
        gameCanvas.SetTrigger("Hide");

        //Check Highscore
        if(score > PlayerPrefs.GetInt("Highscore"))
        {
            float s = score;
            if (s % 1 == 0)
                s += 1;

            PlayerPrefs.SetInt("Highscore", (int)s);
        }
    }
}
