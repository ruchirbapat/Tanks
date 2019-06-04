// Dependencies
using UnityEngine;
using System.Collections;

// Requires a Rigidbody component else it won't work
[RequireComponent(typeof(Rigidbody))]
public class Player : Entity // Inherits from the Entity class
{
    // References to attached componenents
    private GunController gunController;
    private Rigidbody attachedRigidbody;

    // Reference to main rendering camera
    public Camera mainCamera;

    // Current velocity 
    private Vector3 velocity;
    
    // Getter and setter for velocity
    public Vector3 Velocity { get { return velocity; } }

    // Movement properties
    public float movementSpeed;
    public float turnSpeed;

    // Latest harware input from Player
    private Vector3 latestInput;

    // Miscellaneous properties
    private float angle;
    
    // Reference to base of mesh
    public GameObject body;

    // Reference to the crosshair
    public GameObject crosshair;

    // Unity Start function called once per GameObject
    protected override void Start()
    {
        // Call inherited Entity start function first to set up shared properties
        base.Start();

        // Set up variables and find runtime references to objects
        velocity = Vector3.zero;
        attachedRigidbody = GetComponent<Rigidbody>();
        gunController = GetComponentInChildren<GunController>();
        mainCamera = Camera.main;
        
        // Try catch statement for finding the crosshair
        try {
            crosshair = GameObject.FindGameObjectWithTag("Crosshair");
            Cursor.visible = false;
        } catch { };
    }

    // Shows the default cursor when the player dies
    void OnDestroy()
    {
        Cursor.visible = true;
    }

    // Update function called once per frame
    private void Update()
    {
        // Get input from Player, then store it in a normalised vector (i.e. of UNIT distance)
        latestInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        // Multiply the velocity by the movement speed
        velocity = latestInput * movementSpeed;

        // Rotate player to match direction of movement
        float targetAngle = Mathf.Atan2(latestInput.x, latestInput.z) * Mathf.Rad2Deg;
        angle = Mathf.LerpAngle(angle, targetAngle, Time.deltaTime * turnSpeed * latestInput.magnitude);

        // Credits to Sebastian Lague for his code on Mouse Ray Plane Intersection
        Ray mouseRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        float distanceToIntersection;

        // Intersect mouse ray with a Plane to fix parallax issue
        Plane eyeLevelIntersectionPlane = new Plane(Vector3.up, Vector3.up * (gunController.Gun.transform.position.y));

        if (eyeLevelIntersectionPlane.Raycast(mouseRay, out distanceToIntersection))
        {
            Vector3 pt = mouseRay.GetPoint(distanceToIntersection); // Last line of Seb. Lague code 

            // Set the crosshair position and colour, if it is not equal to null
            if (crosshair)
            {
                // Move crosshair
                crosshair.transform.position = pt;
                crosshair.GetComponent<CrosshairAnim>().MatchTargets(mouseRay);
            }

            // Aims the gun at the crosshair
            gunController.Aim(pt);
        }

        // Draw a debug ray from the camera outwards, in the direction of the mouse. This is only visible in the Scene view within Unity
        Debug.DrawRay(mouseRay.origin, mouseRay.direction * 1000, Color.red);

        // Update the Globals Information Datastore with the Player's updated position for the next frame
        Globals.PlayerPosition = gameObject.transform.position;
        Globals.PlayerNextPosition = (gameObject.transform.position + (velocity * Time.fixedDeltaTime)) + (velocity * Time.fixedDeltaTime);

        // Shoot the gun
        if (Input.GetMouseButton(0)) {
            gunController.Shoot();
        }

        // Release trigger callback for burst mode firing
        if(Input.GetMouseButtonUp(0)) {
            gunController.ReleaseTrigger();
        }
    }

    // Increments player's position by their velocity
    private void FixedUpdate()
    {
        // Update player position
        attachedRigidbody.MovePosition(attachedRigidbody.position + velocity * Time.fixedDeltaTime);

        // Update player rotation
        body.transform.rotation = (Quaternion.Euler(Vector3.up * angle));
    }

    // Overload the inherited Die() function (see Entity class for super)
    public override void Die(Vector3 hitDirection)
    {
        // Reduce how many lives the player has left when the player dies
        try {
            FindObjectOfType<GameManager>().playerLivesLeft--;
        } catch { };
        base.Die(hitDirection);
    }

}