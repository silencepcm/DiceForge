using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum State { StartInit, PlayerLancement, Buy, ChangeDes, ActionSup, EndGame }
    private State state;

    [Header("Game")]
    public bool gameOver = false;           //Set true when the game is over
    private int tour = 1;
    private bool action = false;


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
    public PlastineScript choiceCoinScript;


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
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        gameOver = false;
        if (SceneManager.GetActiveScene().name == "Level1")
        {
            initPlayers();
            StartGame();
        }
        else
        {

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





        if (state == State.PlayerLancement)
        {
            if ((action) && (Input.GetKey(KeyCode.Return)))
            {
                action = false;
                actualafkTime = 0f;
                gameUI.resetAFK();
                foreach (var player in players)
                {
                    if (player.lostRollNum < 1)
                    {
                        player.lancerDes(1);
                        player.lancerDes(2);
                    }
                    else
                    {
                        player.lostRollNum--;
                        gameUI.setLostRoll();
                    }
                }
            }
            else if (action)
            {
                actualafkTime += Time.deltaTime;
                if (actualafkTime > afkTime)
                {
                    gameUI.setAFK();
                }
            }
            else if (gameUI.getReady())
            {
                setState(State.Buy);
            }
        }
        else if ((action)&&(state == State.Buy))
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                gameUI.resetAFK();
                frameScript.setDirection("Left");
                camScript.uploadState(frameScript.getNewcamState(), false);
            }
            else
            if (Input.GetKey(KeyCode.RightArrow))
            {
                gameUI.resetAFK();
                frameScript.setDirection("Right");
                camScript.uploadState(frameScript.getNewcamState(), false);
            }
            else
            if (Input.GetKey(KeyCode.UpArrow))
            {
                gameUI.resetAFK();
                frameScript.setDirection("Up");
                camScript.uploadState(frameScript.getNewcamState(), false);
            }
            else
            if (Input.GetKey(KeyCode.DownArrow))
            {
                gameUI.resetAFK();
                frameScript.setDirection("Down");
                camScript.uploadState(frameScript.getNewcamState(), false);
            }
            else
            if (Input.GetKey(KeyCode.Return))
            {
                gameUI.resetAFK();
                action = false;
                players[tour - 1].buy(frameScript.actualObjattached);
            }
        }
        else
        if(state == State.ActionSup)
        {

        } 
        else
        if (state == State.ChangeDes)
        {

        }
    }

    public void setState(State state)
    {
        if (this.state != state) {
            this.state = state;
            if (state == State.PlayerLancement)
            {
                camScript.uploadState(tour, true);
            } else
            if (state == State.Buy)
            {
                frameScript.activate();
                frameScript.typeUpdate();
                camScript.uploadState(frameScript.getNewcamState(), false);
                waitAction();
            } else 
            if (state == State.ActionSup) 
            {
                camScript.uploadState(tour, false);

            }
            else 
            if (state == State.ChangeDes)
            {
                frameScript.desactivate();
            }
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
        tour = 1;
        setState(State.PlayerLancement);
        initCubes();
        action = true;
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

    private void waitAction()
    {
        action = false;
        camScript.waitForAction();
    }
    public void setAction()
    {
        action = true;
    }

    public void endTour()
    {
        tour = (tour + 1) % (players.Count + 1);
        setState(State.PlayerLancement);
    }
}
