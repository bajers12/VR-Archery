using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyThrower : MonoBehaviour
{
    [Header("Movement")]
    public Transform stationPoint;
    public float arriveThreshold = 0.2f;

    [Header("Throw Settings")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float throwInterval = 2.0f;
    public float throwIntervalVariation = 0.5f;

    [Header("Projectile Settings")]
    public float flightTime = 3.5f;    // seconds
    public float arcHeight = 10f;      // world units
    public int damage = 1;             // how much health to remove

    [Header("Target")]
    public EnemyTarget enemyTarget;

    [Header("Animation Timing")]
    [Tooltip("Frames between animation start and actual spawn")]
    public int throwPreThrowFrames = 30;
    [Tooltip("FPS of your Throw animation clip")]
    public float throwAnimationFPS = 30f;

    // runtime
    NavMeshAgent navAgent;
    Animator     animator;
    bool         hasArrived;
    float        timeSinceLastThrow;
    float        nextThrowInterval;
    float        throwPreThrowTime;
    bool         isThrowing;

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (navAgent == null)   Debug.LogError("[EnemyThrower] Missing NavMeshAgent!");
        if (animator == null)   Debug.LogError("[EnemyThrower] Missing Animator!");
        if (stationPoint==null || firePoint==null || projectilePrefab==null || enemyTarget==null)
            Debug.LogError("[EnemyThrower] One or more references not assigned!");

        // compute seconds to wait after animation trigger
        throwPreThrowTime = throwPreThrowFrames / throwAnimationFPS;

        // start moving
        animator.SetBool("isWalking", true);
        navAgent.isStopped = false;
        navAgent.SetDestination(stationPoint.position);

        ScheduleNextThrow();
    }

    void Update()
    {
        if (!hasArrived)
        {
            // still walking → check arrival
            if (!navAgent.pathPending &&
                navAgent.remainingDistance <= Mathf.Max(navAgent.stoppingDistance, arriveThreshold))
            {
                hasArrived = true;
                navAgent.isStopped = true;
                animator.SetBool("isWalking", false);
            }
            return;
        }

        // only count if not already throwing
        if (isThrowing) return;

        timeSinceLastThrow += Time.deltaTime;
        if (timeSinceLastThrow >= nextThrowInterval)
        {
            StartCoroutine(ThrowRoutine());
        }
    }

    IEnumerator ThrowRoutine()
    {
        isThrowing = true;
        // 1) play throw animation
        animator.SetTrigger("doThrow");

        // 2) wait the pre-throw delay
        yield return new WaitForSeconds(throwPreThrowTime);

        // 3) spawn projectile
        var projGO = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        var bezier = projGO.GetComponent<BezierProjectile>();
        if (bezier != null)
            bezier.Launch(firePoint.position, enemyTarget, flightTime, arcHeight, damage);
        else
            Debug.LogError("[EnemyThrower] Projectile prefab missing BezierProjectile!");

        // 4) schedule next throw
        ScheduleNextThrow();
        isThrowing = false;
    }

    void ScheduleNextThrow()
    {
        // pick a new interval with variation
        nextThrowInterval = throwInterval 
                          + Random.Range(-throwIntervalVariation, throwIntervalVariation);
        timeSinceLastThrow = 0f;
    }

    private void OnDrawGizmosSelected()
    {
        if (stationPoint != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(stationPoint.position, arriveThreshold);
            Gizmos.DrawLine(transform.position, stationPoint.position);
        }
        if (firePoint != null && enemyTarget != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(firePoint.position, enemyTarget.transform.position);
        }
    }
}
