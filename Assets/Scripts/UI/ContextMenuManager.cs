using UnityEngine;

public class ContextMenuManager : MonoBehaviour
{
    [SerializeField] private TurretContextMenu _turretMenu;
    [SerializeField] private PumpkinContextMenu _pumpkinMenu;
    [SerializeField] private SproutContextMenu _sproutMenu;

    private enum Menu { Turret, Pumpkin, Sprout, None }
    private Menu _activeMenu;

    private RectTransform _canvasRectTransform;

    private void Awake()
    {
        _activeMenu = Menu.None;
        _canvasRectTransform = GetComponentInParent<RectTransform>();

    }

    public bool DisplayTurretMenu(ATurretController turretController)
    {
        if (!_turretMenu.Display(turretController)) return false;

        if(_activeMenu != Menu.Turret) HideActiveMenu();

        _activeMenu = Menu.Turret;

        SetPositionOnCanvas(_turretMenu.RectTransform, turretController.transform.position);
        return true;
    }

    public bool DisplayPumpkinMenu(PumpkinController pumpkinController)
    {
        if (!_pumpkinMenu.Display(pumpkinController)) return false;

        if(_activeMenu != Menu.Pumpkin) HideActiveMenu();

        _activeMenu = Menu.Pumpkin;

        SetPositionOnCanvas(_pumpkinMenu.RectTransform, pumpkinController.transform.position);
        return true;
    }

    public bool DisplaySproutMenu(PumpkinSprout sproutController)
    {
        if (!_sproutMenu.Display(sproutController)) return false;

        if(_activeMenu != Menu.Sprout) HideActiveMenu();

        _activeMenu = Menu.Sprout;

        SetPositionOnCanvas(_sproutMenu.RectTransform, sproutController.transform.position);
        return true;
    }

    public bool HideActiveMenu()
    {
        switch (_activeMenu)
        {
            case Menu.Turret: _turretMenu.Hide(); break;
            case Menu.Pumpkin: _pumpkinMenu.Hide(); break;
            case Menu.Sprout: _sproutMenu.Hide(); break;
            case Menu.None: return false;
        }

        _activeMenu = Menu.None;
        return true;
    }

    //https://discussions.unity.com/t/how-to-convert-from-world-space-to-canvas-space/117981
    private void SetPositionOnCanvas(RectTransform rectTransform, Vector3 worldPosition)
    {
        Vector2 _viewportPosition = Camera.main.WorldToViewportPoint(worldPosition);
        Vector2 _screenPosition = new
        (((_viewportPosition.x * _canvasRectTransform.sizeDelta.x) - (_canvasRectTransform.sizeDelta.x * 0.5f)),
        ((_viewportPosition.y * _canvasRectTransform.sizeDelta.y) - (_canvasRectTransform.sizeDelta.y * 0.5f)));

        rectTransform.anchoredPosition = _screenPosition;
    }
}