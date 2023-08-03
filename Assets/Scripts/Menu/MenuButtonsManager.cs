using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuButtonsManager : MonoBehaviour
{
    public Transform sizeRect;
    [Range(1f,5f)]
    public float lineInterval;
    List<MainButtonsScript> buttonScripts;
    UnityAction activationEvent;
    UnityAction desactivationEvent;
    public bool activated = true;
    private void Awake()
    {
        initButtonsInFrame();
        
    }
    void initButtonsInFrame()
    {
        buttonScripts = new List<MainButtonsScript>();
        foreach (Transform tform in transform)
        {
            if (tform.CompareTag("Button"))
            {
                buttonScripts.Add(tform.GetComponent<MainButtonsScript>());
                activationEvent += buttonScripts[buttonScripts.Count - 1].activate;
                desactivationEvent += buttonScripts[buttonScripts.Count - 1].desactivate;
            }
        }
    }
    public void activateMenu()
    {
        activated = true;
        activationEvent.Invoke();
    }
    public void desactivateMenu()
    {
        activated = false;
        activationEvent.Invoke();
    }
}
