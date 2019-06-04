// Dependencies
using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    // References to User Interface Elements which have been customised in the Unity Editor
    public GameManager gm; // Game Manager
    public GameObject missionBar; // Current mission bar
    public GameObject resultsMenu; // The results page
    public GameObject pauseMenu; // The entire pause menu
    public TextMeshProUGUI enemiesKilled; // Text element which displays how many enemies were killed 
    public TextMeshProUGUI timeTaken; // Text element which displays how much time was taken
    public TextMeshProUGUI todayDate; // Text element which displays today's date
    public TextMeshProUGUI finalScore; // Text element which displays the user's final score i.e. the level they reached / the maximum no. of levels
    public TextMeshProUGUI livesLeft;
    public TextMeshProUGUI currentSceneText;
    public GameObject playButton;
    public GameObject menuUI;
    public float bannerAnimSpeed;
    public float bannerAnimWaitTime;

    void Awake()
    {
        DontDestroyOnLoad(this);
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
        if (gm == null)
        {
            gm = FindObjectOfType<GameManager>();
        }
    }

    private void LateUpdate()
    {
        livesLeft.text = "x " + gm.playerLivesLeft.ToString();
        currentSceneText.text = gm.currentScene == 0 ? "Menu" : "Level " + gm.currentScene.ToString();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += EveryLevel;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= EveryLevel;
    }

    private void EveryLevel(Scene scene, LoadSceneMode mode)
    {
        GetComponent<Canvas>().worldCamera = GameObject.FindGameObjectWithTag("UI Camera").GetComponent<Camera>();
    }

    public void BackToMenu(bool animResults)
    {
        gm.ResumeGame(); //if not already resumed
        gm.LoadMenu(animResults);
        menuUI.SetActive(true);
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ResetResults(bool animateResults)
    {
        gm.enemiesKilled = 0;
        timeTaken.text = "";
        if(animateResults)
            StartCoroutine(ResetResultsAnim());
    }

    IEnumerator ResetResultsAnim()
    {
        float speed = bannerAnimSpeed;
        float animatePercent = 0;

        while (animatePercent <= 1)
        {
            animatePercent += Time.deltaTime * speed;
            resultsMenu.GetComponent<RectTransform>().anchoredPosition = Vector2.up * Mathf.Lerp(540, -375, animatePercent);
            yield return null;
        }
    }
    
    public void StartGame()
    {
        gm.PlayStartTime = Time.time;
        gm.NextLevel();
        menuUI.SetActive(false);
    }
    public void AnimateNewLevelBanner()
    {
        StopCoroutine(BannerAnim());
        missionBar.GetComponentInChildren<Text>().text = "Level " + (SceneManager.GetActiveScene().buildIndex).ToString();
        StartCoroutine(BannerAnim());
    }

    IEnumerator BannerAnim()
    {
        float delayTime = bannerAnimWaitTime;
        float speed = bannerAnimSpeed;
        float animatePercent = 0;
        int dir = 1;

        float endDelayTime = Time.time + 1 / speed + delayTime;

        while (animatePercent >= 0)
        {
            animatePercent += Time.deltaTime * speed * dir;

            if (animatePercent >= 1)
            {
                animatePercent = 1;
                if (Time.time > endDelayTime)
                {
                    dir = -1;
                }
            }

            missionBar.GetComponent<RectTransform>().anchoredPosition = Vector2.up * Mathf.Lerp(350, 540, animatePercent);
            yield return null;
        }

    }

    public void AnimateResults()
    {
        float timeTaken = Time.time - gm.PlayStartTime;
        StopCoroutine(ResultsAnim());
        enemiesKilled.text = "Enemies Killed: " + gm.enemiesKilled.ToString();
        float minutesDec = (timeTaken / 60);
        int min = Mathf.RoundToInt(minutesDec);
        int sec = (int)(Mathf.Abs(minutesDec - (float)min) * 60);
        this.timeTaken.text = "Time Taken: " + min.ToString() + "m " + sec.ToString() + "s";
        todayDate.text = "Today's Date: " + System.DateTime.UtcNow.ToString("dd/MM/yy");
        finalScore.text = gm.currentScene.ToString() + " / " + gm.highestScene.ToString();
        StartCoroutine(ResultsAnim());
    }

    IEnumerator ResultsAnim()
    {
        float speed = bannerAnimSpeed;
        float animatePercent = 0;

        while (animatePercent <= 1)
        {
            animatePercent += Time.deltaTime * speed;
            resultsMenu.GetComponent<RectTransform>().anchoredPosition = Vector2.up * Mathf.Lerp(-375, 540, animatePercent);
            yield return null;
        }
    }

    public void StartButtonPointerHover()
    {
        playButton.GetComponent<Image>().color = Color.green;
        AudioSource playButtonAudioSource = playButton.GetComponent<AudioSource>();
        playButtonAudioSource.PlayOneShot(playButtonAudioSource.clip);
        playButton.transform.localScale *= 1.2f;
    }

    public void StartButtonPointerExit()
    {
        playButton.GetComponent<Image>().color = Color.white;
        playButton.transform.localScale /= 1.2f;
    }

    public void ShowPauseMenu()
    {
        pauseMenu.SetActive(true);
    }

    public void HidePauseMenu()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }
}