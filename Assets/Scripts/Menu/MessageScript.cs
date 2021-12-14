using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageScript : UIMovementScript
{
    protected override void Start()
    {
        spawn = transform.position;
        generalState = new Vector3(0f, -400f, 0f);
        speed = 3f;
        moving = false;
        activated = false;
    }
}
