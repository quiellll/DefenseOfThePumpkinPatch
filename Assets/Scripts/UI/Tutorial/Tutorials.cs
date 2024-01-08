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
        {TutorialEvents.OpenShop, new OpenShop() },
        {TutorialEvents.Event2, new Event2() }
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
    [SerializeField][TextArea(5,10)] private string _text;
    [SerializeField] private bool _useSmall;
    [SerializeField] private Vector2 _position;
    [SerializeField] private TutorialEvents _event;


}

public enum TutorialEvents
{
    None, OpenShop, Event2
}

public interface ITutorialEvent

{
    public bool Execute();
}

public class OpenShop : ITutorialEvent
{
    public bool Execute()
    {
        if (GameManager.Instance.Shop.gameObject.activeSelf) return false;
        GameManager.Instance.HUD.ToggleShop();
        Debug.Log("hola");
        return true;
    }
}
public class Event2 : ITutorialEvent
{
    public bool Execute()
    {
        Debug.Log("Hola");
        return true;
    }
}




