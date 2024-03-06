using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    [SerializeField] List<GameObject> barriers = new List<GameObject>();
    [SerializeField] Vector3 localBoundsMin;
    [SerializeField] Vector3 localBoundsMax;

    List<Enemy> enemies = new List<Enemy>();

    private void OnEnable()
    {
        SetBarriersActive(false);
    }

    public Vector3 BoundsMin { get { return transform.position + localBoundsMin; } }
    public Vector3 BoundsMax { get { return transform.position + localBoundsMax; } }
    public Vector3 Size { get { return localBoundsMax - localBoundsMin; } }
    public int EnemyCount { get { return enemies.Count; } }

    public void SetBarriersActive(bool active)
    {
        foreach (GameObject barrier in barriers)
        {
            barrier.SetActive(active);
        }
    }

    public void AddEnemy(Enemy enemy)
    {
        enemies.Add(enemy);
    }

    public void RemoveEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);
    }
}
