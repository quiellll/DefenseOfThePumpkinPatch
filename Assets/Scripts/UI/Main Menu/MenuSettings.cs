using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuSettings : MonoBehaviour
{

    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private TMP_Dropdown _graphicsDropdown;

    [SerializeField] private float _defaultMusicVolume;
    [SerializeField] private float _defaultSFXVolume;
    [SerializeField] private int _defaultGraphics;

    [SerializeField] private AudioSource _menuMusic;

    // Start is called before the first frame update
    void Start()
    {
        LoadSettings();
        //_menuMusic = GetComponent<AudioSource>();
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
        _menuMusic.volume = value;
    }

    public void SetSFXVolume(float value)
    {
        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    public void SetGraphics(int value)
    {
        PlayerPrefs.SetInt("Graphics", value);
        QualitySettings.SetQualityLevel(value, true);
    }

}
