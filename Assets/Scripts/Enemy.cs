using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : Entity
{
    public bool shouldAim = true;
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
    public float maxWalkDistance = 0;
    public float randomWalkTime = 1f;

    private float randomWalkTimer = 0f;
    private Vector3 finalPos;

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
        finalPos = transform.position;

#if false
        switch (intelligence)
        {
            case 1:
                StartCoroutine(Intelligence1());
                break;
            case 2:
                StartCoroutine(Intelligence2());
                break;
            case 3:
                StartCoroutine(Intelligence3());
                break;
            default: break;
        }
#endif
    }

    IEnumerator Intelligence1()
    {
        gunController.Shoot();
        yield return new WaitForSeconds(0.2f);
    }

    IEnumerator Intelligence2()
    {
        gunController.Aim(Globals.PlayerNextPosition);
        gunController.Shoot();
        yield return new WaitForSeconds(0.2f);
    }

    IEnumerator Intelligence3()
    {
        Vector3 randomDirection = Random.insideUnitSphere * maxWalkDistance;
        Vector3 nextPos = transform.position + randomDirection;
        NavMeshHit navMeshHitRef;
        NavMesh.SamplePosition(randomDirection, out navMeshHitRef, maxWalkDistance, 1);
        finalPos = navMeshHitRef.position;
        navAgent.SetDestination(finalPos);
        yield return new WaitForSeconds(0.2f);
    }

    private void Update()
    {
        if (player != null)
        {
            if(shouldAim)
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
            if (directLineofSight)
            {
                switch (intelligence)
                {
                    case 2:
                        gunController.Aim(Globals.PlayerNextPosition);
                        break;
                    case 3:
                        if (Vector3.Distance(finalPos, transform.position) <= 2f)
                        {
                            Vector3 randomDirection = Random.insideUnitSphere * maxWalkDistance;
                            Vector3 nextPos = transform.position + randomDirection;
                            nextPos.y = transform.position.y;
                            NavMeshHit navMeshHitRef;
                            NavMesh.SamplePosition(nextPos, out navMeshHitRef, maxWalkDistance, -1);
                            finalPos = navMeshHitRef.position;
                            finalPos.y = transform.position.y;
                            navAgent.SetDestination(finalPos);
                        }
                        break;
                    default: break;
                }
                gunController.Shoot();
            }
            else
            {
                if (navAgent.isActiveAndEnabled && (player != null))
                    navAgent.SetDestination(player.transform.position);
            }
        }
    }

    void OnEnemyDeath()
    {
        currentTask = CurrentTask.BeDead;
    }

}
