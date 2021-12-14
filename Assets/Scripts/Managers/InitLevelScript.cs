using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitLevelScript : MonoBehaviour
{
    public GameUI gameUI;           //The GameUI class


    void Start()
    {
        GameManager.Instance.StartGame();
    }

    void Update()
    {

    }
}
