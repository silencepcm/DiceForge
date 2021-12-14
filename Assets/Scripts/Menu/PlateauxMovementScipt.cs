using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateauxMovementScipt : UIMovementScript
{
    protected override void Start()
    {
        spawn = transform.position;
        generalState = new Vector3(600f, 200f, -200f);
        speed = 3f;
        moving = false;
        activated = false;
    }

    protected override void activateMod()
    {
        switch (gameObject.name)
        {
            case "Noir":
                target.x *= -1; break;
            case "Bleu":
                target.y *= -1; break;
            case "Vert":
                target.y *= -1; target.x *= -1; break;
            case "Rouge":
                break;
        }
    }
}
