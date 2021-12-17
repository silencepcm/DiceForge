using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player
{
    public GameObject plateau;
    public GameManager gameManager;

    private GameObject firstCube;
    private GameObject secondCube;

    private int cardAddedNum;
    private int player_num = 1;
    private int green;
    private int red;
    private int blue;
    private int gold;
    private string name;
    public int manche = 0;
    public int supPlayerPlateNum;
    public int lostRollNum = 0;
    private int waitForRessources = 0;

    private int waitGold = 0;
    private int waitBlue = 0;
    private int waitRed = 0;
    private int waitGreen = 0;

    private int goldMax = 12;
    private int blueMax = 6;
    private int redMax = 6;

    private int greenHundreds = 0;

    private List<GameObject> movingUIcubes;
    public Player(int player_num, string name)
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        this.player_num = player_num;
        this.name = name;
        red = 5;
        blue = 6;
        gold = 5;
        green = 5;
        cardAddedNum = 0;
        supPlayerPlateNum = 0;
        greenHundreds = 0;
        manche = 0;
    }

    public void initPlateau()
    {
        switch (player_num)
        {
            case 1:
                plateau = GameObject.Find("plateauNoir"); break;
            case 2:
                plateau = GameObject.Find("plateauBleu"); break;
            case 3:
                plateau = GameObject.Find("plateauVert"); break;
            case 4:
                plateau = GameObject.Find("plateauRouge"); break;
        }
        Debug.Log(player_num + "     " + name);
        movingUIcubes = new List<GameObject>
        {
            plateau.transform.Find("First").gameObject,
            plateau.transform.Find("Second").gameObject,
            plateau.transform.Find("Third").gameObject,
            plateau.transform.Find("Fourth").gameObject,
            plateau.transform.Find("Fifth").gameObject,

        };
    }

    public void lancerDes(int desNum)
    {
        Debug.Assert(desNum > 0, desNum < 3);
        if (desNum == 1) firstCube.GetComponent<CubeLancementScript>().lancer(this);
        else if (desNum == 2) secondCube.GetComponent<CubeLancementScript>().lancer(this);
        waitForRessources++;
    }
    public void buy(GameObject element)
    {
        BuyElementScript price = element.GetComponent<BuyElementScript>();
        if ((price.priceBlue <= blue) && (price.priceRed <= red)&& (price.priceGold <= gold) && (price.priceGreen <= green))
        {
            uploadRessourses(-price.priceGold, -price.priceRed, -price.priceBlue, -price.priceGreen, false);
            gameManager.frameScript.GetComponent<FrameMovementScript>().desactivate();
            if (element.tag == "Coin")
            {
                gameManager.choiceCoin = element;
                gameManager.setState(GameManager.State.ChangeDes);
                Debug.Log("ChoiceCoin");
                element.GetComponent<BuyElementScript>().getCoin(this);
            }
            else
            if (element.tag == "Card")
            {
                gameManager.setState(GameManager.State.ActionSup);
                element.GetComponent<BuyElementScript>().getCard(this);
            }
        }
        else
        {
            gameManager.gameUI.setNoMoney();
        }
    }
    public int getPlayerNum()
    {
        return player_num;
    }
    public int getCardAddedNum()
    {
        return cardAddedNum;
    }
    public void uploadRessourses(int Gold, int Red, int Blue, int Green, bool isCubeLanced)
    {
        if (isCubeLanced)
        {
            waitForRessources--;
        }
        if ((waitForRessources > 0)&&(isCubeLanced))
        {
            waitRed += Red;
            waitBlue += Blue;
            waitGold += Gold;
            waitGreen += Green;
        }
        else
        {
            gold += Gold + waitGold;
            if (gold > goldMax) gold = goldMax;
            movingUIcubes[0].GetComponent<CubeMovementScipt>().setPoints(gold);
            red += Red + waitRed;
            if (red > redMax) red = redMax;
            movingUIcubes[1].GetComponent<CubeMovementScipt>().setPoints(red);
            blue += Blue + waitBlue;
            if (blue > blueMax) blue = blueMax;
            movingUIcubes[2].GetComponent<CubeMovementScipt>().setPoints(blue);
            green += Green + waitGreen;
            if (green >= 100)
            {
                green -= 100;
                greenHundreds++;
                Debug.Log("GetHundred");
            }
            movingUIcubes[3].GetComponent<CubeMovementScipt>().setPoints(green%10);
            movingUIcubes[4].GetComponent<CubeMovementScipt>().setPoints(green/10);
            reloadWaitingPoints();
        }
    }

    private void reloadWaitingPoints()
    {
        waitRed = 0;
        waitBlue = 0;
        waitGold = 0;
        waitGreen = 0;
    }

    public void setCubesObj()        //At the beginning of the game, binds the cubes to the script 
    {
        firstCube = GameObject.Find("Cubes").transform.Find(player_num.ToString()).Find("CubeOne").gameObject;
        secondCube = GameObject.Find("Cubes").transform.Find(player_num.ToString()).Find("CubeTwo").gameObject;
    }
    public void revertCubes()                   //If the player occupies the plate of the other side, flip the cubes 
    {
        firstCube.GetComponent<CubeLancementScript>().setRotationOtherPlate();
        secondCube.GetComponent<CubeLancementScript>().setRotationOtherPlate();
    }
    public void uploadCardsNum()
    {
        cardAddedNum++;
    }
    public void supPlateUploadMaxs()
    {
        goldMax += 4;
        blueMax += 3;
        redMax += 3;
        foreach(var cube in movingUIcubes)
        {
            cube.GetComponent<CubeMovementScipt>().upgradeLinepointsMax();
        }
    }
    public GameObject getCubeOne()
    {
        return firstCube;
    }
    public GameObject getCubeTwo()
    {
        return secondCube;
    }

    public int getGreen() { return green; }
    public int getRed() { return red; }
    public int getBlue() { return blue; }
    public int getGold() { return gold; }
    public string getName()
    {
        return name;
    }
}
