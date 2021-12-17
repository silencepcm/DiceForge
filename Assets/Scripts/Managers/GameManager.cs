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

    private float getCoinWaiter = 0f;
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
        getCoinWaiter = 0f;
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
        if (endTourWait)
        {
            endTourWaiter += Time.fixedDeltaTime;
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
            getCoinWaiter += Time.fixedTime;
            if (getCoinWaiter > 4000f)
            {
                setState(State.ActionSup);
                choiceCoin.GetComponent<BuyElementScript>().setChoosen();
                actualCubeChoosen.GetComponent<CubeLancementScript>().setTargetPos(actualCubeChoosen.GetComponent<CubeLancementScript>().getStartPos());
                getCoinWaiter = 0f;
                getCoinWait = false;
            }
        }

        if ((state == State.PlayerLancement)&&(!endTourWait))
        {
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
        else if ((!didAnything) && (elementsReady)&&(state == State.Buy))
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
        else
        if(state == State.ActionSup)
        {
            if (elementsReady)
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    endTour();
                    Debug.Log("EndTour");
                } else
                    if (Input.GetKey(KeyCode.Return))
                {
                    setState(State.Buy);
                    Debug.Log("YEAH");
                }
            }
        } 
        else
        if (state == State.ChangeDes)
        {
            if (twoCubesToChoice)
            {
                if (Input.GetKey(KeyCode.Alpha1))
                {
                    players[tour - 1].getCubeTwo().GetComponent<CubeLancementScript>().setTargetPos(players[tour - 1].getCubeTwo().GetComponent<CubeLancementScript>().getStartPos());
                    players[tour - 1].getCubeTwo().GetComponent<CubeLancementScript>().setRotAngle(Quaternion.Euler(players[tour - 1].getCubeTwo().GetComponent<CubeLancementScript>().getActualRotation()));

                    players[tour - 1].getCubeOne().GetComponent<CubeLancementScript>().setRotAngle(Quaternion.Euler(players[tour - 1].getCubeOne().GetComponent<CubeLancementScript>().getActualRotation()));
                    players[tour - 1].getCubeOne().GetComponent<CubeLancementScript>().setTargetPos(players[tour - 1].getCubeOne().transform.position + new Vector3(-50f, 300f, 150f));
                    actualCubeChoosen = players[tour - 1].getCubeOne();
                    twoCubesToChoice = false;
                }
                else
                    if (Input.GetKey(KeyCode.Alpha2))
                {
                    players[tour - 1].getCubeOne().GetComponent<CubeLancementScript>().setTargetPos(players[tour - 1].getCubeTwo().GetComponent<CubeLancementScript>().getStartPos());
                    players[tour - 1].getCubeOne().GetComponent<CubeLancementScript>().setRotAngle(Quaternion.Euler(players[tour - 1].getCubeTwo().GetComponent<CubeLancementScript>().getActualRotation()));

                    players[tour - 1].getCubeTwo().GetComponent<CubeLancementScript>().setRotAngle(Quaternion.Euler(players[tour - 1].getCubeTwo().GetComponent<CubeLancementScript>().getActualRotation()));
                    players[tour - 1].getCubeTwo().GetComponent<CubeLancementScript>().setTargetPos(players[tour - 1].getCubeTwo().transform.position + new Vector3(-50f, 300f, -150));
                    actualCubeChoosen = players[tour - 1].getCubeTwo();
                    twoCubesToChoice = false;
                }
            } else
            {
                if (Input.GetKey(KeyCode.LeftArrow)) {
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
                else if (Input.GetKey(KeyCode.RightArrow)) {
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
                else if (Input.GetKey(KeyCode.DownArrow)) {
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
                    if ((!getCoinWait)&&(Input.GetKey(KeyCode.Return)))
                {
                    getCoinWait = true;
                    choiceCoin.transform.SetParent(actualCubeChoosen.transform);
                    choiceCoin.GetComponent<BuyElementScript>().setTargetPos(actualCubeChoosen.GetComponent<CubeLancementScript>().getActualPlastine().transform.position);
                    choiceCoin.GetComponent<BuyElementScript>().setRotAngle(actualCubeChoosen.GetComponent<CubeLancementScript>().getActualPlastine().transform.rotation);
                    choiceCoin.GetComponent<BuyElementScript>().setScale(actualCubeChoosen.GetComponent<CubeLancementScript>().getActualPlastine().transform.localScale);
                    actualCubeChoosen.GetComponent<CubeLancementScript>().setActualPlastine(choiceCoin);
                }
                else
                    if (Input.GetKey(KeyCode.Escape))
                {
                    players[tour - 1].getCubeOne().GetComponent<CubeLancementScript>().SetChoiceState();
                    players[tour - 1].getCubeTwo().GetComponent<CubeLancementScript>().SetChoiceState();
                    twoCubesToChoice = true;
                }
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
            frameScript.desactivate();
            camScript.waitForAction();
            waitForActionCamera();
        }
        
    }

    //Called when the game starts
    public void StartGame()
    {
        foreach(var player in players)
        {
            player.initPlateau();
        }
        frameScript = GameObject.Find("Frame").GetComponent<FrameMovementScript>();
        gameUI = GameObject.Find("GameUI").GetComponent<GameUI>();
        camScript = Camera.main.GetComponent<CameraScript>();
        frameScript.desactivate();
        gameOver = false;
        UIReady = true;
        tour = 1;
        supActiondid = false;
        twoCubesToChoice = true;
        setState(State.PlayerLancement);
        initCubes();
    }

    public void WinGame()
    {
        gameUI.SetWin();                //Set the game over UI screen
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
        if (supActiondid)
        {

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
