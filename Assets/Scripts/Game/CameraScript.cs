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
    private List<int> stateOrder;
    private void Awake()
    {
        stateOrder = new List<int>();
    }

    protected override void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        base.Start();
        speedRotateToAngle = 1.5f;
        state = 0;
        height = 1800f;

        positions = new List<Vector3>
        {
            new Vector3(2300f, height, GameObject.Find("plateauNoir").transform.position.z),
            new Vector3(90f, height, GameObject.Find("plateauRouge").transform.position.z),
            new Vector3(180f, height, GameObject.Find("plateauVert").transform.position.z),
            new Vector3(-90f, height, GameObject.Find("plateauBleu").transform.position.z),
            new Vector3(0f, height, 2280f),
            new Vector3(0f, height, 310f),
            new Vector3(0f, height, -1580f)
        };
        rotations = new List<Quaternion>
        {
            Quaternion.Euler(60f, 270f, 0f),
            Quaternion.Euler(60f, 270f, 0f),
            Quaternion.Euler(60f, 90f, 0f),
            Quaternion.Euler(60f, 90f, 0f),
            Quaternion.Euler(60f, 180f, 0f),
            Quaternion.Euler(60f, 180f, 0f),
            Quaternion.Euler(60f, 180f, 0f)
        };
        moveSpeed = 1.5f;
        moveEndSkip = 0.2f;
        uploadState(1, false);
        waitAction = false;
    }

    protected override void Update()
    {
        base.Update();
        if ((waitAction)&&(getMoving())&&(Vector3.Distance(transform.position, getTarget()) < 200f))
        {
            waitAction = false;
            gameManager.setAction();
        }
        if ((!getMoving()) && (stateOrder.Count > 0))
        {
            state = stateOrder[0];
            Debug.Log(state);
            setTargetByState();
            stateOrder.RemoveAt(0);
        }
    }

    public void uploadState(int state, bool wait)
    {
        if (wait)
        {
            stateOrder.Add(state);
        }
        else
        {
            stateOrder.Clear();
            this.state = state;
            setTargetByState();
        }
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
}
