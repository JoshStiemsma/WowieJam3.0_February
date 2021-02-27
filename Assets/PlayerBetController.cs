using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PlayerBetController : MonoBehaviour
{
    public enum Player
    {
        Left,
        Right
    }

    public Player playerType;


    KeyCode UpKey,DownKey,ReadyKey;


    int playerTotal = 500;
    int betAmount = 100;
    private float readyCount = 0;
    public TextMeshProUGUI TotalText, BetAmountText,ReadyText;
    string readyUpKey => playerType == Player.Left ? "Left Shift": "Key Pad Enter";
    string GetReadyText;
    string PlayerReadyText = "Ready!";
    public bool PlayerReady;

    public Action OnPlayerReady;

    private void Start()
    {
        GetReadyText = $"Hold {readyUpKey} key to Ready!";
        if (playerType == Player.Left)
        {
            UpKey = KeyCode.W;
            DownKey = KeyCode.S;
            ReadyKey = KeyCode.LeftShift;
        }
        else
        {
            UpKey = KeyCode.Keypad8;
            DownKey = KeyCode.Keypad5;
            ReadyKey = KeyCode.KeypadEnter;

        }
    }


    public void Update()
    {


        if (PlayerReady) return;
        if (Input.GetKeyUp(UpKey))
        {
            if(betAmount + 50 < playerTotal)
            betAmount += 50;
            BetAmountText.text = betAmount.ToString();

        }
        else if (Input.GetKeyUp(DownKey))
        {
            if(betAmount > 100)
            betAmount -= 50;
            BetAmountText.text = betAmount.ToString();

        }

       

        if (Input.GetKey(ReadyKey))
        {
            readyCount += Time.fixedDeltaTime;
        }
        else
        {
            readyCount = 0;
        }

        if(readyCount > 3)
        {
            ReadyText.text = PlayerReadyText;

            PlayerReady = true;
            OnPlayerReady.Invoke();
        }

    }

    public void Reset()
    {
        ReadyText.text = GetReadyText;
        PlayerReady = false;
    }


}
