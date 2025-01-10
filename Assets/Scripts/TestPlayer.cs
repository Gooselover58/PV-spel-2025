using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] float playerBaseSpeed;
    private float playerFinalSpeed;
    private Vector2 movement;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerFinalSpeed = playerBaseSpeed;
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        movement = new Vector2(x, y).normalized * playerFinalSpeed;
        rb.velocity = movement;
    }
}
