using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretRange : MonoBehaviour
{
    [SerializeField] private Turret _turret;
    private MeshRenderer _rangeRenderer;
    private Material _rangeMat;
    private Transform _rangeTransform;
    private Selectable _selectable;

    private bool _isDummy = false;
    
    private readonly float _rangeYOffset = 0.11f;


    private void Awake()
    {
        foreach (var mr in GetComponentsInChildren<MeshRenderer>())
        {
            if (mr.gameObject.name == "Range")
            {
                _rangeRenderer = mr;
                break;
            }
        }

        _rangeMat = _rangeRenderer.material;
        _rangeTransform = _rangeRenderer.transform;

        _rangeTransform.Translate(0f, _rangeYOffset, 0f);
        _rangeMat.SetFloat("_OuterRadius", _turret.OuterRadius);
        _rangeMat.SetVector("_Center", _rangeTransform.position);

        if (_turret.InnerRadius > 0f)
        {
            _rangeMat.SetFloat("_InnerRadius", _turret.InnerRadius);

        }


        if (!TryGetComponent<Selectable>(out var s))
        {
            _isDummy = true;
            return;
        }

        _selectable = s;

        _selectable.Selected.AddListener(() => OnSelected(true));
        _selectable.Deselected.AddListener(() => OnSelected(false));

        _rangeRenderer.enabled = false;
    }

    private void Update()
    {   
        if (!_isDummy || !_rangeTransform.gameObject.activeInHierarchy) return;

        _rangeMat.SetVector("_Center", _rangeTransform.position);

    }

    private void OnSelected(bool selected)
    {
        _rangeRenderer.enabled = selected;
    }

    private void OnDestroy()
    {
        if (_isDummy) return;
        _selectable.Selected.RemoveListener(() => OnSelected(true));
        _selectable.Deselected.RemoveListener(() => OnSelected(false));
    }
}
