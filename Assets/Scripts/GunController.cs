using UnityEngine;

class GunController : MonoBehaviour
{
    private Gun gun;
    public Gun Gun { get { return gun; } }

    private void Awake()
    {
        gun = GetComponent<Gun>();
    }

    public void Shoot()
    {
        gun.Shoot();
    }

    public void ReleaseTrigger() { gun.triggerReleasedLastFrame = true; }
    public void Aim(Vector3 point)
    {
        if (gun != null) {
            gun.transform.LookAt(new Vector3(point.x, gun.transform.position.y, point.z));
        }
    }
    public void TriggerHeld() { }


}


#if false
using UnityEngine;

[RequireComponent(typeof(Gun))]
public class GunController : MonoBehaviour
{
    private Gun gun;
    public Gun Gun { get { return gun; } }

    private void Awake()
    {
        gun = GetComponent<Gun>();
    }

    public void TriggerHeld()
    {
        if (gun != null) { gun.TriggerHeld(); }
    }

    public void TriggerReleased()
    {
        if (gun != null) { gun.TriggerReleased(); }
    }

    public void Aim(Vector3 point)
    {
        if (gun != null) { gun.transform.LookAt(new Vector3(point.x, gun.transform.position.y, point.z));
            //transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        }
    }

    public void Reload()
    {
        if (gun != null) { gun.ReloadGun(); }
    }

}
#endif