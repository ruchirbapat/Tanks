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
        velocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized * movementSpeed;
        Globals.PlayerPosition = gameObject.transform.position;
        Globals.PlayerNextPosition = gameObject.transform.position + velocity;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, Vector3.up * gunController.barrel.position.y);

        float rayLength;
        if (plane.Raycast(ray, out rayLength)) {
            Vector3 point = ray.GetPoint(rayLength);

            //Buggy Trigonometry Code for gun-to-mouse position angle adjustment
            /*angle = Mathf.Acos((Mathf.Abs(point.x - gunController.barrel.position.z)) / (Vector3.Distance(new Vector3(point.x, 0, point.z), new Vector3(gunController.barrel.position.x, 0, gunController.barrel.position.z)))) * Mathf.Rad2Deg //Calculate angle;  
            Debug.Log("Angle: " + angle.ToString()); //Debug value
            gameObject.transform.LookAt(new Vector3(point.x + angle, transform.position.y, point.z + angle)); //Look*/
            if ((new Vector2(point.x, point.z) - new Vector2(transform.position.x, transform.position.z)).sqrMagnitude > 2) {
                gunController.Aim(point);
            }
            gameObject.transform.LookAt(new Vector3(point.x, transform.position.y, point.z));

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