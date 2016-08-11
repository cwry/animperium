using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour
{

    public static GameObject oldSelectedUnit;
    void Update()
    {
        GameObject tileHover = getTileHover();
        SelectionManager.hoverTile = tileHover;
        setSelection(tileHover);
    }

    private GameObject getTileHover()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            return hit.collider.gameObject;
        }
        return null;
    }

    private void setSelection(GameObject hover)
    {
        if (Input.GetMouseButtonDown(0) && !GUIData.pointerOnGUI)
        {
            SelectionManager.selectedTile = hover;
            if (hover == null) return;
            GameObject unit = hover.GetComponent<TileInfo>().unit;
            if (unit != null && !unit.GetComponent<Unit>().hidden) SelectionManager.selectedUnit = unit;
            if (SelectionManager.selectedUnit != null && !TargetingManager.getActive()){
                SelectionManager.onSelectedUnitChanged.fire(SelectionManager.selectedUnit);
            }
            oldSelectedUnit = SelectionManager.selectedUnit;
        }
    }
}
