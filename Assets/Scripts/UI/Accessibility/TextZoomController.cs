using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using SpeechLib;

public class TextZoomController : MonoBehaviour
{
    [SerializeField] private string _bgColor;

    [SerializeField] private TextMeshProUGUI _zoomedText;
    [SerializeField] private TextMeshProUGUI _bgText;
    private TextZoom[] _texts;

    private bool _accesibilityOn;
    private SpVoice _voice;
    private string _displayedText;
    private bool _tutorialPlaying = false;


    private void Awake()
    {
        _voice = new();
        var v = _voice.GetVoices();
        _texts = FindObjectsOfType<TextZoom>(true);

        foreach(TextZoom t in _texts)
        {
            t.OnHover.AddListener(SetZoomedText);
            t.OnExit.AddListener(ClearZoomedText);
        }
    }

    private void SetZoomedText(string text)
    {
        if (!_accesibilityOn) return;
        _displayedText = text;
        _bgText.text = $"<mark=#{_bgColor}>{text}</mark>";
        _zoomedText.text = text;
    }

    private void ClearZoomedText()
    {
        _zoomedText.text = "";
        _bgText.text = "";
        _displayedText = "";
    }

    private void OnDisable()
    {
        foreach (TextZoom t in _texts)
        {
            t.OnHover.RemoveListener(SetZoomedText);
            t.OnExit.RemoveListener(ClearZoomedText);
        }
    }

    public void OnSpeakKey(InputAction.CallbackContext context)
    {
        if (!_accesibilityOn || _tutorialPlaying) return;
        if (!context.started) return;
        if (_displayedText == "") return;

        string tts = "<speak version='1.0' xmlns='http://www.w3.org/2001/10/synthesis' xml:lang='en-US'>"
            + _displayedText + "</speak>";

        _voice.Speak(tts, 
            SpeechVoiceSpeakFlags.SVSFlagsAsync | 
            SpeechVoiceSpeakFlags.SVSFPurgeBeforeSpeak | 
            SpeechVoiceSpeakFlags.SVSFIsXML);


    }

    public void ToggleAccesibility(bool on)
    {
        _accesibilityOn = on;
        if (!on) ClearZoomedText();
    }

    public void SayTutorial()
    {
        string tutorial = "Accessibility mode is on. use your mouse to hover over any text, " +
            "and it will be displayed magnified at the center of the screen. " +
            "Press the SPACE key to have the text narrated by a voice.";

        SetZoomedText(tutorial);
        Invoke(nameof(SpeakTutorial), .1f);
    }

    private void SpeakTutorial()
    {
        _tutorialPlaying = true;
        string tts = "<speak version='1.0' xmlns='http://www.w3.org/2001/10/synthesis' xml:lang='en-US'>"
            + _displayedText + "</speak>";
        _voice.Speak(tts,
            SpeechVoiceSpeakFlags.SVSFPurgeBeforeSpeak |
            SpeechVoiceSpeakFlags.SVSFIsXML);
        ClearZoomedText();
        _tutorialPlaying = false;
    }
}
