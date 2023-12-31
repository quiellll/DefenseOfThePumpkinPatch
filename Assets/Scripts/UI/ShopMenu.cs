using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopMenu : MonoBehaviour
{
    //metodo que hay que asignar a un boton de comprar torreta
    public void BuyTurret(Turret turret)
    {
        GameManager.Instance.CommandManager.ExecuteCommand(new SetTurretToBuy(turret));
    }
}
