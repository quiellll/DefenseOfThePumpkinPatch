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


    // Start is called before the first frame update
    void Start()
    {

        _bigTutorialPanel.gameObject.SetActive(false);
        _smallTutorialPanel.gameObject.SetActive(false);

        if (PlayerPrefs.GetInt("TutorialEnded", 0) == 1)
            return;
        

        SetTutorialToPanel(_dayTutorials.GetFirstTutorial());

    }

    public void EndTutorial()
    {
        PlayerPrefs.SetInt("TutorialEnded", 1);
    }

    private void SetTutorialToPanel(Tutorial tutorial)
    {
        if (tutorial.UseSmall) _smallTutorialPanel.SetTutorial(tutorial);
        else _bigTutorialPanel.SetTutorial(tutorial);
    }

    public void NextTutorial(Tutorial current)
    {
        if (!_dayTutorials.HasNext) EndTutorial();

        else SetTutorialToPanel(_dayTutorials.GetNextTutorial());
    }
}
