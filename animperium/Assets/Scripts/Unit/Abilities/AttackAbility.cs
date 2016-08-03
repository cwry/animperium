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
        abilityInfo.owner = gameObject;
        abilityInfo.checkRange = checkRange;
        abilityInfo.checkAoe = AoeChecks.getAoeByType(aoeType, gameObject.GetComponent<Unit>());
        abilityInfo.execute = (Vec2i target, bool isMainGrid) => {
            AbilityManager.useAbility(abilityInfo, target, isMainGrid);
        };
        abilityInfo.onExecution = executeAbility;
        abilityInfo.abilityID = GetComponent<Unit>().addAbility(abilityInfo);
    }

    void executeAbility(ServerMessage.UnitAbilityMessage msg){
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
            if (unit.playerID != 0 && unit.playerID != gameObject.GetComponent<Unit>().playerID) targetUnits.Add(unit);
        }
        Debug.Log(targetUnits.Count);
        foreach (Unit unit in targetUnits) {
            unit.damage(strength, type);
        }
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
