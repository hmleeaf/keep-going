using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    [SerializeField] List<GameObject> barriers = new List<GameObject>();
    [SerializeField] Vector3 localBoundsMin;
    [SerializeField] Vector3 localBoundsMax;

    private void OnEnable()
    {
        SetBarriersActive(false);
    }

    public Vector3 BoundsMin { get { return transform.position + localBoundsMin; } }
    public Vector3 BoundsMax { get { return transform.position + localBoundsMax; } }
    public Vector3 Size { get { return localBoundsMax - localBoundsMin; } }

    public void SetBarriersActive(bool active)
    {
        foreach (GameObject barrier in barriers)
        {
            barrier.SetActive(active);
        }
    }
}
