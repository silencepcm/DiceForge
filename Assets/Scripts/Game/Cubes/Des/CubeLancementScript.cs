using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeLancementScript : MovingObjectScript
{
    private List<Vector3> rotations;
    private int position;
    private bool startedAnim;
    private bool UpFlight = true;
    private Vector3 startPos;
    private Vector3 upPosition;
    private float lineJump;
    private bool choice;
    public float cubeNum = 0f;
    public GameObject[] plastines = new GameObject[6];
    private Player quiLance = null;
    private float baseSpeed;
    private void Awake()
    {
        rotations = new List<Vector3>
        {
            new Vector3(0f, 0f, 0f),
            new Vector3(0f, 0f, 90f),
            new Vector3(0f, 0f, 180f),
            new Vector3(0f, 0f, 270f),
            new Vector3(90f, 0f, 0f),
            new Vector3(-90f, 0f, 0f),
        };
    }
    protected override void Start()
    {
        base.Start();
        position = 1;
        lineJump = 300f;
        startPos = transform.position;
        upPosition = startPos;
        upPosition.y += lineJump;
        moveSpeed = 5f;
        baseSpeed = moveSpeed;
        speedRotateToAngle = 4f;
        speedRotate = 10f;
        choice = false;
    }
    protected override void Update()
    {
        base.Update();
        if ((!getMoving()) && (moveSpeed != baseSpeed))
        {
            moveSpeed = baseSpeed;
        }


        if (startedAnim)
        {
            if (UpFlight)
            {
                if (!getMoving())
                {
                    UpFlight = false;
                    setTargetPos(startPos);
                }

            } else
            {
                if ((transform.position.y - startPos.y <= 15f) && (!rotatingToAngle())) //Rotate to our angle
                {
                    setRotAngle(Quaternion.Euler(rotations[position - 1]));
                }
                else
                if (Vector3.Distance(transform.position, startPos) < 1.5f)
                {
                    startedAnim = false;
                    BuyElementScript won = plastines[position - 1].GetComponent<BuyElementScript>();
                    quiLance.uploadRessourses(won.goldCoin, won.redCoin, won.blueCoin, won.greenCoin);
                    GameObject.Find("GameUI").GetComponent<GameUI>().resetLancing();
                }
            }
        }
    }

    public void lancer(Player player)
    {
        GameObject.Find("GameUI").GetComponent<GameUI>().setLancing();
        startedAnim = true;
        UpFlight = true;
        setTargetPos(upPosition);
        startRotate();
        position = Random.Range(1, 6);
        quiLance = player;
    }
    public void SetChoiceState()
    {
        if (!startedAnim)
        {
            float enterCubesLine = 0;
            if (cubeNum == 1){
                enterCubesLine = -1;
            }
            else
            {
                enterCubesLine = 1;
            }
            choice = true;
            setTargetPos(Camera.main.transform.position +
                Camera.main.transform.forward * 5f - Camera.main.transform.right * 200f+ Camera.main.transform.up * -100f * enterCubesLine);
            setRotAngle(Camera.main.transform.rotation);
        }
    }
    public void setRotationOtherPlate()
    {
        for(int i = 0; i< rotations.Count; ++i)
        {
            rotations[i] = new Vector3(rotations[i].x, rotations[i].y - 180f, rotations[i].z);
        }
    }
    public void uploadBase(Vector3 uploader, float speed)
    {
        moveSpeed = speed;
        setTargetPos(transform.position + uploader);
        startPos = transform.position + uploader;
        upPosition = startPos;
        upPosition.y += lineJump;
    }
}
