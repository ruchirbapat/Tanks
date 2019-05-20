using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    int currentLevel;
    float enemiesAlive;

    void Awake()
    {
        currentLevel = 1;
    }

    void Update()
    {
        enemiesAlive = GameObject.FindObjectsOfType<Enemy>().Length;
        if(enemiesAlive <= 0)
        {
            LoadLevel(++currentLevel);
        }
    }

    void LoadLevel(int level)
    {
        SceneManager.LoadScene("Level " + level);
        Vector3 playerScreenPos = Camera.main.WorldToScreenPoint(FindObjectOfType<Player>().transform.position);
    }

}