using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.Events;

//en esta clase se guardan las referencias a todos los menus y botones para poder
//desactivarlos y activarlos desde otras clases cuando corresponda
//en esta clase se deberian implementar los callbacks de todos los botones del hud como:
//empezar oleada, cambiar la vel. del tiempo, deshacer accion, mostrar/ocultar tienda, etc
//tambien de las cosas del hud como el contador de oro, de calabazas, etc
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
