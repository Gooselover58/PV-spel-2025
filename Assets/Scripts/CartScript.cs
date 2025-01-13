using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartScript : MonoBehaviour
{
    private Rigidbody2D rb;
    private float cartWeight;
    [SerializeField] float returnForce;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cartWeight = 100;
        ChangeWeight(20f);
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
}
