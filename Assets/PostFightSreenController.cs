using System.Collections;
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


    [SerializeField] private Image LeftReadyImage, RightReadyImage;
    [SerializeField] private TextMeshProUGUI LeftReadyText, RightReadyText;
    [SerializeField] private Image LeftWinCover, RightWinCover;

    float LeftReadyCount, RightReadyCount;
    bool isLeftReady, isRightReady;

    public Action OnPostScreenReady;

    [SerializeField] CanvasGroup canvasGroup;

    private PlayerBetController LBC, RBC;

    string PlayerReadyText = "Ready!";

    string LeftreadyUpKey = "Left Shift";

    string RightreadyUpKey =  "Key Pad Enter";

    string GetReadyText;

    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = this.GetComponent<CanvasGroup>();
        Hide();
    }

    public void Initialize(PlayerBetController leftC, PlayerBetController Rightc)
    {
        LBC = leftC;
        RBC = Rightc;
    }

    void Refresh(Player lostPlayer)
    {

        //Debug.Log($" lost  {lostPlayer.ToString()}");

        //Debug.Log($" left player win?  {lostPlayer != LBC.playerType}");
        //Debug.Log($" r player win?  {lostPlayer != RBC.playerType}");

        LeftWinCover.enabled =(lostPlayer != LBC.playerType);
        RightWinCover.enabled =(lostPlayer != RBC.playerType);


        LeftBet.color = LBC.didPlayerWinBet ? Color.green : Color.red;

        LeftBet.text =(LBC.didPlayerWinBet ? "+":"-") +    LBC.GetBetAmount.ToString();

        LeftTotal.text = LBC.GetTotalAmount.ToString();

        RightBet.color = RBC.didPlayerWinBet ? Color.green : Color.red;

        RightBet.text = (RBC.didPlayerWinBet ? "+" : "-") + RBC.GetBetAmount.ToString();

        RightTotal.text = RBC.GetTotalAmount.ToString();
    }

    public void Show(Player lostPlayer)
    {
        Refresh(lostPlayer);
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void Hide()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (SceneManager.instance.CurrentScene != SceneManager.Scene.PostFight) return;

        if (Input.GetKey(KeyCode.LeftShift) )
        {
            LeftReadyCount += Time.fixedDeltaTime;
            if (LeftReadyCount >= 3)
            {
                isLeftReady = true;
                LeftReadyText.text = PlayerReadyText;
            }
        }
        else if(LeftReadyCount < 3)
        {
            LeftReadyCount =0;
        }

        

        if (Input.GetKey(KeyCode.KeypadEnter) )
        {
            RightReadyCount += Time.fixedDeltaTime;
            if (RightReadyCount >= 3)
            {
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


    }
}
