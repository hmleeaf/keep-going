using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    [SerializeField] GameObject coinPrefab;
    [SerializeField] float coinSpawnChance = 1f;
    [SerializeField] AudioClip breakClip;

    Entity entity;
    AudioSource sfxSource;

    private void Start()
    {
        entity = GetComponent<Entity>();
        sfxSource = GameObject.FindGameObjectWithTag("SFX Audio Source").GetComponent<AudioSource>();
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

            sfxSource.PlayOneShot(breakClip, 1f);
            Destroy(gameObject);
        }
    }
}
