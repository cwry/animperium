using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class MoveUnitButton : MonoBehaviour {
    
	public  void OnClick(PointerEventData data)
    {
        //GameObject tile = gameObject.GetComponent<ContextMenuPosition>().tile;
        PathMovementManager.move(SelectionManager.selectedUnit, GUIData.targetTile);
    }
}
