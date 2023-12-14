using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class HUDMenu : MonoBehaviour
{
    //boton de iniciar oleada
    public GameObject StartWaveButton { get => _startWaveButton; }
    public UnityEvent StartWave; //evento que se lanza al pulsar el boton de iniciar oleada

    [SerializeField] private GameObject _turretConstructionUIPanel; //menu de construccion de torretas
    [SerializeField] private GameObject _startWaveButton;

    //funcion llamada al presionar la telca T (de momento) que oculta/muestra el menu de const. de torretas
    public void OnMenuButtonPressed(InputAction.CallbackContext context)
    {
        if (context.started)
            _turretConstructionUIPanel.SetActive(!_turretConstructionUIPanel.activeSelf);
    }

    //funcion asignada al boton de iniciar oleada
    public void OnStartWaveButtonPressed() => StartWave?.Invoke();
}
