using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : Entity
{

    private Player player;
    private NavMeshAgent navAgent;
    private GunController gunController;

    public enum CurrentTask { Idle, Hunting, Aiming, Shooting, BeDead }
    private CurrentTask currentTask;

    [Header("Movement")]
    public float moveSpeed;
    public float turningSpeed;

    [Header("AI Properties")]
    [Range(1, 10)]
    public int intelligence;

<<<<<<< HEAD
    [Header("Other Attributes")]
    public LayerMask collideMask;
=======
    // Initialise values (once per script)
    void Awake() {
        
    }
>>>>>>> parent of 1a9f3de... Hate AI

    // Start is called before the first frame update
    protected override void Start() {
        base.Start();
        player = FindObjectOfType<Player>();
        navAgent = GetComponent<NavMeshAgent>();
        gunController = GetComponentInChildren<GunController>();
        navAgent.speed = moveSpeed;
        navAgent.angularSpeed = turningSpeed;
        OnDeath += OnEnemyDeath;
        currentTask = CurrentTask.Idle;

        switch (intelligence) {
            case 1:
                StartCoroutine(Attack());
                break;
            case 2:
                break;
            default: break;
        }
    }

    void OnEnemyDeath()
    {
        currentTask = CurrentTask.BeDead;
    }
    
    IEnumerator Attack()
    {
        while(player != null) {

            gunController.Aim(player.transform.position);
            Ray ray = new Ray(transform.position, player.transform.position - transform.position);
            RaycastHit hit;
            Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red);

            if (!Physics.Raycast(ray, out hit, 1000, collideMask)) {
                print("hitting player");
                //direct line of sight to player
                gunController.TriggerHeld();
            }

<<<<<<< HEAD
            yield return null;
        }
    }

#if false
    void Attack()
    {
        currentTask = CurrentTask.Shooting;
        Ray ray = new Ray(transform.position, player.transform.position - transform.position);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red);

        if (!Physics.Raycast(ray, out hit, 1000, collideMask)) {
            //direct line of sight to player
            gunController.TriggerHeld();
        } else {
            //do something
        }
    }

    private void Update()
    {
        gunController.Aim(player.transform.position);
        navAgent.SetDestination(player.transform.position);

        Attack();
        
    }

=======
>>>>>>> parent of 1a9f3de... Hate AI
    void OnEnemyDeath()
    {
        currentTask = CurrentTask.BeDead;
    }

    IEnumerator Attack()
    {
        Ray ray = new Ray(transform.position, player.transform.position - transform.position);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red);
        //if (Physics.Raycast(ray, out hit, vel + skin, collidable, QueryTriggerInteraction.Collide)) {

        if (!Physics.Raycast(ray, out hit, 1000, 9)) {
            // direct line of sight to player
            gunController.Aim(player.transform.position);
            gunController.TriggerHeld();

        }

        yield return null;
    }

    IEnumerator Chase()
    {
        navAgent.SetDestination(player.transform.position);
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        switch(intelligence) {
            case 1:
                StartCoroutine(Attack());
                break;
            case 2:
                StartCoroutine(Chase());
                StartCoroutine(Attack());
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
            case 6:
                break;
            case 7:
                break;
            case 8:
                break;
            case 9:
                break;
            case 10:
                break;
            default:
                break;
        }
    }
<<<<<<< HEAD
#endif

=======
>>>>>>> parent of 1a9f3de... Hate AI
}
