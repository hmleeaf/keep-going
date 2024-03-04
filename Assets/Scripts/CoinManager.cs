using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    int coins;

    private void OnEnable()
    {
        coins = 0;
    }

    public void AddCoins(int coinsToAdd)
    {
        coins += coinsToAdd;
    }

    public int GetCoins()
    {
        return coins;
    }
}
