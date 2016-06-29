using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using System;

public class MeleeAttackAbility : MonoBehaviour
{
    public string abilityID = "melee";
    public int strength;

    void executeAbility(ServerMessage.UnitAbilityMessage msg)
    {
        if (msg.abilityID != abilityID) return;
        GridManager grid = msg.isTargetMainGrid ? Data.mainGrid : Data.subGrid;
        GameObject target = grid.gridData[msg.targetX, msg.targetY].GetComponent<TileInfo>().unit;
        if (target != null)
        {
            target.GetComponent<Unit>().damage(strength, DamageType.MELEE);
        }
    }

    void enumerateAbility(Action<string> enlist)
    {
        enlist(abilityID);
    }

    void rangeCheckAbility(RangeCheckArgs rca)
    {
        if (rca.abilityID != abilityID) return;
        Unit u = gameObject.GetComponent<Unit>();
        GameObject[] inRange = u.currentTile.GetComponent<TileInfo>().getAdjacent();
        List<GameObject> attackable = new List<GameObject>();
        foreach (GameObject go in inRange)
        {
            TileInfo ti = go.GetComponent<TileInfo>();
            if (ti.unit == null) continue;
            int playerID = ti.unit.GetComponent<Unit>().playerID;
            if (playerID != 0 && playerID != u.playerID) attackable.Add(go);
        }

        GameObject[] res = attackable.Count == 0 ? null : attackable.ToArray();

        rca.callback(res);
    }
}
