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


    //metodos que hay que asignar a los botones de la tienda
    public void SetTurretToBuy(Turret turret)
    {
        GameManager.Instance.CommandManager.ExecuteCommand(new SetWareToBuy(turret));
    }

    public void SetPumpkinSproutToBuy(Pumpkin pumpkin)
    {
        GameManager.Instance.CommandManager.ExecuteCommand(new SetWareToBuy(pumpkin));
    }

    public void RemoveWareToBuy()
    {
        GameManager.Instance.CommandManager.ExecuteCommand(new RemoveWareToBuy());
    }

}
