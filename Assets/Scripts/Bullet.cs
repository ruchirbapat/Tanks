using UnityEngine;

public class Bullet : MonoBehaviour
{

    public LayerMask collidable;
    public static int bounceable;
    public Color trailColour;
    public float trailDuration;
    public float speed = 10;
    public float lifetime = 3f;
    private static float skin = .1f;
    public float gunDamageAmount;
    public int maxBounces;
    private int bounces = 0;
    private CameraShake camShake;
    private float instantiateTime;
    private float deathTime;

    AudioSource audioSource;

    private void Awake()
    {
        bounceable = GameObject.FindGameObjectsWithTag("Environment")[0].gameObject.layer;
    }

    void OnDestroy() {
        Camera.main.GetComponent<CameraShake>().Shake();
    }

    private void Start()
    {
        Destroy(gameObject, lifetime);
        audioSource = GetComponent<AudioSource>();
        GetComponent<TrailRenderer>().time = trailDuration;
    }

    private void Update()
    {
        /*if (Time.time > deathTime)
            Destroy(gameObject);//DeleteBullet();*/

        float velocity = speed * Time.deltaTime;
        CheckForCollisions(velocity);
        transform.Translate(Vector3.forward * velocity);
    }

    private void CheckForCollisions(float velocity)
    {
        Ray ray = new Ray(gameObject.transform.position, gameObject.transform.forward);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction, Color.red);
        //if (Physics.Raycast(ray, out hit, vel + skin, collidable, QueryTriggerInteraction.Collide)) {
        if (Physics.Raycast(ray, out hit, velocity + skin, collidable)) {
            HitObject(hit, hit.point);
        }
    }

    private void HitObject(RaycastHit hit, Vector3 hitPoint)
    {
        if (hit.collider.gameObject.layer == bounceable) {

            audioSource.PlayOneShot(audioSource.clip);

            if (bounces >= maxBounces) {
                //DeleteBullet();
                Destroy(gameObject);
                //speed -= 9;
            } else {
                bounces++;
                gameObject.transform.forward = Vector3.Reflect((hit.point - gameObject.transform.position), hit.normal);
            }

        } else {
            if (hit.collider.gameObject.layer == Globals.PlayerLayer) {
                //hit.collider.gameObject.GetComponentInParent<Player>().TakeHit(gunDamageAmount, hit.point, -(hit.collider.transform.forward));
                hit.collider.gameObject.GetComponentInParent<Player>().TakeHit(gunDamageAmount, hit.point, transform.forward);
            }
            else if (hit.collider.gameObject.layer == Globals.EnemyLayer) {
                //hit.collider.gameObject.GetComponentInParent<Enemy>().TakeHit(gunDamageAmount, hit.point, -(hit.collider.transform.forward));
                hit.collider.gameObject.GetComponentInParent<Enemy>().TakeHit(gunDamageAmount, hit.point, transform.forward);
                try { FindObjectOfType<GameManager>().enemiesKilled++; } catch { };
            } else if(hit.collider.gameObject.layer == Globals.BulletLayer) {
                Destroy(hit.collider.gameObject);
            }
            Destroy(gameObject);
            //DeleteBullet();
        }
    }

}
