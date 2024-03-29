﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class GameUI : MonoBehaviour
{

    private GameManager gameManager;
    public GameObject win;
    public GameObject winnerNameText;
    public GameObject ScoreText;
    private int waitForCubesOnPlate = 0;
    private int lancing = 0;
    private bool ready;

    private float noMoneyTimer = 0f;
    private float uiTimerLimit = 3f;

    private bool noMoney;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        waitForCubesOnPlate = 0;
        lancing = 0;
        noMoney = false;
        ready = true;
    }

    void Update()
    {
        if((!ready)&& (lancing == 0) && (waitForCubesOnPlate == 0))
        {
            ready = true;
            gameManager.setUIReady();
        }
        if (noMoney)
        {
            noMoneyTimer += Time.deltaTime;
            Debug.Log("WAIT");
            if (noMoneyTimer > uiTimerLimit)
            {
                noMoneyTimer = 0f;
                noMoney = false;
            }
        } else
        {
        }
    }

    //Called when the game has been won
    public void SetWin()
    {
       // win.GetComponent<UIMovementScript>().activate();
        int score = 0;
        string winnerName = "";
        foreach(var player in gameManager.players)
        {
            ScoreText.GetComponent<TextMeshProUGUI>().text += "Player " + player.getName() + ": " + player.getGreen() + "\n";
            if (player.getGreen() > score)
            {
                score = player.getGreen();
                winnerName = player.getName();
            }
        }
        
        Debug.Log(winnerNameText);
        winnerNameText.GetComponent<TextMeshProUGUI>().text = "Joueur " + winnerName + " gagne!";
    //    win.GetComponent<UIMovementScript>().activate();
    }

    //Called when the 'TRY AGAIN' button is pressed
    public void TryAgainButton()
    {
        SceneManager.LoadScene("Level1");
    }

    public void MenuButton()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Leave()
    {
        Application.Quit();
    }

    public void setLostRoll()
    {

    }
    public void setAFK()
    {

    }
    public void resetAFK()
    {

    }
    public bool getReady()
    {
        return lancing == 0 && waitForCubesOnPlate == 0;
    }
    public void setCubeWait()
    {
        waitForCubesOnPlate++;
        if (ready)
        {
            ready = false;
            gameManager.waitForActionUI();
        }
    }
    public void resetCubeWait()
    {
        waitForCubesOnPlate--;
    }
    public void setLancing()
    {
        lancing++;
        if (ready)
        {
            ready = false;
            gameManager.waitForActionUI();
        }
    }
    public void resetLancing()
    {
        lancing--;
    }
    public void setNoMoney()
    {
        Debug.Log("No money");
        noMoney = true;
    }
}
