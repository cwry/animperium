using UnityEngine;
using System.Collections;
using System;

public class SpawnAbility : MonoBehaviour {

    public AbilityInfo abilityInfo;

    public GameObject prefab;
    public int minRange = 1;
    public int maxRange = 2;
    public int apCost = 12;

    void Awake(){
        abilityInfo.checkRange = checkRange;
        abilityInfo.checkAoe = prefab.GetComponent<Unit>().getFootprint;
        abilityInfo.execute = (Vec2i target, bool isMainGrid) => {
            gameObject.GetComponent<Unit>().actionPoints -= apCost;
            AbilityManager.useAbility(gameObject, abilityInfo.abilityID, target, isMainGrid);
        };
    }

    void executeAbility(ServerMessage.UnitAbilityMessage msg){
        if (abilityInfo.abilityID != msg.abilityID || !Data.isActivePlayer()) return;
        SpawnManager.spawnUnit(msg.isTargetMainGrid ? Data.mainGrid : Data.subGrid, new Vec2i(msg.targetX, msg.targetY), prefab.GetComponent<Unit>().prefabID);
    }

    void getAbilityInfo(Action<AbilityInfo> enlist){
        enlist(abilityInfo);
    }

    GameObject[] checkRange(){
        Unit u = gameObject.GetComponent<Unit>();
        if (u.actionPoints < apCost) return null;
        GameObject[] inRange = u.currentTile.GetComponent<TileInfo>().listTree(minRange, maxRange, null, (TileInfo ti) => {
            GameObject[] fp = abilityInfo.checkAoe(ti);
            foreach (GameObject go in fp) {
                TileInfo fpInfo = go.GetComponent<TileInfo>();
                if (!fpInfo.traversable || fpInfo.unit != null){
                    return false;
                }
            }
            return true;
        });

        return inRange.Length == 0 ? null : inRange;
    }
}
