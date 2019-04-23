using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(GunController))]
public class Player : Entity
{

    private GunController gunController;
    private Rigidbody attachedRigidbody;
    public Camera mainCamera;
    private Vector3 velocity;
    public Vector3 Velocity { get { return velocity; } }
    //private Vector3 angle;
    public float movementSpeed;

    private void Awake()
    {
        velocity = Vector3.zero;
        attachedRigidbody = GetComponent<Rigidbody>();
        gunController = GetComponentInChildren<GunController>();
        Globals.RandomPointOnCircle(transform.position, 1);
        //angle = Vector3.zero;
    }

    private void Update()
    {
        Vector3 latestInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        velocity = latestInput * movementSpeed;
        Globals.PlayerPosition = gameObject.transform.position;
        Globals.PlayerNextPosition = gameObject.transform.position + velocity;

        if (latestInput != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(latestInput);

        
        Ray mouseRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        float distanceToIntersection;

        // intersect with a plane at the same level as the gun to fix a weird parallax issue
        Plane eyeLevelIntersectionPlane = new Plane(Vector3.up, Vector3.up * (gunController.Gun.transform.position.y - transform.position.y));

        Debug.DrawRay(mouseRay.origin, mouseRay.direction, Color.red);

        if (eyeLevelIntersectionPlane.Raycast(mouseRay, out distanceToIntersection)) {
            Vector3 hit = mouseRay.GetPoint(distanceToIntersection);
            gunController.Aim(hit);
        }

        if (Input.GetMouseButton(0)) { gunController.TriggerHeld(); }

        if (Input.GetMouseButtonUp(0)) { gunController.TriggerReleased(); }

        if (Input.GetKeyDown(KeyCode.R)) { gunController.Reload(); }
    }

    private void FixedUpdate()
    {
        attachedRigidbody.MovePosition(attachedRigidbody.position + velocity * Time.fixedDeltaTime);
    }

    //Overrided TakeHit incase I decide Player should have particle system too
    /*public override void TakeHit(float damageAmount, Vector3 point, Vector3 direction)
    {
        Destroy(Instantiate(deathParticleSystem.gameObject, point, Quaternion.FromToRotation(Vector3.forward, direction)) as GameObject, deathParticleSystem.main.startLifetimeMultiplier);
        base.TakeHit(damageAmount, point, direction);
    }*/
}