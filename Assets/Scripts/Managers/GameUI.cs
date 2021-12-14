using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameUI : MonoBehaviour
{

    private GameManager gameManager;
    private int waitForCubesOnPlate = 0;
    private int lancing = 0;


    private float noMoneyTimer = 0f;
    private float uiTimerLimit = 3f;

    private bool noMoney;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        waitForCubesOnPlate = 0;
        lancing = 0;
        noMoney = false;
    }

    void Update()
    {
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
    }
    public void resetCubeWait()
    {
        waitForCubesOnPlate--;
    }
    public void setLancing()
    {
        lancing++;
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
