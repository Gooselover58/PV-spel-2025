using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : EntityMovement
{

    private void Awake()
    {
        GetRigidBody();
        sprintSpeedReset = sprintSpeed;
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.W))
        {
            MoveUp();
        }

        if (Input.GetKey(KeyCode.A))
        {
            MoveLeft();
        }

        if (Input.GetKey(KeyCode.S))
        {
            MoveDown();
        }

        if (Input.GetKey(KeyCode.D))
        {
            MoveRight();
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            sprintSpeed = sprintSpeedReset;
        }
        else
        {
            sprintSpeed = 0;
        }

        ApplyVelocity();

    }

    private void FixedUpdate()
    {



    }
}
