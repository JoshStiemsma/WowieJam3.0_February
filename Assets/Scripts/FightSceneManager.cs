
using System;
using System.Collections;
using UnityEngine;

public class FightSceneManager : MonoBehaviour
{

    public static FightSceneManager instance;
    public enum Scene{
        Betting,
        Fighting,
        Death,
        PostFight,
        PostGame
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
    public OnGameEnd OnGameEndHandler;

    public delegate void OnGameEnd(Player WinPlayer);

    public PostFightSreenController fightScreen;
    public PostGameScreenController EndScreen;

    public GameplayHud hud;

    void Awake()
    {
        if (instance != null) Destroy(this);
        instance = this;
        BettingCanvasGroup = BettingCanvas.GetComponent<CanvasGroup>();
        SetSceneType(Scene.Betting);
    }

    void Start() { 
        CurrentScene = Scene.Betting;
        BettingCanvas.SetActive(true);
        LeftBetCont.OnPlayerReady += OnLeftReady;
        RightBetCont.OnPlayerReady += OnRigtReady;
        fightScreen.Initialize(LeftBetCont,RightBetCont);
        
        fightScreen.OnPostScreenReady += OnPostSceneReady;
       // OnGameEndHandler +=  SetSceneType(Scene.PostGame);
       


    }

    public void PlayerCalledThrow(Player player)
    {
        if(player == Player.Left)
        {
            Debug.Log($"LEFT player thinks  RIGHT player threw");
            if (!RightBetCont.BetOnSelf)
            {
                PlayerCalledThrowSolution(Player.Right,Player.Left);//wrong, right  loser, winner
            }
            else
            {
                PlayerCalledThrowSolution(Player.Left, Player.Right);//wrong, right  loser, winner
            }
        }else
        {
            Debug.Log($"Right player thinks  Left player threw");
            if (!LeftBetCont.BetOnSelf)
            {
                //correct
                PlayerCalledThrowSolution(Player.Left, Player.Right);//wrong, right  loser, winner
            }
            else
            {
                //wrong
                PlayerCalledThrowSolution(Player.Right, Player.Left);//wrong, right  loser, winner
            }
        }
    }


    private void OnPostSceneReady()
    {
        LeftBetCont.Reset();
        RightBetCont.Reset();
        SetSceneType(Scene.Betting);

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
        hud.Show();
        OnStartScene.Invoke();
    }

    private void SetSceneType(Scene s)
    {
        CurrentScene = s;

        if (s == Scene.Fighting) hud.Show();
        else hud.Hide();

        if (s == Scene.Betting)
        {
            BettingCanvasGroup.alpha = 1;
            BettingCanvasGroup.blocksRaycasts = true;
            BettingCanvasGroup.interactable = true;
        }
        else
        {
            BettingCanvasGroup.alpha = 0;
            BettingCanvasGroup.blocksRaycasts = false;
            BettingCanvasGroup.interactable = false;
        }
        
        if(s != Scene.Fighting) fightScreen.Hide();
    }

    public void PlayerDied(Player player)
    {

       StartCoroutine(DeathPhaseRoutine(player));
        
    }


    IEnumerator DeathPhaseRoutine(Player player)
    {
        float timer = 0;
        float totalTime = 5f;
        //start confetti
        while (timer < totalTime)
        {
            timer += Time.fixedDeltaTime;

            yield return new WaitForSeconds(.01f);

        }
        //end confetti

        MoveToPostFight(player);
    }

    void MoveToPostFight(Player player)
    {
        LeftBetCont.EndRound(player);
        RightBetCont.EndRound(player);

        if (CheckGameOver()) return;

        SetSceneType(Scene.PostFight);
        ResetScene();
        fightScreen.Show(player);
        OnRoundEnd.Invoke();
    }
   

    public void PlayerCalledThrowSolution(Player lostPlayer, Player winPlayer)
    {
        Debug.Log($"PlayerCalledThrowSolution lost: {lostPlayer}  win:{winPlayer}");
        LeftBetCont.EndRoundByThrowCall(lostPlayer);
        RightBetCont.EndRoundByThrowCall(lostPlayer);


        if (CheckGameOver()) return;


        SetSceneType(Scene.PostFight);
        ResetScene();
        fightScreen.Show(lostPlayer);
        OnRoundEnd.Invoke();
        
    }

    bool CheckGameOver()
    {
        if (LeftBetCont.GetTotalAmount <= 0)
        {
            OnGameEndHandler(Player.Right);
            SetSceneType(Scene.PostGame);
            return true;
        }
        else if (RightBetCont.GetTotalAmount <= 0)
        {
            OnGameEndHandler(Player.Left);
            SetSceneType(Scene.PostGame);
            return true;
        }
        else
        {
            return false;
        }
    }
}
