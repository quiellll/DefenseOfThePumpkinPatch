using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShopMenu : MonoBehaviour
{
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

    //funcion llamada al presionar la telca T (de momento) que oculta/muestra la tienda
    public void ToggleShop(InputAction.CallbackContext context)
    {
        if (context.started) ToggleShop();
    }

    public void ToggleShop()
    {
        gameObject.SetActive(!gameObject.activeSelf);
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
