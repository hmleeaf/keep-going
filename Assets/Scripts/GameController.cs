using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] PlayerController playerController;
    [SerializeField] Entity playerEntity;

    [Header("Camera")]
    [SerializeField] GameObject HyruleCamera;
    [SerializeField] GameObject LoruleCamera;

    [Header("Smoothing")]
    [SerializeField, Range(0f, 1f)] float smoothingFactor = 0.5f;

    [Header("Pre-generation")]
    [SerializeField] float pregenDistance = 50f;

    [Header("Waves")]
    [SerializeField] List<GameObject> wavesPrefabs;
    [SerializeField] float waveZSize = 60f;
    [SerializeField] Transform wavesContainer;

    [Header("Chaser")]
    [SerializeField] GameObject chaser;
    [SerializeField] float chaserNaturalProgressionRate = 5f;

    [Header("AI")]
    [SerializeField] NavMeshSurface navMeshSurface;

    [Header("Enemy")]
    [SerializeField] GameObject enemyPrefab;
    [SerializeField, Range(0f, 1f)] float enemyChance = 0.25f;

    [Header("World Space Texts")]
    [SerializeField] GameObject ambientTextPrefab;
    [SerializeField] float ambientTextXMin = -10f;
    [SerializeField] float ambientTextXMax = 10f;
    [SerializeField] float ambientTextY = 4f;
    [SerializeField, Range(0f, 1f)] float ambientTextZSpawnZoneFromTop = 0.5f;
    [SerializeField] float ambientTextSpawnInterval = 1f;
    [SerializeField] float ambientTextSpawnChance = 1f;
    [SerializeField] GameObject conditionalTextPrefab;
    [SerializeField] float conditionalTextDuration = 4f;
    [SerializeField] float conditionalTextCooldown = 1f;
    [SerializeField] float coinTextDistance = 110f;
    [SerializeField] float encouragingTextEndPoint = 500f;
    [SerializeField] float indifferentTextEndPoint = 800f;
    [SerializeField] float condescendingTextEndPoint = 1100f;
    [SerializeField] float manipulativeTextEndPoint = 1400f;

    [Header("Tutorial")]
    [SerializeField] float tutorialLength = 60f;
    [SerializeField] GameObject tutorialObject;

    [Header("Game flow")]
    [SerializeField] float transitionableDistance = 1400f;
    [SerializeField] float transitionText1Duration = 3f;
    [SerializeField] float transitionText2Duration = 3f;
    [SerializeField] float transitionTurnDuration = 2f;
    [SerializeField, Multiline] string transitionText1 = "You weren't like this before. \r\nYou weren't an <color=#9D2B29>aberration</color>.";
    [SerializeField, Multiline] string transitionText2 = "Go on then. Try. \r\n<color=#9D2B29>You won't get far.</color>";
    [SerializeField] float endDistance = -200f;

    [Header("Screen Text")]
    [SerializeField] FadeInOutText screenText;
    [SerializeField] FadeInOutImage blackoutOverlay;

    [Header("Coin")]
    [SerializeField] CoinManager coinManager;

    [Header("Audio")]
    [SerializeField] AudioSource audioHappy;
    [SerializeField] AudioSource audioSad;
    [SerializeField] AudioSource audioBackwards;
    [SerializeField] AudioSource audioPanting;

    float progress = 0;
    Vector3 initialHyruleCameraPos;
    Vector3 initialLoruleCameraPos;
    Vector3 initialChaserPosition;
    List<WaveInfo> waves = new List<WaveInfo>();
    List<AmbientText> ambientTexts = new List<AmbientText>();
    Vector2 ambientZRange;
    float lastAmbientTextSpawnTime = float.MinValue;
    Vector3 loruleSpawn;
    Dictionary<Prompts.Condition, AmbientText> activeConditionTexts = new Dictionary<Prompts.Condition, AmbientText>();
    float maxHappyVolume, maxSadVolume, maxPantingVolume, maxBackwardsVolume;
    bool finishedTutorial = false;

    public float Progress { get { return progress; } }
    public float TransitionableDistance { get { return transitionableDistance; } }
    public GameState State { get { return gameState; } }
    public float EndDistance { get { return endDistance; } }

    class WaveInfo
    {
        public Wave wave;
        public Entity enemy;
    }

    public enum GameState
    {
        Hyrule, 
        Lorule, 
        HyruleToLorule, 
    }
    GameState gameState = GameState.Hyrule;

    GameObject ActiveCamera => gameState == GameState.Hyrule ? HyruleCamera : LoruleCamera;
    Vector3 ActiveCameraInitialPos => gameState == GameState.Hyrule ? initialHyruleCameraPos : initialLoruleCameraPos;

    private void Start()
    {
        initialHyruleCameraPos = HyruleCamera.transform.position;
        initialLoruleCameraPos = LoruleCamera.transform.position;
        initialChaserPosition = chaser.transform.position;
        maxHappyVolume = audioHappy.volume;
        maxSadVolume = audioSad.volume;
        maxPantingVolume = audioPanting.volume;
        maxBackwardsVolume = audioBackwards.volume;
    }

    private void OnEnable()
    {
        gameState = GameState.Hyrule;
        HyruleCamera.SetActive(true);
        LoruleCamera.SetActive(false);

        CalculateAmbientZRange();
    }

    private void Update()
    {
        CalcProgress();
        UpdateCamZ();
        GenerateWave();
        UpdateChaser();
        CheckEnemies();
        UpdateAmbientText();
        CheckPlayerHealth();
        CheckConditionalPrompts();
        ClampPlayerZ();
        CheckEnd();
        UpdateAudio();
    }

    void UpdateAudio()
    {
        if (gameState == GameState.Hyrule)
        {
            // lerp happy audio volume from 0 to 1 between start and condescending 
            if (audioHappy.isPlaying)
            {
                audioHappy.volume = maxHappyVolume * Mathf.Clamp01(1 - progress / condescendingTextEndPoint);
            }

            // lerp sad audio volume from 0 to 1 between indifferent and manipulative (stays 1 afterwards until transition)
            if (audioSad.isPlaying)
            {
                audioSad.volume = maxSadVolume * Mathf.Clamp01((progress - indifferentTextEndPoint) / (manipulativeTextEndPoint - indifferentTextEndPoint));
            }

            // stop playing happy audio after condescending (1 stage after sad starts playing, should overlap with sad during indifferent to condescending transition)
            if (audioHappy.isPlaying && progress > condescendingTextEndPoint)
            {
                audioHappy.Stop();
            }

            // start playing sad audio after indifferent (1 stage before happy ends, should overlap with happy during indifferent to condescending transition)
            if (!audioSad.isPlaying && progress > indifferentTextEndPoint)
            {
                audioSad.Play();
            }
        } 
        
        if (gameState == GameState.HyruleToLorule)
        {
            if (audioHappy.isPlaying)
            {
                audioHappy.Stop();
            }

            if (audioSad.isPlaying)
            {
                audioSad.Stop();
            }

            if (!audioBackwards.isPlaying)
            {
                audioBackwards.PlayDelayed(transitionText1Duration + 1f + 2f);
            }
        }


        if (gameState == GameState.Lorule && progress < tutorialLength)
        {
            audioBackwards.volume = maxBackwardsVolume * Mathf.Clamp01(progress / tutorialLength);
        }

        if (gameState == GameState.Lorule && progress < 0f)
        {
            if (audioBackwards.isPlaying)
            {
                audioBackwards.Stop();
            }

            audioHappy.volume = maxHappyVolume * Mathf.Clamp01(progress / endDistance);
            if (!audioHappy.isPlaying)
            {
                audioHappy.Play();
            }
        }

        if (chaser.GetComponent<Chaser>().PlayerInTrigger && !audioPanting.isPlaying && gameState == GameState.Hyrule)
        {
            audioPanting.time = maxPantingVolume * Mathf.Clamp01(1 - (float)playerEntity.Health / playerEntity.MaxHp) * audioPanting.clip.length;
            audioPanting.Play();
        }
        if (!chaser.GetComponent<Chaser>().PlayerInTrigger && audioPanting.isPlaying)
        {
            audioPanting.Stop();
        }
    }

    void CheckEnd()
    {
        if (progress < endDistance)
        {
            StartCoroutine(EndSequence());
        }
    }

    IEnumerator EndSequence()
    {
        blackoutOverlay.FadeIn(Color.white, 5f);
        yield return new WaitForSeconds(5f);
        screenText.FadeIn("Thanks for playing.", Color.black, 3f);
        yield return new WaitForSeconds(3f);
        playerController.DisableInput();
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("Title");
    }

    void ClampPlayerZ()
    {
        if (gameState == GameState.Hyrule)
        {
            float zBound = Camera.main.transform.position.z + ambientZRange.x + 1f;
            if (playerController.transform.position.z < zBound)
            {
                playerController.transform.position = new Vector3(
                    playerController.transform.position.x,
                    playerController.transform.position.y,
                    zBound
                );
            }
        }
        else if (gameState == GameState.Lorule)
        {
            float zBound = Camera.main.transform.position.z - ambientZRange.x - 1f;
            if (playerController.transform.position.z > zBound)
            {
                playerController.transform.position = new Vector3(
                    playerController.transform.position.x,
                    playerController.transform.position.y,
                    zBound
                );
            }
        }
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

        if (!finishedTutorial && progress > tutorialLength)
        {
            finishedTutorial = true;
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
        while (waves.Count * waveZSize + tutorialLength < progress + pregenDistance)
        {
            GameObject obj = SpawnPrefab(wavesPrefabs[Random.Range(0, wavesPrefabs.Count)], (waves.Count + 1) * waveZSize - waveZSize / 2 + tutorialLength, wavesContainer);
            WaveInfo waveInfo = new WaveInfo();
            waveInfo.wave = obj.GetComponent<Wave>();
            waves.Add(waveInfo);
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
        if (!chaser.GetComponent<Chaser>().PlayerInTrigger)
        {
            chaser.transform.position += Vector3.forward * chaserNaturalProgressionRate * Time.deltaTime;
        }
        if (chaser.transform.position.z < newChaserPos.z)
        {
            chaser.transform.position = newChaserPos;
        }

    }

    void CheckPlayerHealth()
    {
        if (playerController.IsDead)
        {
            if (gameState == GameState.Hyrule && progress > transitionableDistance)
            {
                TransitionToLorule();
            }
            else
            {
                Respawn();
            }
        }
    }

    void TransitionToLorule()
    {
        loruleSpawn = playerController.transform.position;
        playerController.TransitionGameState();
        gameState = GameState.HyruleToLorule;
        tutorialObject.SetActive(false);
        playerController.DisableInput();
        foreach (WaveInfo wave in waves)
        {
            foreach (Coin coin in wave.wave.GetComponentsInChildren<Coin>())
            {
                Destroy(coin.gameObject);
            }
        }
        foreach (AmbientText ambientText in ambientTexts)
        {
            Destroy(ambientText.gameObject);
        }
        ambientTexts.Clear();

        DeactivateCrates();
        BuildNavMesh();
        SpawnEnemies();
        
        StartCoroutine(TransitionTexts());
    }

    IEnumerator TransitionTexts()
    {
        blackoutOverlay.FadeIn();
        screenText.FadeIn(transitionText1);
        yield return new WaitForSeconds(transitionText1Duration);
        screenText.FadeOut(1f);
        yield return new WaitForSeconds(2f);
        screenText.FadeIn(transitionText2);
        yield return new WaitForSeconds(transitionText2Duration);
        blackoutOverlay.FadeOut();
        screenText.FadeOut();
        StartCoroutine(EndTransition());
    }

    IEnumerator EndTransition()
    {
        chaser.SetActive(false);
        playerEntity.HealToFull();
        LoruleCamera.transform.position = initialLoruleCameraPos + Vector3.forward * progress;
        progress = loruleSpawn.z;
        LoruleCamera.SetActive(true);
        HyruleCamera.SetActive(false);

        yield return new WaitForSeconds(transitionTurnDuration);
        gameState = GameState.Lorule;
        playerController.EnableInput();
    }

    void Respawn()
    {
        if (gameState == GameState.HyruleToLorule) return;
        if (finishedTutorial)
        {
            tutorialObject.SetActive(false);
        }

        foreach (AmbientText ambientText in ambientTexts)
        {
            Destroy(ambientText.gameObject);
        }
        ambientTexts.Clear();

        if (gameState == GameState.Hyrule)
        {
            foreach (WaveInfo wave in waves)
            {
                Destroy(wave.wave.gameObject);
            }
            waves.Clear();

            playerController.transform.position = new Vector3(0, 1, progress);
            playerEntity.HealToFull();
            chaser.transform.position = initialChaserPosition;
            coinManager.LoseAllCoins();
        }
        else if (gameState == GameState.Lorule)
        {
            for (int i = 0; i < waves.Count - 2; i++) 
            {
                WaveInfo wave = waves[i];
                wave.wave.SetBarriersActive(true);
                if (wave.enemy)
                {
                    wave.enemy.gameObject.SetActive(true);
                    wave.enemy.GetComponent<NavMeshAgent>().Warp(enemyPrefab.transform.position + wave.wave.EnemySpawnPoint);
                    wave.enemy.HealToFull();
                    wave.enemy.gameObject.GetComponent<EnemyAI>().ResetAggro();
                }
            }

            playerEntity.HealToFull();
            playerController.transform.position = new Vector3(0, 1, progress);
        }

        screenText.FadeIn("Keep going", Color.white, 0.3f);
        Invoke(nameof(FadeOutDeathText), 1f);
    }

    void FadeOutDeathText()
    {
        screenText.FadeOut(0.3f);
    }

    void DeactivateCrates()
    {
        foreach (WaveInfo wave in waves)
        {
            wave.wave.SetCratesActive(false);
        }
    }

    void BuildNavMesh()
    {
        navMeshSurface.BuildNavMesh();
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < waves.Count - 2; i++)
        {
            if (Random.value < enemyChance)
            {
                SetupHostileWave(i);
            }
        }
    }

    void SetupHostileWave(int waveIdx)
    {
        WaveInfo waveInfo = waves[waveIdx];
        GameObject enemyObj = Instantiate(enemyPrefab, enemyPrefab.transform.position + waveInfo.wave.EnemySpawnPoint, Quaternion.identity);
        waveInfo.wave.SetBarriersActive(true);
        waveInfo.enemy = enemyObj.GetComponent<Entity>();
    }

    void CheckEnemies()
    {
        if (gameState == GameState.Lorule)
        {
            foreach (WaveInfo waveInfo in waves)
            {
                if (waveInfo.enemy && waveInfo.enemy.isActiveAndEnabled && waveInfo.enemy.Health <= 0)
                {
                    waveInfo.enemy.gameObject.SetActive(false);
                    waveInfo.wave.SetBarriersActive(false);
                }
            }
        }
    }

    void UpdateAmbientText()
    {
        // don't spawn ambient texts during tutorial
        if (progress < tutorialLength && gameState == GameState.Hyrule) return;

        // don't spawn during ending sequence
        if (progress < 0 && gameState == GameState.Lorule) return;

        if (Time.time > lastAmbientTextSpawnTime + ambientTextSpawnInterval)
        {
            lastAmbientTextSpawnTime = Time.time;
            if (Random.value < ambientTextSpawnChance)
            {
                Vector3 position = new Vector3(
                    ambientTexts.Count % 2 == 0 ? ambientTextXMin : ambientTextXMax,
                    ambientTextY,
                    Camera.main.transform.position.z + Random.Range(Mathf.Lerp(ambientZRange.y, ambientZRange.x, ambientTextZSpawnZoneFromTop), ambientZRange.y) * (gameState == GameState.Lorule ? -1 : 1)
                );

                Prompts.Tone tone = Prompts.Tone.Doubtful;
                if (gameState == GameState.Hyrule)
                {
                    if (progress < encouragingTextEndPoint)
                    {
                        tone = Prompts.Tone.Encouraging;
                    }
                    else if (progress < indifferentTextEndPoint)
                    {
                        tone = Prompts.Tone.Indifferent;
                    }
                    else if (progress < condescendingTextEndPoint)
                    {
                        tone = Prompts.Tone.Condescending;
                    }
                    else if (progress < manipulativeTextEndPoint)
                    {
                        tone = Prompts.Tone.Manipulative;
                    }
                }

                ambientTexts.Add(SpawnAmbientText(Prompts.GetPrompt(tone), position));
            }
        }
    }

    AmbientText SpawnAmbientText(string text, Vector3 position)
    {
        GameObject ambientTextObj = Instantiate(ambientTextPrefab, position, Quaternion.identity);
        AmbientText ambientText = ambientTextObj.GetComponent<AmbientText>();
        ambientText.SetText(text);
        return ambientText;
    }

    void CalculateAmbientZRange()
    {
        Ray bottomMiddleRay = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, 0, 0));
        Ray topMiddleRay = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height - 1, 0));
        Plane textPlane = new Plane(Vector3.up, -ambientTextY);
        textPlane.Raycast(bottomMiddleRay, out float bottomEnter);
        textPlane.Raycast(topMiddleRay, out float topEnter);
        ambientZRange = new Vector2(
            bottomMiddleRay.GetPoint(bottomEnter).z,
            topMiddleRay.GetPoint(topEnter).z
        ) - Vector2.one * Camera.main.transform.position.z;
    }

    void CheckConditionalPrompts()
    {
        if (!IsPromptSpawned(Prompts.Condition.WrongDirection) 
            && !IsPromptSpawned(Prompts.Condition.InFog)
            && !IsPromptSpawned(Prompts.Condition.InDeepFog)
            && progress > transitionableDistance
            && playerController.transform.position.z < progress - 1f)
        {
            SpawnConditionalPrompt(Prompts.Condition.WrongDirection);
        }

        if (!IsPromptSpawned(Prompts.Condition.InFog) 
            && !IsPromptSpawned(Prompts.Condition.InDeepFog)
            && progress > transitionableDistance
            && (float)playerEntity.Health / playerEntity.MaxHp < 1f 
            && gameState == GameState.Hyrule)
        {
            DespawnConditionalPrompt(Prompts.Condition.WrongDirection);
            SpawnConditionalPrompt(Prompts.Condition.InFog);
        }

        if (!IsPromptSpawned(Prompts.Condition.InDeepFog)
            && progress > transitionableDistance
            && (float)playerEntity.Health / playerEntity.MaxHp < 0.4f 
            && gameState == GameState.Hyrule)
        {
            DespawnConditionalPrompt(Prompts.Condition.WrongDirection);
            DespawnConditionalPrompt(Prompts.Condition.InFog);
            SpawnConditionalPrompt(Prompts.Condition.InDeepFog);
        }

        if (!IsPromptSpawned(Prompts.Condition.OmitTutorialCrateCoin) 
            && coinManager.Coins < 1
            && playerController.transform.position.z > coinTextDistance
            && playerController.transform.position.z < coinTextDistance + 10f
            && gameState == GameState.Hyrule) 
        {
            SpawnConditionalPrompt(Prompts.Condition.OmitTutorialCrateCoin, 0.75f);
        }

        if (!IsPromptSpawned(Prompts.Condition.ProgressCanTransition)
            && playerController.transform.position.z > transitionableDistance
            && gameState == GameState.Hyrule)
        {
            SpawnConditionalPrompt(Prompts.Condition.ProgressCanTransition, 0.75f);
        }
    }

    AmbientText SpawnCenterPrompt(string text, float offsetFromBottomPercentage)
    {
        Vector3 position = new Vector3(
            0,
            ambientTextY,
            Camera.main.transform.position.z + Mathf.Lerp(ambientZRange.x, ambientZRange.y, offsetFromBottomPercentage) * (gameState == GameState.Lorule ? -1 : 1)
        );
        GameObject ambientTextObj = Instantiate(conditionalTextPrefab, position, Quaternion.identity);
        AmbientText ambientText = ambientTextObj.GetComponent<AmbientText>();
        ambientText.SetText(text);
        return ambientText;
    }

    bool IsPromptSpawned(Prompts.Condition conditionalPrompt)
    {
        return activeConditionTexts.ContainsKey(conditionalPrompt);
    }

    void SpawnConditionalPrompt(Prompts.Condition conditionalPrompt, float offsetFromBottomPercentage = 0.25f)
    {
        activeConditionTexts.Add(
            conditionalPrompt, 
            SpawnCenterPrompt(Prompts.GetPrompt(conditionalPrompt), offsetFromBottomPercentage)
        );
        StartCoroutine(ConditionalPromptCooldown(conditionalPrompt));
    }

    void DespawnConditionalPrompt(Prompts.Condition conditionalPrompt)
    {
        if (activeConditionTexts.ContainsKey(conditionalPrompt))
        {
            Destroy(activeConditionTexts[conditionalPrompt].gameObject);
            activeConditionTexts.Remove(conditionalPrompt);
        }
    }

    IEnumerator ConditionalPromptCooldown(Prompts.Condition conditionalPrompt)
    {
        yield return new WaitForSeconds(conditionalTextDuration + conditionalTextCooldown);
        DespawnConditionalPrompt(conditionalPrompt);
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawSphere(new Vector3(0, ambientTextY, ambientZRange.x + Camera.main.transform.position.z), 1f);
    //    Gizmos.DrawSphere(new Vector3(0, ambientTextY, ambientZRange.y + Camera.main.transform.position.z), 1f);
    //}

    public void TransitionToEnding()
    {
        Debug.Log("Ending triggered");
    }
}
