using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("ScriptableObjects/Tutorials"))]
public class Tutorials : ScriptableObject 
{
    public bool HasNext { get =>_index < _tutorials.Length - 1; }


    [SerializeField] private Tutorial[] _tutorials;

    private int _index = 0;

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
}

[Serializable]
public class Tutorial
{
    public string Title { get => _title; }
    public string Text { get => _text; }
    public bool UseSmall { get => _useSmall; }
    public Vector2 Position { get => _position; }


    [SerializeField] private string _title;
    [SerializeField][TextArea(5,10)] private string _text;
    [SerializeField] private bool _useSmall;
    [SerializeField] private Vector2 _position;
}





