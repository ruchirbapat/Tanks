using UnityEngine;

[RequireComponent(typeof(Gun))]
public class GunController : MonoBehaviour
{
    private Gun gun;
    public Gun Gun { get { return gun; } }
    public Transform barrel;
    public Transform exit;

    private void Awake()
    {
        gun = GetComponent<Gun>();
    }

    public void EquipGun(Gun toEquip)
    {
        if (gun != null) {
            Destroy(gun.gameObject);
        }

        gun = Instantiate(toEquip, barrel.position, barrel.rotation) as Gun;
        gun.transform.parent = barrel;
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
        if (gun != null) { gun.transform.LookAt(point); }
    }

    public void Reload()
    {
        if (gun != null) { gun.ReloadGun(); }
    }

}
