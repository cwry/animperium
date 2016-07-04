using UnityEngine;
using System.Collections;
using System;


public class UpgradeUnitAbility : MonoBehaviour {
    public AbilityInfo abilityInfo;
    public string originPrefabID;
    public GameObject prefab;
    public int minRange = 2;
    public int maxRange = 2;
    public int apCost = 12;

    void Awake(){
        abilityInfo.checkRange = checkRange;
        abilityInfo.checkAoe = AoeChecks.dot;
        abilityInfo.execute = (Vec2i target, bool isMainGrid) => {
            gameObject.GetComponent<Unit>().actionPoints -= apCost;
            AbilityManager.useAbility(gameObject, abilityInfo.abilityID, target, isMainGrid);
        };
    }

    void executeAbility(ServerMessage.UnitAbilityMessage msg){
        if (abilityInfo.abilityID != msg.abilityID) return;
        GridManager grid = msg.isTargetMainGrid ? Data.mainGrid : Data.subGrid;
        GameObject unitObj = grid.gridData[msg.targetX, msg.targetY].GetComponent<TileInfo>().unit;
        Destroy(unitObj);
        if (!Data.isActivePlayer()) return;
        SpawnManager.spawnUnit(grid, new Vec2i(msg.targetX, msg.targetY), prefab.GetComponent<Unit>().prefabID);
    }

    void getAbilityInfo(Action<AbilityInfo> enlist){
        enlist(abilityInfo);
    }

    GameObject[] checkRange(){
        Unit u = gameObject.GetComponent<Unit>();
        if (u.actionPoints < apCost) return null; 
        GameObject[] inRange = u.currentTile.GetComponent<TileInfo>().listTree(minRange, maxRange, null, (TileInfo ti) => {
            if (ti.unit == null) return false;
            Unit unit = ti.unit.GetComponent<Unit>();
            return unit.prefabID == originPrefabID && unit.playerID == Data.playerID;
        });
        return inRange.Length == 0 ? null : inRange;
    }
}
