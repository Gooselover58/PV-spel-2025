using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Headlamp : MonoBehaviour
{

    public GameObject lightCone;
    public GameObject lightCircle;

    SpriteRenderer coneSprite;
    SpriteRenderer circleSprite;

    float lerpTime = 0;


    // Start is called before the first frame update
    void Start()
    {
        coneSprite = lightCone.GetComponent<SpriteRenderer>();
        circleSprite = lightCircle.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            if (lerpTime < 1)
            {
                lerpTime += Time.deltaTime * 10;
            }
            else if (lerpTime > 1)
            {
                lerpTime = 1;
            }
        }
        else
        {
            if (lerpTime > 0)
            {
                lerpTime -= Time.deltaTime * 10;
            }
            else if (lerpTime < 0)
            {
                lerpTime = 0;
            }
        }
        FollowMouse();
        ChangeConeScale();
        ChangeConeRotation();
    }


    private void ChangeConeScale()
    {
        float distance = (circleSprite.transform.position - (transform.position + new Vector3(0, 0.6f, 0))).magnitude;
        lightCone.transform.localScale = new Vector2(3.8f, distance);
    }


    private void ChangeConeRotation()
    {
        Vector2 direction = lightCircle.transform.position - (transform.position + new Vector3(0, 0.6f, 0));
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        lightCone.transform.rotation = Quaternion.AngleAxis(angle -90, Vector3.forward);
    }

    private void FollowMouse()
    {
        Vector2 mousePos = ((Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10)));
        lightCircle.transform.position = Vector2.Lerp(transform.position, mousePos, lerpTime);
        lightCone.transform.position = lightCircle.transform.position;
    }

    private void PutCircleOnPLayer()
    {
        lightCircle.transform.position = transform.position;
    }
}
