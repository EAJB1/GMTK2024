using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public float parallaxFactor;

    private Vector2 startOffset;

    private void Start()
    {
        startOffset = transform.localPosition;
    }

    private void Update()
    {
        transform.position = (Vector2)(transform.parent.position * parallaxFactor) + startOffset;
    }
}
