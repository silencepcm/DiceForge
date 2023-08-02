using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateauPlayerScript : MovingObjectScript
{
    private int supplatenum = 0;
    private float supplateMove = 400f;
    protected override void Start()
    {
        base.Start();
        moveSpeed = 2f;
        if(transform.name == "plateauBlue")
        {
            supplateMove *= -1;
        }
    }



    public void uploadSupplateNum()
    {
        supplatenum++;
        switch (transform.name)
        {
            case "plateauNoir":
                GameObject.Find("plateauRouge").GetComponent<PlateauPlayerScript>().setTargetPos(GameObject.Find("plateauRouge").transform.position + new Vector3(0f, 0f, supplateMove));
                GameObject.Find("Cubes").transform.Find("2").Find("CubeOne").GetComponent<CubeLancementScript>().uploadBase(new Vector3(0f, 0f, supplateMove), moveSpeed);
                GameObject.Find("Cubes").transform.Find("2").Find("CubeTwo").GetComponent<CubeLancementScript>().uploadBase(new Vector3(0f, 0f, supplateMove), moveSpeed);
                break;
            case "plateauVert":
                GameObject.Find("plateauBleu").GetComponent<PlateauPlayerScript>().setTargetPos(GameObject.Find("plateauBleu").transform.position + new Vector3(0f, 0f, supplateMove));
                GameObject.Find("Cubes").transform.Find("4").Find("CubeOne").GetComponent<CubeLancementScript>().uploadBase(new Vector3(0f, 0f, supplateMove), moveSpeed);
                GameObject.Find("Cubes").transform.Find("4").Find("CubeTwo").GetComponent<CubeLancementScript>().uploadBase(new Vector3(0f, 0f, supplateMove), moveSpeed);
                break;
        }
    }
}
