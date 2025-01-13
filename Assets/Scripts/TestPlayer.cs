using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    [SerializeField] float playerBaseSpeed;
    private float playerFinalSpeed;
    private Vector2 movement;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
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
        anim.SetFloat("X", x);
        anim.SetFloat("Y", y);
        bool isMoving = (movement.magnitude > 0) ? true : false;
        anim.SetBool("IsMoving", isMoving);
        rb.velocity = movement;
    }
}
