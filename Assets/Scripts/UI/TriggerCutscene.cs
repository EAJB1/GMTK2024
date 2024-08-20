using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCutscene : MonoBehaviour
{
    [SerializeField] GodCutscenes godCutscenes;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == Player.instance.gameObject)
        {
            Player.instance.inCutscene = true;
            godCutscenes.canMove = true;
            Destroy(gameObject);
        }
    }
}
