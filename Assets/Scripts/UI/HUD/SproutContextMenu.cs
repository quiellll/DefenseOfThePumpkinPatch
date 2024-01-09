using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SproutContextMenu : MonoBehaviour
{
    public Pumpkin Pumpkin { get; private set; }
    public bool Active => gameObject.activeSelf;
    public RectTransform RectTransform { get; private set; }

    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _timeToGrow;



    private void Awake()
    {
        RectTransform = GetComponent<RectTransform>();
    }

    public bool Display(PumpkinSprout sproutController)
    {
        if (Active && Pumpkin == sproutController.Pumpkin) return false;

        if (!Active) gameObject.SetActive(true);

        Pumpkin = sproutController.Pumpkin;

        _icon.sprite = Pumpkin.SproutIcon;
        _timeToGrow.text = "Full growth in: " + (Pumpkin.SproutGrowthPeriod - sproutController.Journeys) + " days/nights.";

        return true;
    }

    public bool Hide()
    {
        if (!Active) return false;

        gameObject.SetActive(false);
        return true;
    }
}
