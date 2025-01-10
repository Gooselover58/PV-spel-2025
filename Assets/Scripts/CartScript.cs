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
        cartWeight = 0;
        ChangeWeight(20f);
    }

    public void ChangeWeight(float change)
    {
        cartWeight += change;
        rb.mass = 0.75f * cartWeight;
        rb.drag = 0.075f * cartWeight;
    }
}
