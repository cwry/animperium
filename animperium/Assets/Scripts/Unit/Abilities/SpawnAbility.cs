using UnityEngine;
using System.Collections;
using System;

public class SpawnAbility : MonoBehaviour {

    public AbilityInfo abilityInfo;

    public GameObject prefab;
    public int minRange = 1;
    public int maxRange = 2;

    void Awake(){
        abilityInfo.owner = gameObject;
        abilityInfo.checkRange = checkRange;
        abilityInfo.checkAoe = prefab.GetComponent<Unit>().getFootprint;
        abilityInfo.execute = (Vec2i target, bool isMainGrid) => {
            AbilityManager.useAbility(abilityInfo, target, isMainGrid);
        };
        abilityInfo.onExecution = executeAbility;
        abilityInfo.abilityID = GetComponent<Unit>().addAbility(abilityInfo);
    }

    void executeAbility(ServerMessage.UnitAbilityMessage msg){
        SpawnManager.spawnUnit(msg.isTargetMainGrid ? Data.mainGrid : Data.subGrid, new Vec2i(msg.targetX, msg.targetY), prefab.GetComponent<Unit>().prefabID);
    }

    GameObject[] checkRange(){
        Unit u = gameObject.GetComponent<Unit>();
        if (u.actionPoints < abilityInfo.apCost) return null;
        TileInfo tile = u.currentTile.GetComponent<TileInfo>();
        if (!tile.grid.isMainGrid) return null;
        GameObject[] inRange = tile.listTree(minRange, maxRange, null, (TileInfo ti) => {
            GameObject[] fp = abilityInfo.checkAoe(ti);
            foreach (GameObject go in fp) {
                TileInfo fpInfo = go.GetComponent<TileInfo>();
                if (!fpInfo.traversable || fpInfo.unit != null || fpInfo.isHole){
                    return false;
                }
            }
            return true;
        });

        return inRange.Length == 0 ? null : inRange;
    }
}
