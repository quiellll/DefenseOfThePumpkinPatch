using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _quitButton;
    [SerializeField] private RectTransform _rectTransform;

    private TutorialManager _manager;
    private Tutorial _currentTutorial;
    private void Start()
    {
        _manager = GetComponentInParent<TutorialManager>();
    }

    public void SetTutorial(Tutorial tutorial)
    {
        gameObject.SetActive(true);
        _currentTutorial = tutorial;
        _title.text = tutorial.Title;
        _text.text = tutorial.Text;
        _rectTransform.anchoredPosition = tutorial.Position;
    }

    public void NextTutorial()
    {
        gameObject.SetActive(false);
        _manager.NextTutorial(_currentTutorial);

    }

    public void EndTutorial()
    {
        _manager.EndTutorial();
        gameObject.SetActive(false);
    }

}
