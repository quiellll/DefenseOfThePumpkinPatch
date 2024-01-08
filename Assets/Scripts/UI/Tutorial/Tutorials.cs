using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = ("ScriptableObjects/Tutorials"))]
public class Tutorials : ScriptableObject 
{
    public bool HasNext { get =>_index < _tutorials.Length - 1; }


    [SerializeField] private Tutorial[] _tutorials;

    private int _index = 0;


    private Dictionary<TutorialEvents, ITutorialEvent> _events = new()
    {
        {TutorialEvents.ActivateShopButton, new ActivateShopButton()},
        {TutorialEvents.OpenShop, new OpenShop()},
        {TutorialEvents.ShowUndoButton, new ShowUndoButton()},
        {TutorialEvents.TutorialUpdateUndoButton, new TutorialUpdateUndoButton()},
        {TutorialEvents.ActivateSeedsTab, new ActivateSeedsTab()},
        {TutorialEvents.ActivateStartButton, new ActivateStartButton()},
    };


    public Tutorial GetFirstTutorial()
    {
        if (_tutorials == null || _tutorials.Length == 0) return null;
        _index = 0;
        return _tutorials[0];
    }
    public Tutorial GetNextTutorial()
    {
        if(_tutorials == null || _tutorials.Length == 0) return null;

        _index++;

        if (_index == _tutorials.Length) return null;

        return _tutorials[_index];
    }

    public bool ExecuteCurrentTutorialEvent()
    {
        var tutorial = _tutorials[_index];
        if (tutorial.Event == TutorialEvents.None) return false;

        return _events[tutorial.Event].Execute();

    }
}

[Serializable]
public class Tutorial
{
    public string Title { get => _title; }
    public string Text { get => _text; }
    public bool UseSmall { get => _useSmall; }
    public Vector2 Position { get => _position; }
    public TutorialEvents Event { get => _event; }  


    [SerializeField] private string _title;
    [SerializeField] [TextArea(5,10)] private string _text;
    [SerializeField] private bool _useSmall;
    [SerializeField] private Vector2 _position;
    [SerializeField] private TutorialEvents _event;
}

public enum TutorialEvents
{
    None, ActivateShopButton, OpenShop, ShowUndoButton, TutorialUpdateUndoButton, ActivateSeedsTab, ActivateStartButton
}

public interface ITutorialEvent

{
    public bool Execute();
}

public class ActivateShopButton : ITutorialEvent
{
    public bool Execute()
    {
        var shopButton = GameManager.Instance.HUD.ShopButton.gameObject;

        if (shopButton.activeSelf) return false;
        else shopButton.SetActive(true);
        return true;
    }
}

public class OpenShop : ITutorialEvent
{
    public bool Execute()
    {
        if (GameManager.Instance.Shop.gameObject.activeSelf) return false;
        GameManager.Instance.HUD.ToggleShop();
        return true;
    }
}

public class ShowUndoButton : ITutorialEvent 
{
    public bool Execute()
    {
        var undoButton = GameManager.Instance.HUD.UndoButton;
        if (undoButton.activeSelf) return false;
        else undoButton.SetActive(true);
        return true;
    }
}

public class TutorialUpdateUndoButton : ITutorialEvent
{
    public bool Execute()
    {
        if (!GameManager.Instance.HUD.isActiveAndEnabled) return false;
        GameManager.Instance.HUD.UpdateUndoButton();
        return true;
    }
}

public class ActivateSeedsTab : ITutorialEvent
{
    public bool Execute()
    {
        var seedsButton = GameManager.Instance.Shop.SeedsTabButton;
        if (seedsButton.activeSelf) return false;
        else seedsButton.SetActive(true);
        return true;
    }
}

public class ActivateStartButton : ITutorialEvent
{
    public bool Execute()
    {
        var startButton = GameManager.Instance.HUD.StartWaveButton;
        if (startButton.activeSelf) return false;
        else startButton.SetActive(true);
        return true;
    }
}