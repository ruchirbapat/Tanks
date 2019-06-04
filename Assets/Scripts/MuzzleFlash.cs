// Dependencies
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script to make muzzle flashes
public class MuzzleFlash : MonoBehaviour
{
    // Public properties
    public GameObject flashHolder; // Parent of all the flash sprites
    public Sprite[] flashSprites; // The actual flash sprites
    public SpriteRenderer[] spriteRenderers; // Sprite Renderers associated with the Sprites, they will render the muzzle flashes
    public float flashTime; // How long to flash for before deactivating the flash

    public void Start()
    {
        // Start the game with no muzzle flash (obviously)
        Deactivate();
    }

    // Function to flash the gun
    public void Activate()
    {
        // Activate flashes
        flashHolder.SetActive(true);

        // Choose a random flash sprite. This is another example of using RNG
        int flashSpriteIndex = Random.Range(0, flashSprites.Length);

        // Update the 4 sprite renderers to display the RNG flash sprite
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].sprite = flashSprites[flashSpriteIndex];
        }

        // Deactivate the flash after a certain time. This time is 'flashTime'
        Invoke("Deactivate", flashTime);
    }

    // Deactivate muzzle flash sprites after set time
    public void Deactivate()
    {
        flashHolder.SetActive(false);
    }

}
