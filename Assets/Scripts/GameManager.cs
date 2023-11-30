using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using GG.Infrastructure.Utils.Swipe;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private SwipeListener swipeListener;
    public TextMeshProUGUI startText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI loseText;
    public TextMeshProUGUI highscoreText;
    public GameObject la;
    public GameObject ra;
    public GameObject ua;
    public GameObject da;
    public GameObject barrier;

    int score = 0;
    bool gameStarted = false;
    string arrowDirection;

    public float SetCountdownTime;
    private float countdownTime;
    private float currentTime;
    public Image countdownCircle;

    private void OnEnable()
    {
        swipeListener.OnSwipe.AddListener(onSwipe);
    }

    private void OnDisable()
    {
        swipeListener.OnSwipe.RemoveListener(onSwipe);
    }

    void Start()
    {
        Invoke("updateHighscore", 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && !gameStarted)
        {
            Vector3 inputpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 barrierpos = barrier.transform.position;
            if (inputpos.y > barrierpos.y) 
            {
                countdownTime = SetCountdownTime;
                startText.gameObject.SetActive(false);
                countdownCircle.gameObject.SetActive(true);
                highscoreText.gameObject.SetActive(false);
                loseText.gameObject.SetActive(false);
                scoreText.text = "Score : 0";
                highscoreText.gameObject.SetActive(true);
                highscoreText.text = "Highscore : " + PlayerPrefs.GetInt("Highscore", 0).ToString();
                gameStarted = true;
                Invoke("SpawnArrow", 0);
            }
        }
        if (!gameStarted) return;
        UpdateCountdown();
    }

    private void onSwipe(string swipe)
    {
        if (arrowDirection.Equals(swipe))
        {
            SpawnArrow();
            score++;
            scoreText.text = "Score : " + score.ToString();

            GameObject arrow = GameObject.FindGameObjectWithTag("Arrow");
            Destroy(arrow);

            checkHighscore();

        }
        else
        {
            Lose();
        }
    }

    

    void SpawnArrow() 
    {
        int ran = Random.Range(1, 3);
        switch (arrowDirection)
        {
            case "Left":
                switch (ran)
                {
                    case 1:
                        Instantiate(ra);
                        arrowDirection = "Right";
                        break;
                    case 2:
                        Instantiate(da);
                        arrowDirection = "Down";
                        break;
                    case 3:
                        Instantiate(ua);
                        arrowDirection = "Up";
                        break;
                }
                break;
            case "Right":
                switch (ran)
                {
                    case 1:
                        Instantiate(la);
                        arrowDirection = "Left";
                        break;
                    case 2:
                        Instantiate(da);
                        arrowDirection = "Down";
                        break;
                    case 3:
                        Instantiate(ua);
                        arrowDirection = "Up";
                        break;
                }
                break;
            case "Down":
                switch (ran)
                {
                    case 1:
                        Instantiate(la);
                        arrowDirection = "Left";
                        break;
                    case 2:
                        Instantiate(ra);
                        arrowDirection = "Right";
                        break;
                    case 4:
                        Instantiate(ua);
                        arrowDirection = "Up";
                        break;
                }
                break;
            case "Up":
                switch (ran)
                {
                    case 1:
                        Instantiate(la);
                        arrowDirection = "Left";
                        break;
                    case 2:
                        Instantiate(ra);
                        arrowDirection = "Right";
                        break;
                    case 3:
                        Instantiate(da);
                        arrowDirection = "Down";
                        break;
                }
                break;
            default:
                int ran4 = Random.Range(1, 4);
                switch (ran4)
                {
                    case 1:
                        Instantiate(la);
                        arrowDirection = "Left";
                        break;
                    case 2:
                        Instantiate(ra);
                        arrowDirection = "Right";
                        break;
                    case 3:
                        Instantiate(da);
                        arrowDirection = "Down";
                        break;
                    case 4:
                        Instantiate(ua);
                        arrowDirection = "Up";
                        break;
                }
                break;
        }
        DecreaseTime();
        ResetCountdown();
        UpdateCountdown();

    }

    void DecreaseTime()
    {
        if (countdownTime - 0.3f >= 0.4f)
        {
            countdownTime = countdownTime - 0.3f;
        }
    }

    void UpdateCountdown()
    {
        currentTime -= Time.deltaTime;
        UpdateCountdownBar();
        Invoke("checkLose", 0.1f);
    }

    void UpdateCountdownBar()
    {
        float fillAmount = currentTime / countdownTime;

        if (countdownCircle != null)
        {
            countdownCircle.fillAmount = fillAmount;
        }
    }

    void ResetCountdown()
    {
        currentTime = countdownTime;
    }

    void checkLose()
    {
        if (currentTime < 0f)
        {
            Lose();
        }
    }

    void Lose()
    {
        GameObject arrow = GameObject.FindGameObjectWithTag("Arrow");
        Destroy(arrow);
        countdownCircle.gameObject.SetActive(false);
        loseText.gameObject.SetActive(true);
        Handheld.Vibrate();
        Invoke("restartGame", 2);
    }

    void checkHighscore()
    {
        if (score > PlayerPrefs.GetInt("Highscore", 0)) 
        {
            PlayerPrefs.SetInt("Highscore", score);
        }
    }

    void updateHighscore()
    {
        scoreText.text = "Highscore : " + PlayerPrefs.GetInt("Highscore", 0).ToString();
    }

    void restartGame()
    {
        SceneManager.LoadScene("Game");
    }
}
