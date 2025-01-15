using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Interaction : MonoBehaviour, IInteractable
{
    public virtual void Interact()
    {

    }
}

public interface IInteractable
{
    public void Interact();
}