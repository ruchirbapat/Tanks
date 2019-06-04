// Dependencies
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

// Requires a NavMeshAgent component to navigate the terrain/arena
[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : Entity
{
    // Miscellaneous properties
    public bool shouldAim = true; // Does the Enemy need to aim at the Player 
    private Player player; // Reference to Player i.e. what it needs to attack and chase
    private NavMeshAgent navAgent; // Ref to Agent
    private GunController gunController; // Ref to Gun Controller
    private float halfHeight;

    [Header("Movement")]
    public float moveSpeed; // Move speed
    public float turningSpeed; // Rotation speed

    [Header("AI")]
    [Range(1, 10)]
    public int intelligence; // Intelligence 

    [Header("Other Attributes")]
    public LayerMask collideMask; // What objects can "block its view of the Player"

    // Start is called before the first frame update
    protected override void Start()
    {
        // Set up inherited properties
        base.Start();
        
        // Set up variables and initialise runtime references
        player = FindObjectOfType<Player>();
        navAgent = GetComponent<NavMeshAgent>();

        if (moveSpeed == 0)
            navAgent.enabled = false;
        else {
            navAgent.speed = moveSpeed;
            navAgent.angularSpeed = turningSpeed;
        }


        gunController = GetComponentInChildren<GunController>();

        // Calculate half the enemy tank's height (used later)
        halfHeight = GetComponent<BoxCollider>().size.y / 2;
    }

    // Update contains the Enemy AI Algorithm
    private void Update()
    {
        // Run artificial intelligence code only if the Player is still alive
        if (player != null)
        {
            // Aim at the Player
            if(shouldAim)
                gunController.Aim(player.transform.position);

            // Check for objects blocking its view of the Player
            Ray ray = new Ray(transform.position, new Vector3(player.transform.position.x, transform.position.y + halfHeight, player.transform.position.z) - new Vector3(transform.position.x, transform.position.y + halfHeight, transform.position.z));
            RaycastHit hit;

            // Is there a direct line of sight to the Player?
            bool directLineofSight = !Physics.Raycast(ray, out hit, Vector3.Distance(player.transform.position, transform.position), collideMask);

            Debug.DrawRay(ray.origin, ray.direction * Vector3.Distance(player.transform.position, transform.position), directLineofSight ? Color.green : Color.red);

            // If the Enemy can "see" the Player
            if (directLineofSight)
            {
                switch (intelligence)
                {
                    case 2:
                        // Aim at the player
                        gunController.Aim(Globals.PlayerNextPosition);
                        break;
                    default: break;
                }

                // Shoot at the Player
                gunController.Shoot();
            }
            else
            {
                // If the Player is obscured from vision, move till it is in view (and therefore 'shootable')
                if (navAgent.isActiveAndEnabled && (player != null))
                    navAgent.SetDestination(player.transform.position);
            }
        }
    }

}
