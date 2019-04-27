using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : Entity
{

    private Player player;
    private NavMeshAgent navAgent;
    private GunController gunController;
    private Quaternion targetRotation;
    private bool reachedRot = true;
    private float rotPercent = 0;

    public enum CurrentTask { Idle, Hunting, Aiming, Shooting, BeDead }
    private CurrentTask currentTask;

    [Header("Movement")]
    public float moveSpeed;
    public float turningSpeed;

    [Header("AI")]
    [Range(1, 10)]
    public int intelligence;

    int[] cantMove = new int[] { 1, 2 };
    int[] shootDirectly = new int[] { 1, 2, 3, 4 };
    int[] shootNext = new int[] { 5, 6 };
    int[] shoot1Bounce = new int[] { 7, 8 };
    int[] shoot2Bounce = new int[] { 9, 10 };

    [Header("Other Attributes")]
    public LayerMask collideMask;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        player = FindObjectOfType<Player>();
        navAgent = GetComponent<NavMeshAgent>();
        if (moveSpeed == 0)
            navAgent.enabled = false;
        else {
            navAgent.speed = moveSpeed;
            navAgent.angularSpeed = turningSpeed;
        }
        gunController = GetComponentInChildren<GunController>();
        OnDeath += OnEnemyDeath;
        currentTask = CurrentTask.Idle;
    }

    private void Update()
    {
        gunController.Aim(player.transform.position);
        Ray ray = new Ray(transform.position, player.transform.position - transform.position);
        RaycastHit hit;

        bool hitsomething = Physics.Raycast(ray, out hit, Vector3.Distance(player.transform.position, transform.position), collideMask);
        if (hitsomething) { 
            print(hit.collider.gameObject.layer);
    }

        Debug.DrawRay(ray.origin, ray.direction * Vector3.Distance(player.transform.position, transform.position), hitsomething ? Color.green : Color.red);
#if false
        if(!directLineOfSightPossible && !cantMove.Contains(intelligence)) {
            //chase
        }
        if(cantMove.Contains(intelligence)) {
            switch(intelligence) {
                case 1:
                    break;
                case 2:
                    break;
            }
        } else {
            navAgent.SetDestination(player.transform.position);
        }

        switch (intelligence) {
            case 1:
                gunController.TriggerHeld();
                break;
            case 2:
                gunController.Aim(Globals.PlayerNextPosition);
                break;
            default: break;
        }

#endif
        if (!hitsomething) {
            //direct line of sight to player
            switch(intelligence) {
                case 1:
                    gunController.TriggerHeld();
                    break;
                case 2:
                    gunController.Aim(Globals.PlayerNextPosition);
                    gunController.TriggerHeld();
                    break;
                default: break;
            }

        } else {
            navAgent.SetDestination(player.transform.position);
        }
    }

    void OnEnemyDeath()
    {
        currentTask = CurrentTask.BeDead;
    }

}
