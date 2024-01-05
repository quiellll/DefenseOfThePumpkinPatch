using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourcesUI : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI _pumpkinsText;
    [SerializeField] TextMeshProUGUI _goldText;

    private void Start()
    {
        GameManager.Instance.PumpkinsChanged.AddListener(SetPumpkinsText);
        GameManager.Instance.GoldChanged.AddListener(SetGoldText);

        SetPumpkinsText(GameManager.Instance.Pumpkins);    
        SetGoldText(GameManager.Instance.Gold);    
    }

    private void OnDestroy()
    {
        GameManager.Instance?.PumpkinsChanged.RemoveListener(SetPumpkinsText);
        GameManager.Instance?.GoldChanged.RemoveListener(SetGoldText);
    }

    private void SetPumpkinsText(int pumpkins)
    {
        _pumpkinsText.text = "Pumpkins: " + pumpkins;
    }
    
    private void SetGoldText(int gold)
    {
        _goldText.text = "    Gold: " + gold;
    }
}
