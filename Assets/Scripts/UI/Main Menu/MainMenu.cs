using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _settingsPanel;

    public void PlayButton()
    {
        SceneManager.LoadScene("UI Test");
    }

    public void SettingsButton()
    {
        _menuPanel.SetActive(false);
        _settingsPanel.SetActive(true);
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
}
