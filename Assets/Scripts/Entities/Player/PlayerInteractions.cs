using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    private TestPlayer player;
    private IInteractable curInteraction;
    [SerializeField] float interactionRange;

    private void Awake()
    {
        player = GetComponent<TestPlayer>();
    }

    private void Update()
    {
        CheckForInteractions();
        CheckForPlayerInput();
    }

    private void CheckForInteractions()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, interactionRange);
        float lowestDist = interactionRange;
        foreach (Collider2D col in cols)
        {
            if (col.GetComponent<IInteractable>() == null)
            {
                continue;
            }
            float distance = (transform.position - col.transform.position).magnitude;
            if (distance < lowestDist)
            {
                curInteraction = col.GetComponent<IInteractable>();
            }
        }
    }

    private void CheckForPlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.E) && curInteraction != null)
        {
            curInteraction.Interact();
            curInteraction = null;
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            player.DropItem();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}
