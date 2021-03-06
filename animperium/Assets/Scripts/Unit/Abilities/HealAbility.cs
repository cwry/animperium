﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using System;
using System.Linq;

public class HealAbility : MonoBehaviour {
    public AbilityInfo abilityInfo;

    public float amt;
    public int minRange = 0;
    public int maxRange = 3;
    public float maxMotivationMultiplier = 1f;
    public bool selfCast = false;
    public AoeType aoeType = AoeType.DOT;
    public UnitType targetType = UnitType.UNDEFINED;

    void Awake() {
        if (selfCast) {
            minRange = 0;
            maxRange = 0;
        }
        abilityInfo.selfCast = selfCast;
        abilityInfo.getAffected = getAffected;
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

    Unit[] getAffected(ServerMessage.UnitAbilityMessage msg) {
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
        return targetUnits.ToArray();
    }

    void executeAbility(ServerMessage.UnitAbilityMessage msg) {
        Unit[] affected = getAffected(msg);

        foreach (Unit unit in affected) {
            unit.hitPoints += amt;
            if (unit.hitPoints > unit.maxHitPoints) {
                float overheal = unit.hitPoints - unit.maxHitPoints;
                float motivation = overheal / amt * (maxMotivationMultiplier - 1) + 1;
                if (unit.attackMultiplier < motivation) unit.attackMultiplier = motivation; 
                unit.hitPoints = unit.maxHitPoints;
            }

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
                if(unit.playerID == Data.playerID && (targetType == UnitType.UNDEFINED || unit.type == targetType)) return true;
            }
            return false;
        });

        return inRange.Length == 0 ? null : inRange;
    }

    GameObject[] getRangeIndicator() {
        return gameObject.GetComponent<Unit>().currentTile.GetComponent<TileInfo>().listTree(minRange, maxRange, null, (TileInfo ti) => {
            return ti.traversable;
        });
    }
}

