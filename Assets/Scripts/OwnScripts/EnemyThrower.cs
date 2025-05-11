using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyThrower : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("Where this thrower should walk to")]
    public Transform stationPoint;
    [Tooltip("How close is 'arrived'")]
    public float arriveThreshold = 0.2f;

    [Header("Throw Settings")]
    [Tooltip("Projectile prefab (needs Rigidbody & Collider)")]
    public GameObject projectilePrefab;
    [Tooltip("Where the projectile spawns")]
    public Transform firePoint;
    [Tooltip("Initial muzzle speed")]
    public float launchSpeed = 15f;
    [Tooltip("Base seconds between throws")]
    public float throwInterval = 2.0f;
    [Tooltip("± random variation on interval")]
    public float throwIntervalVariation = 0.5f;

    [Header("Target")]
    [Tooltip("The stationary target to hit")]
    [SerializeField] public EnemyTarget enemyTarget;

    [Header("Projectile Settings")]
    public float flightTime = 3.5f;    // seconds
    public float arcHeight = 10f;      // world units
    public int damage = 1;       // how much health to remove


    private NavMeshAgent navAgent;
    private bool hasArrived = false;
    private float timeSinceLastThrow = 0f;

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        if (navAgent == null)
            Debug.LogError("[EnemyThrower] Missing NavMeshAgent!");
        if (stationPoint == null || firePoint == null || projectilePrefab == null || enemyTarget == null)
            Debug.LogError("[EnemyThrower] One or more required references are not assigned!");

        // Begin walking toward our station
        navAgent.isStopped = false;
        navAgent.SetDestination(stationPoint.position);

        // Start throw timer at a random offset so they don't all fire in unison
        timeSinceLastThrow = Random.Range(0f, throwInterval);
    }

    void Update()
    {
        if (!hasArrived)
        {
            // Check arrival
            if (!navAgent.pathPending &&
                navAgent.remainingDistance <= Mathf.Max(navAgent.stoppingDistance, arriveThreshold))
            {
                ArriveAtStation();
            }
        }
        else
        {
            // Throw loop
            timeSinceLastThrow += Time.deltaTime;
            if (timeSinceLastThrow >= throwInterval)
            {
                ThrowProjectile();
                // reset timer with variation
                timeSinceLastThrow = timeSinceLastThrow - throwInterval
                                     + Random.Range(-throwIntervalVariation, throwIntervalVariation);
            }
        }
    }

    void ArriveAtStation()
    {
        hasArrived = true;
        navAgent.isStopped = true;
        // Optionally throw immediately:
        // ThrowProjectile();
    }

    void ThrowProjectile()
    {
        GameObject projGO = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        var bezier = projGO.GetComponent<BezierProjectile>();
        if (bezier == null)
        {
            Debug.LogError("Projectile prefab missing BezierProjectile component!");
            Destroy(projGO);
            return;
        }

        // Parameters you can tweak per-throw:

        bezier.Launch(
            firePoint.position,
            enemyTarget,      // your EnemyTarget instance
            flightTime,
            arcHeight,
            damage
        );
    }



    // Computes the high-arc initial velocity so the projectile lands on the target
    bool TryCalculateLaunchVelocity(out Vector3 result)
    {
        Vector3 delta = enemyTarget.transform.position - firePoint.position;
        float g       = Physics.gravity.y;            // negative
        float v2      = launchSpeed * launchSpeed;

        // split into horizontal and vertical components
        float y = delta.y;
        delta.y = 0;
        float x = delta.magnitude;

        // check discriminant of the trajectory equation
        float underRoot = v2 * v2 - g * (g * x * x + 2 * y * v2);
        if (underRoot < 0f)
        {
            result = Vector3.zero;
            return false;
        }

        float root    = Mathf.Sqrt(underRoot);
        float highAng = Mathf.Atan2(v2 + root, -g * x);

        Vector3 dir = delta.normalized;  
        // rotate horizontal dir upward by highAng around the perpendicular axis
        Quaternion rot = Quaternion.AngleAxis(highAng * Mathf.Rad2Deg,
                                               Vector3.Cross(dir, Vector3.up));
        result = rot * dir * launchSpeed;
        return true;
    }

    // Editor gizmos to visualize station and firing line
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
