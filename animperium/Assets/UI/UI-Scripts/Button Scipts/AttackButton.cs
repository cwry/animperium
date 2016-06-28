using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class AttackButton : MonoBehaviour {

	public  void OnClick(PointerEventData data)
    {
        TileInfo target = GUIData.targetTile.GetComponent<TileInfo>();
        AbilityManager.useAbility(SelectionManager.selectedUnit, "melee", target.gridPosition, target.grid.isMainGrid);
    }
}
