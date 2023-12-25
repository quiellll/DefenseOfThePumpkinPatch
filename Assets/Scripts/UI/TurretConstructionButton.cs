using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretConstructionButton : MonoBehaviour
{
    [SerializeField] private Turret _turret;

    //al pulsar un boton de torreta, asigna la torreta a construir que tenga en _turret
    public void SelectTurret()
    {
        //Debug.Log(_turret.name);
        GameManager.Instance.TurretToBuild = _turret;
    }
    
}
