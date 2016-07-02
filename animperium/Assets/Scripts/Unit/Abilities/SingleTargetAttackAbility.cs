using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using System;

public class SingleTargetAttackAbility : MonoBehaviour{
    public AbilityInfo abilityInfo;

    public int strength;
    public DamageType type = DamageType.MELEE;
    public int minRange = 1;
    public int maxRange = 1;

    void Awake(){
        abilityInfo.checkRange = checkRange;
        abilityInfo.checkAoe = AoeChecks.dot;
        abilityInfo.execute = (Vec2i target, bool isMainGrid) => {
            AbilityManager.useAbility(gameObject, abilityInfo.abilityID, target, isMainGrid);
        };
    }

    void executeAbility(ServerMessage.UnitAbilityMessage msg){
        if (msg.abilityID != abilityInfo.abilityID) return;
        GridManager grid = msg.isTargetMainGrid ? Data.mainGrid : Data.subGrid;
        GameObject target = grid.gridData[msg.targetX, msg.targetY].GetComponent<TileInfo>().unit;
        if (target != null){
            target.GetComponent<Unit>().damage(strength, type);
        }
    }

    void getAbilityInfo(Action<AbilityInfo> enlist){
        enlist(abilityInfo);
    }

    GameObject[] checkRange(){
        Unit u = gameObject.GetComponent<Unit>();
        GameObject[] inRange = u.currentTile.GetComponent<TileInfo>().listTree(minRange, maxRange, null, (TileInfo ti) => {
            if (ti.unit == null) return false;
            int playerID = ti.unit.GetComponent<Unit>().playerID;
            return playerID != 0 && playerID != u.playerID;
        });

        return inRange.Length == 0 ? null : inRange;
    }
}
