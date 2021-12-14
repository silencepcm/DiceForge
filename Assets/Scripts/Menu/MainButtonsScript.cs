using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainButtonsScript : UIMovementScript
{
    private float lineSep;
    protected override void Start()
    {
        lineSep = 200f;
        speed = 3f;
        spawn = new Vector3(0f, -200f, 0f);
        generalState = new Vector3(0f, 30f, 0f);
        switch (transform.name)
        {
            case "SkillsButton":
                generalState.y -= lineSep;
                spawn = new Vector3(0f, -600f, 0f); break;
            case "QuitButton":
                generalState.y -= lineSep * 2;
                spawn = new Vector3(0f, -800f, 0f); break;
        }
        activate();
    }
    protected override void desactivateMod()
    {
        speed = 2f;
    }
    protected override void activateMod()
    {
        speed = 3f;
    }
}
