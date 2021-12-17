using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameMovementScript : MovingObjectScript
{
    public GameObject actualObjattached;
    private int statementPos;
    private Vector3 cardScale;
    private Vector3 coinScale;
    private char objTypeAttached;
    private GameManager gameManager;

    protected override void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        base.Start();
        transform.position = new Vector3(actualObjattached.transform.position.x, actualObjattached.transform.position.y+1, actualObjattached.transform.position.z);
        cardScale = new Vector3(17f, 16.5f, 1f);
        coinScale = new Vector3(5f, 3f, 1f);
        if (actualObjattached.tag == "Card")
        {
            setScale(cardScale);
        } else
        {
            setScale(coinScale);
        }
        typeUpdate();
        active = true;
        moveSpeed = 6f;
        speedRotateToAngle = 4f;
        rescaleSpeed = 4f;
    }

    protected override void Update()
    {
        base.Update();
            if (canMove)
            {
                    if (Input.GetKey(KeyCode.KeypadEnter))
                {
                    gameManager.players[gameManager.getTour()-1].buy(actualObjattached);
                }
            }
    }

    public void activate()
    {
        this.gameObject.SetActive(true);
        active = true;
    }
    public void desactivate()
    {
        this.gameObject.SetActive(false);
        active = false;
    }
    public int getStatementPos()
    {
        return statementPos;
    }
    public void typeUpdate()
    {
        switch (actualObjattached.tag)
        {
            case "Card":
                objTypeAttached = 'C';
                if (transform.localScale != cardScale)
                {
                    setScale(cardScale);
                    moveEndSkip = 5f;
                    inputDistanceLimit = 40f;
                    moveSpeed = 6f;
                }
                break;
            case "Coin":
                objTypeAttached = 'M';
                if (transform.localScale != coinScale)
                {
                    setScale(coinScale);
                    moveEndSkip = 5f;
                    inputDistanceLimit = 20f;
                    moveSpeed = 3f;
                }
                break;
        }
    }
    public char getObjAttachedType()
    {
        return objTypeAttached;
    }
    public int getNewcamState()
    {
        if (active)
        {
            switch (objTypeAttached) {
                case 'C':
                    return(actualObjattached.GetComponent<SelectInfoScript>().camState);
                    
                case 'M':
                    return 7;
                default:
                    return 5;
            }
        } else
        {
            return gameManager.getTour()-1;
        }
    }
    public void setDirection(string dir)
    {
        if (canMove)
        {
            switch (dir)
            {
                case "Left":
                    if (actualObjattached.GetComponent<SelectInfoScript>().leftArrow)
                    {
                        actualObjattached = actualObjattached.GetComponent<SelectInfoScript>().leftArrow;
                    }
                    break;
                case "Right":
                    if (actualObjattached.GetComponent<SelectInfoScript>().rightArrow)
                    {
                        actualObjattached = actualObjattached.GetComponent<SelectInfoScript>().rightArrow;

                    }
                    break;
                case "Up":
                    if (actualObjattached.GetComponent<SelectInfoScript>().upArrow)
                    {
                        actualObjattached = actualObjattached.GetComponent<SelectInfoScript>().upArrow;
                    }
                    break;
                case "Down":
                    if (actualObjattached.GetComponent<SelectInfoScript>().downArrow)
                    {
                        actualObjattached = actualObjattached.GetComponent<SelectInfoScript>().downArrow;
                    }
                    break;
            }
        }
        if (actualObjattached)
        {
            setTargetPos(new Vector3(actualObjattached.transform.position.x, actualObjattached.transform.position.y + 1, actualObjattached.transform.position.z));
            setRotAngle(actualObjattached.transform.rotation);
            typeUpdate();
        }
    }
}