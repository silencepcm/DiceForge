using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObjectScript : MonoBehaviour
{
    private Vector3 targetScale;
    private Quaternion targetRotation;
    private Vector3 target;
    private float actualDistance;
    protected bool canMove;
    protected bool canRescale;
    protected bool canRotate;

    protected bool active;
    protected float moveEndSkip = 1f;
    protected float rotateEndSkip = 1f;
    protected float rescaleEndSkip = 1f;
    protected float inputDistanceLimit;
    protected float moveSpeed;
    protected float speedRotateToAngle;
    protected float rescaleSpeed;
    private bool moving;
    private bool rotateToAngle;
    private bool rescaling;
    private bool rotate;
    protected float speedRotate;

    protected virtual void Start()
    {
        init();
    }

    public virtual void init()
    {
        actualDistance = 0f;
        inputDistanceLimit = 30f;
        moveSpeed = 6f;
        canMove = true;
        canRescale = true;
        canRotate = true;
        moving = false;
        rotateToAngle = false;
        rescaling = false;
        active = true;
        speedRotate = 6f;
        speedRotateToAngle = 0.1f;
        rotate = false;
    }

    // Update is called once per frame
    protected virtual void Update()
    {

        if (active)
        {
            if (moving)
            {
                transform.position = Vector3.Lerp(transform.position, target, moveSpeed * Time.fixedDeltaTime);
                actualDistance = Vector3.Distance(transform.position, target);
                if ((!canMove) && (actualDistance < inputDistanceLimit))
                {
                    canMove = true;
                }
                if (actualDistance < moveEndSkip)
                {
                    transform.position = target;
                    moving = false;
                }
            }
            if (rotateToAngle)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speedRotateToAngle * Time.deltaTime);
                if ((!canRotate) && (Vector3.Distance(transform.rotation.eulerAngles, targetRotation.eulerAngles)< rotateEndSkip))
                {
                    canRotate = true;
                }
                if (transform.rotation.eulerAngles == targetRotation.eulerAngles)
                {
                    rotateToAngle = false;
                }
            }
            if (rescaling)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, targetScale, rescaleSpeed * Time.fixedDeltaTime);
                if ((!canRescale) && (Vector3.Distance(transform.localScale, targetScale) < rescaleEndSkip))
                {
                    canRescale = true;
                }
                if (transform.localScale == targetScale)
                {
                    transform.localScale = targetScale;
                    rescaling = false;
                }
            }
            if (rotate)
            {
                Vector3 rotationToAdd = new Vector3(0, speedRotate, speedRotate);
                transform.Rotate(rotationToAdd);
            }
        }
    }
    protected float getDistanceMove()
    {
        return Vector3.Distance(transform.position, target);
    }
    protected float getRotatingDistance()
    {
        return Vector3.Distance(transform.rotation.eulerAngles, targetRotation.eulerAngles);
    }
    protected float getRescalingDistance()
    {
        return Vector3.Distance(transform.localScale, targetScale);
    }

    public void setTargetPos(Vector3 pos)
    {
        target = pos;
        moving = true;
        canMove = false;
        actualDistance = Vector3.Distance(transform.position, target);
    }
    public void setRotAngle(Quaternion angle)
    {
        targetRotation = angle;
        rotateToAngle = true;
        canRotate = false;
        rotate = false;
    }
    public void setScale(Vector3 scale) {
        targetScale = scale;
        rescaling = true;
        canRescale = false;
    }


    public void startRotate()
    {
        rotate = true;
        rotateToAngle = false;
    }
    public void stopRotate()
    {
        rotate = false;
    }
    public bool getMoving()
    {
        return moving;
    }
    public bool getRotating()
    {
        return rotate;
    }
    public bool getRotatingToAngle()
    {
        return rotateToAngle;
    }
    public Vector3 getTarget()
    {
        return target;
    }
    public Quaternion getTargetRotation()
    {
        return targetRotation;
    }
}
