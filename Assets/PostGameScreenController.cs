using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PostGameScreenController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI WinText;
    [SerializeField] private Button EndGame;

    string LeftWintText = "Blue Player Won!";

    string RightWintText = "Red Player Won!";

    private CanvasGroup _canvasGroup;
    private CanvasGroup CanvasGroup
    {
        get
        {
            if (_canvasGroup == null) _canvasGroup = GetComponent<CanvasGroup>();
            return _canvasGroup;

        }
    }
    private void Start()
    {
        FightSceneManager.instance.OnGameEndHandler += Show;
        EndGame.onClick.AddListener(LeaveGame);
        Hide();
    }
    public void Show(Player winPlayer) {
        SetWinText(winPlayer);
        CanvasGroup.alpha = 1;
        CanvasGroup.interactable = true;
        CanvasGroup.blocksRaycasts = true;
    }

    public void Hide() {
        CanvasGroup.alpha = 0;
        CanvasGroup.interactable = false;
        CanvasGroup.blocksRaycasts = false;
    }

    public void SetWinText(Player winPlayer)
    {
        WinText.text = winPlayer == Player.Left ? LeftWintText : RightWintText;
    }



    public void LeaveGame()
    {
        SceneManager.LoadScene("StartScene");
    }
}
