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
    public TextMeshProUGUI livesLeft; // Text element which displays how many lives the player has left
    public TextMeshProUGUI currentSceneText; // Text element which displays the current scene
    public GameObject playButton; // Reference to the Start game button
    public GameObject menuUI; // Reference to the entire main menu UI (a parent GameObject)
    public float bannerAnimSpeed; // Speed at which the Level N banner moves up and down. It's value is set in the Unity Editor
    public float bannerAnimWaitTime; // Time which the Level N banner should pause for before moving back down. Value also set in the Unity Editor

    // Unity's Awake function called once per script
    void Awake()
    {
        // Prevents the canvas and UI Manager object from being destroyed when a new level or scene is loaded.
        // This preserves the GameObject across scenes a.k.a. object persistence
        DontDestroyOnLoad(this);

        // If for some reason another UI manager exists in the scene, destroy this current one
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            // Destroys GameObject and lets garbage collector free resources
            Destroy(gameObject);
        }

        // Gets a reference to the Game Manager if the existing reference is null-valued
        if (gm == null)
        {
            // Finds object in scene with Game Manager script attached
            gm = FindObjectOfType<GameManager>();
        }
    }

    // Called after the Update function. Is used when we want to do UI things after calculation-based update code has been run
    private void LateUpdate()
    {
        // Update how many lives the player has left
        livesLeft.text = "x " + gm.playerLivesLeft.ToString();
        
        // Update the current scene i.e. Level 1/2/3/4... but show "Menu" if we are on the Menu (rather than showing Level 0)
        currentSceneText.text = gm.currentScene == 0 ? "Menu" : "Level " + gm.currentScene.ToString();
    }

    // This function subscribes my custom event listener to the Unity event 'sceneLoaded' which gets called when a scene is loaded
    void OnEnable()
    {
        SceneManager.sceneLoaded += EveryLevel;
    }

    // Unsubscribe from Unity event when the application is quit or scene changes
    void OnDisable()
    {
        SceneManager.sceneLoaded -= EveryLevel;
    }

    // The actual custom event listener. It gets a reference to the UI camera (a special camera I have set up for rendering UI elements)
    private void EveryLevel(Scene scene, LoadSceneMode mode)
    {
        GetComponent<Canvas>().worldCamera = GameObject.FindGameObjectWithTag("UI Camera").GetComponent<Camera>();
    }

    // Callback function to quit the game
    public void QuitGame()
    {
        Application.Quit();
    }

    // Callback function to return back to the main menu from the pause menu
    public void BackToMenu(bool animResults)
    {
        // Resume game
        gm.ResumeGame(); //if not already resumed

        // Load menu, animating results page if required. animResults is a boolean which is set in the Unity Editor
        gm.LoadMenu(animResults);

        // Enable the main menu UI
        menuUI.SetActive(true);

        // Disable the pause menu UI
        pauseMenu.SetActive(false);

        // Reset the Time scale to 1 so that time ticks
        Time.timeScale = 1f;
    }

    // Resets the results page
    public void ResetResults(bool animateResults)
    {
        // Reset important variables
        gm.enemiesKilled = 0;
        timeTaken.text = "";

        // Animate the results page if necessary (check Unity Editor)
        if (animateResults)
            StartCoroutine(ResetResultsAnim());
    }

    // Custom coroutine to animate the results page down 
    IEnumerator ResetResultsAnim()
    {
        // This algorithm moves the results page down past the to b elow the screen
        float speed = bannerAnimSpeed; // Speed at which the results page should move
        float animatePercent = 0; // Percentage which animation is complete

        // Iterate through the animation
        while (animatePercent <= 1)
        {

            // Increase animation percentage by time since last frame
            animatePercent += Time.deltaTime * speed;

            // Linearly interpolate the results page position downwards
            resultsMenu.GetComponent<RectTransform>().anchoredPosition = Vector2.up * Mathf.Lerp(540, -375, animatePercent);

            // Continue the animation here in the next frame
            yield return null;
        }
    }
    
    // Callback function to restart the game
    public void StartGame()
    {
        // Reset play start time
        gm.PlayStartTime = Time.time;

        // Advance to next level
        gm.NextLevel();

        // Reset menu UI
        menuUI.SetActive(false);
    }
    
    // Animates new wave banner
    public void AnimateNewLevelBanner()
    {
        // Stop previous coroutine animation
        StopCoroutine(BannerAnim());
        // Update banner text
        missionBar.GetComponentInChildren<Text>().text = "Level " + (SceneManager.GetActiveScene().buildIndex).ToString();
        // Animate banner
        StartCoroutine(BannerAnim());
    }

    // Animates the new wave banner up and down
    IEnumerator BannerAnim()
    {
        // This algorithm has the same logic as that in the results page animation, except that it pauses and changes direction to animate back down
        float delayTime = bannerAnimWaitTime;
        float speed = bannerAnimSpeed;
        float animatePercent = 0;
        int dir = 1;

        // Calculate end of pause time
        float endDelayTime = Time.time + 1 / speed + delayTime;

        // Animate up
        while (animatePercent >= 0)
        {
            // Increase animation perecentage
            animatePercent += Time.deltaTime * speed * dir;

            // Whether to switch animation direction
            if (animatePercent >= 1)
            {
                animatePercent = 1;

                // Pause till delay is not reached
                if (Time.time > endDelayTime)
                {
                    // Switch direction
                    dir = -1;
                }
            }

            // Animate either upwards or downwards depending on direction and animatePercent (+ or -)
            missionBar.GetComponent<RectTransform>().anchoredPosition = Vector2.up * Mathf.Lerp(350, 540, animatePercent);
            yield return null;
        }

    }

    // Calls the results page animation but sets its properties first 
    public void AnimateResults()
    {
        // Calculate time taken 
        float timeTaken = Time.time - gm.PlayStartTime;
        StopCoroutine(ResultsAnim());

        // Set results page properties (text elements)
        enemiesKilled.text = "Enemies Killed: " + gm.enemiesKilled.ToString();
        float minutesDec = (timeTaken / 60);
        int min = Mathf.RoundToInt(minutesDec);
        int sec = (int)(Mathf.Abs(minutesDec - (float)min) * 60);
        this.timeTaken.text = "Time Taken: " + min.ToString() + "m " + sec.ToString() + "s";

        // Format date
        todayDate.text = "Today's Date: " + System.DateTime.UtcNow.ToString("dd/MM/yy");

        finalScore.text = gm.currentScene.ToString() + " / " + gm.highestScene.ToString();
        
        // Start animation
        StartCoroutine(ResultsAnim());
    }

    // Coroutine to animate the results page upwards
    IEnumerator ResultsAnim()
    {
        // This algorithm follows the same logic as that in the animation to move the results page upwards
        float speed = bannerAnimSpeed;
        float animatePercent = 0;

        while (animatePercent <= 1)
        {
            animatePercent += Time.deltaTime * speed;
            resultsMenu.GetComponent<RectTransform>().anchoredPosition = Vector2.up * Mathf.Lerp(-375, 540, animatePercent);
            yield return null;
        }
    }

    // Callback function for when the user hovers their mouse over the Play button
    public void StartButtonPointerHover()
    {
        // Change button colour
        playButton.GetComponent<Image>().color = Color.green;

        // Play audio
        AudioSource playButtonAudioSource = playButton.GetComponent<AudioSource>();
        playButtonAudioSource.PlayOneShot(playButtonAudioSource.clip);

        // Make button larger
        playButton.transform.localScale *= 1.2f;
    }

    // Callback function for when the user takes their mouse cursor off the Play button
    public void StartButtonPointerExit()
    {
        // Reset button colour
        playButton.GetComponent<Image>().color = Color.white;

        // Reset button size
        playButton.transform.localScale /= 1.2f;
    }

    // Callback function to pause the game (set in Unity Editor)
    public void ShowPauseMenu()
    {
        pauseMenu.SetActive(true);
    }

    // Callback function to unpause the game (set in Unity Editor as well)
    public void HidePauseMenu()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }
}