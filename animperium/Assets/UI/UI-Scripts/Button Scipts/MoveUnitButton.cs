using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class MoveUnitButton : ButtonOnClick {
    
	public override void OnClick(PointerEventData data)
    {
        //GameObject tile = gameObject.GetComponent<ContextMenuPosition>().tile;
        PathMovementManager.move(SelectionManager.selectedUnit, GUIData.targetTile);
    }
}
