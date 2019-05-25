using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [Range(0, 1)]
    public float duration;
    [Range(0, 1)]
    public float magnitude;
    IEnumerator ShakeCam(Vector3 originalPos)
    {
        float timeElapsed = 0.0f;

        while (timeElapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float z = Random.Range(-1f, 1f) * magnitude;
            transform.localPosition = new Vector3(x, originalPos.y, z);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }

    public void Shake()
    {
        Vector3 originalPos = transform.localPosition;
        StartCoroutine(ShakeCam(originalPos));
        transform.localPosition = originalPos;
    }

}