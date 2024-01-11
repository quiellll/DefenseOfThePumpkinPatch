using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[RequireComponent(typeof(TextMeshProUGUI), typeof(RectTransform))]
public class TextZoom : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public UnityEvent<string> OnHover;
    public UnityEvent OnExit;

    private TextMeshProUGUI _text;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnHover.Invoke(_text.text);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnExit.Invoke();
    }

    private void OnDisable()
    {
        OnExit.Invoke();
    }

}
