using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour//, IPointerEnterHandler, IPointerExitHandler
{
    public GameManager gm;
    public GameObject missionBar;
    public GameObject resultsMenu;
    public GameObject pauseMenu;
    public TextMeshProUGUI enemiesKilled;
    public TextMeshProUGUI timeTaken;
    public TextMeshProUGUI todayDate;
    public TextMeshProUGUI finalScore;
    public GameObject playButton;
    public GameObject menuUI;
    public GameObject xBar;
    public GameObject yBar;
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

    public void AnimateBars()
    {
        //first you need the RectTransform component of your canvas
        RectTransform CanvasRect = GetComponent<Canvas>().GetComponent<RectTransform>();

        //then you calculate the position of the UI element
        //0,0 for the canvas is at the center of the screen, whereas WorldToViewPortPoint treats the lower left corner as 0,0. Because of this, you need to subtract the height / width of the canvas * 0.5 to get the correct position.

        Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(FindObjectOfType<Player>().transform.position);
        Vector2 WorldObject_ScreenPosition = new Vector2(
        ((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
        ((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));

        StopCoroutine(AnimateBarsCoroutine(WorldObject_ScreenPosition));
        StartCoroutine(AnimateBarsCoroutine(WorldObject_ScreenPosition));
    }

    IEnumerator AnimateBarsCoroutine(Vector3 playerPos)
    {
        float delayTime = bannerAnimWaitTime;
        float speed = bannerAnimSpeed/2;
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

            xBar.GetComponent<RectTransform>().anchoredPosition = Vector2.right * Mathf.Lerp(0, playerPos.x, animatePercent);
            yBar.GetComponent<RectTransform>().anchoredPosition = Vector2.up * Mathf.Lerp(0, playerPos.y, animatePercent);
            yield return null;
        }
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
        //missionBar.GetComponentInChildren<Text>().text = "Level " + (SceneManager.GetActiveScene().buildIndex).ToString();
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