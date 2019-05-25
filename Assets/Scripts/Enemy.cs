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
    private float halfHeight;

    public enum CurrentTask { Idle, Hunting, Aiming, Shooting, BeDead }
    private CurrentTask currentTask;

    [Header("Movement")]
    public float moveSpeed;
    public float turningSpeed;

    [Header("AI")]
    [Range(1, 10)]
    public int intelligence;

    [Header("Other Attributes")]
    public LayerMask collideMask;
    public float maxCloseness;

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
        halfHeight = GetComponent<BoxCollider>().size.y / 2;
    }

    private void Update()
    {
        gunController.Aim(player.transform.position);
        Ray ray = new Ray(transform.position, new Vector3(player.transform.position.x, transform.position.y + halfHeight, player.transform.position.z) - new Vector3(transform.position.x, transform.position.y + halfHeight, transform.position.z));
        RaycastHit hit;

        bool directLineofSight = !Physics.Raycast(ray, out hit, Vector3.Distance(player.transform.position, transform.position), collideMask);

        Debug.DrawRay(ray.origin, ray.direction * Vector3.Distance(player.transform.position, transform.position), directLineofSight ? Color.green : Color.red);
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
        if (directLineofSight) {
            switch(intelligence) {
                case 2:
                    gunController.Aim(Globals.PlayerNextPosition);
                    break;
                default: break;
            }

            gunController.Shoot();
        } else {
            if (navAgent.isActiveAndEnabled && (player != null))
                navAgent.SetDestination(player.transform.position);
        }
    }

    void OnEnemyDeath()
    {
        currentTask = CurrentTask.BeDead;
    }

}
