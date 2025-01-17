using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    Rigidbody2D rb;
    [SerializeField] float moveSpeed;

    public GameObject playerObject = null;
    public float playerFindRadius;
    [SerializeField] LayerMask playerLayer;

    public float attackRange;
    Coroutine attackCoroutine = null;
    public int damage;

    bool isMoving;
    Animator enemyAnimator;

    Coroutine movePatternCoroutine;

    AudioSource audio;

    enum EnemyType
    {
        Eyeball,
        Overseer,
        THATOne
    }

    [SerializeField] EnemyType enemyType;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyAnimator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        FindPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        FindPlayer();
        if (playerObject != null)
        {
            switch (enemyType)
            {
                case EnemyType.Eyeball:
                    {
                        MoveAtPlayer();
                        IsPlayerInMeleeRange();
                        break;
                    }
                case EnemyType.Overseer:
                    {
                        if (movePatternCoroutine == null)
                        {
                            movePatternCoroutine = StartCoroutine(OverseerStep());
                        }
                        IsPlayerInMeleeRange();
                        break;
                    }
            }
        }
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

    private IEnumerator OverseerStep()
    {
        Vector2 stepDirection = (playerObject.transform.position - transform.position).normalized;
        WalkAnimation(stepDirection);
        yield return new WaitForSeconds(1.0f);
        enemyAnimator.SetBool("IsMoving", true);
        rb.velocity = stepDirection * moveSpeed;
        yield return new WaitForSeconds(0.67f);
        rb.velocity = Vector2.zero;
        enemyAnimator.SetBool("IsMoving", false);
        yield return new WaitForSeconds(1.5f);
        movePatternCoroutine = null;
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
            bool inMeleeRange = Physics2D.OverlapCircle(transform.position, attackRange, playerLayer);
            if (inMeleeRange && attackCoroutine == null)
            {
                attackCoroutine =  StartCoroutine(AttackCoroutine());
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public void PlayAudio()
    {
        audio.Play();
    }
}
