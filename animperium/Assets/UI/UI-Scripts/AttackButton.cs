using UnityEngine;
using System.Collections;

public class AttackButton : MonoBehaviour {

	public void OnClick()
    {
        TileInfo target = GUIData.targetTile.GetComponent<TileInfo>();
        AbilityManager.useAbility(SelectionManager.selectedUnit, 0, target.gridPosition, target.grid.isMainGrid);
    }
}
