using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyElementScript : MovingObjectScript
{
    public GameObject SupPlate;
    public int cardsNum;

    public int greenCoin;
    public int goldCoin;
    public int blueCoin;
    public int redCoin;

    public int priceGold;
    public int priceBlue;
    public int priceGreen;
    public int priceRed;

    private bool choosen = false;
    private Vector3 distanceToPlate = new Vector3(600f, 0f, 100f);


    protected override void Start()
    {
        base.Start();
        speedRotateToAngle = 3f;
        moveSpeed = 2f;
        rescaleEndSkip = 1f;
        rotateEndSkip = 0.5f;
        moveEndSkip = 0.5f;
        choosen = false;
    }

    protected override void Update()
    {
        if (!choosen)
        {
            base.Update();
        }
    }

    public void getCard(Player player)
    {
        Debug.Assert(tag == "Card");
        cardsNum--;
        int cardToPlateShifter = 1;
        if ((player.plateau.name =="plateauBleu") || (player.plateau.name == "plateauVert"))
        {
            cardToPlateShifter = -1;
        }

        GetComponent<SelectInfoScript>().removeFromTarget();
        setTargetPos(player.plateau.transform.position + new Vector3(600f* cardToPlateShifter, 0f, 100f* cardToPlateShifter) + new Vector3(player.getCardAddedNum()*30f* cardToPlateShifter, 2f+player.getCardAddedNum(), player.getCardAddedNum() * 30f* cardToPlateShifter));
        setRotAngle(player.plateau.transform.rotation);
        player.uploadCardsNum();
        switch (transform.name)
        {
            case "SupPlate":
                SupPlate.GetComponent<SupPlateScript>().activate(player);
                player.supPlateUploadMaxs();
                break;
            case "Tempete":


                break;
            case "Deer":
                break;
            case "Splinter":
                if (player.getCubeOne().GetComponent<CubeLancementScript>().getActualPlastine().name == "Gold3")
                {
                    player.uploadRessourses(0, 0, 0, 4, false);
                }
                if (player.getCubeTwo().GetComponent<CubeLancementScript>().getActualPlastine().name == "Gold3")
                {
                    player.uploadRessourses(0, 0, 0, 4, false);
                }
                break;
            case "Fleurs":
                player.uploadRessourses(3, 0, 3, 0, false);
                break;
            case "Scorpion":
                player.uploadRessourses(0, 0, 0, 8, false);
                player.lancerDes(1);
                player.lancerDes(2);
                break;
        }

    }
    public void getCoin(Player player)
    {
        Camera.main.GetComponent<CameraScript>().setChoiceCoin();
        coinToCamera();
        Debug.Assert(tag == "Coin");
        GetComponent<SelectInfoScript>().removeFromTarget();
        transform.SetParent(null, true);
        Debug.Log("GetCoin");
    }

    public void coinToCamera()
    {
        CameraScript cam = Camera.main.GetComponent<CameraScript>();
        Vector3 targetToCamera = new Vector3(cam.getTarget().x-150f, cam.getTarget().y - 350f, cam.getTarget().z);
        setTargetPos(targetToCamera);

    }
    public void setChoosen()
    {
        choosen = true;
    }
}
