using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;

    [Space]

    [SerializeField] float speed;

    Vector2 moveDirection = Vector2.right;

    void FixedUpdate()
    {
        rb.velocity = moveDirection * speed;
    }
}
