using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MovingObjectScript
{
    private GameManager gameManager;
    private float height;
    [Range(1,7)]
    private int state;


    private bool waitAction = false;

    private List<Vector3> positions;
    private List<Quaternion> rotations;

    private Quaternion rotMemory;

    private void Awake()
    {
        height = 1800f;
        positions = new List<Vector3>
        {
            new Vector3(2300f, height, GameObject.Find("plateauNoir").transform.position.z),
            new Vector3(-2300f, height, GameObject.Find("plateauBleu").transform.position.z),
            new Vector3(-2300f, height, GameObject.Find("plateauVert").transform.position.z),
            new Vector3(2300f, height, GameObject.Find("plateauRouge").transform.position.z),
            new Vector3(0f, height, 2280f),
            new Vector3(0f, height, 310f),
            new Vector3(0f, height, -1580f)
        };
        rotations = new List<Quaternion>
        {
            Quaternion.Euler(60f, 270f, 0f),
            Quaternion.Euler(60f, 90f, 0f),
            Quaternion.Euler(60f, 90f, 0f),
            Quaternion.Euler(60f, 270f, 0f),
            Quaternion.Euler(60f, 180f, 0f),
            Quaternion.Euler(60f, 180f, 0f),
            Quaternion.Euler(60f, 180f, 0f)
        };
    }
    protected override void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        base.Start();
        speedRotateToAngle = 1.5f;
        state = 0;
        moveSpeed = 1.5f;
        moveEndSkip = 0.2f;
        uploadState(1);
        waitAction = true;
        rotMemory = transform.rotation;
    }

    protected override void Update()
    {
        base.Update();
        if ((waitAction)&&(getMoving())&&(Vector3.Distance(transform.position, getTarget()) < 200f))
        {
            waitAction = false;
            gameManager.setCameraReady();
            Debug.Log("READY");
        }
    }

    public void uploadState(int state)
    {
        waitForAction();
        this.state = state;
        setTargetByState();
    }

    private void setTargetByState()
    {
        setTargetPos(positions[state-1]);
        setRotAngle(rotations[state-1]);
    }
    public void waitForAction()
    {
        waitAction = true;
    }
    public void setChoiceCoin(GameObject coin)
    {
        gameManager.choiceCoin = coin;
        rotMemory = transform.rotation;
        setRotAngle(Quaternion.Euler(90f, 180f, 0f));
        coin.GetComponent<BuyElementScript>().coinToCamera();
    }
    public void resetChoiceCoin()
    {
        setRotAngle(rotMemory);
    }
}
