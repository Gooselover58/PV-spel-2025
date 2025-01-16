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
    public float weight;
    private Vector2 movement;
    public GameObject heldItem;

    Vector2 moveDirection;
    bool isLightingTheWay;

    AudioSource footstepSound;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        weight = 0;
        ChangeWeight(10f);
        footstepSound = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            isLightingTheWay = true;
        }
        else
        {
            isLightingTheWay = false;
        }
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
        if (movement.magnitude > 0 && !isLightingTheWay)
        {
            moveDirection.x = xMove;
            moveDirection.y = yMove;
        }
        else if (isLightingTheWay)
        {
            Vector2 direction = ((Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized).normalized;
            moveDirection = direction;
        }
    }

    private void ChangeWeight(float change)
    {
        weight += change;
        playerFinalSpeed = playerBaseSpeed / (weight / 10);
    }

    public void PickUpItem(GameObject ob)
    {
        if (HasItem())
        {
            return;
        }
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
        if (!HasItem())
        {
            return;
        }
        iHoldable holdable = heldItem.GetComponent<iHoldable>();
        if (holdable != null)
        {
            ChangeWeight(-holdable.weight);
            heldItem.transform.position = transform.position;
            heldItem.GetComponent<SpriteRenderer>().enabled = true;
            heldItem = null;
        }
    }

    public void RemoveItem()
    {
        iHoldable holdable = heldItem.GetComponent<iHoldable>();
        if (holdable != null)
        {
            ChangeWeight(-holdable.weight);
            Destroy(heldItem.gameObject);
            heldItem = null;
        }
    }

    private bool HasItem()
    {
        bool hasItem = (heldItem == null) ? false : true;
        return hasItem;
    }

    public void PlayFootStepSound()
    {

        footstepSound.pitch = Random.Range(0.8f, 1.2f);
        footstepSound.Play();
    }

}
