using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TestPlayer : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    [SerializeField] float playerBaseSpeed;
    private float playerFinalSpeed;
    private float weight;
    private Vector2 movement;
    private GameObject heldItem;

    Vector2 moveDirection;

    public UnityEvent footstepEvent;
    AudioSource footstepSound;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        weight = 0;
        ChangeWeight(10f);
        footstepEvent = new UnityEvent();
        footstepSound = GetComponent<AudioSource>();
        footstepEvent.AddListener(PlayFootStepSound);
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

    private void ChangeWeight(float change)
    {
        weight += change;
        playerFinalSpeed = playerBaseSpeed / (weight / 10);
    }

    public void PickUpItem(GameObject ob)
    {
        iHoldable holdable = ob.GetComponent<iHoldable>();
        if (holdable != null)
        {
            heldItem = ob;
            heldItem.GetComponent<SpriteRenderer>().enabled = false;
            ChangeWeight(holdable.weight);
        }
    }

    public void DropItem()
    {
        if (heldItem != null)
        {
            iHoldable holdable = heldItem.GetComponent<iHoldable>();
            if (holdable != null)
            {
                ChangeWeight(-holdable.weight);
                heldItem.transform.position = transform.position;
                heldItem.GetComponent<SpriteRenderer>().enabled = true;
                heldItem = null;
            }
        }
    }

    public void PlayFootStepSound()
    {
        footstepSound.Play();
    }

}
