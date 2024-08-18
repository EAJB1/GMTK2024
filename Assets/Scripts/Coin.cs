using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public static int coinsCollected = 0;

    [Space]

    [SerializeField] float speed, coinTime;

    CoinUI coinUI;
    bool collected;

    void Awake()
    {
        coinUI = FindObjectOfType<CoinUI>();
    }

    void Update()
    {
        if (collected)
        {
            transform.position += speed * Time.deltaTime * Vector3.up;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
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

        StartCoroutine(CoinWait());
    }

    IEnumerator CoinWait()
    {
        collected = true;

        yield return new WaitForSeconds(coinTime);
        
        Destroy(gameObject);
    }
}
