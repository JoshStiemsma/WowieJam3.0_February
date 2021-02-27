using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.Events;

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
    public Button LeftReady, RightReady;

    public GameObject BettingCanvas;

    void Start()
    {
        CurrentScene = Scene.Betting;
        BettingCanvas.SetActive(true);

        LeftReady.onClick.AddListener(() => SetPlayerReady(true));
        RightReady.onClick.AddListener(() => SetPlayerReady(false));

    }

    void SetPlayerReady(bool isLeft)
    {

    }


    public void SetSceneFighting()
    {
        SetSceneType(Scene.Fighting);
    }

    private void SetSceneType(Scene s)
    {
        CurrentScene = s;
    }

}
