using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Outline), typeof(Collider))]

public class Selectable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{

    private Outline _outline;
    private void Awake()
    {
        _outline = GetComponent<Outline>();
    }

    private void Start()
    {
        _outline.OutlineMode = Outline.Mode.OutlineAll;
        _outline.OutlineColor = Color.white;
        _outline.OutlineWidth = 8f;
        _outline.enabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManager.Instance.SetSelectedObject(this);
        _outline.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.Instance.RemoveSelectedObject(this);
        _outline.enabled = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log(gameObject.name);
    }


}
