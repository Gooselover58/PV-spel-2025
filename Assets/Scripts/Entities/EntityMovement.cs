using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMovement : MonoBehaviour
{


    
    public float moveSpeed;

    public float sprintSpeed;
    protected float sprintSpeedReset;

    /// <summary>
    /// Accumulated velocity to be normalized
    /// </summary>
    protected Vector2 accumulatedVelocity;

    Rigidbody2D entityRB2D;

    protected void GetRigidBody()
    {
        entityRB2D = GetComponent<Rigidbody2D>();
    }


    protected void MoveUp()
    {
        accumulatedVelocity += Vector2.up;
    }

    protected void MoveDown()
    {
        accumulatedVelocity += Vector2.down;
    }

    protected void MoveLeft()
    {
        accumulatedVelocity += Vector2.left;
    }

    protected void MoveRight()
    {
        accumulatedVelocity += Vector2.right;
    }



    protected void ApplyVelocity()
    {
        entityRB2D.velocity = accumulatedVelocity.normalized * (moveSpeed + sprintSpeed);
        accumulatedVelocity = Vector2.zero;
    }

}
    