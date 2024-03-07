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

    public void AddCoin(int coinValue)
    {
        coins += coinValue;
        UpdateText();
    }

    void UpdateText()
    {
        coinText.text = coins.ToString();
    }
}
