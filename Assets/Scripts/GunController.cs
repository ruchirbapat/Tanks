// Dependencies
using UnityEngine;

// Controls an Enemy or Player's gun
class GunController : MonoBehaviour
{
    // Return reference to Gun
    private Gun gun;
    public Gun Gun { get { return gun; } }

    private void Awake()
    {
        // Find runtime reference to Gun
        gun = GetComponent<Gun>();
    }

    // Wrapper function to shoot the gun
    public void Shoot()
    {
        gun.Shoot();
    }

    // Wrapper function to mark whether the trigger was released
    public void ReleaseTrigger() {
        gun.triggerReleasedLastFrame = true;
    }
    
    // Aims the Player or Enemy gun at a specific point in the game arena
    public void Aim(Vector3 point)
    {
        if (gun != null) {
            
            // Use Unity LookAt function to rotate the gun
            gun.transform.LookAt(new Vector3(point.x, gun.transform.position.y, point.z));
        }
    }
}