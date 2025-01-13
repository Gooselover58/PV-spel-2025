using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartScript : MonoBehaviour
{
    private Rigidbody2D rb;
    private float cartWeight;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cartWeight = 100;
        ChangeWeight(20f);
    }

    public void ChangeWeight(float change)
    {
        cartWeight += change;
        rb.mass = cartWeight;
        rb.drag = cartWeight / 100;
    }
}
