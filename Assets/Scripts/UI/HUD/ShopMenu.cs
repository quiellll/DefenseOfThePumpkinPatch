using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShopMenu : MonoBehaviour
{
    public GameObject BalistaTabButton;
    public GameObject CatapultTabButton;
    public GameObject MagicTabButton;
    public GameObject SeedsTabButton;

    [SerializeField] private GameObject _cancelPurchaseButton;

    private GameObject _purchaseButtonPressed;


    private void Start()
    {
        GameManager.Instance.SelectionManager.WareBuilt.AddListener(HideCancelPurchaseButton);
    }

    private void OnDestroy()
    {
        GameManager.Instance?.SelectionManager?.WareBuilt.RemoveListener(HideCancelPurchaseButton);
    }



    //metodos que hay que asignar a los botones de la tienda
    public void SetTurretToBuy(Turret turret)
    {
        bool executed = GameManager.Instance.CommandManager.ExecuteCommand(new SetWareToBuy(turret));
        if (!executed) 
            ToggleCancelPurchaseButton(false);
    }

    public void SetPurchaseButtonPressed(GameObject button)
    {
        _purchaseButtonPressed = button;
        ToggleCancelPurchaseButton(true);
    }

    public void SetPumpkinSproutToBuy(Pumpkin pumpkin)
    {
        bool executed = GameManager.Instance.CommandManager.ExecuteCommand(new SetWareToBuy(pumpkin));
        if (!executed) ToggleCancelPurchaseButton(false);
    }

    public void RemoveWareToBuy()
    {
        if (_purchaseButtonPressed == null) return;

        GameManager.Instance.CommandManager.ExecuteCommand(new RemoveWareToBuy());
        ToggleCancelPurchaseButton(false);

    }

    private void HideCancelPurchaseButton() => ToggleCancelPurchaseButton(false);

    public void ToggleCancelPurchaseButton(bool active)
    {
        _purchaseButtonPressed.SetActive(!active);
        _cancelPurchaseButton.SetActive(active);
    }

}
