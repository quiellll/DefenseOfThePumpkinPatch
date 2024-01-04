using UnityEngine;

public class BuildManager
{
    public IWare WareToBuild { get; private set; }
    //figura de la torreta o brote de calabaza semitransparente para elegir donde construirla
    public GameObject Dummy { get; private set; }

    private SelectionManager _selectionManager;

    public BuildManager(SelectionManager selectionManager)
    {
        _selectionManager = selectionManager;
    }

    //comprueba si se ha elegido construir una torreta o calabaza y mueve el dummy, lo desactiva o activa segun corresponda
    public void DummyPlacing()
    {
        if (!Dummy) return;

        if (!_selectionManager.SelectedCell || _selectionManager.SelectedCell.Type != WareToBuild.CellType)
        {
            if (Dummy.activeSelf) Dummy.SetActive(false);
            return;
        }

        if (!Dummy.activeSelf) Dummy.SetActive(true);
        Dummy.transform.position = _selectionManager.SelectedCell.transform.position + Vector3.up * Dummy.transform.position.y;
    }

    //instancia el dummy cuando se decide construir una torreta o calabaza
    public void SetWareToBuild(IWare ware)
    {
        if (Dummy != null)
        {
            GameManager.Instance.DestroyDummy(Dummy);
        }

        WareToBuild = ware;
        Dummy = GameManager.Instance.SpawnDummy(WareToBuild.Dummy);

        foreach(var mr in Dummy.GetComponentsInChildren<MeshRenderer>())
        {
            mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        }


        if (_selectionManager.SelectedObject && _selectionManager.SelectedCell
            && _selectionManager.SelectedCell.Type == WareToBuild.CellType)
        {
            Dummy.transform.position = _selectionManager.SelectedCell.transform.position;
            return;
        }

        Dummy.SetActive(false);
    }

    public void RemoveWareToBuild()
    {
        if (Dummy != null)
        {
            GameManager.Instance.DestroyDummy(Dummy);
            Dummy = null;
        }
        WareToBuild = null;
    }

    public bool CanBuildWare(IWare ware)
    {
        return
            WareToBuild != null &&
            WareToBuild == ware &&
            Dummy != null &&
            Dummy.activeSelf &&
            _selectionManager.SelectedCell != null &&
            _selectionManager.SelectedCell.ElementOnTop == null &&
            _selectionManager.SelectedCell.Type == WareToBuild.CellType &&
            (GameManager.Instance.Gold - WareToBuild.BuyPrice) >= 0;
    }
}