using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SproutAttributesMenu : MonoBehaviour
{
    public Pumpkin Pumpkin;

    [SerializeField] private TextMeshProUGUI _buyPrice;

    // Start is called before the first frame update
    void Start()
    {
        _buyPrice.text = "Buy Price: " + Pumpkin.BuyPrice.ToString();
    }
}
