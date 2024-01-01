using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShopMenu : MonoBehaviour
{
    //funcion llamada al presionar la telca T (de momento) que oculta/muestra la tienda
    public void ToggleShop(InputAction.CallbackContext context)
    {
        if (context.started) ToggleShop();
    }
    public void ToggleShop()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    //metodo que hay que asignar a un boton de comprar torreta
    public void BuyTurret(Turret turret)
    {
        GameManager.Instance.CommandManager.ExecuteCommand(new SetTurretToBuy(turret));
    }

    public void BuyPumpkinSprout(Pumpkin pumpkin)
    {
        GameManager.Instance.CommandManager.ExecuteCommand(new SetPumpkinSproutToBuy(pumpkin));
    }
}
