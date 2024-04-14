using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitButton : MonoBehaviour
{
    [SerializeField] private UnitInfo info;
    [SerializeField] private Image cooldownImage;
    [SerializeField] private new Animation animation;
    [SerializeField] private TextMeshProUGUI costText;

    private float cooldownTime = 0.0f;
    private bool interactable = true;

    public bool OnCooldown
    {
        get; private set;
    }

    public bool Clicked
    {
        get; private set;
    }

    private void Start()
    {
        if (info.cost > 0)
        {
            costText.text = info.cost.ToString();
        }
    }

    private void Update()
    {
        if (OnCooldown)
        {
            float normalizedCooldown = (Time.realtimeSinceStartup - cooldownTime) / info.cooldown;
            cooldownImage.fillAmount = 1 - normalizedCooldown;

            if (Time.realtimeSinceStartup - cooldownTime > info.cooldown)
            {
                OnCooldown = false;
                cooldownImage.fillAmount = 0;
            }
        }
    }

    public void OnClick()
    {
        if (info == null)
            return;

        if (!Clicked)
        {
            CursorSpawner.Cancel();
            CancelOthers(this);
            Clicked = true;
            animation.PlayQueued("Anim_hud_unit_button_down");

            CursorSpawner.SetCursorUnit(this, info, () =>
            {
                cooldownTime = Time.realtimeSinceStartup;
                OnCooldown = true;
                GameManager.Numbers.Credits -= info.cost;
            }, () =>
            {
                animation.PlayQueued("Anim_hud_unit_button_up");
            });
        } else
        {
            Clicked = false;
            CursorSpawner.Cancel();
        }
    }

    public static void CancelOthers(UnitButton exclude)
    {
        var otherButtons = FindObjectsByType<UnitButton>(FindObjectsSortMode.None);

        foreach (var button in otherButtons)
        {
            if (exclude == null || button != exclude)
            {
                button.Cancel();
            }
        }
    }

    public void Cancel()
    {
        if (Clicked)
        {
            Clicked = false;
            animation.PlayQueued("Anim_hud_unit_button_up");
        }
    }

    public void SetInteractable(bool interactable)
    {
        this.interactable = interactable;

        if (!interactable)
        {
            animation.PlayQueued("Anim_hud_unit_button_disabled");
        }
    }

    public bool IsInteractable()
    {
        return interactable;
    }

}
