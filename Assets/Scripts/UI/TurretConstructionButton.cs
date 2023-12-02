using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretConstructionButton : MonoBehaviour
{
    [SerializeField] private Turret _turret;

    public void SelectTurret()
    {
        GameManager.Instance.TurretToBuild = _turret;
    }
}
