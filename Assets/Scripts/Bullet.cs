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


    private void Awake()
    {
        bounceable = GameObject.FindGameObjectsWithTag("Environment")[0].gameObject.layer;
    }
    
    void DeleteBullet()
    {
        Destroy(gameObject);
        Camera.main.GetComponent<CameraShake>().Shake();
    }

    private void Start()
    {
        //Destroy(gameObject, lifetime);

        instantiateTime = Time.time;
        deathTime = instantiateTime + (lifetime);

        GetComponent<TrailRenderer>().time = trailDuration;
    }

    private void Update()
    {
        if (Time.time > deathTime)
            DeleteBullet();

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
            //HitObject(hit.collider, hit.point);
            HitObject(hit);
        }
    }

    private void HitObject(RaycastHit hit)
    {
        if (hit.collider.gameObject.layer == bounceable) {

            if (bounces >= maxBounces) {
                DeleteBullet();
                //speed -= 9;
            } else {
                bounces++;
                gameObject.transform.forward = Vector3.Reflect((hit.point - gameObject.transform.position), hit.normal);
            }

        } else {
            if (hit.collider.gameObject.layer == Globals.PlayerLayer) {
                hit.collider.gameObject.GetComponentInParent<Player>().TakeHit(gunDamageAmount, hit.point, -(hit.collider.transform.forward));
            } else if (hit.collider.gameObject.layer == Globals.EnemyLayer) {
                hit.collider.gameObject.GetComponentInParent<Enemy>().TakeHit(gunDamageAmount, hit.point, -(hit.collider.transform.forward));
            }
            DeleteBullet();
        }
    }

}
