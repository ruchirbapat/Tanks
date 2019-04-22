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

    private Transform barrel;
    private Transform exit;

    private void Start()
    {
        bullet.damage = damage;
        bullet.speed = fireIntensity;
        bulletsLeft = magazineSize;
        for (int i = 0; i < gameObject.transform.childCount; i++) {
            if (gameObject.transform.GetChild(i).tag == "Gun_Tip") {
                barrel = gameObject.transform.GetChild(i);
            } else if (gameObject.transform.GetChild(i).tag == "Gun_Exit") {
                exit = gameObject.transform.GetChild(i);
            }
        }
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
            bulletsLeft--;
            nextShot = Time.time + shotDelay / 100;
            Instantiate(bullet, barrel.position, barrel.rotation).damage = damage;
            //print("Bullet created.");
            Instantiate(shell, exit.position, exit.rotation);
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
