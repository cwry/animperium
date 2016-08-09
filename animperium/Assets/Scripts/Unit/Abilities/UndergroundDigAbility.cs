using UnityEngine;
using System.Collections;
using System;

public class UndergroundDigAbility : MonoBehaviour {
    public AbilityInfo abilityInfo;
    public int minRange = 1;
    public int maxRange = 1;

    void Awake() {
        abilityInfo.getRangeIndicator = getRangeIndicator;
        abilityInfo.owner = gameObject;
        abilityInfo.checkRange = checkRange;
        abilityInfo.checkAoe = AoeChecks.dot;
        abilityInfo.execute = (Vec2i target, bool isMainGrid, Action callback) => {
            AbilityManager.useAbility(abilityInfo, target, isMainGrid, callback);
        };
        abilityInfo.onExecution = executeAbility;
        Unit u = GetComponent<Unit>();
        abilityInfo.abilityID = u.addAbility(abilityInfo);
    }

    void executeAbility(ServerMessage.UnitAbilityMessage msg) {
        GridManager grid = msg.isTargetMainGrid ? Data.mainGrid : Data.subGrid;
        UndergroundTile target = grid.gridData[msg.targetX, msg.targetY].GetComponent<UndergroundTile>();
        target.state = UndergroundTileState.REVEALED;
        target.updateSightRange();
    }

    GameObject[] checkRange() {
        Unit u = gameObject.GetComponent<Unit>();
        TileInfo tile = u.currentTile.GetComponent<TileInfo>();
        if (tile.grid.isMainGrid) return null;
        GameObject[] inRange = tile.listTree(minRange, maxRange, (TileInfo ti) => {
            UndergroundTile ut = ti.GetComponent<UndergroundTile>();
            return ut != null && ut.isInSight() && ut.state == UndergroundTileState.REVEALED; 
        }, 
        (TileInfo ti) => {
            UndergroundTile ut = ti.GetComponent<UndergroundTile>();
            return ut != null && ut.isInSight() && ut.state == UndergroundTileState.BLOCKED;
        });

        return inRange.Length == 0 ? null : inRange;
    }

    GameObject[] getRangeIndicator() {
        Unit u = gameObject.GetComponent<Unit>();
        TileInfo tile = u.currentTile.GetComponent<TileInfo>();
        if (tile.grid.isMainGrid) return null;
        GameObject[] inRange = tile.listTree(minRange, maxRange, null, (TileInfo ti) => {
            return ti.traversable; 
        });
        return inRange.Length == 0 ? null : inRange;
    }
}
