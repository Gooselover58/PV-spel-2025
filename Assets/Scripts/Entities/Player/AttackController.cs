using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{

    [SerializeField] float attackRange;
    [SerializeField] float hitRadius;
    [SerializeField] int attackDamage;

    Vector2 mouseDirection;

    [SerializeField] LayerMask hittableObjectsLayer;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mouseDirection = ((Vector2)(Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized).normalized;
    }



    private IEnumerator PickaxeAttack()
    {

        yield return new WaitForSeconds(0.3f);

        PickaxeHit();
    }

    private void PickaxeHit()
    {
        Vector2 hitPosition = (Vector2)transform.position + mouseDirection * attackRange;
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(hitPosition, hitRadius, hittableObjectsLayer);

        foreach (Collider2D hitCollider in hitColliders)
        {
            print(hitCollider.gameObject.name);
        }
    }

    private void OnDrawGizmos()
    {
        Vector2 hitPosition = (Vector2)transform.position + mouseDirection * attackRange;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(hitPosition, hitRadius);
    }


}
