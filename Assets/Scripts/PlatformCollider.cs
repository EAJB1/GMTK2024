using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCollider : MonoBehaviour
{
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject == Player.instance.gameObject)
        {
            if(collision.transform.parent != null)
            {
                return;
            }

            collision.transform.parent = transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject == Player.instance.gameObject)
        {
            if (collision.transform.parent != transform)
            {
                return;
            }

            collision.transform.parent = null;
        }
    }
}
