using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayUI : MonoBehaviour
{
    [SerializeField] GameObject hud, pause;
    [SerializeField] AnimationCurve pauseCurve;
    [SerializeField] float unpauseDuration;

    float time;

    void Start()
    {
        hud.SetActive(true);
        pause.SetActive(false);
    }

    void Update()
    {
        if (Player.instance.gamePaused)
        {
            Pause();
        }
        else if (!Player.instance.gamePaused)
        {
            if (Time.timeScale == 1f)
            {
                time = 0f;
                return;
            }

            time += Time.deltaTime;

            StartCoroutine(SlowUnpause());
        }
    }

    void Pause()
    {
        Time.timeScale = 0f;
        pause.SetActive(true);
    }

    IEnumerator SlowUnpause()
    {
        pause.SetActive(false);
        Time.timeScale = pauseCurve.Evaluate(time);
        yield return new WaitForSeconds(unpauseDuration);
    }
}
