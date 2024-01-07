using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] TutorialPanel _bigTutorialPanel;
    [SerializeField] TutorialPanel _smallTutorialPanel;

    [SerializeField] private Tutorials _dayTutorials;
    [SerializeField] private Tutorials _nightTutorials;

    private Tutorials _currentTutorials;


    // Start is called before the first frame update
    void Start()
    {

        _bigTutorialPanel.gameObject.SetActive(false);
        _smallTutorialPanel.gameObject.SetActive(false);


        if(PlayerPrefs.GetInt($"{_nightTutorials.name}TutorialEnded", 0) == 0)
        {
            if (GameManager.Instance.StartsOnDay) GameManager.Instance.StartBuildMode.AddListener(OnFirstNight);
            else OnFirstNight();
            return;
        }
            

        if (PlayerPrefs.GetInt($"{_dayTutorials.name}TutorialEnded", 0) == 1)
            return;
        
        //on first day

        GameManager.Instance.HUD.StartWaveButton.SetActive(false);
        _currentTutorials = _dayTutorials;
        SetTutorialToPanel(_dayTutorials.GetFirstTutorial());

    }

    private void OnFirstNight()
    {
        GameManager.Instance.StartBuildMode.RemoveListener(OnFirstNight);
        GameManager.Instance.HUD.StartWaveButton.SetActive(false);
        _currentTutorials = _nightTutorials;
        SetTutorialToPanel(_nightTutorials.GetFirstTutorial());

    }

    public void EndTutorial()
    {
        GameManager.Instance.HUD.StartWaveButton.SetActive(true);
        PlayerPrefs.SetInt($"{_currentTutorials.name}TutorialEnded", 1);
    }

    private void SetTutorialToPanel(Tutorial tutorial)
    {
        if (tutorial.UseSmall) _smallTutorialPanel.SetTutorial(tutorial);
        else _bigTutorialPanel.SetTutorial(tutorial);
    }

    public void NextTutorial(Tutorial current)
    {
        if (!_currentTutorials.HasNext) EndTutorial();

        else SetTutorialToPanel(_currentTutorials.GetNextTutorial());
    }

}
