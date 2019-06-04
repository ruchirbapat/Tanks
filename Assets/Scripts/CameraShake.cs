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
            // Generate how much to move camera on X and Z axis by a random amount (RNG)
            float x = Random.Range(-1f, 1f) * magnitude;
            float z = Random.Range(-1f, 1f) * magnitude;

            // Move camera to new position
            transform.localPosition = new Vector3(x, originalPos.y, z);

            // Update progress on animation completion
            timeElapsed += Time.deltaTime;

            // Return to the animation the next frame
            yield return null;
        }
    }

    // This public function calls the above animation function as the coroutine is private
    public void Shake()
    {
        // Store original function
        Vector3 originalPos = transform.localPosition;
        
        // Shake camera 
        StartCoroutine(ShakeCam(originalPos));

        // Reset camera to original position after camera shake has completed
        transform.localPosition = originalPos;
    }

}