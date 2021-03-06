﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class PostFightSreenController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI LeftBet;
    [SerializeField] private TextMeshProUGUI RightBet;
    [SerializeField] private TextMeshProUGUI LeftTotal;
    [SerializeField] private TextMeshProUGUI RightTotal;
    [SerializeField] private TextMeshProUGUI LeftBetSideText,RightBetSideText;


    [SerializeField] private Image LeftReadyImage, RightReadyImage;
    [SerializeField] private TextMeshProUGUI LeftReadyText, RightReadyText;
    [SerializeField] private Image LeftWinCover, RightWinCover;

    float LeftReadyCount, RightReadyCount;
    bool isLeftReady, isRightReady;

    public Action OnPostScreenReady;

    private CanvasGroup _canvasGroup;
    public AudioSource sound;
    private CanvasGroup CanvasGroup
    {
        get
        {
            if (_canvasGroup == null) _canvasGroup = GetComponent<CanvasGroup>();
            return _canvasGroup;

        }
    }

    private PlayerBetController LBC, RBC;

    string PlayerReadyText = "Ready!";

    string LeftreadyUpKey = "Left Shift";

    string RightreadyUpKey =  "Key Pad Enter";

    string GetReadyText;

    // Start is called before the first frame update
    void Start()
    {
        Hide();
    }

    public void Initialize(PlayerBetController leftC, PlayerBetController Rightc)
    {
        LBC = leftC;
        RBC = Rightc;
    }

    void Refresh(Player lostPlayer)
    {
        LeftWinCover.enabled =(lostPlayer != LBC.playerType);

        RightWinCover.enabled =(lostPlayer != RBC.playerType);

        LeftBet.color = LBC.didPlayerWinBet ? Color.green : Color.red;

        LeftBet.text =(LBC.didPlayerWinBet ? "+":"-") +    LBC.GetBetAmount.ToString();

        LeftTotal.text = LBC.GetTotalAmount.ToString();

        RightBet.color = RBC.didPlayerWinBet ? Color.green : Color.red;

        RightBet.text = (RBC.didPlayerWinBet ? "+" : "-") + RBC.GetBetAmount.ToString();

        RightTotal.text = RBC.GetTotalAmount.ToString();



        LeftBetSideText.text = LBC.BetOnSelf ? "<color=blue>Blue</color> Bet On <color=blue>Blue</color>" : "<color=blue>Blue</color> Bet On <color=red>Red</color>"; 
        RightBetSideText.text = RBC.BetOnSelf ? "<color=red>Red</color> Bet On <color=red>Red</color>" : "<color=red>Red</color> Bet On <color=blue>Blue</color>";
    }

    public void Show(Player lostPlayer)
    {
        Refresh(lostPlayer);
        CanvasGroup.alpha = 1;
        CanvasGroup.interactable = true;
        CanvasGroup.blocksRaycasts = true;
    }

    public void Hide()
    {
        CanvasGroup.alpha = 0;
        CanvasGroup.interactable = false;
        CanvasGroup.blocksRaycasts = false;
    }



    bool LeftPlayerHoldingReady = false, RightPlayerHoldingReady = false;
    void Update() { 
    
        if (FightSceneManager.instance.CurrentScene != FightSceneManager.Scene.PostFight) return;





        if (Input.GetKeyDown(KeyCode.LeftShift) && !Input.GetKeyUp(KeyCode.LeftShift)) LeftPlayerHoldingReady = true;
        if (!Input.GetKeyDown(KeyCode.LeftShift) && Input.GetKeyUp(KeyCode.LeftShift)) LeftPlayerHoldingReady = false;

        if (LeftPlayerHoldingReady)
        {
            LeftReadyCount += Time.fixedDeltaTime;
            if (LeftReadyCount >= 3 && !isLeftReady)
            {
                //sound
                sound.Play();
                isLeftReady = true;
                LeftReadyText.text = PlayerReadyText;
            }
        }
        else if(LeftReadyCount < 3)
        {
            LeftReadyCount =0;
        }



        if (Input.GetKeyDown(KeyCode.KeypadEnter) && !Input.GetKeyUp(KeyCode.KeypadEnter)) RightPlayerHoldingReady = true;
        if (!Input.GetKeyDown(KeyCode.KeypadEnter) && Input.GetKeyUp(KeyCode.KeypadEnter)) RightPlayerHoldingReady = false;

        if (RightPlayerHoldingReady)
        {
            RightReadyCount += Time.fixedDeltaTime;
            if (RightReadyCount >= 3 && !isRightReady)
            {
                //sound
                sound.Play();
                isRightReady = true;
                RightReadyText.text = PlayerReadyText;
            }
        }
        else if(RightReadyCount <3)
        {
            RightReadyCount = 0;
        }

        LeftReadyImage.fillAmount = LeftReadyCount / 3f;
        RightReadyImage.fillAmount = RightReadyCount / 3f;


        if (isLeftReady && isRightReady)
        {
            OnPostScreenReady.Invoke();
            Reset();
        }
    }

    public void Reset()
    {
        Hide();
        isLeftReady = false;
        isRightReady = false;
        LeftReadyCount = 0;
        RightReadyCount = 0;
        LeftReadyText.text = $"Hold {LeftreadyUpKey} key to Ready!"; ;
        RightReadyText.text = $"Hold {RightreadyUpKey} key to Ready!"; ;
        LeftPlayerHoldingReady = false;
        RightPlayerHoldingReady = false;

    }
}
