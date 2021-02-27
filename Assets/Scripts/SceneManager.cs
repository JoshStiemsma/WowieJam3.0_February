
using UnityEngine;

public class SceneManager : MonoBehaviour
{
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

    public PlayerBetController LeftBetCont, RightBetCont;

    private bool LeftReady, RightReady;
    void Start()
    {
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
        if (LeftReady && RightReady)
            SetSceneFighting();
    }

    public void SetSceneFighting()
    {
        SetSceneType(Scene.Fighting);
    }

    private void SetSceneType(Scene s)
    {
        CurrentScene = s;
        if (s == Scene.Fighting)
            BettingCanvas.SetActive(false);
    }

}
