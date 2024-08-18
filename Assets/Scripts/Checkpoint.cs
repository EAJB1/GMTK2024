using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public enum StartDirection { Left, Right };

    public StartDirection startDirection = StartDirection.Right;
    public bool startStationary;

    bool triggered;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!triggered && collision.gameObject == Player.instance.gameObject)
        {
            triggered = true;

            Player.instance.SetCurrentCheckpoint(this);

            // Play checkpoint animation and sound
            // Change colour of checkpoint
        }
    }
}
