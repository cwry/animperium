using UnityEngine;
using System.Collections;

public class MoveUnitButton : MonoBehaviour {
    
	public void OnClick()
    {
        //GameObject tile = gameObject.GetComponent<ContextMenuPosition>().tile;
        PathMovementManager.move(SelectionManager.selectedUnit, GUIData.targetTile);
    }
}
