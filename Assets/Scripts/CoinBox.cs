using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBox : MonoBehaviour, IDestructible
{
    CoinManager coinManager;

    private void Start()
    {
        coinManager = FindObjectOfType<CoinManager>();
    }

    public void Destruct()
    {
        coinManager.AddCoins(1);
        Destroy(gameObject);
    }
}
