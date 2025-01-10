using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    [SerializeField] GameObject playerObj;
    [SerializeField] float maxMouseFollowOffset;
    private Vector2 targetPosition;
    private Vector2 lastPosition;
    public Vector2 velocity;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        PlayerFollow();



        MoveCamera();
    }


    void PlayerFollow()
    {
        targetPosition = playerObj.transform.position + (Camera.main.ScreenToWorldPoint(Input.mousePosition) - playerObj.transform.position).normalized * 
            Mathf.Clamp((Camera.main.ScreenToWorldPoint(Input.mousePosition) - playerObj.transform.position).magnitude / 4, 0, maxMouseFollowOffset);
    }

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


}
