using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> waves;
    [SerializeField] private Vector3 spawnPointOffset;
    [SerializeField, Range(0f, 10f)] private float spawnInterval;

    float spawnTimer = 0f;

    private void Update()
    {
        spawnTimer -= Time.deltaTime;

        while (spawnTimer < 0f)
        {
            SpawnWave();
            spawnTimer += spawnInterval;
        }
    }

    void SpawnWave()
    {
        GameObject waveObj = Instantiate(waves[Random.Range(0, waves.Count)]);
        waveObj.transform.position += spawnPointOffset;
    }
}
