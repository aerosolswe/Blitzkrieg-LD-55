using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPopup : MonoBehaviour
{
    [SerializeField] private Animation popupAnimation;
    [SerializeField] private CanvasGroup cg;

    public void Show()
    {
        popupAnimation.Play("popup_show");
        cg.interactable = true;
        cg.blocksRaycasts = true;
    }

    public void Hide()
    {
        popupAnimation.Play("popup_hide");
        cg.interactable = false;
        cg.blocksRaycasts = false;
    }

    public void Continue()
    {
        Hide();
        GameManager.Instance.ShowMainMenu();
    }
}
