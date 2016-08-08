using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using System;

public class DefensiveBuffAbility : MonoBehaviour {
    public AbilityInfo abilityInfo;

    public float magicResist;
    public float meleeResist;
    public float rangedResist;
    public int minRange = 0;
    public int maxRange = 3;
    public bool selfCast = false;
    public AoeType aoeType = AoeType.DOT;
    public UnitType targetType = UnitType.UNDEFINED;

    void Awake() {
        if (selfCast) {
            minRange = 0;
            maxRange = 0;
        }
        abilityInfo.selfCast = selfCast;
        abilityInfo.getRangeIndicator = getRangeIndicator;
        abilityInfo.owner = gameObject;
        abilityInfo.checkRange = checkRange;
        abilityInfo.checkAoe = AoeChecks.getAoeByType(aoeType, gameObject.GetComponent<Unit>());
        abilityInfo.execute = (Vec2i target, bool isMainGrid, Action callback) => {
            AbilityManager.useAbility(abilityInfo, target, isMainGrid, callback);
        };
        abilityInfo.onExecution = executeAbility;
        abilityInfo.abilityID = GetComponent<Unit>().addAbility(abilityInfo);
    }

    void executeAbility(ServerMessage.UnitAbilityMessage msg) {
        GridManager grid = msg.isTargetMainGrid ? Data.mainGrid : Data.subGrid;
        TileInfo target = grid.gridData[msg.targetX, msg.targetY].GetComponent<TileInfo>();
        GameObject[] aoeTargets = abilityInfo.checkAoe(target);
        HashSet<Unit> targetUnits = new HashSet<Unit>();
        foreach (GameObject go in aoeTargets) {
            TileInfo tInfo = go.GetComponent<TileInfo>();
            if (tInfo.unit == null) {
                continue;
            }
            Unit unit = tInfo.unit.GetComponent<Unit>();
            if (unit.playerID == Data.playerID && (targetType == UnitType.UNDEFINED || unit.type == targetType)) targetUnits.Add(unit);
        }

        foreach (Unit unit in targetUnits){
            unit.buffResistance(meleeResist, rangedResist, magicResist);
        }
    }

    GameObject[] checkRange() {
        Unit u = gameObject.GetComponent<Unit>();
        GameObject[] inRange = u.currentTile.GetComponent<TileInfo>().listTree(minRange, maxRange, null, (TileInfo ti) => {
            GameObject[] aoe = abilityInfo.checkAoe(ti);
            foreach (GameObject go in aoe) {
                TileInfo tInfo = go.GetComponent<TileInfo>();
                if (tInfo.unit == null) {
                    continue;
                }
                Unit unit = tInfo.unit.GetComponent<Unit>();
                if (unit.playerID == Data.playerID && (targetType == UnitType.UNDEFINED || unit.type == targetType)) return true;
            }
            return false;
        });

        return inRange.Length == 0 ? null : inRange;
    }

    GameObject[] getRangeIndicator() {
        return gameObject.GetComponent<Unit>().currentTile.GetComponent<TileInfo>().listTree(minRange, maxRange);
    }
}

