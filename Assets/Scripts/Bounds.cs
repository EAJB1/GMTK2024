using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounds : MonoBehaviour
{
    public Vector2 camOffset;
    public float cameraSize;
    public bool lockY, lockX;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, transform.localScale);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + (Vector3)camOffset, new Vector2(2f * cameraSize * (16f / 9f), 2f * cameraSize));
    }
}
