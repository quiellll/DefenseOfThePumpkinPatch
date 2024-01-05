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

    // Panel completo
    [SerializeField] private GameObject _HUDPanel;
    [SerializeField] private PauseMenu _pauseMenu;
    [SerializeField] private GameObject _gameOverScreen;
    [SerializeField] private GameObject _gameWinScreen;

    // Botón para comenzar la oleada
    [SerializeField] private GameObject _startWaveButton;

    // Botón del x1/x2
    [SerializeField] private GameObject _timeScaleButton;

    // Botón de la tienda
    [SerializeField] private GameObject _shopButton;

    // Botón de deshacer
    [SerializeField] private GameObject _undoButton;

    // Contador de días y noches
    [SerializeField] private GameObject _journeyCounter;
    [SerializeField] private Sprite _daytimeSprite;
    [SerializeField] private Sprite _nighttimeSprite;
    [SerializeField] private TextMeshProUGUI _journeyNumber;

    private int _journey = 1;   // Miguel se que has dicho que tiene que empezar en 0 (que tiene sentido) pero entonces se ve un 0 en el juego
    private bool _day = false;
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
        if (_shopMenu.gameObject.activeSelf) _shopButtonCanvas.anchoredPosition += new Vector2(470f, 0f);
        else _shopButtonCanvas.anchoredPosition -= new Vector2(470f, 0f);
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
        _undoButton.SetActive(GameManager.Instance.CommandManager.CanUndo());
    }

    public void ToggleJourneyIcon()
    {
        if (_day) _journeyCounter.GetComponent<Image>().sprite = _daytimeSprite;
        else _journeyCounter.GetComponent<Image>().sprite = _nighttimeSprite;
    }

    public void IncreaseJourney()
    {
        _journey++;
        _journeyNumber.text = Mathf.Floor(_journey / 2).ToString();
        _day = !_day;
        ToggleJourneyIcon();
    }

    public void PauseGame()
    {
        _HUDPanel.SetActive(false);
        _pauseMenu.PauseGame();
    }

    public void ResumeGame()
    {
        _HUDPanel.SetActive(true);
    }

    public void GameOver()
    {
        _HUDPanel.SetActive(false);
        _gameOverScreen.SetActive(true);
    }
    #endregion
}
