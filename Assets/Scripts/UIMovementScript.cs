using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIMovementScript : MonoBehaviour
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
    public float moveSpeed=6f;
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

    RectTransform tform;
    public bool isUI = true;

    protected virtual void Start()
    {
        tform = GetComponent<RectTransform>();
        init();
    }

    public virtual void init()
    {
        actualDistance = 0f;
        inputDistanceLimit = 30f;
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
        switch (tform.tag)
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
        if (state != null)
        {
            state.Invoke();
        }
    }
    protected float getDistanceMove()
    {
        return Vector3.Distance(tform.anchoredPosition, target);
    }
    protected float getRotatingDistance()
    {
        return Vector3.Distance(tform.rotation.eulerAngles, targetRotation.eulerAngles);
    }
    protected float getRescalingDistance()
    {
        return Vector3.Distance(tform.localScale, targetScale);
    }

    public void setTargetPos(Vector3 pos)
    {
        target = pos;
        state += moving;
        isMoving = true;
        lerpMovingCount = 0f;
        canMove = false;
        actualDistance = Vector3.Distance(tform.anchoredPosition, target);
    }
    void moving()
    {
        tform.anchoredPosition = Vector3.Lerp(tform.anchoredPosition, target, lerpMovingCount);
        actualDistance = Vector3.Distance(tform.anchoredPosition, target);
        Debug.Log("Target: " + target + "    Position: " + tform.position);
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
        tform.anchoredPosition = target;
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
        tform.rotation = Quaternion.Slerp(tform.rotation, targetRotation, lerpRotateToAngleCount);

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
        tform.rotation = targetRotation;
        lerpRotateToAngleCount = 0f;
        isRotatingToAngle = false;
        state -= rotatingToAngle;
    }
    public void setScale(Vector3 scale)
    {
        targetScale = scale;
        isScaling = true;
        state += scaling;
        lerpScalingCount = 0f;
        canRescale = false;
    }
    void scaling()
    {
        tform.localScale = Vector3.Lerp(tform.localScale, targetScale, rescaleSpeed * Time.fixedDeltaTime);
        if ((!canRescale) && (Vector3.Distance(tform.localScale, targetScale) <= rescaleEndSkip))
        {
            // canRescale = true;
        }
        if (Vector3.Distance(tform.localScale, targetScale) < 1f)
        {
            stopScaling();
        }
    }
    public void stopScaling()
    {
        tform.localScale = targetScale;
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
        tform.Rotate(rotationToAdd);
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
