using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldableObject : MonoBehaviour, iHoldable, IInteractable
{
    public float weight { get; set; }

    private void Awake()
    {
        weight = 5f;
    }

    public void Interact()
    {
        GenerationManager.player.PickUpItem(gameObject);
    }
}

public interface iHoldable
{
    public float weight { get; set; }
}