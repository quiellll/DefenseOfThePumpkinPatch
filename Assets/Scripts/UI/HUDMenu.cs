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
    [SerializeField] private GameObject _timeScaleButton;

    private GameObject _x1Sprite;
    private GameObject _x2Sprite;

    ShopMenu _shopMenu;

    private void Awake()
    {
        _x1Sprite = _timeScaleButton.transform.GetChild(0).gameObject;
        _x2Sprite = _timeScaleButton.transform.GetChild(1).gameObject;
    }

    private void Start()
    {
        _shopMenu = FindObjectOfType<ShopMenu>(true);
    }


    #region HUD  Listeners
    //estas funciones se asignan a los elementos del HUD en el inspector (botones y demas) como listeners en sus eventos

    public void ToggleShop()
    {
        _shopMenu.ToggleShop();       
    }

    public void StartWave()
    {
        WaveStarted.Invoke();
    }

    public void ToggleTimeScale()
    {
        GameManager.Instance.ToggleTimeScale();

        _x1Sprite.SetActive(!_x1Sprite.activeSelf);
        _x2Sprite.SetActive(!_x2Sprite.activeSelf);

    }

    public void UndoLastCommand()
    {
        GameManager.Instance.CommandManager.UndoLastCommand();
    }

    public void PauseGame()
    {
        //pausa tal
    }


    #endregion
}
