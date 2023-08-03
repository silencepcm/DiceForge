using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    private bool isMoving;
    private float lerpMovingCount = 0f;
    private bool isRotatingToAngle;
    private float lerpRotateToAngleCount = 0f;
    private bool isScaling;
    private bool rotate;
    private float lerpRotateCount = 0f;

    protected float speedRotate;


    UnityAction state;

    public bool isUI = true;

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
        isMoving = false;
        isRotatingToAngle = false;
        isScaling = false;
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
        if(state != null)
        {
            state.Invoke();
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
        state += moving;
        isMoving = true;
        lerpMovingCount = 0f;
        canMove = false;
        actualDistance = Vector3.Distance(transform.position, target);
    }
    void moving()
    {
        transform.position = Vector3.Lerp(transform.position, target, lerpMovingCount);
        actualDistance = Vector3.Distance(transform.position, target);
        Debug.Log("Target: " + target + "    Position: " + transform.position);
        if (lerpMovingCount >= 1f)
        {
            stopMoving();
        }
        else
        {
            lerpMovingCount += moveSpeed * Time.fixedDeltaTime;
        }
    }
    public void stopMoving()
    {
        transform.position = target;
        lerpMovingCount = 0f;
        isMoving = false;
        state -= moving;
    }
    public void setRotAngle(Quaternion angle)
    {
        targetRotation = angle;
        isRotatingToAngle = true;
        state += rotatingToAngle;
        lerpRotateToAngleCount = 0f;
        canRotate = false;
        rotate = false;
    }
    void rotatingToAngle()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lerpRotateToAngleCount);

        if (lerpRotateToAngleCount >= 1f)
        {
            stopRotatingToAngle();
        }
        else
        {
            lerpRotateToAngleCount += speedRotateToAngle * Time.fixedDeltaTime;
        }
    }

    public void stopRotatingToAngle()
    {
        transform.rotation = targetRotation;
        lerpRotateToAngleCount = 0f;
        isRotatingToAngle = false;
        state -= rotatingToAngle;
    }
    public void setScale(Vector3 scale) {
        targetScale = scale;
        isScaling = true;
        state += scaling;
        lerpScalingCount = 0f;
        canRescale = false;
    }
    void scaling()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, rescaleSpeed * Time.fixedDeltaTime);
        if ((!canRescale) && (Vector3.Distance(transform.localScale, targetScale) <= rescaleEndSkip))
        {
           // canRescale = true;
        }
        if (Vector3.Distance(transform.localScale, targetScale) < 1f)
        {
            stopScaling();
        }
    }
    public void stopScaling()
    {
        transform.localScale = targetScale;
        state -= scaling;
        isScaling = false;
    }
    public bool getRescaling()
    {
        return isScaling;
    }
    public void startRotate()
    {
        rotate = true;
        state += rotating;
        isRotatingToAngle = false;
    }
    void rotating()
    {
        Vector3 rotationToAdd = new Vector3(0, speedRotate, speedRotate);
        transform.Rotate(rotationToAdd);
    }
    public void stopRotate()
    {
        rotate = false;
        state -= rotating;
    }
    public bool getMoving()
    {
        return isMoving;
    }
    public bool getRotating()
    {
        return rotate;
    }
    public bool getRotatingToAngle()
    {
        return isRotatingToAngle;
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
