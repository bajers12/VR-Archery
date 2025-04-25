using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private SampleEnemy EnemyPrefab;
    [SerializeField]
    private Transform[] wayPoints;
    [SerializeField]
    private Transform spawnPoint;
    [SerializeField]
    private float spawnInterval = 5.0f;
    [SerializeField]
    private float spawnIntervalVariation = 2.0f;

    private float timeSinceLastSpawn = 0.0f;

    public EnemyTarget enemyTarget;

    // Update is called once per frame
    private void Start()
    {
        float randomTimeOffset = Random.Range(0f, spawnInterval);
        timeSinceLastSpawn = randomTimeOffset;
    }
    void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;
        if(shouldSpawnEnemy())
        {
            spawnEnemyAndSetPath();
            timeSinceLastSpawn = timeSinceLastSpawn - spawnInterval + Random.Range(-spawnIntervalVariation, spawnIntervalVariation);
        }
    }

    bool shouldSpawnEnemy()
    {
        return timeSinceLastSpawn >= spawnInterval;
    }

    void spawnEnemyAndSetPath()
    {
        SampleEnemy newEnemy = Instantiate<SampleEnemy>(EnemyPrefab, spawnPoint.position, spawnPoint.rotation);
        newEnemy.wayPoints = wayPoints;
        newEnemy.enemyTarget = enemyTarget;
    }

    private void OnDrawGizmos()
    {
        for(int i = 0; i < wayPoints.Length; i++)
        {
            if (i == 0) Gizmos.DrawLine(transform.position, wayPoints[0].position);
            if (i == wayPoints.Length - 1) Gizmos.DrawLine(wayPoints[i].position, enemyTarget.transform.position);
            Gizmos.DrawLine(wayPoints[i].position, wayPoints[i].position);
            Gizmos.DrawSphere(wayPoints[i].position, 0.5f);
        }
    }
}
