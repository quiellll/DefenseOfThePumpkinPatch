using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{

    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private TMP_Dropdown _graphicsDropdown;

    [SerializeField] private float _defaultMusicVolume;
    [SerializeField] private float _defaultSFXVolume;
    [SerializeField] private int _defaultGraphics;

    private IAudioManager _audioManager;

    // Start is called before the first frame update
    void Start()
    {
        _audioManager = GameManager.Instance.ServiceLocator.Get<IAudioManager>();

        LoadSettings();

    }

    private void LoadSettings()
    {
        _musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", _defaultMusicVolume);
        _sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", _defaultSFXVolume);

        _graphicsDropdown.value = PlayerPrefs.GetInt("Graphics", _defaultGraphics);
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
    }

}
