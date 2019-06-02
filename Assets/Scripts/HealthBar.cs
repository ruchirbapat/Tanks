using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public GameObject remainingHealthCube;
    public Enemy enemyRef;

    float healthLeft;

    private void Start()
    {
        enemyRef = GetComponentInParent<Enemy>();
    }

    private void Update()
    {
        healthLeft = enemyRef.currentHealth / 100;
        Vector3 scale = remainingHealthCube.transform.localScale;
        scale.x *= healthLeft;
        remainingHealthCube.transform.localScale = scale;
        remainingHealthCube.transform.localPosition = new Vector3(-((1-scale.x) / 2), 0, 0);
    }
}
