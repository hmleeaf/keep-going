using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    [SerializeField] List<GameObject> barriers = new List<GameObject>();
    [SerializeField] List<GameObject> crates = new List<GameObject>();
    [SerializeField] Vector3 localBoundsMin;
    [SerializeField] Vector3 localBoundsMax;
    [SerializeField] Transform enemySpawnPoint;

    private void OnEnable()
    {
        SetBarriersActive(false);
    }

    public Vector3 BoundsMin { get { return transform.position + localBoundsMin; } }
    public Vector3 BoundsMax { get { return transform.position + localBoundsMax; } }
    public Vector3 Size { get { return localBoundsMax - localBoundsMin; } }
    public Vector3 EnemySpawnPoint { get { return enemySpawnPoint.position; } }

    public void SetBarriersActive(bool active)
    {
        foreach (GameObject barrier in barriers)
        {
            barrier.SetActive(active);
        }
    }
    
    public void SetCratesActive(bool active)
    {
        foreach (GameObject crate in crates)
        {
            if (crate != null)
            {
                crate.SetActive(active);
            }
        }
    }
}
