using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    [SerializeField] Animator animator;

    public void StopFadeAnimation()
    {
        animator.speed = 0.0f;
    }
}
