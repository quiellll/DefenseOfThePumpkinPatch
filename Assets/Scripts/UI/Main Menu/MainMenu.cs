using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private GameObject _creditsPanel;

    private void Start()
    {
        _settingsPanel.GetComponent<MenuSettings>().LoadSettings();
    }

    public void PlayButton()
    {
        SceneManager.LoadScene("Level 1");
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
