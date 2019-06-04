// Dependencies
using UnityEngine;

public class Bullet : MonoBehaviour
{

    // Public properties
    public LayerMask collidable; // LayerMask indicates which surfaces the Bullet may collide with
    public static int bounceable; // Stores the layer mask of the bounceable surface
    public Color trailColour; // Property assigned to the colour of the trail (set in Unity Editor)
    public float trailDuration; // How long the trail should be cast behind the Bullet
    public float speed = 10; // Speed of the Bullet, which is updated by the gun
    public float lifetime = 3f; // How long the bullet lasts before automatically being destroyed
    public float gunDamageAmount; // How much damage the Bullet deals
    public int maxBounces; // Maximum number of bounces the Bullet can have before being destroyed
    private int bounces = 0; // How many bounces have occurred
    private CameraShake camShake; // Refernce to CameraShaker GameObject

    // Audio source reference so SFX can be played
    AudioSource audioSource;

    private void Awake()
    {
        // Set the layer of which the bullet can bounce against i.e. find the "environment" objects' layer
        bounceable = GameObject.FindGameObjectsWithTag("Environment")[0].gameObject.layer;
    }

    void OnDestroy() {
        // Shake the camera when the bullet is destroyed
        try {
            Camera.main.GetComponent<CameraShake>().Shake();
        } catch { };
    }

    private void Start()
    {
        // Destroy the object after a specified period of time (lifetime)
        Destroy(gameObject, lifetime);

        // Set runtime reference to audio player component
        audioSource = GetComponent<AudioSource>();

        // Set the trail renderer duration
        GetComponent<TrailRenderer>().time = trailDuration;
    }

    private void Update()
    {
        // Calculate displacement using speed = distance/time
        float displacement = speed * Time.deltaTime;

        // Check for collisions against world
        CheckForCollisions(displacement);

        // Move bullet
        transform.Translate(Vector3.forward * displacement);
    }

    private void CheckForCollisions(float velocity)
    {
        // Cast ray out
        Ray ray = new Ray(gameObject.transform.position, gameObject.transform.forward);
        RaycastHit hit;

        // Draw debug ray in Unity Editor
        Debug.DrawRay(ray.origin, ray.direction, Color.red);
        
        // If any objects are in the way, perform HitObject() function them
        if (Physics.Raycast(ray, out hit, velocity + .1f, collidable)) {
            HitObject(hit, hit.point);
        }
    }

    private void HitObject(RaycastHit hit, Vector3 hitPoint)
    {
        // Check if the object can be bounced on
        if (hit.collider.gameObject.layer == bounceable) {

            // If so, play the bounce audio
            audioSource.PlayOneShot(audioSource.clip);

            if (bounces >= maxBounces) {
                // Destroy if Bullet bounced more than max. times
                Destroy(gameObject);
            } else {
                // Else increase bounce counter
                bounces++;

                // Reflect bullet off the wall, using the collider's normal
                gameObject.transform.forward = Vector3.Reflect((hit.point - gameObject.transform.position), hit.normal);
            }

        } else {
            if (hit.collider.gameObject.layer == Globals.PlayerLayer) {
                // Deal damage to Player if Bullet hit Player 
                hit.collider.gameObject.GetComponentInParent<Player>().TakeHit(gunDamageAmount, hit.point, transform.forward);
            }
            else if (hit.collider.gameObject.layer == Globals.EnemyLayer) {
                // Deal damage to Enemy if Bullet hit Enemy
                hit.collider.gameObject.GetComponentInParent<Enemy>().TakeHit(gunDamageAmount, hit.point, transform.forward);

                // Increment enemiesKilled counter
                try {
                    FindObjectOfType<GameManager>().enemiesKilled++;
                } catch { };
            } else if(hit.collider.gameObject.layer == Globals.BulletLayer) {
                // If Bullet hit another Bullet, destroy both Bullets
                Destroy(hit.collider.gameObject);
            }

            // Else Destroy
            Destroy(gameObject);
        }
    }

}
