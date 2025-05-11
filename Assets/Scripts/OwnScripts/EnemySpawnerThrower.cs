using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerThrower : MonoBehaviour
{
    [Header("Prefab & Spawn Settings")]
    [Tooltip("Prefab with the EnemyThrower component and a NavMeshAgent")]
    [SerializeField] private GameObject enemyThrowerPrefab;
    [Tooltip("Where enemies appear in the scene")]
    [SerializeField] private Transform spawnPoint;
    [Tooltip("Base seconds between spawns")]
    [SerializeField] private float spawnInterval = 10.0f;
    [Tooltip("± random variation on spawn interval")]
    [SerializeField] private float spawnIntervalVariation = 5.0f;

    [Header("Station Assignments")]
    [Tooltip("Fixed positions where throwers should walk to")]
    [SerializeField] private Transform[] stationPoints;

    [Header("Target")]
    [Tooltip("The stationary target the throwers will aim at")]
    [SerializeField] private EnemyTarget enemyTarget;

    private float timeSinceLastSpawn = 0f;
    private DifficultyScaler difficultyScaler;
    private int nextStationIndex = 0;

    private void Start()
    {
        // randomize initial spawn offset so they don't all come at once
        timeSinceLastSpawn = Random.Range(0f, spawnInterval);
        difficultyScaler = DifficultyScaler.instance;
    }

    private void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;

        // difficultyScaler can speed up spawn rate over time
        if (timeSinceLastSpawn * difficultyScaler.GetDifficultyScale() >= spawnInterval)
        {
            SpawnThrower();
            // reset timer plus some random variation
            timeSinceLastSpawn = timeSinceLastSpawn - spawnInterval 
                                 + Random.Range(-spawnIntervalVariation, spawnIntervalVariation);
        }
    }

    private void SpawnThrower()
    {
        // Instantiate the thrower prefab
        GameObject go = Instantiate(enemyThrowerPrefab, spawnPoint.position, spawnPoint.rotation);
        EnemyThrower thrower = go.GetComponent<EnemyThrower>();
        Transform station = stationPoints[nextStationIndex];
        thrower.stationPoint = station;

        // Assign the target it should lob at
        thrower.enemyTarget = enemyTarget;
        if (thrower == null)
        {
            Debug.LogError("[EnemySpawner] Prefab missing EnemyThrower component!");
            return;
        }

        // Assign its station point (round-robin through the array)


        // Advance to the next station for the following spawn
        nextStationIndex = (nextStationIndex + 1) % stationPoints.Length;
    }

    private void OnDrawGizmos()
    {
        // visualize stations from the spawner in the editor
        if (stationPoints != null)
        {
            Gizmos.color = Color.cyan;
            foreach (var st in stationPoints)
            {
                if (st != null)
                {
                    Gizmos.DrawWireSphere(st.position, 0.3f);
                    Gizmos.DrawLine(spawnPoint.position, st.position);
                }
            }
        }

        if (spawnPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(spawnPoint.position, 0.2f);
        }
    }
}
