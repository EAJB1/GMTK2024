using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodCutscenes : MonoBehaviour
{
    [SerializeField] GameplayUI gameplayUI;

    [Space]

    [SerializeField] Vector3 endPosition;
    Vector3 startPosition;
    [SerializeField] float lerp;
    public bool canMove;
    [SerializeField] bool isIntro;

    private void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (Player.instance.inCutscene)
        {
        }

        if (canMove)
        {
            if (Mathf.CeilToInt(transform.position.x) == (int)endPosition.x)
            {
                canMove = false;
                gameplayUI.StartDialogue(isIntro);
            }

            MoveGod(endPosition);
        }
    }

    void MoveGod(Vector2 position)
    {
        transform.position = Vector3.Lerp(transform.position, position, lerp * Time.deltaTime);
    }
}
