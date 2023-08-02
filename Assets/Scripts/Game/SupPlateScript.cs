using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupPlateScript : MovingObjectScript
{
    private GameObject plateau;
    private Vector3 posUpload;
    private bool activated;
    private List<Vector3> positions = new List<Vector3>
    {
        new Vector3(1308f, 0f, -102f)
    };
    protected override void Start()
    {
        posUpload = new Vector3(-98f, 1f, 520f);
        base.Start();
        speedRotateToAngle = 1f;
        activated = false;
        plateau = null;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if ((transform.parent==null)&&(!getMoving()) && (activated))
        {
            transform.SetParent(plateau.transform);
        }
    }
    public void activate(Player player)
    {
        plateau = player.plateau;

        int direction = 1;
        if (player.getPlayerNum() > 2)
        {
            direction = -1;
        }
        setTargetPos(positions[player.getPlayerNum()-1] + new Vector3(0f, player.supPlayerPlateNum+1, direction*player.supPlayerPlateNum*314f));
        setRotAngle(plateau.transform.rotation);
        activated = true;
        plateau.GetComponent<PlateauPlayerScript>().uploadSupplateNum();
    }
}
