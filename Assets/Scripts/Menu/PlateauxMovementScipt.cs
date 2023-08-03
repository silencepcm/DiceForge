using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateauxMovementScipt : MovingObjectScript
{
   
    public void activate()
    {
        setTargetPos(transform.Find("startPosition").transform.position);
    }
   
}
