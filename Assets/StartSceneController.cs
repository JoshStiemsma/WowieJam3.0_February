﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StartSceneController : MonoBehaviour
{
    [SerializeField] private Button StartGameButton;
    // Start is called before the first frame update
    void Start()
    {
        StartGameButton.onClick.AddListener( StartGame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartGame()
    {
        //SceneManager.
    }
}
