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