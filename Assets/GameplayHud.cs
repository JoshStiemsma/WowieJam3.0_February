using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameplayHud : MonoBehaviour
{
    [SerializeField] private Image LeftHealthBar, LeftThrowBar;
    [SerializeField] private Image RightHealthBar, RightThrowBar;


    [SerializeField] private PlayerController LeftPlayer, RightPlayer;

    private CanvasGroup _canvasGroup;
    private CanvasGroup CanvasGroup
    {
        get
        {
            if(_canvasGroup ==null) _canvasGroup = GetComponent<CanvasGroup>();
            return _canvasGroup;

        }
    }


    public void Show()
    {
        CanvasGroup.alpha = 1;
        CanvasGroup.blocksRaycasts = true;
        CanvasGroup.interactable = true;

    }

    public void Hide()
    {
        CanvasGroup.alpha = 0;
        CanvasGroup.blocksRaycasts = false;
        CanvasGroup.interactable = false;
    }

    private void Update()
    {
        if (FightSceneManager.instance.CurrentScene != FightSceneManager.Scene.Fighting) return;

        LeftHealthBar.fillAmount = LeftPlayer.PlayerHealth / 100f;
        RightHealthBar.fillAmount = RightPlayer.PlayerHealth / 100f;

        LeftThrowBar.fillAmount = LeftPlayer.callThrowTimer / 3f;
        RightThrowBar.fillAmount = RightPlayer.callThrowTimer / 3f;
    }
}
