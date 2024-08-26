using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{
    [SerializeField] GameObject hud, pause, intro, end;
    [SerializeField] AnimationCurve pauseCurve;
    [SerializeField] float unpauseDuration;

    [Space]

    [SerializeField] TextMeshProUGUI[] introLines, endLines;
    TextMeshProUGUI currentLine;
    int currentLineIndex;
    [SerializeField] Image hand;

    float time;
    bool isIntro;

    void Start()
    {
        hud.SetActive(true);
        pause.SetActive(false);
        intro.SetActive(false);
        end.SetActive(false);
    }

    void Update()
    {
        IncrementDialogue();

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
        Time.timeScale = pauseCurve.Evaluate(time);
        pause.SetActive(false);
        yield return new WaitForSeconds(unpauseDuration);
    }

    public void StartDialogue(bool check)
    {
        isIntro = check;
        
        currentLineIndex = 0;
        
        if (isIntro)
        {
            intro.SetActive(true);

            foreach (TextMeshProUGUI line in introLines)
            {
                line.gameObject.SetActive(false);
            }
            
            hand.gameObject.SetActive(false);

            SetCurrentLine(introLines);
            
            currentLine.gameObject.SetActive(true);

            return;
        }

        end.SetActive(true);

        foreach (TextMeshProUGUI line in introLines)
        {
            line.gameObject.SetActive(false);
        }

        SetCurrentLine(endLines);

        currentLine.gameObject.SetActive(true);
    }

    TextMeshProUGUI SetCurrentLine(TextMeshProUGUI[] dialogue)
    {
        for (int i = 0; i < dialogue.Length; i++)
        {
            if (i == currentLineIndex)
            {
                currentLine = introLines[i];
            }
        }

        return currentLine;
    }

    void IncrementDialogue()
    {
        if (Player.instance.HasPlayerClicked())
        {
            //Debug.Log("Clicked");
            if(currentLine == null)
            {
                return;
            }

            currentLine.gameObject.SetActive(false);

            currentLineIndex++;

            if (isIntro && currentLineIndex == introLines.Length)
            {
                intro.SetActive(false);

                Player.instance.inCutscene = false;
                return;
            }
            else if (!isIntro && currentLineIndex == endLines.Length)
            {
                end.SetActive(false);

                Player.instance.inCutscene = false;
                return;
            }

            if (isIntro)
            {
                SetCurrentLine(introLines);
            }
            else
            {
                SetCurrentLine(endLines);
            }
            
            currentLine.gameObject.SetActive(true);

            if (isIntro)
            {
                if (currentLineIndex == 1)
                {
                    hand.gameObject.SetActive(true);
                }
                else
                {
                    hand.gameObject.SetActive(false);
                }
            }
        }
    }
}
