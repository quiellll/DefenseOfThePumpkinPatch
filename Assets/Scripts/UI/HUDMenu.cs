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
    public GameObject UndoButton { get => _undoButton; }
    public GameObject TimeScaleButton { get => _timeScaleButton; }
    public UnityEvent WaveStarted; //evento que se lanza al pulsar el boton de iniciar oleada

    [SerializeField] private GameObject _startWaveButton;
    [SerializeField] private GameObject _timeScaleButton;
    [SerializeField] private GameObject _shopButton;
    [SerializeField] private GameObject _undoButton;

    private GameObject _x1Sprite;
    private GameObject _x2Sprite;
    private RectTransform _shopButtonCanvas;

    ShopMenu _shopMenu;

    private void Awake()
    {
        _x1Sprite = _timeScaleButton.transform.GetChild(0).gameObject;
        _x2Sprite = _timeScaleButton.transform.GetChild(1).gameObject;
        _shopButtonCanvas = _shopButton.GetComponent<RectTransform>();
    }

    private void Start()
    {
        _shopMenu = FindObjectOfType<ShopMenu>(true);
        UpdateUndoButton();
    }


    #region HUD  Listeners
    //estas funciones se asignan a los elementos del HUD en el inspector (botones y demas) como listeners en sus eventos

    public void ToggleShop()
    {
        if (_shopMenu.gameObject.activeSelf) _shopButtonCanvas.anchoredPosition += new Vector2(500f, 0f);
        else _shopButtonCanvas.anchoredPosition -= new Vector2(500f, 0f);
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
        UpdateUndoButton();
    }

    public void UpdateUndoButton()
    {
        _undoButton.SetActive(GameManager.Instance.CommandManager.CanUndo() && !GameManager.Instance.IsOnDefense);
    }


    public void PauseGame()
    {
        //pausa tal
    }


    #endregion
}
