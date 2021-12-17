using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMovementScript : MonoBehaviour
{
    protected Vector3 spawn;
    public Vector3 generalState;
    protected Vector3 target;
    public bool activated;
    protected bool moving;
    protected float speed;
    protected virtual void Start()
    {
        spawn = transform.position;
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
        target = Camera.main.transform.position + Camera.main.transform.forward * 10f;
        transform.rotation = Camera.main.transform.rotation;
        activateMod();
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
