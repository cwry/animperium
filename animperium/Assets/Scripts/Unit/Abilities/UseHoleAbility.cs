using UnityEngine;
using System.Collections;

public class UseHoleAbility : MonoBehaviour {

    public AbilityInfo abilityInfo;

    void Awake() {
        abilityInfo.owner = gameObject;
        abilityInfo.checkRange = checkRange;
        abilityInfo.checkAoe = AoeChecks.dot;
        abilityInfo.execute = (Vec2i target, bool isMainGrid) => {
            AbilityManager.useAbility(abilityInfo, target, isMainGrid);
            Unit u = gameObject.GetComponent<Unit>();
            TileInfo ti = u.currentTile.GetComponent<TileInfo>();
            GridManager otherGrid = ti.grid.isMainGrid ? Data.subGrid : Data.mainGrid;
            TileInfo otherTi = otherGrid.gridData[ti.gridPosition.x, ti.gridPosition.y].GetComponent<TileInfo>();
            Camera.main.GetComponent<CameraFocus>().CameraJump(otherTi.gameObject);
        };
        abilityInfo.onExecution = executeAbility;
        abilityInfo.abilityID = GetComponent<Unit>().addAbility(abilityInfo);
    }

    void executeAbility(ServerMessage.UnitAbilityMessage msg) {
        Unit u = gameObject.GetComponent<Unit>();
        TileInfo ti = u.currentTile.GetComponent<TileInfo>();
        GridManager otherGrid = ti.grid.isMainGrid ? Data.subGrid : Data.mainGrid;
        TileInfo otherTi = otherGrid.gridData[ti.gridPosition.x, ti.gridPosition.y].GetComponent<TileInfo>();
        ti.detachUnit();
        otherTi.attachUnit(gameObject);
        transform.position = otherTi.transform.position;
    }

    GameObject[] checkRange() {
        Unit u = gameObject.GetComponent<Unit>();
        TileInfo ti = u.currentTile.GetComponent<TileInfo>();
        GridManager otherGrid = ti.grid.isMainGrid ? Data.subGrid : Data.mainGrid;
        TileInfo otherTi = otherGrid.gridData[ti.gridPosition.x, ti.gridPosition.y].GetComponent<TileInfo>();
        if (!ti.isHole || !otherTi.isHole || !otherTi.traversable || otherTi.unit != null) return null;
        GameObject[] tiles = new GameObject[1];
        tiles[0] = ti.gameObject;
        return tiles;
    }
}
