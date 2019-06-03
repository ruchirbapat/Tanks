using UnityEngine;
using System.Collections.Generic;

public class Gun : MonoBehaviour
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

    [Header("Recoil properties")]
    public float recoilStrength;
    public float recoilTime;

    [Header("Audio properties")]
    AudioSource audioSource;

    [System.NonSerialized]
    private List<Bullet> activeBurstBullets;
    private float nextPossibleShootTime;

    public bool triggerReleasedLastFrame;
    MuzzleFlash muzzleFlash;

    Vector3 recoilSmoothDampVelocity;
    Vector3 initialLocalPosition;

    void Start()
    {
        activeBurstBullets = new List<Bullet>();
        nextPossibleShootTime = Time.time;
        muzzleFlash = GetComponentInChildren<MuzzleFlash>();
        initialLocalPosition = transform.localPosition;
        audioSource = GetComponent<AudioSource>();
    }

    void LateUpdate()
    {
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, initialLocalPosition, ref recoilSmoothDampVelocity, recoilTime);
    }

    public void Shoot()
    {
        bool shootable = false;
        if (gunType == GunType.Auto) {
            if (Time.time >= nextPossibleShootTime) {
                shootable = true;
                nextPossibleShootTime = Time.time + (autoModeShotDelay / 1000);
            }
        }
        else if (gunType == GunType.Burst) {
            activeBurstBullets.RemoveAll((x) => { return x == null; });
            if ((Time.time >= nextPossibleShootTime) && (activeBurstBullets.Count < burstSize)) {
                shootable = true;
                nextPossibleShootTime = Time.time + delayBetweenBursts;
            }
        }

        if(shootable)
        {
            Bullet b = Instantiate(bullet, bulletExitPt.position, transform.rotation) as Bullet;
            b.speed = bulletSpeed;
            b.gunDamageAmount = gunDamage;
            activeBurstBullets.Add(b);
            Instantiate(shell, shellExitPt.position, shellExitPt.rotation);
            try { audioSource.PlayOneShot(audioSource.clip); } catch { };
            muzzleFlash.Activate();
            transform.localPosition -= transform.forward * recoilStrength;
            Camera.main.GetComponent<CameraShake>().Shake();
        }
    }
}