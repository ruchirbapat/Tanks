﻿using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Gun))]
class Gun : MonoBehaviour
{
    public enum GunType { Auto, Burst, SingleShot }

    [Header("Fields to be filled")]
    public Bullet bullet;
    public Shell shell;
    public Transform bulletExitPt;
    public Transform shellExitPt;

    [Header("General Properties")]
    public float gunDamage;
    public float bulletSpeed;
    public GunType gunType;

    [Header("Auto Properties")]
    [Tooltip("Delay between shots in milliseconds (ms)")]
    public float autoModeShotDelay;

    [Header("Burst Properties")]
    public int burstSize;
    [Tooltip("Delay between shots in seconds (s)")]
    public float delayBetweenBursts;

    [System.NonSerialized]
    private List<Bullet> activeBurstBullets;
    private float nextPossibleShootTime;
    public bool triggerReleasedLastFrame;

    void Start()
    {
        activeBurstBullets = new List<Bullet>();
        nextPossibleShootTime = Time.time;
    }

    public void Shoot()
    {
        if (gunType == GunType.Auto) {
            if (Time.time >= nextPossibleShootTime) {
                Bullet b = Instantiate(bullet, bulletExitPt.position, transform.rotation) as Bullet;
                b.speed = bulletSpeed;
                b.gunDamageAmount = gunDamage;
                nextPossibleShootTime = Time.time + (autoModeShotDelay / 1000);
                Instantiate(shell, shellExitPt.position, shellExitPt.rotation);

            }
        } else if (gunType == GunType.SingleShot && triggerReleasedLastFrame) { // single shot is still broken
            Bullet b = Instantiate(bullet, bulletExitPt.position, transform.rotation) as Bullet;
            b.speed = bulletSpeed;
            b.gunDamageAmount = gunDamage;
            Instantiate(shell, shellExitPt.position, shellExitPt.rotation);

        }
        else if (gunType == GunType.Burst) {
            activeBurstBullets.RemoveAll((x) => { return x == null; });
            if ((Time.time >= nextPossibleShootTime) && (activeBurstBullets.Count < burstSize)) {
                Bullet b = Instantiate(bullet, bulletExitPt.position, transform.rotation) as Bullet;
                b.speed = bulletSpeed;
                b.gunDamageAmount = gunDamage;
                activeBurstBullets.Add(b);
                nextPossibleShootTime = Time.time + delayBetweenBursts;
                Instantiate(shell, shellExitPt.position, shellExitPt.rotation);

            }
        }
    }
}


#if false
using UnityEngine;

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
            nextShot = Time.time + shotDelay / 100;
            bulletsLeft--;
            Bullet b = Instantiate(bullet, shootExitPoint.position, shootExitPoint.rotation) as Bullet;
            b.gunDamageAmount = damage;
            b.speed = fireIntensity;
            Instantiate(shell, shellExitPoint.position, shellExitPoint.rotation);
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
#endif