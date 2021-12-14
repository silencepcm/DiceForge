using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyElementScript : MovingObjectScript
{
    public GameObject prefabSupPlate;
    public int cardsNum;

    public int greenCoin;
    public int goldCoin;
    public int blueCoin;
    public int redCoin;

    public int priceGold;
    public int priceBlue;
    public int priceGreen;
    public int priceRed;

    private Vector3 distanceToPlate = new Vector3(500f, 0f, 100f);

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public void getCard(Player player)
    {
        Debug.Assert(tag == "Card");
        float countPos = player.getCardAddedNum()/10+1;
        cardsNum--;
        int cardToPlateShifter = 1;
        if (player.getPlayerNum() > 2)
        {
            cardToPlateShifter = -1;
        }
        if (cardsNum > 0)
        {
            GameObject newCard = Instantiate(prefabSupPlate, transform.position, transform.rotation);
            newCard.GetComponent<BuyElementScript>().init();
            newCard.GetComponent<BuyElementScript>().setTargetPos(player.plateau.transform.position);
            newCard.GetComponent<BuyElementScript>().setRotAngle(player.plateau.transform.rotation);
            player.uploadCardsNum();
        }
        else
        {
            GetComponent<SelectInfoScript>().removeFromTarget();
            setTargetPos(player.plateau.transform.position + distanceToPlate * countPos * cardToPlateShifter);
            setRotAngle(player.plateau.transform.rotation);
        }
        switch (transform.name)
        {
            case "SupPlate":
                Transform supPlate = GameObject.FindGameObjectWithTag("SupPlate").transform.Find((cardsNum+1).ToString());
                supPlate.GetComponent<SupPlateScript>().activate(player);
                player.supPlateUploadMaxs();
                break;
            case "Tempete":
                break;
            case "Deer":
                break;
        }
        GameObject.Find("GameManager").GetComponent<GameManager>().setState(GameManager.State.ActionSup);

    }
    public void getCoin(Player player)
    {
        Debug.Assert(tag == "Coin");
        player.getCoin(gameObject);
    }
}
