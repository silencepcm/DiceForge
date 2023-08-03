using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainButtonsScript : UIMovementScript
{
    private float lineSep;

    private bool activated = false;
    public bool activateOnStart;
    public Transform sizeRect;
    protected override void Start()
    {
        base.Start();
        if (activateOnStart)
        {
           // activate();
        }

        lineSep = 200f;
        Vector3 targetStart = new Vector3(0f, 30f, 0f);
        switch (transform.name)
        {
            case "SkillsButton":
                targetStart.y -= lineSep;
                break;
            case "QuitButton":
                targetStart.y -= lineSep * 2;
                break;
        }
        setTargetPos(targetStart);
    }

    public void activate()
    {
        activated = true;
        //setTargetPos(targe)
      //  speed = 3f;
    }
    public void desactivate()
    {
        activated = false;
    }
}
