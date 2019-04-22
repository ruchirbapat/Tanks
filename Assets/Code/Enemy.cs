using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(BoxCollider))]

public class Enemy : Entity
{

    private Player player;
    private NavMeshAgent agent;
    public Player Player { get { return player; } }
    public NavMeshAgent Agent { get { return agent; } }
    public enum Action { Dead, Idle, Attacking, Chasing };

    [Header("General Properties")]
    public float moveSpeed;
    public float turnSpeed;
    [Range(0.2f, 0.5f)]
    public float scanRate;
    public Action currentAction = Action.Idle;

    [Header("Attack Properties")]
    public float attackRange = 0.5f;
    public float attackPause = 1;
    public float attackDelay = 1;

    private static float halfExtent = 0.5f;
    private float playerHalfExtent;

    private GunController gunController;

    public ParticleSystem deathParticleSystem;

    public float radius;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        playerHalfExtent = 0.5f;
    }

    protected override void Start()
    {
        base.Start();
        OnDeath += OnEnemyDeath;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        agent.angularSpeed = turnSpeed;

        gunController = GetComponentInChildren<GunController>();

        deathParticleSystem.GetComponent<Renderer>().sharedMaterial.color = gameObject.GetComponent<Renderer>().material.color;

        if (!player.dead) { StartCoroutine(Chase()); currentAction = Action.Chasing; } else { agent.destination = Globals.RandomPointOnCircle(new Vector3(0, transform.position.y, 0), 50); }
    }

    void OnEnemyDeath() { currentAction = Action.Dead; }

    void Update()
    {
        if (!player.dead) { if (Time.time > attackDelay) { StartCoroutine(Scan()); } gameObject.transform.LookAt(Globals.PlayerPosition); } else { print("Player is dead."); agent.destination = Globals.RandomPointOnCircle(new Vector3(0, transform.position.y, 0), radius); }
    }

    IEnumerator Chase()
    {
        while (!player.dead) {
            if (currentAction == Action.Chasing) {
                Vector3 chaseDirection = (player.transform.position - gameObject.transform.position).normalized;

                Vector3 chasePosition = player.transform.position - chaseDirection * ((halfExtent + playerHalfExtent) + attackRange);
                //Vector3 chasePosition = Globals.RandomPointOnCircle(player.transform.position, 10);
                //Debug.DrawLine(player.transform.position, chasePosition, Color.red);

                if (!dead) { agent.SetDestination(chasePosition); }
            }

            yield return new WaitForSeconds(scanRate);
        }
    }

    IEnumerator Scan()
    {
        attackDelay = Time.time + attackPause;

        if (!player.dead) {
            Collider[] rangedColliders = Physics.OverlapSphere(transform.position, Mathf.Pow(attackRange + 0.5f + playerHalfExtent, 2));
            foreach (Collider c in rangedColliders) {
                if (c.gameObject.GetComponent<Player>() != null) { Attack(); break; } //StartCoroutine(AttackPlayer()); //Attack the player
            }
            StartCoroutine(Chase());
        }
        yield return new WaitForSeconds(scanRate);
    }

    void Attack()
    {

        gameObject.transform.LookAt(Globals.PlayerNextPosition);

        currentAction = Action.Attacking;

        agent.enabled = false;

        //Vector3 direction = (player.transform.position - gameObject.transform.position).normalized;

        gameObject.transform.LookAt(player.transform.position);

        gunController.TriggerHeld();

        currentAction = Action.Chasing;

        agent.enabled = true;
    }

    public override void TakeHit(float damageAmount, Vector3 point, Vector3 direction)
    {
        print("Took hit");
        base.Damage(damageAmount);
        if (currentHealth <= 0) {
            print("died!");
            Destroy(Instantiate(deathParticleSystem.gameObject, point, Quaternion.FromToRotation(Vector3.forward, direction)) as GameObject, deathParticleSystem.main.startLifetimeMultiplier); print("Created particle system.");
        }
    }
}