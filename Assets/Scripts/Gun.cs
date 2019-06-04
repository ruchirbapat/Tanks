// Dependencies
using UnityEngine;
using System.Collections.Generic;

public class Gun : MonoBehaviour
{
    /*
     * Most fields in this class are assigned values through the Unity Editor, and are NOT hardcoded
     */

    // Properties
    public enum GunType { Auto, Burst } // Type of Gun

    // Public references
    [Header("Fields to be filled")]
    public Bullet bullet; // Reference to Bullet Prefab
    public Shell shell; // Reference to Shell Prefab
    public Transform bulletExitPt; // Reference to position from where a Bullet should be shot from
    public Transform shellExitPt; // Refernece to position from wher a Shell should be ejected from

    // General properties
    [Header("General Properties")]
    public float gunDamage; // Amount of damage the gun will deal per Bullet 
    public float bulletSpeed; // Speed of Bullets it shoots
    public GunType gunType; // Actual type of Gun it is

    // Properties for Auto guns
    [Header("Auto Properties")]
    [Tooltip("Delay between shots in milliseconds (ms)")]
    public float autoModeShotDelay; // Delay between shots in milliseconds

    // Properties for Burst guns
    [Header("Burst Properties")]
    public int burstSize; // How many bullets are in a burst fire
    [Tooltip("Delay between shots in seconds (s)")]
    public float delayBetweenBursts; // Delay between each bullet in the burst (in seconds)

    // Recoil properties 
    [Header("Recoil properties")]
    public float recoilStrength; // Magnitude of recoil
    public float recoilTime; // How long to recover from the recoil

    // Audio clip properties 
    [Header("Audio properties")]
    AudioSource audioSource; // Audio clip to play when a bullet is fired 

    // Miscellaneous properties
    [System.NonSerialized]

    // A List<Bullet> is identical in functionality to an Array!
    private List<Bullet> activeBurstBullets; // Stores the bullets from a burst in this array
    private float nextPossibleShootTime; // The next time a Player can shoot i.e. current time + a small delay between burst shots

    // Private reference to the Muzzle Flash component. Muzzle flash happens on a child object!
    MuzzleFlash muzzleFlash;

    // Unused yet important fields, they are required by Unity Engine for the SmoothDamp functions...
    // These fields are used by the Unity Engine for calculations
    Vector3 recoilSmoothDampVelocity;
    Vector3 initialLocalPosition;

    void Start()
    {
        // Create new List<>
        activeBurstBullets = new List<Bullet>();

        // Set up variables and find runtime references
        nextPossibleShootTime = Time.time;
        muzzleFlash = GetComponentInChildren<MuzzleFlash>();
        initialLocalPosition = transform.localPosition;
        audioSource = GetComponent<AudioSource>();
    }

    void LateUpdate()
    {
        // Recover from recoil if necessary
        // Returns to original position (initialPosition) from current position (transform.position)
        // over a brief period of time (recoilTime). Therfore since it moves a certain distance over time, it has a velocity (ref recoilSmoothDampVelocity)
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, initialLocalPosition, ref recoilSmoothDampVelocity, recoilTime);
    }

    // Shoot() function gets called by the Player and Enemy classes. It shoots a bullet in burst or automatic rifle modes
    public void Shoot()
    {

        /*
         *  The following algorithm decides whether a bullet can be shot, and shoots one if necessary
         */

        // Stores whether a bullet can be shot, its value (true or false) is decided from the conditions below
        bool shootable = false;

        if (gunType == GunType.Auto) {
            // If we are allowed to shoot i.e. a small delay between the last bullet has happened
            if (Time.time >= nextPossibleShootTime) {
                // Then we can shoot
                shootable = true;
                // Calculate the next time a bullet can be fired after
                nextPossibleShootTime = Time.time + (autoModeShotDelay / 1000);
            }
        }
        // For burst mode 
        else if (gunType == GunType.Burst) {
            // Remove destroyed bullets (if the bullets are null)
            activeBurstBullets.RemoveAll((x) => { return x == null; });
            // Condition for a bullet to be shot:
            // 1. Enough time has passed since the last bullet being fired
            // 2. The number of bullets that are in the game arena (activeBurstBullets.Count) is less than the 
            // maximum number of bullets that a burst can fire (burstSize)
            if ((Time.time >= nextPossibleShootTime) && (activeBurstBullets.Count < burstSize)) {
                // Then we can shoot
                shootable = true;
                // Calculate next shoot time
                nextPossibleShootTime = Time.time + delayBetweenBursts;
            }
        }

        // If after all the conditional checking we may shoot a bullet...
        if(shootable)
        {
            // Instantiate a bullet
            Bullet b = Instantiate(bullet, bulletExitPt.position, transform.rotation) as Bullet;
            b.speed = bulletSpeed; // Set it's speed
            b.gunDamageAmount = gunDamage; // Set how much damage it will deliver
            activeBurstBullets.Add(b); // Add it to the List<> of active bullets in the world
            Instantiate(shell, shellExitPt.position, shellExitPt.rotation); // Instantiate a Shell with RNG force valuess
            try {
                audioSource.PlayOneShot(audioSource.clip);
            } catch { }; // Play the Bullet SFX audio
            // Flash muzzle sprites
            muzzleFlash.Activate();
            // Apply recoil i.e. move the gun's position slightly backwards to simualate recoil (initialPosition) but return back to the original
            transform.localPosition -= transform.forward * recoilStrength;
            // Shake camera when bullet fired
            Camera.main.GetComponent<CameraShake>().Shake();
        }
    }
}