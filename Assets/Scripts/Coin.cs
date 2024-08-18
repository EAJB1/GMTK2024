using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public static int coinsCollected = 0;

    CoinUI coinUI;

    void Awake()
    {
        coinUI = FindObjectOfType<CoinUI>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject);
        if(collision.gameObject == Player.instance.gameObject)
        {
            Collect();
            return;
        }
    }

    public void Collect()
    {
        coinsCollected++;

        //Play coin sfx
        SoundManager.instance.PlaySound("Coin");

        coinUI.UpdateCoinUI(coinsCollected);

        Destroy(gameObject);
    }
}
