using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Player : Entity
{

    private GunController gunController;
    private Rigidbody attachedRigidbody;
    public Camera mainCamera;
    private Vector3 velocity;
    public Vector3 Velocity { get { return velocity; } }
    //private Vector3 angle;
    public float movementSpeed;
    public float turnSpeed;
    private float movementSmoothSpeed;

    private Quaternion targetRotation;
    private Vector3 latestInput;

    private float smoothInputMagnitude;
    private float smoothMoveVelocity;
    private float angle;
    private bool rotating = false;

    public GameObject body;
    public LayerMask mouseMask;

    protected void Awake()
    {
        targetRotation = transform.rotation;
    }

    protected override void Start()
    {
        base.Start();
        velocity = Vector3.zero;
        attachedRigidbody = GetComponent<Rigidbody>();
        gunController = GetComponentInChildren<GunController>();
        Globals.RandomPointOnCircle(transform.position, 1);
        //angle = Vector3.zero;
    }

    IEnumerator Turn(float turnTime, Vector3 rotTo)
    {
        float percent = 0;
        float turnSpeed = 1 / turnTime;
        Quaternion targetRotation = Quaternion.LookRotation(rotTo);
        while (percent < 1) {
            percent += Time.deltaTime * turnSpeed;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, percent);
            yield return null;
        }
        rotating = false;
    }

    private void Update()
    {

        latestInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        //smoothInputMagnitude = Mathf.SmoothDamp(smoothInputMagnitude, latestInput.magnitude, ref smoothMoveVelocity, movementSmoothSpeed);
        velocity = latestInput * movementSpeed;// * smoothInputMagnitude;

        float targetAngle = Mathf.Atan2(latestInput.x, latestInput.z) * Mathf.Rad2Deg;
        angle = Mathf.LerpAngle(angle, targetAngle, Time.deltaTime * turnSpeed * latestInput.magnitude);


        // credits to sebastian lague for mouse code
        Ray mouseRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        float distanceToIntersection;

        // intersect with a plane at the same level as the gun to fix a weird parallax issue
        Plane eyeLevelIntersectionPlane = new Plane(Vector3.up, Vector3.up * (gunController.Gun.transform.position.y - transform.position.y));

        if (eyeLevelIntersectionPlane.Raycast(mouseRay, out distanceToIntersection))
        {
            Vector3 pt = mouseRay.GetPoint(distanceToIntersection); // last line of sebastian lague code for mouse ray intersection trick

            gunController.Aim(pt);
        }

        Debug.DrawRay(mouseRay.origin, mouseRay.direction * 1000, Color.red);

        Globals.PlayerPosition = gameObject.transform.position;
        Globals.PlayerNextPosition = (gameObject.transform.position + (velocity * Time.fixedDeltaTime)) + (velocity * Time.fixedDeltaTime);

        if (Input.GetMouseButton(0)) {
            gunController.Shoot();
        }
        if(Input.GetMouseButtonUp(0)) {
            gunController.ReleaseTrigger();
        }

#if false
        if (Input.GetMouseButtonDown(0)) { gunController.TriggerHeld(); }

        if (Input.GetMouseButtonUp(0)) { gunController.TriggerReleased(); }

        if (Input.GetKeyDown(KeyCode.R)) { gunController.Reload(); }
#endif 

    }

    private void FixedUpdate()
    {
           attachedRigidbody.MovePosition(attachedRigidbody.position + velocity * Time.fixedDeltaTime);
           attachedRigidbody.MoveRotation(Quaternion.Euler(Vector3.up * angle));
    }

    //Overrided TakeHit incase I decide Player should have particle system too
    /*public override void TakeHit(float damageAmount, Vector3 point, Vector3 direction)
    {
        Destroy(Instantiate(deathParticleSystem.gameObject, point, Quaternion.FromToRotation(Vector3.forward, direction)) as GameObject, deathParticleSystem.main.startLifetimeMultiplier);
        base.TakeHit(damageAmount, point, direction);
    }*/
}