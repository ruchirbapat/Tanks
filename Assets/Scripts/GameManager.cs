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
    public bool gamePaused = false;
    public float PlayStartTime;
    int enemiesLeft;
    bool backToMenu = false;
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
        highestScene = SceneManager.sceneCountInBuildSettings - 1;
        playerLivesLeft = ogPlayerLives;
        StartCoroutine(CheckEntities(3f));
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
        ogEnemyCount = FindObjectsOfType<Enemy>().Length;
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.H)) { if (currentScene == 0) { um.StartGame(); } else { NextLevel(); } }
        if (Input.GetKeyDown(KeyCode.R)) { ReloadLevel(); }
        if (Input.GetKeyDown(KeyCode.P) && currentScene != 0) {
            if(gamePaused)
            {
                ResumeGame();
            } else
            {
                PauseGame();
            }
        }
    }

    IEnumerator CheckEntities(float delay)
    {
        while (true)
        {
            enemiesLeft = FindObjectsOfType<Enemy>().Length;
            bool playerAlive = (FindObjectOfType<Player>() != null);
            if (!backToMenu) {
                if (ogEnemyCount > 0) {
                    if (enemiesLeft <= 0) {
                        if (currentScene >= highestScene) {
                            backToMenu = true;
                        } else {
                            NextLevel();
                        }
                    } else if (!playerAlive) {
                        if (playerLivesLeft > 0) {
                            ReloadLevel();
                        } else {
                            backToMenu = true;
                        }
                    }
                }

                if (backToMenu) {
                    GameOver();
                }
            }

            if (delay <= 0f) {
                Debug.LogError("Entity Check Delay too low");
            } else {
                yield return new WaitForSeconds(delay + Time.deltaTime);
            }
        }
    }

    public void NextLevel() {
        backToMenu = false;
        StartCoroutine(LoadSceneCoroutine(true));
        print("done couroutine!!");
    }

    IEnumerator LoadSceneCoroutine(bool nextLevel)
    {
        if (nextLevel)
        {
            currentScene++;
            currentScene = (int)Mathf.Clamp(currentScene, 0, highestScene);
        }
        SceneManager.LoadScene(currentScene);
        yield return null;
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(currentScene));
        um.AnimateNewLevelBanner();
    }

    public void ReloadLevel() { StartCoroutine(LoadSceneCoroutine(false)); }
    public void LoadMenu(bool animateResultsPage) {
        currentScene = 0; SceneManager.LoadScene(currentScene);
        if (animateResultsPage) {
            um.ResetResults(animateResultsPage);
        }
    }
    public void GameOver() {
        print("game over");
        um.AnimateResults();
        playerLivesLeft = ogPlayerLives;
        enemiesKilled = 0;
    }
    public void PauseGame()
    {
        gamePaused = true;
        um.ShowPauseMenu();
        Time.timeScale = 0f;
    }
    public void ResumeGame()
    {
        um.HidePauseMenu();
        gamePaused = false;
        Time.timeScale = 1f;
    }
}