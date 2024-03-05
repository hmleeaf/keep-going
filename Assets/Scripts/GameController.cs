using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] PlayerController playerController;

    [Header("Smoothing")]
    [SerializeField, Range(0f, 1f)] float smoothingFactor = 0.5f;

    [Header("Pre-generation")]
    [SerializeField] float pregenDistance = 30f;

    [Header("Waves")]
    [SerializeField] List<GameObject> waves;
    [SerializeField] float wavesGap = 10f;
    [SerializeField] Transform wavesContainer;

    [Header("Ground")]
    [SerializeField] GameObject groundPrefab;
    [SerializeField] float groundLength;
    [SerializeField] Transform groundsContainer;

    [Header("Chaser")]
    [SerializeField] GameObject chaser;
    [SerializeField] float chaserNaturalProgressionRate = 5f;

    float progress = 0;
    float chaserProgress = 0;
    Vector3 initialCameraPosition;
    Vector3 initialChaserPosition;
    int wavesSpawned = 0;
    int groundsSpawned = 0;

    private void Start()
    {
        initialCameraPosition = Camera.main.transform.position;
        initialChaserPosition = chaser.transform.position;
    }

    private void Update()
    {
        CalcProgress();

        UpdateCamZ();
        GenerateWave();
        GenerateGround();
        UpdateChaser();
    }

    void CalcProgress()
    {
        progress = Mathf.Max(progress, playerController.transform.position.z);
    }

    void UpdateCamZ()
    {
        Vector3 newCamPos = initialCameraPosition + Vector3.forward * progress;
        float distance = Vector3.Distance(Camera.main.transform.position, newCamPos);
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, newCamPos, 1/distance);
    }

    void GenerateWave()
    {
        while (wavesSpawned * wavesGap < progress + pregenDistance)
        {
            SpawnPrefab(waves[Random.Range(0, waves.Count)], ++wavesSpawned * wavesGap, wavesContainer);
        }
    }

    void GenerateGround()
    {
        while (groundsSpawned * groundLength < progress + pregenDistance)
        {
            SpawnPrefab(groundPrefab, ++groundsSpawned * groundLength, groundsContainer);
        }
    }

    void SpawnPrefab(GameObject prefab, float zOffset, Transform parent)
    {
        GameObject obj = Instantiate(prefab);
        obj.transform.position += Vector3.forward * zOffset;
        if (parent != null)
        {
            obj.transform.SetParent(parent);
        }
    }

    void UpdateChaser()
    {
        Vector3 newChaserPos = initialChaserPosition + Vector3.forward * progress;
        chaser.transform.position += Vector3.forward * chaserNaturalProgressionRate * Time.deltaTime;
        if (chaser.transform.position.z < newChaserPos.z)
        {
            chaser.transform.position = newChaserPos;
        }

    }

    public void TransitionToAbberation()
    {
        Debug.Log("Transitioning");
    }
}
