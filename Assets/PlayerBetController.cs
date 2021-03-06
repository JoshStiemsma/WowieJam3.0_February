﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;
using System;

public class PlayerBetController : MonoBehaviour
{
   

    public Player playerType;


    KeyCode UpKey,DownKey,ReadyKey,LeftKey,RightKey;


    int playerTotal = 500;

    int betAmount = 100;

    private float readyCount = 0;

    public TextMeshProUGUI TotalText, BetAmountText,ReadyText;

    string readyUpKey => playerType == Player.Left ? "Left Shift": "Key Pad Enter";

    string MustSetBetTest = "Select Side to Bet On!";
    string GetReadyText;

    string PlayerReadyText = "Ready!";

    public bool PlayerReady;

    public Action OnPlayerReady;

    public Image ReadyFillImage, BetSideBackgroundImage;

    public bool BetOnSelf = true;

    private bool betHasBeenSet = false;

    public int GetBetAmount
    {
        get { return betAmount; }
    }
    public int GetTotalAmount
    {
        get { return playerTotal; }
    }

    public AudioSource tick;
    public AudioSource confirm;

    public AudioSource bet;

    private void Start()
    {
        GetReadyText = $"Hold {readyUpKey} key to Ready!";
        if (playerType == Player.Left)
        {
            UpKey = KeyCode.W;
            DownKey = KeyCode.S;
            LeftKey = KeyCode.A;
            RightKey = KeyCode.D;
            ReadyKey = KeyCode.LeftShift;
        }
        else
        {
            UpKey = KeyCode.Keypad8;
            DownKey = KeyCode.Keypad5;
            LeftKey = KeyCode.Keypad4;
            RightKey = KeyCode.Keypad6;
            ReadyKey = KeyCode.KeypadEnter;
        }
        Reset();
    }

    public void Update()
    {
        if (!PlayerReady && FightSceneManager.instance.CurrentScene == FightSceneManager.Scene.Betting)
        {
            CheckPlayerBets();
           if(betHasBeenSet) CheckPlayerReady();
        }
        else
        {
            playerHoldingKey = false;
        }
    }

    public bool didPlayerWinBet = false;
    public void EndRound(Player lostPlayer)
    {
       // pastBetAmount = betAmount;
       // Debug.Log("EndRound");

        didPlayerWinBet = (BetOnSelf && playerType != lostPlayer) || ( !BetOnSelf && playerType == lostPlayer);

        if(didPlayerWinBet) playerTotal += betAmount;
        else playerTotal -= betAmount;

    }

    public void EndRoundByThrowCall(Player lostPlayer)
    {
      //  Debug.Log("EndRoundByThrowCall");

        // Debug.Log($"EndRoundByThrowCall  lost {lostPlayer}");
        didPlayerWinBet =  playerType != lostPlayer;

        if (didPlayerWinBet) playerTotal += betAmount;
        else playerTotal -= betAmount;

    }

    void CheckPlayerBets()
    {

        if (Input.GetKeyUp(UpKey))
        {
            //noise
            tick.Play();
            if (betAmount + 50 <= playerTotal)
                betAmount += 50;
            BetAmountText.text = betAmount.ToString();
        }
        else if (Input.GetKeyUp(DownKey))
        {
            //noise
             tick.Play();
            if (betAmount > 100)
                betAmount -= 50;
            BetAmountText.text = betAmount.ToString();
        }

        if (Input.GetKeyUp(LeftKey))
        {
            //noise
             bet.Play();
            SetBettingSide(true);
        }
        else if (Input.GetKeyUp(RightKey))
        {
            //noise
             bet.Play();
            SetBettingSide(false);
        }

    }

    void SetBettingSide(bool _betOnSelf)
    {
        if (SetBidSideRoutine == null)
        {
            ReadyText.text =  GetReadyText;

            BetOnSelf = _betOnSelf;
            betHasBeenSet = true;
            SetBidSideRoutine = StartCoroutine(AnimateBetsideBackRoutine());
        }
    }


    Coroutine SetBidSideRoutine;
    IEnumerator AnimateBetsideBackRoutine()
    {
        float timer = 0;
        float totalTime = 1f;
        while(timer < totalTime)
        {
            timer += Time.fixedDeltaTime;
            BetSideBackgroundImage.color = Color.Lerp(Color.white,Color.clear, timer/ totalTime) ;
            yield return new WaitForSeconds(.01f);
        }
        SetBidSideRoutine = null;
    }

    bool playerHoldingKey = false;
    void CheckPlayerReady()
    {
       
        
        if (Input.GetKeyDown(ReadyKey) && !Input.GetKeyUp(ReadyKey)) playerHoldingKey = true;
        if (!Input.GetKeyDown(ReadyKey) && Input.GetKeyUp(ReadyKey)) playerHoldingKey = false;

        if (playerHoldingKey)
        {
            readyCount += Time.fixedDeltaTime;
            ReadyFillImage.fillAmount = readyCount / 3f;
        }
        else
        {
            readyCount = 0;
            ReadyFillImage.fillAmount = 0;
        }

        if (readyCount > 3 && !PlayerReady)
        {
            //noise
            confirm.Play();
            ReadyText.text = PlayerReadyText;

            PlayerReady = true;
            OnPlayerReady.Invoke();
        }
    }

    public void Reset()
    {
        ReadyText.text = MustSetBetTest;
        PlayerReady = false;
        betAmount = playerTotal >= betAmount ? betAmount : 50;
        BetAmountText.text = betAmount.ToString();
        readyCount = 0;
        ReadyFillImage.fillAmount = 0;
        TotalText.text = playerTotal.ToString();
        BetOnSelf = true;
        betHasBeenSet = false;
    }


}
