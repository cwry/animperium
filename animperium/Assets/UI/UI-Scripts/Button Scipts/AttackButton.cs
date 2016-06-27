using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class AttackButton : ButtonOnClick {

	public override void OnClick(PointerEventData data)
    {
        TileInfo target = GUIData.targetTile.GetComponent<TileInfo>();
        AbilityManager.useAbility(SelectionManager.selectedUnit, "melee", target.gridPosition, target.grid.isMainGrid);
    }
}
