using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    [SerializeField] Vector3 localBoundsMin;
    [SerializeField] Vector3 localBoundsMax;
    [SerializeField] Transform enemySpawnPoint;

    Barrier[] barriers;
    Crate[] crates;

    private void Start()
    {
        barriers = GetComponentsInChildren<Barrier>();
        crates = GetComponentsInChildren<Crate>();
        SetBarriersActive(false);
    }

    public Vector3 BoundsMin { get { return transform.position + localBoundsMin; } }
    public Vector3 BoundsMax { get { return transform.position + localBoundsMax; } }
    public Vector3 Size { get { return localBoundsMax - localBoundsMin; } }
    public Vector3 EnemySpawnPoint { get { return enemySpawnPoint.position; } }

    public void SetBarriersActive(bool active)
    {
        foreach (Barrier barrier in barriers)
        {
            barrier.gameObject.SetActive(active);
        }
    }
    
    public void SetCratesActive(bool active)
    {
        foreach (Crate crate in crates)
        {
            if (crate != null)
            {
                crate.gameObject.SetActive(active);
            }
        }
    }
}
