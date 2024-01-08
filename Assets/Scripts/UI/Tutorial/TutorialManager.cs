using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] TutorialPanel _bigTutorialPanel;
    [SerializeField] TutorialPanel _smallTutorialPanel;

    [SerializeField] GameObject _confirmationScreen;

    [SerializeField] private Tutorials _dayTutorials;
    [SerializeField] private Tutorials _nightTutorials;
    [SerializeField] private Tutorials _defenseTutorials;

    private Tutorials _currentTutorials;

    private TutorialPanel _activePanel;

    // Start is called before the first frame update
    void Start()
    {

        _bigTutorialPanel.gameObject.SetActive(false);
        _smallTutorialPanel.gameObject.SetActive(false);


        if(PlayerPrefs.GetInt($"{_nightTutorials.name}TutorialEnded", 0) == 0)
        {
            if (GameManager.Instance.StartsOnDay) GameManager.Instance.StartBuildMode.AddListener(OnFirstNight);
            else
            {
                OnFirstNight();
                return;
            }
        }

        if(PlayerPrefs.GetInt($"{_defenseTutorials.name}TutorialEnded", 0) == 0)
        {
            if (GameManager.Instance.StartsOnDay) GameManager.Instance.StartDefenseMode.AddListener(OnFirstDefense);
            else
            { 
                OnFirstNight();
                return;
            }
        }


        if (PlayerPrefs.GetInt($"{_dayTutorials.name}TutorialEnded", 0) == 1)
        {
            return;
        }
        
        //on first day

        GameManager.Instance.HUD.StartWaveButton.SetActive(false);
        _currentTutorials = _dayTutorials;
        SetTutorialToPanel(_dayTutorials.GetFirstTutorial());

    }

    private void OnFirstNight()
    {
        GameManager.Instance.StartBuildMode.RemoveListener(OnFirstNight);
        GameManager.Instance.HUD.StartWaveButton.SetActive(false);
        _currentTutorials = _nightTutorials;
        SetTutorialToPanel(_nightTutorials.GetFirstTutorial());
    }

    private void OnFirstDefense() 
    {
        GameManager.Instance.StartDefenseMode.RemoveListener(OnFirstDefense);
        _currentTutorials = _defenseTutorials;
        SetTutorialToPanel(_defenseTutorials.GetFirstTutorial());
    }

    public void EndCurrentTutorial()
    {
        // Esto para que solo se muestre el botón de start en los tutoriales que aparecen en el modo construir
        if (GameManager.Instance.IsOnDefense) GameManager.Instance.HUD.StartWaveButton.SetActive(false);

        if (!GameManager.Instance.IsOnDefense)
        {
            GameManager.Instance.HUD.StartWaveButton.SetActive(true);
            GameManager.Instance.HUD.ShopButton.SetActive(true);
        }

        PlayerPrefs.SetInt($"{_currentTutorials.name}TutorialEnded", 1);
        Debug.Log($"{_currentTutorials.name} Ended");
    }

    private void SetTutorialToPanel(Tutorial tutorial)
    {
        if (tutorial.UseSmall) _smallTutorialPanel.SetTutorial(tutorial);
        else _bigTutorialPanel.SetTutorial(tutorial);

        _currentTutorials.ExecuteCurrentTutorialEvent();
    }

    public void NextTutorial()
    {
        if (!_currentTutorials.HasNext) EndCurrentTutorial();

        else SetTutorialToPanel(_currentTutorials.GetNextTutorial());
    }

    public void ConfirmTutorialClosing(TutorialPanel current)
    {
        _activePanel = current;
        if (_activePanel == null) return;
        else
        {
            _confirmationScreen.SetActive(true);
            _activePanel.gameObject.SetActive(false);
        }
    }

    public void ResumeTutorial()
    {
        _confirmationScreen.SetActive(false);
        _activePanel.gameObject.SetActive(true);
    }

    public void FinishTutorial()
    {
        // Para activar los botones
        EndCurrentTutorial();

        // Guardar en PlayerPrefs que no quiero ver los tutoriales
        PlayerPrefs.SetInt($"{_dayTutorials.name}TutorialEnded", 1);
        PlayerPrefs.SetInt($"{_nightTutorials.name}TutorialEnded", 1);
        PlayerPrefs.SetInt($"{_defenseTutorials.name}TutorialEnded", 1);

        // Desactivar el propio objeto del tutorial
        gameObject.SetActive(false);
    }
}
