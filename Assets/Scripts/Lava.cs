using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;

    [Space]

    [SerializeField] float moveSpeed;

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        rb.MovePosition(rb.position + moveSpeed * Time.fixedDeltaTime * Vector2.up);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision);
        if (collision.gameObject.CompareTag("Player"))
        {
            Player.instance.Die();
        }
    }
}
