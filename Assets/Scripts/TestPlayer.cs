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

    Vector2 moveDirection;

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
        SetMoveDirection(x, y);
        anim.SetFloat("X", moveDirection.x);
        anim.SetFloat("Y", moveDirection.y);
        bool isMoving = (movement.magnitude > 0) ? true : false;
        anim.SetBool("IsMoving", isMoving);
        rb.velocity = movement;
    }

    private void SetMoveDirection(float xMove, float yMove)
    {
        if (movement.magnitude > 0)
        {
            moveDirection.x = xMove;
            moveDirection.y = yMove;
        }
    }

}
