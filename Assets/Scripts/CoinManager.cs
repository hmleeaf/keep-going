using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI coinText;

    private void OnEnable()
    {
        UpdateText();
    }

    int coins;
    public int Coins { get { return coins; } }

    public void AddCoin(int coinValue)
    {
        coins += coinValue;
        UpdateText();
    }

    public void LoseAllCoins()
    {
        coins = 0;
        UpdateText();
    }

    void UpdateText()
    {
        coinText.text = coins.ToString();
    }
}
