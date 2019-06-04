// Dependencies
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Crosshair animation script (UI)
public class CrosshairAnim : MonoBehaviour
{
    // Which elements the crosshair can interact with (set in Unity Editor)
    public LayerMask hitMask;

    // Reference to the dot at the centre of the crosshair
    public SpriteRenderer dotRend;

    // What colour the dot should turn when hovering over an enemy (set in Unity Editor)
    public Color dotHighlightedColor;
    
    // How fast to spin the crosshair
    public float turnSpeed = 50f;

    // Will store the crosshair original colour
    Color dotOriginalColor;

    private void Start()
    {
        // Store the crosshair original colour
        dotOriginalColor = dotRend.color;
    }

    private void Update()
    {
        // Rotate the crosshair
        transform.Rotate(Vector3.forward * turnSpeed * Time.deltaTime);
    }

    public void MatchTargets(Ray ray)
    {
        // Checks if the crosshair is over an enemy
        if(Physics.Raycast(ray, 1000, hitMask))
        {
            // Change the crosshair colour
            dotRend.color = dotHighlightedColor;
        } else
        {
            // Reset the crosshair colour since the crosshair is NOT over the enemy
            dotRend.color = dotOriginalColor;
        }
    }
}
