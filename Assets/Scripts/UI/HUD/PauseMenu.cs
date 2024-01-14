using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _pauseMenuPanel;
    [SerializeField] private GameObject _settingsMenuPanel;

    public void PauseGame()
    {
        _pauseMenu.SetActive(true);
        _pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
        GameManager.Instance.Paused = true;
    }

    public void ResumeGame()
    {
        _pauseMenuPanel.SetActive(false);
        GameManager.Instance.HUD.ResumeGame();
        Time.timeScale = GameManager.Instance.TimeScale;
        GameManager.Instance.Paused = false;
        _pauseMenu.SetActive(false);
    }

    public void OpenSettings()
    {
        _pauseMenuPanel.SetActive(false);
        _settingsMenuPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        _pauseMenuPanel.SetActive(true);
        _settingsMenuPanel.SetActive(false);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        PlayerPrefs.SetInt("NewGame", 0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit() => Application.Quit();
}
