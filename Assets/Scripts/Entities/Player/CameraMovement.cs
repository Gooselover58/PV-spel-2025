using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    [SerializeField] GameObject playerObj;
    [SerializeField] float maxMouseFollowOffset;

    /// <summary>
    /// If true: targetPosition will follow player. If false: targetPosition will be still, and go where instructed by MoveTargetTo()
    /// </summary>
    public bool isFollowingPlayer;

    private Vector2 targetPosition;
    private Vector2 lastPosition;
    public Vector2 velocity;


    // Start is called before the first frame update
    void Start()
    {
        FindPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        FindPlayer();
        if (Input.GetKeyDown(KeyCode.L))
        {
            isFollowingPlayer = !isFollowingPlayer;
        }

        if (isFollowingPlayer)
        {
            PlayerFollow();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            MoveTargetTo(new Vector2(5, 5));
        }

        MoveCamera();
    }


    /// <summary>
    /// Moves targetPosition to a position near the playerm influenced by the mouse position
    /// </summary>
    void PlayerFollow()
    {
        if (playerObj != null)
        {
            targetPosition = playerObj.transform.position + (Camera.main.ScreenToWorldPoint(Input.mousePosition) - playerObj.transform.position).normalized *
            Mathf.Clamp((Camera.main.ScreenToWorldPoint(Input.mousePosition) - playerObj.transform.position).magnitude / 4, 0, maxMouseFollowOffset);
        }
    }

    /// <summary>
    /// Moves targetPosition to the position of newTarget
    /// </summary>
    /// <param name="newTarget"></param>
    public void MoveTargetTo(Vector2 newTarget)
    {
        targetPosition = newTarget;
    }


    /// <summary>
    /// Moves the camera with Smoothing to targetPosition
    /// </summary>
    void MoveCamera()
    {
        velocity = (Vector2)transform.position - lastPosition;
        lastPosition = transform.position;
        transform.position = (Vector3)Vector2.SmoothDamp(transform.position, targetPosition, ref velocity, 0.065f) + Vector3.back * 10;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawCube(targetPosition, new Vector2(0.5f, 0.5f));
    }

    void FindPlayer()
    {
        if (playerObj == null)
        {
            playerObj = GameObject.FindGameObjectWithTag("Player");
        }
    }

}
