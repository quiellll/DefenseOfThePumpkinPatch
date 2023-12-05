using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class HUDMenu : MonoBehaviour
{
    public GameObject StartWaveButton { get => _startWaveButton; }
    public UnityEvent StartWave;

    [SerializeField] private GameObject _turretConstructionUIPanel;
    [SerializeField] private GameObject _startWaveButton;

    public void OnMenuButtonPressed(InputAction.CallbackContext context)
    {
        if (context.started)
            _turretConstructionUIPanel.SetActive(!_turretConstructionUIPanel.activeSelf);
    }

    public void OnStartWaveButtonPressed() => StartWave?.Invoke();
}
