using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{

    [SerializeField] private Slider _volumeSlider;
    [SerializeField] private TMP_Dropdown _graphicsDropdown;

    [SerializeField] private float _defaultVolume;
    [SerializeField] private int _defaultGraphics;

    // Start is called before the first frame update
    void Start()
    {
        LoadSettings();
    }

    private void LoadSettings()
    {
        if (PlayerPrefs.HasKey("Volume")) _volumeSlider.value = PlayerPrefs.GetFloat("Volume");
        else SetDefaultVolume();

        if (PlayerPrefs.HasKey("Graphics")) _graphicsDropdown.value = PlayerPrefs.GetInt("Graphics");
        else SetDefaultGraphics();
    }

    public void SetVolume()
    {
        PlayerPrefs.SetFloat("Volume", _volumeSlider.value);
    }

    public void SetDefaultVolume()
    {
        _volumeSlider.value = _defaultVolume;
        PlayerPrefs.SetFloat("Volume", _defaultVolume);
    }

    public void SetGraphics()
    {
        PlayerPrefs.SetInt("Graphics", _graphicsDropdown.value);
    }

    public void SetDefaultGraphics()
    {
        _graphicsDropdown.value = _defaultGraphics;
        PlayerPrefs.SetInt("Graphics", _defaultGraphics);
    }
}
