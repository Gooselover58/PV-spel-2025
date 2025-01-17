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

    public float attackRange;
    Coroutine attackCoroutine = null;
    public int damage;

    bool isMoving;
    Animator enemyAnimator;

    


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyAnimator = GetComponent<Animator>();
        
        FindPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        FindPlayer();

        MoveAtPlayer();

        IsPlayerInMeleeRange();
    }

    /// <summary>
    /// Moves toward playerObject if it is not nulL
    /// </summary>
    void MoveAtPlayer()
    {

        if (playerObject != null)
        {
            enemyAnimator.SetBool("IsMoving", true);
            Vector2 moveDirection = (playerObject.transform.position - transform.position).normalized;
            WalkAnimation(moveDirection);

            rb.velocity = moveDirection * moveSpeed;
        }
        else
        {
            enemyAnimator.SetBool("IsMoving", false);
        }
    }

    void WalkAnimation(Vector2 moveDirection)
    {
        enemyAnimator.SetFloat("MovingX", moveDirection.x);
        enemyAnimator.SetFloat("MovingY", moveDirection.y);
    }

    /// <summary>
    /// Checks if player is within melee range
    /// </summary>
    void IsPlayerInMeleeRange()
    {
        if (playerObject != null)
        {
            float playerDistance = (playerObject.transform.position - transform.position).magnitude;

            if (playerDistance < attackRange && attackCoroutine == null)
            {
                attackCoroutine = StartCoroutine(AttackCoroutine());
            }
        }
    }
    
    /// <summary>
    /// Attack with cooldown
    /// </summary>
    /// <returns></returns>
    private IEnumerator AttackCoroutine()
    {
        AttackPlayer();
        yield return new WaitForSeconds(1.5f);
        attackCoroutine = null;
    }

    /// <summary>
    /// Deals damage to player
    /// </summary>
    private void AttackPlayer()
    {
        EntityHealth playerHealth = playerObject.GetComponent<EntityHealth>();
        playerHealth.ChangeHealth(-damage);
        print("Fuck you");
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
