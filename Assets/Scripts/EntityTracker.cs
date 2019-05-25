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

    void LateUpdate()
    {
        bool playerAlive = (FindObjectOfType<Player>() != null);

        if( ( (enemiesInThisScene > 0) && (FindObjectsOfType<Enemy>().Length <= 0) ) || !playerAlive)
        {
            manager.ProgressLevel(playerAlive);
        }
    }
}
