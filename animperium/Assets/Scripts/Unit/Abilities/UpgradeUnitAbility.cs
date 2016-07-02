using UnityEngine;
using System.Collections;
using System;


public class UpgradeUnitAbility : MonoBehaviour {
    public string abilityID = "upgrade";
    public string originPrefabID;
    public GameObject prefab;
    public int minRange = 2;
    public int maxRange = 2;

    void executeAbility(ServerMessage.UnitAbilityMessage msg){
        if (abilityID != msg.abilityID) return;
        GridManager grid = msg.isTargetMainGrid ? Data.mainGrid : Data.subGrid;
        GameObject unitObj = grid.gridData[msg.targetX, msg.targetY].GetComponent<TileInfo>().unit;
        Destroy(unitObj);
        if (!Data.isActivePlayer()) return;
        SpawnManager.spawnUnit(grid, new Vec2i(msg.targetX, msg.targetY), prefab.GetComponent<Unit>().prefabID);
    }

    void enumerateAbility(Action<string> enlist){
        enlist(abilityID);
    }

    void rangeCheckAbility(RangeCheckArgs rca){
        if (rca.abilityID != abilityID) return;
        Unit u = gameObject.GetComponent<Unit>();
        GameObject[] inRange = u.currentTile.GetComponent<TileInfo>().listTree(minRange, maxRange, null, (TileInfo ti) => {
            if (ti.unit == null) return false;
            Unit unit = ti.unit.GetComponent<Unit>();
            return unit.prefabID == originPrefabID && unit.playerID == Data.playerID;
        });

        if (inRange.Length == 0) rca.callback(null);
        rca.callback(inRange);
    }
}
