using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartScript : MonoBehaviour, IInteractable
{
    private Rigidbody2D rb;
    private float cartWeight;
    private int cartGold;
    private Vector2 startPos;
    private Vector2 endPos;
    private float time;
    private bool shouldAlign;
    [SerializeField] float returnForce;
    public static bool isAtEnd;

    private void Awake()
    {
        isAtEnd = false;
        shouldAlign = false;
        rb = GetComponent<Rigidbody2D>();
        cartWeight = 0;
        ChangeWeight(100f);
        ResetCart();
    }

    private void Update()
    {
        if (shouldAlign && endPos != null)
        {
            transform.position = Vector2.Lerp(startPos, endPos, time);
            time += Time.deltaTime;
        }
    }

    public void Interact()
    {
        Type heldType = GenerationManager.player.GetComponent<TestPlayer>().heldItem.GetType();
        if (heldType == typeof(GameObject))
        {
            iHoldable holdable = GenerationManager.player.GetComponent<TestPlayer>().heldItem.GetComponent<iHoldable>();
            ChangeWeight(holdable.weight);
            GenerationManager.player.GetComponent<TestPlayer>().RemoveItem();
            cartGold += 1;
        }
    }

    private void FinishCart()
    {
        time = 0;
        isAtEnd = true;
        rb.isKinematic = true;
        rb.velocity = Vector2.zero;
        StartCoroutine(AlignCart());
    }

    public void ResetCart()
    {
        isAtEnd = false;
        shouldAlign = false;
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = false;
        }
    }

    private void FixedUpdate()
    {
        if (transform.position.y < -1.5f)
        {
            rb.AddForce(Vector2.up * returnForce);
        }
    }

    public void ChangeWeight(float change)
    {
        cartWeight += change;
        rb.mass = cartWeight;
        rb.drag = cartWeight / 100;
    }

    private IEnumerator AlignCart()
    {
        shouldAlign = true;
        yield return new WaitForSeconds(1f);
        shouldAlign = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("CartEnd"))
        {
            startPos = transform.position;
            endPos = col.transform.position;
            col.gameObject.SetActive(false);
            FinishCart();
        }
    }
}
