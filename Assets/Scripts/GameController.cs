using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEditor.AI;
using UnityEngine;
using static GameController;

public class GameController : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] PlayerController playerController;

    [Header("Camera")]
    [SerializeField] GameObject HyruleCamera;
    [SerializeField] GameObject LoruleCamera;

    [Header("Smoothing")]
    [SerializeField, Range(0f, 1f)] float smoothingFactor = 0.5f;

    [Header("Pre-generation")]
    [SerializeField] float pregenDistance = 30f;

    [Header("Waves")]
    [SerializeField] List<GameObject> wavesPrefabs;
    [SerializeField] float wavesGap = 10f;
    [SerializeField] Transform wavesContainer;

    [Header("Ground")]
    [SerializeField] GameObject groundPrefab;
    [SerializeField] float groundLength;
    [SerializeField] Transform groundsContainer;

    [Header("Chaser")]
    [SerializeField] GameObject chaser;
    [SerializeField] float chaserNaturalProgressionRate = 5f;

    [Header("AI")]
    [SerializeField] NavMeshSurface navMeshSurface;

    [Header("Enemy")]
    [SerializeField] GameObject enemyPrefab;
    [SerializeField, Range(0f, 1f)] float enemyChance = 0.25f;

    float progress = 0;
    Vector3 initialHyruleCameraPos;
    Vector3 initialLoruleCameraPos;
    Vector3 initialChaserPosition;
    int wavesSpawned = 0;
    int groundsSpawned = 0;
    List<GameObject> waves = new List<GameObject>();

    public enum GameState
    {
        Hyrule, 
        Lorule
    }
    GameState gameState = GameState.Hyrule;

    GameObject ActiveCamera => gameState == GameState.Hyrule ? HyruleCamera : LoruleCamera;
    Vector3 ActiveCameraInitialPos => gameState == GameState.Hyrule ? initialHyruleCameraPos : initialLoruleCameraPos;

    private void Start()
    {
        initialHyruleCameraPos = HyruleCamera.transform.position;
        initialLoruleCameraPos = LoruleCamera.transform.position;
        initialChaserPosition = chaser.transform.position;
    }

    private void OnEnable()
    {
        gameState = GameState.Hyrule;
        HyruleCamera.SetActive(true);
        LoruleCamera.SetActive(false);
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
        switch (gameState)
        {
            case GameState.Hyrule:
                progress = Mathf.Max(progress, playerController.transform.position.z);
                break;
            case GameState.Lorule:
                progress = Mathf.Min(progress, playerController.transform.position.z);
                break;
        }
    }

    void UpdateCamZ()
    {
        Vector3 newCamPos = ActiveCameraInitialPos + Vector3.forward * progress;
        float distance = Vector3.Distance(ActiveCamera.transform.position, newCamPos);
        ActiveCamera.transform.position = Vector3.Lerp(ActiveCamera.transform.position, newCamPos, 1/distance);
    }

    void GenerateWave()
    {
        while (wavesSpawned * wavesGap < progress + pregenDistance)
        {
            GameObject obj = SpawnPrefab(wavesPrefabs[Random.Range(0, wavesPrefabs.Count)], ++wavesSpawned * wavesGap, wavesContainer);
            waves.Add(obj);
        }
    }

    void GenerateGround()
    {
        while (groundsSpawned * groundLength < progress + pregenDistance)
        {
            SpawnPrefab(groundPrefab, ++groundsSpawned * groundLength, groundsContainer);
        }
    }

    GameObject SpawnPrefab(GameObject prefab, float zOffset, Transform parent)
    {
        GameObject obj = Instantiate(prefab);
        obj.transform.position += Vector3.forward * zOffset;
        if (parent != null)
        {
            obj.transform.SetParent(parent);
        }
        return obj;
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

    public void TransitionToLorule()
    {
        chaser.SetActive(false);
        LoruleCamera.transform.position = initialLoruleCameraPos + Vector3.forward * progress;
        LoruleCamera.SetActive(true);
        HyruleCamera.SetActive(false);
        playerController.TransitionGameState();
        gameState = GameState.Lorule;

        BuildNavMesh();
        SpawnEnemies();
    }

    void BuildNavMesh()
    {
        navMeshSurface.BuildNavMesh();
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < waves.Count - 3; i++)
        {
            if (Random.value < enemyChance)
            {
                SpawnEnemy(i);
            }
        }
    }

    void SpawnEnemy(int waveIdx)
    {
        GameObject enemyObj = Instantiate(enemyPrefab);
        enemyObj.transform.position = enemyPrefab.transform.position + Vector3.forward * (waveIdx * wavesGap + wavesGap * 0.5f);
    }

    public void TransitionToEnding()
    {
        Debug.Log("Ending triggered");
    }
}
