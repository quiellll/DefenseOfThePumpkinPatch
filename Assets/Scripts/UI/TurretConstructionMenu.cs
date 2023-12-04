using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class TurretConstructionMenu : MonoBehaviour
{
    [SerializeField] private GameObject _menuPanel;

    public void OnMenuButtonPressed(InputAction.CallbackContext context)
    {
        if (context.started) _menuPanel.SetActive(!_menuPanel.activeSelf);
    }
}
