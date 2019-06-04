// Dependencies
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Simulates a camera shake when the gun is fired or a bullet explodes (is destroyed)
public class CameraShake : MonoBehaviour
{
    // Duration of camera shake
    [Range(0, 1)]
    public float duration;

    // Magnitude/strength of the camera shakes
    [Range(0, 1)]
    public float magnitude;

    // The ALGORITHM in the following Coroutine shakes the camera
    IEnumerator ShakeCam(Vector3 originalPos)
    {
        // Time since the coroutine started (progress through the camera shake animation)
        float timeElapsed = 0.0f;

        // While the shake animation has not finished...
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