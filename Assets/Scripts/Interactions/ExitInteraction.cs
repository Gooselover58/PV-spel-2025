using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitInteraction : Interaction
{
    public override void Interact()
    {
        if (CartScript.isAtEnd)
        {

        }
        else
        {
            Debug.Log("The cart has not reached the end of the room...");
        }
    }
}
