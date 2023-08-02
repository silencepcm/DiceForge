using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlastineScript : MovingObjectScript
{
    public enum Statement { Mag, Choice, Cube, Table};

    public Statement state;
    private float distance;
    private float rightDistance;
    private int cubeNum = 1;
    private bool calivratedToCube = false;

    protected override void Start()
    {
        base.Start();
        moveSpeed = 5f;
        speedRotateToAngle = 5f;
        distance = 3f * transform.localScale.x;
        rightDistance = 2f * transform.localScale.x;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if(state == Statement.Choice)
        {

        }
        else if (Input.GetKey(KeyCode.KeypadEnter))
        {
            switch (state)
            {
                case Statement.Mag:
                    state = Statement.Choice; break;
                case Statement.Cube:
                    state = Statement.Table; break;
            }
        }
        if((state == Statement.Cube))
        {
            if ((!calivratedToCube)&&(transform.position == transform.parent.position - transform.parent.forward * transform.parent.localScale.x))
            {
                GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
                if (cubeNum == 1)
                {
                    transform.SetParent(gameManager.players[gameManager.getTour()-1].plateau.transform.Find("CubeOne"));
                } else if(cubeNum == 2)
                {
                    transform.SetParent(gameManager.players[gameManager.getTour()-1].plateau.transform.Find("CubeTwo"));
                    //gameManager.players[gameManager.tour].plateau.transform.Find("CubeTwo").GetComponent<CubeLancementScript>().plastines
                }
                calivratedToCube = true;
            }
        }
    }

    public void setMagState()
    {

    }
    public void setCubeState(int cube)
    {
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (cube == 1)
        {
            setTargetPos(gameManager.players[gameManager.getTour()-1].plateau.transform.Find("CubeOne").transform.position - gameManager.players[gameManager.getTour()-1].plateau.transform.Find("CubeOne").transform.forward);
        } else if (cube == 2)
        {
            setTargetPos(gameManager.players[gameManager.getTour()-1].plateau.transform.Find("CubeTwo").transform.position - gameManager.players[gameManager.getTour()-1].plateau.transform.Find("CubeOne").transform.forward);
        }
    }
    public void setChoiceState()
    {
        setTargetPos(Camera.main.transform.position +
    Camera.main.transform.forward * distance + Camera.main.transform.right * rightDistance);
    }
    public void actionPlastine()
    {
        switch (transform.name)
        {
            case "X3":
                X3action(); break;

        }
    }
    private void X3action()
    {
        if (transform.parent.name == "CubeOne")
        {
            //transform.parent.parent.Find("CubeTwo").transform.Find("")
        }
    }
}

