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
            
            Ray ray = new Ray(transform.position, player.transform.position - transform.position);
            RaycastHit hit;
            Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red);

            if (!Physics.Raycast(ray, out hit, 1000, collideMask)) {
                print("hitting player");
                //direct line of sight to player
                gunController.Aim(player.transform.position);
                gunController.TriggerHeld();
            }

            yield return 0 ;
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

    void OnEnemyDeath()
    {
        currentTask = CurrentTask.BeDead;
    }

    IEnumerator AttackEnum(float waitTime, float turnTime)
    {
        while(player != null) {
            if(currentTask != CurrentTask.BeDead) {
                Ray ray = new Ray(transform.position, player.transform.position - transform.position);
                RaycastHit hit;
                Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red);

                if (!Physics.Raycast(ray, out hit, 1000, collideMask)) {
                    //direct line of sight to player
                    gunController.Aim(player.transform.position);
                    gunController.TriggerHeld();
                } else {
                    float turnRate = 1 / turnTime;
                    if (!reachedRot) {
                        rotPercent += (Time.deltaTime * turnRate);
                        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotPercent);
                        if (Quaternion.Angle(transform.rotation, targetRotation) < 1) {
                            reachedRot = true;
                        }
                    } else {
                        targetRotation = Random.rotation;
                        reachedRot = false;
                        yield return new WaitForSeconds(waitTime);
                    }

                }
            }

            yield return new WaitForSeconds(0.166f);
        }
    }

    IEnumerator Chase()
    {
        navAgent.SetDestination(player.transform.position);
        yield return null;
    }

    IEnumerator TurnToRandomPoint(float turnTime, float waitTime)
    {
        float turnRate = 1 / turnTime;
        if (!reachedRot) {
            rotPercent += (Time.deltaTime * turnRate);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotPercent);
            if (Quaternion.Angle(transform.rotation, targetRotation) < 1) {
                reachedRot = true;
            }
        } else {
            targetRotation = Random.rotation;
            reachedRot = false;
            yield return new WaitForSeconds(waitTime);
        }
    }
#endif

}
