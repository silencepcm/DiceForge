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
    private float lerpScalingCount = 0f;
    protected float inputDistanceLimit;
    protected float moveSpeed;
    protected float speedRotateToAngle;
    protected float rescaleSpeed;
    private bool moving;
    private float lerpMovingCount = 0f;
    private bool rotateToAngle;
    private float lerpRotateToAngleCount = 0f;
    private bool rescaling;
    private bool rotate;
    private float lerpRotateCount = 0f;

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
        switch (transform.tag)
        {
            case "MainCamera":
                rotateEndSkip = 0.3f;
                break;
            case "Coin":
                rotateEndSkip = 4f;
                break;
            case "Card":
                rotateEndSkip = 3f;
                break;
        }
    }

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {

        if (active)
        {
            if (moving)
            {
                transform.position = Vector3.Lerp(transform.position, target, lerpMovingCount);
                actualDistance = Vector3.Distance(transform.position, target);
                if (lerpMovingCount>=1f)
                {
                    transform.position = target;
                    lerpMovingCount = 0f;
                    moving = false;
                } else
                {
                    lerpMovingCount += moveSpeed * Time.fixedDeltaTime;
                }
            }
            if (rotateToAngle)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lerpRotateToAngleCount);

                if (lerpRotateToAngleCount >= 1f)
                {
                    transform.rotation = targetRotation;
                    lerpRotateToAngleCount = 0f;
                    rotateToAngle = false;
                }
                else
                {
                    lerpRotateToAngleCount += speedRotateToAngle * Time.fixedDeltaTime;
                }
            }
            if (rescaling)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, targetScale, rescaleSpeed * Time.fixedDeltaTime);
                if ((!canRescale) && (Vector3.Distance(transform.localScale, targetScale) <= rescaleEndSkip))
                {
                    canRescale = true;
                }
                if (Vector3.Distance(transform.localScale, targetScale) < 1f)
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
        lerpMovingCount = 0f;
        canMove = false;
        actualDistance = Vector3.Distance(transform.position, target);
    }
    public void setRotAngle(Quaternion angle)
    {
        targetRotation = angle;
        rotateToAngle = true;
        lerpRotateToAngleCount = 0f;
        canRotate = false;
        rotate = false;
    }
    public void setScale(Vector3 scale) {
        targetScale = scale;
        rescaling = true;
        lerpScalingCount = 0f;
        canRescale = false;
    }

    public bool getRescaling()
    {
        return rescaling;
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
