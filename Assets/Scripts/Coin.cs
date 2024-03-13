using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] int coinValue = 1;
    [SerializeField] AudioClip coinClip;

    CoinManager coinManager;
    AudioSource sfxSource;

    private void Start()
    {
        coinManager = FindObjectOfType<CoinManager>();
        sfxSource = GameObject.FindGameObjectWithTag("SFX Audio Source").GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            coinManager.AddCoin(coinValue);
            sfxSource.PlayOneShot(coinClip, 1f);
            Destroy(gameObject);
        }
    }
}
