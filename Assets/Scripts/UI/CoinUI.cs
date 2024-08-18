using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI coinText;

    void Start()
    {
        coinText.text = "0";
    }

    public void UpdateCoinUI(int count)
    {
        coinText.text = count.ToString();
    }
}
