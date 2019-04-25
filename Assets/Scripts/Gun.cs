﻿using UnityEngine;

public class Gun : MonoBehaviour
{

    public enum Mode { Automatic, Burst, Single };
    public Mode fireMode;

    public Bullet bullet;
    public int magazineSize;
    private int bulletsLeft;
    public Shell shell;

    public float damage;
    public float shotDelay;
    public float fireIntensity;
    public float reloadTime;
    private bool reloading;
    private bool triggerReleased;
    private float nextShot = 0;

    public Transform shootExitPoint;
    public Transform shellExitPoint;

    private void Start()
    {
        bullet.gunDamageAmount = damage;
        bullet.speed = fireIntensity;
        bulletsLeft = magazineSize;
        
        nextShot = Time.time + shotDelay / 100;
        //DebugGun();
    }

    private void LateUpdate()
    {
        if (!reloading && bulletsLeft == 0) {
            ReloadGun();
        }
    }

    private void ShootGun()
    {
        if ((!reloading) && (Time.time > nextShot) && (bulletsLeft > 0)) {
<<<<<<< HEAD
            nextShot = Time.time + shotDelay / 1000;
=======
>>>>>>> parent of 1a9f3de... Hate AI
            bulletsLeft--;
            nextShot = Time.time + shotDelay / 100;
            Instantiate(bullet, shootExitPoint.position, shootExitPoint.rotation).gunDamageAmount = damage;
            //print("Bullet created.");
            Instantiate(shell, shellExitPoint.position, shellExitPoint.rotation);
            //print("Shell created.");
        }
    }

    public void ReloadGun()
    {
        if (!reloading && bulletsLeft != magazineSize) {
            bulletsLeft = magazineSize;
            reloading = false;
        }
    }

    public void TriggerHeld()
    {
        ShootGun();
        triggerReleased = false;
    }

    public void TriggerReleased()
    {
        triggerReleased = true;
    }

    private void DebugGun()
    {
        if (bullet != null) { Debug.Log("Bullet: " + bullet); }
        if (shell != null) { Debug.Log("Shell: " + shell); }
        Debug.Log("Magazine Size: " + magazineSize);
        Debug.Log("Bullets Left: " + bulletsLeft);
        Debug.Log("Shot Delay: " + shotDelay);
        Debug.Log("Fire Intensity: " + fireIntensity);
        Debug.Log("Reload Time: " + reloadTime);
        Debug.Log("Reloading: " + reloading);
        Debug.Log("Trigger Released: " + triggerReleased);
    }
}
