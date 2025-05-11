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
    private float spawnInterval = 10.0f;
    [SerializeField]
    private float spawnIntervalVariation = 5.0f;

    private float timeSinceLastSpawn = 0.0f;
    private DifficultyScaler difficultyScaler;

    public EnemyTarget enemyTarget;

    // Update is called once per frame
    private void Start()
    {
        float randomTimeOffset = Random.Range(0f, spawnInterval);
        timeSinceLastSpawn = randomTimeOffset;
        difficultyScaler = DifficultyScaler.instance;
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
        return timeSinceLastSpawn * difficultyScaler.GetDifficultyScale() >= spawnInterval ;
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
            if (i == 0) {
                Gizmos.DrawLine(transform.position, wayPoints[0].position);
            } else
            {
                Gizmos.DrawLine(wayPoints[i].position, wayPoints[i - 1].position);
            }
            if (i == wayPoints.Length - 1) Gizmos.DrawLine(wayPoints[i].position, enemyTarget.transform.position);
            Gizmos.DrawSphere(wayPoints[i].position, 0.5f);

        }
    }
}
