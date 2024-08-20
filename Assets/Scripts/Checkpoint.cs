using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] SpriteRenderer sr;
    [SerializeField] Animator anim;

    public enum StartDirection { Left, Right };

    public StartDirection startDirection = StartDirection.Right;
    public bool startStationary;

    bool triggered;

    public int checkpointCheatNumber;
    KeyCode[] cheatKeys = { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5 };

    private void Update()
    {
        if (Input.GetKeyDown(cheatKeys[checkpointCheatNumber]))
        {
            Player.instance.SetCurrentCheckpoint(this);
            Player.instance.Die();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!triggered && collision.gameObject == Player.instance.gameObject)
        {
            triggered = true;

            if (Player.instance.xDirection == 1f)
            {
                sr.flipX = false;
            }
            else if (Player.instance.xDirection == -1f)
            {
                sr.flipX = true;
            }

            Player.instance.SetCurrentCheckpoint(this);

            anim.SetBool("Hit", true);

            // Play checkpoint sound

            SoundManager.instance.PlaySound("Checkpoint");
        }
    }

    public void ResetSpriteFlipX()
    {
        sr.flipX = false;
    }
}
