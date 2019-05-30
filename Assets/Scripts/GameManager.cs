using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    UIManager um;
    public int currentScene;
    public int ogPlayerLives;
    public int playerLivesLeft;
    public int ogEnemyCount;
    public int highestScene;
    int enemiesLeft;
    bool gameOver = false;
    public int enemiesKilled;

    void Awake()
    {
        DontDestroyOnLoad(this);
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
        if (um == null)
        {
            um = FindObjectOfType<UIManager>();
        }
        playerLivesLeft = ogPlayerLives;
        StartCoroutine(CheckEntities(3f));
    }

    void OnLevelWasLoaded()
    {
        ogEnemyCount = FindObjectsOfType<Enemy>().Length;
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.H)) { if (currentScene == 0) { um.StartGame(); } else { NextLevel(); } }
    }

    IEnumerator CheckEntities(float delay)
    {
        while (true)
        {
            enemiesLeft = FindObjectsOfType<Enemy>().Length;
            bool playerAlive = (FindObjectOfType<Player>() != null);

            if (!gameOver)
            {
                if (ogEnemyCount > 0)
                {
                    if (enemiesLeft == 0)
                    {
                        if (currentScene == highestScene)
                        {
                            gameOver = true;
                        }
                        else
                        {
                            NextLevel();
                        }
                    }
                    else if (!playerAlive)
                    {
                        if (playerLivesLeft > 0)
                        {
                            ReloadLevel();
                        }
                        else
                        {
                            gameOver = true;
                        }
                    }
                }

                if (gameOver)
                {
                    GameOver();
                }

                if (Input.GetKeyDown(KeyCode.H)) { if (currentScene == 0) { um.StartGame(); } else { NextLevel(); } }

            }

            if (delay <= 0f)
            {
                Debug.LogError("Entity Check Delay too low");
            }
            else
            {
                yield return new WaitForSeconds(delay + Time.deltaTime);
            }
        }
    }

    public void NextLevel() {
        gameOver = false;
        //        currentScene++;
        StartCoroutine(LoadSceneCoroutine(true));
        print("done couroutine!!");
    }

    IEnumerator LoadSceneCoroutine(bool nextLevel)
    {
        if (nextLevel)
            currentScene++;
        SceneManager.LoadScene(currentScene);
        yield return null;
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(currentScene));
        um.AnimateNewLevelBanner();
        um.AnimateBars();
    }

    public void ReloadLevel() { StartCoroutine(LoadSceneCoroutine(false)); }
    public void LoadMenu() { currentScene = 0; SceneManager.LoadScene(currentScene); um.ResetResults(); }
    public void GameOver() { print("game over"); um.AnimateResults(); enemiesKilled = 0; }
 
}

#if false
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int currentSceneIndex = 0;
    bool onMenu = false;
    bool isPaused = false;

    public int highestSceneIndex;
    public float animationTime;
    public Canvas ui;
    public GameObject gameOverUI;
    public Image xBar;
    public Image yBar;
    public int playerMaxLives;
    public int playerLivesLeft;

    public int enemiesInThisScene;
    public int enemiesLeft;

    void OnLevelWasLoaded()
    {
        enemiesInThisScene = FindObjectsOfType<Enemy>().Length;
    }

    void Start()
    {
        playerLivesLeft = playerMaxLives;
        print("Game Manager START");
        DontDestroyOnLoad(this);
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {

        enemiesLeft = FindObjectsOfType<Enemy>().Length;

        if (((enemiesInThisScene > 0) && (enemiesLeft <= 0)) || !playerAlive)
        {
            print("player alive: " + playerAlive.ToString());
            print("enemies: " + enemiesLeft.ToString());
            manager.ProgressLevel(playerAlive);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            ProgressLevel(true);
        }
    }

    IEnumerator ShowResults()
    {
        gameOverUI.SetActive(true);

        float delayTime = 3f;
        float speed = 3f;
        float animatePercent = 0;

        while (animatePercent >= 0)
        {
            animatePercent += Time.deltaTime * speed;
            //need to make scalable
            gameOverUI.GetComponent<RectTransform>().anchoredPosition = Vector2.up * Mathf.Lerp(-950, 0, animatePercent);
            yield return null;
        }

        gameOverUI.SetActive(false);
    }


    void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void BackToMainMenu()
    {
        currentSceneIndex = 0;
        playerLivesLeft = playerMaxLives;
        LoadScene(currentSceneIndex);
    }

    public void ProgressLevel(bool playerAlive)
    {
        bool goingToMenu = false;
        if(playerAlive) {
            currentSceneIndex++;
        } else {
            if(playerLivesLeft <= 0) {
                currentSceneIndex = 0;
                goingToMenu = true;
            }
        }

        if ((goingToMenu) || ((currentSceneIndex - 1) == highestSceneIndex))
        {
            StartCoroutine(ShowResults());
        }
        else
        {
            LoadScene(currentSceneIndex);
        }
#if false
        Vector3 playerScreenPos = Camera.main.WorldToViewportPoint(FindObjectOfType<Player>().transform.position);
        //        print("scene loaded, player is at: " + playerScreenPos.ToString());
        StartCoroutine(AnimateNewLevelUI(playerScreenPos));
        //StartCoroutine(HandleLevel(playerAlive));
#endif
    }

    // It is important that this function is marked private
    private IEnumerator HandleLevel(bool playerAlive)
    {
        if (!playerAlive)
        {
            if(playerLivesLeft <= 0)
            {
                StartCoroutine(ShowResults());
                currentSceneIndex = 0;

                //SceneManager.LoadScene(0); // Load menu
                playerLivesLeft = playerMaxLives; // Reset player lives
                playerAlive = true;
            }
        } else {
            currentSceneIndex++;
        }

        currentSceneIndex = (int)Mathf.Clamp(currentSceneIndex, 0, highestSceneIndex);

      //  if(current)

        SceneManager.LoadScene(currentSceneIndex);
        yield return null;

        Vector3 playerScreenPos = Camera.main.WorldToViewportPoint(FindObjectOfType<Player>().transform.position);
   //        print("scene loaded, player is at: " + playerScreenPos.ToString());
        StartCoroutine(AnimateNewLevelUI(playerScreenPos));
    }

    IEnumerator AnimateNewLevelUI(Vector3 playerPos)
    {
        float percent = 0;
        float speed = 1 / animationTime;
        while(percent != 1)
        {
            if (percent > 1)
                percent = 1;

            xBar.transform.position = new Vector3(Mathf.Lerp(-962, playerPos.x, percent), xBar.transform.position.y, xBar.transform.position.z);
            yBar.transform.position = new Vector3(yBar.transform.position.x, Mathf.Lerp(-542, playerPos.y, percent), yBar.transform.position.z);

            percent += (Time.deltaTime * speed);
            yield return null;
        }
    }
}
#endif