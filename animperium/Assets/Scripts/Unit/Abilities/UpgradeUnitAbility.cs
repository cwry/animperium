using UnityEngine;
using System.Collections;
using System;


public class UpgradeUnitAbility : MonoBehaviour {
    public AbilityInfo abilityInfo;
    public string originPrefabID;
    public GameObject prefab;
    public int minRange = 2;
    public int maxRange = 2;

    void Awake(){
        abilityInfo.getRangeIndicator = getRangeIndicator;
        abilityInfo.getAffected = getAffected;
        abilityInfo.owner = gameObject;
        abilityInfo.checkRange = checkRange;
        abilityInfo.checkAoe = AoeChecks.dot;
        abilityInfo.execute = (Vec2i target, bool isMainGrid, Action callback) => {
            AbilityManager.useAbility(abilityInfo, target, isMainGrid, () => {
                GridManager grid = isMainGrid ? Data.mainGrid : Data.subGrid;
                Unit u = grid.gridData[target.x, target.y].GetComponent<TileInfo>().unit.GetComponent<Unit>();
                int mp = -1;
                MovementAbility ma = u.GetComponent<MovementAbility>();
                if (ma != null) mp = ma.movementPoints;
                SpawnManager.spawnUnit(isMainGrid ? Data.mainGrid : Data.subGrid, target, prefab.GetComponent<Unit>().prefabID, u.actionPoints, mp, u.getHPPercentage(), callback);
            });
        };
        abilityInfo.onExecution = executeAbility;
        abilityInfo.abilityID = GetComponent<Unit>().addAbility(abilityInfo);
    }

    Unit[] getAffected(ServerMessage.UnitAbilityMessage msg) {
        Unit[] affected = new Unit[1];
        GridManager grid = msg.isTargetMainGrid ? Data.mainGrid : Data.subGrid;
        TileInfo target = grid.gridData[msg.targetX, msg.targetY].GetComponent<TileInfo>();
        affected[0] = target.unit.GetComponent<Unit>();
        return affected;
    }

    void executeAbility(ServerMessage.UnitAbilityMessage msg) {
        Unit[] affected = getAffected(msg);
        Destroy(affected[0].gameObject);
    }

    GameObject[] checkRange(){
        Unit u = gameObject.GetComponent<Unit>();
        GameObject[] inRange = u.currentTile.GetComponent<TileInfo>().listTree(minRange, maxRange, null, (TileInfo ti) => {
            if (ti.unit == null) return false;
            Unit unit = ti.unit.GetComponent<Unit>();
            return unit.prefabID == originPrefabID && unit.playerID == Data.playerID;
        });
        return inRange.Length == 0 ? null : inRange;
    }

    GameObject[] getRangeIndicator() {
        return gameObject.GetComponent<Unit>().currentTile.GetComponent<TileInfo>().listTree(minRange, maxRange, null, (TileInfo ti) => {
            return ti.traversable;
        });
    }
}
