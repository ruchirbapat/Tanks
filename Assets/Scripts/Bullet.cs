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

    private void Awake()
    {
        bounceable = GameObject.FindGameObjectsWithTag("Environment")[0].gameObject.layer;
    }

    private void Start()
    {
        Destroy(gameObject, lifetime);

        /*Collider[] collisions = Physics.OverlapSphere(transform.position, skin, collidable);
        if (collisions.Length > 0)
        { HitObject(collisions[0], gameObject.transform.position); }*/
        //print("Bullet created.");
        GetComponent<TrailRenderer>().material.color = trailColour;
        GetComponent<TrailRenderer>().time = trailDuration;
    }

    private void Update()
    {
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

    /*private void HitObject(Collider collider, Vector3 hitPoint) {
        if (collider.gameObject.GetComponent<Player>() != null)
        {
            collider.gameObject.GetComponent<Player>().TakeHit(damage, hitPoint);
        }
        else if (collider.gameObject.GetComponent<Enemy>() != null)
        {
            collider.gameObject.GetComponent<Enemy>().Damage(damage);
        }

        Destroy(gameObject);
    }*/

    private void HitObject(RaycastHit hit)
    {
        if (hit.collider.gameObject.layer == bounceable) {

            if (bounces >= maxBounces) {
                Destroy(this);
                //speed -= 9;
            } else {
                bounces++;
                gameObject.transform.forward = Vector3.Reflect((hit.point - gameObject.transform.position), hit.normal);
            }

        } else {
            if (hit.collider.gameObject.GetComponent<Player>() != null) {
                hit.collider.gameObject.GetComponent<Player>().TakeHit(gunDamageAmount, hit.point, -(hit.collider.transform.forward));
            } else if (hit.collider.gameObject.GetComponent<Enemy>() != null) {
                hit.collider.gameObject.GetComponent<Enemy>().TakeHit(gunDamageAmount, hit.point, -(hit.collider.transform.forward));
            }
            Destroy(gameObject);
        }
    }

}
