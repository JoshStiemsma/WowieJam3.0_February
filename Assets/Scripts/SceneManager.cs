using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    void Start()
    {
        CurrentScene = Scene.Betting;
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
