using UnityEngine;
using System.Collections;
using System;

public class SpawnAbility : MonoBehaviour {

    public string abilityID = "build";
    public GameObject prefab;
    public int minRange = 1;
    public int maxRange = 2;

    void executeAbility(ServerMessage.UnitAbilityMessage msg){
        if (abilityID != msg.abilityID || !Data.isActivePlayer()) return;
        SpawnManager.spawnUnit(msg.isTargetMainGrid ? Data.mainGrid : Data.subGrid, new Vec2i(msg.targetX, msg.targetY), prefab.GetComponent<Unit>().prefabID);
    }

    void enumerateAbility(Action<string> enlist){
        enlist(abilityID);
    }

    void rangeCheckAbility(RangeCheckArgs rca){
        if (rca.abilityID != abilityID) return;
        Unit u = gameObject.GetComponent<Unit>();
        UnitFootprint bf = prefab.GetComponent<UnitFootprint>();

        GameObject[] inRange = u.currentTile.GetComponent<TileInfo>().listTree(minRange, maxRange, null, (TileInfo ti) => {
            GameObject[] fp = bf.getFootprint(ti);
            foreach (GameObject go in fp) {
                TileInfo fpInfo = go.GetComponent<TileInfo>();
                if(!fpInfo.traversable && fpInfo.unit != null){
                    return false;
                }
            }
            return true;
        });

        if (inRange.Length == 0) rca.callback(null);
        rca.callback(inRange);
    }
}
