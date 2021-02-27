
using System;
using UnityEngine;

public class SceneManager : MonoBehaviour
{

    public static SceneManager instance;
    public enum Scene{
        Betting,
        Fighting
    }

    public Scene CurrentScene
    {
        set { m_currentScene = value; }
        get { return m_currentScene; }
    }

    private Scene m_currentScene;

    public GameObject BettingCanvas;
    private CanvasGroup BettingCanvasGroup;
    public PlayerBetController LeftBetCont, RightBetCont;

    private bool LeftReady, RightReady;


    public Action OnStartScene;
    public Action OnRoundEnd;

    void Awake()
    {
        if (instance != null) Destroy(this);
        instance = this;
        BettingCanvasGroup = BettingCanvas.GetComponent<CanvasGroup>();
    }

    void Start() { 
        CurrentScene = Scene.Betting;
        BettingCanvas.SetActive(true);
        LeftBetCont.OnPlayerReady += OnLeftReady;
        RightBetCont.OnPlayerReady += OnRigtReady;
    }

    void OnLeftReady() {
        LeftReady = true;
    }

    void OnRigtReady() {
        RightReady = true;
    }


    public void ResetScene()
    {
        LeftReady = false;
        RightReady = false;
    }

    public void Update()
    {
        if (LeftReady && RightReady && CurrentScene != Scene.Fighting)
            SetSceneFighting();
    }

    public void SetSceneFighting()
    {
        SetSceneType(Scene.Fighting);
        OnStartScene.Invoke();
    }

    private void SetSceneType(Scene s)
    {
        CurrentScene = s;
        BettingCanvas.SetActive(CurrentScene == Scene.Betting);

        if(CurrentScene == Scene.Betting)
        {
            BettingCanvasGroup.alpha = 1;
            BettingCanvasGroup.blocksRaycasts = false;
            BettingCanvasGroup.interactable = false;
        }
        else
        {
            BettingCanvasGroup.alpha = 0;
            BettingCanvasGroup.blocksRaycasts = true;
            BettingCanvasGroup.interactable = true;
        }

    }

    public void PlayerDied(Player player)
    {
        switch (player)
        {
            case Player.Left:
                Debug.Log("left won");
                break;
            case Player.Right:
                Debug.Log("Right won");
                break;
        }
        //balance bets
        LeftBetCont.EndRound(player);
        RightBetCont.EndRound(player);

        SetSceneType(Scene.Betting);
        ResetScene();
        OnRoundEnd.Invoke();
    }

}
