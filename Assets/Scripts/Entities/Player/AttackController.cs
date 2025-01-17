using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{

    // Pickaxe attack parameters
    [SerializeField] float attackRange;
    [SerializeField] float hitRadius;
    [SerializeField] int attackDamage;

    [SerializeField] AudioSource swooshSound;

    /// <summary>
    /// A normalised vector in the direction cursor is from player
    /// </summary>
    Vector2 mouseDirection;

    /// <summary>
    /// Current attack coroutine playing
    /// </summary>
    Coroutine currentAttackCoroutine = null;

    [SerializeField] LayerMask hittableObjectsLayer;

    Animator playerAnimations;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimations = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        mouseDirection = ((Vector2)(Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized).normalized;

        if (Input.GetMouseButtonDown(0) && currentAttackCoroutine == null)
        {
            currentAttackCoroutine = StartCoroutine(nameof(PickaxeAttack));
        }
    }


    /// <summary>
    /// Plays out entire attack, with delay to account for attack windup
    /// </summary>
    /// <returns></returns>
    public IEnumerator PickaxeAttack()
    {
        swooshSound.Play();
        Vector2 direction = mouseDirection;
        playerAnimations.SetFloat("MouseDirectionX", direction.x);
        playerAnimations.SetFloat("MouseDirectionY", direction.y);
        playerAnimations.SetTrigger("PickaxeAttack");
        yield return new WaitForSeconds(0.2f);

        PickaxeHit();

        yield return new WaitForSeconds(0.3f);
        
        currentAttackCoroutine = null;

    }

    /// <summary>
    /// Finds and lists everyrthing that is hit
    /// </summary>
    private void PickaxeHit()
    {
        Vector2 hitPosition = (Vector2)transform.position + mouseDirection * attackRange;
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(hitPosition, hitRadius, hittableObjectsLayer);

        foreach (Collider2D hitCollider in hitColliders)
        {
            GameObject hitObject = hitCollider.gameObject;
            EntityHealth health = hitObject.GetComponent<EntityHealth>();
            health.ChangeHealth(-attackDamage);
        }
    }


    private void OnDrawGizmos()
    {
        Vector2 hitPosition = (Vector2)transform.position + mouseDirection * attackRange;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(hitPosition, hitRadius);
    }


}
