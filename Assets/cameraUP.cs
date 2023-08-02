using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraUP : MonoBehaviour
{
    public bool activateUP = false;
    public bool activateDown = false;
    float lerpingUP = 0f;
    float lerpingDOWN = 0f;
    public float valueUP = 20f;
     float valueDown;
    public float speed = 0.4f;
    float startY;
    private void Start()
    {
        valueDown = transform.position.y;
        startY = transform.position.y;
    }
    void FixedUpdate()
    {
        if(activateUP && lerpingUP < 1f)
        {
            Vector3 pos = transform.position;
            pos.y = Mathf.Lerp(startY, valueUP, lerpingUP);
            transform.position = pos;
            lerpingUP += Time.fixedDeltaTime * speed;
        } else if(activateDown&&lerpingDOWN < 1f)
        {
            Vector3 pos = transform.position;
            pos.y = Mathf.Lerp(20f, valueDown, lerpingDOWN);
            transform.position = pos;
            lerpingDOWN += Time.fixedDeltaTime * speed;
        }
    }
}
