using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SampleEnemy : MonoBehaviour
{
    private int wayPointCount;
    public NavMeshAgent navAgent;
    public Transform[] wayPoints;

    public float AttackRange = 2.0f;
    public int damage = 1;

    public int passedWayPoints = 0;
    public EnemyTarget enemyTarget;

    private void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();

        passedWayPoints = 0;
        wayPointCount = wayPoints.Length;

        if(wayPointCount > 0)
        {
            navAgent.SetDestination(wayPoints[0].transform.position);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(InRangeOfTarget())
        {
            AttackTarget();
            return;
        }

        if(EnemyReachedWaypoint() && !navAgent.pathPending)
        {
            passedWayPoints++;
            SetNextDestination();
        }


       
    }

    void AttackTarget()
    {
        //Todo more stuff
        enemyTarget.GetHit(damage);
        gameObject.SetActive(false);
    }

    bool InRangeOfTarget()
    {
        return Vector3.Distance(enemyTarget.transform.position, transform.position) < AttackRange;
    }

    private void SetNextDestination()
    {
        if(passedWayPoints < wayPointCount)
        {
            bool suc = navAgent.SetDestination(wayPoints[passedWayPoints].transform.position);
        } else if(passedWayPoints == wayPointCount)
        {
            navAgent.SetDestination(enemyTarget.transform.position);
        }
    }

    private bool EnemyReachedWaypoint()
    {
        return navAgent.remainingDistance < navAgent.stoppingDistance;
    }
}
