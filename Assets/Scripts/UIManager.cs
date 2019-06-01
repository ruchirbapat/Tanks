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

    public void BackToMenu()
    {
        gm.LoadMenu();
        menuUI.SetActive(true);
    }

    public void ResetResults()
    {
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
        Vector3 playerPos = Camera.main.WorldToViewportPoint(FindObjectOfType<Player>().transform.position);
        StopCoroutine(AnimateBarsCoroutine(playerPos));
        StartCoroutine(AnimateBarsCoroutine(playerPos));
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
        float currentTime = Time.time;
        StopCoroutine(ResultsAnim());
        enemiesKilled.text = "Enemies Killed: " + gm.enemiesKilled.ToString();
        float minutesDec = (currentTime / 60);
        int min = Mathf.RoundToInt(minutesDec);
        int sec = (int)(Mathf.Abs(minutesDec - (float)min) * 60);
        timeTaken.text = "Time Taken: " + min.ToString() + "m " + sec.ToString() + "s";
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
        AudioSource playButtonAudioSource = playButton.GetComponent<AudioSource>();
        playButtonAudioSource.PlayOneShot(playButtonAudioSource.clip);
        playButton.transform.localScale *= 1.2f;
    }

    public void StartButtonPointerExit()
    {
        playButton.transform.localScale /= 1.2f;
    }

}