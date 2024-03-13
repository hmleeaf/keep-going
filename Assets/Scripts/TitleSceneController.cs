using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneController : MonoBehaviour
{
    [SerializeField] GameObject wave;
    [SerializeField] float waveLength = 60f;
    [SerializeField] float waveScrollRate;
    [SerializeField] float spawnZ;
    [SerializeField] float despawnZ;
    
    LinkedList<GameObject> waves = new LinkedList<GameObject>();

    private void Start()
    {
        for (float i = despawnZ; i <= spawnZ; i += waveLength)
        {
            GameObject newWave = Instantiate(wave, Vector3.forward * i, Quaternion.identity);
            waves.AddLast(newWave);
        }
    }

    private void Update()
    {
        List<GameObject> wavesToKill = new List<GameObject>();

        foreach (GameObject wave in waves)
        {
            wave.transform.position = wave.transform.position - Vector3.forward * waveScrollRate * Time.deltaTime;

            if (wave.transform.position.z < despawnZ)
            {
                wavesToKill.Add(wave);
            }
        }

        foreach (GameObject wave in wavesToKill)
        {
            waves.Remove(wave);
            Destroy(wave);
        }

        Vector3 lastWavePosition = waves.Last.Value.transform.position;
        if (lastWavePosition.z < spawnZ)
        {
            Vector3 newWavePosition = lastWavePosition + Vector3.forward * waveLength;
            GameObject newWave = Instantiate(wave, newWavePosition, Quaternion.identity);
            waves.AddLast(newWave);
        }
    }

    public void SwitchToGameScene()
    {
        SceneManager.LoadScene("Game");
    }
}
