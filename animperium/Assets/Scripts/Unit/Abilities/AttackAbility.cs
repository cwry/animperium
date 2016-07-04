using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using System;

public class AttackAbility : MonoBehaviour{
    public AbilityInfo abilityInfo;

    public int strength;
    public DamageType type = DamageType.MELEE;
    public int minRange = 1;
    public int maxRange = 1;
    public AoeType aoeType = AoeType.DOT;

    void Awake(){
        abilityInfo.checkRange = checkRange;
        abilityInfo.checkAoe = AoeChecks.getAoeByType(aoeType);
        abilityInfo.execute = (Vec2i target, bool isMainGrid) => {
            AbilityManager.useAbility(gameObject, abilityInfo.abilityID, target, isMainGrid);
        };
    }

    void executeAbility(ServerMessage.UnitAbilityMessage msg){
        if (msg.abilityID != abilityInfo.abilityID) return;
        GridManager grid = msg.isTargetMainGrid ? Data.mainGrid : Data.subGrid;
        TileInfo target = grid.gridData[msg.targetX, msg.targetY].GetComponent<TileInfo>();
        GameObject[] aoeTargets = abilityInfo.checkAoe(target);
        HashSet<Unit> targetUnits = new HashSet<Unit>();
        foreach(GameObject go in aoeTargets) {
            TileInfo tInfo = go.GetComponent<TileInfo>();
            if (tInfo.unit == null) {
                continue;
            }
            Unit unit = tInfo.unit.GetComponent<Unit>();
            if (unit.playerID != 0 && unit.playerID != Data.playerID) targetUnits.Add(unit);
        }

        foreach(Unit unit in targetUnits) {
            unit.damage(strength, type);
        }
    }

    void getAbilityInfo(Action<AbilityInfo> enlist){
        enlist(abilityInfo);
    }

    GameObject[] checkRange(){
        Unit u = gameObject.GetComponent<Unit>();
        GameObject[] inRange = u.currentTile.GetComponent<TileInfo>().listTree(minRange, maxRange, null, (TileInfo ti) => {
            GameObject[] aoe = abilityInfo.checkAoe(ti);
            foreach (GameObject go in aoe) {
                TileInfo tInfo = go.GetComponent<TileInfo>();
                if (tInfo.unit == null) {
                    continue;
                }
                Unit unit = tInfo.unit.GetComponent<Unit>();
                if (unit.playerID != 0 && unit.playerID != Data.playerID) return true;
            }
            return false;
        });

        return inRange.Length == 0 ? null : inRange;
    }
}
