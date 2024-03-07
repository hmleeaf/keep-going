using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    [SerializeField] GameObject coinPrefab;
    [SerializeField] float coinSpawnChance = 1f;

    Entity entity;

    private void Start()
    {
        entity = GetComponent<Entity>();
    }

    private void Update()
    {
        if (entity.Health <= 0)
        {
            if (Random.value < coinSpawnChance)
            {
                GameObject coinObj = Instantiate(coinPrefab);
                coinObj.transform.position = transform.position;
                coinObj.transform.SetParent(transform.parent);
            }

            Destroy(gameObject);
        }
    }
}
