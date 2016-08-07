using UnityEngine;
using System.Collections;
using System;

public class SpawnAbility : MonoBehaviour {

    public AbilityInfo abilityInfo;

    public GameObject prefab;
    public int minRange = 1;
    public int maxRange = 2;

    void Awake(){
        abilityInfo.getRangeIndicator = getRangeIndicator;
        abilityInfo.owner = gameObject;
        abilityInfo.checkRange = checkRange;
        abilityInfo.checkAoe = prefab.GetComponent<Unit>().getFootprint;
        abilityInfo.execute = (Vec2i target, bool isMainGrid, Action callback) => {
            AbilityManager.useAbility(abilityInfo, target, isMainGrid, () => {
                SpawnManager.spawnUnit(isMainGrid ? Data.mainGrid : Data.subGrid, target, prefab.GetComponent<Unit>().prefabID, callback);
            });
        };
        abilityInfo.onExecution = executeAbility;
        abilityInfo.abilityID = GetComponent<Unit>().addAbility(abilityInfo);
    }

    void executeAbility(ServerMessage.UnitAbilityMessage msg){
       //nothing to do on both clients
    }

    GameObject[] checkRange(){
        Unit u = gameObject.GetComponent<Unit>();
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

    GameObject[] getRangeIndicator(){
        TileInfo tile = gameObject.GetComponent<Unit>().currentTile.GetComponent<TileInfo>();
        if (!tile.grid.isMainGrid) return null;
        return tile.listTree(minRange, maxRange);
    }
}
