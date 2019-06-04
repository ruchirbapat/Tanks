// Dependencies
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // References
    UIManager um; // Reference to UI mananger
    public int currentScene; // Stores the current scene the game is on
    public int ogPlayerLives; // Stores the maximum number of lives the player has (set in the Unity Editor)
    public int playerLivesLeft; // Stores how many lives the player has left
    public int ogEnemyCount; // Stores how many enemies there originally were in a particular Scene/level
    public int highestScene; // Stores the highest scene the game has to offer (final level)
    public bool gamePaused = false; // Stores the state of the game as a boolean i.e. whether it is paused or not 
    public float PlayStartTime; // Stores the time at which the user began playing (the time they clicked the Play button)
    int enemiesLeft; // Stores how many enemies are left in a particular level
    bool backToMenu = false; // Stores whether the game should return to the menu the next frame
    public int enemiesKilled; // Stores how many enemies have been killed i.e. ogEnemyCount - enemiesLeft

    void Awake()
    {
        // If another instance of the game manager exists, then destroy this instance
        DontDestroyOnLoad(this);
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
        
        // Find a runtime reference to the UI manager
        if (um == null)
        {
            um = FindObjectOfType<UIManager>();
        }

        // Find what the highest level the game can go to, using Unity's SceneManager
        highestScene = SceneManager.sceneCountInBuildSettings - 1;

        // Iniitally set the number of lives the player has to be equal to the max no. of lives they can have
        playerLivesLeft = ogPlayerLives;

        // Start the Game Manager core logic, which checks how many entities are left and updates the game accordingly
        StartCoroutine(CheckEntities(3f));
    }

    // Custom event listener subscribed the Unity event which gets called when a scene is loaded
    void OnEnable()
    {
        SceneManager.sceneLoaded += EveryLevel;
    }


    // Unsubscribe from the event when the scene is changed
    void OnDisable()
    {
        SceneManager.sceneLoaded -= EveryLevel;
    }

    // The actual event that gets called
    private void EveryLevel(Scene scene, LoadSceneMode mode)
    {
        // When a new scene is loaded, count the number of enemies in that scene and store it
        ogEnemyCount = FindObjectsOfType<Enemy>().Length;
    }

    // Called after all other Update functions are called
    private void LateUpdate()
    {
        // If the 'H' key is pressed, the user can skip the current level (this is a cheat code used for debugging/marking purposes)
        if (Input.GetKeyDown(KeyCode.H)) { if (currentScene == 0) { um.StartGame(); } else { NextLevel(); } }

        // Reload the current level if 'R' is pressed 
        if (Input.GetKeyDown(KeyCode.R)) { ReloadLevel(); }

        // Pause/Resume the game if 'P' is pressed and the player is not on the main menu
        if (Input.GetKeyDown(KeyCode.P) && currentScene != 0) {
            // If paused
            if (gamePaused)
            {
                // Then resume
                ResumeGame();
            } else
            {
                // Else pause
                PauseGame();
            }
        }
    }

    // Runs on a timer to check entities and update the scene hierarchy accordingly
    // The following comments will explain the ALGORITHM
    IEnumerator CheckEntities(float delay)
    {
        // Loop forever
        while (true)
        {
            // Count number of enemies left in the scene
            enemiesLeft = FindObjectsOfType<Enemy>().Length;

            // Check whether player is alive 
            bool playerAlive = (FindObjectOfType<Player>() != null);

            // If the game is still being played
            if (!backToMenu) {

                // If were enemies in this scene at all
                if (ogEnemyCount > 0) {

                    // If there are no enemies left to be defeated
                    if (enemiesLeft <= 0) {

                        if (currentScene >= highestScene) {
                            // Go back to the menu if the player has completed the last level
                            backToMenu = true;
                        } else {
                            // Else go to the next level
                            NextLevel();
                        }
                    } else if (!playerAlive) {
                        // If the player is not alive but still has lives left
                        if (playerLivesLeft > 0) {
                            // Reload the level and let them have another go
                            ReloadLevel();
                        } else {
                            // Else the manager should redirect to the main menu 
                            backToMenu = true;
                        }
                    }
                }

                // If we need to go back to the main menu, then the current 'game round' is over
                if (backToMenu) {
                    // End the game (show results etc.)
                    GameOver();
                }
            }

            // Check that the delay is not a garbage value
            if (delay <= 0f) {
                Debug.LogError("Entity Check Delay too low");
            } else {
                // Wait a specified amount of time (delay) before repeating the check all over again
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

    // Reload the current scene without updating the currentSceneIndex
    public void ReloadLevel() {
        StartCoroutine(LoadSceneCoroutine(false));
    }

    // Loads the menu
    public void LoadMenu(bool animateResultsPage) {
        // Set current scene to 0 (menu scene)
        currentScene = 0;

        // Load menu
        SceneManager.LoadScene(currentScene);

        // If we are going to the menu from the pause screen, don't animate the results page UP and DOWN
        // However if the player died or got to the last level, show the results page
        if (animateResultsPage) {
            // Call function to animate results banner
            um.ResetResults(animateResultsPage);
        }

        // Going to the main menu will reset how many lives the player has left, as well as how many enemies they have killed
        playerLivesLeft = ogPlayerLives;
        enemiesKilled = 0;
    }

    // Called when the game ends i.e. player dies or completes last level
    public void GameOver() {
        // Animate results banner
        um.AnimateResults();
    }

    // Pauses game
    public void PauseGame()
    {
        // Update state
        gamePaused = true;

        // Show paused menu
        um.ShowPauseMenu();

        // Stop time
        Time.timeScale = 0f;
    }

    // Resumes game
    public void ResumeGame()
    {
        // Remove pause menu
        um.HidePauseMenu();

        // Update state
        gamePaused = false;

        // Resume time
        Time.timeScale = 1f;
    }
}