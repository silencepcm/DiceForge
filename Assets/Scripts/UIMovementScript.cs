using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMovementScript : MonoBehaviour
{
    protected Vector3 spawn;
    protected Vector3 generalState;
    protected Vector3 target;
    public bool activated;
    protected bool moving;
    protected float speed;
    protected virtual void Start()
    {
        spawn = new Vector3(0f, 0f, 0f);
        generalState = new Vector3(0f, 50f, 0f);
        moving = false;
        speed = 3f;
        activated = false;
    }

    void Update()
    {
        if (moving)
        {
            if (Vector3.Distance(target, transform.position) > 0.2f)
            {
                transform.position = Vector3.Lerp(transform.position, target, speed * Time.fixedDeltaTime);
            }
            else
            {
                transform.position = target;
                moving = false;
            }
        }
    }

    public void activate()
    {
        activated = true;
        moving = true;
        target = generalState;
        activateMod();
        target.x += GameObject.FindObjectOfType<Canvas>().transform.position.x;
        target.y += GameObject.FindObjectOfType<Canvas>().transform.position.y;
    }
    public void desactivate()
    {
        activated = false;
        moving = true;
        target = spawn;
        desactivateMod();
        target.x += GameObject.FindObjectOfType<Canvas>().transform.position.x;
        target.y += GameObject.FindObjectOfType<Canvas>().transform.position.y;
    }

    protected virtual void activateMod()
    {

    }
    protected virtual void desactivateMod()
    {

    }
}
