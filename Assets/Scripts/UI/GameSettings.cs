using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class GameSettings : MonoBehaviour
{

    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private TMP_Dropdown _graphicsDropdown;
    [SerializeField] private Toggle _accesibilityToggle;

    [SerializeField] private float _defaultMusicVolume;
    [SerializeField] private float _defaultSFXVolume;
    [SerializeField] private int _defaultGraphics;

    private IAudioManager _audioManager;
    private TextZoomController _textZoomController;


    public void LoadSettings()
    {
        _audioManager = GameManager.Instance.ServiceLocator.Get<IAudioManager>();

        _musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", _defaultMusicVolume);
        _audioManager.SetMusicVolume(_musicSlider.value);
        _sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", _defaultSFXVolume);
        _audioManager.SetSoundEffectsVolume(_sfxSlider.value);

        _graphicsDropdown.value = PlayerPrefs.GetInt("Graphics", _defaultGraphics);
        if(QualitySettings.GetQualityLevel() != _graphicsDropdown.value)
            QualitySettings.SetQualityLevel(_graphicsDropdown.value, true);

        _textZoomController = FindObjectOfType<TextZoomController>();
        bool accOn = PlayerPrefs.GetInt("Accesibility", 0) == 1;
        _textZoomController.ToggleAccesibility(accOn);
        _accesibilityToggle.isOn = accOn;
    }

    public void SetMusicVolume(float value)
    {
        PlayerPrefs.SetFloat("MusicVolume",value);
        _audioManager.SetMusicVolume(value);
    }

    public void SetSFXVolume(float value)
    {
        PlayerPrefs.SetFloat("SFXVolume", value);
        _audioManager.SetSoundEffectsVolume(value);
    }

    public void SetGraphics(int value)
    {
        PlayerPrefs.SetInt("Graphics", value);
        QualitySettings.SetQualityLevel(value, true);
    }

    public void SetAccesibility(bool value)
    {
        _textZoomController.ToggleAccesibility(value);
        int acc = value ? 1 : 0;
        PlayerPrefs.SetInt("Accesibility", acc);
    }

}
