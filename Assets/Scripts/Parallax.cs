using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Vector2 parallaxFactor;

    private Vector2 startOffset;

    private void Start()
    {
        startOffset = transform.localPosition;
    }

    private void Update()
    {
        transform.position = (transform.parent.position * parallaxFactor) + startOffset;
    }
}
