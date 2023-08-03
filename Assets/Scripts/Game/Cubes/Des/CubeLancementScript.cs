using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CubeLancementScript : MovingObjectScript
{
    private List<Vector3> rotations;
    private int position;
    private bool startedAnim;
    private bool isUpFlight = true;
    private Vector3 startPos;
    private Vector3 upPosition;
    private float lineJump;
    public float cubeNum = 0f;
    public GameObject[] plastines = new GameObject[6];
    private Player quiLance = null;
    private float baseSpeed;
    private Quaternion otherSideCubesRotMod;
    private bool unoRotation = false;
    private float unoRotWaiter = 0f;
    UnityAction state;
    private void Awake()
    {
        rotations = new List<Vector3>
        {
            new Vector3(0f, 0f, 0f),
            new Vector3(0f, 0f, -90f),
            new Vector3(0f, 0f, 180f),
            new Vector3(0f, 0f, -270f),
            new Vector3(90f, 0f, 0f),
            new Vector3(-90f, 0f, 0f),
        };
        otherSideCubesRotMod = Quaternion.Euler(new Vector3(0f, 1f, 0f));
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
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (state != null)
        {
            state.Invoke();
        }
        if((otherSideCubesRotMod.eulerAngles.y == 1f)&&(quiLance != null))
        {
            if ((quiLance.getPlayerNum() == 3) || (quiLance.getPlayerNum() == 4))
            {
                otherSideCubesRotMod.y = 180f;
            } else
            {
                otherSideCubesRotMod.y = 0f;
            }
        }
        if ((!getMoving()) && (moveSpeed != baseSpeed))
        {
            moveSpeed = baseSpeed;
        }
        if (unoRotation)
        {
            unoRotWaiter += Time.fixedTime;
            if (unoRotWaiter > 1700f)
            {
                unoRotation = false;
                unoRotWaiter = 0f;
            }
        }
  
    }
    void UpFlight()
    {
        if (!getMoving())
        {
            state -= UpFlight;
            isUpFlight = false;
            StartCoroutine(DownWaiter());
        }
    }
    IEnumerator DownWaiter()
    {
        yield return new WaitForSeconds(3f);
        setTargetPos(startPos);
        state += DownFlight;
    }
    void DownFlight()
    {
        if ((transform.position.y - startPos.y <= 15f) && (!getRotatingToAngle())) //Rotate to our angle
        {
            stopRotate();
            setRotAngle(Quaternion.Euler(rotations[position - 1] + otherSideCubesRotMod.eulerAngles));
        }
        else
                  if (Vector3.Distance(transform.position, getTarget()) < 1.5f)
        {
            startedAnim = false;
            state = null;
            BuyElementScript won = plastines[position - 1].GetComponent<BuyElementScript>();
            quiLance.uploadRessourses(won.goldCoin, won.redCoin, won.blueCoin, won.greenCoin, true);
            //  Debug.Log("Gold: " + won.goldCoin + ",  " + "Red: " + won.redCoin + ",  " + "Blue: " + won.blueCoin + ",  " + "Green: " + won.greenCoin );
            GameObject.Find("GameUI").GetComponent<GameUI>().resetLancing();
        }
    }
    public void lancer(Player player)
    {
        GameObject.Find("GameUI").GetComponent<GameUI>().setLancing();
        startedAnim = true;
        isUpFlight = true;
        state += UpFlight;
        setTargetPos(upPosition);
        startRotate();
        position = 4;
        quiLance = player;
    }
    public int getStatePosition()
    {
        return position;
    }
    public void SetChoiceState()
    {
        if (!startedAnim)
        {
            float enterCubesLine = cubeNum == 1? -1:1;

            CameraScript cam = Camera.main.GetComponent<CameraScript>();
            Vector3 targetToCamera = new Vector3(cam.getTarget().x + 150f, cam.getTarget().y-800f, cam.getTarget().z+ 150f * enterCubesLine);
            setTargetPos(targetToCamera);
            setRotAngle(Quaternion.Euler(-40f, 30f, -15f));
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
    public GameObject getActualPlastine()
    {
        return plastines[position - 1];
    }
    public void setActualPlastine(GameObject plastine)
    {
        
        plastines[position - 1] = plastine;
    }
    public Vector3 getStartPos()
    {
        return startPos;
    }
    public Vector3 getActualRotation()
    {
        return rotations[position - 1];
    }
    public void setActualRotation()
    {
        setRotAngle(Quaternion.Euler(rotations[position - 1]));
    }

    public void rotateUno(int position)
    {
        if (!unoRotation)
        {
            unoRotation = true;
            setRotAngle(Quaternion.Euler(rotations[position - 1]));
            this.position = position;
            Debug.Log(position);
        }
    }
    public int getPosition()
    {
        return position;
    }
}
