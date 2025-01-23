using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float timer;
    public int seconds, minutes;

    void Update()
    {
        timer += Time.deltaTime;
        seconds = (int)timer % 60;
        minutes = (int)timer / 60;
    }
}
