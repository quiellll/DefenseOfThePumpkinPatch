using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private GameObject _creditsPanel;
    [SerializeField] private Button _continueButton;
    [SerializeField] private string _saveFileName;
    [SerializeField] private Tutorials[] _tutorials;

    private bool _canContinueGame = false;

    private void Awake()
    {
        var directory = Application.dataPath + "/Saves/Binary";
        if (!Directory.Exists(directory))
        {
            _canContinueGame = false;
            return;
        }

        var path = $"{directory}/{_saveFileName}.save";
        if (!File.Exists(path))
        {
            _canContinueGame = false;
            return;
        }

        _canContinueGame = true;
    }

    private void Start()
    {
        _settingsPanel.GetComponent<MenuSettings>().LoadSettings();

        if(!_canContinueGame)
        {
            _continueButton.interactable = false;
        }
    }

    public void PlayButton(bool newGame)
    {
        if (newGame)
        {
            PlayerPrefs.SetInt("NewGame", 1);
            foreach(var t in _tutorials) PlayerPrefs.SetInt($"{t.name}TutorialEnded", 0);
        }
        else PlayerPrefs.SetInt("NewGame", 0);
        SceneManager.LoadScene(1);
    }

    public void SettingsButton()
    {
        _menuPanel.SetActive(false);
        _settingsPanel.SetActive(true);
    }

    public void CreditsButton()
    {
        _menuPanel.SetActive(false);
        _creditsPanel.SetActive(true);
    }

    public void QuitButton()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    public void CloseSettings()
    {
        _menuPanel.SetActive(true);
        _settingsPanel.SetActive(false);
    }

    public void CloseCredits()
    {
        _menuPanel.SetActive(true);
        _creditsPanel.SetActive(false);
    }
}
