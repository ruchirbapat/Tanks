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
        if(Input.GetKeyDown(KeyCode.H))
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

#if false
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    int currentLevel;
    float enemiesAlive;
    Texture2D tex;
    public Text missionText;
    public GameObject ui;
    public Image xBar;
    public Image yBar;

    void Start()
    {
        DontDestroyOnLoad(this);
        DontDestroyOnLoad(ui);
        tex = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Cursors/Crosshair 1.png", typeof(Texture2D));
        currentLevel = SceneManager.GetActiveScene().buildIndex + 1;
        Cursor.SetCursor(tex, Vector2.zero, CursorMode.Auto);
    }

    private void OnMouseEnter()
    {
        Cursor.SetCursor(tex, Vector2.zero, CursorMode.Auto);
    }

    void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    void Update()
    {
        enemiesAlive = GameObject.FindObjectsOfType<Enemy>().Length;
        if(enemiesAlive <= 0)
        {
            LoadLevel(++currentLevel);
        }
        missionText.text = "Level " + currentLevel.ToString();
    }

    void LoadLevel(int level)
    {
        SceneManager.LoadScene("Level " + level);
        Vector3 playerScreenPos = Camera.main.WorldToScreenPoint(FindObjectOfType<Player>().transform.position);
        StartCoroutine(MoveBars(playerScreenPos));
    }
    
    IEnumerator MoveBars(Vector3 pos)
    {
        float delayTime = 2f;
        float speed = 3f;
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

            xBar.rectTransform.anchoredPosition = Vector2.right * Mathf.Lerp(-962, pos.x, animatePercent);
            yBar.rectTransform.anchoredPosition = Vector2.down * Mathf.Lerp(542, pos.y, animatePercent);
            yield return null;
        }

    }

}
#endif