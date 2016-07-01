﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using System;

public class SingleTargetAttackAbility : MonoBehaviour
{
    public string abilityID = "melee";
    public int strength;
    public DamageType type = DamageType.MELEE;
    public int minRange = 0;
    public int maxRange = 2;

    void executeAbility(ServerMessage.UnitAbilityMessage msg){
        if (msg.abilityID != abilityID) return;
        GridManager grid = msg.isTargetMainGrid ? Data.mainGrid : Data.subGrid;
        GameObject target = grid.gridData[msg.targetX, msg.targetY].GetComponent<TileInfo>().unit;
        if (target != null){
            target.GetComponent<Unit>().damage(strength, type);
        }
    }

    void enumerateAbility(Action<string> enlist){
        enlist(abilityID);
    }

    void rangeCheckAbility(RangeCheckArgs rca){
        if (rca.abilityID != abilityID) return;
        Unit u = gameObject.GetComponent<Unit>();
        GameObject[] inRange = u.currentTile.GetComponent<TileInfo>().listTree(minRange, maxRange, null, (TileInfo ti) => {
            if (ti.unit == null) return false;
            int playerID = ti.unit.GetComponent<Unit>().playerID;
            if (playerID != 0 && playerID != u.playerID) return true;
            return false;
        });

        if (inRange.Length == 0) rca.callback(null);
        rca.callback(inRange);
    }
}
