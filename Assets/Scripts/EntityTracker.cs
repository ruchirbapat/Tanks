using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EntityTracker : MonoBehaviour
{
    public int enemiesInThisScene;
    public int enemiesLeft;
    public GameManager manager;

    void Start()
    {
        manager = GetComponentInParent<GameManager>();
    }
    void OnLevelWasLoaded()
    {
        enemiesInThisScene = FindObjectsOfType<Enemy>().Length;
    }

    void Update()
    {
        if(enemiesInThisScene > 0)
        {
            enemiesLeft = FindObjectsOfType<Enemy>().Length;
            if(enemiesLeft == 0)
                manager.NextLevel();
        }
    }
}
