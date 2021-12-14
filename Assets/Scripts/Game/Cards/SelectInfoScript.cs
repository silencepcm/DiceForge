using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectInfoScript : MonoBehaviour
{
    public GameObject leftArrow;
    public GameObject rightArrow;
    public GameObject upArrow;
    public GameObject downArrow;
    [Range(5,7)]
    public int camState;


    public void removeFromTarget()
    {
        if (leftArrow) leftArrow.GetComponent<SelectInfoScript>().rightArrow = rightArrow;
        if (rightArrow) rightArrow.GetComponent<SelectInfoScript>().leftArrow = leftArrow;
        if (upArrow) upArrow.GetComponent<SelectInfoScript>().downArrow = downArrow;
        if (downArrow) downArrow.GetComponent<SelectInfoScript>().upArrow = upArrow;
    }

    public void calibrateArrows()
    {
        if (leftArrow) leftArrow.GetComponent<SelectInfoScript>().rightArrow = gameObject;
        if (rightArrow) rightArrow.GetComponent<SelectInfoScript>().leftArrow = gameObject;
        if (upArrow) upArrow.GetComponent<SelectInfoScript>().downArrow = gameObject;
        if (downArrow) downArrow.GetComponent<SelectInfoScript>().upArrow = gameObject;
    }
    public void destroyArrowLinks()
    {
        if (leftArrow) leftArrow = null;
        if (rightArrow) rightArrow = null;
        if (upArrow) upArrow = null;
        if (downArrow) downArrow = null;
    }
}
