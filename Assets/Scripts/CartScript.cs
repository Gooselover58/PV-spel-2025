using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartScript : MonoBehaviour
{
    private Rigidbody2D rb;
    private float cartWeight;
    private Vector2 startPos;
    private Vector2 endPos;
    private float time;
    [SerializeField] float returnForce;
    public static bool isAtEnd;

    private void Awake()
    {
        isAtEnd = false;
        rb = GetComponent<Rigidbody2D>();
        cartWeight = 0;
        ChangeWeight(100f);
        ResetCart();
    }

    private void Update()
    {
        if (isAtEnd && endPos != null)
        {
            transform.position = Vector2.Lerp(startPos, endPos, time);
            time += Time.deltaTime;
        }
    }

    private void FinishCart()
    {
        time = 0;
        isAtEnd = true;
        rb.isKinematic = true;
    }

    public void ResetCart()
    {
        isAtEnd = false;
        if (rb != null)
        {
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
