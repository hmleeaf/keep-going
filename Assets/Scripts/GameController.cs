using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField, Range(0f, 10f)] float waveIntervalMin;
    [SerializeField, Range(0f, 10f)] float waveIntervalMax;
    [SerializeField, Range(0, 100)] int obstaclesPerWaveMin;
    [SerializeField, Range(0, 100)] int obstaclesPerWaveMax;
    [SerializeField] float spawnZoneMaxXMagnitude = 10f;
    [SerializeField] GameObject obstaclePrefab;
    [SerializeField] GameObject coinPrefab;
    [SerializeField] float spawnZoneZPosition = 20f;
    [SerializeField, Range(0f, 1f), Tooltip("Chance that at the top of an obstacle column, a coin spawns")] float coinChance = 0.1f;

    float nextWaveTimer;
    int maxObstacleColumns;

    private void OnEnable()
    {
        nextWaveTimer = 0;
        maxObstacleColumns = (int)(spawnZoneMaxXMagnitude * 2 / obstaclePrefab.transform.localScale.x);
    }

    private void Update()
    {
        nextWaveTimer -= Time.deltaTime;
        if (nextWaveTimer < 0f)
        {
            SpawnWave();
            nextWaveTimer += Random.Range(waveIntervalMin, waveIntervalMax);
        }
    }

    void SpawnWave()
    {
        int obstaclesToSpawn = Random.Range(obstaclesPerWaveMin, obstaclesPerWaveMax);
        int[] obstacles = new int[maxObstacleColumns];
        for (int i = 0; i < obstaclesToSpawn; i++)
        {
            obstacles[Random.Range(0, maxObstacleColumns)]++;
        }

        float baseX = -(maxObstacleColumns * obstaclePrefab.transform.localScale.x) / 2;
        float gap = (spawnZoneMaxXMagnitude * 2 - maxObstacleColumns * obstaclePrefab.transform.localScale.x) / maxObstacleColumns;
        for (int col = 0; col < maxObstacleColumns; col++)
        {
            for (int height = 0; height < obstacles[col]; height++)
            {
                GameObject obj = Instantiate(Random.value < coinChance ? coinPrefab : obstaclePrefab);
                obj.transform.position = new Vector3(
                    baseX + col * (obstaclePrefab.transform.localScale.x + gap),
                    height * obstaclePrefab.transform.localScale.y,
                    spawnZoneZPosition
                ) + obstaclePrefab.transform.localScale / 2; // center it around the object center
            }
        }
    }
}
