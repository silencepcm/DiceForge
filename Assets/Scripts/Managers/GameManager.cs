using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum State { StartInit, PlayerLancement, Buy, ChangeDes, ActionSup, EndGame, Menu}
    private State state;

    [Header("Game")]
    public bool gameOver = false;           //Set true when the game is over
    private int tour = 1;
    private bool elementsReady = false;
    private bool didAnything = false;

    private bool started = false;
    private float startWaiter = 0f;


    [Header("Players")]
    public List<Player> players;


    [Header("Level")]
    public GameUI gameUI;           //The GameUI class
    private float afkTime = 10000f;
    private float actualafkTime = 0f;


    [Header("SupElements")]
    public GameObject supPlatePrefab;
    public FrameMovementScript frameScript;
    public CameraScript camScript;
    public GameObject choiceCoin;

    private GameObject actualCubeChoosen;


    private List<Vector3> cubeRotations;

private bool cameraReady;
    private bool UIReady;

    private bool twoCubesToChoice;

    private bool getCoinWait;

    private bool supActiondid = false;

    private bool endTourWait = false;
    private float endTourWaiter = 0f;
    #region SINGLETON PATTERN
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();

                if (_instance == null)
                {
                    GameObject container = new GameObject("GameManager");
                    _instance = container.AddComponent<GameManager>();
                    container.tag = "GameManager";
                }
            }

            return _instance;
        }
    }
    #endregion

    void Awake()
    {
        Application.targetFrameRate = 60;
        players = new List<Player>();
        state = State.Menu;
        DontDestroyOnLoad(this.gameObject);
        cubeRotations = new List<Vector3>
        {
            new Vector3(0f, 0f, 0f),
            new Vector3(0f, 0f, 90f),
            new Vector3(0f, 0f, 180f),
            new Vector3(0f, 0f, 270f),
            new Vector3(90f, 0f, 0f),
            new Vector3(-90f, 0f, 0f),
        };
        getCoinWait = false;
    }

    void Start()
    {
        gameOver = false;
        if (SceneManager.GetActiveScene().name == "Level1")
        {
            initPlayers();
            StartGame();
        }
    }

    private void initPlayers()
    {
        for (int i = 1; i <= 4; ++i)
        {
            players.Add(new Player(i, i.ToString()));
        }
    }

    void Update()
    {

        if ((!started) &&(SceneManager.GetActiveScene().name == "Level1")){

            startWaiter += Time.deltaTime;
            if (startWaiter > 1500f)
            {
                StartGame();
            }
        }

        if (started)
        {
            if (endTourWait)
            {
                endTourWaiter += Time.deltaTime;
                Debug.Log(endTourWaiter);
                if (endTourWaiter > 4f)
                {
                    elementsReady = true;
                    endTourWaiter = 0f;
                    endTourWait = false;
                    endTour();
                }
            }


            if (getCoinWait)
            {
                if ((!choiceCoin.GetComponent<BuyElementScript>().getRotatingToAngle()) &&
                    (!choiceCoin.GetComponent<BuyElementScript>().getMoving()) &&
                    (!choiceCoin.GetComponent<BuyElementScript>().getRescaling()))
                {
                    choiceCoin.GetComponent<BuyElementScript>().setChoosen();
                    actualCubeChoosen.GetComponent<CubeLancementScript>().setTargetPos(actualCubeChoosen.GetComponent<CubeLancementScript>().getStartPos());
                    choiceCoin = null;
                    actualCubeChoosen = null;
                    getCoinWait = false;
                    setState(State.ActionSup);
                }
            }
            switch (state)
            {
                case State.PlayerLancement:
                    if (!endTourWait)
                    {
                        int end = 0;
                        foreach (var player in players)
                        {
                            if (player.manche == 1)
                            {
                                end++;
                            }
                        }
                        if (end == players.Count)
                        {
                            gameUI.SetWin();
                            setState(State.EndGame);
                            Debug.Log("End");
                        }
                        if ((!didAnything) && (elementsReady))
                        {
                            if (Input.GetKey(KeyCode.Return))
                            {
                                elementsReady = false;
                                didAnything = true;
                                actualafkTime = 0f;
                                gameUI.resetAFK();
                                foreach (var player in players)
                                {
                                    if (player.lostRollNum < 1)
                                    {
                                        player.lancerDes(1);
                                        player.lancerDes(2);
                                        waitForActionUI();
                                    }
                                    else
                                    {
                                        player.lostRollNum--;
                                        gameUI.setLostRoll();
                                    }
                                }
                            }
                            else
                                if (Input.GetKey(KeyCode.Space))
                            {
                                endTour();
                            }
                        }
                        else if ((elementsReady) && (!didAnything))
                        {
                            actualafkTime += Time.deltaTime;
                            if (actualafkTime > afkTime)
                            {
                                gameUI.setAFK();
                            }
                        }
                        else if ((elementsReady) && (didAnything))
                        {
                            setState(State.Buy);
                        }
                    }
                    break;
                case State.Buy:
                    if ((!didAnything) && (elementsReady))
                    {
                        if (Input.GetKey(KeyCode.LeftArrow))
                        {
                            gameUI.resetAFK();
                            frameScript.setDirection("Left");
                            camScript.uploadState(frameScript.getNewcamState());
                        }
                        else
                        if (Input.GetKey(KeyCode.RightArrow))
                        {
                            gameUI.resetAFK();
                            frameScript.setDirection("Right");
                            camScript.uploadState(frameScript.getNewcamState());
                        }
                        else
                        if (Input.GetKey(KeyCode.UpArrow))
                        {
                            gameUI.resetAFK();
                            frameScript.setDirection("Up");
                            camScript.uploadState(frameScript.getNewcamState());
                        }
                        else
                        if (Input.GetKey(KeyCode.DownArrow))
                        {
                            gameUI.resetAFK();
                            frameScript.setDirection("Down");
                            camScript.uploadState(frameScript.getNewcamState());
                        }
                        else
                        if (Input.GetKey(KeyCode.Return))
                        {
                            didAnything = true;
                            gameUI.resetAFK();
                            players[tour - 1].buy(frameScript.actualObjattached);
                        }
                    }
                    break;
                case State.ActionSup:
                    if (elementsReady)
                    {
                        if (Input.GetKey(KeyCode.Space))
                        {
                            endTour();
                        }
                        else
                            if (Input.GetKey(KeyCode.Return))
                        {
                            if (players[tour - 1].getBlue() >= 2)
                            {
                                setState(State.Buy);
                                choiceCoin = null;
                                didAnything = false;
                                actualCubeChoosen = null;
                            }
                        }
                    }
                    break;
                case State.ChangeDes:
                    if (twoCubesToChoice)
                    {
                        if (Input.GetKey(KeyCode.Alpha1))
                        {
                            players[tour - 1].getCubeTwo().GetComponent<CubeLancementScript>().setTargetPos(players[tour - 1].getCubeTwo().GetComponent<CubeLancementScript>().getStartPos());
                            players[tour - 1].getCubeTwo().GetComponent<CubeLancementScript>().setActualRotation();

                            players[tour - 1].getCubeOne().GetComponent<CubeLancementScript>().setActualRotation();
                            players[tour - 1].getCubeOne().GetComponent<CubeLancementScript>().setTargetPos(players[tour - 1].getCubeOne().transform.position + new Vector3(-50f, 300f, 150f));
                            actualCubeChoosen = players[tour - 1].getCubeOne();
                            twoCubesToChoice = false;
                        }
                        else
                            if (Input.GetKey(KeyCode.Alpha2))
                        {
                            players[tour - 1].getCubeOne().GetComponent<CubeLancementScript>().setTargetPos(players[tour - 1].getCubeOne().GetComponent<CubeLancementScript>().getStartPos());
                            players[tour - 1].getCubeOne().GetComponent<CubeLancementScript>().setActualRotation();

                            players[tour - 1].getCubeTwo().GetComponent<CubeLancementScript>().setActualRotation();
                            players[tour - 1].getCubeTwo().GetComponent<CubeLancementScript>().setTargetPos(players[tour - 1].getCubeTwo().transform.position + new Vector3(-50f, 300f, -150f));
                            actualCubeChoosen = players[tour - 1].getCubeTwo();
                            twoCubesToChoice = false;
                        }
                    }
                    else
                    {
                        Debug.Log(actualCubeChoosen.GetComponent<CubeLancementScript>().getStatePosition());
                        if (Input.GetKey(KeyCode.LeftArrow))
                        {
                            switch (actualCubeChoosen.GetComponent<CubeLancementScript>().getPosition())
                            {
                                case 1:
                                    actualCubeChoosen.GetComponent<CubeLancementScript>().rotateUno(5);
                                    break;
                                case 2:
                                    actualCubeChoosen.GetComponent<CubeLancementScript>().rotateUno(5);
                                    break;
                                case 3:
                                    actualCubeChoosen.GetComponent<CubeLancementScript>().rotateUno(6);
                                    break;
                                case 4:
                                    actualCubeChoosen.GetComponent<CubeLancementScript>().rotateUno(5);
                                    break;
                                case 5:
                                    actualCubeChoosen.GetComponent<CubeLancementScript>().rotateUno(3);
                                    break;
                                case 6:
                                    actualCubeChoosen.GetComponent<CubeLancementScript>().rotateUno(1);
                                    break;
                            }
                        }
                        else if (Input.GetKey(KeyCode.RightArrow))
                        {
                            switch (actualCubeChoosen.GetComponent<CubeLancementScript>().getPosition())
                            {
                                case 1:
                                    actualCubeChoosen.GetComponent<CubeLancementScript>().rotateUno(6);
                                    break;
                                case 2:
                                    actualCubeChoosen.GetComponent<CubeLancementScript>().rotateUno(6);
                                    break;
                                case 3:
                                    actualCubeChoosen.GetComponent<CubeLancementScript>().rotateUno(5);
                                    break;
                                case 4:
                                    actualCubeChoosen.GetComponent<CubeLancementScript>().rotateUno(6);
                                    break;
                                case 5:
                                    actualCubeChoosen.GetComponent<CubeLancementScript>().rotateUno(1);
                                    break;
                                case 6:
                                    actualCubeChoosen.GetComponent<CubeLancementScript>().rotateUno(3);
                                    break;
                            }
                        }
                        else if (Input.GetKey(KeyCode.DownArrow))
                        {
                            switch (actualCubeChoosen.GetComponent<CubeLancementScript>().getPosition())
                            {
                                case 1:
                                    actualCubeChoosen.GetComponent<CubeLancementScript>().rotateUno(4);
                                    break;
                                case 2:
                                    actualCubeChoosen.GetComponent<CubeLancementScript>().rotateUno(1);
                                    break;
                                case 3:
                                    actualCubeChoosen.GetComponent<CubeLancementScript>().rotateUno(2);
                                    break;
                                case 4:
                                    actualCubeChoosen.GetComponent<CubeLancementScript>().rotateUno(3);
                                    break;
                                case 5:
                                    actualCubeChoosen.GetComponent<CubeLancementScript>().rotateUno(2);
                                    break;
                                case 6:
                                    actualCubeChoosen.GetComponent<CubeLancementScript>().rotateUno(2);
                                    break;
                            }
                        }
                        else if (Input.GetKey(KeyCode.UpArrow))
                        {
                            switch (actualCubeChoosen.GetComponent<CubeLancementScript>().getPosition())
                            {
                                case 1:
                                    actualCubeChoosen.GetComponent<CubeLancementScript>().rotateUno(2);
                                    break;
                                case 2:
                                    actualCubeChoosen.GetComponent<CubeLancementScript>().rotateUno(3);
                                    break;
                                case 3:
                                    actualCubeChoosen.GetComponent<CubeLancementScript>().rotateUno(4);
                                    break;
                                case 4:
                                    actualCubeChoosen.GetComponent<CubeLancementScript>().rotateUno(1);
                                    break;
                                case 5:
                                    actualCubeChoosen.GetComponent<CubeLancementScript>().rotateUno(4);
                                    break;
                                case 6:
                                    actualCubeChoosen.GetComponent<CubeLancementScript>().rotateUno(4);
                                    break;
                            }
                        }
                        else
                            if ((!getCoinWait) && (Input.GetKey(KeyCode.Return)))
                        {
                            choiceCoin.GetComponent<BuyElementScript>().setCubeOfCoin(actualCubeChoosen);
                            actualCubeChoosen.GetComponent<CubeLancementScript>().setActualPlastine(choiceCoin);
                            getCoinWait = true;
                        }
                        else
                            if (Input.GetKey(KeyCode.Escape))
                        {
                            players[tour - 1].getCubeOne().GetComponent<CubeLancementScript>().SetChoiceState();
                            players[tour - 1].getCubeTwo().GetComponent<CubeLancementScript>().SetChoiceState();
                            twoCubesToChoice = true;
                        }
                    }
                    break;

            }
        }
    }

    public void setState(State state)
    {
        this.state = state;
        didAnything = false;
        if (state == State.PlayerLancement)
        {
            camScript.uploadState(tour);
            waitForActionCamera();
        }
        else
        if (state == State.Buy)
        {
            frameScript.activate();
            frameScript.typeUpdate();
            camScript.uploadState(frameScript.getNewcamState());
            waitForActionCamera();
        }
        else
        if (state == State.ActionSup)
        {
            if (!supActiondid)
            {
                didAnything = false;
                camScript.uploadState(tour);
                waitForActionCamera();
                supActiondid = true;
                endTourWait = true;
            }
            else
            {
                didAnything = false;
                camScript.uploadState(tour);
                waitForActionCamera();
                supActiondid = true;
            }
        }
        else
        if (state == State.ChangeDes)
        {
            players[tour - 1].getCubeOne().GetComponent<CubeLancementScript>().SetChoiceState();
            players[tour - 1].getCubeTwo().GetComponent<CubeLancementScript>().SetChoiceState();
            choiceCoin = frameScript.actualObjattached;
            twoCubesToChoice = true;
            frameScript.desactivate();
            camScript.waitForAction();
            waitForActionCamera();
        }
        
    }

    //Called when the game starts
    public void StartGame()
    {
        started = true;
        frameScript = GameObject.Find("Frame").GetComponent<FrameMovementScript>();
        gameUI = GameObject.Find("GameUI").GetComponent<GameUI>();
        camScript = Camera.main.GetComponent<CameraScript>();
        frameScript.desactivate();
        gameOver = false;
        UIReady = true;
        tour = 1;
        supActiondid = false;
        twoCubesToChoice = true;
        foreach (var player in players)
        {
            player.initPlateau();
        }
        setState(State.PlayerLancement);
        initCubes();
    }
    public int getTour()
    {
        return tour;
    }
    private void initCubes()
    {
        foreach (var player in players)
        {
            player.setCubesObj();
            if (player.getPlayerNum() > 2)
            {
                player.revertCubes();          //If the player occupies the plate of the other side, flip the cubes 
            }
        }
    }

    public void endTour()
    {
        if (players[tour - 1].manche < 8)
        {
            players[tour - 1].manche++;
        }
        tour += 1;
        if (tour > players.Count)
        {
            tour = 1;
        }

        setState(State.PlayerLancement);
        supActiondid = false;
        Debug.Log(players[tour-1].plateau.name);
    }


    public void waitForActionCamera() {
        elementsReady = false;
        cameraReady = false;
    }
    public void waitForActionUI()
    {
        elementsReady = false;
        UIReady = false;
    }
    public void setCameraReady()
    {
        cameraReady = true;
        if (UIReady)
        {
            elementsReady = true;
        }
    }
    public void setUIReady()
    {
        UIReady = true;
        if (cameraReady)
        {
            elementsReady = true;
        }
    }
}
