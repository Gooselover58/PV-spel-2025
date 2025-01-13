using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    Rigidbody2D rb;
    [SerializeField] float moveSpeed;

    GameObject playerObject = null;
    public float playerFindRadius;
    [SerializeField] LayerMask playerLayer;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        FindPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        FindPlayer();

        MoveAtPlayer();
    }

    /// <summary>
    /// Moves toward playerObject if it is not nulL
    /// </summary>
    void MoveAtPlayer()
    {
        if (playerObject != null)
        {
            Vector2 moveDirection = (playerObject.transform.position - transform.position).normalized;

            rb.velocity = moveDirection * moveSpeed;
        }
    }

    /// <summary>
    /// Tries to find player
    /// </summary>
    private void FindPlayer()
    {
        if (playerObject == null)
        {
            Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, playerFindRadius, playerLayer);
            if (playerCollider != null)
            {
                playerObject = playerCollider.gameObject;
            }
        }
    }
}
