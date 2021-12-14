using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMovementScipt : MonoBehaviour
{
    /* Variables */
    private Settings settings;
    private Vector2 spawn;
    private Vector3 targetPos;
    private GameUI gameUI;

    private bool rotation = false;
    private int actual_line_points;
    private int line_points;
    private int linePointsMax = 5;
    private float radius = 82f;
    private float z1, y1;
    private float angle;
    private bool uiWaiting = false;

    public bool RotationDirection = true;
    public int lineNum;
    public float speed = 1f;

    void Start()
    {
        gameUI = GameObject.Find("GameUI").GetComponent<GameUI>();
        settings = new Settings();
        uiWaiting = false;
        line_points = 0;
        actual_line_points = 0;
        switch (lineNum)
        {
            case 1:
                linePointsMax = 12; break;
            case 2:
                linePointsMax = 6; break;
            case 3:
                linePointsMax = 6; break;
            case 4:
                linePointsMax = 9; break;
            case 5:
                linePointsMax = 9; break;
        }
    }

    void Update()
    {
        if ((actual_line_points != line_points)&&(!rotation)&&(actual_line_points<linePointsMax))
        {
            reloadAngle();
            if ((!RotationDirection)&&(actual_line_points==1)|| (RotationDirection) && (actual_line_points == 0))
            {
                radius = settings.plateauFirstCase[transform.tag][lineNum];
            } else
            {
                radius = settings.plateauCase[transform.tag][lineNum];
            }
            spawn = new Vector2(transform.position.z - radius * Mathf.Cos(angle), transform.position.y - radius * Mathf.Sin(angle)); //VECTOR2(Z, Y);
            rotation = true;
        }
        if (rotation)
        {
            getTargetPosition();
            // Update Counter
            float target = RotationDirection ? -speed * Time.fixedDeltaTime : speed * Time.fixedDeltaTime;
            angle += target;
            if ((!RotationDirection&& angle > Mathf.PI)|| (RotationDirection && angle < 0))
            {
                rotation = false;
                actual_line_points += line_points>actual_line_points ? 1 : -1;
                getTargetPosition();
            }
            transform.position = Vector3.MoveTowards(transform.position, targetPos, 5f + speed);
        }

        if((actual_line_points == line_points) && (!rotation) && (uiWaiting))
        {
            uiWaiting = false;
            gameUI.resetCubeWait();
        }
    }
    private void getTargetPosition()
    {
        y1 = spawn.y + radius * Mathf.Sin(angle);
        z1 = spawn.x + radius * Mathf.Cos(angle);

        targetPos = new Vector3(transform.position.x, y1, z1);
    }
    private void reloadAngle()
    {
        if (transform.tag == "Bleu" || transform.tag == "Vert")
        {
            RotationDirection = actual_line_points < line_points ? false : true;
        }
        else 
        {
            RotationDirection = actual_line_points < line_points ? true : false;
        }
        angle = RotationDirection? Mathf.PI: 0f;

    }


    public void setPoints(int points)
    {
        if (line_points != points)
        {
            line_points = points;
            uiWaiting = true;
            gameUI.setCubeWait();
        }
    }
    public void upgradeLinepointsMax()
    {
        switch (lineNum)
        {
            case 1:
                linePointsMax += 4;
                break;
            case 2:
                linePointsMax += 3;
                break;
            case 3:
                linePointsMax += 3;
                break;
        }
    }

}
