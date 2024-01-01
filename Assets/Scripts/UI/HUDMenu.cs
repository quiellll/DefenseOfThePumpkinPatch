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
    public UnityEvent WaveStarted; //evento que se lanza al pulsar el boton de iniciar oleada

    [SerializeField] private GameObject _startWaveButton;

    ShopMenu _shopMenu;

    private void Start()
    {
        _shopMenu = FindObjectOfType<ShopMenu>();
    }

    public void ToggleShop()
    {
        _shopMenu.ToggleShop();       
    }

    //funcion asignada al boton de iniciar oleada
    public void StartWave()
    {
        WaveStarted.Invoke();
    }

    
}
