using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitInteraction : Interaction
{
    public override void Interact()
    {
        if (CartScript.isAtEnd)
        {
            Debug.Log("Would go into next room if was implemented UwU");
        }
        else
        {
            Debug.Log("The cart has not reached the end of the room...");
        }
    }
}
